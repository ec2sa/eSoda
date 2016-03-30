using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.Tools;
using BarcodeToolkit;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;
using System.Drawing.Imaging;
using System.Web.Security;

namespace Pemi.Esoda.Web.UI.Dokumenty
{
    public partial class KodyDokumentu : System.Web.UI.Page
    {
        protected bool CzyPrzekazany;
        protected void Page_Load(object sender, EventArgs e)
        {
        
            int docID = CoreObject.GetId(Request);
            if (docID == 0)
                Response.Redirect("~/oczekujaceZadania.aspx",true);

         
            if (!IsPostBack)
            {
                XmlReader xr = new DocumentDAO().GetDataMatrix(docID);

                BarcodeData data=null;

                XmlSerializer serializer = new XmlSerializer(typeof(BarcodeData));
                try
                {
                    data = serializer.Deserialize(xr) as BarcodeData;
                }
                catch
                {
                    data = null;
                }
                if (data == null)
                    return;
                
                if(!data.SendDate.HasValue && string.IsNullOrEmpty(data.SentBy))
                {
                    Button1.Text = "Przekaż do wysłania";
                    Button1.CommandName = "Save";
                    statusWysylki.Text = "Dokument nie jest przekazany do wysłania.";
                  
                }
                if (!data.SendDate.HasValue && !string.IsNullOrEmpty(data.SentBy))
                {
                    Button1.Text = "Przekaż do wysłania";
                    Button1.CommandName = "Save";
                    statusWysylki.Text = "Dokument nie jest przekazany do wysłania.(Przekazanie anulował użytkownik: "+data.SentBy+")";

                }


                if (data.SendDate.HasValue && !string.IsNullOrEmpty(data.SentBy) && string.IsNullOrEmpty(data.RKWNumber))
                {
                   
                    Button1.Text = "Anuluj przekazanie do wysłania";
                    Button1.CommandName = "Cancel";
                    statusWysylki.Text = string.Format("Przekazano do wysłania dn. {0:yyyy-MM-dd} o godz. {0:HH:mm}. Użytkownik: {1}", data.SendDate, data.SentBy);
                
                }
                if (data.SendDate.HasValue && !string.IsNullOrEmpty(data.RKWNumber))
                {
                    Button1.Text = "Przekaż do wysłania";
                    Button1.Enabled = false;
                    statusWysylki.Text = string.Format("Zarejestrowano w Rejestrze Korespondencji Wychodzącej dn. {2:yyyy-MM-dd} o godz. {2:HH:mm}. Nr poz.: {0}. Użytkownik: {1}", data.RKWNumber, data.SentBy, data.SendDate);
                }

                BindBarcodeDataToForm(data);
                generatePreview(data);
                BindHistory(docID);
            }
            
         
        }

        private void BindHistory(int docID)
        {
            List<DataMatrixHistoryItem> historyItems= new DocumentDAO().GetDataMatrixHistory(docID);

            historyGrid.DataSource = historyItems;
            historyGrid.RowDataBound += new GridViewRowEventHandler(historyGrid_RowDataBound);
            historyGrid.DataBind();
        }

        void historyGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;
            IDataMatrixService service = new DataMatrixService();

            DataMatrixHistoryItem historyItem = e.Row.DataItem as DataMatrixHistoryItem;
            System.Web.UI.WebControls.Image codePreview = e.Row.FindControl("imgCode") as System.Web.UI.WebControls.Image;

            XmlSerializer serializer = new XmlSerializer(typeof(BarcodeData));
            BarcodeData data = serializer.Deserialize(new StringReader(historyItem.Content)) as BarcodeData;

            byte[] imgContent = service.GetDataMatrix(data, ImageFormat.Png);

            codePreview.ImageUrl = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(imgContent));

            (e.Row.FindControl("lName") as Literal).Text = data.Name;
            (e.Row.FindControl("lAddress") as Literal).Text = data.Address;
            (e.Row.FindControl("lPost") as Literal).Text = data.Post;
            (e.Row.FindControl("lAmount") as Literal).Text = data.Amount.ToString();
            (e.Row.FindControl("lReceiving") as Literal).Text = data.Receiving.ToString();
            (e.Row.FindControl("lNotes") as Literal).Text = data.Notes.IndexOf('|') >= 0 ? data.Notes.Remove(0, data.Notes.IndexOf('|') + 1) : data.Notes;
            (e.Row.FindControl("lDepartment") as Literal).Text = data.Department;

            Literal status =e.Row.FindControl("lStatusWysylki") as Literal; 

            //if (!data.SendDate.HasValue && string.IsNullOrEmpty(data.SentBy))
            //{
            //    status.Text = string.Format("");
            //}

            if (data.SendDate.HasValue && !string.IsNullOrEmpty(data.SentBy) && string.IsNullOrEmpty(data.RKWNumber))
            {
                status.Text = string.Format("Przekazano do wysyłki. Użytkownik: {0}", data.SentBy);
            }

            if (!data.SendDate.HasValue && !string.IsNullOrEmpty(data.SentBy))
            {
                status.Text = string.Format("Anulowano przekazanie do wysyłki. Użytkownik: {0}", data.SentBy);
            }


            if (data.SendDate.HasValue && !string.IsNullOrEmpty(data.SentBy) && !string.IsNullOrEmpty(data.RKWNumber))
            {
                status.Text = string.Format("Zarejestrowano w Rejestrze Korespondencji Wychodzącej dn. {2:yyyy-MM-dd} o godz. {2:HH:mm). Nr poz.: {0}. Użytkownik: {1}",data.RKWNumber, data.SentBy,data.SendDate);
            }
        }

        private void generatePreview(BarcodeData data)
        {
             IDataMatrixService service = new DataMatrixService();
             byte[] imgContent=service.GetDataMatrix(data, ImageFormat.Png);
             imgPreview.Src = string.Format("data:image/png;base64,{0}",Convert.ToBase64String(imgContent));
        }

        protected void btnGenerateOnly_click(object sender, EventArgs e)
        {
            int docID = CoreObject.GetId(Request);
            BarcodeData newData = GetBarcodeDataFromForm();

         

            //documentID temporary in notes!
            newData.Notes = docID.ToString() + "|" + newData.Notes;

            XmlSerializer serializer = new XmlSerializer(typeof(BarcodeData));
          
                newData.SendDate = null;
                newData.SentBy = null;
          
            using (Stream ms = new MemoryStream())
            {
                serializer.Serialize(ms, newData);
                ms.Seek(0, SeekOrigin.Begin);
                XmlReader xr = XmlReader.Create(ms);
                IDataMatrixService service = new DataMatrixService();

                new DocumentDAO().SetDataMatrix(docID, (Guid)Membership.GetUser().ProviderUserKey, xr,true);
                Response.Redirect(Request.Url.AbsoluteUri, true);
            }

        }

        protected void btnGenerate_click(object sender, EventArgs e)
        {
            int docID = CoreObject.GetId(Request);
            BarcodeData newData = GetBarcodeDataFromForm();

            newData.SentBy = Membership.GetUser().Comment;
          
                //documentID temporary in notes!
                newData.Notes = docID.ToString() + "|" + newData.Notes;

                XmlSerializer serializer = new XmlSerializer(typeof(BarcodeData));
                if (Button1.CommandName == "Cancel")
                {
                    newData.SendDate = null;
                }
                using (Stream ms = new MemoryStream())
                {
                    serializer.Serialize(ms, newData);
                    ms.Seek(0, SeekOrigin.Begin);
                    XmlReader xr = XmlReader.Create(ms);
                    IDataMatrixService service = new DataMatrixService();

                    new DocumentDAO().SetDataMatrix(docID, (Guid)Membership.GetUser().ProviderUserKey, xr);
                    Response.Redirect(Request.Url.AbsoluteUri, true);
                }

           
        }

        private BarcodeData GetBarcodeDataFromForm()
        {
            int tmp;
            BarcodeData newData = new BarcodeData();
            if (!int.TryParse(txtAmount.Text, out tmp))
                txtAmount.Text = "0";
            if (!int.TryParse(txtReceiving.Text, out tmp))
                txtReceiving.Text = "0";
            newData.Name = txtName.Text;
            newData.Address = txtAddress.Text;
            newData.Post = txtPost.Text;
            newData.Amount = int.Parse(txtAmount.Text);
            newData.Receiving = int.Parse(txtReceiving.Text);
            newData.Notes = txtNotes.Text;
            newData.Department = txtDepartment.Text;

            int additionFlags = 0;
          
            foreach (ListItem item in cbAdditions.Items)
            {
                if (item.Selected)
                {
                    additionFlags |= (int)Enum.Parse(typeof(Additions), item.Value);
                if((Additions)Enum.Parse(typeof(Additions), item.Value)==Additions.DOD_ZAGRANICZNY){
                    int zone = int.Parse(ddlZone.SelectedValue) << 16;
                    additionFlags += zone;
                }
                }
            }
            newData.SendDate = DateTime.Now;
            newData.Additions = additionFlags;
            return newData;
        }

        private void BindBarcodeDataToForm(BarcodeData data)
        {
            txtName.Text = data.Name;
            txtAddress.Text = data.Address;
            txtPost.Text = data.Post;
            txtAmount.Text = data.Amount.ToString();
            txtReceiving.Text = data.Receiving.ToString();
            txtNotes.Text = data.Notes.IndexOf('|')>=0? data.Notes.Remove(0,data.Notes.IndexOf('|')+1):data.Notes;
            txtDepartment.Text = data.Department;

            IList<AdditionItem> additionsItems = AddidtionsList.GetAll(data.Additions);
            int zone =(data.Additions >> 16);
            if (zone > 0)
            {
                ddlZone.Enabled = true;
                ddlZone.SelectedValue = (zone & 3).ToString();
            }

            cbAdditions.DataSource = additionsItems;
            cbAdditions.DataTextField = "Label";
            cbAdditions.DataValueField = "FlagValue";
            cbAdditions.DataBind();
            foreach (AdditionItem item in additionsItems)
            {
                ListItem li = cbAdditions.Items.FindByValue(item.FlagValue.ToString());
                if (li != null)
                {
                    li.Selected = item.Checked;
                }
            }
        }

        protected void lnkGetAsPdf_click(object sender, EventArgs e)
        {
            BarcodeData data = GetBarcodeDataFromForm();
            IDataMatrixService service = new DataMatrixService();
            string[] address = new string[3];
            address[0] = txtName.Text;
            address[1] = txtAddress.Text;
            address[2] = txtPost.Text;

            byte[] content = service.GetDataMatrixWithAddressAsPdf(data,address);
            if (content.Length == 0)
                return;
            Transmit(content, "doc_" + CoreObject.GetId(Request).ToString() + "_DataMatrix.pdf");
        }

        private void Transmit(byte[] content, string fileName)
        {
            Response.Clear();
            Response.Charset = "UTF-8";
            Response.ContentType = "application/pdf";
            if (Request.Browser.VBScript)
                Response.AppendHeader("content-disposition", string.Format("attachment; filename=\"{0}\"", HttpUtility.UrlEncode(fileName).Replace("+", " ")));
            else
                Response.AppendHeader("content-disposition", string.Format("attachment; filename=\"{0}\"", fileName));

            Response.BinaryWrite(content);
            Response.Flush();

        }

        
    }
}
