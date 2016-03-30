using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Presenters
{
	public interface IAddItemScanView
	{
		int IncomingScansCount { set;}
		void BindScans(string xmlContent, string xpath);
		void BindUnassignedItems(string xmlContent, string xpath);
		void BindConditions();
		int ItemId { set;}
	}
}
