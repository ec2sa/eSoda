using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class AddressDTO:BaseDTO{
        private string _city;

        public string City
        {
            get { return _city.Trim(); }
            set { _city = value; }
        }

        private string _post;

        public string Post
        {
            get { return _post!=null?_post.Trim():null; }
            set { _post = value; }
        }

        private string _postalCode;

        public string PostalCode
        {
            get { return _postalCode!=null?_postalCode.Trim():null; }
            set { _postalCode = value; }
        }

        private string _street;

        public string Street
        {
            get { return _street.Trim(); }
            set { _street = value; }
        }

        private string _building;

        public string Building
        {
            get { return _building.Trim(); }
            set { _building = value; }
        }

        private string _flat;

        public string Flat
        {
            get { return _flat.Trim(); }
            set { _flat = value; }
        }

			public AddressDTO()
			{

			}

      public AddressDTO(string postalCode, string city, string street, string building, string flat, string post)
        {
            this._postalCode = postalCode;
            this._city = city;
            this._street = street;
            this._building = building;
            this._flat = flat;
            this._post = post;
        }
			public AddressDTO(string postalCode, string city, string street, string building, string flat) : this(postalCode,city,street,building,flat,null) { }
    }
}
