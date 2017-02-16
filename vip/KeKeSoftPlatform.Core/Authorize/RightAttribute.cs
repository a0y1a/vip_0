using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using KeKeSoftPlatform.Common;

namespace KeKeSoftPlatform.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RightAttribute : AuthorizeAttribute
    {
        public RightType Right { get; set; }

        public RightAttribute(RightType rightType)
        { 
            base.Order = 0;
            if (Enum.GetValues(typeof(RightType)).Cast<RightType>().Any(m => Convert.ToInt32(m) == ((int)rightType)))
            {
                if (rightType == RightType.Anonymous)
                {
                    Right = RightType.Anonymous
                                    .Add(RightType.Admin)
                                    .Add(RightType.User);
                }
                else if (rightType == RightType.User)
                {
                    Right = RightType.User;
                }
                else if (rightType == RightType.Admin)
                {
                    Right = RightType.Admin;
                }
                
            }
            else
            {
                this.Right = rightType;
            }
        }

        public RightAttribute()
        {
            //默认是普通管理员权限
            this.Right = RightType.Admin;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            ReturnValue<RedirectResult> result = null;
            if (filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
            {
                //if (_User.Current.RightType == RightType.Admin)
                //{
                //    result = new ReturnValue<RedirectResult>
                //    {
                //        IsSuccess = true
                //    };
                //}
                //else
                //{
                    result = new BaseAccess().Validate(filterContext, _User.Current.RightType);
                //}
            }
            else
            {
                //匿名访问
                result = new BaseAccess().Validate(filterContext, RightType.Anonymous);
            }
            if (result.IsSuccess == false)
            {
                filterContext.Result = result.Data;
                base.OnAuthorization(filterContext);
            }
        }
    }
}
