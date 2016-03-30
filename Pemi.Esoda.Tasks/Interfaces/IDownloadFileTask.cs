using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Pemi.Esoda.Tasks
{
	public interface IDownloadFileTask
	{
		Stream GetFile(Guid imageID,out string mimeType,out string fileName,int pageNumber);
		Stream GetTempFile(string path);
        Stream GetTiffToPDF(Guid imageID, out string mimeType, out string fileName);
        //Stream GetTempFileToPDF(string path, out string mimeType, out string fileName);
	}
}
