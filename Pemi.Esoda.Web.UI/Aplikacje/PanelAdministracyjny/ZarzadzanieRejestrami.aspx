<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="ZarzadzanieRejestrami.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.ZarzadzanieRejestrami" Title="Zarządzanie rejestrami" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePanelu" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/OperacjeRejestrow.ascx" TagName="OperacjeRejestrow" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePanelu ID="OperacjePanelu1" runat="server" />
<uc1:OperacjeRejestrow ID="OperacjeRejestrow1" runat="server" />
</asp:Content>

