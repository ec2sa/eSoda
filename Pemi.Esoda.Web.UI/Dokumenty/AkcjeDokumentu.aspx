<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="AkcjeDokumentu.aspx.cs" Inherits="Pemi.Esoda.Web.UI.AkcjeDokumentu" Title="Akcje dla dokumentu" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<esoda:ContextItem id="ContextItem2" runat="server"></esoda:ContextItem>
<h2>Akcje dla dokumentu</h2>
<esoda:DocumentOperations runat="Server" ID="DocumentOperations1" /><hr />
<asp:GridView GridLines="None" runat="server" ID="lista" AutoGenerateColumns="false" CssClass="grid gridDokumentView fullWidth">
<Columns>

<asp:TemplateField>
<HeaderTemplate>Numer</HeaderTemplate>
<ItemTemplate><%# XPath("number") %></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Akcja</HeaderTemplate>
<ItemTemplate><asp:LinkButton runat="server" ID="op3" Text='<%# XPath("nazwa") %>' OnCommand="wykonajAkcje"
CommandName='<%# XPath("szablon") %>' CommandArgument='<%# XPath("@id") %>' />
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Opis</HeaderTemplate>
<ItemTemplate><asp:LinkButton runat="server" ID="op2" Text='<%# XPath("opis") %>' OnCommand="wykonajAkcje"
CommandName='<%# XPath("szablon") %>' CommandArgument='<%# XPath("@id") %>' />
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Czynnoœæ</HeaderTemplate>
<ItemTemplate>
<asp:LinkButton runat="server" ID="op1" Text="wykonaj" ToolTip="wykonanie akcji na bie¿¹cym dokumencie" OnCommand="wykonajAkcje"
CommandName='<%# XPath("szablon") %>' CommandArgument='<%# XPath("@id") %>' />
</ItemTemplate>
</asp:TemplateField>

</Columns>
</asp:GridView>


</asp:Content>

