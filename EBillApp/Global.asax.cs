using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;
namespace EBillApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        /* private static readonly Logger logger = LogManager.GetCurrentClassLogger();
         protected void Application_Error()
         {
             var ex = Server.GetLastError();
             logger.Info(ex.Message.ToString() + Environment.NewLine + DateTime.Now);
             HttpContext.Current.ClearError();
             Response.Redirect("~/Error/LoggerFile", true);
         }*/
    }
}
