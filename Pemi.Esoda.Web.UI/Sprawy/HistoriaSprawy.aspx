<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="HistoriaSprawy.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Sprawy.HistoriaSprawy" Title="Historia sprawy" %>
<%@ Register TagPrefix="esoda" TagName="CaseContext" Src="~/Controls/CaseContextItem.ascx" %>
<%@ Register TagPrefix="esoda" TagName="OperacjeSprawy" Src="~/Controls/OperacjeSprawy.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<esoda:CaseContext runat="Server" ID="CaseContext1" />
<h2>Przegl¹danie historii sprawy</h2>
<esoda:OperacjeSprawy runat="server" ID="OperacjeSprawy1" /><hr />
<asp:GridView GridLines="None" runat="server" ID="lista" AutoGenerateColumns="false" CssClass="grid gridSprawaView fullWidth"  OnRowCommand="execCommand">
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
<ItemTemplate><asp:LinkButton ID="LinkButton1" runat="server" Text="poka¿" CommandName="details" CommandArgument='<%# XPath("id") %>' /></ItemTemplate>
</asp:TemplateField>

</columns>
</asp:GridView>
</asp:Content>

