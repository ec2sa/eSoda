using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Pemi.Esoda.Presenters
{
	public class GetImagePresenter:BasePresenter
	{
		private IGetImageView view;
		private IGetImageTask service;

		private Guid imageID;
		private int width;
		private int height;
		private string mimeType;

		public GetImagePresenter(IGetImageView view)
		{
			this.view = view;
			this.service = new GetImageTask();
		}

		public override void Initialize()
		{
			width = view.Width;
			height = view.Height;
			mimeType = view.MimeType;
			try
			{
				imageID = new Guid(view.ImageID);
			}
			catch
			{
				imageID = Guid.Empty;
			}
			getImageFromStorage(view.PageNumber);
		}

		private void getImageFromStorage(int pageNumber)
		{
			Stream s = service.GetImage(imageID);
			if (s == null) return;
			ImageFormat format = ImageFormat.Gif;
			switch (view.MimeType)
			{
				case "image/gif": format = ImageFormat.Gif; break;
				case "image/png": format = ImageFormat.Png; break;
				case "image/jpg":
				case "image/jpeg": format = ImageFormat.Jpeg; break;
				case "image/tif":
				case "image/tiff": format = ImageFormat.Tiff; break;
			}

      Image img = new Bitmap(s);
        if(pageNumber>0)
          img.SelectActiveFrame(FrameDimension.Page,pageNumber-1);

        img=img.GetThumbnailImage(width,height,null,IntPtr.Zero);
			img.Save(view.ContentStream, format);
		}

		protected override void subscribeToEvents()
		{
			
		}

		protected override void redirectToPreviousView()
		{
			
		}
	}
}
