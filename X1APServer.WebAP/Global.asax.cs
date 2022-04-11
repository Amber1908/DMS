using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using WebApplication1;

namespace X1APServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //UnityConfig.RegisterComponents();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create());

            // remove json self refference loop
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;

            NLogConfig.Initialize();

            // 建立預設資料夾
            //FolderConfig.CreateFolder();
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Errors";
            routeData.Values["action"] = "General";
            routeData.Values["exception"] = exception;
            Response.ContentType = "text/html";
            Response.StatusCode = 500;
            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode)
                {
                    case 404:
                        routeData.Values["action"] = "PageNotFound";
                        break;
                }
            }

            IController errorsController = new WebApplication1.Controllers.ErrorsController();
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            errorsController.Execute(rc);
        }

        protected void Application_PostAuthorizeRequest()
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }

        protected void Application_EndRequest()
        {
            // Any AJAX request that ends in a redirect should get mapped to an unauthorized request
            // since it should only happen when the request is not authorized and gets automatically
            // redirected to the login page.
            
            var context = new HttpContextWrapper(Context);
            if (context.Response.StatusCode == 302 && context.Request.IsAjaxRequest())
            {
                context.Response.Clear();
                Context.Response.StatusCode = 401;
            }
        }

        //void Application_AuthenticateRequest(object sender, EventArgs e)
        //{
        //    //HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

        //    if (Request.IsAuthenticated)
        //    {
        //        // 先取得該使用者的 FormsIdentity
        //        FormsIdentity id = (FormsIdentity)User.Identity;
        //        // 再取出使用者的 FormsAuthenticationTicket
        //        FormsAuthenticationTicket ticket = id.Ticket;
        //        // 將儲存在 FormsAuthenticationTicket 中的角色定義取出，並轉成字串陣列
        //        string[] roles = ticket.UserData.Split(new char[] { ',' });
        //        // 指派角色到目前這個 HttpContext 的 User 物件去
        //        //剛剛在創立表單的時候，你的UserData 放使用者名稱就是取名稱，我放的是群組代號，所以取出來就是群組代號
        //        //然後會把這個資料放到Context.User內
        //        Context.User = new GenericPrincipal(Context.User.Identity, roles);
        //    }

        //}
    }
}
