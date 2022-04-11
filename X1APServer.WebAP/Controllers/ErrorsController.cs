using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebApplication1.Controllers
{
    public class ErrorsController : Controller
    {
        private readonly Logger logger = LogManager.GetLogger("WebErrorLogger");

        public ActionResult General(Exception exception)
        {
            logger.Error(exception.ToString());

            //Log.Error(exception.StackTrace);

            //if (exception.InnerException != null)
            //{
            //    Log.Error(exception.InnerException.Message);
            //}

            return Content("General failure", "text/plain");
        }

        public ActionResult PageNotFound()
        {
            return Content("Not found", "text/plain");
        }

        public ActionResult Http404()
        {
            return Content("Not found", "text/plain");
        }

        public ActionResult Http403()
        {
            return Content("Forbidden", "text/plain");
        }
    }
}