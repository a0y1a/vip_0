using KeKeSoftPlatform.Core;
using System.Web;
using System.Web.Mvc;

namespace KeKeSoftPlatform.App
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new NavigationAttribute());
            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new LogExceptionAttribute());
        }
    }
}
