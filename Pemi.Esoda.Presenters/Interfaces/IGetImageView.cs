using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace Pemi.Esoda.Presenters
{
	public interface IGetImageView
	{
		string MimeType { get;}
		int Width {get;}
		int Height {get;}
    int PageNumber { get;}
		string ImageID { get;}
		Stream ContentStream { get;}
		Stream ContentMemoryStream { set;}
	}


}
