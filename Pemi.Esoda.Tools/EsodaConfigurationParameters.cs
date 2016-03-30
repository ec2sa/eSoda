using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Tools
{
    public static class EsodaConfigurationParameters
    {
        private static int _ticketDuration;
        private static int _ticketLifetime;
        private static bool _isDailyLogItemAccessDenied;

        public static int TicketDuration
        {
            get
            {
                refresh();
                return _ticketDuration;
            }
        }
        public static int TicketLifeTime
        {
            get
            {
                refresh();
                return _ticketLifetime;
            }
        }

        public static bool IsDailyLogItemAccessDenied
        {
            get
            {
                refresh();
                return _isDailyLogItemAccessDenied;
            }
        }

        private static void refresh()
        {
            Dictionary<string, string> items;

            items=new EsodaConfigParametersDAO().GetConfig();
            if (items.Count == 0)
                return;

            int td;
            if (int.TryParse(items["ticketDuration"], out td))
                _ticketDuration = td;

            int tlt;
            if (int.TryParse(items["ticketLifeTime"], out tlt))
                _ticketLifetime = tlt;

            int ad;
            if (int.TryParse(items["dailyLogItemAccessDenied"], out ad))
            {
                _isDailyLogItemAccessDenied = (ad == 1 ? true : false);
            }
        }

        static EsodaConfigurationParameters()
        {
            EsodaConfigurationParameters.refresh();
        }

    }
}
