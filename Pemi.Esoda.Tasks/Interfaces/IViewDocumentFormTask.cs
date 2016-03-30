using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
	public interface IViewDocumentFormTask
	{
        CustomFormDTO GetCustomFormData(int documentID);

        CustomFormDTO GetCustomFormData(int itemID, bool isLegalAct);

        IList<CustomFormHistoryItemDTO> GetCustomFormHistoryList(int documentID);

        IList<CustomFormHistoryItemDTO> GetCustomFormHistoryList(int documentID,bool legalAct);
        
        string GetCustomFormHistoryData(int itemID);
        
        CustomFormHistoryItemDTO GetCustomFormHistoryItem(int itemID);
        
        CustomFormHistoryItemDTO GetCustomFormHistoryItem(int itemID,bool isLegalAct);
	    
	}
}
