using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
	public class ViewDocumentFormTask:IViewDocumentFormTask
	{
        private CustomFormDAO dao = new CustomFormDAO();		

        #region IViewDocumentFormTask Members

        public CustomFormDTO GetCustomFormData(int documentID)
        {
            return GetCustomFormData(documentID,false);
        }

        public CustomFormDTO GetCustomFormData(int documentID,bool isLegalAct)
        {
            return dao.GetCustomFormData(documentID,isLegalAct);
        }

        public IList<CustomFormHistoryItemDTO> GetCustomFormHistoryList(int documentID, bool legalAct)
        {
            return dao.GetCustomFormHistoryList(documentID, legalAct);
        }

        public IList<CustomFormHistoryItemDTO> GetCustomFormHistoryList(int documentID)
        {
            return dao.GetCustomFormHistoryList(documentID,false);
        }
       
        public string GetCustomFormHistoryData(int itemID)
        {
            return dao.GetCustomFormHistoryData(itemID);
        }

        public CustomFormHistoryItemDTO GetCustomFormHistoryItem(int itemID)
        {
            return GetCustomFormHistoryItem(itemID, false);
        }

        public CustomFormHistoryItemDTO GetCustomFormHistoryItem(int itemID,bool isLegalAct)
        {
            return dao.GetCustomFormHistoryItem(itemID,isLegalAct);
        }


        #endregion
    }
}
