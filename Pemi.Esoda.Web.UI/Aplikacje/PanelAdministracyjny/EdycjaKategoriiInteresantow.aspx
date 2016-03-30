<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="EdycjaKategoriiInteresantow.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.EdycjaKategoriiInteresantow" Title="Kategorie interesantów" %>
<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/OperacjeInteresantow.ascx" TagName="OperacjeInteresantow" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/TypInteresanta.ascx" TagName="TypInteresanta" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
<uc1:OperacjeInteresantow ID="OperacjeInteresantow1" runat="server" />
<div id="leftColumn">
<fieldset>
<legend>Typy interesantów</legend>
<asp:RadioButtonList  ID="rblTypInteresanta" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblTypInteresanta_SelectedIndexChanged" DataSourceID="dsTypInteresanta" DataTextField="nazwa" DataValueField="id" OnDataBound="rblTypInteresanta_DataBound">
</asp:RadioButtonList>
    <asp:SqlDataSource ID="dsTypInteresanta" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
        SelectCommand="[Uzytkownicy].[listaTypowInteresanta]" SelectCommandType="StoredProcedure">
    </asp:SqlDataSource>
    <br />

<asp:FormView ID="frmKategoria" runat="server" OnItemInserting="frmKategoria_ItemInserting" OnItemUpdating="frmKategoria_ItemUpdating" OnModeChanging="frmKategoria_ModeChanging" OnItemCommand="frmKategoria_ItemCommand">
<InsertItemTemplate>
<table>
<tr><td>Kategoria:</td><td><asp:TextBox ID="txtKategoria" runat="server" 
        MaxLength="255"></asp:TextBox><br />
    <asp:RequiredFieldValidator ID="rfvKategoria" runat="server" ControlToValidate="txtKategoria"
        Display="Dynamic" ErrorMessage="*" ValidationGroup="InsertCategory">podaj nazwê kategorii</asp:RequiredFieldValidator></td></tr>
<tr><td colspan="2" align="center"><asp:LinkButton ID="lnkInsertCat" runat="server" Text="Zapisz" CommandName="Insert" ValidationGroup="InsertCategory"></asp:LinkButton>
<asp:LinkButton ID="lnkRemoveCat" runat="server" Text="Anuluj" CommandName="Cancel"></asp:LinkButton>
</td></tr>
</table>
</InsertItemTemplate>
<EditItemTemplate>
<table>
<tr><td>Kategoria:</td><td><asp:TextBox ID="txtKategoria" runat="server" 
        Text='<%# Bind("nazwa") %>' MaxLength="255"></asp:TextBox><br />
    <asp:RequiredFieldValidator ID="rfvKategoria" runat="server" ControlToValidate="txtKategoria"
        Display="Dynamic" ErrorMessage="*" ValidationGroup="EditCategory">podaj nazwê kategorii</asp:RequiredFieldValidator></td></tr>
<tr><td colspan="2" align="center"><asp:LinkButton ID="lnkInsertCat" runat="server" Text="Zapisz" CommandName="Update" ValidationGroup="EditCategory"></asp:LinkButton>
<asp:LinkButton ID="lnkRemoveCat" runat="server" Text="Anuluj" CommandName="Cancel"></asp:LinkButton>
</td></tr>
</table>
</EditItemTemplate>
</asp:FormView>
</fieldset>
</div>

<div id="rightColumn">
<fieldset>
<legend>Lista kategorii interesantów</legend>
<asp:GridView ID="gvKategorieInteresantow" CssClass="grid fullWidth" runat="server" AutoGenerateColumns="False" DataKeyNames="id" OnSelectedIndexChanged="gvKategorieInteresantow_SelectedIndexChanged" OnRowCommand="gvKategorieInteresantow_RowCommand">
<EmptyDataTemplate>Brak kategorii dla danego typu interesanta</EmptyDataTemplate>
    <Columns>
        <asp:TemplateField HeaderText="Lp.">
            <ItemTemplate>
                <%# Container.DataItemIndex + 1 %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Typ interesanta" DataField="typ" />
        <asp:BoundField HeaderText="Nazwa kategorii" DataField="kategoria" />
        <asp:TemplateField HeaderText="Akcje">
            <ItemTemplate>
                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Select" CommandArgument='<%# Eval("id") %>'>edytuj</asp:LinkButton><br />
                <asp:LinkButton ID="lnkRemove" runat="server" CommandName="Remove" CommandArgument='<%# Eval("id") %>' OnClientClick="return confirm('Czy napewno chcesz usun¹æ wybran¹ kategoriê?');">usuñ</asp:LinkButton>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
    </Columns>
</asp:GridView>
    <asp:LinkButton ID="lnkAddCategory" runat="server" OnClick="lnkAddCategory_Click">Dodaj kategoriê</asp:LinkButton></fieldset>

</div>
</asp:Content>