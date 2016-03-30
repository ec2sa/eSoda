using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Tasks
{
    public interface IAddItemScanTask
    {
        string GetAvailableScans(string location, string device, string documentType, string source);
        SkanyDataset GetAvailableScansList();
        int GetIncomingScansCount();
        void AssignScanWithRegistryItem(string fileName, int ownerID, int registryID, int itemNumber, string documentName,
        string documentDescription, string elementDescription, bool isMain,bool isRF);
        void AssignExistingScanWithRegistryItem(string fileName, int registryID, int itemNumber,
        string elementDescription, bool isMain, bool isRF);
    }
}
