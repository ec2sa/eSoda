<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacjeRejestrow.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.OperacjeRejestrow" %>
<ul class="opcje">
 <li>
    <asp:LinkButton runat="server" ID="lnkRegistryDetails" Text="Szczegóły rejestru" OnCommand="wykonaj"
      CommandArgument="~/Aplikacje/PanelAdministracyjny/edycjaRejestru.aspx" ToolTip="Zarządzanie rejestrami" /></li>
  <li>
    <asp:LinkButton runat="server" ID="lnkRegistryDefinition" Text="Struktura rejestru" OnCommand="wykonaj"
      CommandArgument="~/Aplikacje/PanelAdministracyjny/BudowaDefinicjiRejestru.aspx" ToolTip="Zarządzanie definicjami rejestrami" /></li>
 <li>
         <asp:LinkButton ID="lnkRegistry" runat="server" Text="Powrót" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/listaRejestrow.aspx" ToolTip="Zarządzanie rejestrami" />
 </li> 
 </ul>
<hr />