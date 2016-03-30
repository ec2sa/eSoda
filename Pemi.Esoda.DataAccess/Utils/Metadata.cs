using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Collections;

namespace Pemi.Esoda.Tools
{
	public class Metadata
	{
		private Dictionary<string, string> items;

		public Dictionary<string, string> Items
		{
			get { return items; }
		}

		public Metadata()
		{
			this.items = new Dictionary<string, string>();
		}

		public Metadata(XmlReader xr):this()
		{
			this.parse(xr,false);
		}

		public Metadata(XPathNodeIterator xpni)
			: this()
		{
			this.parse(xpni, false);
		}

		public Metadata(IDictionary<string,string> meta):this()
		{
		}

		public void Add(string key, string value)
		{
			this.items.Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return this.items.ContainsKey(key);
		}

		public void Remove(string key)
		{
			this.items.Remove(key);
		}

		public int Count
		{
			get { return this.items.Count;}
		}

		public Dictionary<string,string>.KeyCollection Keys
		{
			get { return this.items.Keys; }
		}

		public string this[string key]{
			get
			{
				return
					this.items[key];
			}
		}

		public string GetXml()
		{
			StringBuilder sb=new StringBuilder();
			XmlWriter xw = XmlWriter.Create(sb);
			xw.WriteStartDocument();
			xw.WriteStartElement("metadane");
			foreach (string key in this.items.Keys)
			{
				xw.WriteElementString(key, this.items[key]);
			}
			xw.WriteEndElement();//metadane;
			xw.WriteEndDocument();
			xw.Close();
			return sb.ToString();
		}

		private void parse(IDictionary<string,string> meta, bool clearExisting)
		{
			if (clearExisting)
				this.items.Clear();
			foreach (string key in meta.Keys)
			{
				this.items.Add(key, meta[key]);
			}
		}
		private void parse(XPathNodeIterator xpni, bool clearExisting)
		{
			if (clearExisting)
				this.items.Clear();
			while (xpni.MoveNext())
			{
                string name = xpni.Current.Name;
                if (xpni.Current.InnerXml.Contains("\r\n"))
                {
                    string val = string.Empty;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xpni.Current.OuterXml);
                    XPathNavigator xpn = doc.CreateNavigator();
                    XPathNodeIterator innerXpni = xpn.Select("/"+doc.DocumentElement.Name+"/*");
                    
                    while(innerXpni.MoveNext())
                    {
                        if (val.Length > 0)
                            val += "<br/>&nbsp;";
                        val += innerXpni.Current.Value;
                    }
                    if (val.EndsWith("<br/>&nbsp;"))
                        val = val.Remove(val.Length - 11, 11);
                    if (!this.items.ContainsKey(xpni.Current.Name))
                    {
                        this.items.Add(xpni.Current.Name == "nadawca" ? "interesant" : xpni.Current.Name, val);
                    }
                }
                else
                    if (!this.items.ContainsKey(xpni.Current.Name))
                        this.items.Add(xpni.Current.Name == "nadawca" ? "interesant" : xpni.Current.Name, xpni.Current.Value);
			}
		}

		private void parse(XmlReader xr,bool clearExisting)
		{
			XPathDocument xpd=new XPathDocument(xr);
			XPathNavigator xpn=xpd.CreateNavigator();
			XPathNodeIterator xpni= xpn.Select("/metadane/*");
			this.parse(xpni, clearExisting);
			xr.Close();
		}

		public void Merge(XmlReader xr)
		{
			XPathDocument xpd = new XPathDocument(xr);
			XPathNavigator xpn = xpd.CreateNavigator();
			XPathNodeIterator xpni = xpn.Select("/metadane/*");
			while (xpni.MoveNext())
			{
				if(this.items.ContainsKey(xpni.Current.Name))
					this.items[xpni.Current.Name]=xpni.Current.Value;
				else
					this.items.Add(xpni.Current.Name, xpni.Current.Value);
			}
			xr.Close();
		}

		public void Merge(Metadata metadataToMerge)
		{
		foreach(string key in metadataToMerge.Keys)
			{
				if (this.items.ContainsKey(key))
					this.items[key] = metadataToMerge[key];
				else
					this.items.Add(key,metadataToMerge[key]);
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (string key in this.Keys)
			{
				sb.AppendLine(string.Format("{0}:{1}", key, this[key]));
			}
			return sb.ToString();
		}
	}
}
