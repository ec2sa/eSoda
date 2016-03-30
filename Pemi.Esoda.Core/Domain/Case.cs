using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Core.Domain.Interfaces;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Tools;
using Pemi.Esoda.DTO;
using System.Xml.XPath;
using System.Xml;
using System.Data.Common;

namespace Pemi.Esoda.Core.Domain
{
    public class Case:BaseDomainObject<int>, ICase
    {
        private CaseDAO dao = new CaseDAO();
        
        private string name;
        private string description;
        private DateTime creationDate;
        private string owner;
        private Metadata meta;

        public Case() : base() { }

        public Case(int id)
            : base(id)
        {
            XmlReader xr = dao.GetCase(id);
            if (!xr.Read())
                throw new Exception("Wybrana sprawa nie istnieje");
            try
            {
                XPathDocument xpd = new XPathDocument(xr);
                XPathNavigator xpn = xpd.CreateNavigator();
                this.name = xpn.SelectSingleNode("/sprawa/nazwa") == null ? string.Empty : xpn.SelectSingleNode("/sprawa/nazwa").Value;
                this.description = xpn.SelectSingleNode("/sprawa/opis") == null ? string.Empty : xpn.SelectSingleNode("/sprawa/opis").Value;

                DateTime tmpDate;
                if (DateTime.TryParse(xpn.SelectSingleNode("/sprawa/dataRozpoczecia").Value, out tmpDate))
                    this.creationDate = tmpDate;

                this.owner = (xpn.SelectSingleNode("/sprawa/metadane/referent") == null) ? string.Empty : xpn.SelectSingleNode("/sprawa/metadane/referent").Value;
                if (this.owner.Equals(String.Empty))
                {
                    this.owner = (xpn.SelectSingleNode("/sprawa/referent") == null) ? string.Empty : xpn.SelectSingleNode("/sprawa/metadane/referent").Value;
                }
                //XPathIteratorReader xpir = new XPathIteratorReader(xpn.Select("/dokument/metadane"));

                meta = new Metadata(xpn.Select("/sprawa/metadane/*"));
            }
            catch {
                throw new Exception("Wybrana sprawa nie istnieje");
            }

        }

        public Case(CaseDTO _case)
            : this()
        {
            this.name = _case.Name;
            this.owner = _case.Owner;
            this.description = _case.Description;
            this.creationDate = _case.CreationDate;
            this.meta = new Metadata(_case.Metadata);
        }

        public CaseDTO GetCaseData()
        {
            return new CaseDTO(this.ID, this.name, this.description, this.owner, this.creationDate, this.meta.Items);
        }

        
        #region ICase Members

        string ICase.Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        string ICase.Description
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        DateTime ICase.CreationDate
        {
            get { throw new NotImplementedException(); }
        }

        string ICase.Owner
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IMetadata Members

        Pemi.Esoda.Tools.Metadata IMetadata.Content
        {
            get { throw new NotImplementedException(); }
        }

        void IMetadata.Replace(Pemi.Esoda.Tools.Metadata newMetadata)
        {
            throw new NotImplementedException();
        }

        void IMetadata.Merge(Pemi.Esoda.Tools.Metadata metadataToMerge)
        {
            throw new NotImplementedException();
        }

        #endregion

       
    }
}
