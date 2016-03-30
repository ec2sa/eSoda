<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="RejestrySprawy.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Sprawy.RejestrySprawy" Title="Wpisy w rejestrach dotyczące sprawy" %>
<%@ Register TagName="CaseOperations" TagPrefix="esoda" Src="~/Controls/OperacjeSprawy.ascx" %>
<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/CaseContextItem.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<esoda:ContextItem id="ContextItem2" runat="server"></esoda:ContextItem>
<h2><%=Page.Title %></h2>
<esoda:CaseOperations runat="Server" ID="CaseOperations1" /><hr />
<asp:Repeater runat="server" ID="rejestry" OnItemDataBound="rejestry_ItemDataBound">
<ItemTemplate>
<h3><%#Eval("nazwa") %></h3><asp:LinkButton ID="LinkButton1" runat="server" Text='<%#Eval("nazwa","przejdź do: {0}") %>' PostBackUrl='<%#Eval("idRejestru","~/Rejestry/ListaRejestrow.aspx?id={0}") %>' />
<table class="grid gridSprawaView fullWidth">
      <asp:Xml runat="server" ID="registryContent" />
   </table>
</ItemTemplate>
</asp:Repeater>
</asp:Content>