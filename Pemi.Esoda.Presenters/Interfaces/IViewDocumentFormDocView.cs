using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using Pemi.Esoda.Tools.MSOIntegrationHelper;

namespace Pemi.Esoda.Presenters
{
    public interface IViewDocumentFormDocView
    {
        int DocumentID { get; }
        Modes Mode { get; }
        string FileName { get; set; }
        bool WithSchema { get; set; }
        string ErrorMessage { set; }
    }    
}
