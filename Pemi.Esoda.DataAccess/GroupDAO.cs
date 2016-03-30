using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Pemi.Esoda.DataAccess
{
    public class GroupDAO
    {
        public void CreateGroup(string nazwa, string skrot, bool jednostkaOrganizacyjna, int idRodzica)
        {
            UserDAO ud = new UserDAO();
            ud.CreateOrganizationalUnit(nazwa, skrot, jednostkaOrganizacyjna, idRodzica);
        }

        public IDataReader GetGroupsList()
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[listaGrup]");
            return db.ExecuteReader(cmd);
        }

        public DataSet GetGroupsListDataSet()
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[listaGrup]");
            return db.ExecuteDataSet(cmd);
        }

        public IDataReader GetGroup(int groupId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[pobierzGrupe]",groupId);
            return db.ExecuteReader(cmd);
        }

        public void UpdateGroup(int groupId, int idRodzica, string nazwa, string skrot, bool jednostkaOrganizacyjna)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[aktualizujGrupe]", groupId,nazwa, skrot, jednostkaOrganizacyjna,idRodzica);
            db.ExecuteNonQuery(cmd);            
        }

        public string[] GetGroupsForUser(Guid userId)
        {
            string groups = string.Empty;
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Groups.GetGroupsForUser");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[pobierzListeGrupDlaUzytkownika]", userId);
            DbDataReader dr = (DbDataReader)db.ExecuteReader(cmd);
           
            while (dr.Read())
            {
                if (groups.Length > 0)
                    groups += ",";
                groups += dr.GetString(1);
            }
            return groups.Split(',');
        }

        public void DeleteGroup()
        { }
    }
}
