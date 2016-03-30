<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="edycjaInteresanta.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.edycjaInteresanta" Title="Interesanci" %>
<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/EdycjaInteresantow.ascx" TagName="Interesant" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
<uc1:Interesant ID="ctrlInteresant" runat="server" />
</asp:Content>