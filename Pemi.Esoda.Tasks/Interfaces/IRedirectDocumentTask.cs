using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using System.Collections;

namespace Pemi.Esoda.Tasks
{
	public interface IRedirectDocumentTask
	{
		void RedirectDocument(string actionId,int documentId, Guid userId,string userName,string userFullname,bool workOnPaper,string note, int organizationalUnitId, int employeeId,string ouName,string empName);
        ArrayList RedirectDocumentMultiple(string actionId,int documentId, Guid userId, string userName,string userFullname,bool workOnPaper,string note, int organizationalUnitId, int employeeId,string ouName,string empName, IList<RedirectItem> redirectList);
        bool IsCopy(int documentId);
        DokumentDetails GetDocumentDetails(int documentId);
	}
}
