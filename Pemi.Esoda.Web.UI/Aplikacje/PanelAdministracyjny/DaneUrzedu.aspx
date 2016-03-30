<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="DaneUrzedu.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.DaneUrzedu" Title="Dane Urzêdu" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="opa1" runat="server" />
<asp:FormView ID="frmDaneUrzedu" runat="server" DataSourceID="odsOfficeData" OnItemUpdating="frmDaneUrzedu_ItemUpdating" OnModeChanging="frmDaneUrzedu_ModeChanging">
<ItemTemplate>
<table class="grid">
<tr><th colspan="2" align="center">Podstawowe informacje o urzêdzie:</th></tr>
<tr><th align="right">Pe³na nazwa urzêdu:</th><td><asp:Label ID="lblPelnaNazwa" runat="server" Text='<%# Eval("PelnaNazwa") %>' /></td></tr>
<tr><th align="right">Typ<br /> (np. Starostwo Powiatowe,<br />Urz¹d Gminy):</th><td><asp:Label ID="lblTyp" runat="server" Text='<%# Eval("TypUrzedu") %>' /></td></tr>
<tr><th align="right">Organ kieruj¹cy:</th><td><asp:Label ID="lblOrganKierujacy" runat="server" Text='<%# Eval("OrganKierujacy") %>' /></td></tr>
</table>
<br />
<table class="grid">
<tr><th colspan="2" align="center">Dane podatkowe</th></tr>
<tr><th align="right">NIP:</th><td><asp:Label ID="lblNIP" runat="server" Text='<%# Eval("NIP") %>' /></td></tr>
<tr><th align="right">REGON:</th><td><asp:Label ID="lblREGON" runat="server" Text='<%# Eval("REGON") %>' /></td></tr>
</table>
<br />
<table class="grid">
<tr><th colspan="2" align="center">Adres</th></tr>
<tr><th align="right">Miasto:</th><td><asp:Label ID="Label1" runat="server" Text='<%# Eval("Miasto") %>' /></td></tr>
<tr><th align="right">Ulica:</th><td><asp:Label ID="Label2" runat="server" Text='<%# Eval("Ulica") %>' /></td></tr>
<tr><th align="right">Nr budynku:</th><td><asp:Label ID="Label3" runat="server" Text='<%# Eval("Budynek") %>' /></td></tr>
<tr><th align="right">Nr mieszkania:</th><td><asp:Label ID="Label4" runat="server" Text='<%# Eval("Lokal") %>' /></td></tr>
</table>
<br />
<table class="grid">
<tr><th colspan="2" align="center">Dane kontaktowe</th></tr>
<tr><th align="right">Telefon:</th><td><asp:Label ID="Label5" runat="server" Text='<%# Eval("Telefon") %>' /></td></tr>
<tr><th align="right">Fax:</th><td><asp:Label ID="Label6" runat="server" Text='<%# Eval("Fax") %>' /></td></tr>
<tr><th align="right">strona WWW:</th><td><asp:Label ID="Label7" runat="server" Text='<%# Eval("WWW") %>' /></td></tr>
<tr><th align="right">strona BIP:</th><td><asp:Label ID="Label8" runat="server" Text='<%# Eval("BIP") %>' /></td></tr>
<tr><th align="right">adres e-mail:</th><td><asp:Label ID="Label9" runat="server" Text='<%# Eval("Email") %>' /></td></tr>
</table>
<hr />
<asp:LinkButton ID="lnkEdit" runat="server" Text="Zmodyfikuj" CommandName="Edit" />
</ItemTemplate>
<EditItemTemplate>
<table class="grid">
<tr><th colspan="2" align="center">Podstawowe informacje o urzêdzie:</th></tr>
<tr><th align="right">Pe³na nazwa urzêdu:</th><td><asp:TextBox ID="txt" 
        runat="server" Text='<%# Bind("PelnaNazwa") %>' MaxLength="255" /></td></tr>
<tr><th align="right">Typ (np. Starostwo Powiatowe, Urz¹d Gminy):</th><td>
    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("TypUrzedu") %>' 
        MaxLength="255" /></td></tr>
<tr><th align="right">Organ kieruj¹cy:</th><td><asp:TextBox ID="TextBox2" 
        runat="server" Text='<%# Bind("OrganKierujacy") %>' MaxLength="255" /></td></tr>
</table>
<br />
<table class="grid">
<tr><th colspan="2" align="center">Dane podatkowe</th></tr>
<tr><th align="right">NIP:</th><td><asp:TextBox ID="TextBox3" runat="server" 
        Text='<%# Bind("NIP") %>' MaxLength="20" /></td></tr>
<tr><th align="right">REGON:</th><td><asp:TextBox ID="TextBox4" runat="server" 
        Text='<%# Bind("REGON") %>' MaxLength="10" /></td></tr>
</table>
<br />
<table class="grid">
<tr><th colspan="2" align="center">Adres</th></tr>
<tr><th align="right">Miasto:</th><td><asp:TextBox ID="TextBox5" runat="server" 
        Text='<%# Bind("Miasto") %>' MaxLength="255" /></td></tr>
<tr><th align="right">Ulica:</th><td><asp:TextBox ID="TextBox6" runat="server" 
        Text='<%# Bind("Ulica") %>' MaxLength="255" /></td></tr>
<tr><th align="right">Nr budynku:</th><td><asp:TextBox ID="TextBox7" runat="server" 
        Text='<%# Bind("Budynek") %>' MaxLength="20" /></td></tr>
<tr><th align="right">Nr mieszkania:</th><td><asp:TextBox ID="TextBox8" 
        runat="server" Text='<%# Bind("Lokal") %>' MaxLength="20" /></td></tr>
</table>
<br />
<table class="grid">
<tr><th colspan="2" align="center">Dane kontaktowe</th></tr>
<tr><th align="right">Telefon:</th><td><asp:TextBox ID="TextBox9" runat="server" 
        Text='<%# Bind("Telefon") %>' MaxLength="50" /></td></tr>
<tr><th align="right">Fax:</th><td><asp:TextBox ID="TextBox10" runat="server" 
        Text='<%# Bind("Fax") %>' MaxLength="50" /></td></tr>
<tr><th align="right">strona WWW:</th><td><asp:TextBox ID="TextBox11" 
        runat="server" Text='<%# Bind("WWW") %>' MaxLength="255" /></td></tr>
<tr><th align="right">strona BIP:</th><td><asp:TextBox ID="TextBox12" 
        runat="server" Text='<%# Bind("BIP") %>' MaxLength="255" /></td></tr>
<tr><th align="right">adres e-mail:</th><td><asp:TextBox ID="TextBox13" 
        runat="server" Text='<%# Bind("Email") %>' MaxLength="255" /></td></tr>
</table>
<hr />
<asp:LinkButton ID="lnkUpdate" runat="server" Text="Zapisz zmiany" CommandName="Update" />
<asp:LinkButton ID="lnkCancel" runat="server" Text="Anuluj" CommandName="Cancel" />
</EditItemTemplate>
</asp:FormView>
    <asp:ObjectDataSource ID="odsOfficeData" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="GetOfficeData" TypeName="Pemi.Esoda.DataAccess.OfficeDAO" OnUpdating="odsOfficeData_Updating" UpdateMethod="UpdateOfficeData">
        <UpdateParameters>
            <asp:Parameter Name="PelnaNazwa" Type="String" />
            <asp:Parameter Name="TypUrzedu" Type="String" />
            <asp:Parameter Name="OrganKierujacy" Type="String" />
            <asp:Parameter Name="NIP" Type="String" />
            <asp:Parameter Name="REGON" Type="String" />
            <asp:Parameter Name="Miasto" Type="String" />
            <asp:Parameter Name="Ulica" Type="String" />
            <asp:Parameter Name="Budynek" Type="String" />
            <asp:Parameter Name="Lokal" Type="String" />
            <asp:Parameter Name="Telefon" Type="String" />
            <asp:Parameter Name="Fax" Type="String" />
            <asp:Parameter Name="WWW" Type="String" />
            <asp:Parameter Name="BIP" Type="String" />
            <asp:Parameter Name="Email" Type="String" />
        </UpdateParameters>
    </asp:ObjectDataSource>
</asp:Content>