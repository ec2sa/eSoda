using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace Pemi.Esoda.Tasks
{
	public class DownloadFileTask:IDownloadFileTask
	{
		#region IDownloadFileTask Members

		Stream IDownloadFileTask.GetTempFile(string path)
		{
			if(!File.Exists(path))
				throw new ArgumentException("Nie ma takiego pliku");
			FileStream fs = File.OpenRead(path);
			return fs;
		}

        //Stream IDownloadFileTask.GetTempFileToPDF(string path, out string mimeType, out string fileName)
        //{
        //    if (!File.Exists(path))
        //        throw new ArgumentException("Nie ma takiego pliku");
        //    FileStream fs = File.OpenRead(path);
            
        //    Image img = Image.FromStream(fs);

        //    TiffImageSplitter tiff = new TiffImageSplitter();
        //    PdfDocument doc = new PdfDocument();
        //    int pageCount = tiff.getPageCount(img);

        //    for (int i = 0; i < pageCount; i++)
        //    {
        //        PdfPage page = new PdfPage();
        //        Image tiffImg = tiff.getTiffImage(img, i);
        //        XImage ximg = XImage.FromGdiPlusImage(tiffImg);

        //        page.Width = ximg.PointWidth;
        //        page.Height = ximg.PointHeight;
        //        doc.Pages.Add(page);

        //        XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[i]);
        //        xgr.DrawImage(ximg, 0, 0);
        //    }

        //    mimeType = "application/pdf";
        //    fileName = Path.ChangeExtension(Path.GetFileName(path), "pdf");

        //    MemoryStream ms = new MemoryStream();
        //    doc.Save(ms, false);
        //    return ms;
            
        //}

		Stream IDownloadFileTask.GetFile(Guid imageID, out string mimeType, out string fileName,int pageNumber)
		{
			IItemStorage storage = ItemStorageFactory.Create();
			DocumentDAO dao=new DocumentDAO();

            DTO.DocumentItemDTO item = dao.GetItem(imageID);	
			if(item==null)
				throw new ArgumentException("Nie ma takiego pliku");
			mimeType = item.MimeType;
			fileName = item.OriginalName;
            if (pageNumber > 0 && mimeType.ToLower() == "image/tiff")
            {
                Image img = new Bitmap(storage.Load(imageID));
                img.SelectActiveFrame(FrameDimension.Page, pageNumber - 1);
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
                return ms;
            }
            else
                return storage.Load(imageID);
		}

        Stream IDownloadFileTask.GetTiffToPDF(Guid imageID, out string mimeType, out string fileName)
        {
            IItemStorage storage = ItemStorageFactory.Create();
            DocumentDAO dao = new DocumentDAO();

            DTO.DocumentItemDTO item = dao.GetItem(imageID);
            if (item == null)
                throw new ArgumentException("Nie ma takiego pliku");

            if (item.MimeType.ToLower() == "image/tiff")
            {
                Image img = new Bitmap(storage.Load(imageID));

                TiffImageSplitter tiff = new TiffImageSplitter();
                PdfDocument doc = new PdfDocument();
                int pageCount = tiff.getPageCount(img);

                for (int i = 0; i < pageCount; i++)
                {
                    PdfPage page = new PdfPage();

                    Image tiffImg = tiff.getTiffImage(img, i);

                    XImage ximg = XImage.FromGdiPlusImage(tiffImg);

                    page.Width = ximg.PointWidth;
                    page.Height = ximg.PointHeight;
                    doc.Pages.Add(page);

                    XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[i]);

                    xgr.DrawImage(ximg, 0, 0);
                }

                mimeType = "application/pdf";
                fileName = Path.ChangeExtension(item.OriginalName, "pdf");
                
                MemoryStream ms = new MemoryStream();
                doc.Save(ms, false);
                return ms;
            }
            else
                throw new ArgumentException("Podany plik nie jest plikiem TIFF");
            
            
        }

		#endregion

	
	}
}
