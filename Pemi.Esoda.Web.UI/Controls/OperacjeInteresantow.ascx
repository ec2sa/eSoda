<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacjeInteresantow.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.OperacjeInteresantow" %>
<ul class="opcje">
  <li>
    <asp:LinkButton runat="server" ID="LinkButton1" Text="Kategorie interesantów" OnCommand="wykonaj"
      CommandArgument="~/Aplikacje/PanelAdministracyjny/EdycjaKategoriiInteresantow.aspx" ToolTip="Zarz¹dzanie kategoriami interesantów" /></li>
  <li>
    <asp:LinkButton runat="server" ID="LinkButton2" Text="Interesanci" OnCommand="wykonaj"
      CommandArgument="~/Aplikacje/PanelAdministracyjny/EdycjaInteresantow.aspx" ToolTip="Zarz¹dzanie interesantami" /></li>
        <li>
        <asp:LinkButton ID="lbHistoriaInteresantow" runat="server" Text="Historia interesantów" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/HistoriaInteresantow.aspx" ToolTip="Historia zmian danych interesantów" />
    </li>
</ul>
<hr />