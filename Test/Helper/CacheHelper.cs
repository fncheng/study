using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Helper
{
    public class CacheHelper
    {
        public static object GetCache(string cachKey)
        {
            if (string.IsNullOrEmpty(cachKey))
            {
                return null;
            }
            try
            {
                return HttpContext.Current.Cache.Get(cachKey);
            }
            catch
            {
                return null;
            }
        }

        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }
    }
}