<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
    CodeBehind="DokumentySprawy.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Sprawy.DokumentySprawy"
    Title="Lista dokumentów w sprawie" %>

<%@ Register TagPrefix="esoda" TagName="CaseContext" Src="~/Controls/CaseContextItem.ascx" %>
<%@ Register TagPrefix="esoda" TagName="OperacjeSprawy" Src="~/Controls/OperacjeSprawy.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <esoda:CaseContext runat="Server" ID="CaseContext1" />
    <h2>Dokumenty skojarzone ze spraw¹</h2>
    <esoda:OperacjeSprawy runat="server" ID="OperacjeSprawy1" />
    <hr />
    <asp:GridView GridLines="None" runat="server" AutoGenerateColumns="false" ID="listaDokumentow"
        CssClass="grid gridSprawaView fullWidth" OnRowCommand="wykonaniePolecenia">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    ID</HeaderTemplate>
                <ItemTemplate>
                    <%# XPath("id") %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Rodzaj</HeaderTemplate>
                <ItemTemplate>
                    <%# XPath("rodzaj") %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Opis</HeaderTemplate>
                <ItemTemplate>
                    <%# XPath("opis") %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Data pocz.</HeaderTemplate>
                <ItemTemplate>
                    <%# XPath("data") %></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    Szczegó³y</HeaderTemplate>
                <ItemTemplate>
                    <asp:LinkButton runat="server" Text="zobacz" CommandName="dokument" CommandArgument='<%# XPath("id")%>' />
                    <br />
                    <asp:linkbutton runat="server" Visible='<%# XPath("./@ostatni")!=null?true:false %>' Text="usuñ ze sprawy" CommandName="usunZeSprawy" CommandArgument='<%# XPath("id")%>' />
                    </ItemTemplate>
             
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
