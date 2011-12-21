using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Trails2012.DataAccess
{
    public class Util
    {
        public static PropertyInfo GetPropertyFromExpression<TEntity>(Expression<Func<TEntity, object>> propertyLambda)
        {
            // see http://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression
            Type type = typeof(TEntity);

            MemberExpression memberExpression = propertyLambda.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException(string.Format("Expression '{0}' does not refer to a property.",
                                                          propertyLambda));
            PropertyInfo propInfo = memberExpression.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format("Expression '{0}' refers to a field, not a property.",
                                                          propertyLambda.ToString()));

            if (type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a property that is not from type {1}.", propertyLambda, type));

            return propInfo;
        }

        public static string GetPropertyNameFromExpression<TEntity>(Expression<Func<TEntity, object>> propertyLambda)
        {
            return GetPropertyFromExpression(propertyLambda).Name;
        }

    }
}
