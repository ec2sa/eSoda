<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="Komunikat.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.Komunikat" Title="Komunikat" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="opa1" runat="server" />
<asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
<fieldset>
<legend>Komunikat na stronie logowania</legend>
<asp:ValidationSummary runat="server" ValidationGroup="SaveChanges" DisplayMode="BulletList" />
<br />
<table>
<tr>
    <td><asp:Label runat="server" Text="Data pocz¹tkowa:" AssociatedControlID="tbStartDate" /></td>
    <td>
        <asp:TextBox runat="server" ID="tbStartDate" />
        <ajax:CalendarExtender ID="tbStartDate_CalendarExtender" runat="server" TargetControlID="tbStartDate" />
        <asp:CompareValidator runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="tbStartDate" ValidationGroup="SaveChanges" Text="*" ErrorMessage="B³êdny format daty pocz¹tkowej" />
    </td>
    <td><asp:Label ID="Label1" runat="server" Text="Data koñcowa:" AssociatedControlID="tbEndDate" /></td>
    <td>
        <asp:TextBox runat="server" ID="tbEndDate" />
        <ajax:CalendarExtender ID="tbEndDate_CalendarExtender" runat="server" TargetControlID="tbEndDate" />
        <asp:CompareValidator ID="CompareValidator1" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="tbEndDate" ValidationGroup="SaveChanges" Text="*" ErrorMessage="B³êdny format daty koñcowej" />
    </td>
</tr>
<tr>    
<td><asp:Label ID="Label2" runat="server" Text="Treœæ komunikatu:" AssociatedControlID="tbNotice" /></td>
<td colspan="3"><asp:TextBox runat="server" ID="tbNotice" TextMode="MultiLine" Columns="50" Rows="8" /></td>
</tr>
<tr>
<td>&nbsp;</td><td colspan="3"><asp:Label runat="server" Text="Aktywny" AssociatedControlID="cbIsActive" />
<asp:CheckBox runat="server" ID="cbIsActive" />
</td>
</tr>
<tr><td colspan="4"><asp:LinkButton runat="server" ID="btnSaveNotice" Text="Zapisz" 
        onclick="btnSaveNotice_Click" ValidationGroup="SaveChanges" /></td></tr>
</table>
</fieldset>
</asp:Content>