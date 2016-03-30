using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using Pemi.Esoda.Tools;
using System.IO;
using Pemi.Esoda.DataAccess.Utils;

namespace Pemi.Esoda.Presenters
{
	public class DownloadFilePresenter:BasePresenter
	{
		public IDownloadFileView view;
		public IDownloadFileTask service;

		private void copyStream(Stream source, Stream dest)
		{
			byte[] buffer = new byte[1024];
			int read;
			do
			{
				read = source.Read(buffer, 0, buffer.Length);
				dest.Write(buffer, 0, read);
			} while (read != 0);
		}

		private void getFileFromStorage()
		{
			string mimeType;
			string fileName;
			Stream s = service.GetFile(view.FileID,out mimeType,out fileName,view.PageNumber);
			view.MimeType = mimeType;
			view.DownloadedFileName = fileName;
			s.Seek(0, 0);
			copyStream(s, view.ContentStream);
			s.Close();
		}

        private void getConvertedPDFFileFromStorage()
        {
            string mimeType;
            string fileName;
            Stream s = service.GetTiffToPDF(view.FileID, out mimeType, out fileName);
            view.MimeType = mimeType;
            view.DownloadedFileName = fileName;
            s.Seek(0, 0);
            copyStream(s, view.ContentStream);
            s.Close();
        }

		public DownloadFilePresenter(IDownloadFileView view)
		{
			this.view = view;
			this.service = new DownloadFileTask();
		}


		public override void Initialize()
		{
			if (view.FileID != Guid.Empty){
                if (!view.ToPDF)
                    getFileFromStorage();
                else
                    getConvertedPDFFileFromStorage();
				return;
			}
            if (view.FileName != string.Empty)
            {
                //if (!view.ToPDF)
                    getFileFromTemporarDirectory();
                //else
                //    getFileToPDFFromTemporarDirectory();
                return;
                
                
            }
		}

		private void getFileFromTemporarDirectory()
		{
			string fileName =string.Format("{0}\\{1}",Configuration.PhysicalTemporaryDirectory, Path.GetFileName(view.FileName));
			if (fileName.Length < 5) return;
			Stream s = service.GetTempFile(fileName);// File.OpenRead(fileName);
			view.MimeType = MimeHelper.PobierzTypDlaRozszerzenia(Path.GetExtension(fileName)).Nazwa;
			view.DownloadedFileName = string.Format("obraz_{0}",DateTime.Now.ToString("yyyyMMddmmss"));
			s.Seek(0, 0);
			copyStream(s, view.ContentStream);
			s.Close();
		}

        //private void getFileToPDFFromTemporarDirectory()
        //{
        //    string fileName = string.Format("{0}\\{1}", Configuration.PhysicalTemporaryDirectory, Path.GetFileName(view.FileName));
        //    if (fileName.Length < 5) return;

        //    string mimeType;
        //    string outFileName;
        //    Stream s = service.GetTempFileToPDF(fileName, out mimeType, out outFileName);// File.OpenRead(fileName);
        //    view.MimeType = MimeHelper.PobierzTypDlaRozszerzenia(Path.GetExtension(outFileName)).Nazwa;
        //    view.DownloadedFileName = string.Format("{1}_{0}", DateTime.Now.ToString("yyyyMMddmmss"), outFileName);
        //    s.Seek(0, 0);
        //    copyStream(s, view.ContentStream);
        //    s.Close();
        //}


		protected override void subscribeToEvents()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void redirectToPreviousView()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
