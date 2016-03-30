<%@ Page MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" Language="C#" AutoEventWireup="true" CodeBehind="rodzajeDokumentow.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.rodzajeDokumentow" Title="Zarz¹dzanie kategoriami i rodzajami dokumentów" %>
<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
<div id="singleColumn">
<fieldset>
            <legend>Kategorie dokumentów</legend>
      <asp:Panel ID="pnlDocCatList" runat="server">
            
           <asp:Label ID="lblKategoria" runat="Server" associatedControlId="ddlDocCatList">Kategoria: </asp:Label>
            <asp:DropDownList ID="ddlDocCatList" runat="server" AutoPostBack="True" DataSourceID="dsDocCategories"
                            DataTextField="nazwa" DataValueField="id" 
               OnDataBound="ddlDocCatList_DataBound" 
               onselectedindexchanged="ddlDocCatList_SelectedIndexChanged" >
                        </asp:DropDownList><br />
                        <asp:LinkButton ID="lnkAddCat" runat="server" OnClick="btnAddCat_Click" Text="Dodaj" />
                        <asp:LinkButton ID="lnkEditCat" runat="server" OnClick="btnEditCat_Click" Text="Edytuj" /> 
                        <asp:SqlDataSource ID="dsDocCategories" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
                            SelectCommand="Dokumenty.listaKategorii" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="true" Name="wszystkie" Type="Boolean" />
                            </SelectParameters>
           </asp:SqlDataSource>
           
            </asp:Panel>           
 
        <asp:FormView ID="frmDocCategories" CssClass="grid" runat="server"  OnModeChanging="frmDocCategories_ModeChanging" OnItemInserting="frmDocCategories_ItemInserting" OnItemUpdating="frmDocCategories_ItemUpdating" Visible="False">        
            <ItemTemplate>            
            </ItemTemplate>
            <InsertItemTemplate>
            <table border="0" cellspacing="0" cellpadding="0">
            <thead><tr><th colspan="10">Dodaj kategoriê</th></tr></thead>
            <tbody>
            <tr><th><asp:Label ID="lblNazwa" runat="server" Text="Nazwa:" AssociatedControlID="txtNazwa" /></th><td><asp:TextBox ID="txtNazwa" MaxLength="30" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="rfvNazwa" runat="server" ControlToValidate="txtNazwa" ErrorMessage="*" Text="wymagane" ValidationGroup="AddDocCat"></asp:RequiredFieldValidator></td></tr>
            <tr><th><asp:Label ID="lblSkrot" runat="server" Text="Skrót:" AssociatedControlID="txtSkrot" /></th><td><asp:TextBox ID="txtSkrot" MaxLength="20" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="rfvSkrot" runat="server" ControlToValidate="txtSkrot" ErrorMessage="*" Text="wymagane" ValidationGroup="AddDocCat"></asp:RequiredFieldValidator></td></tr>
            <tr><td></td><td><asp:CheckBox ID="ckbActive" runat="server" Checked="true" Text="Aktywna" /></td></tr>
            <tr><td colspan="2" align="center"><asp:LinkButton ID="btnAdd" runat="server" Text="Dodaj" CommandName="Insert" ValidationGroup="AddDocCat"/><asp:LinkButton ID="btnCancel" runat="server" Text="Anuluj" CausesValidation="False" CommandName="Cancel" /></td></tr>
            </tbody>
            </table>
            </InsertItemTemplate>
            <EditItemTemplate>
                
            <table border="0" cellspacing="0" cellpadding="0">
            <thead><tr><th colspan="10">
                Aktualizuj kategoriê</th></tr></thead>
            <tbody>
            <tr><th><asp:Label ID="lblNazwa" runat="server" Text="Nazwa:" AssociatedControlID="txtNazwa" /></th><td><asp:TextBox ID="txtNazwa" runat="server" MaxLength="30" Text='<%# Bind("nazwa") %>'></asp:TextBox><asp:RequiredFieldValidator ID="rfvNazwa" runat="server" ControlToValidate="txtNazwa" ErrorMessage="*" Text="wymagane" ValidationGroup="EditDocCat"></asp:RequiredFieldValidator></td></tr>
            <tr><th><asp:Label ID="lblSkrot" runat="server" Text="Skrót:" AssociatedControlID="txtSkrot" /></th><td><asp:TextBox ID="txtSkrot" runat="server" MaxLength="20" Text='<%# Bind("skrot") %>'></asp:TextBox><asp:RequiredFieldValidator ID="rfvSkrot" runat="server" ControlToValidate="txtSkrot" ErrorMessage="*" Text="wymagane" ValidationGroup="EditDocCat"></asp:RequiredFieldValidator></td></tr>
            <tr><td></td><td><asp:CheckBox ID="ckbActive" runat="server" Checked='<%# Bind("aktywna") %>' Text="Aktywna" /></td></tr>
            <tr><td colspan="2" align="center"><asp:LinkButton ID="btnAdd" runat="server" Text="Aktualizuj" CommandName="Update" ValidationGroup="EditDocCat" /><asp:LinkButton ID="btnCancel" runat="server" Text="Anuluj" CausesValidation="False" CommandName="Cancel" /></td></tr>
            </tbody>
            </table>
            </EditItemTemplate>
            <EmptyDataTemplate>
                Brak aktualnie zdefiniowanych kategorii dokumentów.<br />
                <asp:LinkButton ID="btnAddDocCat" runat="server" CommandName="New" Text="Dodaj" />
            </EmptyDataTemplate>
        </asp:FormView> 
         </fieldset>   
        <br />
        <asp:Panel ID="pnlDocTypes" runat="server" Visible="false">
        <fieldset><legend>Typy dokumentów w kategorii: <asp:Label ID="lblCategoryName" runat="server"></asp:Label></legend>
        <br />
        <asp:GridView cssClass="grid" GridLines="None" ID="gvDocTypes" runat="server" AutoGenerateColumns="False" DataSourceID="dsDocTypes" DataKeyNames="id" OnSelectedIndexChanged="gvDocTypes_SelectedIndexChanged" AllowPaging="True" AllowSorting="True" PageSize="15">
            <Columns>
                <asp:BoundField DataField="nazwa" HeaderText="Nazwa" SortExpression="nazwa" />
                <asp:BoundField DataField="skrot" HeaderText="Skrót" SortExpression="skrot" />
                <asp:CommandField HeaderText="Operacje" SelectText="edytuj" ShowSelectButton="True" />
            </Columns>
            <EmptyDataTemplate>
                Brak zdefiniowanych typów w wybranej kategorii.
            </EmptyDataTemplate>
          <PagerSettings Mode="NextPreviousFirstLast" />
        </asp:GridView>
        <asp:SqlDataSource ID="dsDocTypes" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
            SelectCommand="[Dokumenty].[listaRodzajow]" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="ddlDocCatList" Name="idKategorii" PropertyName="SelectedValue" DefaultValue="-1" />
                <asp:Parameter DefaultValue="true" Name="wszystkie" Type="Boolean">
                </asp:Parameter>
            </SelectParameters>
        </asp:SqlDataSource>
        <br />
        <asp:LinkButton ID="lnkAddDocType" runat="server" OnClick="lnkAddDocType_Click">Dodaj typ dokumentu</asp:LinkButton><br />
        <asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
        <asp:Panel ID="panelDocTypes" runat="server">
        <asp:FormView ID="frmDocTypes" CssClass="grid" runat="server" 
                OnItemInserting="frmDocTypes_ItemInserting" 
                OnItemUpdating="frmDocTypes_ItemUpdating" 
                OnModeChanging="frmDocTypes_ModeChanging" 
                OnItemCommand="frmDocTypes_ItemCommand" onprerender="frmDocTypes_PreRender">        
        <EditItemTemplate>
        <%--<asp:ValidationSummary runat="server" ID="ValidationSummary" ValidationGroup="UpdateDocCat" />--%>
        <table>
        <tr><th colspan="3">Edycja typu dokumentu</th></tr>
        <tr>
        <th>
            <asp:Label ID="lblNazwa" runat="server" AssociatedControlID="txtNazwa" Text="Nazwa:"></asp:Label></th><td>
                <asp:TextBox ID="txtNazwa" runat="server" Text='<%# Bind("nazwa") %>' 
                    MaxLength="200"><span>&lt;%# Bind(&quot;nazwa&quot;) %&gt;</span><span>&lt;%# Bind(&quot;nazwa&quot;) %&gt;</span></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNazwa" runat="server" ControlToValidate="txtNazwa"
                ErrorMessage="*" Text="wymagane" ValidationGroup="UpdateDocCat" Display="Dynamic"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
        <th>
            <asp:Label ID="lblSkrot" runat="server" AssociatedControlID="txtSkrot" Text="Skrót:"></asp:Label></th><td>
                <asp:TextBox ID="txtSkrot" runat="server" Text='<%# Bind("skrot") %>' 
                    MaxLength="20"><span>&lt;%# Bind(&quot;skrot&quot;) %&gt;</span><span>&lt;%# Bind(&quot;skrot&quot;) %&gt;</span></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvSkrot" runat="server" ControlToValidate="txtSkrot"
                ErrorMessage="*" Text="wymagane" ValidationGroup="UpdateDocCat" Display="Dynamic"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
        <th><asp:Label ID="Label2" runat="server" Text="Opis:" AssociatedControlID="tbDescription" /></th>
        <td><asp:TextBox ID="tbDescription" runat="server" Rows="3" Columns="30" TextMode="MultiLine" Text='<%#Bind("opisCF") %>'  /></td>
        </tr>
        <tr>
        <th><asp:Label ID="Label4" runat="server" Text="Zawiwera akty prawne" AssociatedControlID="cbAktPrawny" /></th>
        <td><asp:CheckBox ID="cbAktPrawny" runat="server" checked='<%#Bind("aktPrawny") %>'  /></td>
        </tr>
        <tr><th colspan="2">Formularz eSoda</th></tr>
        <tr>        
        <th><asp:Label ID="Label1" runat="server" AssociatedControlID="uploadControl" Text="Plik (dll):" /></th>
        <td><asp:FileUpload runat="server" ID="uploadControl"  /><br /><asp:Label ForeColor="Green" Visible='<%# Convert.ToInt32(Eval("jestJuzDLL"))==1?true:false %>' runat="server" ID="lblOryginalName" Text='<%# Bind("oryginalnaNazwaCF") %>' /></td>
        </tr>                        
        <tr>
        <th><asp:Label runat="server" Text="Aktywny:" AssociatedControlID="cbIsActive" /></th>
        <td><asp:CheckBox runat="server" ID="cbIsActive" Checked='<%# Bind("aktywnyCF") %>' /></td>
        </tr>        
        <tr><th colspan="2">Formularz MS Word</th></tr>
        <tr>
        <th><asp:Label ID="Label5" runat="server" AssociatedControlID="wordForm" Text="Plik w formacie Word:" /></th>
        <td>
            <asp:FileUpload runat="server" ID="wordForm"  /><br /><asp:Label ForeColor="Green" Visible='<%# Convert.ToInt32(Eval("jestJuzWord"))==1?true:false %>' runat="server" ID="lblWordOriginal" Text='<%# Bind("oryginalnaNazwaWord") %>' />
            <asp:CustomValidator runat="server" ID="wordFormValidator" 
                ControlToValidate="wordForm" ErrorMessage="Wymagana jest równie¿ schema!" 
                ValidationGroup="UpdateDocCat" 
                onservervalidate="wordValidator_ServerValidate"  ></asp:CustomValidator>    
        </td>
        </tr> 
        <tr>
        <th><asp:Label ID="Label8" runat="server" AssociatedControlID="wordSchema" Text="Schema formularza:" /></th>
        <td>
            <asp:FileUpload runat="server" ID="wordSchema"  /><br /><asp:Label ForeColor="Green" Visible='<%# Convert.ToInt32(Eval("jestJuzWordSchema"))==1?true:false %>' runat="server" ID="lblWordSchemaOriginal" Text='<%# Bind("oryginalnaNazwaWordSchema") %>' />
            <asp:CustomValidator runat="server" ID="wordSchemaValidator" 
                ControlToValidate="wordSchema" ErrorMessage="Wymagany jest równie¿ Word!" 
                ValidationGroup="UpdateDocCat" 
                onservervalidate="wordValidator_ServerValidate"  ></asp:CustomValidator>
        </td>
        </tr> 
        <tr>
        <th><asp:Label ID="Label7" runat="server" Text="Aktywny:" AssociatedControlID="wordFormActive" /></th>
        <td><asp:CheckBox runat="server" ID="wordFormActive" Checked='<%# Bind("aktywnyWord") %>' /></td>
        </tr>
        <tr><td><asp:LinkButton ID="btnSave" runat="server" CommandName="Update" Text="Zapisz" ValidationGroup="UpdateDocCat" /><asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Anuluj" CausesValidation="False" /></td></tr>
        </table>
        </EditItemTemplate>
        <InsertItemTemplate>
        <%--<asp:ValidationSummary runat="server" ID="ValidationSummary" ValidationGroup="UpdateDocCat" />--%>
        <table>
        <tr><th colspan="3">Dodawanie typu dokumentu</th></tr>
        <tr>
        <th>
            <asp:Label ID="lblNazwa" runat="server" AssociatedControlID="txtNazwa" Text="Nazwa:"></asp:Label></th><td>
                <asp:TextBox ID="txtNazwa" runat="server" MaxLength="30"></asp:TextBox><asp:RequiredFieldValidator
                ID="rfvNazwa" runat="server" ErrorMessage="*" ControlToValidate="txtNazwa" Text="wymagane" ValidationGroup="InsertDocType" Display="Dynamic"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
        <th>
            <asp:Label ID="lblSkrot" runat="server" AssociatedControlID="txtSkrot" Text="Skrót:"></asp:Label></th><td>
                <asp:TextBox ID="txtSkrot" runat="server" MaxLength="20"></asp:TextBox><asp:RequiredFieldValidator
                ID="rfvSkrot" runat="server" ErrorMessage="*" ControlToValidate="txtSkrot" Text="wymagane" ValidationGroup="InsertDocType" Display="Dynamic"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
        <th><asp:Label ID="Label2" runat="server" Text="Opis:" AssociatedControlID="tbDescription" /></th>
        <td><asp:TextBox ID="tbDescription" runat="server" Rows="3" Columns="30" TextMode="MultiLine" Text='<%#Bind("opisCF") %>'  /></td>
        </tr>
         <tr>
        <th><asp:Label ID="Label4" runat="server" Text="Zawiwera akty prawne" AssociatedControlID="cbAktPrawny" /></th>
        <td><asp:CheckBox ID="cbAktPrawny" runat="server" checked='<%#Bind("aktPrawny") %>'  /></td>
        </tr>
        <tr><th colspan="2">Formularz eSoda</th></tr>
        <tr>        
        <th><asp:Label ID="Label1" runat="server" AssociatedControlID="uploadControl" Text="Plik (dll):" /></th>
        <td>
            <asp:FileUpload runat="server" ID="uploadControl"  /><br /><asp:Label ForeColor="Green" Visible='<%# Convert.ToInt32(Eval("jestJuzDLL"))==1?true:false %>' runat="server" ID="lblOryginalName" Text='<%# Bind("oryginalnaNazwaCF") %>' />            
        </td>
        </tr>                        
        <tr>
        <th><asp:Label ID="Label3" runat="server" Text="Aktywny:" AssociatedControlID="cbIsActive" /></th>
        <td><asp:CheckBox runat="server" ID="cbIsActive" Checked='<%# Bind("aktywnyCF") %>' /></td>
        </tr>        
        <tr><th colspan="2">Formularz MS Word</th></tr>
        <tr>
        <th><asp:Label ID="Label5" runat="server" AssociatedControlID="wordForm" Text="Plik w formacie Word:" /></th>
        <td>
            <asp:FileUpload runat="server" ID="wordForm"  /><br /><asp:Label ForeColor="Green" Visible='<%# Convert.ToInt32(Eval("jestJuzWord"))==1?true:false %>' runat="server" ID="lblWordOriginal" Text='<%# Bind("oryginalnaNazwaWord") %>' />
            <asp:CustomValidator runat="server" ID="wordFormValidator" 
                ControlToValidate="wordForm" ErrorMessage="Wymagana jest równie¿ schema!" 
                ValidationGroup="UpdateDocCat" 
                onservervalidate="wordValidator_ServerValidate"  ></asp:CustomValidator>
        </td>
        </tr> 
        <tr>
        <th><asp:Label ID="Label8" runat="server" AssociatedControlID="wordSchema" Text="Schema formularza:" /></th>
        <td>
            <asp:FileUpload runat="server" ID="wordSchema"  /><br /><asp:Label ForeColor="Green" Visible='<%# Convert.ToInt32(Eval("jestJuzWordSchema"))==1?true:false %>' runat="server" ID="lblWordSchemaOriginal" Text='<%# Bind("oryginalnaNazwaWordSchema") %>' />
            <asp:CustomValidator runat="server" ID="wordSchemaValidator" 
                ControlToValidate="wordSchema" ErrorMessage="Wymagany jest równie¿ Word!" 
                ValidationGroup="UpdateDocCat" 
                onservervalidate="wordValidator_ServerValidate"  ></asp:CustomValidator>
        </td>
        </tr> 
        <tr>
        <th><asp:Label ID="Label7" runat="server" Text="Aktywny:" AssociatedControlID="wordFormActive" /></th>
        <td><asp:CheckBox runat="server" ID="wordFormActive" Checked='<%# Bind("aktywnyWord") %>' /></td>
        </tr>           
        <tr><td><asp:LinkButton ID="btnSave" runat="server" CommandName="Insert" Text="Dodaj" ValidationGroup="InsertDocType" /><asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Anuluj" CausesValidation="False" /></td></tr>
        </table>
        </InsertItemTemplate>
        </asp:FormView>
        </asp:Panel>        
        <asp:Panel runat="server" ID="panelOverwrite" Visible="false">
            <asp:Label runat="server" ID="lblOverwriteMessage" ForeColor="Red" />
            <asp:Button runat="server" ID="btnOverwrite" Text="Tak" 
                onclick="btnOverwrite_Click" />
            <asp:Button runat="server" ID="btnOverwriteCancel" Text="Nie" 
                onclick="btnOverwriteCancel_Click" />
        </asp:Panel>        
        </fieldset>
          </asp:Panel>
        </div>    
        
      </asp:Content>