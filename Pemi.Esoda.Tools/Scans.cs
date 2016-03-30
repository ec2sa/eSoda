using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Pemi.Esoda.DataAccess;
using System.Data;

namespace Pemi.Esoda.Tools
{
    [DataObject]
    public class Scans
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataView GetScansList(string urzadzenie, string lokalizacja, string rodzajDokumentu, string zrodloDokumentu)
        {
            DataView dvSkany = null;
            SkanyDataset dsSkany = (new ScansDAO()).GetAvailableScansList(Configuration.DocumentsDirectory, Configuration.PhysicalTemporaryDirectory);
            if (dsSkany != null)
            {
                List<SkanyDataset.SkanyRow> ids = new List<SkanyDataset.SkanyRow>();
                foreach (SkanyDataset.SkanyRow row in dsSkany.Skany.Rows)
                {
                    if (urzadzenie.Replace("*", "") != "" && row.urzadzenie != urzadzenie)
                        ids.Add(row);
                    if (lokalizacja.Replace("*", "") != "" && row.lokalizacja != lokalizacja)
                        ids.Add(row);
                    if (rodzajDokumentu.Replace("*", "") != "" && row.rodzaj != rodzajDokumentu)
                        ids.Add(row);
                    if (zrodloDokumentu.Replace("*", "") != "" && row.zrodlo != zrodloDokumentu)
                        ids.Add(row);
                }

                List<SkanyDataset.SkanyRow> distinct = new List<SkanyDataset.SkanyRow>();
                
                foreach (var guid in ids)
                {
                    if (!distinct.Contains(guid))
                        distinct.Add(guid);
                }
                foreach (var row in distinct)
                {
                    dsSkany.Skany.RemoveSkanyRow(row);
                }


                dvSkany = new DataView(dsSkany.Skany);
                dvSkany.Sort = "dataPobrania desc";
                
            }

            return dvSkany;
        } 
    }
}
