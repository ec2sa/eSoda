<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="PrzekierowanieDokumentu.aspx.cs" Inherits="Pemi.Esoda.Web.UI.PrzekierowanieDokumentu" Title="Przekierowanie dokumentu" %>
<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<esoda:ContextItem id="ContextItem2" runat="server"></esoda:ContextItem>
<h2><%=Page.Title %></h2>
<esoda:DocumentOperations runat="Server" ID="DocumentOperations1" />
<hr />
<fieldset runat="server" id="przekazanie" >

<legend>Przekazanie dokumentu</legend>
<%--<asp:ValidationSummary runat="server" DisplayMode="List" ValidationGroup="ActionGroup" />--%>
<table>
<tr>
<td><asp:Label ID="Label6" runat="server" AssociatedControlID="wydzial" Text="Wydzia³" /></td>
<td>
    <asp:DropDownList runat="server" ID="wydzial" AutoPostBack="true"  />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="wydzial" ErrorMessage="Nale¿y wybraæ wydzia³!" ValidationGroup="ActionGroup" InitialValue="0" />
</td>
</tr>
<tr>
<td><asp:Label ID="Label9" runat="server" AssociatedControlID="pracownik" Text="Pracownik" /></td>
<td><asp:DropDownList runat="server" ID="pracownik"/></td>
</tr>
<tr>
        <th>Notatka</th>
        <td>
        <asp:TextBox runat="server" ID="txtNote" TextMode="MultiLine" Rows="4" Columns="40" /></td></tr>
<tr>
<td colspan="2"><asp:LinkButton runat="server" ID="wykonaj" 
        ValidationGroup="ActionGroup" Text="Przekieruj dokument" 
        onclick="wykonaj_Click" /></td>
</tr>
</table>
</fieldset>
</asp:Content>

