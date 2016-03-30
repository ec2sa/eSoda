<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="edycjaGrup.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.edycjaGrup" Title="Zarz¹dzanie grupami (jednostkami organizacyjnymi)" %>
<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
    <asp:DropDownList ID="ddlGroupSelector" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlGroupSelector_SelectedIndexChanged">
        <asp:ListItem Selected="True" Value="True">Wydzia³y</asp:ListItem>
        <asp:ListItem Value="False">Role</asp:ListItem>
    </asp:DropDownList>
<asp:GridView GridLines="None" CssClass="fullWidth grid" ID="gvGroupsList" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="dsGroupsList" OnSelectedIndexChanged="gvGroupsList_SelectedIndexChanged" AllowSorting="True" AllowPaging="True" PageSize="20" OnRowDataBound="gvGroupsList_RowDataBound">
    <AlternatingRowStyle CssClass="pozycjaNieparzysta" />
    <PagerSettings Mode="NextPreviousFirstLast" />
    <Columns>
    <asp:BoundField HeaderText="Nazwa" DataField="nazwa" />
    <asp:BoundField HeaderText="Skr&#243;t" DataField="skrot" />
        <asp:CommandField HeaderText="Operacje" SelectText="edytuj" ShowSelectButton="True" />
    </Columns>
    
        </asp:GridView>
        <asp:SqlDataSource ID="dsGroupsList" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
            SelectCommand="[Uzytkownicy].[listaGrup]" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:Parameter DefaultValue="-1" Name="id" Type="Int32" />
                <asp:ControlParameter ControlID="ddlGroupSelector" DefaultValue="" Name="wydzial"
                    PropertyName="SelectedValue" Type="Boolean" />
            </SelectParameters>
        </asp:SqlDataSource>        
  
        <asp:LinkButton ID="lnkAddGroup" runat="server" OnClick="lnkAddGroup_Click">Dodaj wydzia³</asp:LinkButton>
    <asp:LinkButton ID="lnkAddRole" runat="server" OnClick="lnkAddRole_Click" Visible="False">Dodaj rolê</asp:LinkButton>
    <br />
        
        <asp:FormView CssClass="fullWidth" ID="frmGroup" runat="server" OnItemInserting="frmGroup_ItemInserting" OnItemUpdating="frmGroup_ItemUpdating"  OnItemCommand="frmGroup_ItemCommand" OnModeChanging="frmGroup_ModeChanging" DataKeyNames="id">
            <EditItemTemplate>
            <fieldset ><legend>Edycja wydzia³u</legend>
                <table class="grid fullWidth">
                    <tr>
                        <th>
                            <asp:Label ID="lblNazwa" runat="server" Text="Nazwa:" AssociatedControlID="txtNazwa" /></th>
                        <td>
                            <asp:TextBox CssClass="longTextBox" ID="txtNazwa" runat="server" 
                                Text='<%# Bind("nazwa") %>' MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator
                                ID="rfvNazwa" runat="server" ControlToValidate="txtNazwa" ErrorMessage="*" Text="wymagane" ValidationGroup="UpdateGroup" Display="Dynamic"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblSkrot" runat="server" AssociatedControlID="txtSkrot" Text="Skrót:"></asp:Label></th>
                        <td>
                            <asp:TextBox ID="txtSkrot" runat="server" Text='<%# Bind("skrot") %>' 
                                MaxLength="10"></asp:TextBox><asp:RequiredFieldValidator
                                ID="rfvSkrot" runat="server" ControlToValidate="txtSkrot" ErrorMessage="*" Text="wymagane" ValidationGroup="UpdateGroup" Display="Dynamic"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblParentGroup" runat="server" Text="Grupa nadrzêdna:" AssociatedControlID="ddlParentGroup"></asp:Label></th>
                        <td>
                            <asp:DropDownList ID="ddlParentGroup" runat="server" DataSourceID="dsParentGroupList"
                                DataTextField="nazwa" DataValueField="id" SelectedValue='<%# Bind("idRodzica") %>'>
                            </asp:DropDownList><br />
                            <asp:SqlDataSource ID="dsParentGroupList" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
                                SelectCommand="[Uzytkownicy].[listaGrup]" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="gvGroupsList" Name="id" PropertyName="SelectedValue"
                                        Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:HiddenField ID="hfWydzial" runat="server" Value='<%# Bind("jednostkaOrganizacyjna") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton ID="btnSave" runat="server" Text="Zapisz" CommandName="Update" ValidationGroup="UpdateGroup" /><asp:LinkButton
                                ID="btnCancel" runat="server" CausesValidation="false"
                                Text="Anuluj" CommandName="Cancel" /></td>
                    </tr>
                </table>
                </fieldset>
            </EditItemTemplate>
            <InsertItemTemplate>
            <fieldset>
            <legend>Dodawanie nowego wydzia³u</legend>
                <table class="grid fullWidth">
                    <tr>
                        <th>
                            <asp:Label ID="lblNazwa" runat="server" AssociatedControlID="txtNazwa" Text="Nazwa:"></asp:Label></th>
                        <td>
                            <asp:TextBox CssClass="longTextBox" ID="txtNazwa" runat="server" 
                                MaxLength="100" ></asp:TextBox><asp:RequiredFieldValidator
                                ID="rfvNazwa" runat="server" ControlToValidate="txtNazwa" ErrorMessage="*" Text="wymagane" ValidationGroup="InsertGroup" Display="Dynamic"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblSkrot" runat="server" AssociatedControlID="txtSkrot" Text="Skrót:"></asp:Label></th>
                        <td>
                        
                            <asp:TextBox ID="txtSkrot" runat="server" MaxLength="10"></asp:TextBox><asp:RequiredFieldValidator
                                ID="rfvSkrot" runat="server" ControlToValidate="txtSkrot" ErrorMessage="*" Text="wymagane" ValidationGroup="InsertGroup" Display="Dynamic"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblParentGroup" runat="server" Text="Grupa nadrzêdna:" AssociatedControlID="ddlParentGroup"></asp:Label></th>
                        <td>
                            <asp:DropDownList ID="ddlParentGroup" runat="server" DataSourceID="dsParentGroupList"
                                DataTextField="nazwa" DataValueField="id">
                            </asp:DropDownList><asp:SqlDataSource ID="dsParentGroupList" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
                                SelectCommand="[Uzytkownicy].[listaGrup]" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton ID="btnSave" runat="server" Text="Zapisz" CommandName="Insert" ValidationGroup="InsertGroup" /><asp:LinkButton
                                ID="btnCancel" runat="server" CausesValidation="false"
                                Text="Anuluj" CommandName="Cancel" /></td>
                    </tr>
                </table>
                </fieldset>
            </InsertItemTemplate>
        </asp:FormView>
        
        
        
        <asp:FormView CssClass="fullWidth" ID="frmRole" runat="server" DefaultMode="ReadOnly" OnItemInserting="frmRole_ItemInserting" OnItemUpdating="frmRole_ItemUpdating"  OnItemCommand="frmRole_ItemCommand" OnModeChanging="frmRole_ModeChanging" DataKeyNames="id">
            <EditItemTemplate>
            <fieldset ><legend>Edycja roli</legend>
                <table class="grid fullWidth">
                    <tr>
                        <th>
                            <asp:Label ID="lblNazwa" runat="server" Text="Nazwa:" /></th>
                        <td>
                            <asp:TextBox CssClass="longTextBox" ID="txtNazwa" runat="server" 
                                Text='<%# Bind("nazwa") %>' MaxLength="100"><span __designer:mapid="115">&lt;%# 
                            Bind(&quot;nazwa&quot;) %&gt;</span><span __designer:mapid="115">&lt;%# Bind(&quot;nazwa&quot;) %&gt;</span></asp:TextBox><asp:RequiredFieldValidator
                                ID="rfvNazwa" runat="server" ControlToValidate="txtNazwa" ErrorMessage="*" Text="wymagane" ValidationGroup="UpdateRole" Display="Dynamic"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblSkrot" runat="server" AssociatedControlID="txtSkrot" Text="Skrót:"></asp:Label></th>
                        <td>
                            <asp:TextBox ID="txtSkrot" runat="server" Text='<%# Bind("skrot") %>' 
                                MaxLength="10"><span __designer:mapid="115">&lt;%# Bind(&quot;skrot&quot;) %&gt;</span><span __designer:mapid="115">&lt;%# 
                            Bind(&quot;skrot&quot;) %&gt;</span></asp:TextBox><asp:RequiredFieldValidator
                                ID="rfvSkrot" runat="server" ControlToValidate="txtSkrot" ErrorMessage="*" Text="wymagane" ValidationGroup="UpdateRole" Display="Dynamic"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton ID="btnSave" runat="server" Text="Zapisz" CommandName="Update" ValidationGroup="UpdateRole" /><asp:LinkButton
                                ID="btnCancel" runat="server" CausesValidation="false"
                                Text="Anuluj" CommandName="Cancel" /></td>
                    </tr>
                </table>
                </fieldset>
            </EditItemTemplate>
            <InsertItemTemplate>
            <fieldset>
            <legend>Dodawanie nowej roli</legend>
                <table class="grid fullWidth">
                    <tr>
                        <th>
                            <asp:Label ID="lblNazwa" runat="server" AssociatedControlID="txtNazwa" Text="Nazwa:"></asp:Label></th>
                        <td>
                            <asp:TextBox CssClass="longTextBox" ID="txtNazwa" runat="server" 
                                MaxLength="100" ></asp:TextBox><asp:RequiredFieldValidator
                                ID="rfvNazwa" runat="server" ControlToValidate="txtNazwa" ErrorMessage="*" Text="wymagane" ValidationGroup="InsertRole" Display="Dynamic"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="lblSkrot" runat="server" AssociatedControlID="txtSkrot" Text="Skrót:"></asp:Label></th>
                        <td>
                        
                            <asp:TextBox ID="txtSkrot" runat="server" MaxLength="10"></asp:TextBox><asp:RequiredFieldValidator
                                ID="rfvSkrot" runat="server" ControlToValidate="txtSkrot" ErrorMessage="*" Text="wymagane" ValidationGroup="InsertRole" Display="Dynamic"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton ID="btnSave" runat="server" Text="Zapisz" CommandName="Insert" ValidationGroup="InsertRole" /><asp:LinkButton
                                ID="btnCancel" runat="server" CausesValidation="false"
                                Text="Anuluj" CommandName="Cancel" /></td>
                    </tr>
                </table>
                </fieldset>
            </InsertItemTemplate>
        </asp:FormView>
        
</asp:Content>

        
