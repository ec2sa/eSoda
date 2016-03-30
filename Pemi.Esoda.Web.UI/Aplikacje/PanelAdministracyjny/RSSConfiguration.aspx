<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="RSSConfiguration.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.RSSConfiguration" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="opa1" runat="server" />
<fieldset>
<legend> Konfiguracja RSS</legend>
<asp:Label runat="server" AssociatedControlID="rssUrl">Adres eSody do publikacji w RSS</asp:Label>
<asp:TextBox runat="server" ID="rssUrl" style="width:40em;" />
<asp:Button runat="server" Text="Zapisz" />
</fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
