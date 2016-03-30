using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Presenters
{
	public interface ISessionProvider
	{
		object this[string name] { get;set;}
		object this[int index] { get;set;}
		bool Contains(string name);
		bool Contains(int index);
		void Remove(string name);
		void Clear();
		void Abandon();
	} 
}
