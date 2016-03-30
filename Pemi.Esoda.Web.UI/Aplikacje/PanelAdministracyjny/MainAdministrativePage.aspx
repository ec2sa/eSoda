<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="MainAdministrativePage.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.MainAdministrativePage" Title="Strona g³ówna" %>
<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
</asp:Content>