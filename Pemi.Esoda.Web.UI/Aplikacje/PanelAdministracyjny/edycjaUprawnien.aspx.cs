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
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class edycjaUprawnien : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // role
                LoadRolesList();

                // uzytkownicy
                LoadUsersList();

                // drzewo
                LoadRoleUsersTree();
            }
         }

        private void LoadUsersList()
        {
            lstRoles.DataSource = (new RolesDAO()).GetRoles();
            lstRoles.DataValueField = "idRoliAsp";
            lstRoles.DataTextField = "nazwa";
            lstRoles.DataBind();
        }

        private void LoadRolesList()
        {
            lstUsers.DataSource = (new UserDAO()).GetEmployeeList();
            lstUsers.DataValueField = "idTozsamosci";
            lstUsers.DataTextField = "etykieta";
            lstUsers.DataBind();
        }

        private void LoadRoleUsersTree()
        {
            tvURTree.Nodes.Clear();
            if (lstRoles.Items.Count > 0)
            {
                foreach (ListItem rola in lstRoles.Items)
                {
                    TreeNode node = new TreeNode(rola.Text, rola.Value);
                    tvURTree.Nodes.Add(node);
                }
            }

            if (lstUsers.Items.Count > 0)
            {
                foreach(TreeNode roleNode in tvURTree.Nodes)
                {
                    foreach (ListItem userItem in lstUsers.Items)
                    {
                      
                      if (Roles.IsUserInRole(Membership.GetUser(new Guid(userItem.Value)).UserName, roleNode.Text))
                        {
                            TreeNode node = new TreeNode(userItem.Text, userItem.Value);
                            roleNode.ChildNodes.Add(node);
                        }
                    }
                }
            }
            tvURTree.ExpandAll();
        }

        private bool CheckSelections()
        {
            if (lstUsers.GetSelectedIndices().Length <= 0)
            {
                WebMsgBox.Show(this, "Wybierz u¿ytkownika");
                return false;
            }
            if (lstRoles.GetSelectedIndices().Length <= 0)
            {
                WebMsgBox.Show(this, "Wybierz rolê");
                return false;
            }
            return true;
        }

        protected void btnAddUserToRole_Click(object sender, EventArgs e)
        {
            if (CheckSelections())
            {
                foreach (int selectedUser in lstUsers.GetSelectedIndices())
                {
                    MembershipUser user = Membership.GetUser(new Guid(lstUsers.Items[selectedUser].Value));
                    foreach (int selectedRole in lstRoles.GetSelectedIndices())
                    {
                        if (!Roles.IsUserInRole(user.UserName, lstRoles.Items[selectedRole].Text))
                            Roles.AddUserToRole(user.UserName, lstRoles.Items[selectedRole].Text);
                    }
                }
                lstRoles.SelectedIndex = -1;
                lstUsers.SelectedIndex = -1;
                LoadRoleUsersTree();
            }           
        }

        protected void btnAddUsersToRole_Click(object sender, EventArgs e)
        {
            if (CheckSelections())
            {
                LoadRoleUsersTree();
            }
        }

        protected void btnRemoveUserFromRole_Click(object sender, EventArgs e)
        {
            if (CheckSelections())
            {
                foreach (int selectedUser in lstUsers.GetSelectedIndices())
                {
                    MembershipUser user = Membership.GetUser(new Guid(lstUsers.Items[selectedUser].Value));
                    if (!user.UserName.Equals("admin"))
                    {
                        foreach (int selectedRole in lstRoles.GetSelectedIndices())
                        {
                            if (Roles.IsUserInRole(user.UserName, lstRoles.Items[selectedRole].Text))
                                Roles.RemoveUserFromRole(user.UserName, lstRoles.Items[selectedRole].Text);
                        }
                    }
                }
                lstRoles.SelectedIndex = -1;
                lstUsers.SelectedIndex = -1;
                LoadRoleUsersTree();
            }
        }

        protected void btnRemoveUsersFromRole_Click(object sender, EventArgs e)
        {
            if (CheckSelections())
            {
                LoadRoleUsersTree();
            }
        }

        protected void tvURTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            lstUsers.SelectedIndex = -1;
            lstRoles.SelectedIndex = -1;
            TreeNode node = tvURTree.SelectedNode;
            if (node.Parent != null)
            {
                lstRoles.SelectedValue = node.Parent.Value;
                lstUsers.SelectedValue = node.Value;
            }
            else
            {
                lstRoles.SelectedValue = node.Value;
            }
        }
    }
}
