<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
  Codebehind="ZarzadzanieRodzajamiSpraw.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.ZarzadzanieRodzajamiSpraw"
  Title="Zarz¹dzanie rodzajami spraw" %>
<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego"
  TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
    <div id="singleColumn">
        &nbsp;
        <fieldset>
            <legend>Lista rodzajów spraw</legend>
            <asp:GridView GridLines="None" ID="GridView1" runat="server"
    DataSourceID="ObjectDataSource1" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
    CssClass="fullWidth grid" DataKeyNames="id" EnableViewState="False" PageSize="20">
                <PagerSettings Mode="NextPreviousFirstLast" />
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True"
        SortExpression="id" Visible="False" />
                    <asp:BoundField  DataField="nazwa" SortExpression="nazwa"
        HeaderText="Nazwa rodzaju sprawy">
                        <ItemStyle Width="35em" />
                    </asp:BoundField>
                    <asp:BoundField DataField="opis" SortExpression="opis"
        HeaderText="Opis rodzaju sprawy" />
                    <asp:BoundField DataField="iloscDniNaRozpatrzenie" ItemStyle-HorizontalAlign="Center" HeaderText="Il. dni na rozpatrzenie" />
                    <asp:ButtonField Text="szczegó³y" CommandName="Select" HeaderText="operacje" />
                </Columns>
                <SelectedRowStyle BackColor="Red" CssClass="wybrany" />
                <AlternatingRowStyle CssClass="pozycjaNieparzysta" />
            </asp:GridView>
            <asp:LinkButton runat="server" ID="addNew" Text="Dodaj nowy rodzaj sprawy" OnCommand="addNew_Command" />
        </fieldset>
        <asp:FormView  CssClass="grid fullWidth" ID="FormView1" runat="server" DataKeyNames="id" DataSourceID="ODS2" OnItemDeleted="FormView1_ItemDeleted" OnItemInserted="FormView1_ItemInserted" OnItemUpdated="FormView1_ItemUpdated">
            <EditItemTemplate>
             <tr>
                    <th colspan="2">
                        Edycja rodzaju sprawy</th>
                </tr>
                <tr>
                    <th>
                        Nazwa</th>
                    <td>
                        <asp:TextBox CssClass="longTextBox" ID="nazwaTextBox" runat="server" 
                            Text='<%# Bind("nazwa") %>' MaxLength="60" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="nazwaTextBox"
          Display="Dynamic" ErrorMessage="Trzeba podaæ nazwê!" />
                    </td>
                </tr>
                
                <tr>
                    <th>
                        Opis</th>
                    <td>
                        <asp:TextBox CssClass="longTextBox" ID="opisTextBox" runat="server" 
                            Text='<%# Bind("opis") %>' MaxLength="1000" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="opisTextBox"
          Display="Dynamic" ErrorMessage="Trzeba podaæ opis!" ValidationGroup="spr"/>
                    </td>
                </tr>
                <tr>
                    <th>Il. dni na rozpatrzenie</th>
                    <td>
                        <asp:TextBox ID="ilDniNaRozpatrzenie" runat="server" MaxLength="3" Text='<%#Bind("iloscDniNaRozpatrzenie") %>' />
                        <asp:CompareValidator runat="server" ValidationGroup="spr" ID="ilDniNaRozpatrzenieValidotor" ControlToValidate="ilDniNaRozpatrzenie" Operator="GreaterThan" ValueToCompare="0" Type="Integer" ErrorMessage="Nieprawid³owa wartoœæ!" />
                    </td>
                </tr>                
                <tr>
                    <td colspan="2">
                    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                    CommandName="Update" Text="zapisz" ValidationGroup="spr"></asp:LinkButton>
                
                <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" 
                    CommandName="Cancel" Text="anuluj"></asp:LinkButton>               
                    </td>
                </tr>
                
                
            </EditItemTemplate>
            <InsertItemTemplate>
            <tr>
                    <th colspan="2">
                        Dodawanie rodzaju sprawy</th>
                </tr>
               <tr>
                    <th>
                        Nazwa</th>
                    <td>
                        <asp:TextBox ID="nazwaTextBox" runat="server" Text='<%# Bind("nazwa") %>' 
                            MaxLength="60" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="nazwaTextBox"
          Display="Dynamic" ErrorMessage="Trzeba podaæ nazwê!" ValidationGroup="spr" />
                    </td>
                </tr>
                
                <tr>
                    <th>
                        Opis</th>
                    <td>
                        <asp:TextBox ID="opisTextBox" runat="server" Text='<%# Bind("opis") %>' 
                            MaxLength="1000" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="opisTextBox"
          Display="Dynamic" ErrorMessage="Trzeba podaæ opis!" ValidationGroup="spr" />
                    </td>
                </tr>
               <tr>
                    <th>Il. dni na rozpatrzenie</th>
                    <td>
                        <asp:TextBox ID="ilDniNaRozpatrzenie" MaxLength="3" runat="server" Text='<%#Bind("iloscDniNaRozpatrzenie") %>' />
                        <asp:CompareValidator runat="server" ValidationGroup="spr" ID="ilDniNaRozpatrzenieValidotor" ControlToValidate="ilDniNaRozpatrzenie" Operator="GreaterThan" ValueToCompare="0" Type="Integer" ErrorMessage="Nieprawid³owa wartoœæ!" />
                    </td>
                </tr> 
                <tr>
                    <td colspan="2">
                        <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Insert"
          Text="zapisz" ValidationGroup="spr"></asp:LinkButton>
                        <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
          Text="anuluj"></asp:LinkButton>
                    </td>
                </tr>
                
            </InsertItemTemplate>
            <ItemTemplate>
                <tr>
                    <th colspan="2">
                        Szczegó³y rodzaju sprawy</th>
                </tr>
                <tr>
                    <th>
                        Nazwa</th>
                    <td>
                        <asp:Label ID="nazwaTextBox" runat="server" Text='<%# Bind("nazwa") %>' />
                    </td>
                </tr>
                <tr>
                    <th>
                        Opis</th>
                    <td>
                        <asp:Label ID="opisTextBox" runat="server" Text='<%# Bind("opis") %>' />
                    </td>
                </tr>
                <tr>
                    <th>Il. dni na rozpatrzenie</th>
                    <td>
                        <asp:Label ID="ilDniNaRozpatrzenie" runat="server" Text='<%#Bind("iloscDniNaRozpatrzenie") %>' />
                    </td>
                </tr> 
                <tr>
                    <td colspan="2">
                        <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" CommandName="Edit"
          Text="edytuj"></asp:LinkButton>
                        <asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" CommandName="Delete"
          Text="usuñ"></asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:FormView>
        <asp:ObjectDataSource ID="ODS2" runat="server" DeleteMethod="Delete" InsertMethod="Insert"
      SelectMethod="GetDataBy" UpdateMethod="Update"
      
          
            TypeName="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.RodzajeSprawTableAdapters.listaRodzajowSprawTableAdapter">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="id" Type="Int32" />
                <asp:Parameter Name="nazwa" Type="String" />
                <asp:Parameter Name="opis" Type="String" />
                <asp:Parameter Name="definicja" Type="Object" />
                <asp:Parameter Name="iloscDniNaRozpatrzenie" Type="Int32" />
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="GridView1" DefaultValue="0" Name="id" PropertyName="SelectedValue"
          Type="Int32" />
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Name="nazwa" Type="String" />
                <asp:Parameter Name="opis" Type="String" />
                <asp:Parameter Name="definicja" Type="Object" />
                <asp:Parameter Name="iloscDniNaRozpatrzenie" Type="Int32" />
            </InsertParameters>
        </asp:ObjectDataSource>
    &nbsp;
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
      SelectMethod="GetData" 
          TypeName="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.RodzajeSprawTableAdapters.listaRodzajowSprawTableAdapter" 
          DeleteMethod="Delete" InsertMethod="Insert" UpdateMethod="Update">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="id" Type="Int32" />
                <asp:Parameter Name="nazwa" Type="String" />
                <asp:Parameter Name="opis" Type="String" />
                <asp:Parameter Name="definicja" Type="Object" />
                <asp:Parameter Name="iloscDniNaRozpatrzenie" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="nazwa" Type="String" />
                <asp:Parameter Name="opis" Type="String" />
                <asp:Parameter Name="definicja" Type="Object" />
                <asp:Parameter Name="iloscDniNaRozpatrzenie" Type="Int32" />
            </InsertParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
