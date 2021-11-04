using System.Web;
using System.Web.Optimization;

namespace AdminPortal
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/plugins/jquery-ui.min.js",
                        "~/Scripts/plugins/jquery.ui.touch-punch.min.js",
                        "~/Scripts/plugins/jquery.dataTables.min.js",
                        "~/Scripts/popper.min.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/jquery-datatable/dataTables.bootstrap.min.js",
                        "~/Scripts/jquery-datatable/dataTable-select.js",
                        "~/Scripts/plugins/bootstrap-datepicker.min.js",
                        "~/Scripts/plugins/jquery.slimscroll.min.js",
                        "~/Scripts/plugins/perfect-scrollbar.min.js",
                        "~/Scripts/plugins/smooth-scrollbar.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/softui").Include(
                        "~/Scripts/soft-ui-dashboard.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundles/Login/style").Include(
                      "~/Content/style.css"));

            bundles.Add(new ScriptBundle("~/bundles/Login/Scripts").Include(
                      "~/Scripts/jquery.min.js",
                      "~/Scripts/popper.js",
                      "~/Scripts/main.js"
                      ));

            bundles.Add(new StyleBundle("~/Contents/Dashboard/style").Include(
                      "~/Content/nucleo-icons.css",
                      "~/Content/nucleo-svg.css",
                      "~/Content/soft-ui-dashboard.css",
                      "~/Scripts/jquery-datatable/dataTables.bootstrap.css",
                      "~/Scripts/jquery-datatable/dataTable-select.css",
                      "~/Content/custom.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/Dashboard/Scripts").Include(
                      "~/Scripts/core/soft-ui-dashboard.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/stepform/js").Include(
                      "~/Content/step_form/jquery.steps.js"
                      ));

            bundles.Add(new StyleBundle("~/Contents/stepform/style").Include(
                       "~/Content/step_form/style.css"
                      ));
        }
    }
}
