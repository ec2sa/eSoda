using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Pemi.Esoda.DataAccess;
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Core.Domain
{
	public class Statystyki
	{
		protected static StatystykiDAC dac{
			get{
				return new StatystykiDAC();
			}
		}
		public static List<Pemi.Esoda.DTO.StatystykaDTO> ListaStatystyk()
		{
			return dac.ListaStatystyk();
		}

		public static System.Data.DataSet WywolanieStatystyki(string procedura, params object[] parametry)
		{
			return dac.WywolanieStatystyki(procedura, parametry);
		}

		public static XmlReader WywolanieStatystykiXml(string procedura, params object[] parametry)
		{
			return dac.WywolanieStatystykiXml(procedura, parametry);
		}

        public static Collection<SimpleLookupDTO> ListaStatusowSpraw(string zrodloDanych)
        {
            return dac.ListaStatusowSpraw(zrodloDanych);
        }
	}
}
