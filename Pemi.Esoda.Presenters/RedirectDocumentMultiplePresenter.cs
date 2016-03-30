using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.DTO;
namespace Pemi.Esoda.Presenters
{
    public class RedirectDocumentMultiplePresenter : BasePresenter
    {
        private IRedirectDocumentMultipleView view;
        private IEditRegistryItemTask service;
        private ISessionProvider session;

        private IList<RedirectItem> itemList = new List<RedirectItem>();
        private IList<RedirectItem> ItemList
        {
            get
            {
                if (session["RedirectListItem"] != null)
                    itemList = (List<RedirectItem>)session["RedirectListItem"];
                return itemList;

            }
            set
            {
                session["RedirectListItem"] = value;
            }
        }
        private int documentID;
        private string actionID;

        //private bool isCopy; // czy dokument jest kopia
        private DokumentDetails dd;

        public RedirectDocumentMultiplePresenter(IRedirectDocumentMultipleView view, ISessionProvider session)
        {
            this.view = view;
            this.service = new EditRegistryItemTask();
            this.session = session;
            subscribeToEvents();
        }

        public override void Initialize()
        {
            session.Remove("RedirectListItem");
            view.OrganizationalUnits = service.GetOrganizationalUnits();
            view.Employees = service.GetEmployees(view.OrganizationalUnitId);
            view.RedirectList = itemList;
        }

        public void OnViewLoaded()
        {
            actionID = session["idAkcji"] != null ? session["idAkcji"].ToString() : null;
            
            //if (session["idDokumentu"] != null) 
            if(view.DocumentId >0 && !string.IsNullOrEmpty(actionID))
            {
                documentID = view.DocumentId; // int.Parse(session["idDokumentu"].ToString());

                IRedirectDocumentTask srv = new RedirectDocumentTask();
                //dd = srv.GetDokumentDetails(int.Parse(session["idDokumentu"].ToString()));
                dd = srv.GetDocumentDetails(documentID);
                view.Notice = dd.Notice;
                view.Description = dd.Description;

                if (dd.IsCopy)
                {
                    view.WorkOnPaperEnable = false;
                }

                //dokument (papierowy lub kopia) nie wychodzi ze sprawy
                if (dd.IsInCase)
                {
                    view.WorkOnPaperEnable = false;
                }

                if (session["RedirectListItem"] != null)
                    itemList = (List<RedirectItem>)session["RedirectListItem"];
            }
            else
            {
                view.ReturnTo = "~/Logon.aspx";
            }

            //isCopy = srv.IsCopy();

            //if (isCopy)
            //{
            //    view.WorkOnPaperEnable = false;
            //}
        }

        protected override void redirectToPreviousView()
        {

        }

        protected override void subscribeToEvents()
        {
            view.ActionExecuted += new EventHandler(view_ActionExecuted);
            view.OrganizationalUnitChanged += new EventHandler(view_OrganizationalUnitChanged);
            view.AddToRedirectList += new EventHandler(view_AddToRedirectList);
        }

        void view_AddToRedirectList(object sender, EventArgs e)
        {
            try
            {
                if (view.OrganizationalUnitId != 0)
                {
                    if (view.SelectedItemID == -1)
                    {
                        SaveItem();
                    }
                    else
                    {
                        EditItem();
                    }
                }
                else
                {
                    view.Message = "Proszê wybraæ wydzia³!";
                }
            }
            catch (Exception ex)
            {
                view.Message = ex.Message;
            }

            #region todel
            //bool isDuplicate = false;

            //try
            //{
            //    //if (session["RedirectListItem"] != null)
            //    //    itemList = (List<RedirectItem>)session["RedirectListItem"];

            //    if (view.SelectedItemID == -1) //save new
            //    {
            //        //sprawdzenie czy juz taki pracownik i wydzial nie zostal wczesniej dodany do listy
            //        for (int i = 0; i < itemList.Count; i++)
            //        {
            //            if (((RedirectItem)itemList[i]).EmployeeID == view.EmployeeId && ((RedirectItem)itemList[i]).OrganizationalUnitID == view.OrganizationalUnitId)
            //            {
            //                isDuplicate = true;
            //                break;
            //            }
            //        }

            //        if (!isDuplicate)
            //        {
            //            itemList.Add(new RedirectItem(
            //                //int.Parse(session["idDokumentu"].ToString()),
            //                documentID,
            //                view.OrganizationalUnitId,
            //                view.OUName,
            //                view.EmployeeId,
            //                view.EmpName,
            //                view.Note,
            //                view.WorkOnPaper,
            //                view.CommandID,
            //                view.AllHistory,
            //                view.AllScans
            //                ));

            //            if (view.WorkOnPaper)
            //            {
            //                RedirectItem currentItem = null;
            //                for (int j = 0; j < itemList.Count; j++)
            //                {
            //                    if (j != itemList.Count - 1)
            //                    {
            //                        currentItem = (RedirectItem)itemList[j];
            //                        currentItem.WorkOnPaper = false;
            //                        currentItem.AllHistory = false;
            //                        currentItem.AllScans = false;
            //                    }
            //                }
            //            }

            //            session["RedirectListItem"] = itemList;
            //            ClearForm();
            //        }
            //        else
            //        {
            //            view.Message = "Wybrany wydzia³ i pracownik widnieje ju¿ na liœcie.";
            //        }
            //    }
            //    else //edit
            //    {
            //        RedirectItem item = itemList[view.SelectedItemID];

            //        //sprawdzenie czy juz taki pracownik i wydzial nie zostal wczesniej dodany do listy
            //        for (int i = 0; i < itemList.Count; i++)
            //        {
            //            if ((view.SelectedItemID != i) && (((RedirectItem)itemList[i]).EmployeeID == view.EmployeeId && ((RedirectItem)itemList[i]).OrganizationalUnitID == view.OrganizationalUnitId))
            //            {
            //                isDuplicate = true;
            //                break;
            //            }
            //        }

            //        if (!isDuplicate)
            //        {
            //            item.Note = view.Note;
            //            item.AllHistory = view.AllHistory;
            //            item.AllScans = view.AllScans;
            //            item.CommandID = view.CommandID;
            //            item.WorkOnPaper = view.WorkOnPaper;
            //            item.OrganizationalUnitID = view.OrganizationalUnitId;
            //            item.EmployeeID = view.EmployeeId;
            //            item.EmployeeName = view.EmpName;
            //            item.OrganizationalUnitName = view.OUName;

            //            if (item.WorkOnPaper)
            //            {
            //                for (int j = 0; j < itemList.Count; j++)
            //                {
            //                    if (j != view.SelectedItemID)
            //                    {
            //                        ((RedirectItem)itemList[j]).WorkOnPaper = false;
            //                        ((RedirectItem)itemList[j]).AllHistory = false;
            //                        ((RedirectItem)itemList[j]).AllScans = false;
            //                    }
            //                }
            //            }

            //            view.SelectedItemID = -1;
            //            view.AddToRedirectButtonName = "Dodaj do dekretacji";
            //            //dodane 1
            //            view.ShowCancelChangesButton = false;
            //            ClearForm();

            //            session["RedirectListItem"] = itemList;
            //        }
            //        else
            //        {
            //            view.Message = "Wybrany wydzia³ i pracownik widnieje ju¿ na liœcie.";
            //            view.AddToRedirectButtonName = "Zapisz zmiany";
            //            //dodane 1
            //            view.ShowCancelChangesButton = true;
            //        }
            //    }
            //    // zakomentowane 1
            //    //view.ShowCancelChangesButton = false;
            //}
            //catch (Exception ex)
            //{
            //    view.Message = ex.Message;
            //}
            #endregion

        }

        void view_OrganizationalUnitChanged(object sender, EventArgs e)
        {
            view.Employees = service.GetEmployees(view.OrganizationalUnitId);
        }

        void view_ActionExecuted(object sender, EventArgs e)
        {
            if (ItemList != null && ItemList.Count > 0)
            {
                if (((IsWorkOnPaper && !dd.IsCopy) || dd.IsCopy) || dd.IsInCase)
                {
                    IRedirectDocumentTask srv = new RedirectDocumentTask();
                    //srv.RedirectDocument(session["idAkcji"].ToString(), int.Parse(session["idDokumentu"].ToString()), view.UserId, view.UserName, view.UserFullName, view.WorkOnPaper, view.Note, view.OrganizationalUnitId, view.EmployeeId, view.OUName, view.EmpName);
                    //srv.RedirectDocumentMultiple(session["idAkcji"].ToString(), int.Parse(session["idDokumentu"].ToString()), view.UserId, view.UserName, view.UserFullName, view.WorkOnPaper, view.Note, view.OrganizationalUnitId, view.EmployeeId, view.OUName, view.EmpName, ItemList);
                    srv.RedirectDocumentMultiple(actionID, documentID, view.UserId, view.UserName, view.UserFullName, view.WorkOnPaper, view.Note, view.OrganizationalUnitId, view.EmployeeId, view.OUName, view.EmpName, ItemList);
                    //if (IsDocVisibleForUser(view.UserId, int.Parse(session["idDokumentu"].ToString())))
                    if (IsDocVisibleForUser(view.UserId, documentID))
                        view.ReturnTo = "~/Dokumenty/HistoriaDokumentu.aspx?id="+view.DocumentId.ToString();
                    else
                        view.ReturnTo = "~/OczekujaceZadania.aspx";
                }
                else
                {
                    view.Message = "Nie zaznaczono opcji [Praca na papierze (oryginale)]. Proszê wybraæ osobê, która powinna otrzymaæ orygina³.";
                }
            }
            else
            {
                view.Message = "Proszê wybraæ osoby do dekretacji!";
            }
        }

        public void GetRedirectList()
        {
            //if (session["RedirectListItem"] != null)
            //    itemList = (List<RedirectItem>)session["RedirectListItem"];

            view.RedirectList = itemList;
        }

        public void OnSelectedItemIDChanged()
        {
            try
            {
                GetSelectedItem(view.SelectedItemID);
                view.ShowCancelChangesButton = true;
                view.AddToRedirectButtonName = "Zapisz zmiany";
            }
            catch (Exception e)
            {
                view.Message = e.Message;
            }
        }

        public void OnDeleteSelectedItem()
        {
            try
            {
                bool stat = DeleteSelectedItem(view.SelectedItemID);
                view.AddToRedirectButtonName = "Dodaj do dekretacji";
                view.ShowCancelChangesButton = false;
                if (stat == false)
                {
                    view.Message = "Dekretacja tylko do edycji!";
                }
                ClearForm();
            }
            catch (Exception e)
            {
                view.Message = e.Message;
            }
        }

        public void OnCancelChangeSelectedItem()
        {
            view.AddToRedirectButtonName = "Dodaj do dekretacji";
            view.ShowCancelChangesButton = false;
            ClearForm();
        }

        private bool DeleteSelectedItem(int selectedItemID)
        {
            bool state = false;

            //if (session["RedirectListItem"] != null)
            //    itemList = (List<RedirectItem>)session["RedirectListItem"];

            RedirectItem i = itemList[selectedItemID];

            if (!i.WorkOnPaper)
            {
                itemList.RemoveAt(selectedItemID);
                session["RedirectListItem"] = itemList;
                state = true;
                ClearForm();
            }
            view.SelectedItemID = -1;
            return state;
        }

        private void GetSelectedItem(int selectedItemID)
        {
            //if (session["RedirectListItem"] != null)
            //    itemList = (List<RedirectItem>)session["RedirectListItem"];

            RedirectItem i = itemList[selectedItemID];

            view.AllHistory = i.AllHistory;
            view.AllScans = i.AllScans;
            view.CommandID = i.CommandID;
            view.OrganizationalUnitId = i.OrganizationalUnitID;
            view.Employees = service.GetEmployees(view.OrganizationalUnitId);
            view.EmployeeId = i.EmployeeID;
            view.WorkOnPaper = i.WorkOnPaper;
            view.Note = i.Note;

            if (i.WorkOnPaper)
            {
                view.AllHistoryEnable = false;
                view.AllScanEnable = false;
            }
            else
            {
                view.AllHistoryEnable = true;
                view.AllScanEnable = true;
            }
        }

        private void ClearForm()
        {
            view.AllHistory = false;
            view.AllScans = false;
            view.AllHistoryEnable = true;
            view.AllScanEnable = true;
            view.CommandID = false;
            view.Note = string.Empty;
            view.OrganizationalUnitId = 0;//1
            view.Employees = service.GetEmployees(view.OrganizationalUnitId);
            view.WorkOnPaper = false;
        }

        private bool IsWorkOnPaper
        {
            get
            {
                bool stat = false;

                //if (session["RedirectListItem"] != null)
                //    itemList = (List<RedirectItem>)session["RedirectListItem"];

                if (itemList != null)
                {
                    foreach (RedirectItem i in itemList)
                    {
                        if (i.WorkOnPaper == true)
                        {
                            stat = true;
                            break;
                        }
                    }
                }
                return stat;
            }
        }

        private bool IsDocVisibleForUser(Guid userId, int docId)
        {
            return (new DocumentDAO()).IsDocVisibleForUser(docId, userId);
        }

        private void SaveItem()
        {
            bool isDuplicate = false;


            //sprawdzenie czy juz taki pracownik i wydzial nie zostal wczesniej dodany do listy
            for (int i = 0; i < itemList.Count; i++)
            {
                if (((RedirectItem)itemList[i]).EmployeeID == view.EmployeeId && ((RedirectItem)itemList[i]).OrganizationalUnitID == view.OrganizationalUnitId)
                {
                    isDuplicate = true;
                    break;
                }
            }

            if (!isDuplicate)
            {
                itemList.Add(new RedirectItem(
                    //int.Parse(session["idDokumentu"].ToString()),
                    documentID,
                    view.OrganizationalUnitId,
                    view.OUName,
                    view.EmployeeId,
                    view.EmpName,
                    view.Note,
                    view.WorkOnPaper,
                    view.CommandID,
                    view.AllHistory,
                    view.AllScans
                    ));

                if (view.WorkOnPaper)
                {
                    RedirectItem currentItem = null;
                    for (int j = 0; j < itemList.Count; j++)
                    {
                        if (j != itemList.Count - 1)
                        {
                            currentItem = (RedirectItem)itemList[j];
                            currentItem.WorkOnPaper = false;
                            currentItem.AllHistory = false;
                            currentItem.AllScans = false;
                        }
                    }
                }

                session["RedirectListItem"] = itemList;
                ClearForm();
            }
            else
            {
                view.Message = "Wybrany wydzia³ i pracownik widnieje ju¿ na liœcie.";
            }

        }

        private void EditItem()
        {
            bool isDuplicate = false;

            RedirectItem item = itemList[view.SelectedItemID];

            //sprawdzenie czy juz taki pracownik i wydzial nie zostal wczesniej dodany do listy
            for (int i = 0; i < itemList.Count; i++)
            {
                if ((view.SelectedItemID != i) && (((RedirectItem)itemList[i]).EmployeeID == view.EmployeeId && ((RedirectItem)itemList[i]).OrganizationalUnitID == view.OrganizationalUnitId))
                {
                    isDuplicate = true;
                    break;
                }
            }

            if (!isDuplicate)
            {
                item.Note = view.Note;
                item.AllHistory = view.AllHistory;
                item.AllScans = view.AllScans;
                item.CommandID = view.CommandID;
                item.WorkOnPaper = view.WorkOnPaper;
                item.OrganizationalUnitID = view.OrganizationalUnitId;
                item.EmployeeID = view.EmployeeId;
                item.EmployeeName = view.EmpName;
                item.OrganizationalUnitName = view.OUName;

                if (item.WorkOnPaper)
                {
                    for (int j = 0; j < itemList.Count; j++)
                    {
                        if (j != view.SelectedItemID)
                        {
                            ((RedirectItem)itemList[j]).WorkOnPaper = false;
                            ((RedirectItem)itemList[j]).AllHistory = false;
                            ((RedirectItem)itemList[j]).AllScans = false;
                        }
                    }
                }

                view.SelectedItemID = -1;
                view.AddToRedirectButtonName = "Dodaj do dekretacji";
                //dodane 1
                view.ShowCancelChangesButton = false;
                ClearForm();

                session["RedirectListItem"] = itemList;
            }
            else
            {
                view.Message = "Wybrany wydzia³ i pracownik widnieje ju¿ na liœcie.";
                view.AddToRedirectButtonName = "Zapisz zmiany";
                //dodane 1
                view.ShowCancelChangesButton = true;
            }
        }
    }
}


