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
        private TransactionScope _transactionScope = null;

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
            //if (_context.Entry(entityToDelete).State == EntityState.Detached)
            try
            {
                _context.Set<TEntity>().Attach(entityToDelete);

            }
            finally
            {
                _context.Set<TEntity>().Remove(entityToDelete);
            } 
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
            //_context.Entry(entityToUpdate).State = EntityState.Modified;
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
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
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
