// ReSharper disable ConditionIsAlwaysTrueOrFalse
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Linq;
using Trails2012.DataAccess.NHib.Conventions;
using Trails2012.DataAccess.NHib.Mappings;
using Trails2012.DataAccess.NHib.Overrides;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib
{
    public class NHibRepository : IRepository
    {
        readonly ISessionFactory _sessionFactory;
        public NHibRepository()
        {
            _sessionFactory = CreateSessionFactory();
        }

        private ISessionFactory CreateSessionFactory()
        {

            AutoPersistenceModel model = AutoMap.AssemblyOf<Trail>(new TrailsConfiguration())
                .IgnoreBase<EntityBase>()
                .Conventions.AddFromAssemblyOf<CustomForeignKeyConvention>()
                // does this line even work? Does not appear to add CustomPrimaryKeyConvention
                .Conventions.Add<CustomPrimaryKeyConvention>()
                .UseOverridesFromAssemblyOf<DifficultyMappingOverride>();

#if(DEBUG)
            const bool exportFiles = true;

            return Fluently.Configure()
                .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2008.ConnectionString
                              (c => c.FromConnectionStringWithKey("TrailsContext")).ShowSql())
                //.ProxyFactoryFactory("NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle")
                .Mappings(m =>
                              {
                                  m.FluentMappings.AddFromAssemblyOf<PersonMap>();
                                  if (exportFiles)
                                      m.FluentMappings.ExportTo(@"C:\Projects\Personal\Trails2012\ExportedMappings");

                                  m.AutoMappings.Add(model);
                                  if (exportFiles)
                                      m.AutoMappings.ExportTo(@"C:\Projects\Personal\Trails2012\ExportedMappings");
                              })
                .BuildSessionFactory();

#else
                return Fluently.Configure()
                    .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey("TrailsContext")))
                    .Mappings(m =>
                                    {
                                        m.FluentMappings.AddFromAssemblyOf<PersonMap>();
                                        m.AutoMappings.Add(model);
                                    })
                    .BuildSessionFactory();
#endif

        }

        #region IRepository Members

        public void BeginTransaction()
        {
            NHibernateSessionTracker.GetCurrentSession(_sessionFactory).BeginTransaction();
        }

        public void CommitTransaction()
        {
            ISession session = NHibernateSessionTracker.GetCurrentSession(_sessionFactory);
            if (session.Transaction != null &&
                session.Transaction.IsActive)
            {
                session.Transaction.Commit();
                session.Close();
            }
            NHibernateSessionTracker.CloseSession(_sessionFactory);
        }

        public void RollbackTransaction()
        {
            ISession session = NHibernateSessionTracker.GetCurrentSession(_sessionFactory);
            if (session.Transaction != null &&
                session.Transaction.IsActive)
            {
                session.Transaction.Rollback();
                session.Close();
            }
            NHibernateSessionTracker.CloseSession(_sessionFactory);
        }

        public void SaveChanges()
        {
            ISession session = NHibernateSessionTracker.GetCurrentSession(_sessionFactory);           
            session.Flush();
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            ISession session = NHibernateSessionTracker.GetCurrentSession(_sessionFactory);
            session.Delete(entity);
        }

        public void Delete<TEntity>(int id) where TEntity : class
        {
            ISession session = NHibernateSessionTracker.GetCurrentSession(_sessionFactory);
            TEntity entityToDelete = session.Get<TEntity>(id);
            Delete(entityToDelete);
        }

        public IEnumerable<TEntity> List<TEntity>() where TEntity : class
        {
            ISession session = NHibernateSessionTracker.GetCurrentSession(_sessionFactory);           
            return session.CreateCriteria<TEntity>().List<TEntity>();
        }

        public IEnumerable<TEntity> ListIncluding<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class
        {
            ISession session = NHibernateSessionTracker.GetCurrentSession(_sessionFactory);
            ICriteria criteria = session.CreateCriteria<TEntity>();
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {
                string propertyName = Util.GetPropertyNameFromExpression(includeProperty);
                criteria.SetFetchMode(propertyName, FetchMode.Eager);
            }
            return criteria.List<TEntity>();
        }

        public TEntity GetById<TEntity>(int id) where TEntity : class
        {
            ISession session = NHibernateSessionTracker.GetCurrentSession(_sessionFactory);           
            return session.Get<TEntity>(id);            
        }

        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class
        {
            ISession session = NHibernateSessionTracker.GetCurrentSession(_sessionFactory);
            session.SaveOrUpdate(entity);
            return entity;
        }

        public TEntity Update<TEntity>(TEntity entity) where TEntity : class
        {
            ISession session = NHibernateSessionTracker.GetCurrentSession(_sessionFactory);
            session.Update(entity);
            return entity;
        }

        public IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, object>>[] includeProperties = null) where TEntity : class
        {
            // Use Linq to NHibernate to build the query
            ISession session = NHibernateSessionTracker.GetCurrentSession(_sessionFactory);
            IQueryable<TEntity> query = session.Query<TEntity>();

            // Where 
            if (filter != null) 
                query = query.Where(filter);

            // Order By
            if (orderBy != null) 
                query = orderBy(query);

            // Include Properties
            if (includeProperties != null)
                foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
                {
                    query = query.Fetch(includeProperty);
                }

            return query;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            NHibernateSessionTracker.CloseSession(_sessionFactory);          
            if(_sessionFactory != null)
                _sessionFactory.Dispose();
        }

        #endregion
    }

}
// ReSharper restore ConditionIsAlwaysTrueOrFalse
