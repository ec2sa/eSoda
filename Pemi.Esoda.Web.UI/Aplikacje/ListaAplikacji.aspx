<%@ Page Title="Aplikacje" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master"
    AutoEventWireup="true" CodeBehind="ListaAplikacji.aspx.cs" Inherits="Pemi.Esoda.Web.UI.ListaAplikacji" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>Lista dostępnych aplikacji</h2>
    <ul class="applicationList">
    <li>
        <asp:LinkButton CssClass="link" ID="HyperLink1" runat="server" Text="Dziennik Kancelaryjny"
            ToolTip="Przeglądanie dziennika/rejestracja pism" PostBackUrl="~/Aplikacje/DziennikKancelaryjny/PrzegladDziennikaSimple.aspx" /></li>
            <li>
        <asp:LinkButton CssClass="link" ID="LinkButton2" runat="server" Text="Rejestr Korespondencji Wychodzącej"
            ToolTip="Przeglądanie / rejestracja przesyłek wychodzących" PostBackUrl="~/Aplikacje/RKW/Przegladanie.aspx" /></li>
 <%--   <li>
        <asp:LinkButton CssClass="link" ID="HyperLink2" runat="server" Text="Statystyki i Raporty"
            ToolTip="Przeglądanie statystyk i raportów" PostBackUrl="~/Aplikacje/Statystyki/PrzegladanieStatystyk.aspx" /></li>
             --%>
    <li>
        <asp:LinkButton CssClass="link" ID="LinkButton1" runat="server" Text="Panel administracyjny"
            ToolTip="Konfigurowanie i zarządzanie systemem e-S.O.D.A." PostBackUrl="~/Aplikacje/PanelAdministracyjny/MainAdministrativePage.aspx" /></li>
            <li>
        <asp:LinkButton CssClass="link" ID="HyperLink3" runat="server" Text="Raporty do pobrania"
            ToolTip="Pobieranie i wysyłka raportów" PostBackUrl="~/Aplikacje/Raporty/PrzegladanieRaportow.aspx" /></li>
            <li>
			<asp:LinkButton CssClass="link" ID="HyperLink4" runat="server" Text="Wysyłka raportów"
           ToolTip="Pobieranie i wysyłka raportów" PostBackUrl="~/Aplikacje/Raporty/WysylkaRaportow.aspx" /></li>
</asp:Content>
