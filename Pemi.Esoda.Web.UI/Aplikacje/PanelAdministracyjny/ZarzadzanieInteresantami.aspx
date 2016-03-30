<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="ZarzadzanieInteresantami.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.ZarzadzanieInteresantami" Title="Zarz¹dzanie interesantami" %>
<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/OperacjeInteresantow.ascx" TagName="OperacjeInteresantow" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
<uc1:OperacjeInteresantow ID="OperacjeInteresantow1" runat="server" />
</asp:Content>