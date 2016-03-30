<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentContextItem.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.DocumentContextItem" %>
<div id="dokumentContent">
<h1>Dokument - widok</h1>

</div>
<asp:GridView GridLines="None" runat="Server" ID="opisZadania" AutoGenerateColumns="false" CssClass="grid gridDokument fullWidth">
<EmptyDataTemplate>Brak skojarzonego zadania</EmptyDataTemplate>
<Columns>

<asp:TemplateField>
<HeaderTemplate>Nr sys.</HeaderTemplate>
<ItemTemplate><%# XPath("id") %></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Interesant</HeaderTemplate>
<ItemTemplate><%# XPath("interesant") %></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Typ<br /> interesanta</HeaderTemplate>
<ItemTemplate><%# XPath("typInteresanta")%></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Typ <br />zadania</HeaderTemplate>
<ItemTemplate><%# XPath("typ") %></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Status</HeaderTemplate>
<ItemTemplate><%# XPath("status") %></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Znak pisma/<br />JRWA</HeaderTemplate>
<ItemTemplate><%# XPath("znakPisma") %></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Rodzaj</HeaderTemplate>
<ItemTemplate><%# XPath("rodzaj") %></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Data<br />pocz.</HeaderTemplate>
<ItemTemplate><%# XPath("data") %></ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>


<asp:Literal ID="ltrSearchCriteria" runat="server" />
