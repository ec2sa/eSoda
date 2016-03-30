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
using Pemi.Esoda.DTO;
using System.IO;
using System.Text;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class rodzajeDokumentow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ClientScript.IsStartupScriptRegistered(GetType(), "FormActive"))
            {
                string script = @"                                                               
                                function CustomFormActive(fileObjId, checkObjId, originalNameObjId)
                                {		                                	                                                                                                    
                                    var oriFileName = document.getElementById(originalNameObjId) != null ? document.getElementById(originalNameObjId).innerHTML : '';
                                    
	                                if (document.getElementById(fileObjId).value.length > 0 || oriFileName.length > 0)
                                    {		                                
                                        document.getElementById(checkObjId).checked=true;
                                    }
	                                else
                                        document.getElementById(checkObjId).checked=false;
                                }

                                function WordFormActive(wordObjId, schemaObjId, activeObjId, oWordNameObjId, oSchemaNameObjId)
                                {		                                                           
                                    var oriWord = document.getElementById(oWordNameObjId) != null ? document.getElementById(oWordNameObjId).innerHTML : '';
                                    var oriSchema = document.getElementById(oSchemaNameObjId) != null ? document.getElementById(oSchemaNameObjId).innerHTML : '';

	                                if ((document.getElementById(wordObjId).value.length > 0 || oriWord.length > 0) && (document.getElementById(schemaObjId).value.length > 0 || oriSchema.length > 0))      
                                    {
                                        document.getElementById(activeObjId).checked=true;
                                    }
                                    else document.getElementById(activeObjId).checked=false;
                                }

                                ";

                ClientScript.RegisterStartupScript(GetType(), "FormActive", script, true);
            }

            lblMessage.Text = string.Empty;

            if (!Page.IsPostBack)
            {
                // frmDocCategories.DataSource = (new DocumentDAO()).GetDocumentCategories();
                // frmDocCategories.DataBind();
            }


            //lnkEditCat.Enabled = ddlDocCatList.Items.Count > 0;
            //pnlDocTypes.Visible = ddlDocCatList.Items.Count > 0;
            int catId = -1;
            if (ddlDocCatList.SelectedIndex > -1)
            {

                if (int.TryParse(ddlDocCatList.SelectedItem.Value, out catId))
                {
                    lblCategoryName.Text = ddlDocCatList.SelectedItem.Text;
                }
            }
        }

        public string APath
        {
            get
            {
                return Page.Request.ApplicationPath == "/" ? "" : Page.Request.ApplicationPath;
            }
        }

        private void LoadDocCategories()
        {
            //DropDownList ddlDocCatList = frmDocCategories.FindControl("ddlDocCatList") as DropDownList;
            //if (ddlDocCatList != null)
            //{
            //    ddlDocCatList.DataSource = (new DocumentDAO()).GetDocumentCategories();
            //    ddlDocCatList.DataTextField = "nazwa";
            //    ddlDocCatList.DataValueField = "id";
            //    ddlDocCatList.DataBind();
            //}
        }

        protected void frmDocCategories_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            switch (e.NewMode)
            {
                case FormViewMode.Edit:
                    int catId = int.Parse(ddlDocCatList.SelectedItem.Value);
                    frmDocCategories.DataSource = (new DocumentDAO()).GetCategory(catId);
                    frmDocCategories.DataBind();
                    break;

                case FormViewMode.ReadOnly:
                    frmDocCategories.Visible = false;
                    pnlDocCatList.Visible = true;
                    break;

                case FormViewMode.Insert:
                    break;
                default: break;
            }
            frmDocCategories.ChangeMode(e.NewMode);
        }

        protected void frmDocCategories_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (Page.IsValid)
            {
                DocumentDAO dd = new DocumentDAO();
                string nazwa = ((TextBox)frmDocCategories.FindControl("txtNazwa")).Text;
                string skrot = ((TextBox)frmDocCategories.FindControl("txtSkrot")).Text;
                bool aktywny = ((CheckBox)frmDocCategories.FindControl("ckbActive")).Checked;
                if (dd.AddDocCategory(nazwa, skrot, aktywny))
                {
                    frmDocCategories.ChangeMode(FormViewMode.ReadOnly);
                    ddlDocCatList.DataBind();
                    frmDocCategories.Visible = false;
                    pnlDocCatList.Visible = true;
                }
                else
                {
                    WebMsgBox.Show(this, "Nie uda³o siê dodaæ kategorii dokumentu");
                }
            }
        }

        protected void frmDocCategories_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                DocumentDAO dd = new DocumentDAO();
                int id = int.Parse(ddlDocCatList.SelectedItem.Value);
                string nazwa = ((TextBox)frmDocCategories.FindControl("txtNazwa")).Text;
                string skrot = ((TextBox)frmDocCategories.FindControl("txtSkrot")).Text;
                bool aktywny = ((CheckBox)frmDocCategories.FindControl("ckbActive")).Checked;
                dd.UpdateDocCategory(id, nazwa, skrot, aktywny);
                frmDocCategories.ChangeMode(FormViewMode.ReadOnly);
                frmDocCategories.Visible = false;
                pnlDocCatList.Visible = true;
                ddlDocCatList.DataBind();
            }
        }

        protected void btnAddCat_Click(object sender, EventArgs e)
        {
            //if (ddlDocCatList.SelectedIndex > -1)
            //{
            frmDocCategories.ChangeMode(FormViewMode.Insert);
            pnlDocCatList.Visible = false;
            frmDocCategories.Visible = true;
            //}
            //else
            //    WebMsgBox.Show(this, "Wybierz kategoriê");
        }

        protected void btnEditCat_Click(object sender, EventArgs e)
        {
            if (ddlDocCatList.SelectedIndex > -1)
            {
                frmDocCategories.ChangeMode(FormViewMode.Edit);
                int catId = int.Parse(ddlDocCatList.SelectedItem.Value);
                frmDocCategories.DataSource = (new DocumentDAO()).GetCategory(catId);
                frmDocCategories.DataBind();
                pnlDocCatList.Visible = false;
                frmDocCategories.Visible = true;
            }
            else
                WebMsgBox.Show(this, "Wybierz kategoriê");
        }

        protected void lnkAddDocType_Click(object sender, EventArgs e)
        {
            frmDocTypes.ChangeMode(FormViewMode.Insert);
            //frmDocTypes.Visible = true;
            panelDocTypes.Visible = true;
            frmDocCategories.ChangeMode(FormViewMode.ReadOnly);
            frmDocCategories.Visible = false;
        }

        protected void frmDocTypes_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (Page.IsValid)
            {
                bool allowOverwrite = false;

                try
                {
                    string nazwa = ((TextBox)frmDocTypes.FindControl("txtNazwa")).Text;
                    string skrot = ((TextBox)frmDocTypes.FindControl("txtSkrot")).Text;
                    int catId = int.Parse(ddlDocCatList.SelectedItem.Value);
                    bool isActive = ((CheckBox)frmDocTypes.FindControl("cbIsActive")).Checked;
                    bool isLegalAct= ((CheckBox)frmDocTypes.FindControl("cbAktPrawny")).Checked;
                    bool isOverwrite = false;
                    string className = null;
                    string description = ((TextBox)frmDocTypes.FindControl("tbDescription")).Text;
                    string oryginalFileName = null;
                    string fileName = null;

                    FileUpload fu = (FileUpload)frmDocTypes.FindControl("uploadControl");

                    //form:
                    FileUpload wordForm = (FileUpload)frmDocTypes.FindControl("wordForm");
                    FileUpload wordSchema = (FileUpload)frmDocTypes.FindControl("wordSchema");

                    //form: false
                    bool isWordFormActive = ((CheckBox)frmDocTypes.FindControl("wordFormActive")).Checked;
                    string wordOriginalFilename = null;
                    string wordFilename = null;
                    string wordSchemaFilename = null;
                    string wordSchemaOriginalFilename = null;

                    if (fu != null && fu.HasFile)
                    {
                        oryginalFileName = Path.GetFileName(fu.PostedFile.FileName);
                        fileName = SaveFile(fu);
                    }

                    //form:
                    if (wordForm != null && wordForm.HasFile)
                    {
                        wordOriginalFilename = Path.GetFileName(wordForm.PostedFile.FileName);
                        wordFilename = SaveFile(wordForm);
                    }

                    if (wordSchema != null && wordSchema.HasFile)
                    {
                        wordSchemaOriginalFilename = Path.GetFileName(wordSchema.PostedFile.FileName);
                        wordSchemaFilename = SaveFile(wordSchema);
                    }

                    CustomFormDTO customForm = new CustomFormDTO(0, catId, nazwa, skrot, true,isLegalAct, fileName, oryginalFileName, isActive, className, description, wordFilename, wordOriginalFilename, wordSchemaFilename, wordSchemaOriginalFilename, isWordFormActive);

                    ViewState["CustomForm"] = customForm;

                    CustomFormDAO cfdao = new CustomFormDAO(customForm);
                    
                    cfdao.Save(isOverwrite, out allowOverwrite);

                    frmDocTypes.ChangeMode(FormViewMode.ReadOnly);
                    panelDocTypes.Visible = false;
                    gvDocTypes.DataBind();
                }
                catch (Exception ex)
                {
                    if (allowOverwrite)
                    {
                        panelOverwrite.Visible = true;
                        panelDocTypes.Visible = false;
                        lblOverwriteMessage.Text = ex.Message;
                    }
                    else
                    {
                        lblMessage.Text = ex.Message;
                        panelOverwrite.Visible = false;
                    }
                }
            }
        }
        
        protected void frmDocTypes_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                bool allowOverwrite = false;
                try
                {
                    string nazwa = ((TextBox)frmDocTypes.FindControl("txtNazwa")).Text;
                    string skrot = ((TextBox)frmDocTypes.FindControl("txtSkrot")).Text;
                    int catId = int.Parse(ddlDocCatList.SelectedItem.Value);
                    int docId = int.Parse(gvDocTypes.SelectedDataKey.Value.ToString());

                    bool isActive = ((CheckBox)frmDocTypes.FindControl("cbIsActive")).Checked;
                    bool isLegalAct = ((CheckBox)frmDocTypes.FindControl("cbAktPrawny")).Checked;
                    bool isOverwrite = false;
                    string className = null;
                    string description = ((TextBox)frmDocTypes.FindControl("tbDescription")).Text;
                    string oryginalFileName = null;
                    string fileName = null;

                    //form: false;//
                    bool isWordFormActive = ((CheckBox)frmDocTypes.FindControl("wordFormActive")).Checked;
                    string wordOriginalFilename = null;
                    string wordFilename = null;
                    string wordSchemaFilename = null;
                    string wordSchemaOriginalFilename = null;

                    FileUpload fu = (FileUpload)frmDocTypes.FindControl("uploadControl");

                    //form: 
                    FileUpload wordForm = (FileUpload)frmDocTypes.FindControl("wordForm");
                    FileUpload wordSchema = (FileUpload)frmDocTypes.FindControl("wordSchema");

                    if (fu != null && fu.HasFile)
                    {
                        oryginalFileName = Path.GetFileName(fu.PostedFile.FileName);
                        fileName = SaveFile(fu);
                    }

                    //form: 
                    if (wordForm != null && wordForm.HasFile)
                    {
                        wordOriginalFilename = Path.GetFileName(wordForm.PostedFile.FileName);
                        wordFilename = SaveFile(wordForm);
                    }

                    if (wordSchema != null && wordSchema.HasFile)
                    {
                        wordSchemaOriginalFilename = Path.GetFileName(wordSchema.PostedFile.FileName);
                        wordSchemaFilename = SaveFile(wordSchema);
                    }

                    CustomFormDTO customForm = new CustomFormDTO(docId, catId, nazwa, skrot, true,isLegalAct, fileName, oryginalFileName, isActive, className, description, wordFilename, wordOriginalFilename, wordSchemaFilename, wordSchemaOriginalFilename, isWordFormActive);

                    ViewState["CustomForm"] = customForm;

                    CustomFormDAO cfdao = new CustomFormDAO(customForm);
                    
                    cfdao.Save(isOverwrite, out allowOverwrite);
                    
                    panelDocTypes.Visible = false;
                    gvDocTypes.DataBind();
                }
                catch (Exception ex)
                {
                    if (allowOverwrite)
                    {
                        panelOverwrite.Visible = true;
                        panelDocTypes.Visible = false;
                        lblOverwriteMessage.Text = ex.Message;
                    }
                    else
                    {
                        lblMessage.Text = ex.Message;
                        panelOverwrite.Visible = false;
                    }
                }
            }
        }

        private string SaveFile(FileUpload fu)
        {
            string ext = Path.GetExtension(fu.PostedFile.FileName);

            string temporaryFileName = string.Empty;

            try
            {
                temporaryFileName = String.Concat(Guid.NewGuid().ToString(), ext);
                fu.SaveAs(Server.MapPath(String.Concat(this.APath, "/temp/", temporaryFileName)));
            }
            catch (HttpException ex)
            {
                throw new Exception(ex.Message);
            }

            return temporaryFileName;
        }

        protected void gvDocTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int catID = int.Parse(ddlDocCatList.SelectedItem.Value);
            int docId = int.Parse(gvDocTypes.SelectedDataKey.Value.ToString());
            frmDocTypes.ChangeMode(FormViewMode.Edit);
            panelDocTypes.Visible = true;
            //frmDocTypes.Visible = true;
            frmDocTypes.DataSource = (new DocumentDAO()).GetDocTypeForCategory(catID, docId);
            frmDocTypes.DataBind();


        }

        protected void ddlDocCatList_DataBound(object sender, EventArgs e)
        {
            lnkEditCat.Enabled = ddlDocCatList.Items.Count > 0;
            pnlDocTypes.Visible = ddlDocCatList.Items.Count > 0;
            int catId = -1;
            if (ddlDocCatList.SelectedIndex > -1)
            {
                if (int.TryParse(ddlDocCatList.SelectedItem.Value, out catId))
                {
                    lblCategoryName.Text = ddlDocCatList.SelectedItem.Text;
                }
            }
        }

        protected void frmDocTypes_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            frmDocTypes.ChangeMode(e.NewMode);
        }

        protected void frmDocTypes_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Cancel"))
            {
                frmDocTypes.ChangeMode(FormViewMode.ReadOnly);
                panelDocTypes.Visible = false;
            }
        }

        protected void ddlDocCatList_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelDocTypes.Visible = false;
        }

        protected void btnOverwrite_Click(object sender, EventArgs e)
        {
            if (ViewState["CustomForm"] != null)
            {
                try
                {
                    bool allowOverwrite;

                    CustomFormDTO customForm = (CustomFormDTO)ViewState["CustomForm"];
                    CustomFormDAO cfdao = new CustomFormDAO(customForm);

                    cfdao.Save(true, out allowOverwrite);
                    panelOverwrite.Visible = false;
                    gvDocTypes.DataBind();
                }
                catch (Exception ex)
                {
                    this.lblMessage.Text = ex.Message;
                    panelOverwrite.Visible = false;
                }
            }
        }

        protected void btnOverwriteCancel_Click(object sender, EventArgs e)
        {
            panelOverwrite.Visible = false;
            panelDocTypes.Visible = true;
        }

        protected void frmDocTypes_PreRender(object sender, EventArgs e)
        {
            if (((FormView)sender).FindControl("cbIsActive") != null)
            {
                string isActiveID = ((FormView)sender).FindControl("cbIsActive").ClientID;
                string uploadControlID = ((FormView)sender).FindControl("uploadControl").ClientID;
                string originalName = originalName = ((FormView)sender).FindControl("lblOryginalName").ClientID;

                ((FileUpload)((FormView)sender).FindControl("uploadControl")).Attributes.Add("onChange", "CustomFormActive('" + uploadControlID + "','" + isActiveID + "','" + originalName + "')");

                // form:
                string wordActiveID = ((FormView)sender).FindControl("wordFormActive").ClientID;
                string wordFormID = ((FormView)sender).FindControl("wordForm").ClientID;
                string wordSchemaID = ((FormView)sender).FindControl("wordSchema").ClientID;
                string wordOriginalID = ((FormView)sender).FindControl("lblWordOriginal").ClientID;
                string wordSchemaOriginalID = ((FormView)sender).FindControl("lblWordSchemaOriginal").ClientID;

                ((FileUpload)((FormView)sender).FindControl("wordForm")).Attributes.Add("onChange", "WordFormActive('" + wordFormID + "','" + wordSchemaID + "','" + wordActiveID + "','" + wordOriginalID + "','" + wordSchemaOriginalID + "')");
                ((FileUpload)((FormView)sender).FindControl("wordSchema")).Attributes.Add("onChange", "WordFormActive('" + wordFormID + "','" + wordSchemaID + "','" + wordActiveID + "','" + wordOriginalID + "','" + wordSchemaOriginalID + "')");

            }
        }

        protected void wordValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            FileUpload wordForm = ((FileUpload)((FormView)frmDocTypes).FindControl("wordForm"));
            FileUpload wordSchema = ((FileUpload)((FormView)frmDocTypes).FindControl("wordSchema"));
            Label wordOriginal = (Label)(((FormView)frmDocTypes).FindControl("lblWordOriginal"));
            Label wordSchemaOriginal = (Label)(((FormView)frmDocTypes).FindControl("lblWordSchemaOriginal"));

            //&& wordOriginal != null && wordSchemaOriginal != null

            if (wordForm != null && wordSchema != null)
            {
                //if ((!String.IsNullOrEmpty(wordForm.PostedFile.FileName) || !String.IsNullOrEmpty(wordOriginal.Text)) && (!String.IsNullOrEmpty(wordSchema.PostedFile.FileName) || !String.IsNullOrEmpty(wordSchemaOriginal.Text)))
                if (!String.IsNullOrEmpty(wordForm.PostedFile.FileName) && !String.IsNullOrEmpty(wordSchema.PostedFile.FileName))
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                }

            }
        }
    }
}