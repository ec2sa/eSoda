using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Text;
using System.IO;
using System.Web.Hosting;

[WebService(Namespace = "http://pemi.esoda.esp/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class ESPWebService : System.Web.Services.WebService
{
    public ESPWebService ()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

//      <?xml version="1.0" encoding="utf-8" ?>
//      <dokumenty>
//          <dokument id="{DB0CDBA6-4018-4c0e-8C0C-27ADB57586F2}" nazwa="Dokument1" opis="Dokument pierwszy" />
//          ....
//      </dokumenty>

    private string GenerateDocumentsList()
    {
        Random rnd = new Random(DateTime.Now.Millisecond);
        StringBuilder sb = new StringBuilder();
        XmlWriter xw = XmlWriter.Create(sb);

        xw.WriteStartElement("dokumenty");

        //for (int i = 0; i < rnd.Next() % 10; i++)
        //{
            string docGuid = Guid.NewGuid().ToString();
            xw.WriteStartElement("dokument");
            xw.WriteAttributeString("id", docGuid);
            xw.WriteAttributeString("nazwa", string.Format("Dokument nr {0}", "1"));
            xw.WriteAttributeString("opis", string.Format("Dokument o ID={0}", docGuid));
            xw.WriteEndElement();
        //}

        xw.WriteEndElement();
        xw.Close();

        return sb.ToString();
    }

    [WebMethod]
    public string GetAwaitingDocuments()
    {
        return GenerateDocumentsList();
    }

    [WebMethod]
    public string GetAwaitingDocumentsFromLocation(Guid locationId)
    {
        return GenerateDocumentsList();
    }

    byte[] GetStreamData(FileStream fs)
    {
        byte[] data = new byte[fs.Length];
        fs.Read(data, 0, (int)fs.Length);
        fs.Close();
        return data;
    }

    [WebMethod]
    public string GetDocument(Guid docId)
    {
        FileStream xmlBody = new FileStream(HostingEnvironment.ApplicationPhysicalPath + "/Wniosek.xml", FileMode.Open);
        FileStream xslStyle = new FileStream(HostingEnvironment.ApplicationPhysicalPath + "/Styl.xslt", FileMode.Open);
        FileStream att1 = new FileStream(HostingEnvironment.ApplicationPhysicalPath + "/Active.pdf", FileMode.Open);


        StringBuilder sb = new StringBuilder();
        XmlWriter xw = XmlWriter.Create(sb);

        xw.WriteStartElement("eDokument");

            // pierwszy element sterujący
            xw.WriteStartElement("ePaczka");
                
                // dane XML, XSLT, SCHEMA 
                xw.WriteStartElement("Dokument");
                xw.WriteAttributeString("typ", "Dane");
                xw.WriteValue(Convert.ToBase64String(GetStreamData(xmlBody), Base64FormattingOptions.InsertLineBreaks));
                xw.WriteEndElement();

                xw.WriteStartElement("Dokument");
                xw.WriteAttributeString("typ", "Styl");
                xw.WriteValue(Convert.ToBase64String(GetStreamData(xslStyle), Base64FormattingOptions.InsertLineBreaks));
                xw.WriteEndElement();

                //xw.WriteStartElement("Dokument");
                //xw.WriteAttributeString("typ", "Schemat");
                //xw.WriteEndElement();

                // załączniki dokumentu
                xw.WriteStartElement("Dokument");
                xw.WriteAttributeString("typ", "Zalacznik");
                xw.WriteValue(Convert.ToBase64String(GetStreamData(att1), Base64FormattingOptions.InsertLineBreaks));
                xw.WriteEndElement();

            xw.WriteEndElement();

            // drugi element sterujący
            xw.WriteStartElement("PlikSterujacy");

                xw.WriteStartElement("IdDokumentu");
                xw.WriteValue(docId.ToString());
                xw.WriteEndElement();

                xw.WriteStartElement("IdKlienta");
                xw.WriteValue(Guid.NewGuid().ToString());
                xw.WriteEndElement();

                xw.WriteStartElement("Metadane");
                xw.WriteEndElement();
            
            xw.WriteEndElement();

        xw.WriteEndElement();
        xw.Close();

        //return sb.ToString();
        return Convert.ToBase64String(Encoding.Unicode.GetBytes(sb.ToString()));
    }

    [WebMethod]
    public string GetSingleDocument()
    {
        FileStream xmlBody = new FileStream(HostingEnvironment.ApplicationPhysicalPath + "/Wniosek.xml", FileMode.Open);
        FileStream xslStyle = new FileStream(HostingEnvironment.ApplicationPhysicalPath + "/Styl.xslt", FileMode.Open);
        FileStream att1 = new FileStream(HostingEnvironment.ApplicationPhysicalPath + "/Active.pdf", FileMode.Open);
        
        
        StringBuilder sb = new StringBuilder();
        XmlWriter xw = XmlWriter.Create(sb);

        xw.WriteStartElement("eDokument");

        // pierwszy element sterujący
        xw.WriteStartElement("ePaczka");

        // dane XML, XSLT, SCHEMA 
        xw.WriteStartElement("Dokument");
        xw.WriteAttributeString("typ", "Dane");
        xw.WriteValue(Convert.ToBase64String(GetStreamData(xmlBody), Base64FormattingOptions.InsertLineBreaks));
        xw.WriteEndElement();

        xw.WriteStartElement("Dokument");
        xw.WriteAttributeString("typ", "Styl");
        xw.WriteValue(Convert.ToBase64String(GetStreamData(xslStyle), Base64FormattingOptions.InsertLineBreaks));
        xw.WriteEndElement();

        //xw.WriteStartElement("Dokument");
        //xw.WriteAttributeString("typ", "Schemat");
        //xw.WriteEndElement();

        // załączniki dokumentu
        xw.WriteStartElement("Dokument");
        xw.WriteAttributeString("typ", "Zalacznik");
        xw.WriteValue(Convert.ToBase64String(GetStreamData(att1), Base64FormattingOptions.InsertLineBreaks));
        xw.WriteEndElement();

        xw.WriteEndElement();

        // drugi element sterujący
        xw.WriteStartElement("PlikSterujacy");

        xw.WriteStartElement("IdDokumentu");
        xw.WriteValue(Guid.NewGuid().ToString());
        xw.WriteEndElement();

        xw.WriteStartElement("IdKlienta");
        xw.WriteValue(Guid.NewGuid().ToString());
        xw.WriteEndElement();

        xw.WriteStartElement("Metadane");
        xw.WriteEndElement();

        xw.WriteEndElement();

        xw.WriteEndElement();
        xw.Close();

        return sb.ToString();
    }

    [WebMethod]
    public string ConfirmDocumentReceive(Guid docId)
    {
        return string.Empty;
    }
}
