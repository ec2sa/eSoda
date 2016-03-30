<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
    CodeBehind="PrzegladDziennika.aspx.cs" Inherits="Pemi.Esoda.Web.UI.PrzegladDziennika"
    Title="Przegl¹d dziennika" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>
        Przegl¹danie dziennika kancelaryjnego</h2>
    <div id="singleColumn" runat="server">
        <asp:LinkButton runat="server" ID="rezerwacja" Text="[Rezerwacja pozycji w rejestrze]" />
        <asp:LinkButton runat="server" ID="rezerwacjaF" Text="[Rezerwacja pozycji w rejestrze faktur]"
            OnClick="rezerwujPozycjeRF" />
        <asp:LinkButton runat="server" Visible="true" ID="esp" Text="[SprawdŸ ESP]" OnClick="esp_Click" />
        <asp:LinkButton runat="server" Visible="true" ID="epuap" Text="[SprawdŸ skrytkê ePUAP]"
            OnClick="epuap_Click" />
        <hr />
        <table class="fullWidth" style="background-color: #F7F5F2;">
            <tr>
                <td style="width: 18em;">
                    <strong style="vertical-align: middle;">
                        <asp:RadioButton runat="server" ID="rbDocuments" Text="Dokumenty" GroupName="itemTypes"
                            Checked="true" AutoPostBack="true" OnCheckedChanged="toggleItemType" />
                        <asp:RadioButton runat="server" ID="rbInvoices" Text="Faktury" GroupName="itemTypes"
                            AutoPostBack="true" OnCheckedChanged="toggleItemType" /></strong>
                </td>
                <td>
                    <h3>
                        Filtrowanie rejestru</h3>
                </td>
            </tr>
        </table>
        <%--<fieldset>
<legend>Filtrowanie rejestru</legend>--%>
        <asp:Panel ID="pnlSearch" runat="server" DefaultButton="zastosujFiltr">
            <asp:Literal runat="Server" ID="opisOkresu" />
            &nbsp;
            <asp:LinkButton runat="server" ID="btnToggleFilters" Text="ukryj filtry" OnClick="toggleFilters" />
            <table class="searchForm fullWidth" runat="server" id="searchTable">
                <tr>
                    <th>
                        <asp:Label ID="Label5" runat="Server" AssociatedControlID="zakresDat" Text="Wybrany okres" />
                    </th>
                    <th>
                        <asp:Label ID="Label1" runat="Server" AssociatedControlID="sdataWplywu" Text="Data wp³ywu" />
                    </th>
                    <th>
                        <asp:Label ID="Label2" runat="Server" AssociatedControlID="sdataPisma" Text="Data pisma" />
                    </th>
                    <th>
                        <asp:Label ID="Label3" runat="Server" AssociatedControlID="snumerPisma" Text="Znak pisma" />
                    </th>
                    <th>
                        <asp:Label ID="Label4" runat="Server" AssociatedControlID="snadawca" Text="Interesant" />
                    </th>
                    <td rowspan="6">
                        <asp:LinkButton runat="Server" ID="zastosujFiltr" Text="Szukaj" ValidationGroup="searching" />
                        <asp:LinkButton runat="Server" ID="czyscFiltr" Text="Wyczyœæ" OnClick="czyscFiltry" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList runat="server" ID="zakresDat" AutoPostBack="true">
                            <asp:ListItem Value="dzisiaj" Text="Dzisiaj" />
                            <asp:ListItem Value="tydzien" Text="Bie¿¹cy tydzieñ" />
                            <asp:ListItem Value="miesiac" Text="Bie¿¹cy miesi¹c" Selected="true" />
                            <asp:ListItem Value="zakres" Text="Zakres dat" />
                        </asp:DropDownList>
                        <asp:LinkButton runat="Server" ID="changeDateRangeLink" Text="zmieñ zakres" />
                        <br />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="sdataWplywu" />
                        <ajax:CalendarExtender ID="sdataWplywu_CalendarExtender" runat="server" TargetControlID="sdataWplywu" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="sdataPisma" />
                        <ajax:CalendarExtender ID="sdataPisma_CalendarExtender" runat="server" TargetControlID="sdataPisma" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="snumerPisma" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="snadawca" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="Label6" runat="Server" AssociatedControlID="kategoria" Text="Kategoria" />
                    </th>
                    <th>
                        <asp:Label ID="lblRodzajDokumentu" runat="server" AssociatedControlID="ddlRodzajDokumentu"
                            Text="Rodzaj dokumentu" />
                    </th>
                    <th>
                        <asp:Label ID="Label7" runat="Server" AssociatedControlID="wartoscKategorii" Text="Nr" />
                    </th>
                    <th>
                        <asp:Label ID="Label8" runat="Server" AssociatedControlID="typKorespondencji" Text="Typ koresp." />
                    </th>
                    <th>
                        <asp:Label ID="Label9" runat="Server" AssociatedControlID="wartoscTypKorespondencji"
                            Text="Nr" />
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList runat="server" ID="kategoria" AutoPostBack="True" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRodzajDokumentu" runat="server" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="wartoscKategorii" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="typKorespondencji" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="wartoscTypKorespondencji" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <asp:Label ID="label10" runat="server" AssociatedControlID="" Text="Status" />
                    </th>
                    <th colspan="2">
                        <asp:Label ID="Label11" runat="server" Text="Wydzia³" />
                    </th>
                    <th colspan="2">
                        <asp:Label ID="Label12" runat="server" Text="Pracownik" />
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlWydzial" runat="server" AutoPostBack="True" />
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlPracownik" runat="server" />
                    </td>
                </tr>
            </table>
            <div runat="server" id="przedzialDat" visible="false">
                <table>
                    <tr>
                        <th>
                            Data pocz¹tkowa
                        </th>
                        <th>
                            Data koñcowa
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:Calendar runat="server" ID="dataPoczatkowa" Visible="false" />
                            <asp:Label ID="lblOd" runat="server" Text="Od: " />
                            <asp:TextBox ID="txtDataOd" runat="server" />
                            <asp:CompareValidator ID="cmpvDataOd" runat="server" ControlToValidate="txtDataOd"
                                Type="Date" Operator="DataTypeCheck" ValidationGroup="searching" ErrorMessage="*"
                                Text="B³êdny format daty (rrrr-mm-dd)" />
                            <ajax:CalendarExtender ID="txtDataOd_CalendarExtender" runat="server" TargetControlID="txtDataOd" />
                        </td>
                        <td>
                            <asp:Calendar runat="server" ID="dataKoncowa" Visible="false" />
                            <asp:Label ID="lblDo" runat="server" Text="Do: " />
                            <asp:TextBox ID="txtDataDo" runat="server" />
                            <asp:CompareValidator ID="cmpvDataDo" runat="server" ControlToValidate="txtDataDo"
                                Type="Date" Operator="DataTypeCheck" ValidationGroup="searching" ErrorMessage="*"
                                Text="B³êdny format daty (rrrr-mm-dd)" />
                            <ajax:CalendarExtender ID="txtDataDo_CalendarExtender" runat="server" TargetControlID="txtDataDo" />
                        </td>
                    </tr>
                </table>
                <asp:LinkButton runat="server" ID="zatwierdzWyborDat" Text="ZatwierdŸ wybór zakresu" /><br />
                <br />
            </div>
            <asp:LinkButton runat="server" ID="lbtnAdvancedSearch" Text="Wyszukiwanie uproszczone"
                PostBackUrl="~/Aplikacje/DziennikKancelaryjny/PrzegladDziennikaSimple.aspx" />
        </asp:Panel>
        <div id="pager">
            Nawigacja (strony dziennika):&nbsp;
            <asp:LinkButton runat="server" ID="pierwszaStrona" Text="pierwsza" CommandName="Page"
                CommandArgument="First" OnCommand="zmianaStrony" />
            &nbsp;
            <asp:LinkButton runat="server" ID="poprzedniaStrona" Text="poprzednia" CommandName="Page"
                CommandArgument="Prev" OnCommand="zmianaStrony" />
            &nbsp; <strong>aktualna</strong>&nbsp;<asp:DropDownList runat="Server" ID="nrStrony"
                AutoPostBack="true" OnSelectedIndexChanged="zmianaNumeruStrony" />
            &nbsp;z <strong>
                <asp:Literal runat="Server" ID="liczbaStron" />
            </strong>&nbsp;
            <asp:LinkButton runat="server" ID="nastepnaStrona" Text="nastêpna" CommandName="Page"
                CommandArgument="Next" OnCommand="zmianaStrony" />
            &nbsp;
            <asp:LinkButton runat="server" ID="ostatniaStrona" Text="ostatnia" CommandName="Page"
                CommandArgument="Last" OnCommand="zmianaStrony" />
            &nbsp; Pozycje na stronie:
            <asp:DropDownList runat="server" ID="rozmiarStrony" OnSelectedIndexChanged="zmianaNumeruStrony"
                AutoPostBack="true">
                <asp:ListItem Value="5" />
                <asp:ListItem Value="10" />
                <asp:ListItem Value="15" />
                <asp:ListItem Value="20" />
                <asp:ListItem Value="25" />
                <asp:ListItem Value="30" />
                <asp:ListItem Value="50000" Text="wszystkie" />
            </asp:DropDownList>
        </div>
        <div class="dziennikScroll">
            <asp:GridView GridLines="None" runat="server" ID="lista" CssClass="listaPozycjiRejestru grid fullWidth"
                AutoGenerateColumns="false" EmptyDataText="Brak pozycji w wybranym okresie" OnRowCommand="wykonaniePolecenia"
                AlternatingRowStyle-CssClass="pozycjaNieparzysta">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Nr
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#XPath("numer")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ControlStyle-Width="8em">
                        <HeaderTemplate>
                            Data:<br />
                            [R]ejestracji<br />
                            [W]p³ywu<br />
                            [P]isma
                        </HeaderTemplate>
                        <ItemTemplate>
                            <strong>R:</strong><%# XPath("dataRejestracji")%><br />
                            <strong>W:</strong><%# XPath("dataWplywu")%><br />
                            <strong>P:</strong><%# XPath("dataPisma")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <% if (rbDocuments.Checked)
                               { %>
                            Znak pisma
                            <% }
                               else
                               { %>
                            <strong>[N]</strong>umer faktury
                            <br />
                            <strong>[K]</strong>wota
                            <%} %>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <% if (rbDocuments.Checked)
                               { %>
                            <%#XPath("numerPisma")%>
                            <% }
                               else
                               { %>
                            <strong>N: </strong>
                            <%#XPath("numerPisma")%>
                            <br />
                            <strong>K: </strong>
                            <%#XPath("kwota")%>
                            z³
                            <%} %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                             <% if (rbDocuments.Checked)
                               { %>
                                    Interesant
                            <% }
                               else
                               { %>
                                    Nadawca pisma
                            <% } %>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#XPath("nadawca")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Opis<br />
                            korespondencji
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# XPath("opis") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Typ koresp./<br />
                            numer
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# (XPath("typKorespondencji/@nazwa") != null) ? XPath("typKorespondencji/@nazwa") : XPath("typKorespondencji/rodzaj")%>
                            <br />
                            <%# (XPath("typKorespondencji/wartosc","nr: {0}").ToString().Length > 0) ? XPath("typKorespondencji/wartosc","nr: {0}") : XPath("typKorespondencji","nr: {0}") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Klasyfikacja dokumentu<br />
                            [K]ategoria [R]odzaj [N]umer
                        </HeaderTemplate>
                        <ItemTemplate>
                            <strong>K:</strong>
                            <%#XPath("klasyfikacjaDokumentu/kategoria") %><br />
                            <strong>R:</strong>
                            <%#XPath("klasyfikacjaDokumentu/rodzaj")%><br />
                            <strong>N:</strong>
                            <%#XPath("klasyfikacjaDokumentu/wartosc")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Znak Referenta:<br />
                            [W]ydzia³<br />
                            [P]racownik
                        </HeaderTemplate>
                        <ItemTemplate>
                            <strong>W:</strong>
                            <%#XPath("znakReferenta/wydzial") %><br />
                            <strong>P:</strong>
                            <%#XPath("znakReferenta/pracownik") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Status
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# XPath("status") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-CssClass="screenOnly" HeaderStyle-CssClass="screenOnly">
                        <HeaderTemplate>
                            Op.
                        </HeaderTemplate>
                        <ItemTemplate>
                             <% if (rbInvoices.Checked == false)
                               { %>
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="itemEdit" CommandArgument='<%#XPath("numer") %>'
                                ToolTip="edycja pozycji rejestru" Text="[E]" />
                            <br />
                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="itemScans" CommandArgument='<%#XPath("numer") %>'
                                ToolTip="skany skojarzone z pozycj¹ rejestru" Text="[S]"  Font-Bold='<%#XPath("skany")!=null%>' />
                            <br />
                            <asp:LinkButton ID="LinkButton3" runat="server" CommandName="itemHistory" CommandArgument='<%#XPath("numer") %>'
                                ToolTip="historia zmian pozycji rejestru" Text="[SZ]" />
                            <br />
                            <asp:LinkButton ID="LinkButton4" runat="server" CommandName="itemDocHistory" CommandArgument='<%#XPath("numer") %>'
                                ToolTip="historia dokumentu pozycji dziennika" Text="[HD]" />
                            <%}
                               else
                               { %>
                            <asp:LinkButton ID="LinkButton5" runat="server" CommandName="itemEditRF" CommandArgument='<%#XPath("numer") %>'
                                ToolTip="edycja pozycji rejestru faktur" Text="[E]" />
                            <br />
                            <asp:LinkButton ID="LinkButton6" runat="server" CommandName="itemScansRF" CommandArgument='<%#XPath("numer") %>'
                                ToolTip="skany skojarzone z pozycj¹ rejestru faktur" Text="[S]"  Font-Bold='<%#XPath("skany")!=null%>'  />
                            <br />
                            <asp:LinkButton ID="LinkButton7" runat="server" CommandName="itemHistoryRF" CommandArgument='<%#XPath("numer") %>'
                                ToolTip="historia zmian pozycji rejestru faktur" Text="[SZ]" />
                            <br />
                            <asp:LinkButton ID="LinkButton8" runat="server" CommandName="itemDocHistoryRF" CommandArgument='<%#XPath("numer") %>'
                                ToolTip="historia dokumentu pozycji rejestru faktur" Text="[HD]" />
                            <%} %>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblID" Text='<%# XPath("idRejestru") %>' /></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <%--</fieldset>--%>
    </div>
    <div id="dailyLogDefInfo" runat="server">
        <asp:Label runat="server" ID="dailyLogDefMessage" ForeColor="Red" Text="Brak definicji dziennika kancelaryjnego na bie¿¹cy rok!" />
    </div>
</asp:Content>
