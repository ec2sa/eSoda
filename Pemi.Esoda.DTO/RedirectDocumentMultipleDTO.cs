using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.DTO
{
   public class RedirectItem
    {
        private int itemID;
        private int documentID;
        private int organizationalUnitID;
        private string organizationalUnitName;
        private int employeeID;
        private string employeeName;
        private string note;
        private bool workOnPaper;
        private bool commandID;
        private bool allHistory;
        private bool allScans;

        public RedirectItem(int documentID, int organizationalUnitID, string organizationalUnitName, int employeeID, string employeeName, string note, bool workOnPaper, bool commandID, bool allHistory, bool allScans)
        {
            //this.itemID = itemID;
            this.documentID = documentID;
            this.organizationalUnitName = organizationalUnitName;
            this.organizationalUnitID = organizationalUnitID;
            this.employeeName = employeeName;
            this.employeeID = employeeID;
            this.note = note;
            this.workOnPaper = workOnPaper;
            this.commandID = commandID;
            this.allHistory = allHistory;
            this.allScans = allScans;
        }

        public int ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }

        public int DocumentID
        {
            get { return documentID; }
            set { documentID = value; }
        }

        public int OrganizationalUnitID
        {
            get { return organizationalUnitID; }
            set { organizationalUnitID = value; }
        }

        public string OrganizationalUnitName
        {
            get { return organizationalUnitName; }
            set { organizationalUnitName = value; }
        }

        public int EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        public bool WorkOnPaper
        {
            get { return workOnPaper; }
            set { workOnPaper = value; }
        }

        public bool CommandID
        {
            get { return commandID; }
            set { commandID = value; }
        }

        public bool AllHistory
        {
            get { return allHistory; }
            set { allHistory = value; }
        }

        public bool AllScans
        {
            get { return allScans; }
            set { allScans = value; }
        }
    }
}

