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

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class TypInteresanta : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int Columns
        {
            set { this.rblTypInteresanta.RepeatColumns = value; }
        }

        public int SelectedType
        {
            get
            {
                int typeId = -1;
                if (int.TryParse(rblTypInteresanta.SelectedValue, out typeId))
                    return typeId;
                else
                    return -1;
            }                    
        }

        public bool OsobaFizyczna
        {
            set
            {
                rblTypInteresanta.Items.FindByValue("1").Enabled = value;
            }
        }

        public event EventHandler TypeChanged
        {
            add { rblTypInteresanta.SelectedIndexChanged += value; }
            remove { rblTypInteresanta.SelectedIndexChanged -= value; }
        }

        protected void rblTypInteresanta_DataBound(object sender, EventArgs e)
        {
            ListItem item = new ListItem("Wszyscy", "-1");
            item.Selected = true;
            ((RadioButtonList)sender).Items.Insert(0, item);
            ((RadioButtonList)sender).Items.FindByValue("1").Enabled = false;
        }

        protected void rblTypInteresanta_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (TypeChanged != null)
            //    TypeChanged(sender, e);
        }
    }
}