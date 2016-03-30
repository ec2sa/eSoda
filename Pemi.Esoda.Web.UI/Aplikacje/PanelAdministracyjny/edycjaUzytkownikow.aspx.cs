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
using Pemi.Esoda.Tools;
using Pemi.Esoda.Web.UI.Controls;

namespace Pemi.Esoda.Web.UI
{
    public partial class edycjaUzytkownikow : System.Web.UI.Page
    {
        public event EventHandler SelectedUserChanged;

        public int SelectedUserID
        {
            set
            {
                ViewState["SelectedUserID"] = value;
            }
            get
            {
                if (ViewState["SelectedUserID"] != null)
                    return (int)ViewState["SelectedUserID"];
                else
                    return -1;
            }

        }
        public void OnSelectedUserChanged()
        {
            if (SelectedUserChanged != null)
                SelectedUserChanged(this, null);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadEmployeeList();
            }
            lnkAddUser.Visible = !frmUser.Visible || frmUser.CurrentMode != FormViewMode.Insert;

            EdycjaPINu pinEdit = (frmUser.FindControl("pinEdit") as EdycjaPINu);
            if (pinEdit != null)
            {
                pinEdit.AdminMode(true);
                if (frmUser.CurrentMode == FormViewMode.Insert)
                {
                    pinEdit.ValidationGroup = "AddingUser";
                    pinEdit.EditMode(false);
                }
                else
                {
                    pinEdit.EditMode(true);
                    pinEdit.ValidationGroup = "ValidatePIN";
                }
            }

            this.SelectedUserChanged += new EventHandler(edycjaUzytkownikow_SelectedUserChanged);
            definicjaZastepstw.CoverChanged += new EventHandler(definicjaZastepstw_CoverChanged);
        }

        void definicjaZastepstw_CoverChanged(object sender, EventArgs e)
        {
            LoadEmployeeList();
        }

        void edycjaUzytkownikow_SelectedUserChanged(object sender, EventArgs e)
        {
            definicjaZastepstw.DublerID = this.SelectedUserID;
        }

        private void LoadEmployeeList()
        {
           gvUsersList.DataBind();
        }

        protected void gvUsersList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch(e.CommandName)
            {
                case "Select":
                    dsUser.SelectParameters.Clear();
                    dsUser.SelectParameters.Add("idToz", gvUsersList.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString());
                    frmUser.Visible = true;
                    frmUser.ChangeMode(FormViewMode.Edit);
                    frmUser.DataBind();

                    SelectedUserID = new UserDAO().GetUserID(new Guid(gvUsersList.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString()));
                    definicjaZastepstw.Visible = true;
                   
                    EdycjaPINu pinEdit = (frmUser.FindControl("pinEdit") as EdycjaPINu);
                    if (pinEdit != null)
                    {
                        pinEdit.UserId = new Guid(dsUser.SelectParameters["idToz"].DefaultValue.ToString());
                        pinEdit.AdminMode(true);
                        if (frmUser.CurrentMode == FormViewMode.Insert)
                        {
                            pinEdit.ValidationGroup = "AddingUser";
                            pinEdit.EditMode(false);
                        }
                        else
                        {
                            pinEdit.EditMode(true);
                            pinEdit.ValidationGroup = "ValidatePIN";
                        }
                    }

                    OnSelectedUserChanged();

                    break;

                    

                default: break;
            }
        }

        protected void frmUser_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            string nazwisko, imie, wydzial, stanowisko,login, pin=string.Empty;
            bool active = false, locked=false;
            MembershipUser user = null;
            if(e.CommandArgument!= null && e.CommandArgument.ToString().Length>0)
                user = Membership.GetUser(new Guid(e.CommandArgument.ToString()));

            switch (e.CommandName)
            {
                case "UserCancel":
                    frmUser.Visible = false;
                    lnkAddUser.Visible = true;
                    definicjaZastepstw.Visible = false;
                    break;

                case "InsertUser":
                    if (Page.IsValid)
                    {
                        login = ((TextBox)frmUser.FindControl("txtLogin")).Text;
                        string haslo = ((TextBox)frmUser.FindControl("txtPassword")).Text;
                        string email = ((TextBox)frmUser.FindControl("txtEmail")).Text;
                        stanowisko = ((TextBox)frmUser.FindControl("txtStanowisko")).Text;
                        nazwisko = ((TextBox)frmUser.FindControl("txtNazwisko")).Text;
                        imie = ((TextBox)frmUser.FindControl("txtImie")).Text;
                        wydzial = ((DropDownList)frmUser.FindControl("ddlWydzial")).SelectedItem.Text;
                        active = ((CheckBox)frmUser.FindControl("ckbActive")).Checked;
                        bool chatAvailable = ((CheckBox)frmUser.FindControl("chChatAvailable")).Checked;
                        bool manager = ((CheckBox)frmUser.FindControl("chManager")).Checked;

                        EdycjaPINu pinEdit = ((EdycjaPINu)frmUser.FindControl("pinEdit"));
                        if (pinEdit != null)
                            pin = pinEdit.PIN;

                        if (Membership.FindUsersByEmail(email).Count>0)
                        {
                            WebMsgBox.Show(this, string.Format("U¿ytkownik o adresie e-mail: [{0}] ju¿ istnieje. WprowadŸ inny adres email.", email));
                            return;
                        }
                        if (Membership.FindUsersByName(login).Count >0)
                        {
                            WebMsgBox.Show(this, string.Format("U¿ytkownik [{0}] ju¿ istnieje. Podaj inn¹ nazwê (login) u¿ytkownika.",login));
                            return;
                        }
                        UserDAO ud = new UserDAO();

                        MembershipUser usr = Membership.CreateUser(login, haslo, email);

                        usr.IsApproved = (login.Equals("admin")) ? true : active;
                        usr.UnlockUser();
                        Membership.UpdateUser(usr);

                        ud.CreateEmployee(nazwisko, imie, (Guid)usr.ProviderUserKey,stanowisko);
                        ud.SetUserPIN((Guid)usr.ProviderUserKey, pin);
                        ud.SetAvailableChat((Guid)usr.ProviderUserKey, chatAvailable);
                        ud.SetManager((Guid)usr.ProviderUserKey, manager);
                        //ud.CreateOrganizationalUnit(row["wydzial"].ToString(), row["skrotwydzialu"].ToString(), true, 0);
                        Roles.AddUserToRole(usr.UserName, wydzial);
                        lnkAddUser.Visible = true;
                        frmUser.Visible = false;
                        gvUsersList.DataBind();
                        frmUser.DataBind();
                        definicjaZastepstw.Visible = false;
                    }
                    break;
                
                case "UpdateUser":
                    if (Page.IsValid)
                    {
                        nazwisko = ((TextBox)frmUser.FindControl("txtNazwisko")).Text;
                        imie = ((TextBox)frmUser.FindControl("txtImie")).Text;
                        stanowisko = ((TextBox)frmUser.FindControl("txtStanowisko")).Text;
                        wydzial = ((DropDownList)frmUser.FindControl("ddlWydzial")).SelectedItem.Text;
                        (new UserDAO()).UpdateEmployee(new Guid((e.CommandArgument.ToString())),nazwisko,imie,0,stanowisko);

                        string oldEmail = user.Email;
                     
                        active = ((CheckBox)frmUser.FindControl("ckbActive")).Checked;
                        locked = ((CheckBox)frmUser.FindControl("ckbLocked")).Checked;

                        bool chatAvailable = ((CheckBox)frmUser.FindControl("chChatAvailable")).Checked;
                        bool manager = ((CheckBox)frmUser.FindControl("chManager")).Checked;

                        if (!locked)
                            user.UnlockUser();
                        user.IsApproved = active;

                        if (Membership.FindUsersByEmail(user.Email).Count > 0 && !user.Email.Equals(oldEmail))
                        {
                            WebMsgBox.Show(this, string.Format("Adres e-mail: [{0}] jest u¿ywany przez innego u¿ytkownika. WprowadŸ inny adres email.", user.Email));
                            return;
                        }
                        user.Email = ((TextBox)frmUser.FindControl("txtEmail")).Text;
                        Membership.UpdateUser(user);
                        if(!Roles.IsUserInRole(user.UserName, wydzial))
                            Roles.AddUserToRole(user.UserName, wydzial);
                        string oldWydzial = ((HiddenField)frmUser.FindControl("hfOldGroup")).Value;
                        if(!wydzial.Equals(oldWydzial) && Roles.IsUserInRole(user.UserName,oldWydzial))
                            Roles.RemoveUserFromRole(user.UserName,oldWydzial);

                        UserDAO ud = new UserDAO();
                        ud.SetAvailableChat((Guid)user.ProviderUserKey, chatAvailable);
                        ud.SetManager((Guid)user.ProviderUserKey, manager);
                        
                        lnkAddUser.Visible = true;
                        frmUser.Visible = false;
                        gvUsersList.DataBind();
                        frmUser.DataBind();
                        definicjaZastepstw.Visible = false;
                    }
                    break;

                case "UpdatePassword":
                    if (Page.IsValid)
                    {
                        string newPass = ((TextBox)frmUser.FindControl("txtNewPassword")).Text;
                        string oldPass = "";

                        try
                        {
                            oldPass = user.ResetPassword();
                            if (user.ChangePassword(oldPass, newPass))
                            {
                                WebMsgBox.Show(this, "Has³o zosta³o zmienione.");
                            }
                            else
                            {
                                WebMsgBox.Show(this, "Nie uda³o siê zmieniæ has³a.");
                            }
                        }
                        catch (Exception ex)
                        {
                            WebMsgBox.Show(this, ex.Message);
                        }
                    }
                    break;

                case "UpdatePIN":
                    if (Page.IsValid)
                    {
                        MembershipUser usr = Membership.GetUser(new Guid(dsUser.SelectParameters["idToz"].DefaultValue.ToString()));
                        EdycjaPINu pinEdit = frmUser.FindControl("pinEdit") as EdycjaPINu;
                        (new UserDAO()).SetUserPIN((Guid)usr.ProviderUserKey, pinEdit.PIN);
                    }
                    break;

                default: break;
            }                
        }

        protected void lnkAddUser_Click(object sender, EventArgs e)
        {
            frmUser.ChangeMode(FormViewMode.Insert);
            frmUser.Visible = true;
            definicjaZastepstw.Visible = false;
            SelectedUserID = -1;

            EdycjaPINu pinEdit = (frmUser.FindControl("pinEdit") as EdycjaPINu);
            if (pinEdit != null)
            {
                pinEdit.AdminMode(true);
                if (frmUser.CurrentMode == FormViewMode.Insert)
                {
                    pinEdit.ValidationGroup = "AddingUser";
                    pinEdit.EditMode(false);
                }
                else
                {
                    pinEdit.EditMode(true);
                    pinEdit.ValidationGroup = "ValidatePIN";
                }
            }

            OnSelectedUserChanged();
        }

        protected void ddlWydzial_DataBound(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Insert(0, new ListItem("--- wybierz ---","0"));
        }

        protected void cvPassword_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (args.Value.Length >= Membership.MinRequiredPasswordLength);
            if(!args.IsValid)
                ((CustomValidator)source).Text = "Has³o musi mieæ minimum " + Membership.MinRequiredPasswordLength + " znaków.";
        }

        protected void ddlPageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvUsersList.PageIndex = int.Parse(((DropDownList)sender).SelectedValue);
            gvUsersList.DataBind();
        }

        protected void ddlPageSelector_DataBound(object sender, EventArgs e)
        {
            if (gvUsersList.PageCount != ((DropDownList)sender).Items.Count)
            {
                for (int i = 0; i < gvUsersList.PageCount; i++)
                    ((DropDownList)sender).Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
                ((DropDownList)sender).SelectedIndex = gvUsersList.PageIndex;
            }
        }
    }
}
