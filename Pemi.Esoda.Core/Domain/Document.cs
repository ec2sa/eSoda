using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;
using System.Xml.XPath;
using Pemi.Esoda.DTO;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Core.Domain
{
	public class Document:BaseDomainObject<int>, IDocument
	{
		private DocumentDAO dao = new DocumentDAO();

		private string name;

		private string description;

		private DateTime creationDate;

		private string owner;

		private Metadata meta;

		public Document() : base() { }

		public Document(int id)
			: base(id)
		{
            using (XmlReader xr = dao.GetDocument(id))
            {
                if (!xr.Read())
                    throw new Exception(string.Format("Wybrany dokument nie istnieje {0}", id));
                XPathDocument xpd = new XPathDocument(xr);
                XPathNavigator xpn = xpd.CreateNavigator();
                this.name = xpn.SelectSingleNode("/dokument/nazwa") == null ? string.Empty : xpn.SelectSingleNode("/dokument/nazwa").Value;
                this.description = xpn.SelectSingleNode("/dokument/opis") == null ? string.Empty : xpn.SelectSingleNode("/dokument/opis").Value;

                DateTime tmpDate;
                if (DateTime.TryParse(xpn.SelectSingleNode("/dokument/@dataUtworzenia").Value, out tmpDate))
                    this.creationDate = tmpDate;

                this.owner = xpn.SelectSingleNode("/dokument/wlasciciel").Value;
                //XPathIteratorReader xpir = new XPathIteratorReader(xpn.Select("/dokument/metadane"));
                meta = new Metadata(xpn.Select("/dokument/metadane/*"));
            }
		}

		public Document(DocumentDTO doc):this()
		{
			this.name = doc.Name;
			this.owner = doc.Owner;
			this.description = doc.Description;
			this.creationDate = doc.CreationDate;
			this.meta = new Metadata(doc.Metadata);
		}

		public DocumentDTO GetDocumentData()
		{
			return new DocumentDTO(this.ID, this.name, this.description, this.owner, this.creationDate, this.meta.Items);
		}

		#region IDocument Members

		string IDocument.Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				this.isModified = true;
			}
		}

		string IDocument.Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
				this.isModified = true;
			}
		}

		System.Collections.ObjectModel.Collection<DocumentItemDTO> IDocument.GetItems()
		{
			return dao.GetItems(this.ID);
		}

		void IDocument.AddItem(IDocumentItem item)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		void IDocument.RemoveItem(IDocumentItem item)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		void IDocument.UpdateItem(IDocumentItem item)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		IDocumentItem IDocument.GetItem(Guid itemID)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		DateTime IDocument.CreationDate
		{
			get { return this.creationDate; }
		}

		string IDocument.Owner
		{
			get { return this.owner; }
		}

		#endregion

		#region IMetadata Members

		Metadata IMetadata.Content
		{
			get { return this.meta;}
		}

		void IMetadata.Replace(Metadata newMetadata)
		{
			this.meta = newMetadata;
		}

		void IMetadata.Merge(Metadata metadataToMerge)
		{
			this.meta.Merge(metadataToMerge);
		}

		#endregion
	}
}
