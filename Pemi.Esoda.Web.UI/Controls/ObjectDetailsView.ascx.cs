using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.DTO;
using Pemi.Esoda.Core.Domain;
using System.Collections.ObjectModel;
using System.Xml;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class ObjectDetailsView : System.Web.UI.UserControl
    {
        private Collection<DocumentDTO> dokument;
        private Collection<CaseDTO> sprawa;

        public int DocumentID;
        public int CaseID;

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (DocumentID != 0)
                GetDocumentDetails();
            else if (CaseID != 0)
                GetCaseDetails();
        }

        private void GetDocumentDetails()
        {            
            Document doc = new Document(DocumentID);
            dokument = new Collection<DocumentDTO>();
            dokument.Add(doc.GetDocumentData());
            daneDokumentu.DataSource = dokument;
            daneDokumentu.DataBind();
            detailsView.ActiveViewIndex = 1;
        }

        private void GetCaseDetails()
        {
            Case _case = new Case(CaseID);           
            sprawa = new Collection<CaseDTO>();
            sprawa.Add(_case.GetCaseData());
            daneSprawy.DataSource = sprawa;
            daneSprawy.DataBind();
            detailsView.ActiveViewIndex = 0;
        }
    }
}