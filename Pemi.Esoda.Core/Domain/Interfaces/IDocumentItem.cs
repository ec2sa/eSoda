using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Core.Domain
{
	public interface IDocumentItem
	{
		Guid ID { get;}
		string OriginalName { get;}
		string Description { get; set;}
		bool IsMain { get;set;}
		string MimeType { get; set;}
	}
}
