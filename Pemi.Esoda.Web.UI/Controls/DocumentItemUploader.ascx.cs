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
using System.IO;
using System.Collections.Generic;
using Pemi.Esoda.DTO;
using System.Data.Common;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class DocumentItemUploader : System.Web.UI.UserControl
    {
        public string temporaryFileName
        {
            get
            {
                if (Session["{A71C9EAF-D6D8-40a8-8E16-A4BAE8BE970B}"] == null) return string.Empty;
                return Session["{A71C9EAF-D6D8-40a8-8E16-A4BAE8BE970B}"].ToString();
            }
            set
            {
                if (value != null)
                    Session["{A71C9EAF-D6D8-40a8-8E16-A4BAE8BE970B}"] = value;
                else
                    Session.Remove("{A71C9EAF-D6D8-40a8-8E16-A4BAE8BE970B}");
            }
        }

        public int IdDokumentu
        {
            get
            {
                //int id;
                //if (Session["idDokumentu"] == null)
                //    return 0;
                //if (!int.TryParse(Session["idDokumentu"].ToString(), out id))
                //    return 0;
                //return id;
                return CoreObject.GetId(Request);
            }
            set
            {
                Session["idDokumentu"] = value;
            }
        }

        public Guid IdOryginalu
        {
            get
            {
                if (Session["originalItemId"] == null)
                    return Guid.Empty;
                return new Guid(Session["originalItemId"].ToString());
            }
            set
            {
                if (value != Guid.Empty)
                    Session["originalItemId"] = value;
                else
                    Session.Remove("originalItemId");
            }
        }

        public event EventHandler CancelUpload;

        public void OnCancelUpload(EventArgs e)
        {
            if (CancelUpload != null)
                CancelUpload(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdOryginalu = Guid.Empty;
                views.ActiveViewIndex = 0;
            }
        }

        protected void pobierz_Click(object sender, EventArgs e)
        {
            if (fileToUpload.HasFile)
            {
                views.ActiveViewIndex = 1;
                fileName.Text = fileToUpload.FileName;
                uploadResult.Text = "Plik zosta³ poprawnie pobrany i jest gotowy do zapisania w systemie";
                uploadedFileSize.Text = ((decimal)fileToUpload.FileContent.Length / 1024).ToString("#.##") + " KB";
                temporaryFileName = Guid.NewGuid().ToString();
                mimeType.Text = fileToUpload.PostedFile.ContentType;
                fileToUpload.PostedFile.SaveAs(Pemi.Esoda.Tools.Configuration.PhysicalTemporaryDirectory + "\\" + temporaryFileName);

                CheckIfDocumentItemExists();

            }
        }

        private void CheckIfDocumentItemExists()
        {
            // Guid originalGuid = new DocumentDAO().CheckIfDocumentItemExists(IdDokumentu.ToString(), fileToUpload.FileName);

            if (IdOryginalu != Guid.Empty)
            {
                forceUpdate.Visible = true;
                forceUpdate.Enabled = true;
                forceUpdate.Checked = true;
                return;
            }
            //if (originalGuid != Guid.Empty)
            //{
            //  forceUpdate.Visible = true;
            //  forceUpdate.Enabled = true;
            //  forceUpdate.Checked = false;
            //  IdOryginalu = originalGuid;
            //}
            //else
            // {
            forceUpdate.Visible = false;
            // }

        }

        protected void anuluj_Click(object sender, EventArgs e)
        {
            views.ActiveViewIndex = 0;
            IdOryginalu = Guid.Empty;
            DeleteTemporaryFile();
            OnCancelUpload(new EventArgs());
        }

        protected void zapisz_Click(object sender, EventArgs e)
        {
            DocumentDAO dao = new DocumentDAO();
            FileStream fs = File.OpenRead(Pemi.Esoda.Tools.Configuration.PhysicalTemporaryDirectory + "\\" + temporaryFileName);
            if (forceUpdate.Visible && forceUpdate.Checked)
            {
                dao.AddNewVersionOfDocumentItem(IdDokumentu, IdOryginalu, fileDescription.Text, fs, mimeType.Text, fileName.Text, DocumentItemCategory.Uploaded);
                IdOryginalu = Guid.Empty;
            }
            else
            {
                Guid itemId = Guid.Empty;
                dao.AddNewDocumentItem(IdDokumentu, fileName.Text, fileDescription.Text, fs, mimeType.Text, ref itemId,DocumentItemCategory.Uploaded);
                //int docId = dao.AddNewDocument(new Guid(Membership.GetUser().ProviderUserKey.ToString()), md.GetXml());

                DocumentItemDTO docItem = dao.GetItem(itemId);
                string imie = String.Empty, nazwisko=string.Empty;
                DbDataReader dr = (DbDataReader)(new UserDAO()).GetEmployee(new Guid(Membership.GetUser().ProviderUserKey.ToString()));
                if (dr.Read())
                {
                    imie = dr["imie"].ToString();
                    nazwisko = dr["nazwisko"].ToString();
                }
                dr.Close();

                List<string> paramList = new List<string>();
                paramList.Add(fileName.Text);
                ActionLogger al = new ActionLogger(new ActionContext(new Guid("9cd585bb-2a06-4c24-b415-aa3f8b00ea5f"), new Guid(Membership.GetUser().ProviderUserKey.ToString()), Membership.GetUser().UserName, Membership.GetUser().Comment, paramList));

                al.AppliesToDocuments.Add(IdDokumentu);
                al.ActionData.Add("idDokumentu", IdDokumentu.ToString());
                al.ActionData.Add("idPracownika", Membership.GetUser().UserName);
                al.ActionData.Add("imie", imie);
                al.ActionData.Add("nazwisko", nazwisko);
                al.ActionData.Add("dataDodania", docItem.CreationDate.ToString());
                al.ActionData.Add("idPliku", docItem.ID.ToString());
                al.ActionData.Add("nazwaPliku", fileName.Text);

                /* Nr systemowy dokumentu logicznego
                   Id pracownika (login) (który dokonuje operacji do³¹czenia)
                   imie pracownika
                   nazwisko pracownika, 
                   Data dolaczenia pliku
                   Czas dolaczenia pliku
                   Id systemowe (guid?) dolaczanego pliku
                   Nazwa do³¹czanego pliku*/


                al.Execute();
            }
            fs.Close();
            DeleteTemporaryFile();

            Response.Redirect("~/Dokumenty/SkladnikiDokumentu.aspx?id="+CoreObject.GetId(Request).ToString());
        }

        private void DeleteTemporaryFile()
        {
            if (File.Exists(Pemi.Esoda.Tools.Configuration.PhysicalTemporaryDirectory + "\\" + temporaryFileName))
                File.Delete(Pemi.Esoda.Tools.Configuration.PhysicalTemporaryDirectory + "\\" + temporaryFileName);
        }

        protected void changeFile_Click(object sender, EventArgs e)
        {
            views.ActiveViewIndex = 0;
            DeleteTemporaryFile();
        }
    }
}