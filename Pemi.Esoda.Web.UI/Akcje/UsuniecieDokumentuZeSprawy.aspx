<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsuniecieDokumentuZeSprawy.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Akcje.UsuniecieDokumentuZeSprawy" MasterPageFile="~/MasterPages/SingleColumn.Master" %>

<asp:Content runat="server" ID="mainContent" ContentPlaceHolderID="main">
    <h2>Usuwanie dokumentu nr <%=Session["docToRemoveFromCase"]  %> ze sprawy.</h2>
    <fieldset>
    <legend>Uwaga</legend>
    <asp:Panel runat="server" ID="todo">
      <p class="errorMsg">Wykonanie tej akcji spowoduje usunięcie dokumentu ze sprawy i jego przeniesienie na Twoje konto.</p>
      <p class="errorMsg">Jeżeli usuwany dokument jest jedynym w sprawie, wówczas sprawa zostanie usunięta.</p>
      <br />
      <asp:LinkButton runat="server" ID="btnRemove" Text="Wykonaj" OnClick="executeAction" />
      <asp:LinkButton runat="server" ID="btnCancel" Text="Anuluj"  OnClick="cancelAction"/>
      </asp:Panel>
      <asp:Panel runat="server" ID="done">
      <asp:Label runat="server" ID="successMsg" />
      <br /><br />
       <asp:LinkButton runat="server" ID="btnReturn" Text="Powrót"  OnClick="returnFromAction"/>
      </asp:Panel>
    </fieldset>
</asp:Content>
   