using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pemi.Esoda.Presenters.Interfaces
{
    public interface IViewLegalActView
    {
        int DocumentID { get; }
        string FileName { get; set; }
        string ErrorMessage { set; } 
    }
}
