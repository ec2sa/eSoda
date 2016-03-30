using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Pemi.Esoda.DTO;
using AjaxControlToolkit;
using System.Text;


namespace Pemi.Esoda.Web.UI
{
    public partial class eSodaChatUsers : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //Users.DataSource = Membership.GetAllUsers();
            //Users.DataBind();
            //UsersCls users = new UsersCls();
            //Users.DataSource = users.Users;
            //Users.DataBind();

            //if (!Page.IsPostBack)
            //{
            //    ScriptManager sm = ScriptManager.GetCurrent(this.Page);
            //    sm.Scripts.Add(new ScriptReference("/JSONTest.js"));
            //    sm.Services.Add(new ServiceReference("/services/ChatService.asmx"));
            //}
        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string GetDynamicContent(string contextKey)
        {
            StringBuilder sTemp = new StringBuilder();

            MembershipUser us = Membership.GetUser(new Guid(contextKey));
            string style = @"border: #060F40 1px solid;
                              color: #060F40;
                              background: #ffffcc;
                              padding:5px;";

                if (us.IsOnline)
                    sTemp.Append(@"<div style=""" + style + @""">Dostępny</div>");
                else
                    sTemp.Append(@"<div style=""" + style + @""">Niedostępny</div>");

              

            return sTemp.ToString(); 
        }

        protected void Users_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Programmatically reference the PopupControlExtender
                PopupControlExtender pce = e.Row.FindControl("PopupControlExtender1") as PopupControlExtender;

                // Set the BehaviorID
                string behaviorID = string.Concat("pce", e.Row.RowIndex);
                pce.BehaviorID = behaviorID;

                // Programmatically reference the Image control
                //Image i = (Image)e.Row.Cells[1].FindControl("Image1");

                System.Web.UI.WebControls.Image i = (System.Web.UI.WebControls.Image)e.Row.Cells[1].FindControl("Image1");
                
                // Add the clie nt-side attributes (onmouseover & onmouseout)
                string OnMouseOverScript = string.Format("$find('{0}').showPopup();", behaviorID);
                string OnMouseOutScript = string.Format("$find('{0}').hidePopup();", behaviorID);

                i.Attributes.Add("onmouseover", OnMouseOverScript);
                i.Attributes.Add("onmouseout", OnMouseOutScript);
            }
        } 
    }
}
