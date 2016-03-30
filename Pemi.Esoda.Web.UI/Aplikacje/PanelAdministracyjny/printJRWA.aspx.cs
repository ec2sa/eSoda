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
using System.Xml;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class printJRWA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int l1=0; //, l2=0, l3=0, l4=0, l5=0;
            LoadJRWATree();

            /*
             * <xsl:attribute name="Text">
				<xsl:value-of select="symbol"/>
			</xsl:attribute>

			<xsl:attribute name="Value">
				<xsl:value-of select="nazwa"/>
			</xsl:attribute>

			<xsl:attribute name="ToolTip">
				<xsl:value-of select="kategoriaAKM"/>
			</xsl:attribute>

			<xsl:attribute name="NavigateURL">
				<xsl:value-of select="kategoriaAIK"/>
			</xsl:attribute>

			<xsl:attribute name="Target">
				<xsl:value-of select="uwagi"/>
			</xsl:attribute>
             */

            // I poziom
            foreach (TreeNode node in tvPrintJRWA.Nodes)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.Text = node.Text;
                cell.ColumnSpan = 5;

                row.Cells.Add(cell);
                //row.Cells.Add(new TableCell());
                //row.Cells.Add(new TableCell());
                //row.Cells.Add(new TableCell());
                //row.Cells.Add(new TableCell());

                // has³o
                cell = new TableCell();
                cell.Text = node.Value;
                row.Cells.Add(cell);

                // akm
                cell = new TableCell();
                cell.Text = node.ToolTip;
                row.Cells.Add(cell);

                //aik
                cell = new TableCell();
                cell.Text = node.NavigateUrl;
                row.Cells.Add(cell);

                //uwagi
                cell = new TableCell();
                cell.Text = node.Target;
                row.Cells.Add(cell);

                tabJRWA.Rows.Add(row);
                // II poziom
                foreach (TreeNode node2 in node.ChildNodes)
                {
                    l1++;
                    row = new TableRow();
                    cell = new TableCell();
                    cell.Text = node2.Text;
                    cell.ColumnSpan = 4;

                    row.Cells.Add(new TableCell());
                    row.Cells.Add(cell);
                    //row.Cells.Add(new TableCell());
                    //row.Cells.Add(new TableCell());
                    //row.Cells.Add(new TableCell());

                    cell = new TableCell();
                    cell.Text = node2.Value;
                    row.Cells.Add(cell);

                    // akm
                    cell = new TableCell();
                    cell.Text = node2.ToolTip;
                    row.Cells.Add(cell);

                    //aik
                    cell = new TableCell();
                    cell.Text = node2.NavigateUrl;
                    row.Cells.Add(cell);

                    //uwagi
                    cell = new TableCell();
                    cell.Text = node2.Target;
                    row.Cells.Add(cell);
                    
                    tabJRWA.Rows.Add(row);
                    // III poziom
                    foreach (TreeNode node3 in node2.ChildNodes)
                    {
                        row = new TableRow();
                        cell = new TableCell();
                        cell.Text = node3.Text;
                        cell.ColumnSpan = 3;

                        row.Cells.Add(new TableCell());
                        row.Cells.Add(new TableCell());
                        row.Cells.Add(cell);
                        //row.Cells.Add(new TableCell());
                        //row.Cells.Add(new TableCell());

                        cell = new TableCell();
                        cell.Text = node3.Value;
                        row.Cells.Add(cell);

                        // akm
                        cell = new TableCell();
                        cell.Text = node3.ToolTip;
                        row.Cells.Add(cell);

                        //aik
                        cell = new TableCell();
                        cell.Text = node3.NavigateUrl;
                        row.Cells.Add(cell);

                        //uwagi
                        cell = new TableCell();
                        cell.Text = node3.Target;
                        row.Cells.Add(cell);

                        tabJRWA.Rows.Add(row);
                        // IV poziom
                        foreach (TreeNode node4 in node3.ChildNodes)
                        {
                            row = new TableRow();
                            cell = new TableCell();
                            cell.Text = node4.Text;
                            cell.ColumnSpan = 2;

                            row.Cells.Add(new TableCell());
                            row.Cells.Add(new TableCell());                           
                            row.Cells.Add(new TableCell());
                            row.Cells.Add(cell);
                            //row.Cells.Add(new TableCell());

                            cell = new TableCell();
                            cell.Text = node4.Value;
                            row.Cells.Add(cell);

                            // akm
                            cell = new TableCell();
                            cell.Text = node4.ToolTip;
                            row.Cells.Add(cell);

                            //aik
                            cell = new TableCell();
                            cell.Text = node4.NavigateUrl;
                            row.Cells.Add(cell);

                            //uwagi
                            cell = new TableCell();
                            cell.Text = node4.Target;
                            row.Cells.Add(cell);

                            tabJRWA.Rows.Add(row);
                            // V poziom
                            foreach (TreeNode node5 in node4.ChildNodes)
                            {
                                row = new TableRow();
                                cell = new TableCell();
                                cell.Text = node5.Text;

                                row.Cells.Add(new TableCell());
                                row.Cells.Add(new TableCell());
                                row.Cells.Add(new TableCell());                               
                                row.Cells.Add(new TableCell());
                                row.Cells.Add(cell);
                                
                                cell = new TableCell();
                                cell.Text = node5.Value;
                                row.Cells.Add(cell);

                                // akm
                                cell = new TableCell();
                                cell.Text = node5.ToolTip;
                                row.Cells.Add(cell);

                                //aik
                                cell = new TableCell();
                                cell.Text = node5.NavigateUrl;
                                row.Cells.Add(cell);

                                //uwagi
                                cell = new TableCell();
                                cell.Text = node5.Target;
                                row.Cells.Add(cell);

                                tabJRWA.Rows.Add(row);
                            }
                        }
                    }
                }
            } 
        }

        private void LoadJRWATree()
        {
            JrwaDAO jd = new JrwaDAO();
            xmldsPrintJRWA.Data = "";
            xmldsPrintJRWA.Data = jd.GetFullJrwaList();
            
            XmlDocument doc = xmldsPrintJRWA.GetXmlDocument();
            tvPrintJRWA.Nodes.Clear();
            tvPrintJRWA.DataBind();
            tvPrintJRWA.ExpandAll();
        }

        protected void tvPrintJRWA_DataBound(object sender, EventArgs e)
        {
        }
    }
}