<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" AutoEventWireup="true" CodeBehind="RejestryDokumentu.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Dokumenty.RejestryDokumentu" Title="Wpisy w rejestrach dotycz¹ce dokumentu" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<esoda:ContextItem id="ContextItem2" runat="server"></esoda:ContextItem>
<h2><%=Page.Title %></h2>
<esoda:DocumentOperations runat="Server" ID="DocumentOperations1" /><hr />
<asp:Repeater runat="server" ID="rejestry" OnItemDataBound="rejestry_ItemDataBound">
<ItemTemplate>
<h3><%#Eval("nazwa") %></h3>
<asp:LinkButton ID="registryLink" runat="server" Text='<%#Eval("nazwa","przejdŸ do: {0}") %>' PostBackUrl='<%#Eval("idRejestru","~/Rejestry/ListaRejestrow.aspx?id={0}") %>' />
<table class="grid gridDokumentView fullWidth">
      <asp:Xml runat="server" ID="registryContent" />
   </table>
</ItemTemplate>
</asp:Repeater>
</asp:Content>

