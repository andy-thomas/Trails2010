using NHibernate;

namespace Trails2012.DataAccess.NHib
{
    internal class NHibernateSessionTracker
    {
        private readonly ISessionFactory _sessionFactory;
        static ISession _session;

        public NHibernateSessionTracker(ISessionFactory sessionFactory)
        {
            this._sessionFactory = sessionFactory;
        }

        public static ISession GetCurrentSession(ISessionFactory sessionFactory)
        {
            if (_session == null)
            {
                _session = sessionFactory.OpenSession();
            }
            return _session;
        }

        internal static void CloseSession(ISessionFactory sessionFactory)
        {
            if (_session != null)
            {
                sessionFactory.Close();
                _session = null;
            }
        }
    }
}
