using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Pemi.Esoda.DataAccess;
using System.IO;
using System.Text;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class edycjaJRWA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadJRWATree(); 
            }
            lblError.Text = "";
            lblError.ForeColor = System.Drawing.Color.Black;
        }

        private void SetError(string message)
        {
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Text = message;
        }

        private void LoadJRWATree()
        {
            try
            {
                string selected = "";
                if(tvJRWA.SelectedNode != null)
                    selected = tvJRWA.SelectedNode.ValuePath;
                
                JrwaDAO jd = new JrwaDAO();
                dsxmlJRWA.Data = jd.GetJRWAListXML();

                tvJRWA.DataBind();               

                // dodanie elementu ROOT
                TreeNode node = new TreeNode("JRWA","0");
                node.Target = "1";
                while(tvJRWA.Nodes.Count>0)
                    node.ChildNodes.Add(tvJRWA.Nodes[0]);

                foreach (TreeNode tNode in node.ChildNodes)
                    SetInactiveNodes(tNode);
                //tvJRWA.Nodes.Clear();
                //node.Selected = true;
                node.Expand();
                tvJRWA.Nodes.Add(node); 
                
                if(tvJRWA.FindNode(selected) != null)
                    tvJRWA.FindNode(selected).Select();
                ExpandNode(tvJRWA.SelectedNode);                
            }
            catch
            { }
        }

        private void SetInactiveNodes(TreeNode node)
        {
            foreach (TreeNode sNode in node.ChildNodes)
            {
                SetInactiveNodes(sNode);
            }
            if (node.Target.Equals("0"))
            {
                node.Text += " [NIEAKTYWNE]";
            }
        }

        private void ExpandNode(TreeNode node)
        {
            if (node != null)
            {
                if (node.Parent != null)
                    ExpandNode(node.Parent);
                node.Expand();
            }
        }

        private void LoadTargetJRWA(string id)
        {
            tvTargetJRWA.Nodes.Clear();

            // root nodes
            foreach (TreeNode sNode in tvJRWA.Nodes)
            {
                if (sNode.Target.Equals("1"))
                {
                    TreeNode ntNode = new TreeNode(sNode.Text, sNode.Value);
                    // child nodes
                    CopyChildNodes(ntNode, sNode, id);
                    tvTargetJRWA.Nodes.Add(ntNode);
                }
            }

            tvTargetJRWA.ExpandAll();
        }

        private void CopyChildNodes(TreeNode tNode, TreeNode sNode, string id)
        {
            foreach (TreeNode node in sNode.ChildNodes)
            {
                if (!node.Value.Equals(id) && node.Target.Equals("1"))
                {
                    TreeNode ntNode = new TreeNode(node.Text, node.Value);
                    CopyChildNodes(ntNode, node, id);
                    tNode.ChildNodes.Add(ntNode);
                }
            }
        }

        private bool IsNodeLevelValid(TreeNode node)
        {
            TreeNode lNode = node;
            int i = 5; // JRWA ma max. 5 zagnie¿d¿eñ
            while (i >= 1 && lNode.Parent!= null)
            {
                lNode = lNode.Parent;
                i--;
            }
            return (lNode.Parent == null);
        }

        private void LoadJRWA(int id)
        {
            JrwaDAO jd = new JrwaDAO();
            frmJRWA.DataSource = jd.GetJRWA(id);
            frmJRWA.DataBind();
        }

        protected void frmJRWA_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (Page.IsValid)
            {
                JrwaDAO jd = new JrwaDAO();

                string symbol = ((TextBox)frmJRWA.FindControl("txtSymbol")).Text;
                symbol = ((Label)frmJRWA.FindControl("lblPArentSymbol")).Text + symbol;
                string nazwa = ((TextBox)frmJRWA.FindControl("txtNazwa")).Text;
                string katAKM = ((TextBox)frmJRWA.FindControl("txtKategoriaAKM")).Text;
                string katAIK = ((TextBox)frmJRWA.FindControl("txtKategoriaAIK")).Text;
                string uwagi = ((TextBox)frmJRWA.FindControl("txtUwagi")).Text;
                bool bAktywna = ((CheckBox)frmJRWA.FindControl("ckbAktywna")).Checked;

                int ?idRodzica = null;
                if(tvJRWA.SelectedNode != null && !tvJRWA.SelectedValue.Equals("0"))
                    idRodzica = int.Parse(tvJRWA.SelectedNode.Value);

                if (jd.ExistsJRWA(symbol,-1))
                {
                    WebMsgBox.Show(this, string.Format("JRWA o symbolu {0} ju¿ istnieje.", symbol));
                    e.Cancel = true;
                    return;
                }
                else
                    jd.InsertJRWA(idRodzica, symbol, nazwa, katAKM, katAIK, uwagi, bAktywna);                
                LoadJRWATree();
                frmJRWA.ChangeMode(FormViewMode.ReadOnly);
                frmJRWA.DataBind();
            }
        }
        protected void frmJRWA_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                JrwaDAO jd = new JrwaDAO();

                string parentSymbol = string.Empty;

                int id = int.Parse(tvJRWA.SelectedNode.Value);
                int? idRodzica = null;

                string symbol = ((TextBox)frmJRWA.FindControl("txtSymbol")).Text;
                Label lblParentSymbol = ((Label)frmJRWA.FindControl("lblParentSymbol"));
                if (lblParentSymbol != null)
                {
                    if (lblParentSymbol.Visible && lblParentSymbol.Text.Length > 0)
                    {
                        parentSymbol = lblParentSymbol.Text;
                        symbol = lblParentSymbol.Text + symbol;
                        idRodzica = jd.GetJRWAIdBySymbol(parentSymbol);
                    }
                    else
                    {
                        parentSymbol = symbol.Substring(0, symbol.Length - 1);
                        idRodzica = jd.GetJRWAIdBySymbol(parentSymbol);
                        if (idRodzica == -1 && parentSymbol.Length > 0)
                        {
                            WebMsgBox.Show(this, string.Format("Brak aktywnego wêz³a JRWA o symbolu {0}.", parentSymbol));
                            e.Cancel = true;
                            return;
                        }
                    }
                }

                string nazwa = ((TextBox)frmJRWA.FindControl("txtNazwa")).Text;
                string katAKM = ((TextBox)frmJRWA.FindControl("txtKategoriaAKM")).Text;
                string katAIK = ((TextBox)frmJRWA.FindControl("txtKategoriaAIK")).Text;
                string uwagi = ((TextBox)frmJRWA.FindControl("txtUwagi")).Text;
                bool bAktywna = ((CheckBox)frmJRWA.FindControl("ckbAktywna")).Checked;

                

                //if(jd.Get

                HiddenField hfParentJRWAId = ((HiddenField)frmJRWA.FindControl("hfTargetJRWAParentId"));
                if(hfParentJRWAId != null && hfParentJRWAId.Value != null && hfParentJRWAId.Value.Length>0 && parentSymbol.Length==0)
                {
                    idRodzica = int.Parse(hfParentJRWAId.Value);
                }
                else                
                    if (tvJRWA.SelectedNode != null && tvJRWA.SelectedNode.Parent != null && parentSymbol.Length==0)
                    {
                        idRodzica = int.Parse(tvJRWA.SelectedNode.Parent.Value);
                    }
                
                if (idRodzica == 0)
                    idRodzica = null;

                if (parentSymbol.Length > 0)
                    idRodzica = jd.GetJRWAIdBySymbol(parentSymbol);
                else
                    idRodzica = null;

                if (jd.ExistsJRWA(symbol, id))
                {
                    WebMsgBox.Show(this, string.Format("JRWA o symbolu {0} ju¿ istnieje.", symbol));
                    e.Cancel = true;
                    return;
                }
                else
                    jd.UpdateJRWA(id, idRodzica, symbol, nazwa, katAKM, katAIK, uwagi, bAktywna);
                frmJRWA.ChangeMode(FormViewMode.ReadOnly);
                LoadJRWA(id);           
                LoadJRWATree();
            }
        }

        protected void tvJRWA_SelectedNodeChanged(object sender, EventArgs e)
        {            
            int id = int.Parse(tvJRWA.SelectedNode.Value);
            frmJRWA.ChangeMode(FormViewMode.ReadOnly);
            LoadJRWA(id);
            pnlTargetJRWA.Visible = false;
            frmJRWA.Visible = true;
        }

        protected void lnkAddJRWA_Click(object sender, EventArgs e)
        {
            frmJRWA.ChangeMode(FormViewMode.Insert);
            frmJRWA.Visible = true;
            pnlTargetJRWA.Visible = false;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (tvJRWA.SelectedNode != null)
            {
                frmJRWA.ChangeMode(FormViewMode.Edit);
                int id = int.Parse(tvJRWA.SelectedNode.Value);
                LoadJRWA(id);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            frmJRWA.ChangeMode(FormViewMode.ReadOnly);
            int id;
            if (tvJRWA.SelectedNode != null)
            {
                if (int.TryParse(tvJRWA.SelectedNode.Value, out id))
                    LoadJRWA(id);
            }
            else
                frmJRWA.DataBind();
        }

        protected void frmJRWA_ModeChanging(object sender, FormViewModeEventArgs e)
        {
           
        }

        protected void lnkExportJRWA_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", string.Format("attachment; filename=ExportJRWA_{0}.xml",DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")));
            Response.Charset = "utf-8";

            Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
            Response.ContentType = "text/xml";
            //System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            
            JrwaDAO jd = new JrwaDAO();
            string export = jd.ExportJRWA();

            //stringWrite.Write(export);

            //Response.Write(stringWrite.ToString());
            Response.Write(export);
            Response.End();
        }

        protected void lnkImportJRWA_Click(object sender, EventArgs e)
        {
            if (fuplJRWA.HasFile)
            {
                try
                {
                    string filePath = Server.MapPath(ConfigurationManager.AppSettings["katalogRoboczy"].ToString()) + "\\" + fuplJRWA.FileName;
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    fuplJRWA.SaveAs(filePath);

                    DataSet dsJRWA = new DataSet();
                    dsJRWA.ReadXml(filePath);

                    JrwaDAO jd = new JrwaDAO();
                    jd.ImportJRWA(dsJRWA.GetXml());
                    File.Delete(filePath);
                    LoadJRWATree();
                }
                catch //(Exception ex)
                {
                    SetError("Nie uda³o siê zaimportowaæ danych.");
                }
            }
            else
                SetError("Wybierz plik z danymi do zaimportowania.");
        }

        protected void lnkMove_Click(object sender, EventArgs e)
        {
            frmJRWA.Visible = false;
            pnlTargetJRWA.Visible = true;
            LoadTargetJRWA(tvJRWA.SelectedNode.Value);
        }

        protected void tvTargetJRWA_SelectedNodeChanged(object sender, EventArgs e)
        {
            pnlTargetJRWA.Visible = false;
            frmJRWA.Visible = true;
            
            // UWAGA!!! mnie jmuz dzis nie ma - musze odebrac dowod. wrzucilem co mialem .
            // OK.
            
            ((Label)frmJRWA.FindControl("lblTargetParentJRWA")).Text = tvTargetJRWA.SelectedNode.Text;
            ((HiddenField)frmJRWA.FindControl("hfTargetJRWAParentId")).Value = tvTargetJRWA.SelectedNode.Value;

            Label lblParentSymbol = ((Label)frmJRWA.FindControl("lblPArentSymbol"));
            if(lblParentSymbol != null)
            {
                lblParentSymbol.Visible = true;
                if(Session["symbol"] == null)
                    Session["symbol"] = ((TextBox)frmJRWA.FindControl("txtSymbol")).Text;
                ((TextBox)frmJRWA.FindControl("txtSymbol")).Text = ((TextBox)frmJRWA.FindControl("txtSymbol")).Text.Substring(((TextBox)frmJRWA.FindControl("txtSymbol")).Text.Length - 1);

                lblParentSymbol.Text = (new JrwaDAO()).GetJRWASymbolById(int.Parse(tvTargetJRWA.SelectedNode.Value));
            }
            ((TextBox)frmJRWA.FindControl("txtSymbol")).Width = 10;
            ((TextBox)frmJRWA.FindControl("txtSymbol")).MaxLength = 1;
            ((RangeValidator)frmJRWA.FindControl("rvSymbol")).Enabled = true;
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            pnlTargetJRWA.Visible = false;
            frmJRWA.Visible = true;
        }

        protected void tvJRWA_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            string a = e.Node.Target;
        }

        protected void tvJRWA_PreRender(object sender, EventArgs e)
        {
        }

        protected void frmJRWA_DataBound(object sender, EventArgs e)
        {
            Label parentSymbol = ((Label)frmJRWA.FindControl("lblParentSymbol"));
            if (parentSymbol != null)
            {
                int jrwaID = -1;
                if (!int.TryParse(tvJRWA.SelectedValue, out jrwaID))
                    jrwaID = 0;
                    
                parentSymbol.Text = (new JrwaDAO()).GetJRWASymbolById(jrwaID);
            }           
        }

        protected void lnkClearParent_Click(object sender, EventArgs e)
        {
            ((Label)frmJRWA.FindControl("lblTargetParentJRWA")).Text = string.Empty;
            ((HiddenField)frmJRWA.FindControl("hfTargetJRWAParentId")).Value = string.Empty;

            Label lblParentSymbol = ((Label)frmJRWA.FindControl("lblPArentSymbol"));
            if (lblParentSymbol != null)
            {
                lblParentSymbol.Visible = false;
                lblParentSymbol.Text = string.Empty; // (new JrwaDAO()).GetJRWASymbolById(int.Parse(tvTargetJRWA.SelectedNode.Value));
            }
            ((TextBox)frmJRWA.FindControl("txtSymbol")).Width = 60;
            ((TextBox)frmJRWA.FindControl("txtSymbol")).MaxLength = 5;
            if (Session["symbol"] != null)
            {
                ((TextBox)frmJRWA.FindControl("txtSymbol")).Text = Session["symbol"].ToString();
                Session["symbol"] = null;
            }
            ((RangeValidator)frmJRWA.FindControl("rvSymbol")).Enabled = false;
            
        } 
    }
}
