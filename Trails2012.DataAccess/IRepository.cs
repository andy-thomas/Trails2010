using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Trails2012.DataAccess
{
    /// <summary>
    /// Repository interface
    /// </summary>
    public interface IRepository : IDisposable
    {
        #region Commented code
        ///// <summary>
        ///// Creates the schema.
        ///// </summary>
        //void CreateSchema();

        ///// <summary>
        ///// Called when request begins (to allow session-per-request pattern).
        ///// </summary>
        //void BeginRequest();

        ///// <summary>
        ///// Called when request ends (to allow session-per-request pattern).
        ///// </summary>
        //void EndRequest(); 
        #endregion

        /// <summary>
        /// Begins a transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commits the active transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rollbacks the active transaction.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Saves the specified object.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Deletes the specified object.
        /// </summary>
        void Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Deletes the object specified by the id.
        /// </summary>
        void Delete<TEntity>(int id) where TEntity : class;

        /// <summary>
        /// Returns an enumerable list of entities of the given type
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <returns></returns>
        IEnumerable<TEntity> List<TEntity>() where TEntity : class;
        IEnumerable<TEntity> ListIncluding<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class;

        /// <summary>
        /// Gets the entity by id.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        TEntity GetById<TEntity>(int id) where TEntity : class;

        /// <summary>
        /// Insert a new instance in the repo
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Insert<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Updates an attach entity in the repo
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// General purpose search method
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IQueryable<TEntity> Get<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "") where TEntity : class;

    }
}

