using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.DTO
{
    [Serializable]
    public class CustomFormDTO
    {
        public int DocumentTypeID { get; set; } //0 means new
        public int DocumentCategoryID { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentTypeAbbr { get; set; }
        public bool DocumentTypeActive {get;set;}
        public string Filename { get; set; }
        public string OriginalFilename { get; set; }
        public bool IsCFActive { get; set; }
        public bool IsMSOActive { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public string XmlData { get; set; }
        public string FormHash { get; set; }
        public string DataHash { get; set; }
        public string WordFilename { get; set; }
        public string WordOriginalFilename { get; set; }
        public string WordSchemaFilename { get; set; }
        public string WordSchemaOriginalFilename { get; set; }
        public bool IsLegalAct { get; set; }
        
        public CustomFormDTO(int documentTypeID,int documentCategoryID,string documentTypeName,string documentTypeAbbr,bool documentTypeActive,bool isLegalAct,string filename, string oryginalFilename, bool isActive, string className, string description)
        {
            this.DocumentTypeID = documentTypeID;
            this.DocumentCategoryID = documentCategoryID;
            this.DocumentTypeName = documentTypeName;
            this.DocumentTypeAbbr = documentTypeAbbr;
            this.IsLegalAct = isLegalAct;
            this.DocumentTypeActive = documentTypeActive;
            this.Filename = filename;
            this.OriginalFilename = oryginalFilename;
            this.IsCFActive = isActive;
            this.ClassName = className;
            this.Description = description;
        }

        public CustomFormDTO(int documentTypeID, int documentCategoryID, string documentTypeName, string documentTypeAbbr, bool documentTypeActive, bool isLegalAct, string filename, string oryginalFilename, bool isCFActive, string className, string description, string wordFilename, string wordOriginalFilename, string wordSchemaFilename, string wordSchemaOriginalFilename, bool isMSOActive)
        {
            this.DocumentTypeID = documentTypeID;
            this.DocumentCategoryID = documentCategoryID;
            this.DocumentTypeName = documentTypeName;
            this.DocumentTypeAbbr = documentTypeAbbr;
            this.DocumentTypeActive = documentTypeActive;
            this.Filename = filename;
            this.OriginalFilename = oryginalFilename;
            this.IsCFActive = isCFActive;
            this.IsMSOActive = isMSOActive;
            this.IsLegalAct = isLegalAct;
            this.ClassName = className;
            this.Description = description;
            this.WordFilename = wordFilename;
            this.WordOriginalFilename = wordOriginalFilename;
            this.WordSchemaFilename = wordSchemaFilename;
            this.WordSchemaOriginalFilename = wordSchemaOriginalFilename;
        }

        public CustomFormDTO(int documentTypeID, int documentCategoryID, string fileName, string originalFileName, string className, bool isActive, bool isLegalAct, string xmlData, string formHash, string dataHash)
        {
            this.DocumentTypeID = documentTypeID;
            this.DocumentCategoryID = documentCategoryID;
            this.Filename = fileName;
            this.OriginalFilename = originalFileName;
            this.ClassName = className;
            this.IsCFActive = isActive;
            this.IsLegalAct = isLegalAct;
            this.XmlData = xmlData;
            this.FormHash = formHash;
            this.DataHash = dataHash;
        }
    }    

    public class CustomFormVisibilityDTO
    {
        public bool CustomFormVisible { get; set; }
        
        public bool WordFormVisible { get; set; }
        
        public bool WordFormEditVisible { get; set; }
        
        public bool XmlVisible { get; set; }
        
        public bool HistoryVisible { get; set; }
        
        public bool LegalActXmlVisible { get; set; }
        
        public bool LegalActHistoryVisible { get; set; }

        public bool SendToEPUAPVisible { get; set; }

        public CustomFormVisibilityDTO(bool customFormVisible, bool wordFormVisible, bool wordFormEditVisible, bool xmlVisible, bool historyVisible,bool legalActXmlVisible,bool legalActHistoryVisible, bool sendToEPUAPVisible)
        {
            this.CustomFormVisible = customFormVisible;
            this.WordFormVisible = wordFormVisible;
            this.WordFormEditVisible = wordFormEditVisible;
            this.XmlVisible = xmlVisible;
            this.HistoryVisible = historyVisible;
            this.LegalActHistoryVisible = legalActHistoryVisible;
            this.LegalActXmlVisible = legalActXmlVisible;
            this.SendToEPUAPVisible = sendToEPUAPVisible;
        }
    }

    public class CustomFormHistoryItemDTO
    {
        public int ItemID { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public string XmlData { get; set; }        

        public CustomFormHistoryItemDTO() { }
        public CustomFormHistoryItemDTO(int itemID, DateTime date, string username)
        {
            this.ItemID = itemID;
            this.Date = date;
            this.Username = username;
        }
    }
}
