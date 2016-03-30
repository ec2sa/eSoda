using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Pemi.Esoda.Tools;
using System.Xml;
using System.Xml.XPath;
using System.Data.Common;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class BudowaDefinicjiRejestru : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegistryDAO rd = new RegistryDAO();
            if (Session["idRejestru"] != null)
            {
                int regId = int.Parse(Session["idRejestru"].ToString());
                if (rd.RegHasData(regId))
                {
                    WebMsgBox.Show(this, "Nie można edytować definicji tego rejestru, ponieważ rejestr zawiera dane");
                    Response.Redirect("~/Aplikacje/PanelAdministracyjny/ListaRejestrow.aspx", true);
                }
            }

            if (!IsPostBack)
            {
                if (Session["idDefinicji"] != null)
                {
                    int regDefId = int.Parse(Session["idDefinicji"].ToString());
                    XmlDocument doc = new XmlDocument();
                    string def = string.Empty;

                    using (DbDataReader dr = (DbDataReader)(new RegistryDAO()).GetRegistryDefinition(regDefId))
                    {
                        if (dr.Read())
                        {
                            def = dr["definicja"].ToString();
                            doc.LoadXml(def);
                            SetRegDef(doc);
                        }
                        else
                            SetRegDef(GetRegDef());
                    }
                }
                else
                {
                    // insert
                }
                LoadRegTree();
            }
        }

        /// <summary>
        /// Gets a registry definition from Session. If one does not exist, creates new, empty definition.
        /// </summary>
        /// <returns>XML document containing registry definition</returns>
        XmlDocument GetRegDef()
        {
            XmlDocument doc;
            if (Session["RegDefinition"] == null)
            {
                doc = new XmlDocument();
                doc.AppendChild(doc.CreateXmlDeclaration("1.0", null, null));
                doc.AppendChild(doc.CreateElement("definicjaRejestru"));
                XmlNode pola = doc.CreateElement("pola");
                doc.SelectSingleNode("/definicjaRejestru").AppendChild(pola);
                XmlNode wyszukiwanie = doc.CreateElement("wyszukiwanie");
                doc.SelectSingleNode("/definicjaRejestru").AppendChild(wyszukiwanie);
                Session["RegDefinition"] = doc;
            }
            else
            {
                doc = (XmlDocument)Session["RegDefinition"];
            }
            return doc;
        }

        public int SearchFieldCount
        {
            get
            {
                XmlDocument doc = GetRegDef();
                int count = 0;

                foreach (XmlNode node in doc.SelectNodes("//pole"))
                {
                    if (!node.HasChildNodes)
                    {
                        string nazwa = node.Attributes["nazwa"].Value;
                        string etykieta = node.Attributes["etykieta"].Value;

                        if (doc.SelectSingleNode(string.Format("//kryterium[@etykieta='{0}' or @etykieta='{1}']", nazwa, etykieta)) != null)
                        {
                            count++;
                        }
                    }
                }

                return count;
            }
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);
        }

        /// <summary>
        /// Stores the registry definition after applied changes.
        /// </summary>
        /// <param name="doc">XML document with registry definition after applying changes</param>
        void SetRegDef(XmlDocument doc)
        {
            SetCreateColumnAttribute(ref doc);
            Session["RegDefinition"] = doc;
        }

        /// <summary>
        /// Loads registry definition to a treeview and sets the root node by rewriting nodes to new root node.
        /// </summary>
        void LoadRegTree()
        {
            litRegPreview.Text = Server.HtmlEncode(GetRegDef().OuterXml.ToString());
            xmldsStrukturaRejestru.Data = GetRegDef().OuterXml;
            tvStrukturaRejestru.DataBind();
            TreeNode rootNode = new TreeNode("DEFINICJA", "-1");
            while (tvStrukturaRejestru.Nodes.Count > 0)
            {
                foreach (TreeNode node in tvStrukturaRejestru.Nodes)
                {
                    if (node.Value != "-1")
                    {
                        tvStrukturaRejestru.Nodes.Remove(node);
                        TreeNode newNode = new TreeNode(node.Text, node.Value);
                        CopyChildNodes(newNode, node);
                        rootNode.ChildNodes.Add(newNode);
                        break;
                    }
                }
            }
            tvStrukturaRejestru.Nodes.Clear();
            tvStrukturaRejestru.Nodes.Add(rootNode);
            tvStrukturaRejestru.Nodes[0].ExpandAll();
            tvStrukturaRejestru.Nodes[0].Select();
            LoadGroups();
        }

        /// <summary>
        /// Recursively copies child nodes from source node to target node (all but the root node in this case).
        /// </summary>
        /// <param name="tNode"></param>
        /// <param name="sNode"></param>
        private void CopyChildNodes(TreeNode tNode, TreeNode sNode)
        {
            foreach (TreeNode node in sNode.ChildNodes)
            {
                if (node.Value != "-1")
                {
                    TreeNode ntNode = new TreeNode(node.Text, node.Value);
                    CopyChildNodes(ntNode, node);
                    tNode.ChildNodes.Add(ntNode);
                }
            }
        }

        /// <summary>
        /// Checks if node with given attribute 'nazwa' exists in specified nodes collection
        /// </summary>
        /// <param name="node">Given node</param>
        /// <param name="childs">Given node collection</param>
        /// <returns>True, if collection contains a node, otherwise false</returns>
        bool NodeListContains(XmlNode node, XmlNodeList childs)
        {
            foreach (XmlNode child in childs)
            {
                if (node.Attributes["nazwa"].Value == child.Attributes["nazwa"].Value)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Creates a list with nodes and leafs that the selected element can be moved to.
        /// </summary>
        void LoadGroups()
        {
            ddlGroupsList.Items.Clear();
            if (tvStrukturaRejestru.SelectedNode != null)
            {
                XmlDocument doc = GetRegDef();
                XmlNodeList list = doc.SelectNodes("//pole");
                XmlNodeList selected = doc.SelectNodes(string.Format("//pole[@nazwa='{0}']//pole", tvStrukturaRejestru.SelectedNode.Value));
                //XmlNodeList selectedChilds = null;

                //if (selected != null)
                //    selectedChilds = selected.ChildNodes;

                foreach (XmlNode node in list)
                {
                    if (node.Attributes["nazwa"].Value != tvStrukturaRejestru.SelectedNode.Value && !NodeListContains(node, selected)) //node.HasChildNodes)
                    {
                        ListItem item = new ListItem(node.Attributes["nazwa"].Value, node.Attributes["nazwa"].Value);
                        ddlGroupsList.Items.Add(item);
                    }
                }
                ddlGroupsList.Items.Insert(0, new ListItem("Węzeł główny", "top_parent_node"));
            }
        }

        /// <summary>
        /// Adds new element to registry definition
        /// </summary>
        protected void lnkAddNewElement_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                XmlDocument doc = GetRegDef();
                if (doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", txtNazwaElementu.Text)) == null)
                {
                    XmlElement pole = doc.CreateElement("pole");
                    pole.SetAttribute("nazwa", txtNazwaElementu.Text);
                    pole.SetAttribute("typ", ddlTypElementu.SelectedValue);
                    pole.SetAttribute("wymagane", (ckbRequiredField.Checked) ? "1" : "0");
                    pole.SetAttribute("prefiks", txtPrefix.Text);
                    pole.SetAttribute("zawijaj", (ckbWrapAfterElement.Checked) ? "1" : "0");
                    pole.SetAttribute("etykieta", txtEtykieta.Text);
                    pole.SetAttribute("tworzKolumny", (ckbCreateColumns.Checked) ? "1" : "0");

                    if (tvStrukturaRejestru.SelectedNode == null || tvStrukturaRejestru.SelectedNode.Value.Equals("-1"))
                    {
                        doc.SelectSingleNode("/definicjaRejestru/pola").AppendChild(pole);
                        //doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", tvStrukturaRejestru.SelectedNode.Value)).AppendChild(pole);
                    }
                    else
                    {
                        // TODO
                        doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", tvStrukturaRejestru.SelectedNode.Value)).AppendChild(pole);
                    }

                    if (ckbUseAsFilter.Checked)
                    {
                        XmlElement kryterium = doc.CreateElement("kryterium");
                        string etykieta = (txtEtykieta.Text.Length > 0) ? txtEtykieta.Text : txtNazwaElementu.Text;
                        kryterium.SetAttribute("etykieta", etykieta);
                        kryterium.SetAttribute("xpath", "");
                        doc.SelectSingleNode("/definicjaRejestru/wyszukiwanie").AppendChild(kryterium);
                        doc.SelectSingleNode(string.Format("//kryterium[@etykieta='{0}']", etykieta)).Attributes["xpath"].Value = string.Format("contains(wpis/{0}/text(),{{0}})", GetElementPath(pole));
                    }
                    SetRegDef(doc);
                    LoadRegTree();

                    lnkAddElement.Visible = true;
                    lnkAddNewElement.Visible = false;
                    lnkSaveElement.Visible = true;
                }
                else
                    WebMsgBox.Show(this, string.Format("Element o nazwie '{0}' już istnieje.", txtNazwaElementu.Text));
            }
        }

        /// <summary>
        /// Saves selected and modified registry definition element
        /// </summary>   
        protected void lnkSaveElement_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string prevName = string.Empty;
                XmlDocument doc = GetRegDef();
                if (tvStrukturaRejestru.SelectedNode != null && !tvStrukturaRejestru.SelectedNode.Value.Equals("-1"))
                {
                    if (doc.SelectNodes(string.Format("//pole[@nazwa='{0}']", txtNazwaElementu.Text)).Count == 0 || txtNazwaElementu.Text.Equals(tvStrukturaRejestru.SelectedNode.Value))
                    {
                        XmlNode current = doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", tvStrukturaRejestru.SelectedNode.Value));
                        prevName = current.Attributes["nazwa"].Value;
                        current.Attributes["nazwa"].Value = txtNazwaElementu.Text;
                        current.Attributes["etykieta"].Value = txtEtykieta.Text;
                        if (!current.HasChildNodes)
                        {
                            current.Attributes["typ"].Value = ddlTypElementu.Text;
                            current.Attributes["wymagane"].Value = (ckbRequiredField.Checked) ? "1" : "0";
                            current.Attributes["prefiks"].Value = txtPrefix.Text;
                            current.Attributes["zawijaj"].Value = (ckbWrapAfterElement.Checked) ? "1" : "0";
                            if (current.Attributes["tworzKolumny"] != null)
                            {
                                //current.Attributes["tworzKolumny"].RemoveAll();
                                current.Attributes.RemoveNamedItem("tworzKolumny");
                            }
                        }
                        else
                        {
                            if (current.Attributes["tworzKolumny"] == null)
                            {
                                XmlAttribute attr = doc.CreateAttribute("tworzKolumny");
                                attr.Value = (ckbCreateColumns.Checked) ? "1" : "0";
                                current.Attributes.Append(attr);
                            }
                            else
                                current.Attributes["tworzKolumny"].Value = (ckbCreateColumns.Checked) ? "1" : "0";
                            //current.Attributes["tworzKolumny"].Value = (ckbCreateColumns.Checked) ? "1" : "0";
                        }

                        XmlNode currParent = current.ParentNode;
                        //currParent.ReplaceChild(pole, current);  
                        string etykieta = (txtEtykieta.Text.Length > 0) ? txtEtykieta.Text : txtNazwaElementu.Text;
                        if (ckbUseAsFilter.Checked)
                        {
                            if (doc.SelectSingleNode(string.Format("//kryterium[@etykieta='{0}' ]", prevName)) == null)
                            {
                                XmlElement kryterium = doc.CreateElement("kryterium");
                                kryterium.SetAttribute("etykieta", etykieta);
                                kryterium.SetAttribute("xpath", "");
                                doc.SelectSingleNode("/definicjaRejestru/wyszukiwanie").AppendChild(kryterium);
                                string xPathCriterion=string.Format("contains(wpis/{0}/text(),{{0}})", GetElementPath(current));
                                //XmlNode criterionToDelete=doc.SelectSingleNode("/definicjaRejestru/wyszukiwanie/kryterium[1]/[@xpath='"+xPathCriterion+"']");
                                //if (criterionToDelete != null)
                                //    criterionToDelete.ParentNode.RemoveChild(criterionToDelete);

                                doc.SelectSingleNode(string.Format("//kryterium[@etykieta='{0}']", etykieta)).Attributes["xpath"].Value = xPathCriterion;
                            }
                            else
                            {
                                
                                doc.SelectSingleNode(string.Format("//kryterium[@etykieta='{0}']", prevName)).Attributes["etykieta"].Value = etykieta;
                                doc.SelectSingleNode(string.Format("//kryterium[@etykieta='{0}']", etykieta)).Attributes["xpath"].Value = string.Format("contains(wpis/{0}/text(),{{0}})", GetElementPath(current));
                            }
                        }
                        else
                        {
                            XmlNode kryterium = doc.SelectSingleNode(string.Format("//kryterium[@etykieta='{0}' or @etykieta='{1}']", tvStrukturaRejestru.SelectedNode.Value, current.Attributes["etykieta"].Value));
                            XmlNode parent = (kryterium != null) ? kryterium.ParentNode : null;
                            if (parent != null && kryterium != null)
                            {
                                parent.RemoveChild(kryterium);
                            }
                        }
                        SetRegDef(doc);
                        LoadRegTree();
                    }
                    else
                        WebMsgBox.Show(this, string.Format("Element o nazwie '{0}' już istnieje. Użyj innej nazwy dla nowego elementu.", txtNazwaElementu.Text));
                }
            }
        }

        /// <summary>
        /// Gets full XPATH value for a specified node in a XML document.
        /// </summary>
        /// <param name="node">Given node</param>
        /// <returns>Node path</returns>
        string GetElementPath(XmlNode current)
        {
            XmlNode node = current;
            string path = string.Empty;
            while (node.ParentNode != null && node.Attributes["nazwa"] != null)
            {
                if (path.Length > 0)
                    path = path.Insert(0, "/");
                path = path.Insert(0, node.Attributes["nazwa"].Value);
                node = node.ParentNode;
            }

            return path;
        }

        /// <summary>
        /// Sets the 'tworzKolumny' attribute for nodes (not leafs), after modifying the definition.
        /// </summary>
        /// <param name="node">Nodde from which setting starts</param>
        /// <param name="value">Value for 'tworzKolumny'</param>
        void SetParentAttribute(XmlNode node, string value)
        {
            if (node != null && node.HasChildNodes)
            {
                if (node.Attributes["tworzKolumny"] != null)
                {
                    node.Attributes["tworzKolumny"].Value = value;
                }

                if (node.ParentNode != null && !node.ParentNode.Name.Equals("pola"))
                    SetParentAttribute(node.ParentNode, node.Attributes["tworzKolumny"].Value);

            }
        }

        /// <summary>  
        /// Sets a 'tworzKolumny' attribute for each node (if such not exists).
        /// Also removes that attribute in all leafs.
        /// Validates ocurrences of that attribute in registry definition.
        /// </summary>
        /// <param name="doc">XML document, to which the changes applies (registry definition)</param>
        void SetCreateColumnAttribute(ref XmlDocument doc)
        {
            foreach (XmlNode node in doc.SelectNodes("//pole"))
            {
                if (node.HasChildNodes)
                {
                    if (node.Attributes["tworzKolumny"] == null)
                    {
                        XmlAttribute attr = doc.CreateAttribute("tworzKolumny");

                        foreach (XmlNode child in node.ChildNodes)
                        {
                            if (child.Attributes["tworzKolumny"] != null)
                            {
                                if (child.Attributes["tworzKolumny"].Value.Equals("1"))
                                {
                                    attr.Value = "1";
                                    break;
                                }
                                else
                                    attr.Value = "0";
                            }
                        }

                        node.Attributes.Append(attr);
                    }
                    else
                    {
                        //foreach (XmlNode child in node.ChildNodes)
                        //{
                        //    if (child.Attributes["tworzKolumny"] != null)
                        //    {
                        //        if (child.Attributes["tworzKolumny"].Value.Equals("1"))
                        //        {
                        //            node.Attributes["tworzKolumny"]
                        //            break;
                        //        }
                        //        else
                        //            attr.Value = "0";
                        //    }
                        //}
                    }
                }
                else
                    node.Attributes.RemoveNamedItem("tworzKolumny");
            }

            /*foreach (XmlNode node in doc.SelectNodes("//pole"))
            {
                if(node.Attributes["tworzKolumny"] != null)
                    SetParentAttribute(node, node.Attributes["tworzKolumny"].Value);
            }*/
        }

        /// <summary>
        /// Removes selected element from registry definition.
        /// </summary>
        protected void lnkRemoveElement_Click(object sender, EventArgs e)
        {
            if (tvStrukturaRejestru.SelectedNode != null && !tvStrukturaRejestru.SelectedNode.Value.Equals("-1"))
            {
                XmlDocument doc = GetRegDef();
                XmlNode node = doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", tvStrukturaRejestru.SelectedNode.Value));
                XmlNode kryterium = doc.SelectSingleNode(string.Format("//kryterium[@etykieta='{0}' or @etykieta='{1}']", tvStrukturaRejestru.SelectedNode.Value, node.Attributes["etykieta"].Value));
                XmlNode parent = node.ParentNode;
                if (parent != null && node != null)
                {
                    parent.RemoveChild(node);
                    if (parent.ChildNodes.Count == 0)
                    {
                        if (parent.Attributes["tworzKolumny"] != null)
                        {
                            //parent.Attributes["tworzKolumny"].RemoveAll();
                            parent.Attributes.RemoveNamedItem("tworzKolumny");
                        }
                    }
                }

                if (parent != null && kryterium != null)
                {
                    parent = kryterium.ParentNode;
                    parent.RemoveChild(kryterium);
                }
                lnkAddElement_Click(sender, e);
                SetRegDef(doc);
                LoadRegTree();

                lnkAddElement.Visible = true;
                lnkAddNewElement.Visible = false;
                lnkSaveElement.Visible = false;
            }
            else
                WebMsgBox.Show(this, "Wybierz element do usunięcia");
        }

        /// <summary>
        /// Loads selected registry definition element data.
        /// </summary>
        protected void tvStrukturaRejestru_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (tvStrukturaRejestru.SelectedNode != null && !tvStrukturaRejestru.SelectedNode.Value.Equals("-1"))
            {
                LoadGroups();
                XmlDocument doc = GetRegDef();
                XmlNode elem = doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", tvStrukturaRejestru.SelectedNode.Value));
                if (elem != null)
                {
                    lnkAddElement.Visible = true;
                    lnkAddNewElement.Visible = false;
                    lnkSaveElement.Visible = true;

                    txtNazwaElementu.Text = elem.Attributes["nazwa"].Value;
                    txtEtykieta.Text = elem.Attributes["etykieta"].Value;
                    //ckbCreateColumns.Checked = elem.Attributes["tworzKolumny"].Value.Equals("1");
                    if (!elem.HasChildNodes)
                    {
                        if (elem.Attributes["typ"] != null)
                            ddlTypElementu.SelectedValue = elem.Attributes["typ"].Value;
                        if (elem.Attributes["wymagane"] != null)
                            ckbRequiredField.Checked = elem.Attributes["wymagane"].Value.Equals("1");
                        if (elem.Attributes["prefiks"] != null)
                            txtPrefix.Text = elem.Attributes["prefiks"].Value;
                        if (elem.Attributes["zawijaj"] != null)
                            ckbWrapAfterElement.Checked = elem.Attributes["zawijaj"].Value.Equals("1");
                        ckbCreateColumns.Enabled = false;
                        string etykieta = (txtEtykieta.Text.Length > 0) ? txtEtykieta.Text : txtNazwaElementu.Text;
                        ckbUseAsFilter.Checked = (doc.SelectSingleNode(string.Format("//kryterium[@etykieta='{0}']", etykieta)) != null);
                    }
                    else
                    {
                        if (elem.Attributes["tworzKolumny"] == null)
                        {
                            XmlAttribute attr = doc.CreateAttribute("tworzKolumny");
                            attr.Value = "0";
                            elem.Attributes.Append(attr);
                        }
                        ckbCreateColumns.Checked = elem.Attributes["tworzKolumny"].Value.Equals("1");
                        ckbCreateColumns.Enabled = true;
                    }
                    ddlTypElementu.Enabled = ckbRequiredField.Enabled = txtPrefix.Enabled = ckbWrapAfterElement.Enabled = ckbUseAsFilter.Enabled = !elem.HasChildNodes;
                }
            }
        }

        /// <summary>
        /// Saves the registry definition. If one does not exist yet, it ads a new one.
        /// </summary>
        protected void lnkSaveDefinition_Click(object sender, EventArgs e)
        {
            int defId = -1;
            if (Session["idDefinicji"] == null)
            {
                defId = (new RegistryDAO()).AddRegistryDefinition(GetRegDef().OuterXml);
                if (defId > 0)
                    Session["idDefinicji"] = defId;
                else
                    Session["idDefinicji"] = null;
            }
            else
            {
                defId = int.Parse(Session["idDefinicji"].ToString());
                (new RegistryDAO()).UpdateRegistryDefinition(defId, GetRegDef().OuterXml);
            }
            WebMsgBox.Show(this, "Definicja rejestru została zapisana.");
        }

        /// <summary>
        /// Moves the selected element to the element chosen from available nodes to move to.
        /// </summary>
        protected void lnkMoveToGroup_Click(object sender, EventArgs e)
        {
            if (ddlGroupsList.SelectedValue != null && !tvStrukturaRejestru.SelectedNode.Value.Equals("-1"))
            {
                XmlDocument doc = GetRegDef();

                if (ddlGroupsList.SelectedValue.Equals("top_parent_node"))
                {
                    // przypisz do głównego węzła
                    XmlNode temp = doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", tvStrukturaRejestru.SelectedNode.Value)).Clone();
                    lnkRemoveElement_Click(sender, e);
                    doc.SelectSingleNode("//pola").AppendChild(temp);
                }
                else
                {
                    // przypisz do wybranego
                    string nodeName = ddlGroupsList.SelectedValue;
                    XmlNode selected = doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", nodeName));
                    XmlNode temp = doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", tvStrukturaRejestru.SelectedNode.Value)).Clone();
                    if (selected.Attributes["tworzKolumny"] != null && temp.Attributes["tworzKolumny"] != null)
                    {
                        if (selected.Attributes["tworzKolumny"].Value.Equals("0") && temp.Attributes["tworzKolumny"].Value.Equals("1"))
                        {
                            WebMsgBox.Show(this, "Węzeł, który przenosisz, ma zaznaczoną opcję tworzenia kolumn dla elementów potomnych, podczas gdy węzeł do którego przenosisz, nie ma ustawionej tej opcji. Ustaw tworzenie kolumn w węźle nadrzednym, aby móc przenieść do niego wybrany węzeł.");
                        }
                        else
                        {
                            lnkRemoveElement_Click(sender, e);
                            doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", nodeName)).AppendChild(temp);
                        }
                    }
                    else
                    {
                        lnkRemoveElement_Click(sender, e);
                        doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", nodeName)).AppendChild(temp);
                    }
                }
                SetRegDef(doc);
                LoadRegTree();
            }
        }

        /// <summary>
        /// Prepares GUI to add new element to the selected one (or root).
        /// </summary>
        protected void lnkAddElement_Click(object sender, EventArgs e)
        {
            txtNazwaElementu.Enabled = true;
            txtPrefix.Enabled = true;
            txtEtykieta.Enabled = true;
            txtNazwaElementu.Text = txtPrefix.Text = txtEtykieta.Text = "";

            ddlTypElementu.Enabled = true;

            ckbCreateColumns.Checked = false;
            ckbCreateColumns.Enabled = false;
            ckbRequiredField.Enabled = true;
            ckbUseAsFilter.Enabled = true;
            ckbWrapAfterElement.Enabled = true;

            ckbCreateColumns.Checked = ckbRequiredField.Checked = ckbUseAsFilter.Checked = ckbWrapAfterElement.Checked = false;

            lnkAddNewElement.Visible = true;
            lnkAddElement.Visible = false;
            lnkSaveElement.Visible = false;
        }

        /// <summary>
        /// Moves selected element one place above. Moving element is possible only within the parent on the current depth level.
        /// </summary>
        protected void lnkMoveUp_Click(object sender, EventArgs e)
        {
            if (tvStrukturaRejestru.SelectedNode != null && !tvStrukturaRejestru.SelectedNode.Value.Equals("-1"))
            {
                XmlDocument doc = GetRegDef();
                XmlNode selected = doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", tvStrukturaRejestru.SelectedNode.Value));
                XmlNode prevNode = selected.PreviousSibling;
                if (prevNode != null)
                {
                    XmlNode parent = selected.ParentNode;
                    XmlNode tmp = selected.Clone();
                    parent.InsertBefore(tmp, selected);
                    parent.ReplaceChild(prevNode, selected);
                    SetRegDef(doc);
                    LoadRegTree();
                }
            }
            else
                WebMsgBox.Show(this, "Wybierz element do przesunięcia");
        }

        /// <summary>
        /// Moves selected element one place below. Moving element is possible only within the parent on the current depth level.
        /// </summary>
        protected void lnkMoveDown_Click(object sender, EventArgs e)
        {
            if (tvStrukturaRejestru.SelectedNode != null && !tvStrukturaRejestru.SelectedNode.Value.Equals("-1"))
            {
                XmlDocument doc = GetRegDef();
                XmlNode selected = doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", tvStrukturaRejestru.SelectedNode.Value));
                XmlNode nextNode = selected.NextSibling;
                if (nextNode != null)
                {
                    XmlNode parent = selected.ParentNode;
                    XmlNode tmp = selected.Clone();
                    parent.InsertAfter(tmp, selected);
                    parent.ReplaceChild(nextNode, selected);
                    SetRegDef(doc);
                    LoadRegTree();
                }
            }
            else
                WebMsgBox.Show(this, "Wybierz element do przesunięcia");
        }

        /// <summary>
        /// Validates if parent and childs of the selected node have valid 'tworzKolumny' attribute values.
        /// Shows a message, if they don't.
        /// </summary>
        protected void ckbCreateColumns_CheckedChanged(object sender, EventArgs e)
        {
            if (tvStrukturaRejestru.SelectedNode != null && !tvStrukturaRejestru.SelectedNode.Value.Equals("-1"))
            {
                XmlDocument doc = GetRegDef();
                XmlNode selected = doc.SelectSingleNode(string.Format("//pole[@nazwa='{0}']", tvStrukturaRejestru.SelectedNode.Value));
                if (selected != null)
                {
                    if (selected.ParentNode != null && !selected.ParentNode.Name.Equals("pola"))
                    {
                        if (selected.ParentNode.Attributes["tworzKolumny"].Value.Equals("0") && ckbCreateColumns.Checked)
                        {
                            WebMsgBox.Show(this, "Element nadrzedny nie tworzy kolumn. Ustaw tworzenie kolumn najpierw w elemencie nadrzednym");
                            ckbCreateColumns.Checked = false;
                        }
                    }

                    foreach (XmlNode child in selected.ChildNodes)
                    {
                        if (child.HasChildNodes)
                        {
                            if (child.Attributes["tworzKolumny"].Value.Equals("1") && !ckbCreateColumns.Checked)
                            {
                                WebMsgBox.Show(this, string.Format("Element podrzędny '{0}' tworzy kolumny. Aby odznaczyć tworzenie kolumn dla bieżącego elementu, najpierw odznacz tę opcję w elementach podrzędnych.", child.Attributes["nazwa"].Value));
                                ckbCreateColumns.Checked = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        protected void ckbUseAsFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                if (SearchFieldCount >= 3)
                {
                    WebMsgBox.Show(this, "W rejestrach można wyszukiwać dane po maks. 3 kryteriach");
                    ((CheckBox)sender).Checked = false;
                }
            }
            //WebMsgBox.Show(this, "Ilość pól: " + SearchFieldCount.ToString());
        }


    }
}