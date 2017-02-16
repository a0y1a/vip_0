using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using KeKeSoftPlatform.Common;

namespace KeKeSoftPlatform.Core
{
    public enum UserType
    {
        User,
        SystemAdmin
    }
    public class _User:BaseUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string OpenId { get; set; }
        public DateTime LoginDate { get; set; }
        public RightType RightType { get; set; }

        public string UserLoginTicketId
        {
            get
            {
                return EncryptUtils.MD5Encrypt(this.Id + this.LoginDate.ToOADate().ToString());
            }
        }

        public UserType Type { get; set; }

        public static _User Current
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Request.IsAuthenticated == false)
                {
                    throw new Exception("未登录访问错误_User");
                }
                return (HttpContext.Current.User as FormsPrincipal<_User>).UserData;
            }
        }
    }

    public enum RightType
    {
        /// <summary>
        /// 匿名访问
        /// </summary>
        Anonymous = 1,
        /// <summary>
        /// User
        /// </summary>
        User = 2,
        /// <summary>
        /// Admin
        /// </summary>
        Admin = 4
       
    }
}
