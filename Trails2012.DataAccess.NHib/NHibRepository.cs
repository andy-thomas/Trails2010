// ReSharper disable ConditionIsAlwaysTrueOrFalse
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;
using Trails2012.DataAccess.NHib.Conventions;
using Trails2012.DataAccess.NHib.Mappings;
using Trails2012.DataAccess.NHib.Overrides;
using Trails2012.Domain;

namespace Trails2012.DataAccess.NHib
{
    [Export(typeof(IRepository))]
    public class NHibRepository : IRepository
    {
        readonly ISessionFactory _sessionFactory;
        public NHibRepository()
        {
            _sessionFactory = CreateSessionFactory();
        }

        public NHibRepository(bool useInMemoryDatabase)
        {
            _sessionFactory = CreateSessionFactory(useInMemoryDatabase);
        }

        private ISessionFactory CreateSessionFactory(bool useInMemoryDatabase = false)
        {
                AutoPersistenceModel model = AutoMap.AssemblyOf<Trail>(new TrailsConfiguration())
                    .IgnoreBase<EntityBase>()
                    .Conventions.AddFromAssemblyOf<CustomForeignKeyConvention>()
                    // does this line even work? Does not appear to add CustomPrimaryKeyConvention
                    .Conventions.Add<CustomPrimaryKeyConvention>()
                    .UseOverridesFromAssemblyOf<DifficultyMappingOverride>();

#if(DEBUG)
            const bool exportFiles = true;

            try
            {

                if (useInMemoryDatabase)
                {
                    FluentConfiguration config = Fluently.Configure()
                        .Database(SQLiteConfiguration.Standard.InMemory)
                        //.ExposeConfiguration(BuildSchema)     this does not appear to work - see comment 1
                        .Mappings(m =>
                        {
                            m.FluentMappings.AddFromAssemblyOf<PersonMap>();
                            if (exportFiles)
                                m.FluentMappings.ExportTo(
                                    @"C:\Projects\Personal\Trails2012\ExportedMappings");

                            m.AutoMappings.Add(model);
                            if (exportFiles)
                                m.AutoMappings.ExportTo(
                                    @"C:\Projects\Personal\Trails2012\ExportedMappings");
                        });

                    ISessionFactory sessionFactory = config.BuildSessionFactory();

                    // Build the schema here using the current session - see Comment 1
                    SessionSource sessionSource = new SessionSource(config);
                    ISession session = NHibernateSessionTracker.GetCurrentSession(sessionFactory);
                    sessionSource.BuildSchema(session);
 
                    return sessionFactory;

                }
                else
                {
                    return Fluently.Configure()
                        .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2008.ConnectionString
                                      (c => c.FromConnectionStringWithKey("TrailsContext")).ShowSql())
                        //.ProxyFactoryFactory("NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle")
                        .Mappings(m =>
                                      {
                                          m.FluentMappings.AddFromAssemblyOf<PersonMap>();
                                          if (exportFiles)
                                              m.FluentMappings.ExportTo(
                                                  @"C:\Projects\Personal\Trails2012\ExportedMappings");

                                          m.AutoMappings.Add(model);
                                          if (exportFiles)
                                              m.AutoMappings.ExportTo(
                                                  @"C:\Projects\Personal\Trails2012\ExportedMappings");
                                      })
                        .BuildSessionFactory();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
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
            //ICriteria criteria = session.CreateCriteria<TEntity>();
            //foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            //{
            //    string propertyName = Util.GetPropertyNameFromExpression(includeProperty);
            //    criteria.SetFetchMode(propertyName, FetchMode.Eager);
            //}
            //return criteria.List<TEntity>();

            IQueryable<TEntity> query = session.Query<TEntity>();
            // Include Properties
            if (includeProperties != null)
                foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
                {
                    query = query.Fetch(includeProperty);
                }

            return query.ToList();
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



        // Comment 1
        // Andy - this should (and does) build the schema for the SQLite in memory database
        // However, it appears to not take effect when running unit tests ("error no such table...")
        // So instead, I create the configuration and SessionFactory, and then get the current session.
        // Only the do I explicitly build the schema. This way, the schema is built using the same session that the unit test use.
        // see http://stackoverflow.com/questions/4325800/testing-nhibernate-with-sqlite-no-such-table-schema-is-generated 
        // and James' link for some more background.
        private void BuildSchema(Configuration cfg)
        {
            // This will write the schema creation script to file
            // However, see comment 1 
            string path = @"C:\Projects\Personal\Trails2012\ExportedMappings\Schema.sql";
            if (File.Exists(path)) File.Delete(path);

            Action<string> updateExport = x =>
            {
                using (
                     FileStream file = new FileStream(path, FileMode.Append, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(x);
                    sw.Close();
                }
            };

            // Option 1 - see http://stackoverflow.com/questions/2096808/sqlite-no-such-table-when-saving-object
            //ISession session = GetSessionFactory().OpenSession();
            ////the key point is pass your session.Connection here 
            //new SchemaExport(cfg).Execute(true, true, false, session.Connection, null);

            // Option 2 - from http://stackoverflow.com/questions/2483424/make-fluent-nhibernate-output-schema-update-to-file
            //new SchemaUpdate(cfg).Execute(updateExport, true);

            // Option 3 - from Fluent Nhib Wiki - http://wiki.fluentnhibernate.org/Schema_generation
            //new SchemaExport(cfg).Create(false, true);
        }


    }

}
// ReSharper restore ConditionIsAlwaysTrueOrFalse
