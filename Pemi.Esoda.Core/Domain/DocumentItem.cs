using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Core.Domain
{
	public class DocumentItem:BaseDomainObject<Guid>, IDocumentItem,IMetadata
	{

		Guid IDocumentItem.ID
		{
			get { return this.ID; }
		}

		private string originalName;

		string IDocumentItem.OriginalName
		{
			get { return originalName; }
		}

		private string description;

	  string IDocumentItem.Description
		{
			get { return description; }
			set { description = value; }
		}

		private string mimeType;

		string IDocumentItem.MimeType
		{
			get { return mimeType; }
			set { mimeType = value; }
		}

		private bool isMain;

		bool IDocumentItem.IsMain
		{
			get { return isMain; }
			set { isMain = value; }
		}

		private Metadata meta;

		public DocumentItem(string originalName, string mimeType, string description):base()
		{
			this.originalName = originalName;
			this.mimeType = mimeType;
			this.description = description;
		}

		public DocumentItem(Guid itemID)
			: base(itemID)
		{
			
		}

		public Guid Save(Stream itemContent)
		{
			IItemStorage storage = ItemStorageFactory.Create();
			return storage.Save(itemContent);
		}

		Metadata IMetadata.Content
		{
			get { return this.meta; }
		}

		void IMetadata.Replace(Metadata newMetadata)
		{
			this.meta = newMetadata;
		}

		void IMetadata.Merge(Metadata metadataToMerge)
		{
			this.meta.Merge(metadataToMerge);
		}

	}
}
