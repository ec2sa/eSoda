<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Shared.error" Title="B��d aplikacji" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<h2>Wyst�pi� nieoczekiwany b��d w trakcie dzia�ania aplikacji</h2>
<div style="text-align:center;">
<asp:Image runat="server" ImageUrl="~/App_Themes/StandardLayout/img/don_t_panic_button.jpg" AlternateText="uuups..." />
<p><asp:HyperLink runat="server" Text="Wr�� do strony g��wnej" NavigateUrl="~/Logon.aspx" /></p>
<p>Szczeg�y b��du dopst�pne s� dla administratora <asp:HyperLink ID="HyperLink1" runat="server" Text="tutaj" NavigateUrl="~/elmah.axd" /></p>
</div>
</asp:Content>

