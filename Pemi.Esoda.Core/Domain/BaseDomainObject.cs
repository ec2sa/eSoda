using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Core.Domain
{
	public abstract class BaseDomainObject<T>
	{
		private T id = default(T);

		protected bool isModified;

		public bool IsModified
		{
			get { return this.isModified; }
		}
		
		public T ID
		{
			get { return id; }
		}

		public bool IsNew
		{
			get { return ID == null || ID.Equals(default(T)); }
		}

		public BaseDomainObject(T id):this()
		{
			this.id = id;
		}

		public BaseDomainObject()
		{
			this.isModified = false;
		}

	}
}
