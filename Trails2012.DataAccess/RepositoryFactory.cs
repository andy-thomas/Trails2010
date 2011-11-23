using System.ComponentModel.Composition;

namespace Trails2012.DataAccess
{
    public class RepositoryFactory
    {
        [Import]
        public IRepository Repository { get; set; }
    }
}
