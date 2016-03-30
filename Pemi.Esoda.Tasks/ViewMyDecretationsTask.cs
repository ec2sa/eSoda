using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Tasks
{
    public class ViewMyDecretationsTask:IViewMyDecretationsTask
    {
        private MyDecretationsDAO dao = new MyDecretationsDAO();



        public IList<MyDecretationsSearchResult> GetDecretations(MyDecretationsSearchConditions sc, out int totalRowCount,object userGuid)
        {

            return dao.GetDecretations(sc, out totalRowCount,userGuid );
        }

        public IList<SimpleLookupDTO> GetDepartments()
        {
            return new List<SimpleLookupDTO>();
        }

        public IList<SimpleLookupDTO> GetClients()
        {
            return new List<SimpleLookupDTO>();
        }

    
    }
}
