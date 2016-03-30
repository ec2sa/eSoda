using System;
using System.Collections.Generic;
using System.Web;

namespace Pemi.Esoda.DTO
{
    [Serializable]
    public class MSOWebServiceCallContextDTO
    {
        public string Ticket { get; set; }
        public Guid DocumentGUID { get; set; }
        public int DocumentTypeID { get; set; }
        public int? LastHistoryID { get; set; }
        public Guid? LastVersionGuid { get; set; }
        public string Description { get; set; }
        public MSOCallContextType ContextType { get; set; }
        public string DesiredName { get; set; }
        public string OriginalName { get; set; }

        public MSOWebServiceCallContextDTO() { }

        public MSOWebServiceCallContextDTO(string ticket,Guid documentGuid, int documentTypeID, int? lastHistoryID)
        {
            Ticket = ticket;
            DocumentGUID = documentGuid;
            DocumentTypeID = documentTypeID;
            LastHistoryID = lastHistoryID;
            ContextType = MSOCallContextType.Form;
        }

        public MSOWebServiceCallContextDTO(string ticket, Guid documentGuid, int documentTypeID, Guid? lastVersionGuid, string description,string desiredName,string originalName)
        {
            Ticket = ticket;
            DocumentGUID = documentGuid;
            DocumentTypeID = documentTypeID;
            LastVersionGuid = lastVersionGuid;
            Description = description;
            DesiredName = desiredName;
            OriginalName = originalName;
            ContextType = MSOCallContextType.BinaryFile;
        }
    }

    public enum MSOCallContextType
    {
        BinaryFile,
        Form
    }
}
