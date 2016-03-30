using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Pemi.Esoda.DTO
{
    public enum DocumentItemCategory { Uploaded='U', Created='C' };

    public class DocumentItemDTO
    {
        private bool browsable;

        private Guid id;

        public Guid ID
        {
            get { return this.id; }
        }
        private Guid fsguid;

        public Guid FSGUID
        {
            get { return fsguid; }
            set { fsguid = value; }
        }

        private string originalName;

        public string OriginalName
        {
            get { return originalName; }
        }

        private string description;

        public string Description
        {
            get { return description; }
        }

        private string mimeType;

        public string MimeType
        {
            get { return mimeType; }
        }

        private bool isMain;

        public bool IsMain
        {
            get { return isMain; }
        }

        private DocumentItemCategory category;

        public DocumentItemCategory Category
        {
            get { return category; }
        }

        public bool Browsable
        {
            get { return this.browsable; }
        }

        private Guid originalItemID;

        public Guid OriginalItemID
        {
            get { return originalItemID; }
        }

        private List<DocumentItemDTO> previousVersions;

        public Collection<DocumentItemDTO> PreviousVersions
        {
            get { return new Collection<DocumentItemDTO>(previousVersions); }
        }

        private DateTime creationDate;

        public DateTime CreationDate
        {
            get { return creationDate; }
        }

        public DocumentItemDTO(Guid id, Guid fsguid, string originalName, string mimeType, string description, bool isMain, bool isBrowsable)
            :
            this(id, fsguid, originalName, mimeType, description, isMain, isBrowsable, Guid.Empty, DateTime.Now) { }

        public DocumentItemDTO(Guid id, Guid fsguid, string originalName, string mimeType, string description, bool isMain, bool isBrowsable, DocumentItemCategory category)
            :this(id, fsguid, originalName, mimeType, description, isMain, isBrowsable, Guid.Empty, DateTime.Now, category) { }

        public DocumentItemDTO(Guid id, Guid fsguid, string originalName, string mimeType, string description, bool isMain, bool isBrowsable, Guid originalItemId, DateTime creationDate)
        {
            this.id = id;
            this.fsguid = fsguid;
            this.originalName = originalName;
            this.mimeType = mimeType;
            this.description = description;
            this.isMain = isMain;
            this.browsable = isBrowsable;
            this.originalItemID = originalItemId;
            this.creationDate = creationDate;
            this.previousVersions = new List<DocumentItemDTO>();
            this.category = DocumentItemCategory.Uploaded;// "U";//uploaded
        }

        public DocumentItemDTO(Guid id, Guid fsguid, string originalName, string mimeType, string description, bool isMain, bool isBrowsable, Guid originalItemId, DateTime creationDate, DocumentItemCategory category)
        {
            this.id = id;
            this.fsguid = fsguid;
            this.originalName = originalName;
            this.mimeType = mimeType;
            this.description = description;
            this.isMain = isMain;
            this.browsable = isBrowsable;
            this.originalItemID = originalItemId;
            this.creationDate = creationDate;
            this.previousVersions = new List<DocumentItemDTO>();
            this.category = category;
        }

    }


}
