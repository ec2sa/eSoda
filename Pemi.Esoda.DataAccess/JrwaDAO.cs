using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Pemi.Esoda.DataAccess
{
    public class JrwaDAO
    {
        public string GetJRWAListXML()
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("[Sprawy].[listaJRWAjakoXml]");
            Trace.Write("Pobieranie JRWA");
            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[listaJRWAjakoXml]", null, false);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
            if (xr.Read())
              return xr.ReadOuterXml();
            return string.Empty;
        }

        public string GetActiveJRWAListXml()
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("[Sprawy].[listaJRWAjakoXml]");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[listaJRWAjakoXml]", null, true);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
            if (xr.Read())
                return xr.ReadOuterXml();
            return string.Empty; 
        }      

        public string GetFullJrwaList()
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("[Sprawy].[listaJRWA]");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[listaJRWA]");
            DataSet dsJRWA = db.ExecuteDataSet(cmd);

            if (dsJRWA.Tables.Count > 0)
            {
                dsJRWA.DataSetName = "JRWA";
                dsJRWA.Tables[0].TableName = "JRWAItem";

                DataRelation dRel = new DataRelation("parentJRWA", dsJRWA.Tables["JRWAItem"].Columns["id"], dsJRWA.Tables["JRWAItem"].Columns["idRodzica"], true);
                dRel.Nested = true;
                dsJRWA.Relations.Add(dRel);
            }

            return dsJRWA.GetXml();
        }

        public IDataReader GetJRWA(int id)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[pobierzJRWA]", id, false);
            return db.ExecuteReader(cmd);
        }

        public void InsertJRWA(int ?idRodzica, string symbol, string nazwa, string katAKM, string katAIK, string uwagi, bool bAktywna)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("[Sprawy].[dodajJRWA]");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[dodajJRWA]", idRodzica, symbol, nazwa, katAKM, katAIK, uwagi, bAktywna);
            db.ExecuteNonQuery(cmd);
        }

        public bool ExistsJRWA(string symbol, int jrwaId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("[Sprawy].[getJRWABySymbol]");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[getJRWABySymbol]", symbol, jrwaId);
            using (DbDataReader dr = db.ExecuteReader(cmd) as DbDataReader)
            {
                return (dr.HasRows) ? dr.Read() : false;
            }            
        }

        public bool IsActiveJRWA(string symbol, int jrwaId)
        {
            if (ExistsJRWA(symbol, jrwaId))
            {
                using (DbDataReader dr = (DbDataReader)GetJRWA(jrwaId))
                {
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            return bool.Parse(dr["aktywny"].ToString());
                        }
                    }
                }
                return false;
            }
            else
                return false;
        }

        public void UpdateJRWA(int id, int ?idRodzica, string symbol, string nazwa, string katAKM, string katAIK, string uwagi, bool bAktywna)
        {            
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("[Sprawy].[aktualizujJRWA]");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[aktualizujJRWA]", id, idRodzica, symbol, nazwa, katAKM, katAIK, uwagi, bAktywna);
            db.ExecuteNonQuery(cmd);
        }

        public string ExportJRWA()
        {            
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Eksport JRWA");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[eksportJRWA]");
            SqlDataReader dr = (SqlDataReader)db.ExecuteReader(cmd);
            //string export = "";

            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);
            
            xw.WriteStartElement("JRWATree");
                        
            if (dr.HasRows)
            {
                while (dr.Read()) // if
                {
                    xw.WriteStartElement("Sprawy.JRWA");

                    if (dr["id"] != null)
                        xw.WriteAttributeString("id", dr["id"].ToString());
                    if (dr["idRodzica"] != null)
                        xw.WriteAttributeString("idRodzica", dr["idRodzica"].ToString());
                    if (dr["symbol"] != null)
                        xw.WriteAttributeString("symbol", dr["symbol"].ToString());
                    if (dr["nazwa"] != null)
                        xw.WriteAttributeString("nazwa", dr["nazwa"].ToString());
                    if (dr["kategoriaAKM"] != null)
                        xw.WriteAttributeString("kategoriaAKM", dr["kategoriaAKM"].ToString());
                    if (dr["kategoriaAIK"] != null)
                        xw.WriteAttributeString("kategoriaAIK", dr["kategoriaAIK"].ToString());
                    if (dr["uwagi"] != null)
                        xw.WriteAttributeString("uwagi", dr["uwagi"].ToString());
                    if (dr["aktywny"] != null)
                        xw.WriteAttributeString("aktywny", dr["aktywny"].ToString());

                    xw.WriteEndElement();
                    //export = dr.GetString(0);
                }
            }
            xw.WriteEndElement();
            xw.Flush();
            xw.Close();
            dr.Close();
            return sb.ToString().Substring(sb.ToString().IndexOf('>') + 1); // export; // sb.ToString(); // export;
        }

        public void ImportJRWA(string xmlData)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Import JRWA");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[importJRWA]", xmlData);
            db.ExecuteNonQuery(cmd);
        }

        public string GetJRWASymbolById(int jrwaId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("[Sprawy].[getJRWASymbolById]");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[getJRWASymbolById]", jrwaId);
            object obj = db.ExecuteScalar(cmd);
            return (obj != null) ? obj.ToString() : string.Empty;
        }

        public int GetJRWAIdBySymbol(string symbol)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("[Sprawy].[getJRWAIdBySymbol]");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[getJRWAIdBySymbol]", symbol);
            object obj = db.ExecuteScalar(cmd);
            return (obj != null) ? int.Parse(obj.ToString()) : -1;
        }
    }
}