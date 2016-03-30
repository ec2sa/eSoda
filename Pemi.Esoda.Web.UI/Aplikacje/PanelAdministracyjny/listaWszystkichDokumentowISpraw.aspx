<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
    CodeBehind="listaWszystkichDokumentowISpraw.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.listaWszystkichDokumentowISpraw"
    Title="Dokumenty i sprawy" %>

<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="esoda" TagName="CaseContext" Src="~/Controls/CaseContextItem.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
    <div style="float: left; width: 10%;">
        <fieldset style="display: block;">
            <legend>Filtruj</legend>
            <asp:CheckBox ID="ckbDokumenty" Checked="true" runat="server" Text="Dokumenty" AutoPostBack="true"
                OnCheckedChanged="filtruj" /><br />
            <asp:CheckBox ID="ckbSprawy" Checked="true" runat="server" Text="Sprawy" AutoPostBack="true"
                OnCheckedChanged="filtruj" /><br />
        </fieldset>
    </div>
    <div style="float: right; width: 89%;">
        <h2>
            Lista spraw i dokumentów</h2>
        <asp:GridView GridLines="None" runat="server" ID="gvCaseDocList" AutoGenerateColumns="False"
            CssClass="grid fullWidth" AllowSorting="True" DataSourceID="odsDocCaseList" AllowPaging="True"
            PagerSettings-Position="TopAndBottom" OnDataBound="gvCaseDocList_DataBound">
            <EmptyDataTemplate>
                <p class="emptyRow">
                    Brak dokumentów/spraw</p>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkId" runat="server" CommandName="Sort" CommandArgument="id">Nr sys.</asp:LinkButton></HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("id") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkNrSys" runat="server" CommandName="Sort" CommandArgument="nr">Nr Dz. Kanc.</asp:LinkButton></HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("nr") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkInteresant" runat="server" CommandName="Sort" CommandArgument="interesant">Interesant</asp:LinkButton></HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("interesant") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkTypInteresanta" runat="server" CommandName="Sort" CommandArgument="typInteresanta">Typ<br />interesanta</asp:LinkButton></HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("typInteresanta")%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkTyp" runat="server" CommandName="Sort" CommandArgument="typ">Typ<br />zadania</asp:LinkButton></HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("typ") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkStatus" runat="server" CommandName="Sort" CommandArgument="status">Status</asp:LinkButton></HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("status") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkZnakPisma" runat="server" CommandName="Sort" CommandArgument="znakPisma">Znak pisma/<br />JRWA</asp:LinkButton></HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("znakPisma") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkRodzaj" runat="server" CommandName="Sort" CommandArgument="rodzaj">Rodzaj</asp:LinkButton></HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("rodzaj") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkData" runat="server" CommandName="Sort" CommandArgument="data">Data<br />pocz.</asp:LinkButton></HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("data") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkPracownik" runat="server" CommandName="Sort" CommandArgument="Pracownik">Pracownik</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%#Eval("Pracownik") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkWydzial" runat="server" CommandName="Sort" CommandArgument="Wydzial">Wydzia³</asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("Wydzial") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Szczegó³y</HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton2" runat="server" Text="zobacz" CommandName='<%# Eval("typ") %>'
                            CommandArgument='<%# Eval("id") %>' ToolTip="Przejœcie do szczegó³ów sprawy/dokumentu"
                            OnCommand="podglad" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerTemplate>
                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tr align="center">
                        <td>
                            <asp:LinkButton ID="lnkFirst" runat="server" CommandName="Page" CommandArgument="First">&lt;&lt;</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkPrev" runat="server" CommandName="Page" CommandArgument="Prev">&lt;</asp:LinkButton>
                        </td>
                        <td>
                            strona:
                            <asp:DropDownList ID="ddlPageSelector" runat="server" AutoPostBack="True" OnDataBinding="ddlPageSelector_DataBinding"
                                OnDataBound="ddlPageSelector_DataBound" OnSelectedIndexChanged="ddlPageSelector_SelectedIndexChanged" />
                            z
                            <%=gvCaseDocList.PageCount %>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkNext" runat="server" CommandName="Page" CommandArgument="Next">&gt;</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkLast" runat="server" CommandName="Page" CommandArgument="Last">&gt;&gt;</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </PagerTemplate>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsDocCaseList" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetDocCaseFullList" TypeName="Pemi.Esoda.DataAccess.ActionDAO">
            <SelectParameters>
                <asp:Parameter Name="sortParam" Type="String" />
                <asp:Parameter Name="filterParam" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
