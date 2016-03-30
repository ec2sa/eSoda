<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" AutoEventWireup="true" CodeBehind="Formularz.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Formularz" Title="Formularz" %>
<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<%@ Register TagName="DocPreview" TagPrefix="esoda" Src="~/Controls/ESPDocumentPreview.ascx" %>

<%@ Register src="../Controls/CustomFormWrapperControl.ascx" tagname="CustomFormWrapperControl" tagprefix="uc1" %>
<%@ Register src="../Controls/OperacjeFormularza.ascx" tagname="OperacjeFormularza" tagprefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">

<esoda:ContextItem id="ContextItem1" runat="server"></esoda:ContextItem>
<h2>Formularz</h2>
<esoda:DocumentOperations runat="Server" ID="opcjeDokumentu1" />
<hr />
<uc1:OperacjeFormularza ID="OperacjeFormularza" runat="server" />
<asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
<asp:Label runat="server" ID="lblWarning" ForeColor="#FFA200" Text="Dane pochodz¹ z innej wersji formularza!" Visible="false" />
    <uc1:CustomFormWrapperControl ID="CustomFormWrapperControl1" runat="server" />
</asp:Content>
