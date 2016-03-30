using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using System.IO;

namespace Pemi.Esoda.Presenters
{
    public class AddItemScanPresenter : BasePresenter
    {
        private IAddItemScanView view;
        private IAddItemScanTask service;
        private ISessionProvider session;
        private int itemId
        {
            get
            {
                int tmp;
                if (!session.Contains("addingScanToItem")) return 0;
                if (!int.TryParse(session["addingScanToItem"].ToString(), out tmp)) return 0;
                return tmp;
            }
        }

        private int registryId
        {
            get
            {
                int tmp;
                if (!session.Contains("addingScanToRegistry")) return 0;
                if (!int.TryParse(session["addingScanToRegistry"].ToString(), out tmp)) return 0;
                return tmp;
            }
        }

        public AddItemScanPresenter(IAddItemScanView view, ISessionProvider session)
        {
            this.view = view;
            this.session = session;
            this.service = new AddItemScanTask();
        }

        public void SearchForScans(string location, string device, string documentType, string source)
        {
            //return;
            // to poni¿ej przenieœæ do odsLIstaSkanow
            location = ((location != "*") ? "='" + location + "'" : "");
            device = ((device != "*") ? "='" + device + "'" : "");
            documentType = ((documentType != "*") ? "='" + documentType + "'" : "");
            source = ((source != "*") ? "='" + source + "'" : "");
            string availableScans = service.GetAvailableScans(location, device, documentType, source);
            service.GetAvailableScansList();
            string xpathQuery = string.Format("/dokumenty/dokument[pochodzenie/lokalizacja{0} and pochodzenie/urzadzenie{1} and pochodzenie/rodzaj{2} and pochodzenie/zrodlo{3}]", location, device, documentType, source);
            view.BindScans(availableScans, xpathQuery);
        }


        public override void Initialize()
        {
            view.BindConditions();
            view.ItemId = itemId;
            ((IView)view).ViewTitle = "Skojarzanie skanów z pozycj¹ dziennika kancelaryjnego";
        }

        protected override void subscribeToEvents()
        {

        }

        protected override void redirectToPreviousView()
        {

        }

        public void AssignScanWithRegistryItem(string[] fileNames, string elementDescription, bool isMain,bool isRF)
        {
            bool isGuid = false;
            Guid g;
            if (fileNames.Length == 1)
            {
                try
                {
                    g = new Guid(fileNames[0]);
                    isGuid = true;
                }
                catch
                {
                }
            }

            if (isGuid)
            {
                service.AssignExistingScanWithRegistryItem(fileNames[0], registryId, itemId, elementDescription, isMain,isRF);
                return;
            }

            foreach (string fileName in fileNames)
            {
                service.AssignScanWithRegistryItem(fileName, 3, registryId, itemId, "skan dokumentu", null, elementDescription, isMain,isRF);

                string usuwanePliki = Path.GetFileNameWithoutExtension(fileName);
                foreach (string plik in Directory.GetFiles(Path.GetDirectoryName(fileName), usuwanePliki + "*.*"))
                {
                    File.Delete(plik);
                }
            }
        }
    }
}
