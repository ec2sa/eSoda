using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Tools
{
    public class LoginNotifier
    {
        static readonly string ZONE_ESODA = "ESODA";
        static readonly string ZONE_DOKUMENTOMAT = "DOKUMENTOMAT";

        public enum Zone { ESoda, Dokumentomat} ;
        
        private static string GetZone(Zone strefa)
        {
            string zone = string.Empty;
            switch (strefa)
            {
                case Zone.ESoda: zone = ZONE_ESODA; break;
                case Zone.Dokumentomat: zone = ZONE_DOKUMENTOMAT; break;
                default:
                    break;
            }
            return zone;
        }

        public static void Log_Success(string username, string remoteIP, Zone strefa)
        {           
            (new LoginHistoryDAO()).NotifyLogin(username, remoteIP, "Login OK", GetZone(strefa));
        }

        public static void Log_BadUsername(string username, string remoteIP, Zone strefa)
        {
            (new LoginHistoryDAO()).NotifyLogin(username, remoteIP, "B³êdna nazwa u¿ytkownika", GetZone(strefa));
        }

        public static void Log_BadPassword(string username, string password, string remoteIP, Zone strefa)
        {
            (new LoginHistoryDAO()).NotifyLogin(username, remoteIP, string.Format("B³edne has³o: {0}", password), GetZone(strefa));
        }
    }
}
