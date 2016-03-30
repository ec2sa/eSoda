using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;
using Pemi.Esoda.DataAccess;
using System.Xml;

namespace Pemi.Esoda.Tasks
{
	public class AssignDocumentToCaseTask:IAssignDocumentToCaseTask
	{
		private CaseDAO dao = new CaseDAO();

		#region IAssignDocumentToCaseTask Members

		Collection<SimpleLookupDTO> IAssignDocumentToCaseTask.GetBriefcaseList(Guid userId, int year)
		{
			Collection<SimpleLookupDTO> lista = dao.GetBriefcaseList(userId, year);
			lista.Insert(0, new Pemi.Esoda.DTO.SimpleLookupDTO(0, "- wybierz -"));
			return lista;
		}

		Collection<SimpleLookupDTO> IAssignDocumentToCaseTask.GetCaseNumbersList(int caseTypeId)
		{
			Collection<SimpleLookupDTO> lista = dao.GetCaseNumbers(caseTypeId);
			lista.Insert(0, new Pemi.Esoda.DTO.SimpleLookupDTO(0, "- wybierz -"));
			return lista;
		}

		Collection<SimpleLookupDTO> IAssignDocumentToCaseTask.GetCaseKindList()
		{
			return dao.GetCaseKinds();
		}

        Collection<SimpleLookupDTO> IAssignDocumentToCaseTask.GetCaseKindsFromBriefcase(int briefcaseId)
        {
            return (new BriefcaseDAO()).GetCaseKindsFromBriefcase(briefcaseId);
            //return (new BriefcaseDAO()).GetCaseKindsFromBriefcaseXML(briefcaseId);
        }

        public Collection<SimpleLookupDTO> GetAvailableYears()
        {
            return dao.GetAvailableYears();
        }

		string IAssignDocumentToCaseTask.GetDocumentData(int documentId)
		{
			using (XmlReader xr = dao.GetDocumentDataForCase(documentId))
			{
				if (!xr.Read())
					return string.Empty;
				return xr.ReadOuterXml();
			}
		}

		int IAssignDocumentToCaseTask.AssignDocumentToCase(Guid userId, int documentId, int caseNumber)
		{
            if(caseNumber==0)
                return -1; // throw new ArgumentException(string.Format("Nale¿y wskazaæ sprawê, do której zostanie przypisany dokument {0} {1}", userId, documentId));
            dao.AssignDocumentToExistingCase(userId, documentId, caseNumber);
            return 0;
		}

		int IAssignDocumentToCaseTask.AssignDocumentToNewCase(Guid userId, int documentId, int caseTypeId, int caseKindId, string description, DateTime? documentDate, string documentSignature, string sender)
		{
            if (caseTypeId == 0)
                return -1; // throw new ArgumentException(string.Format("Nale¿y wskazaæ oznaczenie teczki, w której zostanie utworzona sprawa! {0} {1} {2}", userId, caseTypeId, caseKindId));
			return dao.AssignDocumentToNewCase(userId, documentId, caseTypeId, caseKindId, description, documentDate, documentSignature,sender);
		}

		
       

        #endregion
    }
}
