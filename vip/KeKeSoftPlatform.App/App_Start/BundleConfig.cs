using System.Web;
using System.Web.Optimization;

namespace KeKeSoftPlatform.App
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                       "~/Scripts/jquery-1.9.1.min.js",
                       "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/alertify/alertify.min.js",
                        //"~/Scripts/tooltip.js",
                        "~/Scripts/common.js"));

            // 使用 Modernizr 的开发版本进行开发和了解信息。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.min.css",
                        "~/Content/alertify/alertify.core.css",
                        "~/Content/alertify/alertify.bootstrap.css",
                        "~/Content/font-awesome.min.css",
                        "~/Content/site.css"));


            bundles.Add(new ScriptBundle("~/bundles/common/weui").Include(
                     "~/Scripts/weui/jweixin-1.0.0.js",
                     "~/Scripts/weui/weui.min.js",
                     "~/Scripts/jquery-1.9.1.min.js"));

            bundles.Add(new StyleBundle("~/Content/weui/css").Include(
                    "~/Content/weui/weui.min-1.0.0.css"));

        }
    }
}
