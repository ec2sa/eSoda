<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master"
    AutoEventWireup="true" CodeBehind="Przegladanie.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.RKW.Przegladanie" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>
        Rejestr Korespondencji Wychodzącej</h2>
    <div id="singleColumn" runat="server">
        <asp:Label runat="server" AssociatedControlID="docIdToRegister" Text="Nr sys.:" />
        <asp:TextBox runat="server" ID="docIdToRegister" Style="width: 7em;" />
        <asp:LinkButton runat="server" ID="register" CommandName="register" Text="Zarejestruj dokument"
            OnCommand="registerDocument" ValidationGroup="registration" OnClientClick="document.getElementById('ctl00_main_rkwMessage').innerHtml='';" />
        &nbsp;
        <asp:RequiredFieldValidator ValidationGroup="registration" runat="server" ControlToValidate="docIdToRegister"
            ErrorMessage="Trzeba wpisać nr systemowy dokumentu!" Display="Dynamic" />
        <asp:CompareValidator runat="server" ValidationGroup="registration" ControlToValidate="docIdToRegister"
            Display="Dynamic" Operator="DataTypeCheck" Type="Integer" ErrorMessage="Nr sys. dokumentu musi być liczbą całkowitą!" />
        <asp:CustomValidator runat="server" ValidationGroup="registration" ControlToValidate="docIdToRegister"
            Display="Dynamic" ErrorMessage="Wybrany dokument nie istnieje lub został już zarejestrowany."
            OnServerValidate="checkDocument" />
        <hr />
        <fieldset>
            <legend>Przeglądanie zawartości rejestru</legend>
            <h3>
                Filtrowanie rejestru</h3>
            <table class="layout">
                <tr>
                    <td style="padding-right: 1em;">
                        <asp:Label runat="server" AssociatedControlID="fEntryNumber" Text="Nr pozycji" /><br />
                        <asp:TextBox runat="server" ID="fEntryNumber" Style="width: 4em;" />
                    </td>
                    <td style="padding-right: 1em;">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="fDateFrom" Text="Data rejestracji" /><br />
                        Od:
                        <asp:TextBox runat="server" ID="fDateFrom" Style="width: 8em;" /><br />
                        Do:
                        <asp:TextBox runat="server" ID="fDateTo" Style="width: 8em;" />
                    </td>
                    <td style="padding-right: 1em;">
                        <asp:Label ID="Label2" runat="server" AssociatedControlID="fOrganizationalUnit" Text="Wydział" /><br />
                        <asp:DropDownList runat="server" ID="fOrganizationalUnit" />
                    </td>
                    <td style="padding-right: 1em;">
                        <asp:Label ID="Label3" runat="server" AssociatedControlID="fRemarks" Text="Uwagi" /><br />
                        <asp:TextBox runat="server" ID="fRemarks" Style="width: 8em;" />
                    </td>
                    <td>
                        <asp:Label runat="server" AssociatedControlID="fNewestFirst" Text="Sortowanie" /><br />
                        <asp:DropDownList runat="server" ID="fNewestFirst">
                            <asp:ListItem Value="1" Text="od najnowszego" />
                            <asp:ListItem Value="0" Text="od najstarszego" />
                        </asp:DropDownList>
                    </td>
                    <td style="vertical-align: middle; padding-left: 2em;">
                        <asp:Button runat="server" ID="btnApplyFilter" Text="Filtruj" OnClick="applyFilters" />
                    </td>
                </tr>
            </table>
            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="fDateFrom" />
            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="fDateTo" />
            <hr />
            <div id="pager">
                Nawigacja (strony rejestru):&nbsp;
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
                <asp:LinkButton runat="server" ID="nastepnaStrona" Text="następna" CommandName="Page"
                    CommandArgument="Next" OnCommand="zmianaStrony" />
                &nbsp;
                <asp:LinkButton runat="server" ID="ostatniaStrona" Text="ostatnia" CommandName="Page"
                    CommandArgument="Last" OnCommand="zmianaStrony" />
                &nbsp; Pozycje na stronie:
                <asp:DropDownList runat="server" ID="rozmiarStrony" OnSelectedIndexChanged="zmianaRozmiaruStrony"
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
                <asp:GridView runat="server" ID="rkwGrid" AutoGenerateColumns="false" CssClass="grid">
                    <Columns>
                        <asp:BoundField HeaderText="Nr" DataField="NrPozycji" />
                        <asp:BoundField HeaderText="Data rej." DataField="DataRejestracji" />
                        <asp:BoundField HeaderText="Znak pisma" DataField="ZnakPisma" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <strong>[W]</strong>ydział<br />
                                <strong>[P]</strong>racownik</HeaderTemplate>
                            <ItemTemplate>
                                <strong>W: </strong>
                                <%#Eval("Wydzial") %><br />
                                <strong>P: </strong>
                                <%#Eval("Pracownik") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Klasyfikacja dokumentu" DataField="TypDokumentu" />
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Adresat</HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("NazwaAdresata") %><br />
                                <%#Eval("UlicaAdresata") %><br />
                                <%#Eval("KodIMiastoAdresata") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Typ korespondencji" DataField="TypKorespondencji" />
                        <asp:BoundField HeaderText="Uwagi" DataField="Uwagi" />
                    </Columns>
                    <EmptyDataTemplate>
                        brak wpisów w rejestrze.</EmptyDataTemplate>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
</asp:Content>
