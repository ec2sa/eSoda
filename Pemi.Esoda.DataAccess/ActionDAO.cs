using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Pemi.Esoda.DataAccess
{
    [DataObject]
    public class ActionDAO
    {
        public XmlReader GetAvailableActions(int objectId, Guid userId, ActionMask actionTypeMask, ActionType actionType)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            char[] symbols ={ 'L', 'I', 'S' };

            DbCommand cmd = db.GetStoredProcCommand("Akcje.pobierzDostepneAkcje", objectId, userId, actionTypeMask, symbols[(int)actionType]);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
            return xr; // zamykany
        }

        public int WriteAction(Guid actionType, Guid userId, string actionContent, string actionDescription)
        {
            Database db = DatabaseFactory.CreateDatabase();
            int actionID = (int)db.ExecuteScalar("Akcje.odnotujWykonanie", actionType, userId, actionContent, actionDescription);
            return actionID;
        }

        public void RedirectDocument(string actionId, int documentId, Guid userId, string userName, string userFullname, bool workOnPaper,
          string note, int organizationalUnitId, int employeeId, string ouName, string empName)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (actionId == "450423F0-4819-4BE3-8BF4-E42C17815B59")
            {
                db.ExecuteNonQuery("Akcje.przekazanieDokumentu", documentId, userId, organizationalUnitId, employeeId);
                List<string> p = new List<string>();
                p.Add(ouName);
                p.Add(empName);
                ActionContext ctx = new ActionContext(new Guid("450423F0-4819-4BE3-8BF4-E42C17815B59"), userId, userName, userFullname, p);
                ActionLogger al = new ActionLogger(ctx);
                al.AppliesToDocuments.Add(documentId);
                al.ActionData.Add("notatka", note);
                al.ActionData.Add("wydzial", ouName);
                al.ActionData.Add("pracownik", empName);
                al.Execute();
            }
            if (actionId == "10672122-E570-4A04-9D5D-A6667C27FE3D")
            {
                db.ExecuteNonQuery("Akcje.dekretacjaDokumentu", documentId, userId, organizationalUnitId, employeeId);
                List<string> p = new List<string>();
                p.Add(ouName);
                p.Add(empName);
                ActionContext ctx = new ActionContext(new Guid("10672122-E570-4A04-9D5D-A6667C27FE3D"), userId, userName, userFullname, p);
                ActionLogger al = new ActionLogger(ctx);
                al.AppliesToDocuments.Add(documentId);
                al.ActionData.Add("wersjaPapierowa", workOnPaper ? "tak" : "nie");
                al.ActionData.Add("notatka", note);
                al.ActionData.Add("wydzial", ouName);
                al.ActionData.Add("pracownik", empName);
                al.Execute();
            }
        }

        public XmlReader GetActionDefinition(Guid definitionId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Akcje.PobierzDefinicje", definitionId);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
            return xr; // zamykany
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DataView GetDocCaseFullList(string sortParam, string filterParam)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Akcje.listaDokumentowISpraw");
            cmd.CommandTimeout = 600;
            DataSet dsDocCaseList = db.ExecuteDataSet(cmd);
            DataView dvDocCaseList = null;
            if (dsDocCaseList.Tables.Count > 0)
            {
                dvDocCaseList = new DataView(dsDocCaseList.Tables[0]);
                dvDocCaseList.Sort = sortParam;
                dvDocCaseList.RowFilter = filterParam;
            }
            return dvDocCaseList;
        }

        public string[] GetDataAndXslt(int actionID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string[] rv = null;
            using (IDataReader r = db.ExecuteReader("[Akcje].[PobierzDaneIXsltAkcji]", actionID))
            {
                rv = new string[2];
                if (r.Read())
                {
                    rv[0] = r.IsDBNull(0) ? string.Empty : r.GetString(0);
                    rv[1] = r.IsDBNull(1) ? string.Empty : r.GetString(1);
                }
               
            } 
            return rv;
        }

        public string GetXslt(Guid actionID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string rv = null;
            using (IDataReader r = db.ExecuteReader("[Akcje].[PobierzXsltAkcji]", actionID))
            {
                 rv = string.Empty;
                if (r.Read())
                {
                    rv = r.IsDBNull(0) ? string.Empty : r.GetString(0);
                }
               
            } 
            return rv;
        }


        public IDataReader GetDocToReceive(Guid userId, bool doPracownika)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Akcje.listaDokumentowDoOdbioru", userId, doPracownika);
            return db.ExecuteReader(cmd);
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataView GetAwaitingTasks(Guid userId, string sortParam, string filterParam,bool doWiadomosci)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("[Akcje].[listaOczekujacychZadan]", userId,doWiadomosci);
            cmd.CommandTimeout = 600;
            DataSet dsTasks = db.ExecuteDataSet(cmd);
            DataView dvTasks = null;
            if (dsTasks.Tables.Count > 0)
            {
                dvTasks = new DataView(dsTasks.Tables[0]);
                dvTasks.Sort = sortParam;
                dvTasks.RowFilter = filterParam;
            }
            return dvTasks;
        }
    }

    public enum ActionType
    {
        CalledFromList,
        CalledFromView,
        CalledBySystem
    }
    public enum ActionMask
    {
        Document = 1,
        Case = 2,
        File = 4,
        RegistryItem = 8,
        User = 16
    }
}