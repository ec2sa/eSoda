<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
    CodeBehind="OczekujaceZadania.aspx.cs" Inherits="Pemi.Esoda.Web.UI.OczekujaceZadania"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <div style="float: left; width: 15%;">
        <fieldset>
            <legend>Filtrowanie zadañ</legend>
            <asp:RadioButton GroupName="adresat" ID="ckbMoje" runat="server" Text="Moje" AutoPostBack="true"
                Checked="true" OnCheckedChanged="filtruj" /><br />
            <asp:RadioButton GroupName="adresat" ID="ckbMojWydzial" runat="server" Text="Mojego wydzia³u"
                Checked="false" AutoPostBack="true" OnCheckedChanged="filtruj" /><br />
            <asp:RadioButton GroupName="adresat" ID="ckbWszystkie" runat="server" Text="Wszystkie"
                Checked="false" AutoPostBack="true" OnCheckedChanged="filtruj" />
            <hr />
            <asp:RadioButton GroupName="adresat" ID="cbDoWiadomosci" runat="server" Text="Do wiadomoœci"
                Checked="false" AutoPostBack="true" OnCheckedChanged="filtruj" />
            <hr />
            <asp:CheckBox ID="ckbDokumenty" Checked="true" runat="server" Text="Dokumenty" AutoPostBack="true"
                OnCheckedChanged="filtruj" /><br />
            <asp:CheckBox ID="ckbSprawy" Checked="false" runat="server" Text="Sprawy" AutoPostBack="true"
                OnCheckedChanged="filtruj" /><br />
            <asp:CheckBox ID="ckbFaktury" Checked="true" runat="server" Text="Faktury" AutoPostBack="true"
                OnCheckedChanged="filtruj" /><br />
            <hr />
            <asp:Label ID="lblFilterTypInteresanta" runat="server" Text="Typ interesanta:" AssociatedControlID="ddlTypInteresanta" /><br />
            <asp:DropDownList ID="ddlTypInteresanta" runat="server" AutoPostBack="true" OnSelectedIndexChanged="filtruj">
                <asp:ListItem Text="Dowolny" Value="-1" Selected="true" />
                <asp:ListItem Text="Osoba fizyczna" Value="1" />
                <asp:ListItem Text="Firma" Value="2" />
                <asp:ListItem Text="Instytucja" Value="3" />
            </asp:DropDownList>
            <hr />
            <asp:LinkButton ID="lnkNewDoc" runat="server" Text="Nowy dokument..." PostBackUrl="~/Akcje/NowyDokumentLogiczny.aspx" />
        </fieldset>
         <a runat="server" id="rssLink" class="rssLink" title="Oczekuj¹ce dokumenty jako RSS" href="#" target="_blank">RSS</a>
    </div>
    <div style="float: right; width: 83%;">
        <h1>
            Lista spraw i dokumentów przypisanych do u¿ytkownika lub wydzia³u</h1>
           
        <asp:GridView ID="gvAwaitingTasks" runat="server" CssClass="grid fullWidth" AllowSorting="True"
            DataSourceID="odsAwaitingTasks" AutoGenerateColumns="False" AllowPaging="True">
            <EmptyDataTemplate>
                <p class="emptyRow">
                    Aktualnie nie ma spe³niaj¹cych kryteria wyszukiwania zadañ przypisanych u¿ytkownikowi/wydzia³owi</p>
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
                        <%# (Eval("nr").ToString().Equals("0")) ? "" : Eval("nr") %></ItemTemplate>
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
                    
                        <asp:LinkButton ID="lnkDW"  runat="server" CommandName="Sort" CommandArgument="doWiadomosci" title="do wiadomoœci">DW</asp:LinkButton>
                        </HeaderTemplate>
                       
                    <ItemTemplate>
                    <span class='<%# Eval("doWiadomosci").ToString().Equals("1") || Eval("doWiadomosci").ToString().Equals("0") ? "checkedIcon" : ""%>'>
                    <span>
                        <%# Eval("doWiadomosci").ToString().Equals("1") || Eval("doWiadomosci").ToString().Equals("0") ? "Tak" : ""%></span></span></ItemTemplate>
                         
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
                <%--<asp:TemplateField>
<HeaderTemplate>Wydzial<br />Pracownik</HeaderTemplate>
<ItemTemplate>
<%# Eval("Wydzial") %><br /><%# Eval("Pracownik") %>
</ItemTemplate>
</asp:TemplateField>--%>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Szczegó³y</HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" Text="zobacz" CommandName='<%# Eval("typ") %>'
                            CommandArgument='<%# Eval("id") %>' ToolTip="Przejœcie do szczegó³ów sprawy/dokumentu"
                            OnCommand="wykonaj" />
                            

                        <asp:ImageButton ID="LinkButton21" Visible='<%# Eval("typ").ToString().Equals("Dokument")?true:false %>'
                            runat="server" CommandName='<%# Eval("typ","{0}_s") %>' CommandArgument='<%# Eval("id") %>' ToolTip="Przejœcie do skanów dokumentu"
                            OnCommand="wykonaj" ImageUrl="~/App_Themes/StandardLayout/img/skany.PNG" />

                          
                            <%--<asp:LinkButton  Visible='<%# Eval("typ").ToString().Equals("Dokument")?true:false %>'
                            ID="LinkButton21" runat="server" Text="zobacz skany" CommandName='<%# Eval("typ","{0}_s") %>'
                            CommandArgument='<%# Eval("id") %>' ToolTip="Przejœcie do skanów dokumentu"
                            OnCommand="wykonaj" />--%>
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
                            <%=gvAwaitingTasks.PageCount %>
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
        <asp:ObjectDataSource ID="odsAwaitingTasks" runat="server" OldValuesParameterFormatString="{0}"
            SelectMethod="GetAwaitingTasks" TypeName="Pemi.Esoda.DataAccess.ActionDAO">
            <SelectParameters>
                <asp:Parameter Name="userId" Type="Object" />
                <asp:Parameter Name="sortParam" Type="String" />
                <asp:Parameter Name="filterParam" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:GridView Visible="false" GridLines="None" runat="server" ID="lista" AutoGenerateColumns="false"
            CssClass="grid fullWidth" OnRowCommand="lista_RowCommand">
            <EmptyDataTemplate>
                <p class="emptyRow">
                    Aktualnie nie ma spe³niaj¹cych kryteria wyszukiwania zadañ przypisanych u¿ytkownikowi/wydzia³owi</p>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Nr sys.</HeaderTemplate>
                    <ItemTemplate>
                        <%# XPath("nr") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Interesant</HeaderTemplate>
                    <ItemTemplate>
                        <%# XPath("interesant") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Typ<br />
                        interesanta</HeaderTemplate>
                    <ItemTemplate>
                        <%# XPath("typInteresanta")%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Typ
                        <br />
                        zadania</HeaderTemplate>
                    <ItemTemplate>
                        <%# XPath("typ") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Status</HeaderTemplate>
                    <ItemTemplate>
                        <%# XPath("status") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Znak pisma/<br />
                        JRWA</HeaderTemplate>
                    <ItemTemplate>
                        <%# XPath("znakPisma") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Rodzaj</HeaderTemplate>
                    <ItemTemplate>
                        <%# XPath("rodzaj") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Data<br />
                        pocz.</HeaderTemplate>
                    <ItemTemplate>
                        <%# XPath("data") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Szczegó³y</HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" Text="zobacz" CommandName='<%# XPath("typ") %>' CommandArgument='<%# XPath("id") %>'
                            ToolTip="Przejœcie do szczegó³ów sprawy/dokumentu" OnCommand="wykonaj" />

                        <asp:ImageButton ID="LinkButton212" runat="server" 
                            CommandName='<%# XPath("typ") %>_S' CommandArgument='<%# XPath("id") %>' ToolTip="Przejœcie do skanów dokumentu"
                            OnCommand="wykonaj" ImageUrl="~/App_Themes/StandardLayout/img/skany.PNG" />                        
                      
                        <%--<asp:LinkButton runat="server" Text="zobacz skany" CommandName='<%# XPath("typ") %>_S' CommandArgument='<%# XPath("id") %>'
                            ToolTip="Przejœcie do skanów dokumentu" OnCommand="wykonaj" />--%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
