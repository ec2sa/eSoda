using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public abstract class BaseDTO
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { this._id = value; } 
        }
    }
}
