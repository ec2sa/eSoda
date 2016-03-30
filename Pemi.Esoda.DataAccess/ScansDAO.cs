using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Pemi.Esoda.Tools;


namespace Pemi.Esoda.DataAccess
{
    public class ScansDAO
    {
        public string GetAvailableScansMetadata(string virtualWorkingDirectory, string physicalWorkingDirectory)
        {
            RegistryDAO rdao = new RegistryDAO();

            string unassignedScans = string.Empty;
            using (XmlReader usr = rdao.GetUnassignedScans())
            {
                if (usr.Read())
                    unassignedScans = usr.ReadInnerXml();
            }

            string[] files = Directory.GetFiles(physicalWorkingDirectory, "*.xml", SearchOption.TopDirectoryOnly);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);
            xw.WriteStartDocument();
            xw.WriteStartElement("dokumenty");
            xw.WriteAttributeString("katalogDokumentow", virtualWorkingDirectory);
            foreach (string fileName in files)
            {
                string xsr = string.Empty;
                /////////////////
                try
                {
                    using (XmlReader xr = XmlTextReader.Create(fileName))
                    {
                        xr.MoveToContent();
                        string text = xr.ReadOuterXml();

                        if (text.Contains("<dokument>"))
                        {
                            using (XmlReader xr2 = XmlTextReader.Create(fileName))
                            {
                                xr2.MoveToContent();
                                xsr = xr2.ReadInnerXml();
                            }

                            xw.WriteStartElement("dokument");
                            xw.WriteRaw(xsr);
                            xw.WriteEndElement();
                        }
                    }
                }
                catch
                {
                    //throw new Exception(string.Format("B³ad pobierania metadanych skanów: {0} Plik: {1} ",xsr, fileName));
                }

                /////////////////
                //XmlReader xr = XmlTextReader.Create(fileName);
                //xr.MoveToContent();
                //xw.WriteNode(xr, true); 
                //xr.Close();
            }
            xw.WriteRaw(unassignedScans);
            xw.WriteEndElement();
            //dokumenty
            xw.WriteEndDocument();
            xw.Close();
            return sb.ToString();
        }

        public SkanyDataset GetAvailableScansList(string documentsDirectory, string physicalWorkingDirectory)
        {
            SkanyDataset dsScans = (new RegistryDAO()).GetUnassignedScansDataSet(documentsDirectory);

            //foreach (SkanyDataset.SkanyRow row in dsScans.Skany.Rows)
            //{
            //    row.katalogPlikuDokumentu = documentsDirectory;
            //    row.nazwaPlikuDokumentu = row.guid.ToString();
            //}
            //dsScans.Skany.AcceptChanges();

            // skany nieprzypisane

            string[] files = Directory.GetFiles(physicalWorkingDirectory, "*.xml", SearchOption.TopDirectoryOnly);

            foreach (string fileName in files)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileName);

                    string dataPobrania = doc.SelectSingleNode("/dokument/dataPobrania").InnerText;
                    string nazwaPliku = doc.SelectSingleNode("/dokument/nazwaPlikuDokumentu").InnerText;
                    string katalogPliku = doc.SelectSingleNode("/dokument/katalogPlikuDokumentu").InnerText;
                    string nazwaSkanu = doc.SelectSingleNode("/dokument/nazwaPlikuSkanu").InnerText;
                    string liczbaStron = doc.SelectSingleNode("/dokument/liczbaStron").InnerText;
                    string miniatura = doc.SelectSingleNode("/dokument/miniatura").InnerText;
                    string pierwszaStrona = doc.SelectSingleNode("/dokument/pierwszaStrona").InnerText;
                    string lokalizacja = doc.SelectSingleNode("/dokument/pochodzenie/lokalizacja").InnerText;
                    string urzadzenie = doc.SelectSingleNode("/dokument/pochodzenie/urzadzenie").InnerText;
                    string rodzaj = doc.SelectSingleNode("/dokument/pochodzenie/rodzaj").InnerText;
                    string zrodlo = doc.SelectSingleNode("/dokument/pochodzenie/zrodlo").InnerText;

                    dsScans.Skany.AddSkanyRow(Guid.Empty, dataPobrania, nazwaPliku, liczbaStron, lokalizacja, urzadzenie, rodzaj, zrodlo, katalogPliku, nazwaSkanu, miniatura, pierwszaStrona);
                }
                catch
                { }
            }
            dsScans.AcceptChanges();

            //////////////////////

            return dsScans;
        }
    }
}
