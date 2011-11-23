using System.ComponentModel.Composition;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Trails2012.Tests")] 

namespace Trails2012.DataAccess
{
    public class RepositoryFactory
    {
        [Import]
        public IRepository Repository { get; set; }

        public RepositoryFactory(){}

        // This constructor is visible only to this namespace and to the Trails2012.Tests assembly for dependency injection purposes
        // (Though, in this case it is not needed, since the setter on the Repository property is public)
        internal RepositoryFactory(IRepository repository) { Repository = repository; }

    }
}
