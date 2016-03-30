using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Core.Domain.Interfaces
{
    public interface ICase: IMetadata
    {
        /// <summary>
        /// Nazwa sprawy
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Opis sprawy
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Data założenia sprawy w systemie
        /// </summary>
        DateTime CreationDate { get; }

        /// <summary>
        /// Właściciel - założyciel sprawy w systemie
        /// </summary>
        string Owner { get; }

       
    }
}
