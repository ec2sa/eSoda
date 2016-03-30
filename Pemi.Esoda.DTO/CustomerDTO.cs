using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class CustomerDTO : BaseDTO
    {
        private int _customerTypeId;

        public int CustomerTypeId
        {
            get { return _customerTypeId; }
            set { _customerTypeId = value; }
        }

        private string _customerTypeName;

        public string CustomerTypeName
        {
            get { return _customerTypeName; }
            set { _customerTypeName = value; }
        }

        private int? _customerCategory;

        public int? CustomerCategory
        {
            get { return _customerCategory; }
            set { _customerCategory = value; }
        }

        private string _name;

        private string _firstName;

        public string FirstName
        {
            get { return _firstName;/* _idTypuInteresanta == 1 ? _imie : null;*/ }
            set { _firstName = value == "" ? null : value; }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName;/*_idTypuInteresanta==1?_nazwisko:null;*/ }
            set { _lastName = value == "" ? null : value; }
        }

        public string Name
        {
            get { return _name;/* _idTypuInteresanta > 1 ? _nazwa : null; */}
            set { _name = value == "" ? null : value; }
        }

        public string Nip { get; set; }

        public string NumberSMS { get; set; }

        private AddressDTO _address = new AddressDTO();

        public AddressDTO Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public CustomerDTO(int customerTypeID, string customerTypeName, AddressDTO address)
        {
            this._customerTypeId = customerTypeID;
            this._customerTypeName = customerTypeName;
            if (address != null)
                this._address = address;
        }

        public CustomerDTO(int customerTypeID, string customerTypeName) : this(customerTypeID, customerTypeName, null) { }

        public CustomerDTO(int customerTypeID) : this(customerTypeID, null, null) { }
    }
}
