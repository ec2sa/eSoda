using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Collections.Generic;
using Pemi.Esoda.DTO;
using System.Web.Hosting;
using System.Reflection;

namespace Pemi.Esoda.Web.UI
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new AssemblyResourceProvider());          

            AssemblyResourceProvider providerInstance = new AssemblyResourceProvider();
            HostingEnvironment hostingEnvironmentInstance = (HostingEnvironment)typeof(HostingEnvironment).InvokeMember("_theHostingEnvironment", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null);
            if (hostingEnvironmentInstance == null)
                return;
            MethodInfo mi = typeof(HostingEnvironment).GetMethod("RegisterVirtualPathProviderInternal", BindingFlags.NonPublic | BindingFlags.Static);
            if (mi == null)
                return;

            mi.Invoke(hostingEnvironmentInstance, new object[] { (VirtualPathProvider)providerInstance });

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

        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();

        //    Server.ClearError();

        //}

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
