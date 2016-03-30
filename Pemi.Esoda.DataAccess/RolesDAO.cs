using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Pemi.Esoda.DataAccess
{
    public class RolesDAO
    {
        public IDataReader GetRoles()
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[listaRol]");

            return db.ExecuteReader(cmd);
        }
    }
}
