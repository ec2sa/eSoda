using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;


namespace Pemi.Esoda.DTO
{
	public class DocumentDTO
	{
		private int id;

		public int Id
		{
			get { return id; }
		}

		private string name;

		public string Name
		{
			get { return name; }
		}

		private string description;

		public string Description
		{
			get { return description; }
		}

		private DateTime creationDate;

		public DateTime CreationDate
		{
			get { return creationDate; }
		}

		private string owner;

		public string Owner
		{
			get { return owner; }
		}

		private IDictionary<string, string> metadata;

		public IDictionary<string, string> Metadata
		{
			get { return metadata; }
		}

		public DocumentDTO(int id,string name, string description, string owner, DateTime creationDate, IDictionary<string, string> metadata)
		{
			this.id = id;
			this.name = name;
			this.description = description;
			this.creationDate = creationDate;
			this.owner = owner;
			this.metadata = metadata;
		}

	}

    public class DokumentDetails
    {
        public readonly bool IsCopy;
        public readonly string Description;
        public readonly string Notice;
        public readonly bool IsInCase;        

        public DokumentDetails(bool isCopy, string description, string notice, bool isInCase)
        {
            this.IsCopy = isCopy;
            this.Description = description;
            this.Notice = notice;
            this.IsInCase = isInCase;            
        }
    }
}
