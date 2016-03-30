using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
	public class ViewRegistryTask:IViewRegistryTask
	{
		private RegistryDAO dao = new RegistryDAO();

		private int totalItems;

		#region IViewRegistryTask Members

		string IViewRegistryTask.GetItemsPage(Guid userId, int registryId, int pageNumber, int pageSize, DateTime startDate, DateTime endDate,
			string searchIncomeDate, string searchDocumentDate, string searchDocumentNumber, string searchSenderName,
			int searchCorrespondenceCategory, int searchCorrespondenceType, string searchCategoryValue,
            int searchCorrespondenceKind, string searchTypeValue, int searchCorrespondenceStatus, int searchCorrespondenceDept, int searchCorrespondenceWorker,bool isRF)
		{
            int startingIndex = pageNumber; //(pageNumber-1)*pageSize+1;
			using (XmlReader xr = dao.GetItemsPage(userId, registryId, startingIndex, pageSize, startDate, endDate,searchIncomeDate,
				searchDocumentDate, searchDocumentNumber, searchSenderName, searchCorrespondenceCategory, 
                searchCorrespondenceType, searchTypeValue, searchCorrespondenceKind,  searchCategoryValue, searchCorrespondenceStatus, searchCorrespondenceDept, searchCorrespondenceWorker, out this.totalItems,isRF))
			{
				if (!xr.Read())
					return string.Empty;
				return xr.ReadOuterXml();
			}
		}

        string IViewRegistryTask.GetItemsPage(Guid userId, int registryId, int pageNumber, int pageSize, DateTime startDate, DateTime endDate,bool showInvoices)
        {
            int startingIndex = pageNumber; //(pageNumber-1)*pageSize+1;
            using (XmlReader xr = dao.GetItemsPage(userId, registryId, startingIndex, pageSize, startDate, endDate,showInvoices, out this.totalItems))
            {
                if (!xr.Read())
                    return string.Empty;
                return xr.ReadOuterXml();
            }
        }

		int IViewRegistryTask.TotalItemsCount
		{
			get { return this.totalItems; }
		}

		System.Collections.ObjectModel.Collection<SimpleLookupDTO> IViewRegistryTask.GetCorrespondenceCategories()
		{
			System.Collections.ObjectModel.Collection<SimpleLookupDTO> items = dao.GetDocumentCategories();
			items.Insert(0, new SimpleLookupDTO(-1, "- wszystkie -"));
			return items;
		}

		System.Collections.ObjectModel.Collection<SimpleLookupDTO> IViewRegistryTask.GetCorrespondenceTypes()
		{
			System.Collections.ObjectModel.Collection<SimpleLookupDTO> items = dao.GetCorrespondenceTypes();
			items.Insert(0, new SimpleLookupDTO(-1, "- wszystkie -"));
			return items;
		}

        System.Collections.ObjectModel.Collection<SimpleLookupDTO> IViewRegistryTask.GetCorrespondenceKinds(int catId)
        {
            System.Collections.ObjectModel.Collection<SimpleLookupDTO> items = dao.GetDocumentTypes(catId);
            items.Insert(0, new SimpleLookupDTO(-1, "- wszystkie -"));
            return items;
        }

		#endregion

		#region IViewRegistryTask Members


		int IViewRegistryTask.GetCurrentRegistryId(int definitionId, int year)
		{
			return dao.GetCurrentRegistryId(definitionId, year);
		}

		#endregion

        #region IViewRegistryTask Members


        System.Collections.ObjectModel.Collection<SimpleLookupDTO> IViewRegistryTask.GetCorrespondenceStatus()
        {
            System.Collections.ObjectModel.Collection<SimpleLookupDTO> items = dao.GetCorrespondenceStatus();
            items.Insert(0, new SimpleLookupDTO(-1, "- dowolny -"));
            return items;
        }

        System.Collections.ObjectModel.Collection<SimpleLookupDTO> IViewRegistryTask.GetCorrespondenceDepts()
        {
            System.Collections.ObjectModel.Collection<SimpleLookupDTO> items = dao.GetCorrespondenceDepts();
            items.Insert(0, new SimpleLookupDTO(0, "Niezadekretowane"));
            items.Insert(0, new SimpleLookupDTO(-1, "(brak filtrowania)"));
            return items;
        }

        System.Collections.ObjectModel.Collection<SimpleLookupDTO> IViewRegistryTask.GetCorrespondenceWorkers(int deptId)
        {
            System.Collections.ObjectModel.Collection<SimpleLookupDTO> items = dao.GetCorrespondenceWorkers(deptId);
            items.Insert(0, new SimpleLookupDTO(0, "Niezadekretowane"));
            items.Insert(0, new SimpleLookupDTO(-1, "(brak filtrowania)"));
            return items;
        }

        #endregion

        #region IViewRegistryTask Members


        public string GetItemsPage(Guid userId, int registryId, int pageNumber, int pageSize, DateTime startDate, DateTime endDate, int year)
        {
            int startingIndex = pageNumber; 
            using (XmlReader xr = dao.GetItemsPage(userId, registryId, startingIndex, pageSize, startDate, endDate, year, out this.totalItems))
            {
                if (!xr.Read())
                    return string.Empty;
                return xr.ReadOuterXml();
            }
        }

        #endregion
    }
}
