<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="WyszukiwarkaLista.aspx.cs" Inherits="Pemi.Esoda.Web.UI.WyszukiwarkaLista" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>Wyszukiwarka</h2>
    <asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
    
    <ul class="searchList">
        <li><asp:LinkButton CssClass="link" runat="server" Text="Wyszukiwarka Spraw" ToolTip="Wyszukiwarka spraw" PostBackUrl="~/Wyszukiwarka/WyszukiwarkaSpraw.aspx" /></li>
        <li><asp:LinkButton ID="LinkButton1" CssClass="link" runat="server" Text="Wyszukiwarka Dokumentów" ToolTip="Wyszukiwarka dokumentów" PostBackUrl="~/Wyszukiwarka/WyszukiwarkaDokumentow.aspx" /></li>
        <li><asp:LinkButton CssClass="link" runat="server" ID="myDecretaction" Text="Moje Dekretacje" ToolTip="Moje dekretacje" PostBackUrl="~/Wyszukiwarka/MojeDekretacje.aspx" /></li>
    </ul>
</asp:Content>
