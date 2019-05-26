using System.Web;
using System.Web.Optimization;

namespace eTUTOR
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //user resource
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/vendor/bootstrap/css/bootstrap.min.css",
                      "~/Content/vendor/font-awesome/css/font-awesome.min.css",
                      "~/Content/build/css/css-plugin-collections.css",
                      "~/Content/build/css/pe-icon-7-stroke.css",
                      "~/Content/build/css/flaticon.css"

            ));
            bundles.Add(new ScriptBundle("~/Content/js").Include(
                      "~/Content/vendor/jquery/jquery.min.js",
                      "~/Content/vendor/bootstrap/js/bootstrap.min.js"
            ));
            
            //admin resource
            bundles.Add(new StyleBundle("~/Admin/Content/css").Include(
                      "~/Content/vendor/bootstrap/css/bootstrap.min.css",
                      "~/Content/vendor/font-awesome/css/font-awesome.min.css",
                      "~/Content/vendor/font-awesome/css/material-dashboard.css",
                      "~/Content/build/admin/css/demo.css",
                      "~/Content/build/admin/css/switch.css",
                      "~/Content/vendor/lightbox2/dist/css/lightbox.min.css"
            ));
            bundles.Add(new ScriptBundle("~/Admin/Content/js").Include(
                      
            ));
        }
    }
}
