<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
  Codebehind="PrzekazanieSprawy.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Akcje.PrzekazanieSprawy"
  Title="Przekazanie sprawy" %>
<%@ Register TagPrefix="esoda" TagName="CaseContext" Src="~/Controls/CaseContextItem.ascx" %>
<%@ Register TagPrefix="esoda" TagName="OperacjeSprawy" Src="~/Controls/OperacjeSprawy.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<esoda:CaseContext runat="Server" ID="cc1" />
  <h2>
    <%=Page.Title %>
  </h2>
  <esoda:OperacjeSprawy runat="server" ID="ops1" />
  <hr />
  <fieldset runat="server" id="przekazanie">
    <legend>Przekazanie sprawy</legend>
    <table>
      <tr>
        <td>
          <asp:Label ID="Label6" runat="server" AssociatedControlID="wydzial" Text="Wydzia³" /></td>
        <td>
          <asp:DropDownList runat="server" ID="wydzial" AutoPostBack="true" DataSourceID="dsGroups"
            DataValueField="id" DataTextField="nazwa" /></td>
      </tr>
      <tr>
        <td>
          <asp:Label ID="Label9" runat="server" AssociatedControlID="pracownik" Text="Pracownik" /></td>
        <td>
          <asp:DropDownList runat="server" ID="pracownik" DataSourceID="dsEmployees" DataValueField="id"
            DataTextField="pelnaNazwa" OnDataBound="podpieteDane" /></td>
      </tr>
      <tr>
        <th>Notatka</th>
        <td>
          <asp:TextBox runat="server" ID="txtNote" TextMode="MultiLine" Rows="4" Columns="40" /></td>
      </tr>
      <tr>
        <td colspan="2">
          <asp:LinkButton runat="server" ID="wykonaj" Text="Przeka¿ sprawê" OnClick="executeCommand" /></td>
      </tr>
    </table>
  </fieldset>
  
  <asp:SqlDataSource runat="server" ID="dsGroups" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
    SelectCommand="uzytkownicy.listaGrup" SelectCommandType="StoredProcedure" />
  <asp:SqlDataSource runat="server" ID="dsEmployees" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
    SelectCommand="[Uzytkownicy].[listaPracownikowKomorkiOrganizacyjnej]" SelectCommandType="StoredProcedure">
    <SelectParameters>
      <asp:ControlParameter ControlID="wydzial" PropertyName="SelectedValue" Name="idKomorki" />
    </SelectParameters>
  </asp:SqlDataSource>
</asp:Content>
