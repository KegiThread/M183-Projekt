using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Role_Based_Authorization
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string[] Roles;
        public static Dictionary<string, object> UserRoles;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Do Proper Role Handling here using Entity etc.
            // Store elements Temporarily in the Application Context
            
            Roles = new string[] { "Administrator", "User" };
            
            UserRoles = new Dictionary<string, object>();
            UserRoles.Add("admin", "Administrator");
            UserRoles.Add("user", "User");
            
        }
    }
}
