using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace KeKeSoftPlatform.Core
{
    public class NavigationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!string.IsNullOrWhiteSpace(filterContext.HttpContext.Request.QueryString[Navigation.TOP_KEY]))
            {
                filterContext.HttpContext.Session[Navigation.TOP_KEY] = int.Parse(filterContext.HttpContext.Request.QueryString[Navigation.TOP_KEY]);
            }

            if (!string.IsNullOrWhiteSpace(filterContext.HttpContext.Request.QueryString[Navigation.ITEM_KEY]))
            {
                filterContext.HttpContext.Session[Navigation.ITEM_KEY] = int.Parse(filterContext.HttpContext.Request.QueryString[Navigation.ITEM_KEY]);
            }
        }
    }
}
