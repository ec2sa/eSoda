using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Pemi.Esoda.Tools
{
    public class CoreObject
    {
        public static int GetId(HttpRequest request)
        {
            int objectId=0;
            if (request.QueryString["id"] != null)
                int.TryParse(request.QueryString["id"].ToString(), out objectId);
            return objectId;
        }
    }
}
