<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="edycjaUzytkownikow.aspx.cs" Inherits="Pemi.Esoda.Web.UI.edycjaUzytkownikow" Title="Zarz¹dzanie pracownikami" %>
<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/EdycjaPINu.ascx" TagName="EdycjaPINu" TagPrefix="uc1" %>
<%@ Register src="../../Controls/DefinicjaZastepstw.ascx" tagname="DefinicjaZastepstw" tagprefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
<div id="singleColumn">
        <asp:GridView GridLines="None" CssClass="fullWidth grid" ID="gvUsersList" runat="server" AllowPaging="True" AutoGenerateColumns="False"  DataSourceID="dsUsersList" OnRowCommand="gvUsersList_RowCommand" DataKeyNames="idTozsamosci" PageSize="20" AllowSorting="True">
            <PagerSettings Mode="NextPreviousFirstLast" />
            <AlternatingRowStyle CssClass="pozycjaNieparzysta" />
            <Columns>
                <asp:TemplateField HeaderText="Nazwisko" SortExpression="nazwisko"><ItemTemplate><%# Eval("nazwisko") %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Imiê" SortExpression="imie"><ItemTemplate><%# Eval("imie") %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Login" SortExpression="UserName"><ItemTemplate><%# Eval("UserName") %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Wydzia³" SortExpression="skrot"><ItemTemplate><asp:Label ID="lblWydzial" runat="server" Text='<%# Eval("skrot") %>' ToolTip='<%# Eval("nazwa") %>'></asp:Label></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Zastêpstwa">
                <ItemTemplate><%#(bool)Eval("jestDublerem")==true?'+' : '-' %></ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField HeaderText="Operacje" SelectText="edytuj" ShowSelectButton="True" />
            </Columns>
            <PagerTemplate>
            <table border="0" cellspacing="0" cellpadding="0" width="100%">
            <tr align="center">
            <td><asp:LinkButton ID="lnkFirst" runat="server" CommandName="Page" CommandArgument="First">&lt;&lt;</asp:LinkButton></td>
            <td><asp:LinkButton ID="lnkPrev" runat="server" CommandName="Page" CommandArgument="Prev">&lt;</asp:LinkButton></td>
            <td>strona: <asp:DropDownList ID="ddlPageSelector" runat="server" AutoPostBack="True" 
                    ondatabound="ddlPageSelector_DataBound" 
                    onselectedindexchanged="ddlPageSelector_SelectedIndexChanged" /> z <%=gvUsersList.PageCount %></td>
            <td><asp:LinkButton ID="lnkNext" runat="server" CommandName="Page" CommandArgument="Next">&gt;</asp:LinkButton></td>
            <td><asp:LinkButton ID="lnkLast" runat="server" CommandName="Page" CommandArgument="Last">&gt;&gt;</asp:LinkButton></td>
            </tr></table>
            </PagerTemplate>
        </asp:GridView>
        <asp:LinkButton ID="lnkAddUser" runat="server" OnClick="lnkAddUser_Click">Dodaj pracownika</asp:LinkButton><br />
        <asp:SqlDataSource ID="dsUsersList" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
            SelectCommand="Uzytkownicy.listaPracownikow" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
         
         <asp:FormView ID="frmUser" runat="server" DefaultMode="Insert" Height="124px" Width="290px" DataSourceID="dsUser" OnItemCommand="frmUser_ItemCommand" Visible="False">
           <InsertItemTemplate>
           <fieldset>
           <legend>Dodanie nowego pracownika</legend>
           <table>
                <tr>
                <td align="center">                    
                    <table>
                    <tr><td align="left">
                        <asp:Label ID="lblNazwisko" runat="server" AssociatedControlID="txtNazwisko" Text="Nazwisko:"></asp:Label><asp:RequiredFieldValidator ID="rfvNazwisko" runat="server"
                            ErrorMessage="*" ControlToValidate="txtNazwisko" Display="Dynamic" ValidationGroup="AddingUser">wymagane</asp:RequiredFieldValidator><br />
                        <asp:TextBox ID="txtNazwisko" runat="server" Width="200px" 
                            Text='<%# Bind("nazwisko") %>' MaxLength="100" ></asp:TextBox></td>
                    <td align="left">
                        <asp:Label ID="lblImie" runat="server" AssociatedControlID="txtImie" Text="Imiê:"></asp:Label><asp:RequiredFieldValidator ID="rfvImie" runat="server" ControlToValidate="txtImie"
                            Display="Dynamic" ErrorMessage="*" ValidationGroup="AddingUser">wymagane</asp:RequiredFieldValidator><br />
                        <asp:TextBox ID="txtImie" runat="server" Width="100px" 
                            Text='<%# Bind("imie") %>' MaxLength="50"></asp:TextBox></td></tr>
                    <tr><td colspan="2" align="left">
                        <asp:Label ID="lblWydzial" runat="server" AssociatedControlID="ddlWydzial" Text="Wydzia³:"></asp:Label><asp:RequiredFieldValidator ID="rfvWydzial" runat="server" ControlToValidate="ddlWydzial"
                            Display="Dynamic" ErrorMessage="*" ValidationGroup="AddingUser" InitialValue="0">wymagane</asp:RequiredFieldValidator><br /><asp:DropDownList ID="ddlWydzial" runat="server" DataSourceID="dsWydzial" DataTextField="nazwa" DataValueField="id" Width="312px" SelectedValue='<%# Bind("id") %>' OnDataBound="ddlWydzial_DataBound"></asp:DropDownList><br />
                        <asp:SqlDataSource ID="dsWydzial" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
                            SelectCommand="select id, nazwa from Uzytkownicy.Grupa  where jednostkaOrganizacyjna=1 order by nazwa asc"></asp:SqlDataSource>
                    </td></tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="E-mail:"></asp:Label><asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                    Display="Dynamic" ErrorMessage="*" ValidationGroup="AddingUser">wymagane</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                    Display="Dynamic" ErrorMessage="*" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="AddingUser">b³êdny adres email</asp:RegularExpressionValidator><br />
                                <asp:TextBox ID="txtEmail" runat="server" Width="300px" MaxLength="256"></asp:TextBox></td>
                        </tr>
                          <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="Label1" runat="server" AssociatedControlID="txtStanowisko" Text="Stanowisko:"></asp:Label><br />
                                <asp:TextBox ID="txtStanowisko" runat="server" Width="300px" MaxLength="100"></asp:TextBox></td>
                        </tr>
                         <tr>
                            <td colspan="2" align="left">
                            <asp:CheckBox runat="server" ID="chChatAvailable" Text="aktywny komunikator" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                            <asp:CheckBox runat="server" ID="chManager" Text="wgl¹d do historii dekretacji" />
                            </td>
                        </tr>
                        
                    </table>                
                    <asp:LinkButton ID="btnZapisz" runat="server" Text="Zapisz" CommandName="InsertUser" CommandArgument='<%# Eval("idTozsamosci") %>' ValidationGroup="AddingUser" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Anuluj" CommandName="UserCancel" CausesValidation="False" />
                    </td>
                <td align="center">  
                <table>
                <tr><td align="left">
                    <asp:Label ID="lblLogin" runat="server" AssociatedControlID="txtLogin" Text="Login:"></asp:Label><asp:RequiredFieldValidator ID="rfvLogin" runat="server" ControlToValidate="txtLogin"
                        Display="Dynamic" ErrorMessage="*" ValidationGroup="AddingUser">wymagane</asp:RequiredFieldValidator><br />
                    <asp:TextBox ID="txtLogin" runat="server" Width="150px" 
                        Text='<%# Eval("UserName") %>' MaxLength="128"></asp:TextBox><br />
                    <asp:CheckBox ID="ckbActive" runat="server" Text="(aktywny)" TextAlign="Left" Checked='<%# Bind("aktywny") %>' /><br />
                    <asp:CheckBox ID="ckbLocked" runat="server" Text="(zablokowany)" Visible="false" TextAlign="Left" Checked='<%# Bind("zablokowany") %>' />
                    </td></tr>
                 <tr><td align="left">
                     <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="Has³o:"></asp:Label><asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                         Display="Dynamic" ErrorMessage="*" ValidationGroup="AddingUser">wymagane</asp:RequiredFieldValidator><asp:CustomValidator
                             ID="cvPassword" runat="server" ControlToValidate="txtPassword" Display="Dynamic"
                             ErrorMessage="*" OnServerValidate="cvPassword_ServerValidate" ValidationGroup="AddingUser" EnableClientScript="False"></asp:CustomValidator><br />
                     <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="150px" 
                         MaxLength="128"></asp:TextBox></td></tr>
                <tr><td align="left">
                    <asp:Label ID="lblPassword2" runat="server" AssociatedControlID="txtPassword2" Text="Powtórz has³o:"></asp:Label><asp:CompareValidator ID="cvPasswords" runat="server" ControlToCompare="txtPassword"
                        ControlToValidate="txtPassword2" Display="Dynamic" ErrorMessage="*" ValidationGroup="AddingUser">has³a ró¿ni¹ siê</asp:CompareValidator>
                    <asp:RequiredFieldValidator ID="rfvPassword2" runat="server" ControlToValidate="txtPassword2"
                        Display="Dynamic" ErrorMessage="*" ValidationGroup="AddingUser">wymagane</asp:RequiredFieldValidator><br />
                    <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" Width="150px" 
                        MaxLength="128"></asp:TextBox></td></tr>
                </table>             
                    <asp:LinkButton ID="btnZapiszHaslo" runat="server" Text="Zapisz" CommandArgument='<%# Eval("idTozsamosci") %>' CommandName="UpdatePassword" Visible="False" />                
                </td>
                <td>
                <fieldset>
           <legend>DokumentoMat</legend>
           <uc1:EdycjaPINu ID="pinEdit" runat="server" />           
           </fieldset>
                </td>
                </tr>
                </table>
           </fieldset>
           
           
           </InsertItemTemplate>
           
            <EditItemTemplate>
           <fieldset>
             <legend>Edycja pracownika</legend>
           <table>
                <tr>
                <td align="center">                    
                    <table>
                    <tr><td align="left">
                        <asp:Label ID="lblNazwisko" runat="server" AssociatedControlID="txtNazwisko" Text="Nazwisko:"></asp:Label><asp:RequiredFieldValidator ID="rfvNazwisko" runat="server" ControlToValidate="txtNazwisko"
                            Display="Dynamic" ErrorMessage="*" ValidationGroup="UpdatingUser">wymagane</asp:RequiredFieldValidator><br /><asp:TextBox ID="txtNazwisko" runat="server" Width="200px" Text='<%# Bind("nazwisko") %>' ></asp:TextBox></td>
                    <td align="left">
                        <asp:Label ID="lblImie" runat="server" AssociatedControlID="txtImie" Text="Imiê:"></asp:Label><asp:RequiredFieldValidator ID="rfvImie" runat="server" ControlToValidate="txtImie"
                            Display="Dynamic" ErrorMessage="*" ValidationGroup="UpdatingUser">wymagane</asp:RequiredFieldValidator><br /><asp:TextBox ID="txtImie" runat="server" Width="100px" Text='<%# Bind("imie") %>'></asp:TextBox></td></tr>
                    <tr><td colspan="2" align="left">
                        <asp:Label ID="lblWydzial" runat="server" AssociatedControlID="ddlWydzial" Text="Wydzia³:"></asp:Label><asp:RequiredFieldValidator ID="rfvWydzial" runat="server" ControlToValidate="ddlWydzial"
                            Display="Dynamic" ErrorMessage="*" InitialValue="0" ValidationGroup="UpdatingUser">wymagane</asp:RequiredFieldValidator><br /><asp:DropDownList ID="ddlWydzial" runat="server" DataSourceID="dsWydzial" DataTextField="nazwa" DataValueField="id" Width="312px" SelectedValue='<%# Bind("id") %>'></asp:DropDownList><br />
                        <asp:SqlDataSource ID="dsWydzial" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
                            SelectCommand="select id, nazwa from Uzytkownicy.Grupa where jednostkaOrganizacyjna=1 order by nazwa asc"></asp:SqlDataSource>
                        <asp:HiddenField ID="hfOldGroup" runat="server" Value='<%# Bind("nazwa") %>' />
                    </td></tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="E-mail:"></asp:Label><asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                    Display="Dynamic" ErrorMessage="*" ValidationGroup="UpdatingUser">wymagane</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                    Display="Dynamic" ErrorMessage="*" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ValidationGroup="UpdatingUser">b³êdny adres email</asp:RegularExpressionValidator><br />
                                <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("email") %>' Width="300px"></asp:TextBox></td>
                        </tr>
                         <tr>
                            <td align="left" colspan="2">
                                <asp:Label ID="Label1" runat="server" AssociatedControlID="txtStanowisko" Text="Stanowisko:"></asp:Label><br />
                                <asp:TextBox ID="txtStanowisko" runat="server" Width="300px" MaxLength="100" Text='<%# Bind("stanowisko") %>' ></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                            <asp:CheckBox runat="server" ID="chChatAvailable" Text="aktywny komunikator" Checked='<%#(bool)Eval("chatAvailable")?true:false %>' />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                            <asp:CheckBox runat="server" ID="chManager" Text="wgl¹d do historii dekretacji" Checked='<%#(bool)Eval("kierownik")?true:false %>'/>
                            </td>
                        </tr>
                    </table>                
                    <asp:LinkButton ID="btnZapisz" runat="server" Text="Zapisz" CommandName="UpdateUser" CommandArgument='<%# Eval("idTozsamosci") %>' ValidationGroup="UpdatingUser" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Anuluj" CommandName="UserCancel" /></td>
                <td align="center">  
                <table>
                <tr><td align="left">
                    <asp:Label ID="lblLogin" runat="server" AssociatedControlID="txtLogin" Text="Login:"></asp:Label><br />
                    <asp:TextBox ID="txtLogin" ReadOnly="true" runat="server" Width="150px" Text='<%# Eval("UserName") %>'></asp:TextBox><br />
                    <asp:CheckBox ID="ckbActive" runat="server" Text="(aktywny)" TextAlign="Left" Checked='<%# Bind("aktywny") %>' /><br />
                    <asp:CheckBox ID="ckbLocked" runat="server" Text="(zablokowany)" TextAlign="Left" Checked='<%# Bind("zablokowany") %>' />
                    </td></tr>
                 <tr><td align="left">
                     <asp:Label ID="lblNewPassword" runat="server" AssociatedControlID="txtNewPassword"
                         Text="Nowe has³o:"></asp:Label><asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword"
                         Display="Dynamic" ErrorMessage="*" ValidationGroup="UpdatePassword">wymagane</asp:RequiredFieldValidator>
                     <asp:CustomValidator ID="cvNewPassword" runat="server" ControlToValidate="txtNewPassword"
                         Display="Dynamic" EnableClientScript="False" ErrorMessage="*" OnServerValidate="cvPassword_ServerValidate"
                         ValidationGroup="UpdatePassword"></asp:CustomValidator><br /><asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" Width="150px"></asp:TextBox></td></tr>
                <tr><td align="left">
                    <asp:Label ID="lblNewPassword2" runat="server" AssociatedControlID="txtNewPassword2"
                        Text="Powtórz has³o:"></asp:Label><asp:RequiredFieldValidator ID="rfvNewPassword2" runat="server" ControlToValidate="txtNewPassword2"
                        Display="Dynamic" ErrorMessage="*" ValidationGroup="UpdatePassword">wymagane</asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cpvNewPassword2" runat="server" ControlToCompare="txtNewPassword"
                        ControlToValidate="txtNewPassword2" Display="Dynamic" ErrorMessage="*" ValidationGroup="UpdatePassword">has³a ró¿ni¹ siê</asp:CompareValidator><br /><asp:TextBox ID="txtNewPassword2" runat="server" TextMode="Password" Width="150px"></asp:TextBox></td></tr>
                </table>             
                    <asp:LinkButton ID="btnZapiszHaslo" runat="server" Text="Zapisz" CommandArgument='<%# Eval("idTozsamosci") %>' CommandName="UpdatePassword" ValidationGroup="UpdatePassword" />                
                </td>
                <td>
                 <fieldset>
           <legend>DokumentoMat</legend>
           <uc1:EdycjaPINu ID="pinEdit" runat="server" />           
           </fieldset>
                
                </td>
                </tr>
                </table>
               </fieldset>                
            </EditItemTemplate>
        </asp:FormView>
    
        <asp:SqlDataSource ID="dsUser" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
            SelectCommand="Uzytkownicy.pobierzPracownika" SelectCommandType="StoredProcedure" UpdateCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="gvUsersList" Name="idToz" PropertyName="SelectedValue"
                    Type="Int64"/>
            </SelectParameters>
        </asp:SqlDataSource>
        
        <uc2:DefinicjaZastepstw ID="definicjaZastepstw" runat="server" Visible="false" />
        
    </div>
</asp:Content>