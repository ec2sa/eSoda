<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="HistoriaDokumentu.aspx.cs" Inherits="Pemi.Esoda.Web.UI.HistoriaDokumentu" Title="Historia dokumentu" %>
<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<esoda:ContextItem id="ContextItem2" runat="server"></esoda:ContextItem>
<h2>Przegl¹danie historii dokumentu</h2>
<esoda:DocumentOperations runat="Server" ID="DocumentOperations1" /><hr />
<asp:GridView GridLines="None" runat="server" ID="lista" AutoGenerateColumns="false" CssClass="grid gridDokumentView fullWidth" OnRowCommand="execCommand">
<columns>

<asp:TemplateField>
<HeaderTemplate>Data</HeaderTemplate>
<ItemTemplate><%# XPath("substring(data,1,10)") %></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Czas</HeaderTemplate>
<ItemTemplate><%# XPath("substring(data,12,8)")%></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Pracownik</HeaderTemplate>
<ItemTemplate><%# XPath("pracownik") %></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Status po</HeaderTemplate>
<ItemTemplate><%# XPath("status") %></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Opis</HeaderTemplate>
<ItemTemplate><%# XPath("opis") %></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Szczegó³y</HeaderTemplate>
<ItemTemplate><asp:LinkButton runat="server" Text="poka¿" CommandName="details" CommandArgument='<%# XPath("id") %>' /></ItemTemplate>
</asp:TemplateField>
</columns>
</asp:GridView>
</asp:Content>

