using KeKeSoftPlatform.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace KeKeSoftPlatform.Core
{
    public class BaseAccess
    {
        public virtual ReturnValue<RedirectResult> Validate(AuthorizationContext filterContext,RightType currentRight)
        {
            IEnumerable<object> rightAttributeCollection = null;
            rightAttributeCollection = filterContext.ActionDescriptor.GetCustomAttributes(typeof(RightAttribute), false);
            if (rightAttributeCollection == null || !rightAttributeCollection.Any())
            {
                rightAttributeCollection = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(RightAttribute), true);
            }

            if (rightAttributeCollection != null && rightAttributeCollection.Any())
            {
                var accessAttribute = rightAttributeCollection.First() as RightAttribute;
                if (accessAttribute.Right.Has(currentRight))
                {
                    return new ReturnValue<RedirectResult>
                    {
                        IsSuccess = true
                    };
                }
            }
            return new ReturnValue<RedirectResult>
            {
                IsSuccess = false,
                Data = new RedirectResult("/system/Unauthorized")
            };
        }
    }
}
