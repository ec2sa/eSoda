using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Web.Security;
using System.Data.SqlClient;
using Pemi.Esoda.DTO;
using System.Collections.Specialized;

namespace Pemi.Esoda.DataAccess
{
	public class UserDAO
	{
		public void CreateEmployee(string lastname, string firstname,Guid userID,string stanowisko)
		{
			Database db = DatabaseFactory.CreateDatabase();
			DbCommand cmd = db.GetStoredProcCommand("Uzytkownicy.dodanieUzytkownika", userID, lastname, firstname, null, null, 4,stanowisko);
			db.ExecuteNonQuery(cmd);
		}

        public void CreateOrganizationalUnit(string groupName, string groupSymbol, bool isOrganizationalUnit,int parentOrganizationalUnit)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[dodanieGrupy]", groupName,groupSymbol,isOrganizationalUnit, parentOrganizationalUnit);
            db.ExecuteNonQuery(cmd);
        }

		public string GetAssignedItems(Guid userId)
		{
            //SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            //if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            //DbCommand cmd = db.GetStoredProcCommand("Akcje.listaPrzypisanychZadan ", userId);
            //XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
            //return xr;

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Akcje].[listaOczekujacychZadan]", userId,0);
            cmd.CommandTimeout = 600;
            DataSet dsZadania = db.ExecuteDataSet(cmd);

            if (dsZadania.Tables.Count > 0)
            {
                dsZadania.DataSetName = "zadania";
                dsZadania.Tables[0].TableName = "zadanie";
                return dsZadania.GetXml();
            }
            return string.Empty;
		}

        public string GetAllAssignedItems()
        {
            Guid userId = Guid.Empty;
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Akcje].[listaOczekujacychZadan]", userId);
            cmd.CommandTimeout = 600;
            DataSet dsZadania = db.ExecuteDataSet(cmd);

            if (dsZadania.Tables.Count > 0)
            {
                dsZadania.DataSetName = "zadania";
                dsZadania.Tables[0].TableName = "zadanie";
                return dsZadania.GetXml();
            }
            return string.Empty;
        }

        public IDataReader GetEmployeeList()
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[listaPracownikow]");
            cmd.CommandTimeout = 600;
            return db.ExecuteReader(cmd);
        }

        public IDataReader GetEmployee(Guid userId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[pobierzPracownika]", userId);
            cmd.CommandTimeout = 600;
            return db.ExecuteReader(cmd);   
        }

        public IDataReader GetWorkersListByGroup(int groupId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[listaPracownikowKomorkiOrganizacyjnej]", groupId);
            return db.ExecuteReader(cmd);            
        }

        public void UpdateEmployee(Guid userId, string nazwisko, string imie, int wydzial,string stanowisko)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[aktualizujPracownika]", userId, imie, nazwisko, wydzial,stanowisko);
            db.ExecuteNonQuery(cmd);
        }

        public void UpdateEmployeePassword(Guid userId, string login, string oldPass, string newPass)
        {
            MembershipUser user = Membership.GetUser(userId);
            if(user != null)
                user.ChangePassword(oldPass, newPass);
        }

        public void SetUserPIN(Guid userId, string pin)
        {
            string sPin = (pin == string.Empty) ? "1234" : pin;
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[setUserPIN]", userId, sPin);
            db.ExecuteNonQuery(cmd);
        }

        public bool PINIsValid(Guid userId, string pin)
        {
            bool isValid = false;
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[isPINValid]", userId, pin);
            using(DbDataReader dr = (DbDataReader)db.ExecuteReader(cmd))
            {
                isValid = dr.Read();
            }
            return isValid;
        }

        public void GetCustomerTypeCat(int userId, out int typeId, out int catId)
        {
            typeId = -1;
            catId = -1;
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[pobierzTypIKategorieInteresanta]", userId);
            using (DbDataReader dr = (DbDataReader)db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    if (!int.TryParse(dr["idTypuUzytkownika"].ToString(), out typeId))
                        typeId = -1;

                    if (!int.TryParse(dr["idKategorii"].ToString(), out catId))
                        catId = -1;
                }
            }
        }

        public void SetAvailableChat(Guid userId, bool isAvailable)
        {            
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[setChatAvailable]", userId, isAvailable);
            db.ExecuteNonQuery(cmd);
        }

        public bool GetAvailableChat(Guid userId)
        {
            bool isAvailable = false;
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[getChatAvailable]", userId);
            using (DbDataReader dr = (DbDataReader)db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    isAvailable = bool.Parse(dr["chatAvailable"].ToString());
                }
            }
            return isAvailable;
        }

        public void SetManager(Guid userId, bool isManager)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[setManager]", userId, isManager);
            db.ExecuteNonQuery(cmd);
        }

        public bool GetManager(Guid userId)
        {
            bool isManager = false;
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[getManager]", userId);
            using (DbDataReader dr = (DbDataReader)db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    isManager = bool.Parse(dr["kierownik"].ToString());
                }
            }
            return isManager;
        }

       

        public List<SimpleLookupDTO> GetUsersFromDepartment(int departmentID, int dublerID)
        {
            List<SimpleLookupDTO> items = new List<SimpleLookupDTO>();

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Uzytkownicy.coverGetUsersFromDepartment", departmentID, dublerID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    items.Add(new SimpleLookupDTO(int.Parse(dr["id"].ToString()), dr["nazwa"].ToString()));
                }
            };

            return items;
        }

        public List<SimpleLookupDTO> GetDepartments()
        {
            List<SimpleLookupDTO> items = new List<SimpleLookupDTO>();

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Uzytkownicy.coverGetDepartments");

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                items.Add(new SimpleLookupDTO(-1, "--wszystkie--"));

                while (dr.Read())
                {
                    items.Add(new SimpleLookupDTO(int.Parse(dr["id"].ToString()), dr["nazwa"].ToString()));
                }
            };

            return items;
        }

        public int GetUserID(Guid userGuid)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Uzytkownicy.getCoverUserID", userGuid);

            int userID = -1;

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    userID = int.Parse(dr["id"].ToString());
                }
            };

            return userID;
        }

        public void SetCover(int userID, int dublerID, DateTime startDate, DateTime endDate)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Uzytkownicy.coverSetCover", userID, dublerID, startDate, endDate);

            db.ExecuteNonQuery(cmd);            
        }

        public void DelCover(int coverID)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Uzytkownicy.coverDelCover", coverID);

            db.ExecuteNonQuery(cmd);     
            
        }

        public IList<CoverDTO> GetCoverList(int dublerID)
        {
            List<CoverDTO> items = new List<CoverDTO>();

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Uzytkownicy.coverGetCoverList", dublerID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    items.Add(new CoverDTO()
                    {
                        CoverID=int.Parse(dr["id"].ToString()),
                        DublerID = dublerID,
                        UserDepartment = dr["userDepartment"].ToString(),
                        UserName = dr["userName"].ToString(),
                        UserSurname = dr["userSurname"].ToString(),
                        StartDate = DateTime.Parse(dr["startDate"].ToString()),
                        EndDate = DateTime.Parse(dr["endDate"].ToString())
                    }
                    );
                }                
            };

            return items;
        }

        public IList<AvailableCoverDTO> GetAvailableCover(Guid userGuid)
        {
            List<AvailableCoverDTO> items = new List<AvailableCoverDTO>();

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Uzytkownicy.coverGetCover", userGuid);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                items.Add(new AvailableCoverDTO() {UserLogin="-1", UserFullName="--wybierz--"});

                while (dr.Read())
                {
                    items.Add(new AvailableCoverDTO()
                    {
                        UserFullName = dr["userName"].ToString(),
                        UserLogin = dr["userLogin"].ToString()
                    }
                    );
                }
            };

            return items;
        }

        public bool IsAvailableCover(Guid userGuid)
        {
            bool isCover = false;

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetSqlStringCommand("select Uzytkownicy.isCover(@dublerGuid)");
            db.AddInParameter(cmd, "@dublerGuid", DbType.Guid);
            db.SetParameterValue(cmd, "@dublerGuid", userGuid);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    isCover = dr.IsDBNull(0) ? false : dr.GetBoolean(0);
                }
            }

            return isCover;
        }

        public bool IsCoverDublerLogin(string userName)
        {
            bool isCover = false;

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetSqlStringCommand("select Uzytkownicy.isCoverDublerLogin(@userName)");
            db.AddInParameter(cmd, "@userName", DbType.String);
            db.SetParameterValue(cmd, "@userName", userName);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    isCover = dr.IsDBNull(0) ? false : dr.GetBoolean(0);
                }
            }

            return isCover;
        }
	}

    
}
