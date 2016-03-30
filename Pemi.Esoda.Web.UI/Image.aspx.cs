using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Pemi.Esoda.Presenters;
using System.IO;
using Pemi.Esoda.DataAccess.Utils;

namespace Pemi.Esoda.Web.UI
{
	public partial class Image : System.Web.UI.Page,IGetImageView
	{
		private string imageID;
		private string mimeType;
		private int width;
		private int height;
    private int pageNumber;

    public int PageNumber
    {
      get { return pageNumber; }
      set { pageNumber = value; }
    }

	#region IGetImageView Members

		public string ImageID
		{
			get { return imageID; }
			set { imageID = value; }
		}

		public string MimeType
		{
			get { return mimeType; }
			set { mimeType = value; }
		}
	
		public int Width
		{
			get { return width; }
			set { width = value; }
		}

		public int Height
		{
			get { return height; }
			set { height = value; }
		}

		

		#endregion

		protected void Page_Init(object sender, EventArgs e)
		{
			imageID = Request["id"].ToString();
			if (Request["type"] != null && Request["type"].ToString().ToLower() == "preview")
			{
				Width = 450;
				Height = 600;
			}
			if (Request["w"] != null)
			{
				int width;
				if(int.TryParse(Request["w"],out width)){
					Width=width;
					Height=Width*133/100;
				}
			}
      if (Request["mt"] != null)
      {
        mimeType = MimeHelper.PobierzTypDlaRozszerzenia(Request["mt"].ToString()).Nazwa;
      }
      if (Request["pn"] != null)
      {
        int pn;
        if (!int.TryParse(Request["pn"].ToString(), out pn))
          PageNumber = 0;
        else
          PageNumber = pn;
      }
			GetImagePresenter presenter = new GetImagePresenter(this);
			presenter.Initialize();
		}

		#region IGetImageView Members

		Stream IGetImageView.ContentStream
		{
			get {Response.Clear();
				Response.ContentType = this.MimeType;
				
				return Response.OutputStream;
		 }
		}

		Stream IGetImageView.ContentMemoryStream
		{
			set
			{
				MemoryStream ms = (MemoryStream)value;
			}
		}

		#endregion
	}
}
