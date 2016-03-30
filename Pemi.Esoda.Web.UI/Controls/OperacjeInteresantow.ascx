<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacjeInteresantow.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.OperacjeInteresantow" %>
<ul class="opcje">
  <li>
    <asp:LinkButton runat="server" ID="LinkButton1" Text="Kategorie interesant�w" OnCommand="wykonaj"
      CommandArgument="~/Aplikacje/PanelAdministracyjny/EdycjaKategoriiInteresantow.aspx" ToolTip="Zarz�dzanie kategoriami interesant�w" /></li>
  <li>
    <asp:LinkButton runat="server" ID="LinkButton2" Text="Interesanci" OnCommand="wykonaj"
      CommandArgument="~/Aplikacje/PanelAdministracyjny/EdycjaInteresantow.aspx" ToolTip="Zarz�dzanie interesantami" /></li>
        <li>
        <asp:LinkButton ID="lbHistoriaInteresantow" runat="server" Text="Historia interesant�w" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/HistoriaInteresantow.aspx" ToolTip="Historia zmian danych interesant�w" />
    </li>
</ul>
<hr />