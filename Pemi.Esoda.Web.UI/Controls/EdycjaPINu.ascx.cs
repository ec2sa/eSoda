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
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class EdycjaPINu : System.Web.UI.UserControl
    {
        private bool adminMode;
        private bool editMode;
        private Guid userId;

        public string ValidationGroup
        {
            get { return cvPIN.ValidationGroup; }
            set
            {
                rfvPIN.ValidationGroup = rfvPIN2.ValidationGroup = rfvConfirmPass.ValidationGroup = lnkSavePIN.ValidationGroup = cvPIN.ValidationGroup = cuvPIN.ValidationGroup = value;
            }
        }

        public string PIN
        {
            get { return txtPIN.Text; }
        }

        public Guid UserId
        {
            set { userId = value; }
        }

        public string ConfirmPAssword
        {
            get
            {
                if (adminMode) return string.Empty;
                else
                    return txtPINConfirm.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            lblMsg.Text = "";
        }

        public void AdminMode(bool mode)
        {
            adminMode = mode;
           
            lblConfirm.Visible = !adminMode;
            txtPINConfirm.Visible = !adminMode;
            rfvConfirmPass.Enabled = !adminMode;
            lnkSavePIN.Visible = !adminMode || editMode;
        }

        public void EditMode(bool mode)
        {
            editMode = mode;

            lblConfirm.Visible = !adminMode;
            txtPINConfirm.Visible = !adminMode;
            rfvConfirmPass.Enabled = !adminMode;
            lnkSavePIN.Visible = !adminMode || editMode;
        }

        protected void lnkSavePIN_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    if (Membership.ValidateUser(Page.User.Identity.Name, txtPINConfirm.Text) || adminMode)
                    {
                        (new UserDAO()).SetUserPIN(userId, txtPIN.Text);
                        lblMsg.Text = "PIN zosta³ zmieniony.";
                    }
                    else
                    {
                        lblMsg.Text = "Nie uda³o siê zmieniæ numeru PIN. B³êdne has³o !!!";
                    }                        
                }
                catch 
                {
                    if(!adminMode)
                    lblMsg.Text = "Nie uda³o siê zmieniæ numeru PIN.";
                }
            }
        }

        protected void cuvPIN_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ((args.Value.Length==0 && adminMode) || args.Value.Length >= 4);
        }
    }
}