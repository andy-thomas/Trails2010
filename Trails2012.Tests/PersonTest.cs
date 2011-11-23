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
        private int _id = 1;
        private CompositionContainer _container;
        private RepositoryFactory _repositoryFactory;

        [TestInitialize]
        public void Setup()
        {
            // Options for setting up dependency injection:

            // 1. Use the TypeCatalog and explicitly reference the types - this requires a reference to the injected type, 
            // which kind of defeats the purpose of having it as a pluggable component
            //TypeCatalog repositoryCatalog = new TypeCatalog(typeof(IRepository), typeof(EFRepository)),typeof(NHibernateRepository))

            // 2. Use the Directory Catalog and point to the folder in which our executable is.
            //      (which in this case is likely to be C:\Projects\Personal\Trails2012\Trails2012.Tests\bin\Debug)
            // NOTE: we do not have an explicit reference to our plugabble repository
            // NOTE: so a version is not copied in automatically when built. 
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

            _container = new CompositionContainer(repositoryCatalog);

            _repositoryFactory = new RepositoryFactory();
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
            Person person = _repositoryFactory.Repository.GetById<Person>(_id);
            Assert.IsNotNull(person);
            Assert.AreEqual(person.Id, _id);
            Console.WriteLine("Person found: id = {0}, Full Name = {1}",
                              person.Id, person.FullName);
        }


    }
}

    
