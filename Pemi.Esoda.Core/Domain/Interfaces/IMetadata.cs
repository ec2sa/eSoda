using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Core.Domain
{
	public interface IMetadata
	{
		Metadata Content { get;}

		void Replace(Metadata newMetadata);
		
		void Merge(Metadata metadataToMerge);


	}
}
