using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Web;
using System.Web.Caching;
using System.Runtime.Serialization;
using System.Web.Security;
using System.ComponentModel;
using System.Data;

namespace Pemi.Esoda.DTO
{
    //public class ChatCls
    //{
    //    private Guid m_id;

    //    public Guid Id
    //    {
    //        get { return m_id; }
    //    }

    //    private List<string> m_messages = new List<string>();

    //    public List<string> Messages
    //    {
    //        get 
    //        {               
    //            return m_messages; 
    //        }
    //    }

    //    private List<Chatter> m_chatters = new List<Chatter>();

    //    public List<Chatter> Chatters
    //    {
    //        get { return m_chatters; }
    //        set { m_chatters = value; }
    //    }

    //    public static ReadOnlyCollection<ChatCls> ActiveChats()
    //    {
    //        if (HttpContext.Current.Cache["Chats"] != null)
    //        {
    //            List<ChatCls> chats = ((List<ChatCls>)HttpContext.Current.Cache["Chats"]);
    //            return new ReadOnlyCollection<ChatCls>(chats);
    //        }
    //        else
    //        {
    //            return new ReadOnlyCollection<ChatCls>(new List<ChatCls>());
    //        }

    //        //if (HttpContext.Current.Application["Chats"] != null)
    //        //{
    //        //    List<ChatCls> chats = ((List<ChatCls>)HttpContext.Current.Application["Chats"]);
    //        //    return new ReadOnlyCollection<ChatCls>(chats);
    //        //}
    //        //else
    //        //{
    //        //    return new ReadOnlyCollection<ChatCls>(new List<ChatCls>());
    //        //}
    //    }
        
    //    public string SendMessage(Chatter chatter, string message)
    //    {
    //        string messageMask = "{0} @ {1} : {2}";
    //        message = string.Format(messageMask, chatter.Name, DateTime.Now.ToString(), message);
    //        m_messages.Add(message);
    //        return message;
    //    }

    //    public ChatCls()
    //    {
    //        m_id = Guid.NewGuid();
    //    }
    //}

    //public class Chatter
    //{
    //    private Guid m_id;

    //    public Guid Id
    //    {
    //        get { return m_id; }
    //    }

    //    private string m_name;

    //    public string Name
    //    {
    //        get { return m_name; }
    //    }

    //    public static Dictionary<Guid, Chatter> ActiveChatters()
    //    {
    //        Dictionary<Guid, Chatter> retval = new Dictionary<Guid, Chatter>();
    //        if (HttpContext.Current.Application["Chatters"] != null)
    //        {
    //            List<Chatter> chatters = ((List<Chatter>)HttpContext.Current.Application["Chatters"]);
    //            foreach (Chatter chatter in chatters)
    //            {
    //                retval.Add(chatter.Id, chatter);
    //            }
    //        }
    //        return retval;
    //    }
       
    //    public void Join(ChatCls chat)
    //    {
    //        chat.Chatters.Add(this);
    //    }

    //    public Chatter(Guid id, string name)
    //    {
    //        m_id = id;
    //        m_name = name;
    //    }
    //}

    [DataContract]
    public class MessageCls
    {
        [DataMember]
        public string From { get; set; }
        [DataMember]
        public string To { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public bool IsRead { get; set; }
        [DataMember]
        public string FromGuid { get; set; }
        [DataMember]
        public string ToGuid { get; set; }

        public MessageCls(string from, string to, string message, string date): this(from, to, message, date, string.Empty, string.Empty)
        {
        }

        public MessageCls(string from, string to, string message, string date, string fromGuid, string toGuid)
        {
            this.From = from;
            this.To = to;
            this.Message = message;
            this.Date = date;
            this.IsRead = false;
            this.FromGuid = fromGuid;
            this.ToGuid = toGuid;
        }
    }

    [DataContract]
    public class ChatterCls
    {
        [DataMember]
        public string FromGuid { get; set; }
        [DataMember]
        public string From { get; set; }

        public ChatterCls(string fromGuid, string from)
        {
            this.FromGuid = fromGuid;
            this.From = from;
        }
    }

    public class UserCls
    {
        public string UserName { get; set; }
        public bool IsOnline { get; set; }
        public bool IsMessageSent { get; set; }
        public string Guid { get; set; }

        public UserCls(string userName, bool isOnline, bool isMessageSent, string guid)
        {
            this.UserName = userName;
            this.IsOnline = isOnline;
            this.IsMessageSent = isMessageSent;
            this.Guid = guid;
        }
    }

    public class UsersCls
    {
        private List<UserCls> users = null;

        public List<UserCls> Users 
        {
            get
            {
                return users;
            }
        }

        private bool findChatter(string n)
        {
            List<string> ch = WhoSentMessage();
            if (ch == null || ch.Count == 0)
                return false;

             foreach (string s in ch)
             {
                 if (s == n)
                     return true;                 
             }
             return false;
        }

        public UsersCls()
        {
            users = new List<UserCls>();
            UserCls usr = null;
            
            foreach (MembershipUser u in Membership.GetAllUsers())
            {                    
                if (u.UserName != Membership.GetUser().UserName)
                {
                    if (findChatter(u.UserName))
                    {
                        usr = new UserCls(u.UserName, u.IsOnline, true, u.ProviderUserKey.ToString());
                    }
                    else
                    {
                        usr = new UserCls(u.UserName, u.IsOnline, false, u.ProviderUserKey.ToString());
                    }

                    users.Add(usr);
                }

            }
        }

        private List<string> WhoSentMessage()
        {
            MembershipUser me = Membership.GetUser();
            List<string> chatters = new List<string>();

            List<MessageCls> messages = (List<MessageCls>)HttpContext.Current.Cache["MSG"];
            if (messages == null || messages.Count == 0)
                return null;

            foreach (MessageCls m in messages)
            {
                if (m.ToGuid == me.ProviderUserKey.ToString() && !m.IsRead)
                {
                    chatters.Add(m.From);
                }
            }

            return chatters;
        }        
    }   
}
