using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Xml;
using Pemi.Esoda.DataAccess;
using BarcodeToolkit;
using System.Xml.Serialization;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI
{
    public partial class DataMatrix : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
            try
            {
            int docID;
            if (!int.TryParse(Request.QueryString["id"], out docID))
                throw new Exception("Brak ID w wywołaniu");

           
                XmlReader xr = new DocumentDAO().GetDataMatrix(docID);

                BarcodeData data;

                XmlSerializer serializer = new XmlSerializer(typeof(BarcodeData));
                data = serializer.Deserialize(xr) as BarcodeData;

                if (data == null)
                    return;

                IDataMatrixService service = new DataMatrixService();
                string content = service.GetDataMatrixAsHtml(data);

                Response.ClearContent();
                Response.Write(content);
                Response.Flush();
            }
            catch (Exception ex)
            {
                Response.Redirect("~/shared/error.aspx");
            }
        }
    }
}
