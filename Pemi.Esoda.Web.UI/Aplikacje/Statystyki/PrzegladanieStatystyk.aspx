<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master"
  AutoEventWireup="true" Codebehind="PrzegladanieStatystyk.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.Statystyki.PrzegladanieStatystyk"
  Title="Przegl¹danie statystyk" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
  <h2>
    <%=Page.Title %>
  </h2>
<div id="leftNarrowColumn">
  <div id="blokListyStatystyk" class="blokListyStatystyk" runat="server">
  <h2>Lista raportów</h2>
  <ul>
  <li><asp:LinkButton runat="server" ID="rpt1" PostBackUrl="RaportDokumentowISpraw.aspx" Text="Dokumenty i sprawy pracowników" /></li>
  </ul>
    <h2>Lista statystyk</h2>
    <asp:DropDownList runat="Server" ID="listaStatystyk">
    </asp:DropDownList>
    <asp:LinkButton runat="server" ID="wyborStatystyki" Text="Wybierz" OnClick="wyborStatystyki_Click" />
  </div>
  <div id="blokParametrowStatystyki" class="blokParametrowStatystyki" runat="server">
    <asp:LinkButton runat="server" ID="powrotDoListyStatystyk" Text="« Powrót do listy"
      OnClick="powrotDoListyStatystyk_Click" />
    <h2 runat="server" id="naglowekParametrowStatystyki"></h2>
    <fieldset>
      <legend>Informacje o statystyce</legend>
      <dl>
        <dt>Tytu³</dt>
        <dd runat="server" id="nazwaStatystyki"></dd>
        <dt>Opis</dt>
        <dd runat="server" id="opisStatystyki"></dd>
      </dl>
    </fieldset>
    <fieldset>
      <legend>Proszê podaæ wartoœci parametrów</legend>
      <div runat="server" id="dynamicznePola" class="dynamicznePola"></div>
    </fieldset>
    <asp:LinkButton CssClass="linkGenerujStat" runat="server" ID="generowanieStatystyki"
      Text="Generowanie statystyki" OnClick="generowanieStatystyki_Click" />
  </div>
</div>
<div id="rightWideColumn">
  <div id="blokTresciStatystyki" class="blokTresciStatystyki" runat="server">
    <h2>Przegl¹danie wybranej statystyki</h2>
   
    <asp:Xml runat="server" ID="statystykaXml" />
  </div>
  <div id="blokBledu" class="blokBledu" runat="server">
    <h2>Ostatnia operacja zakoñczy³a siê niepowodzeniem.</h2>
  </div>
  </div>
  <asp:DropDownList>
  <asp:ListItem Text="" Value=""></asp:ListItem>
  </asp:DropDownList>
</asp:Content>
