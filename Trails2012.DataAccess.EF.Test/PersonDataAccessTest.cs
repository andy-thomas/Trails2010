using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trails2012.Domain;

namespace Trails2012.DataAccess.EF.Test
{
    [TestClass]
    public class PersonDataAccessTest
    {

        [TestMethod]
        public void ShouldGetAllPersons()
        {
            using (TrailsContext context = new TrailsContext())
            {
                DbSet<Person> persons = context.Set<Person>();  // or use context.Persons;
                persons.Load();
                Assert.IsTrue(persons.Local.Count > 0);
                Console.WriteLine("There are {0} persons", persons.Count());
            }
        }

        [TestMethod]
        public void ShouldGetPersonById()
        {
            const int id = 1;
            using (TrailsContext context = new TrailsContext())
            {
                DbSet<Person> persons = context.Persons;
                if (persons != null)
                {
                    Person person = persons.Find(id);
                    Assert.AreEqual(person.Id, id);
                    Console.WriteLine("Person found: id = {0}, Full Name = {1}",
                        person.Id, person.FullName);
                }
            }
        }

        [TestMethod]
        public void ShouldGetPersonsByLastName()
        {
            using (TrailsContext context = new TrailsContext())
            {
                DbSet<Person> persons = context.Persons;
                var personQuery = from p in persons
                                    where p.LastName == "Smith"
                                    select p;
                List<Person> personList = personQuery.ToList();
                Assert.IsTrue(personList.Count > 0);
                Console.WriteLine("There are {0} people with the specified name", personList.Count());
            }
        }

        [TestMethod]
        public void ShouldAddNewPerson()
        {
            using (new TransactionScope())
            {
                using (TrailsContext context = new TrailsContext())
                {
                    DbSet<Person> persons = context.Persons;
                    persons.Load();
                    int priorCount = persons.Local.Count;

                    Person newPerson = new Person
                    {
                        FirstName = "testFirstName",
                        LastName = "testLastName",
                        DateOfBirth = DateTime.Today.AddYears(-32),
                        Gender = 'M'
                    };

                    // Add the new person to the context
                    Person person = persons.Add(newPerson);

                    // Save the context to the database, causing the database to be updated
                    context.SaveChanges();
                    // At this stage, the database has the new row, so the identity has been incremented and assigned 
                    Assert.IsTrue(person.Id > 0);
                    Assert.IsTrue(newPerson.Id > 0);
 
                    persons.Load();
                    int postCount = persons.Local.Count;

                    Assert.AreEqual(postCount, priorCount + 1);
                }
                // Rollback the transaction, to make the test repeatable
            }
        }


    }
}
