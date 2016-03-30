using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class ZarzadzanieRodzajamiSpraw : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
         
        }

        protected void addNew_Command(object sender, CommandEventArgs e)
        {
            FormView1.ChangeMode(FormViewMode.Insert);            
        }

        protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                WebMsgBox.Show(this, e.Exception.InnerException != null ? e.Exception.InnerException.Message : e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInEditMode = true;
            }
        }

        protected void FormView1_ItemDeleted(object sender, FormViewDeletedEventArgs e)
        {
            GridView1.SelectedIndex = -1;

            if (e.Exception != null)
            {
                WebMsgBox.Show(this, e.Exception.InnerException != null ? e.Exception.InnerException.Message : e.Exception.Message);
                e.ExceptionHandled = true;
            }
        }

        protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            GridView1.SelectedIndex = -1;
           
            if (e.Exception != null)
            {
                WebMsgBox.Show(this, e.Exception.InnerException != null ? e.Exception.InnerException.Message : e.Exception.Message);
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;
            }
        }
    }
}