<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master"
    AutoEventWireup="true" CodeBehind="DodawanieSkanow.aspx.cs" Inherits="Pemi.Esoda.Web.UI.DodawanieSkanow"
    Title="Dodawanie skanÛw do pozycji dziennika" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>Dodawanie skanÛw do pozycji rejestru
    <asp:Literal runat="server" ID="numerPozycji" /></h2>
    <asp:UpdatePanel runat="server" ID="up1">
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:LinkButton ID="LinkButton1" runat="server" Text="PowrÛt do przeglπdania dziennika"
                            OnClick="GoBack" CssClass="link" />
                        &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" Text="PowrÛt do listy skanÛw pozycji dziennika"
                            OnClick="GoBackToEdit" CssClass="link" />
                    </td>
                    <td style="text-align: right">
                        Nowe skany:
                        <asp:Literal runat="server" ID="liczbaNowychSkanow" Text="0" />&nbsp;
                        <asp:LinkButton runat="Server" ID="pobierzNoweSkany" Text="Przetwarzaj" OnClick="pobierzSkany" />
                    </td>
                </tr>
            </table>
            <fieldset><legend>Wyszukiwanie skanÛw</legend>
            <asp:Label AssociatedControlID="ddlLokalizacja" runat="server" ID="Label1">Lokalizacja:</asp:Label>
            <asp:DropDownList runat="Server" ID="ddlLokalizacja" AutoPostBack="false" OnSelectedIndexChanged="listy_SelectedIndexChanged" />
            <asp:Label AssociatedControlID="ddlUrzadzenie" runat="server" ID="etykietaWyszukiwania">Urzπdzenie:</asp:Label>
            <asp:DropDownList runat="Server" ID="ddlUrzadzenie" AutoPostBack="false" OnSelectedIndexChanged="listy_SelectedIndexChanged" />
            <asp:Label AssociatedControlID="ddlRodzajDokumentu" runat="server" ID="Label2">Rodzaj:</asp:Label>
            <asp:DropDownList runat="Server" ID="ddlRodzajDokumentu" AutoPostBack="false" OnSelectedIndexChanged="listy_SelectedIndexChanged" />
            <asp:Label AssociatedControlID="ddlZrodloDokumentu" runat="server" ID="Label3">èrÛd≥o:</asp:Label>
            <asp:DropDownList runat="Server" ID="ddlZrodloDokumentu" AutoPostBack="false" OnSelectedIndexChanged="listy_SelectedIndexChanged" />
            <asp:LinkButton runat="server" ID="szukajDokumentow" Text=" &raquo; wyúwietl dokumenty &raquo;"
                CssClass="link" ToolTip="szukanie dokumentÛw spe≥niajπcych podane kryteria" OnClick="filtrujSkany" />
            </fieldset>
            <%--<table style="width:100%;">
<tr>
<td>A</td>
<td>B
</td>
</tr>
</table>--%>
            <div id="leftColumn">
                <div runat="server" id="widokListy">
                    <h3>Skany oczekujπce</h3>
                    <asp:LinkButton runat="server" ID="opiszGrupe" CssClass="link" Text="&raquo; Opisz grupÍ dokumentÛw &raquo;"
                        ToolTip="opisuje zaznaczone dokumenty jako ca≥oúÊ" OnCommand="opisanieGrupySkanow" />&nbsp;
                    <%--	<asp:LinkButton runat="server" ID="rodzajSkanow" Text="Pokaø nieprzypisane" CommandArgument="nieprzypisane" OnClick="zmienRodzajSkanow" /> --%>
                    <!-- div class="ramka" -->
                    <asp:Repeater runat="server" Visible="false" ID="listaSkanow" EnableViewState="true"
                        OnItemCommand="dokumenty_ItemCommand" OnItemDataBound="dokumenty_ItemDataBound">
                        <ItemTemplate>
                            <div class="skanInfo" runat="server" id="skanInfo">
                                <table>
                                    <tr>
                                        <td rowspan="3" style="width: 60px">
                                            <asp:Label ID="lblImageNAme" runat="server" Text='<%# XPath("pierwszaStrona") %>' /><br />
                                            <asp:Label ID="lblImageId" runat="server" Text=' <%# XPath("guid") %>' />
                                            <asp:ImageButton runat="server" ID="btnMiniatura" ImageUrl='<%# Pemi.Esoda.Tools.Configuration.VirtualTemporaryDirectory+"/"+ XPath("pierwszaStrona")%>'
                                                ToolTip="klikniÍcie - przejúcie do opisywania dokumentu" AlternateText="miniatura pierwszej strony dokumentu"
                                                CommandArgument='<%#XPath("nazwaPlikuDokumentu")%>' CommandName="opiszPojedynczy" /><br />
                                            <asp:CheckBox runat="Server" ID="cbDodajDoOpisu" CssClass="cbGrupuj" ToolTip="pozwala wybraÊ kilka dokumentÛw do jednokrotnego opisania"
                                                Text="grupuj" />
                                            <asp:LinkButton runat="Server" CssClass="link" ID="edycjaDokumentu" Text="Edycja"
                                                CommandName="edytujDokument" CommandArgument='<%#XPath("nazwaPlikuDokumentu")%>' />
                                        </td>
                                        <th style="width: 8em;">Dokument</th>
                                        <td>
                                            <%#XPath("nazwaPlikuDokumentu")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th style="width: 8em;">Liczba stron</th>
                                        <td>
                                            <%#XPath("liczbaStron")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Data pobrania</th>
                                        <td>
                                            <%#XPath("dataPobrania")%>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:GridView ID="gvListSkanow" CssClass="grid fullWidth" runat="server" AllowPaging="True"
                        PagerSettings-Position="TopAndBottom" AutoGenerateColumns="False" DataSourceID="odsScansList"
                        OnRowCommand="gvListSkanow_RowCommand" OnRowDataBound="gvListSkanow_RowDataBound">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="skanInfo" runat="server" id="skanInfo">
                                        <table>
                                            <tr>
                                                <td rowspan="3" style="width: 60px">
                                                    <asp:ImageButton runat="server" ID="btnMiniatura" ImageUrl='<%# Pemi.Esoda.Tools.Configuration.VirtualTemporaryDirectory+"/"+ Eval("pierwszaStrona") %>'
                                                        ToolTip="klikniÍcie - przejúcie do opisywania dokumentu" AlternateText="miniatura pierwszej strony dokumentu"
                                                        CommandArgument='<%#Eval("nazwaPlikuDokumentu")%>' CommandName="opiszPojedynczy" /><br />
                                                    <asp:CheckBox runat="Server" ID="cbDodajDoOpisu" CssClass="cbGrupuj" ToolTip="pozwala wybraÊ kilka dokumentÛw do jednokrotnego opisania"
                                                        Text="grupuj" />
                                                    <asp:LinkButton runat="Server" CssClass="link" ID="edycjaDokumentu" Text="Edycja"
                                                        CommandName="edytujDokument" CommandArgument='<%#Eval("nazwaPlikuDokumentu")%>' />
                                                </td>
                                                <th style="width: 8em;">Dokument</th>
                                                <td>
                                                    <%#Eval("nazwaPlikuDokumentu")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th style="width: 8em;">Liczba stron</th>
                                                <td>
                                                    <%#Eval("liczbaStron")%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Data pobrania</th>
                                                <td>
                                                    <%#Eval("dataPobrania")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
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
                                        <asp:DropDownList ID="ddlPageSelector" runat="server" AutoPostBack="True" OnDataBound="ddlPageSelector_DataBound"
                                            OnSelectedIndexChanged="ddlPageSelector_SelectedIndexChanged" />
                                        z
                                        <%=gvListSkanow.PageCount %>
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
                    <asp:ObjectDataSource ID="odsScansList" runat="server" OldValuesParameterFormatString="original_{0}"
                        SelectMethod="GetScansList" TypeName="Pemi.Esoda.Tools.Scans">
                      <SelectParameters>
                        <asp:ControlParameter ControlID="ddlUrzadzenie" PropertyName="SelectedValue" Name="urzadzenie" />
                        <asp:ControlParameter ControlID="ddlLokalizacja" PropertyName="SelectedValue" Name="lokalizacja" />
                        <asp:ControlParameter ControlID="ddlRodzajDokumentu" PropertyName="SelectedValue" Name="rodzajDokumentu" />
                        <asp:ControlParameter ControlID="ddlZrodloDokumentu" PropertyName="SelectedValue" Name="zrodloDokumentu" />
                      </SelectParameters>
                        </asp:ObjectDataSource>
                    <hr />
                </div>
                <!-- /div -->
                <div runat="Server" id="widokSzczegolow" visible="false">
                    <h3>Podglπd pierwszej strony skanu</h3>
                    <asp:LinkButton runat="server" ID="powrotDoListy" CssClass="link" Text="´powrÛt do listy´"
                        ToolTip="powrÛt do listy skanÛw" OnClick="powrotDoListy_Click" />&nbsp; <span runat="Server"
                            id="widokNawigacjiZalacznikow">
                            <asp:LinkButton runat="server" ID="poprzedniZal" CssClass="link" Text="&laquo; Poprzedni w grupie &laquo;"
                                OnClick="poprzedniZalacznik" />
                            &nbsp;<asp:Literal runat="server" ID="pozycjaZalacznika" Text="1 z n" />
                            &nbsp;
                            <asp:LinkButton runat="server" ID="nastepnyZal" CssClass="link" Text="&raquo; NastÍpny w grupie &raquo;"
                                OnClick="nastepnyZalacznik" />
                        </span>
                    <hr />
                    <hr />
                    <div>
                        <asp:Image runat="server" ID="podglad" />
                    </div>
                </div>
            </div>
            <div id="rightColumn">
                <div id="opisywanieSkanu" runat="Server" visible="false">
                    <asp:CheckBox runat="server" ID="isMainItem" Text="G≥Ûwny?" /><br />
                    <asp:Label ID="Label4" runat="server" AssociatedControlID="opisElementu" Text="KrÛtki opis" />
                    <asp:TextBox runat="server" ID="opisElementu" TextMode="MultiLine" Rows="5" Columns="40" /><br />
                    <asp:LinkButton runat="Server" ID="zapisanieZmian" Text="Dodaj" OnClick="dodajSkan" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
