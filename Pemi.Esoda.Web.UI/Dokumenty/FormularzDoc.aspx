<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" AutoEventWireup="true"
    CodeBehind="FormularzDoc.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Dokumenty.FormularzDoc"
    Title="Untitled Page" %>

<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<%@ Register TagName="DocPreview" TagPrefix="esoda" Src="~/Controls/ESPDocumentPreview.ascx" %>
<%@ Register Src="../Controls/OperacjeFormularza.ascx" TagName="OperacjeFormularza"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <esoda:ContextItem ID="ContextItem1" runat="server"></esoda:ContextItem>
    <h2><%=this.Title %></h2>
    <%--<esoda:DocumentOperations runat="Server" ID="opcjeDokumentu1" />--%>
    <hr />
    <%--<uc1:OperacjeFormularza ID="OperacjeFormularza" runat="server" />--%>
    <asp:Label runat="server" ID="lblMessage" ForeColor="Red" /><br />
    <asp:LinkButton ID="lbtnBack" runat="server" Text="Powrót" OnClick="lbtnBack_Click" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
