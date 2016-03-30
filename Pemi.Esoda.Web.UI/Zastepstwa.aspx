<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/SingleColumn.Master" CodeBehind="Zastepstwa.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Zastepstwa" %>
<%@ Register src="Controls/AktualneZastepstwa.ascx" tagname="AktualneZastepstwa" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<h2>Twoje zastępstwa</h2>
    <uc1:AktualneZastepstwa ID="AktualneZastepstwa1" runat="server" />

</asp:Content>