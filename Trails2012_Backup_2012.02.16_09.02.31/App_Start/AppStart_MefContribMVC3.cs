[assembly: WebActivator.PreApplicationStartMethod(typeof(Trails2012.App_Start.AppStart_MefContribMVC3), "Start")]

namespace Trails2012.App_Start
{
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using System.Web.Mvc;
    using MefContrib.Hosting.Conventions;
    using MefContrib.Web.Mvc;

    public static class AppStart_MefContribMVC3
    {
        public static void Start()
        {
            // Register the CompositionContainerLifetimeHttpModule HttpModule.
            // This makes sure everything is cleaned up correctly after each request.
            CompositionContainerLifetimeHttpModule.Register();

            // Create MEF catalog based on the contents of ~/bin.
            //
            // Note that any class in the referenced assemblies implementing in "IController"
            // is automatically exported to MEF. There is no need for explicit [Export] attributes
            // on ASP.NET MVC controllers. When implementing multiple constructors ensure that
            // there is one constructor marked with the [ImportingConstructor] attribute.
            var catalog = new AggregateCatalog(
                new DirectoryCatalog(@"bin\PlugIns\"), // see comment 1
                new ConventionCatalog(new MvcApplicationRegistry())); // Note: add your own (convention)catalogs here if needed.

            // Comment 1
            // Andy - change the directory from "bin" to "bin\Plugins" - 
            // Use the PlugIns diectory to hold the MEF plug ins, including all dependent dlls, 
            // and use probing to allow the executing assembly to locate the dependent assemblies.
            // That way, the plug in dlls do not deleted on each rebuild.
            // However, it requires the PlugIns folder to be manually created, and plug in dlls need to be manually added in there.

            // Tell MVC3 to use MEF as its dependency resolver.
            var dependencyResolver = new CompositionDependencyResolver(catalog);
            DependencyResolver.SetResolver(dependencyResolver);

            // Tell MVC3 to resolve dependencies in controllers
            ControllerBuilder.Current.SetControllerFactory(
                new CompositionControllerFactory(
                    new CompositionControllerActivator(dependencyResolver)));

            // Tell MVC3 to resolve dependencies in filters
            FilterProviders.Providers.Remove(FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider));
            FilterProviders.Providers.Add(new CompositionFilterAttributeFilterProvider(dependencyResolver));

            // Tell MVC3 to resolve dependencies in model validators
            ModelValidatorProviders.Providers.Remove(ModelValidatorProviders.Providers.OfType<DataAnnotationsModelValidatorProvider>().Single());
            ModelValidatorProviders.Providers.Add(
                new CompositionDataAnnotationsModelValidatorProvider(dependencyResolver));

            // Tell MVC3 to resolve model binders through MEF. Note that a model binder should be decorated
            // with [ModelBinderExport].
            ModelBinderProviders.BinderProviders.Add(
                new CompositionModelBinderProvider(dependencyResolver));
        }
    }
}