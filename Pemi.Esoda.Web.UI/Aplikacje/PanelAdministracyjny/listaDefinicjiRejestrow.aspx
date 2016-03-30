<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="listaDefinicjiRejestrow.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.listaDefinicjiRejestrow" Title="Lista definicji rejestrów" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<fieldset>
<legend>Definicje rejestrów</legend>
<asp:GridView ID="gvListaDefinicji" runat="server" CssClass="grid fullWidth" 
        AutoGenerateColumns="False" DataSourceID="odsListaDefinicji" 
        onrowcommand="gvListaDefinicji_RowCommand" >
    <Columns>
        <asp:BoundField DataField="id" HeaderText="Id" />
        <asp:BoundField DataField="nazwa" HeaderText="Nazwa" />
        <asp:TemplateField HeaderText="Operacje">
        <ItemTemplate>
        <asp:LinkButton ID="lnkEdycjaDefinicji" runat="server" Text="edycja" CommandName="DefEdit" CommandArgument='<%# Eval("id") %>' />
        </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    </asp:GridView>    
<asp:ObjectDataSource ID="odsListaDefinicji" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="GetRegistryDefinitionList" 
        TypeName="Pemi.Esoda.DataAccess.RegistryDAO" />
        <asp:LinkButton ID="lnkAddDef" runat="server" Text="Dodaj definicję rejestru" />
</fieldset>
</asp:Content>
