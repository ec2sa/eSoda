using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class CaseDTO
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

		public CaseDTO(int id,string name, string description, string owner, DateTime creationDate, IDictionary<string, string> metadata)
		{
			this.id = id;
			this.name = name;
			this.description = description;
			this.creationDate = creationDate;
			this.owner = owner;
			this.metadata = metadata;
		}
    }
}
