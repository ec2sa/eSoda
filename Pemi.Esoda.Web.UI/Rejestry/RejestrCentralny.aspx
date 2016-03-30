<%@ Page Title="Rejestry Centralne" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master"
    AutoEventWireup="true" CodeBehind="RejestrCentralny.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Rejestry.RejestrCentralny" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2></h2>
    Rejestr:<asp:DropDownList ID="registryType" runat="server" DataSourceID="registryTypesDS"
        DataTextField="RegistryName" DataValueField="ID"></asp:DropDownList>
    &nbsp; Rok:<asp:TextBox ID="registryYear" runat="server" style="width:4em;"></asp:TextBox>
    &nbsp;
    Wyświetlanie:<asp:DropDownList runat="server" ID="registryOrder">
    <asp:ListItem Value="asc" Text="od najstarszych" />
    <asp:ListItem Value="desc" Text="od najnowszych" />
    </asp:DropDownList>
    <asp:Button runat="server" Text="Wybierz" OnClick="changeRegistry" />
    <asp:Panel runat="server" ID="insertWrapper">
    <asp:FormView ID="formView4" runat="server" DataKeyNames="ID" DataSourceID="registryDS"
        EnableModelValidation="True" CssClass="fullWidth" OnItemInserting="formView_ItemInserting"
        OnItemInserted="formView_ItemInserted">
        <InsertItemTemplate>
            <fieldset><legend>Dodawanie pozycji Rejestru Zarządzeń</legend>
                <asp:Label ID="Label1" runat="server" AssociatedControlID="fDocumentID" Text="Nr sys. dok." />
                <asp:TextBox runat="server" ID="fDocumentID" />
                <asp:LinkButton runat="server" ID="lnkAssignDataFromDocument" Text="<-" CommandName="assignDataToInsert"
                    OnCommand="assignData" CssClass="findDoc" />
                    <asp:HyperLink Text="podgląd" runat="server" ID="showDocLink" Target="_blank" Visible="false" />
                <table class="grid fullWidth">
                    <tr>
                        <th>Data zarządzenia </th>
                        <th>w sprawie </th>
                        <th>Uwagi </th>
                        <th>&nbsp; </th>
                    </tr>
                    <tr class="expandedTextboxes">
                        <td>
                            <asp:TextBox ID="ItemDateTextBox" runat="server" Text='<%# Bind("ItemDate") %>' ValidationGroup="entryForm" />
                            <asp:CompareValidator ValidationGroup="entryForm" ID="v2" runat="server" ControlToValidate="ItemDateTextBox"
                                Type="Date" Operator="DataTypeCheck" Text="*" ErrorMessage="Format:  rrrr-mm-dd"
                                Display="Dynamic" />
                        </td>
                        <td>
                            <asp:TextBox ID="ItemSubjectTextBox" runat="server" Text='<%# Bind("ItemSubject") %>'
                                ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:TextBox ID="RemarksTextBox" runat="server" Text='<%# Bind("Remarks") %>' ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Insert"
                                Text="Zarejestruj" ValidationGroup="entryForm" />
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="True" CommandName="Cancel"
                                Text="Anuluj" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Literal runat="server" ID="insertError" Visible="false" Text="Nie udało się zapisać pozycji" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="entryForm"
                    EnableClientScript="true" />
            </fieldset>
        </InsertItemTemplate>
        <ItemTemplate>
            <asp:LinkButton runat="server" Text="Nowy wpis..." ID="lnkNewEntry" OnClick="lnkNewEntry_click" />
        </ItemTemplate>
        <EmptyDataTemplate>
            <asp:LinkButton runat="server" Text="Nowy wpis..." ID="lnkNewEntry" OnClick="lnkNewEntry_click" /></EmptyDataTemplate>
    </asp:FormView>
    <asp:FormView ID="formView3" runat="server" DataKeyNames="ID" DataSourceID="registryDS"
        EnableModelValidation="True" CssClass="fullWidth" OnItemInserting="formView_ItemInserting"
        OnItemInserted="formView_ItemInserted">
        <InsertItemTemplate>
            <fieldset><legend>Dodawanie pozycji Rejestru Umów</legend>
                <asp:Label ID="Label1" runat="server" AssociatedControlID="fDocumentID" Text="Nr sys. dok." />
                <asp:TextBox runat="server" ID="fDocumentID" />
                <asp:LinkButton runat="server" ID="lnkAssignDataFromDocument" Text="<-" CommandName="assignDataToInsert"
                    OnCommand="assignData" CssClass="findDoc" />
                        <asp:HyperLink Text="podgląd" runat="server" ID="showDocLink" Target="_blank" Visible="false" />
                <table class="grid fullWidth">
                    <tr>
                        <th>Data umowy </th>
                        <th>Strona umowy </th>
                        <th>Przedmiot umowy </th>
                        <th>Symbol
                            <br />
                            kom. org. </th>
                        <th>Uwagi </th>
                        <th>&nbsp; </th>
                    </tr>
                    <tr class="expandedTextboxes">
                        <td>
                            <asp:TextBox ID="ItemDateTextBox" runat="server" Text='<%# Bind("ItemDate") %>' ValidationGroup="entryForm" />
                            <asp:CompareValidator ValidationGroup="entryForm" ID="v2" runat="server" ControlToValidate="ItemDateTextBox"
                                Type="Date" Operator="DataTypeCheck" Text="*" ErrorMessage="Format:  rrrr-mm-dd"
                                Display="Dynamic" />
                        </td>
                        <td>
                            <asp:TextBox ID="ItemSideTextBox" runat="server" Text='<%# Bind("ItemSide") %>' ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:TextBox ID="ItemSubjectTextBox" runat="server" Text='<%# Bind("ItemSubject") %>'
                                ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:TextBox ID="ItemOrganizationalUnitTextBox" runat="server" Text='<%# Bind("OrganizationalUnit") %>'
                                ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:TextBox ID="RemarksTextBox" runat="server" Text='<%# Bind("Remarks") %>' ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Insert"
                                Text="Zarejestruj" ValidationGroup="entryForm" />
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="True" CommandName="Cancel"
                                Text="Anuluj" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Literal runat="server" ID="insertError" Visible="false" Text="Nie udało się zapisać pozycji" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="entryForm"
                    EnableClientScript="true" />
            </fieldset>
        </InsertItemTemplate>
        <ItemTemplate>
            <asp:LinkButton runat="server" Text="Nowy wpis..." ID="lnkNewEntry" OnClick="lnkNewEntry_click" />
        </ItemTemplate>
        <EmptyDataTemplate>
            <asp:LinkButton runat="server" Text="Nowy wpis..." ID="lnkNewEntry" OnClick="lnkNewEntry_click" /></EmptyDataTemplate>
    </asp:FormView>
    <asp:FormView ID="formView2" runat="server" DataKeyNames="ID" DataSourceID="registryDS"
        EnableModelValidation="True" CssClass="fullWidth" OnItemInserting="formView_ItemInserting"
        OnItemInserted="formView_ItemInserted">
        <InsertItemTemplate>
            <fieldset><legend>Dodawanie pozycji Rejestru Postanowień</legend>
                <asp:Label ID="Label1" runat="server" AssociatedControlID="fDocumentID" Text="Nr sys. dok." />
                <asp:TextBox runat="server" ID="fDocumentID" />
                <asp:LinkButton runat="server" ID="lnkAssignDataFromDocument" Text="<-" CommandName="assignDataToInsert"
                    OnCommand="assignData" CssClass="findDoc" />
                        <asp:HyperLink Text="podgląd" runat="server" ID="showDocLink" Target="_blank" Visible="false" />
                <table class="grid fullWidth">
                    <tr>
                        <th>Data postanowienia </th>
                        <th>Strona postanowienia </th>
                        <th>Znak sprawy </th>
                        <th>Dotyczy </th>
                        <th>Symbol
                            <br />
                            kom. org. </th>
                        <th>Uwagi </th>
                        <th>&nbsp; </th>
                    </tr>
                    <tr class="expandedTextboxes">
                        <td>
                            <asp:TextBox ID="ItemDateTextBox" runat="server" Text='<%# Bind("ItemDate") %>' ValidationGroup="entryForm" />
                            <asp:CompareValidator ValidationGroup="entryForm" ID="v2" runat="server" ControlToValidate="ItemDateTextBox"
                                Type="Date" Operator="DataTypeCheck" Text="*" ErrorMessage="Format:  rrrr-mm-dd"
                                Display="Dynamic" />
                        </td>
                        <td>
                            <asp:TextBox ID="ItemSideTextBox" runat="server" Text='<%# Bind("ItemSide") %>' ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:TextBox ID="ItemReferenceNumberTextBox" runat="server" Text='<%# Bind("ItemReferenceNumber") %>'
                                ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:TextBox ID="ItemSubjectTextBox" runat="server" Text='<%# Bind("ItemSubject") %>'
                                ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:TextBox ID="ItemOrganizationalUnitTextBox" runat="server" Text='<%# Bind("OrganizationalUnit") %>'
                                ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:TextBox ID="RemarksTextBox" runat="server" Text='<%# Bind("Remarks") %>' ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Insert"
                                Text="Zarejestruj" ValidationGroup="entryForm" />
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="True" CommandName="Cancel"
                                Text="Anuluj" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Literal runat="server" ID="insertError" Visible="false" Text="Nie udało się zapisać pozycji" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="entryForm"
                    EnableClientScript="true" />
            </fieldset>
        </InsertItemTemplate>
        <ItemTemplate>
            <asp:LinkButton runat="server" Text="Nowy wpis..." ID="lnkNewEntry" OnClick="lnkNewEntry_click" />
        </ItemTemplate>
        <EmptyDataTemplate>
            <asp:LinkButton runat="server" Text="Nowy wpis..." ID="lnkNewEntry" OnClick="lnkNewEntry_click" /></EmptyDataTemplate>
    </asp:FormView>
    <asp:FormView ID="formView1" runat="server" DataKeyNames="ID" DataSourceID="registryDS"
        EnableModelValidation="True" CssClass="fullWidth" OnItemInserting="formView_ItemInserting"
        OnItemInserted="formView_ItemInserted">
        <InsertItemTemplate>
            <fieldset><legend>Dodawanie pozycji Rejestru Decyzji</legend>
                <asp:Label ID="Label1" runat="server" AssociatedControlID="fDocumentID" Text="Nr sys. dok." />
                <asp:TextBox runat="server" ID="fDocumentID" />
                <asp:LinkButton runat="server" ID="lnkAssignDataFromDocument" Text="<-" CommandName="assignDataToInsert"
                    OnCommand="assignData" CssClass="findDoc" />
                        <asp:HyperLink Text="podgląd" runat="server" ID="showDocLink" Target="_blank" Visible="false" />
                <table class="grid fullWidth">
                    <tr>
                        <th>Data decyzji </th>
                        <th>Strona decyzji </th>
                        <th>Znak sprawy </th>
                        <th>Przedmiot decyzji </th>
                        <th>Symbol
                            <br />
                            kom. org. </th>
                        <th>Uwagi </th>
                        <th>&nbsp; </th>
                    </tr>
                    <tr class="expandedTextboxes">
                        <td>
                            <asp:TextBox ID="ItemDateTextBox" runat="server" Text='<%# Bind("ItemDate") %>' ValidationGroup="entryForm" />
                            <asp:CompareValidator ValidationGroup="entryForm" ID="v2" runat="server" ControlToValidate="ItemDateTextBox"
                                Type="Date" Operator="DataTypeCheck" Text="*" ErrorMessage="Format:  rrrr-mm-dd"
                                Display="Dynamic" />
                        </td>
                        <td>
                            <asp:TextBox ID="ItemSideTextBox" runat="server" Text='<%# Bind("ItemSide") %>' ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:TextBox ID="ItemReferenceNumberTextBox" runat="server" Text='<%# Bind("ItemReferenceNumber") %>'
                                ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:TextBox ID="ItemSubjectTextBox" runat="server" Text='<%# Bind("ItemSubject") %>'
                                ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:TextBox ID="ItemOrganizationalUnitTextBox" runat="server" Text='<%# Bind("OrganizationalUnit") %>'
                                ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:TextBox ID="RemarksTextBox" runat="server" Text='<%# Bind("Remarks") %>' ValidationGroup="entryForm" />
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Insert"
                                Text="Zarejestruj" ValidationGroup="entryForm" />
                            &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="True" CommandName="Cancel"
                                Text="Anuluj" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:Literal runat="server" ID="insertError" Visible="false" Text="Nie udało się zapisać pozycji" />
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="entryForm"
                    EnableClientScript="true" />
            </fieldset>
        </InsertItemTemplate>
        <ItemTemplate>
            <asp:LinkButton runat="server" Text="Nowy wpis..." ID="lnkNewEntry" OnClick="lnkNewEntry_click" />
        </ItemTemplate>
        <EmptyDataTemplate>
            <asp:LinkButton runat="server" Text="Nowy wpis..." ID="lnkNewEntry" OnClick="lnkNewEntry_click" /></EmptyDataTemplate>
    </asp:FormView>
    </asp:Panel>
    <fieldset><legend>Zawartość rejestru</legend>
        <asp:GridView ID="GridView4" runat="server" AllowPaging="True" CssClass="grid fullWidth expandedTextboxes"
            AllowSorting="false" AutoGenerateColumns="False" DataSourceID="registryDS" EnableModelValidation="True"
            DataKeyNames="ID" OnRowCommand="GridView1_RowCommand" Caption="Rejestr Zarządzeń"
            OnRowDataBound="bindSearchCriteria">
            <EmptyDataTemplate>Rejestr jest pusty lub nie zawiera wpisów spełniających kryteria
                filtrowania! &nbsp;
                <asp:LinkButton runat="server" Text="Wyczyść kryteria filtrowania" OnClick="resetFilters" />
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>Lp.<br />
                        <asp:TextBox runat="server" ID="filterPositionNumber" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("PositionNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("PositionNumber") %>'></asp:Label>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data zarządzenia" SortExpression="ItemDate">
                    <HeaderTemplate>Data zarządzenia<br />
                        <asp:TextBox runat="server" ID="filterDateFrom" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ItemDate", "{0:yyyy-MM-dd}") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("ItemDate", "{0:yyyy-MM-dd}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="" SortExpression="ItemSubject">
                    <HeaderTemplate>W sprawie<br />
                        <asp:TextBox runat="server" ID="filterSubject" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("ItemSubject") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("ItemSubject") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Uwagi" SortExpression="Remarks">
                    <HeaderTemplate>Uwagi<br />
                        <asp:TextBox runat="server" ID="filteRemarks" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Remarks") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Button runat="server" Text="Filtruj" OnClick="filterRegistry" />
                        <br />
                        <asp:Button ID="LinkButton4" runat="server" Text="Wyczyść" OnClick="resetFilters" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="DocumentID" runat="server" Style="width: 70%" Text='<%# Bind("DocumentID") %>' />
                        <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="assignDataToEdit"
                            CssClass="findDoc" Text="&lt;-"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("DocumentID","~/Dokumenty/SkladnikiDokumentu.aspx?id={0}") %>'
                            Target="_blank" Text='<%# Eval("DocumentID")==null?string.Empty:"dokument" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" EditText="Edytuj" UpdateText="Zapisz" CancelText="Anuluj" />
            </Columns>
        </asp:GridView>
        <asp:GridView ID="GridView3" runat="server" AllowPaging="True" CssClass="grid fullWidth expandedTextboxes"
            AllowSorting="false" AutoGenerateColumns="False" DataSourceID="registryDS" EnableModelValidation="True"
            DataKeyNames="ID" OnRowCommand="GridView1_RowCommand" Caption="Rejestr Umów"
            OnRowDataBound="bindSearchCriteria">
            <EmptyDataTemplate>Rejestr jest pusty lub nie zawiera wpisów spełniających kryteria
                filtrowania! &nbsp;
                <asp:LinkButton runat="server" Text="Wyczyść kryteria filtrowania" OnClick="resetFilters" />
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField >
                    <HeaderTemplate>Lp.<br />
                        <asp:TextBox runat="server" ID="filterPositionNumber" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("PositionNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("PositionNumber") %>'></asp:Label>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data umowy" SortExpression="ItemDate">
                    <HeaderTemplate>Data umowy<br />
                        <asp:TextBox runat="server" ID="filterDateFrom" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ItemDate", "{0:yyyy-MM-dd}") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("ItemDate", "{0:yyyy-MM-dd}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Strona umowy" SortExpression="ItemSide">
                    <HeaderTemplate>Strona umowy<br />
                        <asp:TextBox runat="server" ID="filterSide" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("ItemSide") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("ItemSide") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Przedmiot umowy" SortExpression="ItemSubject">
                    <HeaderTemplate>Przedmiot umowy<br />
                        <asp:TextBox runat="server" ID="filterSubject" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("ItemSubject") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("ItemSubject") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Symbol kom. org." SortExpression="OrganizationalUnit">
                    <HeaderTemplate>Symbol kom. org.<br />
                        <asp:TextBox runat="server" ID="filterOrganizationalUnit" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("OrganizationalUnit") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("OrganizationalUnit") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Uwagi" SortExpression="Remarks">
                    <HeaderTemplate>Uwagi<br />
                        <asp:TextBox runat="server" ID="filteRemarks" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Remarks") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Button runat="server" Text="Filtruj" OnClick="filterRegistry" />
                        <br />
                        <asp:Button ID="LinkButton4" runat="server" Text="Wyczyść" OnClick="resetFilters" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="DocumentID" runat="server" Style="width: 70%" Text='<%# Bind("DocumentID") %>' />
                        <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="assignDataToEdit"
                            CssClass="findDoc" Text="&lt;-"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("DocumentID","~/Dokumenty/SkladnikiDokumentu.aspx?id={0}") %>'
                            Target="_blank" Text='<%# Eval("DocumentID")==null?string.Empty:"dokument" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" EditText="Edytuj" UpdateText="Zapisz" CancelText="Anuluj" />
            </Columns>
        </asp:GridView>
        <asp:GridView ID="GridView2" runat="server" AllowPaging="True" CssClass="grid fullWidth expandedTextboxes"
            AllowSorting="false" AutoGenerateColumns="False" DataSourceID="registryDS" EnableModelValidation="True"
            DataKeyNames="ID" OnRowCommand="GridView1_RowCommand" Caption="Rejestr Postanowień"
            OnRowDataBound="bindSearchCriteria">
            <EmptyDataTemplate>Rejestr jest pusty lub nie zawiera wpisów spełniających kryteria
                filtrowania! &nbsp;
                <asp:LinkButton runat="server" Text="Wyczyść kryteria filtrowania" OnClick="resetFilters" />
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>Lp.<br />
                        <asp:TextBox runat="server" ID="filterPositionNumber" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("PositionNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("PositionNumber") %>'></asp:Label>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data postanowienia" SortExpression="ItemDate">
                    <HeaderTemplate>Data postanowienia<br />
                        <asp:TextBox runat="server" ID="filterDateFrom" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ItemDate", "{0:yyyy-MM-dd}") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("ItemDate", "{0:yyyy-MM-dd}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Strona postanowienia" SortExpression="ItemSide">
                    <HeaderTemplate>Strona postanowienia<br />
                        <asp:TextBox runat="server" ID="filterSide" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("ItemSide") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("ItemSide") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Znak sprawy" SortExpression="ItemReferenceNumber">
                    <HeaderTemplate>Znak sprawy<br />
                        <asp:TextBox runat="server" ID="filterReferenceNumber" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("ItemReferenceNumber") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("ItemReferenceNumber") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Dotyczy" SortExpression="ItemSubject">
                    <HeaderTemplate>Dotyczy<br />
                        <asp:TextBox runat="server" ID="filterSubject" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("ItemSubject") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("ItemSubject") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Symbol kom. org." SortExpression="OrganizationalUnit">
                    <HeaderTemplate>Symbol kom. org.<br />
                        <asp:TextBox runat="server" ID="filterOrganizationalUnit" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("OrganizationalUnit") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("OrganizationalUnit") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Uwagi" SortExpression="Remarks">
                    <HeaderTemplate>Uwagi<br />
                        <asp:TextBox runat="server" ID="filteRemarks" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("Remarks") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Button runat="server" Text="Filtruj" OnClick="filterRegistry" />
                        <br />
                        <asp:Button ID="LinkButton4" runat="server" Text="Wyczyść" OnClick="resetFilters" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="DocumentID" runat="server" Style="width: 70%" Text='<%# Bind("DocumentID") %>' />
                        <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="assignDataToEdit"
                            CssClass="findDoc" Text="&lt;-"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("DocumentID","~/Dokumenty/SkladnikiDokumentu.aspx?id={0}") %>'
                            Target="_blank" Text='<%# Eval("DocumentID")==null?string.Empty:"dokument" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" EditText="Edytuj" UpdateText="Zapisz" CancelText="Anuluj" />
            </Columns>
        </asp:GridView>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" CssClass="grid fullWidth expandedTextboxes"
            AllowSorting="false" AutoGenerateColumns="False" DataSourceID="registryDS" EnableModelValidation="True"
            DataKeyNames="ID" EmptyDataText="Rejestr nie zawiera wpisów!" OnRowCommand="GridView1_RowCommand"
            Caption="Rejestr Decyzji">
            <EmptyDataTemplate>Rejestr jest pusty lub nie zawiera wpisów spełniających kryteria
                filtrowania! &nbsp;
                <asp:LinkButton runat="server" Text="Wyczyść kryteria filtrowania" OnClick="resetFilters" />
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>Lp.<br />
                        <asp:TextBox runat="server" ID="filterPositionNumber" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("PositionNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("PositionNumber") %>'></asp:Label>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data decyzji" SortExpression="ItemDate">
                    <HeaderTemplate>Data decyzji<br />
                        <asp:TextBox runat="server" ID="filterDateFrom" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ItemDate", "{0:yyyy-MM-dd}") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("ItemDate", "{0:yyyy-MM-dd}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Strona decyzji" SortExpression="ItemSide">
                 <HeaderTemplate>Strona decyzji<br />
                        <asp:TextBox runat="server" ID="filterSide" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("ItemSide") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("ItemSide") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Znak sprawy" SortExpression="ItemReferenceNumber">
                    <HeaderTemplate>Znak sprawy<br />
                        <asp:TextBox runat="server" ID="filterReferenceNumber" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("ItemReferenceNumber") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("ItemReferenceNumber") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Przedmiot decyzji" SortExpression="ItemSubject">
                    <HeaderTemplate>Przedmiot decyzji<br />
                        <asp:TextBox runat="server" ID="filterSubject" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("ItemSubject") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("ItemSubject") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Symbol kom. org." SortExpression="OrganizationalUnit">
                    <HeaderTemplate>Symbol kom. org.<br />
                        <asp:TextBox runat="server" ID="filterOrganizationalUnit" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("OrganizationalUnit") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("OrganizationalUnit") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Uwagi" SortExpression="Remarks">
                    <HeaderTemplate>Uwagi<br />
                        <asp:TextBox runat="server" ID="filteRemarks" CssClass="RCFilter" />
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("Remarks") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("Remarks") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>Operacje</HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="DocumentID" runat="server" Style="width: 70%" Text='<%# Bind("DocumentID") %>' />
                        <asp:LinkButton runat="server" CausesValidation="False" CommandName="assignDataToEdit"
                            CssClass="findDoc" Text="&lt;-"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:HyperLink runat="server" NavigateUrl='<%# Eval("DocumentID","~/Dokumenty/SkladnikiDokumentu.aspx?id={0}") %>'
                            Target="_blank" Text='<%# Eval("DocumentID")==null?string.Empty:"dokument" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" EditText="Edytuj" UpdateText="Zapisz" CancelText="Anuluj" />
            </Columns>
        </asp:GridView>
    </fieldset>
    <asp:LinqDataSource ID="registryDS" runat="server" ContextTypeName="Pemi.Esoda.DataAccess.ESodaDataContext"
        TableName="CentralRegistries" EnableInsert="True" EnableUpdate="True" OnSelecting="registryDS_Selecting"></asp:LinqDataSource>
    <asp:LinqDataSource ID="registryTypesDS" runat="server" ContextTypeName="Pemi.Esoda.DataAccess.ESodaDataContext"
        OrderBy="RegistryName" Select="new (ID, RegistryName)" TableName="CentralRegistryTypes">
    </asp:LinqDataSource>
</asp:Content>
