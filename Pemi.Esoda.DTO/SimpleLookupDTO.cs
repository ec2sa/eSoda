using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.DTO
{
	public class SimpleLookupDTO:BaseDTO, ILookupDTO
	{
		protected string description;

		public SimpleLookupDTO(int id, string description)
		{
			this.ID = id;
			this.description = description;
		}

		#region ILookupDTO Members

	
	public string Description
		{
			get { return description; }
		}

		#endregion

	}
}
