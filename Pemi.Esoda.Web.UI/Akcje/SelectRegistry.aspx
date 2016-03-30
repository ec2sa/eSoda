<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="SelectRegistry.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Akcje.SelectRegistry" Title="Przypisanie do rejestru" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<h3>Proszę wybrac rejestr, do którego będzie dokonany wpis</h3>
<asp:LinkButton runat="server" ID="anuluj" Text="Powrót" onclick="anuluj_Click" />
<hr />
<table>
<tr>
<td><asp:Label runat="server" ID="lblAvailableYear" AssociatedControlID="dostepneLata" Text="Wybierz rok:" /></td>
<td><asp:DropDownList runat="server" ID="dostepneLata" AutoPostBack="True" DataTextField="Description" DataValueField="Id" 
        onselectedindexchanged="dostepneLata_SelectedIndexChanged" /></td>
</tr>
<tr>
<td><asp:Label runat="server" ID="lblRejestry" Text="Wybierz rejestr:" AssociatedControlID="rejestry" /></td>
<td><asp:DropDownList runat="server" ID="rejestry"></asp:DropDownList></td>
</tr>
<tr><td colspan="2"><asp:LinkButton runat="server" ID="select" Text="Wybierz" onclick="select_Click" /> </td></tr>
</table>
<p runat="server" id="message" class="errorMsg">Nie ma żadnego dostępnego rejestru dla wybranego dokumentu/sprawy lub dostępne rejestry zawierają już wpis dotyczący wybranego dokumentu/sprawy</p>


</asp:Content>

