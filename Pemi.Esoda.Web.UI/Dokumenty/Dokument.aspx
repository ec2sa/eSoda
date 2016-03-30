<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="Dokument.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Dokument" Title="Dokument" %>
<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<%@ Register TagName="DocPreview" TagPrefix="esoda" Src="~/Controls/ESPDocumentPreview.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<esoda:ContextItem id="ContextItem1" runat="server"></esoda:ContextItem>

<h2>Informacje o dokumencie</h2>
<div class="opisZadania">

</div>
<esoda:DocumentOperations runat="Server" ID="opcjeDokumentu1" />
<hr />
<div runat="server" id="obszarSzczegolow">
<asp:DetailsView runat="server" ID="daneDokumentu" AutoGenerateRows="false" BackColor="#E1E7C3">
<Fields>
<asp:BoundField DataField="Name" HeaderText="Nazwa dokumentu (znak pisma)" />
<asp:BoundField DataField="Description" HeaderText="Krótki opis" />
<asp:BoundField DataField="Owner" HeaderText="W³aœciciel dokumentu" />
<asp:BoundField DataField="CreationDate" HeaderText="Data utworzenia" />
<asp:TemplateField>
<HeaderTemplate>Metadane</HeaderTemplate>
<ItemTemplate>
<asp:GridView GridLines="None" runat="server" ID="meta" DataSource='<%#Eval("Metadata") %>' AutoGenerateColumns="False">
<Columns>
<asp:TemplateField>
<HeaderTemplate>Nazwa</HeaderTemplate>
<ItemTemplate><%# Eval("Key") %>&nbsp;</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Wartoœæ</HeaderTemplate>
<ItemTemplate>&nbsp;<%# Eval("Value") %></ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ItemTemplate>
</asp:TemplateField>
</Fields>
</asp:DetailsView>
</div>
</asp:Content>
