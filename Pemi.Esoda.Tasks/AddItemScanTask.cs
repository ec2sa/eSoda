using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Core;
using System.IO;
using Pemi.Esoda.Tools;
using System.Xml.XPath;
using Pemi.Esoda.DataAccess.Utils;
using System.ComponentModel;
using System.Data;

namespace Pemi.Esoda.Tasks
{
    [DataObject]
	public class AddItemScanTask:IAddItemScanTask
	{
		private ScansDAO sdao = new ScansDAO();
		private RegistryDAO rdao = new RegistryDAO();

		#region IAddItemScanTask Members

		string IAddItemScanTask.GetAvailableScans(string location, string device, string documentType, string source)
		{
			return sdao.GetAvailableScansMetadata(Configuration.VirtualTemporaryDirectory, Configuration.PhysicalTemporaryDirectory);
		}

       
        SkanyDataset IAddItemScanTask.GetAvailableScansList()
        {
            return sdao.GetAvailableScansList(Configuration.VirtualTemporaryDirectory, Configuration.PhysicalTemporaryDirectory);
        }

        //[DataObjectMethod(DataObjectMethodType.Select, true)]
        //DataView GetScansList()
        //{
        //    SkanyDataset dsSkany = ao.GetAvailableScansList(Configuration.VirtualTemporaryDirectory, Configuration.PhysicalTemporaryDirectory);
        //}


		int IAddItemScanTask.GetIncomingScansCount()
		{
			return 18;
		}

		void IAddItemScanTask.AssignScanWithRegistryItem(string fileName,int ownerID, int registryID, int itemNumber, string documentName,
            string documentDescription, string elementDescription, bool isMain, bool isRF)
		{
				IItemStorage storage = ItemStorageFactory.Create();
				TypMime mimeType = MimeHelper.PobierzTypDlaRozszerzenia(Path.GetExtension(fileName));
				Guid scanGuid = Guid.Empty;
				using (FileStream fs = File.OpenRead(fileName))
				{
					scanGuid = storage.Save(fs);
				}
				XPathDocument xpd = new XPathDocument(File.OpenRead(Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".xml"));
				XPathNavigator xpn = xpd.CreateNavigator();
				Metadata md = new Metadata();
				md.Add("dataPobrania", xpn.SelectSingleNode("/dokument/dataPobrania").Value);
				md.Add("nazwaPlikuSkanu", xpn.SelectSingleNode("/dokument/nazwaPlikuDokumentu").Value);
				md.Add("liczbaStron", xpn.SelectSingleNode("/dokument/liczbaStron").Value);
				md.Add("lokalizacjaSkanu", xpn.SelectSingleNode("/dokument/pochodzenie/lokalizacja").Value);
				md.Add("urzadzenie", xpn.SelectSingleNode("/dokument/pochodzenie/urzadzenie").Value);
				md.Add("rodzajSkanu", xpn.SelectSingleNode("/dokument/pochodzenie/rodzaj").Value);
				md.Add("zrodloSkanu", xpn.SelectSingleNode("/dokument/pochodzenie/zrodlo").Value);

				rdao.AddNewScan(ownerID, registryID, itemNumber, documentName, documentDescription, scanGuid, Path.GetFileName(fileName), elementDescription, mimeType.Nazwa, isMain, mimeType.Browsable, md.GetXml(),isRF);
		}




    public void AssignExistingScanWithRegistryItem(string fileName,  int registryID, int itemNumber, string elementDescription, bool isMain,bool isRF)
    {
      rdao.AddExistingScan( registryID, itemNumber, new Guid(fileName), elementDescription, isMain,isRF);
    }

    #endregion
  }
}
