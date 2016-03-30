using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Pemi.Esoda.Presenters
{
	public interface IDownloadFileView
	{
		string MimeType { set;}
		Guid FileID { get;}
		string FileName { get;}
		string DownloadedFileName { set;}
		Stream ContentStream { get;}
        int PageNumber { get;}
        bool ToPDF { get; }
	}
}
