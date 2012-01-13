using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Trails2012
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());

            // Andy - The default error handler does not pass handled exceptions through to Elmah,
            // so instead of using the MVC version of the class, use our custom version, which 
            // writes passes exceptions through to elmah 
            filters.Add(new Trails2012.Attributes.HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("favicon.ico"); 
            routes.IgnoreRoute("{favicon}", new { favicon = @"(./)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            //routes.MapRoute(
            //    "TripAjaxSearch",
            //    "Trip/getAutoComplete/",
            //    new { controller = "Trip", action = "getAutoComplete" }
            //);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //DependencyResolver.SetResolver(new Trails2010DependencyResolver());
        }

        public class Trails2010DependencyResolver
        : IDependencyResolver
        {
            public object GetService(Type serviceType)
            {
                //if (serviceType == typeof(Controllers.HomeController))
                //{
                //    var controller = Activator.CreateInstance(serviceType) as Controllers.HomeController;
                //    controller.MessageText = "Welcome, this text has been injected!";
                //    return controller;
                //}

                if (serviceType.IsInterface)
                {
                    if (serviceType == typeof(IControllerFactory)) return new DefaultControllerFactory();
                    if (serviceType == typeof(IControllerActivator)) return null;
                    if (serviceType == typeof(IFilterProvider)) return GlobalFilters.Filters;
                    if (serviceType == typeof(IViewEngine)) return new RazorViewEngine();
                    if (serviceType == typeof(IViewPageActivator)) return null;
                }

                return Activator.CreateInstance(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return new object[] { GetService(serviceType) };
            }
        }
    }
}