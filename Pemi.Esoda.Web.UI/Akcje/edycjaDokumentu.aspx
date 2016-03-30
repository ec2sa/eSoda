<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" AutoEventWireup="true"
  Codebehind="edycjaDokumentu.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Akcje.edycjaDokumentu"
  Title="Zmiana danych dokumentu" %>
  <%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
  <%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
  <%@ Register Src="~/Controls/Interesant.ascx" TagName="Interesant" TagPrefix="esoda" %>
<%@ Register Src="~/Controls/ListaInteresantow.ascx" TagName="ListaInteresantow" TagPrefix="esoda" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<esoda:ContextItem id="ContextItem2" runat="server"></esoda:ContextItem>
  <h2>
    <%=Page.Title %>
  </h2>
  <esoda:DocumentOperations runat="Server" ID="DocumentOperations1" />
  <hr />
  <div id="leftColumn">
  <fieldset>
    <legend>Edycja danych dokumentu</legend>
  <table class="grid">
    <tr>
      <th>Interesant</th>
      <td>
        <asp:DropDownList runat="server" ID="interesant" DataSourceID="dsInteresant" DataValueField="id"
          DataTextField="pelnaNazwa" OnDataBound="interesant_DataBound" 
              Visible="False" />
          <br />
          <asp:Label ID="lblInteresant" runat="server"></asp:Label>
          <asp:HiddenField ID="hfCustomerId" runat="server" />
          <br />
          <asp:LinkButton ID="btnZmien" runat="server" onclick="btnZmien_Click">Zmieñ</asp:LinkButton>
      </td>
    </tr>
    <tr>
      <th>Status</th>
      <td>
        <asp:DropDownList runat="server" ID="status" DataSourceID="dsStatus" DataTextField="nazwa"
          DataValueField="id" Enabled="false" /></td>
    </tr>
    <tr>
      <th>Znak pisma</th>
      <td>
        <asp:TextBox runat="server" ID="znakPisma" /></td>
    </tr>
    <tr>
      <th>Kategoria</th>
      <td>
        <asp:DropDownList runat="server" ID="kategoria" DataSourceID="dsKategorie" DataValueField="id"
          DataTextField="nazwa" AutoPostBack="true" OnDataBound="kategoria_DataBound" /></td>
    </tr>
    <tr>
      <th>Rodzaj</th>
      <td>
      <asp:UpdatePanel runat="server" UpdateMode="Conditional">
      <ContentTemplate>
        <asp:DropDownList runat="server" ID="rodzaj" DataSourceID="dsRodzaje" DataValueField="id"
          DataTextField="nazwa"  />
      </ContentTemplate>
      <Triggers><asp:AsyncPostBackTrigger ControlID="kategoria" EventName="SelectedIndexChanged" /></Triggers>
      </asp:UpdatePanel>
      </td>
    </tr>
    <tr>
    <th>Data pisma</th>
    <td><asp:TextBox runat="server" ID="txtDataPisma" Enabled="false" /></td>
    </tr>
    <tr>
    <th>Opis</th>
    <td><asp:TextBox runat="server" ID="txtOpis" Enabled="false" /></td>
    </tr>
  </table>
  <asp:LinkButton runat="server" ID="save" Text="zapisz zmiany" OnClick="saveClick" />
  <asp:LinkButton runat="server" ID="cancel" Text="anuluj" OnClick="cancelClick" /></fieldset>
  <asp:SqlDataSource ID="dsInteresant" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
    SelectCommand="[Uzytkownicy].[listaInteresantow]" SelectCommandType="StoredProcedure">
  </asp:SqlDataSource>
  
  <asp:SqlDataSource ID="dsStatus" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
    SelectCommand="[Dokumenty].[listaStatusow]" SelectCommandType="StoredProcedure">
  </asp:SqlDataSource>
  
  <asp:SqlDataSource runat="server" ID="dsKategorie" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
    SelectCommand="[Dokumenty].[listaKategorii]" SelectCommandType="StoredProcedure" />
  
  <asp:SqlDataSource runat="server" ID="dsRodzaje" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
    SelectCommand="[Dokumenty].[listaRodzajow]" SelectCommandType="StoredProcedure">
    <SelectParameters>
    <asp:ControlParameter ControlID="kategoria" Name="idKategorii" PropertyName="SelectedValue" DefaultValue="0" Type="int32" />
    </SelectParameters>
  </asp:SqlDataSource>
  </div>
  <div id="rightColumn">
    <asp:LinkButton ID="lnkSearchAgain" runat="server" Text="Wyszukaj ponownie" 
          Visible="false" onclick="lnkSearchAgain_Click" />
        <esoda:ListaInteresantow Visible="false" ID="customersList" runat="server" />
        <asp:LinkButton ID="lnkSelectCustomer" runat="server" Text="Wybierz" 
          Visible="false" onclick="lnkSelectCustomer_Click" />
        <esoda:Interesant Visible="false" ID="customer" runat="server" />
  </div>
</asp:Content>
