using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trails2012.DataAccess;
using Trails2012.DataAccess.NHib;
using Trails2012.Domain;

namespace Trails2012.DataAcees.NHib.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PersonTest
    {
        public PersonTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion

        [TestMethod]
        public void ShouldGetAllPersons()
        {
            using (IRepository repository = new NHibRepository())
            {
                IEnumerable<Person> persons = repository.List<Person>();
                Assert.IsTrue(persons != null && persons.Count() > 0);
                Console.WriteLine("There are {0} persons", persons.Count());
            }
        }

        [TestMethod]
        public void ShouldGetPersonById()
        {
            const int id = 1;
            using (IRepository repository = new NHibRepository())
            {
                Person person = repository.GetById<Person>(id);
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
            using (IRepository repository = new NHibRepository())
            {
                IEnumerable<Person> persons = repository.List<Person>();
                var personQuery = from p in persons
                                  where p.LastName == "Smith"
                                  select p;
                List<Person> personList = personQuery.ToList();
                Assert.IsTrue(personList.Count > 0);
                Console.WriteLine("There are {0} people with the specified name", personList.Count());
            }
        }

        [TestMethod]
        public void ShouldGetPersonByNameUsingLinq2NHib()
        {
            const string lastName = "on";
            using (IRepository repository = new NHibRepository())
            {
                IQueryable<Person> query = repository.Get<Person>(
                    l => l.LastName.Contains(lastName),
                    q => q.OrderBy(l => l.FirstName),   
                    null);
                List<Person> personList = query.ToList();
                Assert.IsTrue(personList.Count > 0);
                Console.WriteLine("There are {0} people with the specified name", personList.Count());
            }
        }

        [TestMethod]
        public void ShouldAddNewPerson()
        {
            using (IRepository repository = new NHibRepository())
            {
                repository.BeginTransaction();

                IEnumerable<Person> persons = repository.List<Person>();
                int priorCount = persons.Count();

                Person newPerson = new Person
                                       {
                                           FirstName = "testFirstName",
                                           LastName = "testLastName",
                                           DateOfBirth = DateTime.Today.AddYears(-32),
                                           Gender = "M"
                                       };

                // Add the new person to the context
                Person person = repository.Insert(newPerson);

                // Save the context to the database, causing the database to be updated
                repository.SaveChanges();
                // At this stage, the database has the new row, so the identity has been incremented and assigned 
                Assert.IsTrue(person.Id > 0);
                Assert.IsTrue(newPerson.Id > 0);

                persons = repository.List<Person>();
                int postCount = persons.Count();

                Assert.AreEqual(postCount, priorCount + 1);

                // Rollback the transaction, to make the test repeatable
                repository.RollbackTransaction();
            }
        }

    }
}

