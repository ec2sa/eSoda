<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="Sprawa.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Sprawy.Sprawa" Title="Informacje o sprawie" %>
<%@ Register TagPrefix="esoda" TagName="CaseContext" Src="~/Controls/CaseContextItem.ascx" %>
<%@ Register TagPrefix="esoda" TagName="OperacjeSprawy" Src="~/Controls/OperacjeSprawy.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<esoda:CaseContext runat="Server" ID="cc1" />
<h2>Informacje o sprawie</h2>
<esoda:OperacjeSprawy runat="server" ID="ops1" />
<hr />
<div runat="server" id="obszarSzczegolow">
<asp:DetailsView runat="server" ID="daneSprawy" AutoGenerateRows="false" BackColor="#DDE7E8">
<Fields>
<asp:BoundField DataField="Description" HeaderText="Krótki opis" />
<asp:BoundField DataField="Owner" HeaderText="W³aœciciel sprawy" />
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