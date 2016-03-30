using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.eSoda.CustomForms.Inferfaces;
using System.Xml;
using Pemi.eSoda.CustomForms;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Pemi.Esoda.Web.UI
{
    public partial class CustomFormWrapperControl : System.Web.UI.UserControl,ICustomForm
    {
        private string _assemblyName;
        
        private string _resourceName;

        protected override object SaveControlState()
        {
            object obj = base.SaveControlState();
            return new object[] { obj, _assemblyName, _resourceName };

        }

        protected override void LoadControlState(object state)
        {
            object[] s = state as object[];
            if (s != null)
            {
                base.LoadControlState(s[0]);
                _assemblyName = (string)s[1];
                _resourceName = (string)s[2];
            }
            else
            {
                _assemblyName = string.Empty;
                _resourceName = string.Empty;
            }
        }

        public ICustomForm CustomForm
        {
            get
            {
                ICustomForm icf = formPlaceholder.FindControl("customFormControl") as ICustomForm;
                return icf;
            }
        }

        public bool IsEditable
        {
            set
            {
                lnkEdit.Visible = value;
            }
        }

        private string getASCXContent()
        {
            if (!string.IsNullOrEmpty(_assemblyName) && !string.IsNullOrEmpty(_resourceName))
            {           
                return string.Format("~/CustomForms/{0}/{1}.ascx", _assemblyName, _resourceName);
            }
            return null;
        }

        private string getHashString(byte[] hash)
        {
            if (hash == null)
                return null;
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

        private string getPhysicalFileHash()
        {
            string assemblyPath = Path.Combine(HttpRuntime.BinDirectory, string.Format(@"{1}\{0}.dll", _assemblyName, _assemblyName));
            byte[] hash = null;
            using (FileStream fs = File.OpenRead(assemblyPath))
            {
                hash = MD5.Create().ComputeHash(fs);
            }
            return getHashString(hash);

        }

        protected void Page_Init(object sender, EventArgs e)
        {
        
            Page.RegisterRequiresControlState(this);

            
        }

        void CustomFormWrapperControl_ContentSaving(object sender, EventArgs e)
        {
          
                    if (ContentSaving != null)
                    {
                        ContentSaving(this, new EventArgs());
                    }
               
            
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;
            try
            {
                string virtualPath = getASCXContent();
                if (virtualPath != null)
                {
                    Control c = LoadControl(virtualPath);
                    if (c as ICustomForm == null)
                        throw new ArgumentException("Invalid control - doesn't implement ICustomForm interface");
                    c.ID = "customFormControl";
                    (c as ICustomForm).ContentSaving += new EventHandler(CustomFormWrapperControl_ContentSaving);
                    formPlaceholder.Controls.Add(c);
                }
                if (CustomForm != null)
                {
                    //CustomForm.DisplayMode = CustomFormDisplayMode.Insert;
                    lblDocumentType.Text = "Zawartość formularza";


                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            if(CustomForm!=null)
            CustomForm.DisplayMode = CustomFormDisplayMode.Edit;
        }

        public void LoadContent(string assemblyName, string resourceName,string formHash)
        {
            _assemblyName = assemblyName;
            _resourceName = resourceName;
            if (getPhysicalFileHash() != formHash)
            {
             
                throw new ArgumentException("Form assembly has been changed outside eSoda application.");
            }
        }

        #region ICustomForm Members

        public event EventHandler ContentSaving;

        public CustomFormDisplayMode DisplayMode
        {
            get
            {
                if (CustomForm != null)
                    return CustomForm.DisplayMode;
                else
                    return CustomFormDisplayMode.Insert;
            }
            set
            {
                if (CustomForm != null)
                {
                    CustomForm.DisplayMode = value;
                   
                }

            }
        }

        public XmlDocument GetFormContent()
        {
            if (CustomForm != null)
                return CustomForm.GetFormContent();
            else
                return null;
        }

        public bool IsFormContentValid
        {
            get
            {
                if (CustomForm != null)
                    return CustomForm.IsFormContentValid;
                else
                    return false;
            }
        }

        public void SetFormContent(XmlDocument formContent)
        {
            if (CustomForm != null)
                CustomForm.SetFormContent(formContent);
        }

        public bool HasContent
        {
            get
            {
                if (CustomForm != null)
                    return CustomForm.HasContent;
                else
                    return false;
            }
        }

        #endregion
    }
}