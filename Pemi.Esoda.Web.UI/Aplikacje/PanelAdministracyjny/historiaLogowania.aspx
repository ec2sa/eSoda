<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="historiaLogowania.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.historiaLogowania" Title="Historia logowania" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
<asp:GridView ID="gvLoginHistory" runat="server" AutoGenerateColumns="False" CssClass="grid fullWidth" AllowSorting="True" AllowPaging="True" DataSourceID="odsLoginHistory">
<AlternatingRowStyle CssClass="pozycjaNieparzysta" />
            <EmptyDataTemplate>
                <asp:Label ID="lblNoLogs" runat="server" ForeColor="Red" Text="Brak wpisów w historii logowania !!!" />
            </EmptyDataTemplate>
    <Columns>
        <asp:BoundField DataField="data" HeaderText="Data i czas" SortExpression="data" />
        <asp:BoundField DataField="userLogin" HeaderText="U¿ytkownik" SortExpression="userLogin" />
        <asp:BoundField DataField="remoteIP" HeaderText="IP komputera" SortExpression="remoteIP" />
        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="status" />
        <asp:BoundField DataField="zone" HeaderText="Strefa" SortExpression="zone" />
    </Columns>
    <PagerTemplate>
            <table border="0" cellspacing="0" cellpadding="0" width="100%">
            <tr align="center">
            <td><asp:LinkButton ID="lnkFirst" runat="server" CommandName="Page" CommandArgument="First">&lt;&lt;</asp:LinkButton></td>
            <td><asp:LinkButton ID="lnkPrev" runat="server" CommandName="Page" CommandArgument="Prev">&lt;</asp:LinkButton></td>
            <td><%=gvLoginHistory.PageIndex+1 %> z <%=gvLoginHistory.PageCount %></td>
            <td><asp:LinkButton ID="lnkNext" runat="server" CommandName="Page" CommandArgument="Next">&gt;</asp:LinkButton></td>
            <td><asp:LinkButton ID="lnkLast" runat="server" CommandName="Page" CommandArgument="Last">&gt;&gt;</asp:LinkButton></td>
            </tr></table>
            </PagerTemplate>
</asp:GridView>
<asp:ObjectDataSource ID="odsLoginHistory" runat="server" OldValuesParameterFormatString="" SelectMethod="GetLoginHistory" SortParameterName="sortParam" TypeName="Pemi.Esoda.DataAccess.LoginHistoryDAO" >
    <SelectParameters>
        <asp:Parameter Name="sortParam" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
</asp:Content>

