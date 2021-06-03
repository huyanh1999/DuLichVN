using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DC.Webs.Common
{
    public static class DependencySolve
    {
        public static T GetServiceName<T>()
        {
            return (T)DependencyResolver.Current.GetService(typeof(T));
        }
    }
}