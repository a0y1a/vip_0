using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using KeKeSoftPlatform.Core;
using System.Data.Entity;
using FluentValidation.Mvc;
using FluentValidation;
using KeKeSoftPlatform.Common;
using System.IO;
using System.Web.WebPages;
using System.Text.RegularExpressions;
//using StackExchange.Profiling;

namespace KeKeSoftPlatform.App
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            AuthenticateRequest += MvcApplication_AuthenticateRequest;
        }
        void MvcApplication_AuthenticateRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            //解密加密 cookie
            FormsPrincipal<_User>.RestoreUser(app.Context);
        }

        protected void Application_Start()
        {
            //注册区域
            AreaRegistration.RegisterAllAreas();
            //注册全局过滤器
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //注册路由
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //注册 css样式表 和 js文件
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //数据校验
            FluentValidationModelValidatorProvider.Configure();
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            ValidatorOptions.ResourceProviderType = typeof(Resources);

            DisplayModeProvider.Instance.Modes.RemoveAt(0);
            //判断是否请求是PC端还是手机端
            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("mobile")
            {
                ContextCondition = (context) => Regex.Match(context.GetOverriddenUserAgent(), @"iPhone|Android").Success
            });

            if (!Directory.Exists(Server.MapPath(@"~/Upload/Template")))
            {
                Directory.CreateDirectory(Server.MapPath(@"~/Upload/Template"));
            }
            if (!Directory.Exists(Server.MapPath(@"~/Upload/Import")))
            {
                Directory.CreateDirectory(Server.MapPath(@"~/Upload/Import"));
            }
            if (!Directory.Exists(Server.MapPath(@"~/Upload/File")))
            {
                Directory.CreateDirectory(Server.MapPath(@"~/Upload/File"));
            }
            if (!Directory.Exists(Server.MapPath(@"~/Upload/Test")))
            {
                Directory.CreateDirectory(Server.MapPath(@"~/Upload/Test"));
            }

            //StackExchange.Profiling.EntityFramework6.MiniProfilerEF6.Initialize();
        }

        //protected void Application_BeginRequest()
        //{
        //    MiniProfiler.Start();
        //}

        //protected void Application_EndRequest()
        //{
        //    MiniProfiler.Stop();
        //}
    }
}
