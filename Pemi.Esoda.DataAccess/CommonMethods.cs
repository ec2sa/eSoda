using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.IO;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Pemi.Esoda.DataAccess
{
    public static class CommonMethods
    {
        public static XmlReader GetXmlReaderAndCloseConnection(DbCommand cmd, Database db)
        {

            XmlReader dr = (db as SqlDatabase).ExecuteXmlReader(cmd);
            try
            {
                if (dr.Read())
                {
                    XmlReader nxr = XmlReader.Create(new StringReader(dr.ReadOuterXml()));
                    return nxr;
                }
                return XmlReader.Create(new StringReader("<root/>"));
            }
            finally
            {
                dr.Close();
                if (cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
                    cmd.Connection.Close();
            }

        }
    }
}
