using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;


namespace Nomina.WS
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }
        private const string _WebApiPrefix = "api";
        private static string _WebApiExecutionPath = String.Format("~/{0}", _WebApiPrefix);
        private static bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(_WebApiExecutionPath);
        }
        private void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("{resource}.axd/{*pathInfo}");
            RouteValueDictionary RouteDefault = new RouteValueDictionary();
            routes.MapHttpRoute(name: "CustomApi", routeTemplate: "api/{controller}/{action}/{value}", defaults: new { action = "get", value = RouteParameter.Optional });
            routes.MapHttpRoute(name: "CustomApiDoble", routeTemplate: "api/{controller}/{action}/{value}/{value2}", defaults: new { action = "get", value = RouteParameter.Optional });
            routes.MapHttpRoute(name: "CustomApiTriple", routeTemplate: "api/{controller}/{action}/{value}/{value2}/{value3}", defaults: new { action = "get", value = RouteParameter.Optional });

        }
        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}