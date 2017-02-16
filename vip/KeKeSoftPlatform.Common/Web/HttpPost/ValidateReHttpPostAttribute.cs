using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace KeKeSoftPlatform.Common
{
    public class ValidateReHttpPostAttribute : ActionFilterAttribute
    {
        private PageTokenViewBase _PageTokenView;
        public ValidateReHttpPostAttribute()
        {
            this._PageTokenView = new SessionPageTokenView();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.HttpMethod.Equals("POST") && filterContext.RequestContext.HttpContext.Request.IsAjaxRequest()==false)
            {
                if (this._PageTokenView.TokensMatch == false)
                {
                    throw new ArgumentNullException("Invaild Http Post!");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
