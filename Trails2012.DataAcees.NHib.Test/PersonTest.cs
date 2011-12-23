using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib.Test
{
    [TestClass]
    public class PersonTest
    {
        private static IRepository _repository;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            _repository = new NHibRepository(true);
            Util.SeedData(_repository);
        }

        [TestMethod]
        public void ShouldGetAllPersons()
        {
            IEnumerable<Person> persons = _repository.List<Person>();
            Assert.IsTrue(persons != null && persons.Count() > 0);
            Console.WriteLine("There are {0} persons", persons.Count());
        }

        [TestMethod]
        public void ShouldGetPersonById()
        {
            const int id = 1;
            {
                Person person = _repository.GetById<Person>(id);
                if (person != null)
                {
                    Assert.AreEqual(person.Id, id);
                    Console.WriteLine(
                        "Person found: id = {0}, First Name = {1}, Last Name = {2}, DOB = {3}, Gender = {4}, FullName = {5}",
                        person.Id, person.FirstName, person.LastName, person.DateOfBirth, person.GenderDesc,
                        person.FullName);
                }
            }
        }

        [TestMethod]
        public void ShouldGetPersonsByLastName()
        {
            IEnumerable<Person> persons = _repository.List<Person>();
            var personQuery = from p in persons
                              where p.LastName == "Smith"
                              select p;
            List<Person> personList = personQuery.ToList();
            Assert.IsTrue(personList.Count > 0);
            Console.WriteLine("There are {0} people with the specified name", personList.Count());
        }

        [TestMethod]
        public void ShouldGetPersonByNameUsingLinq2NHib()
        {
            const string lastName = "on";
            IQueryable<Person> query = _repository.Get<Person>(
                l => l.LastName.Contains(lastName),
                q => q.OrderBy(l => l.FirstName),
                null);
            List<Person> personList = query.ToList();
            Assert.IsTrue(personList.Count > 0);
            Console.WriteLine("There are {0} people with the specified name", personList.Count());
        }

        [TestMethod]
        public void ShouldAddNewPerson()
        {
            _repository.BeginTransaction();

            IEnumerable<Person> persons = _repository.List<Person>();
            int priorCount = persons.Count();

            Person newPerson = new Person
                                   {
                                       FirstName = "testFirstName",
                                       LastName = "testLastName",
                                       DateOfBirth = DateTime.Today.AddYears(-32),
                                       Gender = "M"
                                   };

            // Add the new person to the context
            Person person = _repository.Insert(newPerson);

            // Save the context to the database, causing the database to be updated
            _repository.SaveChanges();
            // At this stage, the database has the new row, so the identity has been incremented and assigned 
            Assert.IsTrue(person.Id > 0);
            Assert.IsTrue(newPerson.Id > 0);

            persons = _repository.List<Person>();
            int postCount = persons.Count();

            Assert.AreEqual(postCount, priorCount + 1);

            // Rollback the transaction, to make the test repeatable
            _repository.RollbackTransaction();
        }
    }


}


