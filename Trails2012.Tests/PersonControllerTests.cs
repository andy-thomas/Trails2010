using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Trails2012.Controllers;
using Trails2012.DataAccess;
using Trails2012.Domain;

namespace Trails2012.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PersonControllerTests
    {
        private CompositionContainer _container;
        private RepositoryFactory _repositoryFactory;


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


        //[TestInitialize]
        public void SetupRepositoryUsingMEF()
        {
            // It would be nice to specify a Moq wrapper of an IRepository 
            // and register it with MEF
            // so that it gets injected into the RepositoryFactory
            TypeCatalog repositoryCatalog = new TypeCatalog(typeof(IRepository), typeof(Mock<IRepository>));
            _repositoryFactory = new RepositoryFactory();
            _container = new CompositionContainer(repositoryCatalog);
            _container.SatisfyImportsOnce(_repositoryFactory);

            // However this does not work.
            // It causes the following MEF error: 1) No valid exports were found that match the constraint '((exportDefinition.ContractName == "Trails2012.DataAccess.IRepository") AndAlso (exportDefinition.Metadata.ContainsKey("ExportTypeIdentity") AndAlso "Trails2012.DataAccess.IRepository".Equals(exportDefinition.Metadata.get_Item("ExportTypeIdentity"))))', invalid exports may have been rejected.
            //      because the Moq wrapper is not decorated with the MEF attribute:
            // [Export(typeof(IRepository))]
            // Hmm...I do not like that MEF requires you to decorate classes with attributes - 
            //      other IOC containers do not do that.

            // Still, no biggie. RepositoryFactory is a test-specific artifact anyway.
            // Instead, we will not use MEF, but inject the IRepository manually.
            // I have set the controllers up with constructors which MEF uses, 
            //      and which take IRepository as parameter, so we can just use those constructors.

        }

        [TestMethod]
        public void ShouldDisplayIndex()
        {
            // Arrange
            List<Person> persons = new List<Person>
                                       {
                                           new Person {Id = 1, FirstName = "TestFirstName1", LastName = "TestLastName1"},
                                           new Person {Id = 2, FirstName = "TestFirstName2", LastName = "TestLastName2"},
                                           new Person {Id = 3, FirstName = "TestFirstName3", LastName = "TestLastName3"}
                                       };

            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(r => r.List<Person>()).Returns(persons);

            PersonController controller = new PersonController(repositoryMock.Object);

            // Act
            ViewResult result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
            
            // check the view data
            Assert.IsNotNull(result.ViewData["GenderList"]);
            Assert.IsTrue(result.ViewData["GenderList"].GetType() == typeof(Dictionary<string, string>));
            Assert.IsTrue(((Dictionary<string, string>)result.ViewData["GenderList"]).ContainsKey("M"));

            // check the Model
            Assert.IsNotNull(result.Model);            
            Assert.AreEqual(persons.Count, ((List<Person>)result.Model).Count);
            Assert.AreEqual(persons[1], ((List<Person>)result.Model)[1]);
        }

        [TestMethod]
        public void ShouldDisplayDetails()
        {
            // Arrange
            const int id = 1;
            Person person = new Person {Id = 1, FirstName = "TestFirstName1", LastName = "TestLastName1"};

            var repositoryMock = new Mock<IRepository>();
            repositoryMock.Setup(r => r.GetById<Person>(id)).Returns(person).Verifiable("GetById was not called with the correct Id");
            // Note: using Verifiable() is not considered best practice - see http://code.google.com/p/moq/issues/detail?id=220
            //      use .Verify() instead
 
            PersonController controller = new PersonController(repositoryMock.Object);

            // Act
            ViewResult result = controller.Details(id);

            // Assert
            repositoryMock.Verify();
            repositoryMock.Verify(r => r.GetById<Person>(id), Times.Exactly(1));

            Assert.IsNotNull(result);

            // check the view data
            Assert.IsNotNull(result.ViewData["GenderList"]);
            Assert.IsTrue(result.ViewData["GenderList"].GetType() == typeof(Dictionary<string, string>));
            Assert.IsTrue(((Dictionary<string, string>)result.ViewData["GenderList"]).ContainsKey("M"));

            // check the Model
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(person, result.Model);
        }

        [TestMethod]
        public void ShouldDisplayCreate()
        {
            // Arrange
            var repositoryMock = new Mock<IRepository>();
            Person person = new Person();
            PersonController controller = new PersonController(repositoryMock.Object);

            // Act
            ActionResult result = controller.Create();

            // Assert
           Assert.IsNotNull(result);

            // check the Model
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(Person), ((ViewResultBase)(result)).Model.GetType());
            Assert.AreEqual(person.Id, ((Person)((ViewResultBase)(result)).Model).Id);
        }

        [TestMethod]
        public void ShouldCreateOnPostback()
        {
            // Arrange
            Person person = new Person { FirstName = "TestFirstName1", LastName = "TestLastName1" };

            var repositoryMock = new Mock<IRepository>();            

            PersonController controller = new PersonController(repositoryMock.Object);
            controller.SetFakeControllerContext(); // Use the extension method in our MvcMoqHelper class

            // Act
            ActionResult result = controller.Create(person);

            // Assert
            repositoryMock.Verify(r => r.Insert(person), Times.Once());
            repositoryMock.Verify(r => r.SaveChanges(), Times.AtLeastOnce());

            // check that the controller returns a redirect to action where the action method is "Index"
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
            Assert.AreEqual("Index", (((RedirectToRouteResult)result)).RouteValues["action"]);
        }

        [TestMethod]
        public void ShouldCreateOnAjaxPostback()
        {
            // Note: to mock the controller's Request property to represent an Ajax request, see
            // http://stackoverflow.com/questions/970198/how-to-mock-the-request-on-controller-in-asp-net-mvc

            // Arrange
            Person person = new Person { FirstName = "TestFirstName1", LastName = "TestLastName1" };

            var repositoryMock = new Mock<IRepository>();
            var httpRequestMock = new Mock<HttpRequestBase>();
            var contextMock = new Mock<HttpContextBase>();

            httpRequestMock.SetupGet(r => r.Headers).Returns(new System.Net.WebHeaderCollection
                                                                 {
                                                                     {"X-Requested-With", "XMLHttpRequest"}
                                                                 }
                );
            contextMock.SetupGet(x => x.Request).Returns(httpRequestMock.Object);

            List<Person> persons = new List<Person>
                                       {
                                           new Person {Id = 1, FirstName = "TestFirstName1", LastName = "TestLastName1"},
                                           new Person {Id = 2, FirstName = "TestFirstName2", LastName = "TestLastName2"},
                                           new Person {Id = 3, FirstName = "TestFirstName3", LastName = "TestLastName3"}
                                       };
            repositoryMock.Setup(r => r.List<Person>()).Returns(persons);

            PersonController controller = new PersonController(repositoryMock.Object);
            controller.ControllerContext = new ControllerContext(contextMock.Object, new RouteData(), controller);

            // Act
            ActionResult result = controller.Create(person);

            // Assert
            repositoryMock.Verify(r => r.Insert(person), Times.Once());
            repositoryMock.Verify(r => r.SaveChanges(), Times.AtLeastOnce());

            Assert.IsNotNull(result);

            // check that the control returns a "_List" partial view with the results of the repository List<Person> method
            Assert.AreEqual(typeof(PartialViewResult), result.GetType());
            Assert.AreEqual("_List", (((PartialViewResult)result)).ViewName);
            //Assert.AreEqual(persons, (((PartialViewResult)result)).Model);
            Assert.AreEqual(persons.Count, ((List<Person>)(((PartialViewResult)result)).Model).Count);
            Assert.AreEqual(persons[1], ((List<Person>)(((PartialViewResult)result)).Model)[1]);
        }

        [TestMethod]
        public void ShouldDisplayEdit()
        {
            // Arrange
            const int id = 1;
            Person person = new Person { Id = 1, FirstName = "TestFirstName1", LastName = "TestLastName1" };

            var repositoryMock = new Mock<IRepository>();
            PersonController controller = new PersonController(repositoryMock.Object);

            repositoryMock.Setup(r => r.GetById<Person>(id)).Returns(person);

            // Act
            ActionResult result = controller.Edit(id);

            // Assert
            repositoryMock.Verify(r => r.GetById<Person>(id), Times.Exactly(1)); 
            
            Assert.IsNotNull(result);

            // check the Model
            Assert.IsNotNull(result);

            // check that the control returns a "_Edit" partial view with the results of the repository GetById<Person> method
            Assert.AreEqual(typeof(PartialViewResult), result.GetType());
            Assert.AreEqual("_Edit", (((PartialViewResult)result)).ViewName);
            Assert.AreEqual(typeof(Person), ((PartialViewResult)(result)).Model.GetType());
            Assert.AreEqual(person.Id, ((Person)((PartialViewResult)(result)).Model).Id);
        }

        [TestMethod]
        public void ShouldEditOnPostback()
        {
            // Arrange
            Person person = new Person { FirstName = "TestFirstName1", LastName = "TestLastName1" };

            var repositoryMock = new Mock<IRepository>();

            PersonController controller = new PersonController(repositoryMock.Object);
            controller.SetFakeControllerContext(); // Use the extension method in our MvcMoqHelper class

            // Act
            ActionResult result = controller.Edit(person);

            // Assert
            repositoryMock.Verify(r => r.Update(person), Times.Once());
            repositoryMock.Verify(r => r.SaveChanges(), Times.AtLeastOnce());
            repositoryMock.Verify(r => r.List<Person>(), Times.AtLeastOnce());

            // check that the controller returns a redirect to action where the action method is "Index"
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
            Assert.AreEqual("Index", (((RedirectToRouteResult)result)).RouteValues["action"]);
        }

        [TestMethod]
        public void ShouldRedirectEditToIndexIfInvallidModelOnPostback()
        {
            // Arrange
            Person person = new Person { FirstName = "TestFirstName1", LastName = "TestLastName1" };

            var repositoryMock = new Mock<IRepository>();

            PersonController controller = new PersonController(repositoryMock.Object);
            controller.SetFakeControllerContext(); // Use the extension method in our MvcMoqHelper class

            // Add a validation broken rule so that the ModelState becomes invalid
            controller.ModelState.AddModelError("testkey", @"test error message");

            // Act
            ActionResult result = controller.Edit(person);

            // Assert
            repositoryMock.Verify(r => r.Update(person), Times.Never());
            repositoryMock.Verify(r => r.List<Person>(), Times.Never());

            // check that the controller returns a redirect to action where the action method is "Index"
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
            Assert.AreEqual("Index", (((RedirectToRouteResult)result)).RouteValues["action"]);
        }

        [TestMethod]
        public void ShouldDisplayEditIfInvallidModelOnAjaxPostback()
        {
            // Arrange
            Person person = new Person { FirstName = "TestFirstName1", LastName = "TestLastName1" };

            var repositoryMock = new Mock<IRepository>();
            var httpRequestMock = new Mock<HttpRequestBase>();
            var contextMock = new Mock<HttpContextBase>();

            httpRequestMock.SetupGet(r => r.Headers).Returns(new System.Net.WebHeaderCollection
                                                                 {
                                                                     {"X-Requested-With", "XMLHttpRequest"}
                                                                 }
                );
            contextMock.SetupGet(x => x.Request).Returns(httpRequestMock.Object);

            PersonController controller = new PersonController(repositoryMock.Object);
            controller.ControllerContext = new ControllerContext(contextMock.Object, new RouteData(), controller);

            // Add a validation broken rule so that the ModelState becomes invalid
            controller.ModelState.AddModelError("testkey", @"test error message");

            // Act
            ActionResult result = controller.Edit(person);

            // Assert
            repositoryMock.Verify(r => r.Update(person), Times.Never());
            repositoryMock.Verify(r => r.List<Person>(), Times.Never());

            // check that the controller returns a redirect to action where the action method is "Index"
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(Person), ((ViewResultBase)(result)).Model.GetType());
            Assert.AreEqual(person.Id, ((Person)((ViewResultBase)(result)).Model).Id);

            Assert.IsNull(((ViewResultBase)result).ViewData["GenderList"]);
        }



        [TestMethod]
        public void ShouldEditOnAjaxPostback()
        {
            // Note: to mock the controller's Request property to represent an Ajax request, see
            // http://stackoverflow.com/questions/970198/how-to-mock-the-request-on-controller-in-asp-net-mvc

            // Arrange
            Person person = new Person { FirstName = "TestFirstName1", LastName = "TestLastName1" };

            var repositoryMock = new Mock<IRepository>();
            var httpRequestMock = new Mock<HttpRequestBase>();
            var contextMock = new Mock<HttpContextBase>();

            httpRequestMock.SetupGet(r => r.Headers).Returns(new System.Net.WebHeaderCollection
                                                                 {
                                                                     {"X-Requested-With", "XMLHttpRequest"}
                                                                 }
                );
            contextMock.SetupGet(x => x.Request).Returns(httpRequestMock.Object);

            List<Person> persons = new List<Person>
                                       {
                                           new Person {Id = 1, FirstName = "TestFirstName1", LastName = "TestLastName1"},
                                           new Person {Id = 2, FirstName = "TestFirstName2", LastName = "TestLastName2"},
                                           new Person {Id = 3, FirstName = "TestFirstName3", LastName = "TestLastName3"}
                                       };
            repositoryMock.Setup(r => r.List<Person>()).Returns(persons);

            PersonController controller = new PersonController(repositoryMock.Object);
            controller.ControllerContext = new ControllerContext(contextMock.Object, new RouteData(), controller);

            // Act
            ActionResult result = controller.Edit(person);

            // Assert
            repositoryMock.Verify(r => r.Update(person), Times.Once());
            repositoryMock.Verify(r => r.SaveChanges(), Times.AtLeastOnce());
            repositoryMock.Verify(r => r.List<Person>(), Times.AtLeastOnce());

            Assert.IsNotNull(result);

            // check that the control returns a "_List" partial view with the results of the repository List<Person> method
            Assert.AreEqual(typeof(PartialViewResult), result.GetType());
            Assert.AreEqual("_List", (((PartialViewResult)result)).ViewName);
            //Assert.AreEqual(persons, (((PartialViewResult)result)).Model);
            Assert.AreEqual(persons.Count, ((List<Person>)(((PartialViewResult)result)).Model).Count);
            Assert.AreEqual(persons[1], ((List<Person>)(((PartialViewResult)result)).Model)[1]);

            Assert.IsNotNull(((PartialViewResult)result).ViewData["GenderList"]);
        }

        [TestMethod]
        public void ShouldDisplayDelete()
        {
            // Arrange
            const int id = 1;
            Person person = new Person { Id = 1, FirstName = "TestFirstName1", LastName = "TestLastName1" };

            var repositoryMock = new Mock<IRepository>();
            PersonController controller = new PersonController(repositoryMock.Object);

            repositoryMock.Setup(r => r.GetById<Person>(id)).Returns(person);

            // Act
            ActionResult result = controller.Delete(id);

            // Assert
            repositoryMock.Verify(r => r.GetById<Person>(id), Times.Exactly(1));

            Assert.IsNotNull(result);

            // check the Model
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(Person), ((ViewResultBase)(result)).Model.GetType());
            Assert.AreEqual(person.Id, ((Person)((ViewResultBase)(result)).Model).Id);
        }

        [TestMethod]
        public void ShouldDeleteOnPostback()
        {
            // Arrange
            const int id = 1;
            Person person = new Person { Id = 1, FirstName = "TestFirstName1", LastName = "TestLastName1" };

            var repositoryMock = new Mock<IRepository>();
            PersonController controller = new PersonController(repositoryMock.Object);

            repositoryMock.Setup(r => r.GetById<Person>(id)).Returns(person);

            // Act
            ActionResult result = controller.DeleteConfirmed(id);

            // Assert
            repositoryMock.Verify(r => r.GetById<Person>(id), Times.Once());
            repositoryMock.Verify(r => r.Delete(person), Times.Once());
            repositoryMock.Verify(r => r.SaveChanges(), Times.Once());

            // Assert
            Assert.IsNotNull(result);
            // check that the controller returns a redirect to action where the action method is "Index"
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
            Assert.AreEqual("Index", (((RedirectToRouteResult)result)).RouteValues["action"]);
        }

        [TestMethod]
        public void ShouldEditRedirectToIndexOnExceptionDuringPostback()
        {
            // Arrange
            Person person = new Person { FirstName = "TestFirstName1", LastName = "TestLastName1" };

            var repositoryMock = new Mock<IRepository>();

            PersonController controller = new PersonController(repositoryMock.Object);
            controller.SetFakeControllerContext(); // Use the extension method in our MvcMoqHelper class

            // Force the repository mock repository's Update method to throw an exception
            repositoryMock.Setup(r => r.Update(person)).Throws<InvalidOperationException>();

            // Act
            ActionResult result = controller.Edit(person);

            // Assert
            repositoryMock.Verify(r => r.Update(person), Times.AtLeastOnce());
            repositoryMock.Verify(r => r.SaveChanges(), Times.Never());
            repositoryMock.Verify(r => r.List<Person>(), Times.Never());

            // check that the controller returns a redirect to action where the action method is "Index"
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
            Assert.AreEqual("Index", (((RedirectToRouteResult)result)).RouteValues["action"]);
        }

    }
}
