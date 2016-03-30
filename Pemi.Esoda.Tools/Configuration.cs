using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

namespace Pemi.Esoda.Tools
{
	public class Configuration
	{
		public static string VirtualTemporaryDirectory
		{
			get
			{
				return System.Web.Configuration.WebConfigurationManager.AppSettings["katalogRoboczy"];
			}
		}

        public static string DocumentsDirectory
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["katalogDokumentow"];
            }
        }
	
		public static string PhysicalTemporaryDirectory
		{
			get
			{
				return HttpContext.Current.Server.MapPath(Configuration.VirtualTemporaryDirectory);
			}
		}

		public static string ScannersConfigurationFile
		{
			get
			{
				return System.Web.Configuration.WebConfigurationManager.AppSettings["plikKonfiguracyjnySkanerow"];
			}
		}
	}
}
