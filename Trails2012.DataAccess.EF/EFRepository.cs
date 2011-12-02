using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace Trails2012.DataAccess.EF
{
    [Export(typeof(IRepository))]
    public class EFRepository : IRepository
    {
        // see http://www.asp.net/entity-framework/tutorials/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application

        readonly TrailsContext _context = new TrailsContext();
        private TransactionScope _transactionScope;

        public EFRepository()
        {
            _context.Configuration.ProxyCreationEnabled = false;
        }

        public void BeginTransaction()
        {
            _transactionScope = new TransactionScope();
        }

        public void CommitTransaction()
        {
            if (_transactionScope != null)
            {
                try
                {
                    _transactionScope.Complete();
                }
                finally 
                {
                    _transactionScope.Dispose();
                    _transactionScope = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            if(_transactionScope != null)
            {
                _transactionScope.Dispose();
                _transactionScope = null;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Delete<TEntity>(TEntity entityToDelete) where TEntity : class
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
                _context.Set<TEntity>().Attach(entityToDelete);
            _context.Set<TEntity>().Remove(entityToDelete);
        }

        public void Delete<TEntity>(int id) where TEntity : class
        {
            TEntity entityToDelete = _context.Set<TEntity>().Find(id);
            Delete(entityToDelete);
        }

        public IEnumerable<TEntity> List<TEntity>() where TEntity : class
        {
             return _context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> ListIncluding<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
        {
            bool cachedSetting = _context.Configuration.ProxyCreationEnabled;
            _context.Configuration.ProxyCreationEnabled = false;

            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (includeProperties != null)
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            IEnumerable<TEntity> list = query.ToList();
            _context.Configuration.ProxyCreationEnabled = cachedSetting;

            return list;
        } 

        public TEntity GetById<TEntity>(int id) where TEntity : class
        {
            return _context.Set<TEntity>().Find(id);
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class
        {
            return _context.Set<TEntity>().Add(entity);
        }

        public TEntity Update<TEntity>(TEntity entityToUpdate) where TEntity : class
        {
            _context.Set<TEntity>().Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            return entityToUpdate;
        }

        public IQueryable<TEntity> Get<TEntity>(
                Expression<Func<TEntity, bool>> filter = null, 
                Func<IQueryable<TEntity>, 
                IOrderedQueryable<TEntity>> orderBy = null,
                string includeProperties = "") where TEntity : class
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query;
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        } 

    }
}

// _Comment 1_
// When the list of Locations is returned in the Location|_SelectAjaxEditing partial view
//      it is serialized into JSON in order to get injected into the Telerik grid (by using the GridModel class, I guess)
// When this happens, we get the following error:
//      "A circular reference was detected while serializing an object of type 'System.Data.Entity.DynamicProxies".
// This appears to be a known bug in Entity Framework:
//      see for instance: http://www.google.ca/search?q=DynamicProxies+%22A+circular+reference+was+detected+while+serializing+an+object+of+type+%22&hl=en&num=10&lr=&ft=i&cr=&safe=images
//      http://stackoverflow.com/questions/4606232/circular-reference-exception-with-json-serialisation-with-mvc3-and-ef4-ctp5w
//      http://stackoverflow.com/questions/5588143/ef-4-1-code-first-json-circular-reference-serialization-error
//      http://stackoverflow.com/questions/7608372/entityframework-to-json-workaround-a-circular-reference-was-detected-while-se
// It seems that the most effective way of getting around this is to load the actual objects rather than the EF proxies. In order to do this,
//      it is necessary to turn off the ProxyCreationEnabled setting on the EF context temporarily, and to specifically eagerly load all required 
//      related object using the "Include" commend in the query.
//      see http://stackoverflow.com/questions/6686525/c-sharp-entity-framework-loading-from-database-without-proxy-classes.
// So, my implementation hack was to add a "ListIncluding" method to the repository, which is based on the AllIncluding method which is included in MVC scaffolding
//      see http://stackoverflow.com/questions/6954387/mvc-3-scaffolding-the-model-passed-to-the-view-throws-sql-errror
// However, this was still returning proxies for the main calls, so I enhanced it to turn off the ProxyCreationEnabled flag, and return a list rather than IQueryable 
//      (though returning IQueryable<> is probably better). This fixes the issue where JSON serialization is used, so may only occur in this application whenever 
//      AJAX is being used (say, when using the Telerik grid control).

