<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master"AutoEventWireup="true" CodeBehind="printJRWA.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.printJRWA" Title="Drukowanie drzewa JRWA" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <a href="#" onclick="javascript:window.print();return false;">Drukuj</a>
    <asp:LinkButton runat="server" PostBackUrl="~/Aplikacje/PanelAdministracyjny/edycjaJRWA.aspx" Text="powrót" />
    <br />
    <asp:Table ID="tabJRWA" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
        GridLines="Both" CssClass="visibleOnPrint">
        <asp:TableRow ID="hrow1" runat="server">
            <asp:TableHeaderCell runat="server" ColumnSpan="5" Wrap="False">SYMBOLE KLASYFIKACYJNE</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server" RowSpan="2">HAS£A KLASYFIKACYJNE</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server" ColumnSpan="2">KATEGORIA ARCHIWALNA</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server" RowSpan="2">UWAGI</asp:TableHeaderCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
            <asp:TableHeaderCell runat="server">I</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server">II</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server">III</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server">IV</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server">V</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server">W komórce macierzystej</asp:TableHeaderCell>
            <asp:TableHeaderCell runat="server">W innych komórkach</asp:TableHeaderCell>
        </asp:TableRow>
    </asp:Table>
    <asp:TreeView ID="tvPrintJRWA" runat="server" DataSourceID="xmldsPrintJRWA" Visible="false" ExpandDepth="FullyExpand" OnDataBound="tvPrintJRWA_DataBound">
        <DataBindings>
            <asp:TreeNodeBinding DataMember="JRWAItem" NavigateUrlField="NavigateUrl" TargetField="Target"
                TextField="Text" ToolTipField="ToolTip" ValueField="Value" />
        </DataBindings>
    </asp:TreeView>
    <asp:XmlDataSource ID="xmldsPrintJRWA" runat="server" TransformFile="~/Aplikacje/PanelAdministracyjny/xsltJrwaPrint.xslt"
        XPath="JRWA/JRWAItem" EnableCaching="False"></asp:XmlDataSource>
    <br />
</asp:Content>