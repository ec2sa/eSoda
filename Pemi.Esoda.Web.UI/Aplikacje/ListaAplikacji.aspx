<%@ Page Title="Aplikacje" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master"
    AutoEventWireup="true" CodeBehind="ListaAplikacji.aspx.cs" Inherits="Pemi.Esoda.Web.UI.ListaAplikacji" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>Lista dost�pnych aplikacji</h2>
    <ul class="applicationList">
    <li>
        <asp:LinkButton CssClass="link" ID="HyperLink1" runat="server" Text="Dziennik Kancelaryjny"
            ToolTip="Przegl�danie dziennika/rejestracja pism" PostBackUrl="~/Aplikacje/DziennikKancelaryjny/PrzegladDziennikaSimple.aspx" /></li>
            <li>
        <asp:LinkButton CssClass="link" ID="LinkButton2" runat="server" Text="Rejestr Korespondencji Wychodz�cej"
            ToolTip="Przegl�danie / rejestracja przesy�ek wychodz�cych" PostBackUrl="~/Aplikacje/RKW/Przegladanie.aspx" /></li>
 <%--   <li>
        <asp:LinkButton CssClass="link" ID="HyperLink2" runat="server" Text="Statystyki i Raporty"
            ToolTip="Przegl�danie statystyk i raport�w" PostBackUrl="~/Aplikacje/Statystyki/PrzegladanieStatystyk.aspx" /></li>
             --%>
    <li>
        <asp:LinkButton CssClass="link" ID="LinkButton1" runat="server" Text="Panel administracyjny"
            ToolTip="Konfigurowanie i zarz�dzanie systemem e-S.O.D.A." PostBackUrl="~/Aplikacje/PanelAdministracyjny/MainAdministrativePage.aspx" /></li>
            <li>
        <asp:LinkButton CssClass="link" ID="HyperLink3" runat="server" Text="Raporty do pobrania"
            ToolTip="Pobieranie i wysy�ka raport�w" PostBackUrl="~/Aplikacje/Raporty/PrzegladanieRaportow.aspx" /></li>
            <li>
			<asp:LinkButton CssClass="link" ID="HyperLink4" runat="server" Text="Wysy�ka raport�w"
           ToolTip="Pobieranie i wysy�ka raport�w" PostBackUrl="~/Aplikacje/Raporty/WysylkaRaportow.aspx" /></li>
</asp:Content>
