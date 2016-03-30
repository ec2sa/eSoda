<%@ Page Title="Aplikacje" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master"
    AutoEventWireup="true" CodeBehind="ListaAplikacji.aspx.cs" Inherits="Pemi.Esoda.Web.UI.ListaAplikacji" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>Lista dostêpnych aplikacji</h2>
    <ul class="applicationList">
    <li>
        <asp:LinkButton CssClass="link" ID="HyperLink1" runat="server" Text="Dziennik Kancelaryjny"
            ToolTip="Przegl¹danie dziennika/rejestracja pism" PostBackUrl="~/Aplikacje/DziennikKancelaryjny/PrzegladDziennikaSimple.aspx" /></li>
            <li>
        <asp:LinkButton CssClass="link" ID="LinkButton2" runat="server" Text="Rejestr Korespondencji Wychodz¹cej"
            ToolTip="Przegl¹danie / rejestracja przesy³ek wychodz¹cych" PostBackUrl="~/Aplikacje/RKW/Przegladanie.aspx" /></li>
 <%--   <li>
        <asp:LinkButton CssClass="link" ID="HyperLink2" runat="server" Text="Statystyki i Raporty"
            ToolTip="Przegl¹danie statystyk i raportów" PostBackUrl="~/Aplikacje/Statystyki/PrzegladanieStatystyk.aspx" /></li>
             --%>
    <li>
        <asp:LinkButton CssClass="link" ID="LinkButton1" runat="server" Text="Panel administracyjny"
            ToolTip="Konfigurowanie i zarz¹dzanie systemem e-S.O.D.A." PostBackUrl="~/Aplikacje/PanelAdministracyjny/MainAdministrativePage.aspx" /></li>
            <li>
        <asp:LinkButton CssClass="link" ID="HyperLink3" runat="server" Text="Raporty do pobrania"
            ToolTip="Pobieranie i wysy³ka raportów" PostBackUrl="~/Aplikacje/Raporty/PrzegladanieRaportow.aspx" /></li>
            <li>
			<asp:LinkButton CssClass="link" ID="HyperLink4" runat="server" Text="Wysy³ka raportów"
           ToolTip="Pobieranie i wysy³ka raportów" PostBackUrl="~/Aplikacje/Raporty/WysylkaRaportow.aspx" /></li>
</asp:Content>
