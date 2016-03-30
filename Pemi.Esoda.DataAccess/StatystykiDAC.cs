using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Collections.ObjectModel;

namespace Pemi.Esoda.DataAccess
{
	public class StatystykiDAC
	{
		public List<StatystykaDTO> ListaStatystyk()
		{
			Database db = DatabaseFactory.CreateDatabase();
			List<StatystykaDTO> lista = new List<StatystykaDTO>();

			using (IDataReader dr = db.ExecuteReader("Statystyki.listaStatystyk"))
			{
				while (dr.Read())
				{
					SqlDataReader sdr = dr as SqlDataReader;
					if (sdr == null) throw new Exception("Nie uda³o siê pobraæ metadanych statystyki");
					SqlXml metadane = sdr.GetSqlXml(3);
					string xslt = sdr.IsDBNull(4)?string.Empty:sdr.GetSqlXml(4).Value;
					List<ParametrStatystyki> parametry = new List<ParametrStatystyki>();
					XmlReader xr=metadane.CreateReader();
					XPathDocument xpd = new XPathDocument(xr);
					XPathNavigator xpn = xpd.CreateNavigator();
					string procedura = xpn.SelectSingleNode("/statystyka/procedura").Value;
					XPathNodeIterator xpni = xpn.Select("/statystyka/parametry/parametr");
					while (xpni.MoveNext())
					{
                        parametry.Add(new DTO.ParametrStatystyki(xpni.Current.GetAttribute("etykieta", ""), xpni.Current.GetAttribute("typ", ""), xpni.Current.GetAttribute("wymagany", "").ToLower() == "tak" ? true : false, xpni.Current.GetAttribute("domyslnie", ""), xpni.Current.GetAttribute("zrodloDanych", "")));
					}
					xr.Close();
					lista.Add(new StatystykaDTO(dr.GetInt32(0),procedura, dr.GetString(1),dr.IsDBNull(2)?"":dr.GetString(2),parametry,xslt==null?null: xslt));
				}
			}
			return lista;
		}

		public DataSet WywolanieStatystyki(string nazwaProcedury, params object[] parametry)
		{
			Database db = DatabaseFactory.CreateDatabase();
			return db.ExecuteDataSet(nazwaProcedury, parametry);
		}

		public XmlReader WywolanieStatystykiXml(string nazwaProcedury,params object[] parametry){
			SqlDatabase db = (SqlDatabase)DatabaseFactory.CreateDatabase();
			DbCommand cmd=db.GetStoredProcCommand(nazwaProcedury, parametry);
			return CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
		}

        public Collection<SimpleLookupDTO> ListaStatusowSpraw(string datasource)
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader(datasource))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(int.Parse(idr["Value"].ToString()), idr["Text"].ToString()));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public IDataReader DokumentyISprawy(int idWydzialu, int? idPracownika, DateTime? dataOd, DateTime? dataDo)
        {
            Database db = DatabaseFactory.CreateDatabase();
            return db.ExecuteReader("[Statystyki].[rptDokumentyIsprawy]", idWydzialu, idPracownika, dataOd, dataDo);
        }

	}
}
