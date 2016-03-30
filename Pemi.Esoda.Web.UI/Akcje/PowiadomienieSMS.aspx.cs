using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.DTO;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Tools;
using System.Web.Security;
using SMSService;

namespace Pemi.Esoda.Web.UI.Akcje
{
    public partial class PowiadomienieSMS : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int caseId = CoreObject.GetId(Request);
                if (caseId == 0)
                {
                    wyslij.Enabled = false;
                    return;
                }

                SMSData data = new CaseDAO().GetSMSDataForCase(caseId);
                txtNumerSMS.Text = data.PhoneNumber;
                txtMessage.Text = data.Message;
            }
        }

        protected bool SendSMS(string number, string message)
        {
            
            SMSServiceConfiguration config=new SMSServiceDAO().GetSMSConfig();
            SMSClientConfiguration clientConfig=new SMSClientConfiguration(){
                Username=config.Username
                ,Password=config.Password
                ,TestOnly=config.IsTest
                ,Sender=config.Sender
            };
            SMSClient client = new SMSClient(clientConfig);

            return client.Send(number, message, config.IsFlash);
        }
        protected void wyslij_click(object sender, EventArgs e)
        {
            int caseId = CoreObject.GetId(Request);

            bool wasSent = SendSMS(txtNumerSMS.Text, txtMessage.Text);

            List<string> parameters = new List<string>();
            parameters.Add(wasSent ? "Udane" : "Nieudane");
            parameters.Add(txtNumerSMS.Text);
            parameters.Add(txtMessage.Text);
            ActionLogger al = new ActionLogger(new ActionContext(new Guid("1F00EEAE-D0DB-4267-A62A-07A95FDF8E25"), new Guid(Membership.GetUser().ProviderUserKey.ToString()), Membership.GetUser().UserName, Membership.GetUser().Comment, parameters));
            al.AppliesToCases.Add(caseId);
            al.Execute();

            Response.Redirect("~/Sprawy/HistoriaSprawy.aspx?id=" + caseId.ToString(), false);
        }
    }
}
