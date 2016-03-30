using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class MSOTemplateDTO
    {
        public int TemplateID { set; get; }
        public string FileName { set; get; }
        public string OriginalFileName { set; get; }
        public bool IsSecure { get; set; }
        public bool IsActive { get; set; }

        public MSOTemplateDTO(int templateID, string fileName, string originalFileName, bool isSecure)
            : this(templateID, fileName, originalFileName, isSecure, false) { }

        #region todel
        //{
        //    this.TemplateID = templateID;
        //    this.FileName = fileName;
        //    this.OriginalFileName = originalFileName;
        //    this.IsSecure = isSecure;
        //}
        #endregion

        public MSOTemplateDTO(int templateID, string fileName, string originalFileName)
            : this(templateID, fileName, originalFileName, false, false) { }

        #region todel
        //{
        //    this.TemplateID = templateID;
        //    this.FileName = fileName;
        //    this.OriginalFileName = originalFileName;            
        //}
        #endregion

        public MSOTemplateDTO(int templateID, string fileName, string originalFileName, bool isSecure, bool isActive)
        {
            this.TemplateID = templateID;
            this.FileName = fileName;
            this.OriginalFileName = originalFileName;
            this.IsSecure = isSecure;
            this.IsActive = isActive;
        }
    }
}
