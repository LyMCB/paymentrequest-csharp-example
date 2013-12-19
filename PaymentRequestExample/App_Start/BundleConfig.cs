using System.Web;
using System.Web.Optimization;

namespace PaymentRequestExample
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                                                                 "~/Scripts/jquery-{version}.js",
                                                                 "~/Scripts/jquery.unobtrusive*",
                                                                 "~/Scripts/jquery.validate*",
                                                                 "~/Scripts/jquery.signalR-{version}.js",
                                                                 "~/Scripts/knockout-{version}.js",
                                                                 "~/Scripts/bootstrap-switch.min.js",
                                                                 "~/Scripts/bootstrap.js",
                                                                 "~/Scripts/Application.js",
                                                                 "~/Scripts/common.js"));

            bundles.Add(new ScriptBundle("~/Scripts/knockout").Include("~/Scripts/knockout-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                                                             "~/Content/bootstrap.css",
                                                             "~/Content/bootstrap-responsive.css",
                                                             "~/Content/bootstrap-datepicker.css",
                                                             "~/Content/bootstrap-override.css",
                                                             "~/Content/font-awesome.css",
                                                             "~/Content/flags.css",
                                                             "~/Content/site.css",
                                                             "~/Content/flat-ui-fonts.css",
                                                             "~/Content/bootstrap-switch.css",
                                                             "~/Content/site-responsive.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}