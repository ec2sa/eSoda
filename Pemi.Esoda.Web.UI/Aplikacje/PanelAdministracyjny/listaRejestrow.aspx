<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="listaRejestrow.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.listaRejestrow" Title="Rejestry" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <uc1:OperacjePaneluAdministracjnego ID="OperacjePanelu" runat="server" />
    <div id="leftColumn" style="width: 17%;">
        <fieldset>
            <legend>Filtry</legend>
            <table>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblRok" runat="server" Text="Rok:" AssociatedControlID="ddlRok" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRok" runat="server" AutoPostBack="True" 
        ondatabound="ddlRok_DataBound">
                            <asp:ListItem Text="- wszystkie -" Value="-1" Selected="True" />
                            <asp:ListItem Text="2007" Value="2007" />
                            <asp:ListItem Text="2008" Value="2008" />
                            <asp:ListItem Text="2009" Value="2009" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:LAbel ID="lblStatus" runat="server" Text="Status:" AssociatedControlID="ddlStatus" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True">
                            <asp:ListItem Text="- wszystkie -" Selected="True" Value="-1" />
                            <asp:ListItem Text="Aktywne" Value="0" />
                            <asp:ListItem Text="Archiwalne" Value="1" />
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div id="rightColumn" style="width: 82%;">
        <asp:GridView CssClass="grid fullWidth" ID="gvRegistersList" runat="server" AllowSorting="True" 
        AutoGenerateColumns="False" DataSourceID="odsRegistersList" 
        onrowcommand="gvRegistersList_RowCommand" DataKeyNames="id" 
        onrowdatabound="gvRegistersList_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Lp.">
                    <ItemTemplate>
                        <%# Container.DataItemIndex+1 %>. 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="nazwa" HeaderText="Nazwa" SortExpression="nazwa" />
                <asp:BoundField DataField="nazwaJRWA" HeaderText="JRWA" SortExpression="nazwaJRWA" />
                <asp:BoundField DataField="nazwaWydzialu" HeaderText="Wydział główny" SortExpression="nazwaWydzialu" />
                <asp:TemplateField HeaderText="Operacje">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" Text="edycja" CommandName="RegEdit" CommandArgument='<%# Eval("id") %>' />
                        <asp:LinkButton ID="lnkPreview" runat="server" Text="podgląd" CommandName="Preview" CommandArgument='<%# Eval("id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsRegistersList" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetRegistersList" 
        TypeName="Pemi.Esoda.DataAccess.RegistryDAO" >
            <SelectParameters>
                <asp:ControlParameter ControlID="gvRegistersList" DefaultValue="" 
            Name="sortParam" PropertyName="SelectedValue" Type="String"></asp:ControlParameter>
                <asp:ControlParameter ControlID="ddlRok" DefaultValue="-1" Name="rok" 
            PropertyName="SelectedValue" Type="Int32"></asp:ControlParameter>
                <asp:ControlParameter ControlID="ddlStatus" DefaultValue="" Name="status" 
                      PropertyName="SelectedValue" Type="Int32"></asp:ControlParameter>
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:LinkButton ID="lnkNewRegister" runat="server" Text="Nowy rejestr" 
        onclick="lnkNewRegister_Click" />
    </div>
</asp:Content>