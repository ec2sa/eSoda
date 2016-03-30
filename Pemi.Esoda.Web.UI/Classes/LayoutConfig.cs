using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pemi.Esoda.DataAccess;
using System.Web.UI;

namespace Pemi.Esoda.Web.UI.Classes
{
    public static class LayoutConfig
    {

        private static string currentLogo1File
        {
            get
            {
                if (HttpContext.Current.Application["logo1"] == null)
                    HttpContext.Current.Application["logo1"] = new EsodaConfigParametersDAO().GetConfig()["logo1"];

                return HttpContext.Current.Application["logo1"].ToString();
            }
        }

        private static string currentLogo2File
        {
            get
            {
                if (HttpContext.Current.Application["logo2"] == null)
                    HttpContext.Current.Application["logo2"] = new EsodaConfigParametersDAO().GetConfig()["logo2"];

                return HttpContext.Current.Application["logo2"].ToString();
            }
        }

        private static string currentBgColor
        {
            get
            {
                if (HttpContext.Current.Application["logoBG"] == null)
                    HttpContext.Current.Application["logoBG"] = new EsodaConfigParametersDAO().GetConfig()["logoBG"];

                return HttpContext.Current.Application["logoBG"].ToString();
            }
        }

        public static string Logo1Url
        {
            get
            {
                if (HttpContext.Current.Handler is Page)
                    return (HttpContext.Current.Handler as Page).ResolveUrl("~/App_Themes/StandardLayout/logos/" + currentLogo1File);
                else
                    return string.Empty;

            }
        }

        public static string Logo2Url
        {
            get
            {
                if (HttpContext.Current.Handler is Page)
                    return (HttpContext.Current.Handler as Page).ResolveUrl("~/App_Themes/StandardLayout/logos/" + currentLogo2File);
                else
                    return string.Empty;

            }
        }

        public static string BGColor { get { return currentBgColor + ';'; } }

        public static void Invalidate()
        {
            HttpContext.Current.Application.Remove("logo1");
            HttpContext.Current.Application.Remove("logo2");
            HttpContext.Current.Application.Remove("logoBG");
        }
    }
}