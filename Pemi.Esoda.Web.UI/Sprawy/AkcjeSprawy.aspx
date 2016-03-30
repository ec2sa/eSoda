<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="AkcjeSprawy.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Sprawy.AkcjeSprawy" Title="Akcje dla sprawy" %>
<%@ Register TagPrefix="esoda" TagName="CaseContext" Src="~/Controls/CaseContextItem.ascx" %>
<%@ Register TagPrefix="esoda" TagName="OperacjeSprawy" Src="~/Controls/OperacjeSprawy.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<esoda:CaseContext runat="Server" ID="CaseContext1" />
<h2>Akcje dla sprawy</h2>
<esoda:OperacjeSprawy runat="server" ID="OperacjeSprawy1" /><hr />
<asp:GridView GridLines="None" runat="server" ID="lista" AutoGenerateColumns="false" CssClass="grid gridSprawaView fullWidth">
<Columns>

<asp:TemplateField>
<HeaderTemplate>Numer</HeaderTemplate>
<ItemTemplate><%# XPath("number") %></ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Akcja</HeaderTemplate>
<ItemTemplate>
<asp:LinkButton runat="server" ID="op3" Text='<%# XPath("nazwa") %>' ToolTip="wykonanie akcji na bie¿¹cej sprawie" OnCommand="wykonajAkcje"
CommandName='<%# XPath("szablon") %>' CommandArgument='<%# XPath("@id") %>' />
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Opis</HeaderTemplate>
<ItemTemplate>
<asp:LinkButton runat="server" ID="op2" Text='<%# XPath("opis") %>' ToolTip="wykonanie akcji na bie¿¹cej sprawie" OnCommand="wykonajAkcje"
CommandName='<%# XPath("szablon") %>' CommandArgument='<%# XPath("@id") %>' />
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>Czynnoœæ</HeaderTemplate>
<ItemTemplate>
<asp:LinkButton runat="server" ID="op1" Text="wykonaj" ToolTip="wykonanie akcji na bie¿¹cej sprawie" OnCommand="wykonajAkcje"
CommandName='<%# XPath("szablon") %>' CommandArgument='<%# XPath("@id") %>' />
</ItemTemplate>
</asp:TemplateField>

</Columns>
</asp:GridView>

</asp:Content>
