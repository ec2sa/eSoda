using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using Pemi.eSoda.CustomForms;

namespace Pemi.Esoda.Presenters
{
	public interface IViewDocumentFormView
	{        
        int DocumentId { get; }
        string Message { set; }
        bool WarningVisible { set; }
        bool EditButtonVisible { set; }
        bool HideCustomFormWrapper { set; }
        string XmlData { set; get; }
        #region todel
        //string FileName { set; get; }
        //string OriginalFileName { set; get; }
        //string ClassName { set; get; }
        #endregion
        CustomFormDisplayMode CurrentMode { set; get; }
        void LoadFormWrapper(string assemblyName, string resourceName, string formHash);
        bool IsFormWrapperContentValid { get; }
	}
}
