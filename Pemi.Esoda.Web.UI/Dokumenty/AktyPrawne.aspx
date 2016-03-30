<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" AutoEventWireup="true" CodeBehind="AktyPrawne.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Dokumenty.AktyPrawne" Title="Widok aktów prawnych" %>
<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<%@ Register TagName="DocPreview" TagPrefix="esoda" Src="~/Controls/ESPDocumentPreview.ascx" %>
<%@ Register src="../Controls/OperacjeAktow.ascx" tagname="OperacjeAktow" tagprefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">

<esoda:ContextItem id="ContextItem1" runat="server"></esoda:ContextItem>
<h2>Widok aktów prawnych</h2>
<esoda:DocumentOperations runat="Server" ID="opcjeDokumentu1" />
<hr />
<uc1:OperacjeAktow runat="server" ID="OperacjeAktow" />

<asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
</asp:Content>
