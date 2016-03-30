using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace Pemi.Esoda.Tasks
{
    public class EditRegistryItemTask : IEditRegistryItemTask
    {
        private RegistryDAO dao = new RegistryDAO();

        #region IEditRegistryItemTask Members

        int IEditRegistryItemTask.AcquireItemID(int registryId, Guid userID, string userName, string fullName,bool isRF)
        {
            int[] ids = dao.AcquireItemID(registryId, userID,isRF);
            List<string> parameters = new List<string>();
            parameters.Add(ids[0].ToString());
            ActionLogger al = new ActionLogger(new ActionContext(new Guid("5F5E6C05-A007-4121-BD56-56317F154C28"), userID, userName, fullName, parameters));
            al.AppliesToDocuments.Add(ids[1]);
            al.ActionData.Add("numerPozycjiDziennika", ids[0].ToString());
            al.Execute();
            return ids[0];
        }

        string IEditRegistryItemTask.GetItem(int itemId, int registryId,bool isRF)
        {
            using (XmlReader xr = dao.GetItem(itemId, registryId,isRF))
            {
                if (!xr.Read())
                    return string.Empty;
                return xr.ReadOuterXml();
            }
        }

        int IEditRegistryItemTask.UpdateItem(int itemId, int registryId, string itemContent, Guid userID, string userName, string fullName, bool isRF)
        {
            int idd= dao.SaveItem(registryId, itemId, itemContent,isRF);
            List<string> parameters = new List<string>();
            parameters.Add(itemId.ToString());
            ActionLogger al = new ActionLogger(new ActionContext(new Guid("35E02BB1-3EB2-49FF-85B3-AB838E3B5C3B"), userID, userName, fullName, parameters));
            XPathDocument xpd = new XPathDocument(new StringReader(itemContent));
            XPathNavigator xp = xpd.CreateNavigator();
            al.AppliesToDocuments.Add(idd);
            al.ActionData.Add("numerPozycjiDziennika", itemId.ToString());
            al.ActionData.Add("dataPisma", xp.SelectSingleNode("/wpis/dataPisma").Value);
            al.ActionData.Add("dataWplywu", xp.SelectSingleNode("/wpis/dataWplywu").Value);
            al.ActionData.Add("nadawca", xp.SelectSingleNode("/wpis/nadawca").Value);
            al.ActionData.Add("znakPisma", xp.SelectSingleNode("/wpis/numerPisma").Value);
            al.ActionData.Add("opis", xp.SelectSingleNode("/wpis/opis").Value);
            al.ActionData.Add("kategoriaDokumentu", xp.SelectSingleNode("/wpis/klasyfikacjaDokumentu/kategoria").Value);
            al.ActionData.Add("rodzajDokumentu", xp.SelectSingleNode("/wpis/klasyfikacjaDokumentu/rodzaj").Value);
            al.ActionData.Add("numerDokumentu", xp.SelectSingleNode("/wpis/klasyfikacjaDokumentu/wartosc").Value);
            al.ActionData.Add("typKorespondencji", xp.SelectSingleNode("/wpis/typKorespondencji/rodzaj").Value);
            al.ActionData.Add("numerKorespondencji", xp.SelectSingleNode("/wpis/typKorespondencji/wartosc").Value);
            al.ActionData.Add("uwagi", xp.SelectSingleNode("/wpis/uwagi").Value);
            al.ActionData.Add("znakReferenta", xp.SelectSingleNode("/wpis/znakReferenta/pracownik").Value);
            al.ActionData.Add("wydzial", xp.SelectSingleNode("/wpis/znakReferenta/wydzial").Value);
            al.ActionData.Add("kwota", string.IsNullOrEmpty(xp.SelectSingleNode("/wpis/kwota").Value) ? "0" : xp.SelectSingleNode("/wpis/kwota").Value);
            al.ActionData.Add("dodatkoweMaterialy", xp.SelectSingleNode("/wpis/dodatkoweMaterialy").Value);
            al.Execute();
            return idd;
        }


       public  System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> GetOrganizationalUnits()
        {
            System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> lista = dao.GetOrganizationalUnits();
            lista.Insert(0, new Pemi.Esoda.DTO.SimpleLookupDTO(0, "- nieokreœlony -"));
            return lista;
        }

        public System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> GetEmployees(int organizationalUnitId)
        {
            System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> lista = dao.GetEmployees(organizationalUnitId);
            lista.Insert(0, new Pemi.Esoda.DTO.SimpleLookupDTO(0, "- nieokreœlony -"));
            return lista;
        }

        System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> IEditRegistryItemTask.GetCustomers(int customerType)
        {
            return dao.GetCustomers(customerType);
        }

        System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> IEditRegistryItemTask.GetDocumentTypes(int categoryId)
        {
            return dao.GetDocumentTypes(categoryId);
        }

        int IEditRegistryItemTask.CreateCustomer(Pemi.Esoda.DTO.CustomerDTO newCustomer)
        {
            return dao.CreateCustomer(newCustomer);
        }


        System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> IEditRegistryItemTask.GetDocumentCategories()
        {
            return dao.GetDocumentCategories();
        }

        System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> IEditRegistryItemTask.GetCorrespondenceTypes()
        {
            return dao.GetCorrespondenceTypes();
        }
     
        public bool IsGetDailyLogItemAccessDenied(int registryId, int itemId, Guid userId)
        {
            return dao.IsDailyLogItemAccessDenied(registryId, itemId, userId);
        }

        #endregion
    }
}