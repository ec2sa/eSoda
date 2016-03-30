<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomFormWrapperControl.ascx.cs" Inherits="Pemi.Esoda.Web.UI.CustomFormWrapperControl" %>
<h3><asp:Literal runat="server" ID="lblDocumentType" /></h3>

<asp:label runat="server" ID="lblError" style="color:Red;" />
<asp:LinkButton runat="server" ID="lnkEdit" Text="Edycja" 
    onclick="lnkEdit_Click" />
   <br /><br />
<asp:Panel runat="server" ID="formPlaceholder" />

</asp:Content>