using System;
using NHibernate;
using NHibernate.Context;

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
            // Option 1 (see also Dispose method)
            // Use GetCurrentSession method
            // see http://stackoverflow.com/questions/1035466/fluentnhibernate-and-not-recognizing-the-session
            //ISession session = null;
            //try
            //{
            //    session = sessionFactory.GetCurrentSession();
            //}
            //catch (HibernateException)
            //{
            //    // do nothing here -- leave session as null
            //}

            //if (session == null)
            //{
            //    session = sessionFactory.OpenSession();
            //    CurrentSessionContext.Bind(session);
            //}

            //return sessionFactory.GetCurrentSession();

            // Option 2
            // Or this is simpler...
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
                // If using Option 1, incude this
                //var session = CurrentSessionContext.Unbind(sessionFactory);
                //session.Dispose(); 

                sessionFactory.Close();
                _session = null;
            }
        }
    }
}
