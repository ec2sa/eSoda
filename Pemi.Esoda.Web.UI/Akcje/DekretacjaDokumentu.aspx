<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
  Codebehind="DekretacjaDokumentu.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Akcje.DekretacjaDokumentu"
  Title="Dekretowanie dokumentu" %>
  <%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
  <%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<esoda:ContextItem id="ContextItem2" runat="server"></esoda:ContextItem>
  <h2>
    <%=Page.Title %>
  </h2>
  <esoda:DocumentOperations runat="Server" ID="DocumentOperations1" />
  <hr />
  <fieldset>
    <legend>Dekretacja dokumentu</legend>
    <table>
      <tr>
        <th>
          <asp:Label ID="Label6" runat="server" AssociatedControlID="wydzial"
            Text="Wydzia³" /></th>
        <td>
          <asp:DropDownList runat="server" ID="wydzial" AutoPostBack="true" OnSelectedIndexChanged="obslugaZmianyWydzialu" /></td>
      </tr>
      <tr>
        <th>
          <asp:Label ID="Label9" runat="server" AssociatedControlID="pracownik"
            Text="Pracownik" /></th>
        <td>
          <asp:DropDownList runat="server" ID="pracownik" /></td>
      </tr>
        <tr>
            <td colspan="2">
                <asp:CustomValidator ID="cvDekretacja" runat="server" Display="Dynamic" ErrorMessage="Musi byæ okreœlony wydzia³ lub pracownik"></asp:CustomValidator></td>
        </tr>
        <tr>
        <th><asp:Label runat="server" AssociatedControlID="cbPaper" Text="Praca na wersji papierowej" /></th>
        <td><asp:CheckBox runat="server" ID="cbPaper" /></td>
        </tr>
        <tr>
        <th>Notatka</th>
        <td>
        <asp:TextBox runat="server" ID="txtNote" TextMode="MultiLine" Rows="4" Columns="40" /></td></tr>
      <tr><td colspan="2">
      <asp:LinkButton runat="server" ID="wykonaj" Text="zadekretuj" />
      <asp:LinkButton runat="server" ID="anuluj" Text="anuluj" OnClick="anuluj_Click" />
      </td></tr>
    </table>
  </fieldset>
</asp:Content>
