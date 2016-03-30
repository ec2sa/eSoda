using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Web.Security;

namespace Pemi.Esoda.DataAccess
{
    [DataObject]
    public class GetAllUsers
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataSet CustomGetAllUsers()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt = ds.Tables.Add("Users");

            MembershipUserCollection muc;
            muc = Membership.GetAllUsers();

            dt.Columns.Add("UserName", Type.GetType("System.String"));
            dt.Columns.Add("Email", Type.GetType("System.String"));
            dt.Columns.Add("Comment", Type.GetType("System.String"));
            dt.Columns.Add("ProviderUserKey", Type.GetType("System.Object"));

            UserDAO ud = new UserDAO();

            foreach (MembershipUser mu in muc)
            {                
                if (Membership.GetUser().UserName != mu.UserName && ud.GetAvailableChat((Guid)mu.ProviderUserKey))
                {
                    DataRow dr;
                    dr = dt.NewRow();
                    dr["UserName"] = mu.UserName;
                    dr["Email"] = mu.Email;
                    dr["Comment"] = mu.Comment;
                    dr["ProviderUserKey"] = mu.ProviderUserKey;
                    dt.Rows.Add(dr);
                }
            }
            return ds;
        }
    }
}
