<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/SingleColumn.Master"CodeBehind="edycjaUprawnien.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.edycjaUprawnien" Title="Zarz¹dzanie rolami pracowników (poziomy uprawnieñ)" %>
<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
<div id="singleColumn">
<fieldset>
<legend>Zarz¹dzanie rolami u¿ytkowników:</legend>
    <table>
    <tr>
    <td><fieldset>
    <legend>U¿ytkownicy</legend><asp:Panel ID="pnlUsers" runat="server" Height="400px">
    <asp:ListBox CssClass="ramka" ID="lstUsers" runat="server" SelectionMode="Multiple" Height="390px" ></asp:ListBox>
    </asp:Panel>
    </fieldset>
    </td>
    <td style="text-align:center;">
        <asp:LinkButton ID="btnAddUserToRole" runat="server" Text="» przypisz »" OnClick="btnAddUserToRole_Click" /><br />
        <asp:LinkButton ID="btnAddUsersToRole" runat="server" Text="»»" OnClick="btnAddUsersToRole_Click" Enabled="False" Visible="False" /><br />
        <asp:LinkButton ID="btnRemoveUserFromRole" runat="server" Text="« usuñ «" OnClick="btnRemoveUserFromRole_Click" /><br />
        <asp:LinkButton ID="btnRemoveUsersFromRole" runat="server" Text="««" OnClick="btnRemoveUsersFromRole_Click" Enabled="False" Visible="False" /><br />
    </td>
    <td><fieldset>
    <legend>Role</legend>
    <asp:Panel ID="pnlRoles" runat="server" Height="400px" Width="260px" ScrollBars="Auto">
    <asp:ListBox ID="lstRoles" CssClass="ramka" runat="server" SelectionMode="Multiple" Height="380px" ></asp:ListBox>
    </asp:Panel>
    </fieldset>
    </td>    
        <td>
           <fieldset><legend>Aktualne przypisania do ról</legend>
           <asp:Panel ID="Panel1" runat="server" Height="400px" Width="260px" ScrollBars="Auto" BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px">
        <asp:TreeView ID="tvURTree" runat="server" ImageSet="Arrows" Font-Size="Small" OnSelectedNodeChanged="tvURTree_SelectedNodeChanged">
            <SelectedNodeStyle CssClass="treeSelectedNode"/>
        </asp:TreeView>
            </asp:Panel>
        </fieldset>
        </td>
    </tr>
    </table>
   </fieldset>
   </div>
    </asp:Content>