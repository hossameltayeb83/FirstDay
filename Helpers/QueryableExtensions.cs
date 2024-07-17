using FirstDay.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FirstDay.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string sortProperty, SortDirection sortOrder)
        {
            var type = typeof(T);
            var property = type.GetProperty(sortProperty);
            var parameter = Expression.Parameter(type, "e");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var typeArguments = new Type[] { type, property.PropertyType };
            var methodName = sortOrder == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";
            var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, source.Expression, Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<T>(resultExp);
        }
        //public static IQueryable<T> OrderByNavigationProperty<T>(this IQueryable<T> source, string sortProperty,string navigationProperty,SortDirection sortOrder)
        //{
        //    var type = typeof(T);
        //    var parameter = Expression.Parameter(type, "e");
        //    var navProperty = type.GetProperty(navigationProperty);
        //    var navtype = navProperty.GetType();
        //    var navAccess = Expression.MakeMemberAccess(parameter, navProperty);
        //    var property = navtype.GetProperty(sortProperty);
        //    var propertyAccess= Expression.MakeMemberAccess(navAccess, property);
        //    var orderByExp = Expression.Lambda(propertyAccess, parameter);
        //    var typeArguments = new Type[] { type, navProperty.PropertyType };
        //    var methodName = sortOrder == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";
        //    var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, source.Expression, Expression.Quote(orderByExp));

        //    return source.Provider.CreateQuery<T>(resultExp);
        //}
    }
}
