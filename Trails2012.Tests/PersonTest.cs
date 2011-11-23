using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trails2012.DataAccess;
using Trails2012.Domain;

namespace Trails2012.Tests
{
    [TestClass]
    public class PersonTest
    {
        private const int Id = 1;
        private CompositionContainer _container;
        private RepositoryFactory _repositoryFactory;

        [TestInitialize]
        public void Setup()
        {
            // Options for setting up dependency injection using MEF:
            // e.g. see http://msdn.microsoft.com/en-us/magazine/ee291628.aspx
            //      and http://buksbaum.us/2011/08/22/gentle-introduction-to-mefpart-two/

            // 1. Use the TypeCatalog and explicitly reference the types - this requires a reference to the injected type, 
            // which kind of defeats the purpose of having it as a pluggable component
            //TypeCatalog repositoryCatalog = new TypeCatalog(typeof(IRepository), typeof(EFRepository)),typeof(NHibernateRepository))

            // 2. Use the Directory Catalog and point to the folder in which our executable is.
            //      (which in this case is likely to be C:\Projects\Personal\Trails2012\Trails2012.Tests\bin\Debug)
            // NOTE: we do not (indeed, we should not) have an explicit reference to the assembly containing our plugabble repository 
            // NOTE:         (i.e. the one that uses Entity Framework - "Trails2012.DataAccess.EF")
            // NOTE: so a version of the assembly file is _not_ copied into the executing folder automatically whenever this ("Trails2102.Test") assembly is built. 
            // NOTE: We need to explicitly place a _current_copy of the Trails2012.DataAccess.EF.dll file into our executing folder.
            DirectoryCatalog repositoryCatalog = new DirectoryCatalog(@".\");

            // 3. Use the Directory Catalog and point to the folder which contains our pluggable component
            //      (which in this case is likely to be C:\Projects\Personal\Trails2012\Trails2012.DataAccess.EF\bin\Debug)
            //Assembly executingAssembly = Assembly.GetExecutingAssembly();
            //bool found = executingAssembly.GetCustomAttributes(typeof(DebuggableAttribute), false).Length > 0;
            //string configString = (found ? "debug" : "release");
            //DirectoryCatalog repositoryCatalog = new DirectoryCatalog(@"..\..\..\Trails2012.DataAccess.EF\bin\" + configString);

            // 4. Use an AssemblyCatalog??? Not sure this works - it would need the pluggable component to be part of the assembly
            //AssemblyCatalog repositoryCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            // Of course, we can use all/any of the above and combine the using an AggregateCatalog (http://mef.codeplex.com/wikipage?title=Using%20Catalogs)
            // e.g. var catalog = new AggregateCatalog(
            //                  new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()), 
            //                  new DirectoryCatalog(@".\Extensions"));  


            // 5. Skip dependency injection and inject the repository explicitly (just for this Test assembly).
            // However, to do that we need to get a reference to the pluggable repository, in which case, we would need a reference in this test assembly
            // Use an internal constructor on the RepositoryFactory class and set the InternalsVisibleTo attribute of the DataAccess assembly to include this Test assembly
            //EFRepository repository = new EFRepository();
            //_repositoryFactory = new RepositoryFactory(repository);
            // or
            //_repositoryFactory = new RepositoryFactory();
            // _repositoryFactory.Repository = repository;

            _repositoryFactory = new RepositoryFactory();

            // Now, set up the MEF container and use it to hydrate the _repositoryFactory object
            _container = new CompositionContainer(repositoryCatalog);
            _container.SatisfyImportsOnce(_repositoryFactory);

        }

        [TestMethod]
        public void ShouldPlugInARepository()
        {
            Assert.IsNotNull(_repositoryFactory.Repository);
            Console.WriteLine("We are using the following repository: {0}", _repositoryFactory.Repository.GetType());
        }

        [TestMethod]
        public void ShouldGetAllPersons()
        {
            IEnumerable<Person> persons = _repositoryFactory.Repository.List<Person>();
            Assert.IsNotNull(persons);
            int count = persons.Count();
            Assert.IsTrue(count > 0);
            Console.WriteLine("There are {0} persons", count);
        }

        [TestMethod]
        public void ShouldGetPersonById()
        {
            Person person = _repositoryFactory.Repository.GetById<Person>(Id);
            Assert.IsNotNull(person);
            Assert.AreEqual(person.Id, Id);
            Console.WriteLine("Person found: id = {0}, Full Name = {1}",
                              person.Id, person.FullName);
        }

        [TestMethod]
        public void ShouldGetPersonsByLastName()
        {
            // Get all persons whose name contains "on" and sort by first name descending
            IQueryable<Person> personQuery = _repositoryFactory.Repository.Get<Person>(
                p => p.LastName.Contains("on"),
                q => q.OrderByDescending(p => p.FirstName));
            List<Person> personList = personQuery.ToList();
            Assert.IsTrue(personList.Count > 0);
            Console.WriteLine("There are {0} people with the specified name", personList.Count());
        }

        [TestMethod]
        public void ShouldAddNewPerson()
        {
            IEnumerable<Person> persons = _repositoryFactory.Repository.List<Person>();
            int priorCount = persons.Count();

            Person newPerson = new Person
                                   {
                                       FirstName = "testFirstName",
                                       LastName = "testLastName",
                                       DateOfBirth = DateTime.Today.AddYears(-32),
                                       Gender = 'F'
                                   };

            // Begin transaction
            _repositoryFactory.Repository.BeginTransaction();

            // Add the new person to the context
            Person person = _repositoryFactory.Repository.Insert(newPerson);

            // Save the context to the database, causing the database to be updated
            _repositoryFactory.Repository.SaveChanges();

            // At this stage, the database has the new row, so the identity has been incremented and assigned 
            Assert.IsTrue(person.Id > 0);
            Assert.IsTrue(newPerson.Id > 0);

            persons = _repositoryFactory.Repository.List<Person>();
            int postCount = persons.Count();

            Assert.AreEqual(postCount, priorCount + 1);
            // Rollback the transaction, to make the test repeatable
            _repositoryFactory.Repository.RollbackTransaction();
        }

    }
}

    
