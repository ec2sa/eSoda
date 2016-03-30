using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Pemi.Esoda.DataAccess
{
	public class ItemStorageFactory
	{
		public static IItemStorage Create()
		{
            string katalog = System.Web.Configuration.WebConfigurationManager.AppSettings["katalogDokumentow"];
            if(katalog!=null && Directory.Exists(katalog))
			    return new FileSystemItemStorage(katalog);
            throw new FormatException("Niepoprawny lub nieokre�lony katalog dokument�w");
            
		}
	}
}
