<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacjePaneluAdministracyjnego.ascx.cs"
    Inherits="Pemi.Esoda.Web.UI.Controls.OperacjePaneluAdministracyjnego" %>
<h2>
    Panel administracyjny /
    <%=Page.Title %>
</h2>
<ul class="opcje">
    <li>
        <asp:LinkButton runat="server" ID="LinkButton1" Text="Pracownicy" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/EdycjaUzytkownikow.aspx" 
            ToolTip="Zarz¹dzanie pracownikami" /></li>
    <li>
        <asp:LinkButton runat="server" ID="LinkButton2" Text="Wydzia³y i role" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/EdycjaGrup.aspx" 
            ToolTip="Zarz¹dzanie struktur¹ organizacyjn¹" /></li>
    <li>
        <asp:LinkButton runat="server" ID="LinkButton8" Text="Zarz¹dzanie rolami" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/EdycjaUprawnien.aspx" 
            ToolTip="Zarz¹dzanie rolami pracowników (poziomy uprawnieñ)" /></li>
    <li>
        <asp:LinkButton runat="server" ID="LinkButton3" Text="JRWA" OnCommand="wykonaj" CommandArgument="~/Aplikacje/PanelAdministracyjny/EdycjaJRWA.aspx"
            ToolTip="Zarz¹dzanie struktur¹ JRWA" /></li>
    <li>
        <asp:LinkButton runat="server" ID="LinkButton6" Text="Dokumenty" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/rodzajeDokumentow.aspx" 
            ToolTip="Zarz¹dzanie kategoriami i rodzajami dokumentów" /></li>
    <li>
        <asp:LinkButton runat="server" ID="LinkButton5" Text="Sprawy" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/zarzadzanieRodzajamiSpraw.aspx"
            ToolTip="Zarz¹dzanie rodzajami spraw" /></li>
    <li>
        <asp:LinkButton runat="server" ID="lnkDocCaseFullList" Text="Lista dokumentów/spraw"
            OnCommand="wykonaj" CommandArgument="~/Aplikacje/PAnelAdministracyjny/listaWszystkichDokumentowISpraw.aspx"
            ToolTip="Lista wszystkich dokumentów i spraw w systemie" /></li>
    <li>
        <asp:LinkButton ID="LinkButton9" runat="server" OnCommand="wykonaj" CommandArgument="~/Aplikacje/PanelAdministracyjny/edycjaTeczek.aspx"
            ToolTip="Zarz¹dzanie teczkami">Teczki</asp:LinkButton>
    </li>
    <li>
        <asp:LinkButton runat="server" ID="LinkButton4" Text="Import u¿ytkowników i ról"
            OnCommand="wykonaj" CommandArgument="~/Aplikacje/PanelAdministracyjny/importuzytkownikow.aspx"
            ToolTip="Import pracowników i ich przypisañ do wydzia³ów " /></li>
    <li>
        <asp:LinkButton Visible="false" runat="server" ID="LinkButton11" Text="TEST-edycja interesantów"
            OnCommand="wykonaj" CommandArgument="~/Aplikacje/PanelAdministracyjny/edycjainteresanta.aspx"
            ToolTip="testowa formatka do edycji interesantow - do doprecyzowania " /></li>
    <li>
        <asp:LinkButton runat="server" ID="LinkButton12" Text="Zarz¹dzanie interesantami"
            OnCommand="wykonaj" CommandArgument="~/Aplikacje/PanelAdministracyjny/ZarzadzanieInteresantami.aspx"
            ToolTip="Edycja interesantow i ich kategorii" />
    </li>
    <li>
        <asp:LinkButton runat="server" ID="LinkButton13" Text="Akcje" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/zarzadzanieAkcjami.aspx" 
            ToolTip="Zarz¹dzanie akcjami" /></li>
    <li>
        <asp:LinkButton ID="lnkRegistry" runat="server" Text="Rejestry" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/listaRejestrow.aspx" ToolTip="Zarz¹dzanie rejestrami" />
    </li>
    <li>
        <asp:LinkButton ID="lnkDailyLog" runat="server" Text="Dziennik kancelaryjny" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/DziennikKancelaryjny.aspx" ToolTip="Zarz¹dzanie Dziennikiem Kancelaryjnym" />
    </li>
    <li>
        <asp:LinkButton ID="LinkButton15" runat="server" Text="Historia logowania" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/HistoriaLogowania.aspx" ToolTip="Historia logowania do systemu" />
    </li>
    <li>
        <asp:LinkButton runat="server" ID="LinkButton10" Text="Log b³êdów" OnCommand="wykonaj"
            CommandArgument="~/elmah.axd" 
            ToolTip="Nieobs³u¿one b³êdy w trakcie dzia³ania aplikacji" /></li>
    <li>
        <asp:LinkButton ID="linkbutton19" runat="server" Text="Dane Urzêdu" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/DaneUrzedu.aspx" ToolTip="Dane o urzêdzie" />
    </li>
    <li>
        <asp:LinkButton ID="linkbutton14" runat="server" Text="Komunikat" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/Komunikat.aspx" ToolTip="Komunikat na stronie logowania" />
    </li>
     <li>
        <asp:LinkButton ID="linkbutton16" runat="server" Text="Szablon MS Office" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/SzablonMSO.aspx" ToolTip="Szablon MS Office" />
    </li>
     <li>
        <asp:LinkButton ID="linkbutton17" runat="server" Text="Dni wolne" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/DniWolneOdPracy.aspx" ToolTip="Dni wolne od pracy" />
    </li>
    <li>
        <asp:LinkButton ID="lblNowyRok" runat="server" Text="Nowy rok" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/NowyRok.aspx" ToolTip="Nowy rok" />
    </li>  
    <li>
        <asp:LinkButton ID="lblSMSConfig" runat="server" Text="Powiadomienia SMS" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/SMSConfig.aspx" ToolTip="Konfiguracja powiadomieñ SMS" />
    </li>    
    <li>
        <asp:LinkButton ID="lblDataMatrixPrint" runat="server" Text="Drukowanie kodu" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/DataMatrixPrint.aspx" ToolTip="Konfiguracja strony wydruku kodu" />
    </li>  
    <li>
        <asp:LinkButton ID="lbOCRConfig" runat="server" Text="Konfiguracja OCR" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/OCRConfiguration.aspx" ToolTip="Konfiguracja OCR" />
    </li>
     <li>
        <asp:LinkButton ID="lbRSSConfig" runat="server" Text="Konfiguracja RSS" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/RSSConfiguration.aspx" ToolTip="Konfiguracja RSS" />
    </li>
      <li>
        <asp:LinkButton ID="lbLogos" runat="server" Text="Konfiguracja szaty graficznej" OnCommand="wykonaj"
            CommandArgument="~/Aplikacje/PanelAdministracyjny/LayoutConfiguration.aspx" ToolTip="Konfiguracja szaty graficznej aplikacji" />
    </li>
 
    <li>
        <asp:LinkButton runat="server" ID="LinkButton7" Text="powrót" OnCommand="wykonaj"
            CommandArgument="~/OczekujaceZadania.aspx" />
    </li>
</ul>
<hr />
