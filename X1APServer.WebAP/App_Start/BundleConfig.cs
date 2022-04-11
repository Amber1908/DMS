using System.Web;
using System.Web.Optimization;

namespace X1APServer
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Common").Include());

            bundles.Add(new Bundle("~/Scripts/Index").Include(
                "~/Scripts/dist/Index.*",
                "~/Scripts/dist/Index~PDFViewer.*",
                "~/Scripts/dist/npm.*",
                "~/Scripts/dist/runtime.*"));

            bundles.Add(new ScriptBundle("~/Scripts/PDFViewer").Include(
                "~/Scripts/dist/PDFViewer.*",
                "~/Scripts/dist/Index~PDFViewer.*",
                "~/Scripts/dist/npm.*",
                "~/Scripts/dist/runtime.*"));

            bundles.Add(new ScriptBundle("~/Scripts/PlayGround").Include(
                "~/Scripts/dist/PlayGround.*",
                "~/Scripts/dist/npm.*",
                "~/Scripts/dist/runtime.*"));

            bundles.Add(new StyleBundle("~/Content/CSS/Common").Include(
                    "~/Content/CSS/bootstrap.min.css",
                    "~/Content/CSS/style.css",
                    "~/Content/CSS/animate.css",
                    "~/Content/CSS/biomdcareStyle.css",
                    "~/Content/CSS/font-awesome.css",
                    "~/Content/CSS/datetime.css",
                    "~/Content/CSS/bootstrap-datepicker3.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/Index").Include(
                    "~/Content/CSS/workfeed.css"
                ));

            bundles.Add(new StyleBundle("~/Content/PDFViewer").Include());
        }
    }
}
