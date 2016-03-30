using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pemi.Esoda.Tools.MSOIntegrationHelper;

namespace Pemi.Esoda.Presenters
{
    public interface IViewDocumentItemDocView
    {
        int DocumentID { get; }
        Modes Mode { get; }
        string ItemGuid { get; }
        string Description { get; }
        string FileName { get; set; }
        string ErrorMessage { set; }     
    }
}
