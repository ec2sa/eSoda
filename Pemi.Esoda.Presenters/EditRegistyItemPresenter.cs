using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using System.Xml.XPath;
using System.IO;
using Pemi.Esoda.DTO;
using Pemi.Esoda.DataAccess;
using System.Data.Common;
using System.Xml;

namespace Pemi.Esoda.Presenters
{
    public class EditRegistyItemPresenter : BasePresenter
    {

      

        protected bool IsInvoice
        {
            get { return session["isRF"] != null || session["isRFEdited"] != null; }
            
        }

        private int itemId
        {
            get
            {
                int tmp;
                if (!session.Contains("itemId")) return 0;
                if (!int.TryParse(session["itemId"].ToString(), out tmp)) return 0;
                return tmp;
            }
        }

        public int ItemID
        {
            get { return this.itemId; }
        }

        private int registryId
        {
            get
            {
                int tmp;
                if (!session.Contains("registryId")) return 0;
                if (!int.TryParse(session["registryId"].ToString(), out tmp)) return 0;
                return tmp;
            }
        }

        private IEditRegistyItemView view = null;

        private ISessionProvider session = null;

        private IEditRegistryItemTask service = null;

        public EditRegistyItemPresenter(IEditRegistyItemView view, ISessionProvider sessionProvider)
        {
            this.view = view;
            this.session = sessionProvider;
            this.service = new EditRegistryItemTask();
            subscribeToEvents();
        }

        public override void Initialize()
        {
            session.Remove("isRF");
            ((IView)view).ViewTitle = "Edycja pozycji dziennika kancelaryjnego";
            if (session["itemIdRequest"] != null)
            {
                session["registryId"] = session["itemIdRequest"];

                if (session["itemRFRequest"] != null)
                {
                  
                  
                    session["itemId"] = service.AcquireItemID(registryId, ((IView)view).UserID, view.UserName, view.UserFullName, true);
                    session.Remove("itemRFRequest");
                    session["isRF"] = true;

                }
                else
                {
                  
                   
                    session["itemId"] = service.AcquireItemID(registryId, ((IView)view).UserID, view.UserName, view.UserFullName, false);
                }

                
            }
            view.IsInvoice = IsInvoice;
            if (itemId == 0 || registryId == 0)
            {
                //(view as IView).ErrorMessage = "Nieprawid³owe wywo³anie strony!";
                throw new ArgumentException("Nieprawid³owe wywo³anie strony!");
            }

           

            if (session["itemIdRequest"] != null)
            {
                view.IsInInsertState = true;
                view.ItemID = itemId;
                getItemData();
                getDictionaries(false);
                session.Remove("itemIdRequest");
            }
            else
            {
                view.IsInEditState = true; 
                view.ItemID = itemId;
                getItemData();
                getDictionaries(true);

            }

            view.IsDailyLogItemAccessDenied = service.IsGetDailyLogItemAccessDenied(registryId, itemId, ((IView)view).UserID);


        }

        protected override void subscribeToEvents()
        {
            view.ItemCommand += new EventHandler<ExecutingCommandEventArgs>(ExecuteItemCommand);

        }

        void ExecuteItemCommand(object sender, ExecutingCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "edytuj":
                    view.IsInEditState = true;
                    getItemData();
                    getDictionaries(true);
                    break;
                case "anuluj":
                    view.IsInPreviewState = true;
                    getItemData();
                    break;
                //case "zapisz":
                //  SaveItem();
                //  view.IsInPreviewState = true;
                //  getItemData();
                //  break;
                case "nowyInteresant":
                    //	getItemData();
                    //	getDictionaries(false);
                    view.IsInCustomerInsertState = true;
                    break;
                //case "zapiszNowegoInteresanta":
                //  //zapiszNowegoInteresanta();
                //  view.IsInEditState = true;
                //  getItemData();
                //  break;
                //case "listaInteresantow":
                //  view.IsInInsertState = true;
                //  getItemData();
                //  getDictionaries(false);
                //  break;
                case "zapiszZmianyInteresanta":
                    //zapis
                    view.IsInInsertState = true;
                    getItemData();
                    //	getDictionaries(false);
                    break;
                case "ustawSlownik":
                    if (e.CommandArgument.ToString() == "customer")
                        view.Customers = service.GetCustomers(view.CustomerType);
                    if (e.CommandArgument.ToString() == "employee")
                        view.Employees = service.GetEmployees(view.OrganizationalUnitId);
                    if (e.CommandArgument.ToString() == "documentTypes")
                        view.DocumentTypes = service.GetDocumentTypes(view.DocumentCategory);
                    break;
            }
        }

        public void SaveItem(string item, string espDocId)
        {
            int idd = service.UpdateItem(itemId, registryId, item, ((IView)view).UserID, view.UserName, view.UserFullName,view.IsInvoice);
            if (espDocId != string.Empty)
            {
                Guid espDocumentId = new Guid(espDocId);
                SaveESPDocumentAttachments(espDocumentId, idd);
                (new DocumentDAO()).DeleteESPDocument(espDocumentId, idd);
            }
            //view.IsInPreviewState = true;
            getItemData();

        }

        void SaveESPDocumentAttachments(Guid docId, int newDocId)
        {
            IItemStorage storage = ItemStorageFactory.Create();
            using (DbDataReader dr = (DbDataReader)(new DocumentDAO()).GetESPDocumentData(docId))
            {
                if (dr.Read())
                {
                    string zalaczniki = dr["zalaczniki"].ToString();
                    XmlDocument attachments = new XmlDocument();
                    attachments.LoadXml(zalaczniki);

                    foreach (XmlNode attachment in attachments.SelectNodes("//zalacznik"))
                    {
                        Guid attachId = new Guid(attachment.Attributes["id"].Value);
                        Guid newItemId = new Guid();
                        Stream attachStream = storage.LoadGuid(attachId);
                        attachStream.Seek(0, SeekOrigin.Begin);
                       
                        (new DocumentDAO()).AddNewDocumentItem(newDocId, attachment.Attributes["nazwa"].Value, "za³¹cznik", attachStream, attachment.Attributes["mime"].Value, ref newItemId,DocumentItemCategory.Uploaded); 
                    }
                }
            }
        }

        protected void getItemData()
        {
            getItemData(IsInvoice);
        }

        protected void getItemData(bool isRF)
        {
            view.ItemContent = service.GetItem(itemId, registryId,isRF);
        }

        protected void getDictionaries(bool setSelectedValues)
        {
            view.Customers = service.GetCustomers(1);
            view.OrganizationalUnits = service.GetOrganizationalUnits();
            view.Employees = service.GetEmployees(view.OrganizationalUnitId);
            view.DocumentCategories = service.GetDocumentCategories();
            view.DocumentTypes = service.GetDocumentTypes(view.DocumentCategory);
            view.CorrespondenceTypes = service.GetCorrespondenceTypes();

            if (setSelectedValues)
            {
                XPathDocument xpd = new XPathDocument(new StringReader(view.ItemContent));
                XPathNavigator xpnav = xpd.CreateNavigator();
                try
                {
                    //view.CustomerType = int.Parse(xpnav.SelectSingleNode("pozycja/nadawca/@typ").Value);
                    view.CustomerId = int.Parse(xpnav.SelectSingleNode("pozycja/nadawca/@id").Value);
                    view.OrganizationalUnitId = int.Parse(xpnav.SelectSingleNode("pozycja/znakReferenta/wydzial/@id").Value);
                    view.Employees = service.GetEmployees(view.OrganizationalUnitId);
                    view.EmployeeId = int.Parse(xpnav.SelectSingleNode("pozycja/znakReferenta/pracownik/@id").Value);
                    view.DocumentCategory = int.Parse(xpnav.SelectSingleNode("pozycja/klasyfikacjaDokumentu/kategoria/@id").Value);
                    view.DocumentTypes = service.GetDocumentTypes(view.DocumentCategory);
                    view.DocumentType = int.Parse(xpnav.SelectSingleNode("pozycja/klasyfikacjaDokumentu/rodzaj/@id").Value);
                    view.CorrespondenceType = int.Parse(xpnav.SelectSingleNode("pozycja/typKorespondencji/@id").Value);
                }
                catch { }
            }
      

        }

        protected override void redirectToPreviousView()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SaveNewCustomer(int customerType, string firstName, string lastName, string companyName, string postalCode, string city, string street, string building, string flat, string post)
        {
            CustomerDTO customer = new CustomerDTO(customerType);
            customer.FirstName = firstName;
            customer.LastName = lastName;
            customer.Name = companyName;
            customer.Address.PostalCode = postalCode;
            customer.Address.City = city;
            customer.Address.Street = street;
            customer.Address.Building = building;
            customer.Address.Flat = flat;
            customer.Address.Post = post;

            int id = service.CreateCustomer(customer);
            view.IsInCustomerInsertState = false;
            //		getItemData();
            //		getDictionaries(false);
            view.CustomerType = customerType;
            view.CustomerId = id;
        }
    }
}
