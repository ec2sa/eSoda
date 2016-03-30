<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/SingleColumn.Master"
    CodeBehind="edycjaTeczek.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.edycjaTeczek" %>

<%@ Register Src="../../Controls/NumeracjaSpraw.ascx" TagName="NumeracjaSpraw" TagPrefix="uc1" %>
<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
    <table class="layout fullWidth">
        <tr>
            <td>
                <asp:LinkButton ID="lnkBriefcaseAdd" runat="server" OnClick="lnkBriefcaseAdd_Click"
                    CausesValidation="False">Dodaj teczkê</asp:LinkButton>
                <a runat="server" id="lnkCreateBriefcaseGroup" visible="false">Podteczki</a>
                <asp:FormView ID="frmTeczka" runat="server" OnItemUpdating="frmTeczka_ItemUpdating"
                    OnItemInserting="frmTeczka_ItemInserting" Visible="False" OnModeChanging="frmTeczka_ModeChanging"
                    DataKeyNames="id" DataSourceID="odsBriefcase" OnDataBound="frmTeczka_DataBound">
                    <InsertItemTemplate>
                        <fieldset>
                            <legend>Dodawanie teczki</legend>
                            <table border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblTytul" runat="server" AssociatedControlID="txtTytul" Text="Tytu³:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtTytul" runat="server" Width="95%" MaxLength="500"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvTytul" runat="server" Display="Dynamic" ControlToValidate="txtTytul"
                                            ErrorMessage="*" Text="wymagane" ValidationGroup="AddBriefcase" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblRodzajSprawy" runat="server" AssociatedControlID="ddlRodzajSprawy"
                                            Text="Rodzaj sprawy:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlRodzajSprawy" runat="server" DataSourceID="dsRodzajeSpraw"
                                            DataTextField="nazwa" DataValueField="id" AppendDataBoundItems="True" OnDataBinding="ddlRodzajSprawy_DataBinding"
                                            ValidationGroup="CaseKindSelection">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:RequiredFieldValidator ID="rfvRodzajSprawy" runat="server" ControlToValidate="ddlRodzajSprawy"
                                            InitialValue="-1" Display="Dynamic" ValidationGroup="CaseKindSelection" ErrorMessage="*"
                                            Text="wybierz rodzaj sprawy" />
                                        <br />
                                        <asp:SqlDataSource ID="dsRodzajeSpraw" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
                                            SelectCommand="[Sprawy].[listaRodzajowSpraw]" SelectCommandType="StoredProcedure">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblRodzajeSpraw" runat="server" Text="Rodzaje spraw:" AssociatedControlID="lstCaseKinds"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:ListBox ID="lstCaseKinds" runat="server"></asp:ListBox>
                                        <asp:CustomValidator ID="cvRodzajeSpraw" runat="server" ErrorMessage="dodaj przynajmniej jeden rodzaj sprawy"
                                            OnServerValidate="cvRodzajeSpraw_ServerValidate" ValidationGroup="AddBriefcase">
                                        </asp:CustomValidator>
                                        <br />
                                        <asp:LinkButton ID="lnkAddCaseKind" runat="server" OnClick="lnkAddCaseKind_Click"
                                            ValidationGroup="CaseKindSelection">Dodaj rodzaj sprawy</asp:LinkButton><br />
                                        <asp:LinkButton ID="lnkRemoveCaseKind" runat="server" OnClick="lnkRemoveCaseKind_Click">Usuñ rodzaj sprawy</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblJRWA" runat="server" AssociatedControlID="txtJRWA" Text="JRWA:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtJRWA" runat="server"></asp:TextBox>
                                        <asp:LinkButton ID="lnkJRWATree" runat="server" OnClick="lnkJRWATree_Click">Wybierz JRWA...</asp:LinkButton>
                                        <br />
                                        <asp:Label ID="lblOpisJRWA" runat="server"></asp:Label>
                                        <asp:HiddenField ID="hfIdJRWA" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblWydzialy" runat="server" Text="Wydzia³y:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:LinkButton ID="lnkAddGroup" runat="server" CausesValidation="False" OnClick="lnkAddGroup_Click">Wybierz wydzia³...</asp:LinkButton>
                                        <br />
                                        <asp:ListBox ID="lstGroups" runat="server"></asp:ListBox>
                                        <asp:LinkButton ID="lnkRemoveGroup" runat="server" OnClick="lnkRemoveGroup_Click">Usuñ wydzia³ z listy</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblPracownicy" runat="server" Text="Pracownicy:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlPracownicy" runat="server">
                                        </asp:DropDownList>
                                        <asp:LinkButton ID="lnkAddWorker" runat="server" OnClick="lnkAddWorker_Click">Dodaj pracownika</asp:LinkButton>
                                        <br />
                                        <asp:ListBox ID="lstWorkers" runat="server"></asp:ListBox>
                                        <asp:LinkButton ID="lnkRemoveWorker" runat="server" OnClick="lnkRemoveWorker_Click">Usuñ pracownika z listy</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblOdpowiedzialny" runat="server" AssociatedControlID="ddlOdpowiedzialny"
                                            Text="Osoba odpowiedzialna:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlOdpowiedzialny" runat="server" DataTextField="pelnaNazwa"
                                            DataValueField="id">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvOdpowiedzialny" runat="server" ControlToValidate="ddlOdpowiedzialny"
                                            InitialValue="-1" ErrorMessage="*" Text="wymagane" ValidationGroup="AddBriefcase" />
                                        <asp:SqlDataSource ID="dsOdpowiedzialny" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
                                            SelectCommand="[Uzytkownicy].[listaPracownikow]" SelectCommandType="StoredProcedure">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblRok" runat="server" AssociatedControlID="txtRok" Text="Rok:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtRok" runat="server" Text="<%# DateTime.Now.Year.ToString() %>"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label runat="server" ID="lblAktywna" AssociatedControlID="cbAktywna" Text="Aktywna:" />
                                    </th>
                                    <td>
                                        <asp:CheckBox runat="server" ID="cbAktywna" Checked="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label runat="server" ID="lblArchiwalna" AssociatedControlID="cbArchiwalna" Text="Archiwalna:" />
                                    </th>
                                    <td>
                                        <asp:CheckBox runat="server" ID="cbArchiwalna" Checked="false" AutoPostBack="true"
                                            OnCheckedChanged="cbArchiwalna_CheckedChanged" />
                                        &nbsp;
                                        <asp:Label runat="server" ID="lblArchiwalnaInfo" ForeColor="Red" Visible="false"
                                            Text="Wszystkie sprawy w teczce zostan¹ zamkniête!" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <uc1:NumeracjaSpraw ID="NumeracjaSpraw1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:LinkButton ID="lnkAdd" runat="server" CommandName="Insert" Text="Dodaj" ValidationGroup="AddBriefcase" />
                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" Text="Anuluj" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </InsertItemTemplate>
                    <EditItemTemplate>
                        <fieldset>
                            <legend>Edycja teczki</legend>
                            <table border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblTytul" runat="server" AssociatedControlID="txtTytul" Text="Tytu³:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtTytul" runat="server" Width="95%" Text='<%# Bind("tytul") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvTytul" runat="server" Display="Dynamic" ControlToValidate="txtTytul"
                                            ErrorMessage="*" Text="wymagane" ValidationGroup="EditBriefcase" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblRodzajSprawy" runat="server" AssociatedControlID="ddlRodzajSprawy"
                                            Text="Rodzaj sprawy:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlRodzajSprawy" runat="server" DataSourceID="dsRodzajeSpraw"
                                            DataTextField="nazwa" DataValueField="id" ValidationGroup="CaseKindSelection"
                                            AppendDataBoundItems="True" OnDataBinding="ddlRodzajSprawy_DataBinding">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:RequiredFieldValidator ID="rfvRodzajSprawy" runat="server" ControlToValidate="ddlRodzajSprawy"
                                            InitialValue="-1" Display="Dynamic" ValidationGroup="CaseKindSelection" ErrorMessage="*"
                                            Text="wybierz rodzaj sprawy" />
                                        <asp:HiddenField ID="hfRodzajSprawy" runat="server" Value='<%# Bind("idRodzajuSprawy") %>' />
                                        <br />
                                        <asp:SqlDataSource ID="dsRodzajeSpraw" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
                                            SelectCommand="[Sprawy].[listaRodzajowSpraw]" SelectCommandType="StoredProcedure">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblRodzajeSpraw" runat="server" Text="Rodzaje spraw:" AssociatedControlID="lstCaseKinds"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:ListBox ID="lstCaseKinds" runat="server"></asp:ListBox>
                                        <asp:CustomValidator ID="cvRodzajeSpraw" runat="server" ErrorMessage="dodaj przynajmniej jeden rodzaj sprawy"
                                            OnServerValidate="cvRodzajeSpraw_ServerValidate" ValidationGroup="EditBriefcase">
                                        </asp:CustomValidator>
                                        <br />
                                        <asp:LinkButton ID="lnkAddCaseKind" runat="server" OnClick="lnkAddCaseKind_Click"
                                            ValidationGroup="CaseKindSelection">Dodaj rodzaj sprawy</asp:LinkButton><br />
                                        <asp:LinkButton ID="lnkRemoveCaseKind" runat="server" OnClick="lnkRemoveCaseKind_Click">Usuñ rodzaj sprawy</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblJRWA" runat="server" AssociatedControlID="txtJRWA" Text="JRWA:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtJRWA" runat="server" Text='<%# Bind("symbolJRWA") %>'></asp:TextBox>
                                        <asp:LinkButton ID="lnkJRWATree" runat="server" OnClick="lnkJRWATree_Click">Wybierz JRWA...</asp:LinkButton>
                                        <br />
                                        <asp:Label ID="lblOpisJRWA" runat="server" Text='<%# Bind("nazwaJRWA") %>'></asp:Label>
                                        <asp:HiddenField ID="hfIdJRWA" runat="server" Value='<%# Bind("idJRWA") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblWydzialy" runat="server" Text="Wydzia³y:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:LinkButton ID="lnkAddGroup" runat="server" CausesValidation="False" OnClick="lnkAddGroup_Click">Wybierz wydzia³...</asp:LinkButton>
                                        <br />
                                        <asp:ListBox ID="lstGroups" runat="server"></asp:ListBox>
                                        <asp:LinkButton ID="lnkRemoveGroup" runat="server" OnClick="lnkRemoveGroup_Click">Usuñ wydzia³ z listy</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblPracownicy" runat="server" Text="Pracownicy:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlPracownicy" runat="server">
                                        </asp:DropDownList>
                                        <asp:LinkButton ID="lnkAddWorker" runat="server" OnClick="lnkAddWorker_Click">Dodaj pracownika</asp:LinkButton>
                                        <br />
                                        <asp:ListBox ID="lstWorkers" runat="server"></asp:ListBox>
                                        <asp:LinkButton ID="lnkRemoveWorker" runat="server" OnClick="lnkRemoveWorker_Click">Usuñ pracownika z listy</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblOdpowiedzialny" runat="server" AssociatedControlID="ddlOdpowiedzialny"
                                            Text="Osoba odpowiedzialna:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddlOdpowiedzialny" runat="server" DataTextField="pelnaNazwa"
                                            DataValueField="id" OnDataBound="ddlOdpowiedzialny_DataBound">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvOdpowiedzialny" runat="server" ControlToValidate="ddlOdpowiedzialny"
                                            InitialValue="-1" ErrorMessage="*" Text="wymagane" ValidationGroup="EditBriefcase" />
                                        <asp:SqlDataSource ID="dsOdpowiedzialny" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
                                            SelectCommand="[Uzytkownicy].[listaPracownikow]" SelectCommandType="StoredProcedure">
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label ID="lblRok" runat="server" AssociatedControlID="txtRok" Text="Rok:"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txtRok" runat="server" Text='<%# Bind("rok") %>'></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label runat="server" ID="lblAktywna" AssociatedControlID="cbAktywna" Text="Aktywna:" />
                                    </th>
                                    <td>
                                        <asp:CheckBox runat="server" ID="cbAktywna" Checked='<%# Bind("aktywna") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="right">
                                        <asp:Label runat="server" ID="lblArchiwalna" AssociatedControlID="cbArchiwalna" Text="Archiwalna:" />
                                    </th>
                                    <%-- Enabled='<%# ((bool)Eval("archiwalna")==true ? false : true)%>'--%>
                                    <td>
                                        <asp:CheckBox runat="server" ID="cbArchiwalna" Checked='<%# Bind("archiwalna") %>'
                                            AutoPostBack="true" OnCheckedChanged="cbArchiwalna_CheckedChanged" />
                                        &nbsp;
                                        <asp:Label runat="server" ID="lblArchiwalnaInfo" ForeColor="Red" Visible="false"
                                            Text="Wszystkie sprawy w teczce zostan¹ zamkniête!" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <uc1:NumeracjaSpraw ID="NumeracjaSpraw1" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:LinkButton ID="lnkAdd" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="EditBriefcase" />
                                        <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" Text="Anuluj" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </EditItemTemplate>
                </asp:FormView>
                <asp:ObjectDataSource ID="odsBriefcase" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="GetBriefcase" TypeName="Pemi.Esoda.DataAccess.BriefcaseDAO" InsertMethod="InsertBriefcase"
                    UpdateMethod="UpdateBriefcase" OnInserting="odsBriefcase_Inserting" OnUpdating="odsBriefcase_Updating">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="0" Name="id" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                        <asp:Parameter Name="idJRWA" Type="Int32" />
                        <asp:Parameter Name="idRodzajuSprawy" Type="Int32" />
                        <asp:Parameter Name="prefix" Type="String" />
                        <asp:Parameter Name="suffix" Type="String" />
                        <asp:Parameter Name="rok" Type="Int32" />
                        <asp:Parameter Name="tytul" Type="String" />
                        <asp:Parameter Name="nastepnyNumer" Type="Int32" />
                        <asp:Parameter Name="adresat" Type="String" />
                        <asp:Parameter Name="aktywna" DefaultValue="true" Type="Boolean" />
                        <asp:Parameter Name="archiwalna" DefaultValue="false" Type="Boolean" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="idJRWA" Type="Int32" />
                        <asp:Parameter Name="idRodzajuSprawy" Type="Int32" />
                        <asp:Parameter Name="prefix" Type="String" />
                        <asp:Parameter Name="suffix" Type="String" />
                        <asp:Parameter Name="rok" Type="Int32" />
                        <asp:Parameter Name="tytul" Type="String" />
                        <asp:Parameter Name="nastepnyNumer" Type="Int32" />
                        <asp:Parameter Name="adresat" Type="String" />
                        <asp:Parameter Name="aktywna" Type="Boolean" DefaultValue="true" />
                        <asp:Parameter Name="archiwalna" Type="Boolean" DefaultValue="false" />
                    </InsertParameters>
                </asp:ObjectDataSource>
            </td>
            <td>
                <asp:Panel ID="pnlJRWA" runat="server" Visible="False" HorizontalAlign="Left">
                    <fieldset>
                        <legend>Drzewo JRWA</legend>
                        <asp:LinkButton ID="lnkAddJRWA" runat="server" OnClick="lnkAddJRWA_Click" Enabled="False"
                            Visible="False">Wstaw do teczki</asp:LinkButton>
                        <br />
                        <asp:LinkButton ID="lnkCloseJRWA" runat="server" OnClick="lnkCloseJRWA_Click">Ukryj</asp:LinkButton>
                        <br />
                        <asp:TreeView ID="tvJRWA" runat="server" AutoGenerateDataBindings="False" DataSourceID="dsxmlJRWA"
                            OnSelectedNodeChanged="tvJRWA_SelectedNodeChanged" ImageSet="Arrows">
                            <DataBindings>
                                <asp:TreeNodeBinding DataMember="jrwaItem" TextField="opis" ValueField="id" />
                            </DataBindings>
                            <SelectedNodeStyle CssClass="treeSelectedNode" />
                        </asp:TreeView>
                        <asp:XmlDataSource ID="dsxmlJRWA" runat="server" XPath="jrwaTree/jrwaItem" EnableCaching="False">
                        </asp:XmlDataSource>
                    </fieldset>
                </asp:Panel>
                <asp:Panel ID="pnlGroups" runat="server" Visible="False" HorizontalAlign="Left">
                    <fieldset>
                        <legend>Drzewo wydzia³ów</legend>
                        <asp:LinkButton ID="lnkAddGroupToBriefcase" runat="server" CausesValidation="False"
                            OnClick="lnkAddGroupToBriefcase_Click" Visible="False">Dodaj do teczki</asp:LinkButton>
                        <asp:LinkButton ID="lnkCloseGroups" runat="server" OnClick="lnkCloseGroups_Click">Ukryj</asp:LinkButton>
                        <br />
                        <asp:TreeView ID="tvGroups" runat="server" DataSourceID="dsxmlGroups" AutoGenerateDataBindings="False"
                            ImageSet="Arrows" OnSelectedNodeChanged="tvGroups_SelectedNodeChanged">
                            <DataBindings>
                                <asp:TreeNodeBinding DataMember="Group" TextField="Text" ValueField="Value" />
                            </DataBindings>
                            <SelectedNodeStyle CssClass="treeSelectedNode" />
                        </asp:TreeView>
                        <asp:XmlDataSource ID="dsxmlGroups" runat="server" TransformFile="~/Aplikacje/PanelAdministracyjny/xsltGroups.xslt"
                            XPath="Groups/Group" EnableCaching="False"></asp:XmlDataSource>
                    </fieldset>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <fieldset>
        <legend>Lista teczek</legend>Przed.:<asp:TextBox runat="server" ID="fPrefix" Style="width: 3em;" />&nbsp;
        Przy.:<asp:TextBox runat="server" ID="fSuffix" Style="width: 3em;" />&nbsp; Rok:<asp:TextBox
            runat="server" ID="fYear" Style="width: 3em;" />&nbsp; Archiwalna:<asp:CheckBox runat="server"
                ID="fIsArchive" />&nbsp; Aktywna:<asp:CheckBox runat="server" ID="fIsActive" />&nbsp;
        <asp:Button runat="server" ID="btnFilter" Text="Filtruj" 
            onclick="btnFilter_Click" />
        <asp:Button runat="server"
            ID="btnClearFilter" Text="Wyczyœæ filtry" onclick="btnClearFilter_Click" />
        <asp:GridView ID="gvNewBriefcaseList" runat="server" CssClass="grid" DataSourceID="odsCasesList"
            AutoGenerateColumns="False" EnableModelValidation="True" AllowPaging="True" PageSize="20" OnRowCommand="gvBriefcaseList_RowCommand">
            <Columns>
                <asp:TemplateField SortExpression="Prefix">
                    <HeaderTemplate>
                        <label title="Przedrostek (symbol wydzia³u)">
                            Przed.</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Prefix") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Suffix">
                    <HeaderTemplate>
                        <label title=" Przyrostek (JRWA+dodatkowe oznaczenie)">
                            Przyr.</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Suffix") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Year">
                    <HeaderTemplate>
                        Rok
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("Year") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tytu³" SortExpression="Title">
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="OrganizationalUnit">
                    <HeaderTemplate>
                        <label title="Komórka organizacyjna">
                            Kom.</label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("OrganizationalUnit") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="IsActive">
                    <HeaderTemplate>
                        <asp:Label ID="Label9" runat="server" Text="Aktywna" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("IsActive") %>' Enabled="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="IsArchive">
                    <HeaderTemplate>
                        <asp:Label ID="Label10" runat="server"  Text="Archiwalna" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("IsArchive") %>' Enabled="false" />
                    </ItemTemplate>
                </asp:TemplateField>
 <asp:TemplateField HeaderText="Operacje">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEditBriefcase" runat="server" Text="edytuj" CommandName="select" CommandArgument='<%# Eval("id") %>' />
                            <br />
                            <asp:LinkButton ID="lnkPreviewBriefcase" runat="server" Text="podgl¹d" CommandName="preview" CommandArgument='<%# Eval("id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsCasesList" runat="server" EnablePaging="True" OnSelecting="odsCasesList_Selecting"
            SelectCountMethod="GetCasesCount" SelectMethod="GetCases" TypeName="Pemi.Esoda.DataAccess.CaseListDAO"
            OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:Parameter Name="filter" Type="Object" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </fieldset>
</asp:Content>
