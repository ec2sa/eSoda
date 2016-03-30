<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" AutoEventWireup="true" CodeBehind="FormularzXml.aspx.cs" Inherits="Pemi.Esoda.Web.UI.FormularzXml" Title="Widok XML formularza" %>
<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<%@ Register TagName="DocPreview" TagPrefix="esoda" Src="~/Controls/ESPDocumentPreview.ascx" %>

<%@ Register src="../Controls/OperacjeFormularza.ascx" tagname="OperacjeFormularza" tagprefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">

<esoda:ContextItem id="ContextItem1" runat="server"></esoda:ContextItem>
<h2>Widok XML formularza</h2>
<%--<esoda:DocumentOperations runat="Server" ID="opcjeDokumentu1" />
<hr />
<uc1:OperacjeFormularza ID="OperacjeFormularza" runat="server" />  --%>  
<asp:Label runat="server" ID="lblMessage" ForeColor="Red" /><br />
<asp:LinkButton ID="lbtnBack" runat="server" Text="Powrót" OnClick="lbtnBack_Click" />
</asp:Content>
