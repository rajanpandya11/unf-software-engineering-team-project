using System.Web;
using System.Web.Optimization;

namespace WebApplication2
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                      "~/Scripts/jquery-ui-{version}.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/creative.js",
                      "~/Scripts/creative.min.js",
                      "~/Scripts/jquery.magnific-popup.min.js",
                      "~/Scripts/scrollreveal.min.js",
                      "~/Scripts/respond.js", 
                      "~/scripts/datatables/jquery.datatables.js",

                      "~/Scripts/datatables/datatables.bootstrap.js",
                      "~/Scripts/scrollreveal.js",
                      "~/Scripts/scrollreveal.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-lumen.css",
                      "~/Content/creative.css",      
                      "~/Content/font-awesome.css",
                      "~/Content/magnific-popup.css",                    
                      "~/content/datatables/css/datatables.bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/highcharts").Include(
                    "~/Scripts/Highcharts-4.0.1/js/highcharts.js"));
        }


    }
}
