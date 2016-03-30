<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Shared.error" Title="B³¹d aplikacji" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<h2>Wyst¹pi³ nieoczekiwany b³¹d w trakcie dzia³ania aplikacji</h2>
<div style="text-align:center;">
<asp:Image runat="server" ImageUrl="~/App_Themes/StandardLayout/img/don_t_panic_button.jpg" AlternateText="uuups..." />
<p><asp:HyperLink runat="server" Text="Wróæ do strony g³ównej" NavigateUrl="~/Logon.aspx" /></p>
<p>Szczegó³y b³êdu dopstêpne s¹ dla administratora <asp:HyperLink ID="HyperLink1" runat="server" Text="tutaj" NavigateUrl="~/elmah.axd" /></p>
</div>
</asp:Content>

