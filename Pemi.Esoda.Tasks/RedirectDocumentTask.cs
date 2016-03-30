using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.DTO;
using System.Collections;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;

namespace Pemi.Esoda.Tasks
{
    public class RedirectDocumentTask : IRedirectDocumentTask
    {


        #region IRedirectDocumentTask Members

        void IRedirectDocumentTask.RedirectDocument(string actionId, int documentId, Guid userId, string userName, string userFullname, bool workOnPaper, string note, int organizationalUnitId, int employeeId, string ouName, string empName)
        {
            new ActionDAO().RedirectDocument(actionId, documentId, userId, userName, userFullname, workOnPaper, note, organizationalUnitId, employeeId, ouName, empName);
        }

        ArrayList IRedirectDocumentTask.RedirectDocumentMultiple(string actionId, int documentId, Guid userId, string userName, string userFullname, bool workOnPaper, string note, int organizationalUnitId, int employeeId, string ouName, string empName, IList<RedirectItem> redirectList)
        {
            if (redirectList.Count == 0)
                throw new Exception("Lista do dekretacji jest pusta");

            StringBuilder sb = new StringBuilder();

            bool paper = false;
            // sprawdza, czy jest opcja pracy na papierze
            foreach (RedirectItem ri in redirectList)
            {
                sb.AppendLine(string.Format("[{0}/{1}] ", ri.OrganizationalUnitName, ri.EmployeeName));
                if (ri.WorkOnPaper && !paper)
                {
                    paper = ri.WorkOnPaper;                   
                }                
            }

            // je¿eli nie, papier dostaje pierwzy z listy
            if (!paper && redirectList.Count>0)
                redirectList[0].WorkOnPaper = true;

            Database db = DatabaseFactory.CreateDatabase();
            ArrayList failList = new ArrayList();
            ArrayList okList = new ArrayList();
 
            using (DbConnection connection = db.CreateConnection())
            {
                //TODO:zamknac polaczenie
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

                List<string> parList = new List<string>();
                parList.Add(ouName);
                parList.Add(empName);
                ActionContext ac = new ActionContext(new Guid("10672122-E570-4A04-9D5D-A6667C27FE3D"), userId, userName, userFullname, parList);
                ActionLogger al = new ActionLogger(ac);

                al.AppliesToDocuments.Add(documentId);

                int iDocInCase = (new DocumentDAO()).IsDocumentInAnyCase(documentId);

                // jedzie po liœcie
                foreach (RedirectItem ri in redirectList)
                {
                    try
                    {                        
                        bool isCopy = !ri.WorkOnPaper || (iDocInCase > 0);
                        int docId = (new DocumentDAO()).CopyDocument(db, transaction, documentId, userId, isCopy, ri.OrganizationalUnitID, ri.EmployeeID, ri.AllScans, ri.AllHistory);
                        ri.ItemID = docId;
                        al.AppliesToDocuments.Add(docId);
                    }
                    catch
                    {
                        failList.Add(ri);
                    }
                }
                transaction.Commit();
               
                // odnotowanie akcji
                //al.ActionData.Add("notatka", note);
                al.ActionData.Add("info", "Dekretacja do: "+sb.ToString());
                int actionID = al.Execute();

                if (iDocInCase > 0)
                {
                    try
                    {
                        db.ExecuteNonQuery("Dokumenty.zapiszNotatke", documentId, actionID, string.Format("Dekretacja do: {0}", sb.ToString()));
                    }
                    catch
                    {
                        System.Diagnostics.Debug.Print(actionID + "-" + documentId + "-" + string.Format("Dekretacja do: {0}", sb.ToString()));
                    }
                }

                foreach (RedirectItem ri in redirectList)
                {

                    try
                    {
                        if (ri.CommandID)
                        {
                            db.ExecuteNonQuery("Dokumenty.ustawDoWiadomosci", ri.ItemID);
                        }

                        if (ri.Note.Length > 0)
                        {
                            db.ExecuteNonQuery("Dokumenty.zapiszNotatke", ri.ItemID, actionID, string.Format("Dekretacja do: {0} ({1})", sb.ToString(), ri.Note));
                        }
                        else
                        {
                            db.ExecuteNonQuery("Dokumenty.zapiszNotatke", ri.ItemID, actionID, string.Format("Dekretacja do: {0}", sb.ToString()));
                        }
                    }
                    catch
                    {
                        System.Diagnostics.Debug.Print(actionID + " " + ri.ItemID + " " + ri.Note);
                    }
                    
                }               
            }

            return (failList.Count > 0) ? failList : null;
        }
       
        public bool IsCopy(int documentId)
        {
            DocumentDAO dao = new DocumentDAO();
            return  dao.IsCopy(documentId);
        }
      
        public DokumentDetails GetDocumentDetails(int documentId)
        {
            DocumentDAO dao = new DocumentDAO();
            return dao.GetDocumentDetails(documentId);
        }

        #endregion
    }
}