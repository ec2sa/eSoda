using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Services;
using System.Web.Caching;
using Pemi.Esoda.DTO;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.IO;
using System.Xml;


namespace Pemi.Esoda.Web.UI.services
{
    /// <summary>
    /// Summary description for ChatService
    /// </summary>
    [ScriptService]
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.     

    public class ChatService : System.Web.Services.WebService
    {               
        [WebMethod]
        public void SetMessage(string message, string from, string to, string fromGuid, string toGuid)
        {
            List<MessageCls> m = (List<MessageCls>)HttpContext.Current.Cache["MSG"];
            MessageCls msg = new MessageCls(from, to, message, DateTime.Now.ToString(), fromGuid, toGuid);
            m.Add(msg);
        }
        
        [WebMethod]
        public string GetMessage(string fromGuid, string toGuid)
        {
            StringBuilder sb = new StringBuilder();

            List<MessageCls> allList = (List<MessageCls>)HttpContext.Current.Cache["MSG"];
            List<MessageCls> list = new List<MessageCls>();

            foreach (MessageCls i in allList)
            {
                if (i.ToGuid == toGuid && i.FromGuid== fromGuid && !i.IsRead)
                {
                    list.Add(i);
                    i.IsRead = true;
                }
            }

            DataContractJsonSerializer json = new DataContractJsonSerializer(list.GetType());

            MemoryStream ms = new MemoryStream();

            json.WriteObject(ms, list);
            //Encoding.Default.GetString(ms.ToArray());
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());

            return jsonString;
        }
        
        [WebMethod]
        public string GetChatters(string toGuid)
        {
            StringBuilder sb = new StringBuilder();

            List<MessageCls> list = (List<MessageCls>)HttpContext.Current.Cache["MSG"];

            List<ChatterCls> chatters = new List<ChatterCls>();

            if (list == null || list.Count == 0) return null;

            foreach (MessageCls i in list)
            {
                if (i.ToGuid == toGuid && !i.IsRead)
                {                    
                    if (!FindChatter(chatters, i.From))
                        chatters.Add(new ChatterCls(i.FromGuid, i.From));
                }
            }

            if (chatters.Count == 0) return null;

            DataContractJsonSerializer json = new DataContractJsonSerializer(chatters.GetType());

            MemoryStream ms = new MemoryStream();

            json.WriteObject(ms, chatters);
            
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());

            return jsonString;
        }

        [WebMethod]
        public bool IsNewMessage(string toGuid)
        {
            List<MessageCls> list = (List<MessageCls>)HttpContext.Current.Cache["MSG"];
            if (list == null || list.Count == 0) return false;

            foreach (MessageCls m in list)
            {
                if (m.ToGuid == toGuid && !m.IsRead)
                    return true;
            }

            return false;
        }

        private bool FindChatter(List<ChatterCls> list, string from)
        {
            foreach (ChatterCls i in list)
            {
                if (i.From == from)
                    return true;
            }
            return false;
        }
    }
}
