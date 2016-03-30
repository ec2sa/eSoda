<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ObjectDetailsView.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.ObjectDetailsView" %>
<asp:MultiView runat="server" ID="detailsView">
<asp:View runat="server" ID="CaseView">
<asp:DetailsView runat="server" ID="daneSprawy" AutoGenerateRows="false" BackColor="#DDE7E8">
<Fields>
<asp:BoundField DataField="Description" HeaderText="Krótki opis" />
<asp:BoundField DataField="Owner" HeaderText="Właściciel sprawy" />
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
<HeaderTemplate>Wartość</HeaderTemplate>
<ItemTemplate>&nbsp;<%# Eval("Value") %></ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ItemTemplate>
</asp:TemplateField>
</Fields>
</asp:DetailsView>
</asp:View>
<asp:View runat="server" ID="DocView">
<asp:DetailsView runat="server" ID="daneDokumentu" AutoGenerateRows="false" BackColor="#E1E7C3">
<Fields>
<asp:BoundField DataField="Name" HeaderText="Nazwa dokumentu (znak pisma)" />
<asp:BoundField DataField="Description" HeaderText="Krótki opis" />
<asp:BoundField DataField="Owner" HeaderText="Właściciel dokumentu" />
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
<HeaderTemplate>Wartość</HeaderTemplate>
<ItemTemplate>&nbsp;<%# Eval("Value") %></ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</ItemTemplate>
</asp:TemplateField>
</Fields>
</asp:DetailsView>
</asp:View>
</asp:MultiView>