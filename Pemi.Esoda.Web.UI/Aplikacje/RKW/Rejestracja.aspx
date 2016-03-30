<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master"
    AutoEventWireup="true" CodeBehind="Rejestracja.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.RKW.Rejestracja" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>
        Rejestracja pozycji w Rejestrze Korespondencji Wychodzącej</h2>
    <asp:LinkButton runat="server" PostBackUrl="~/Aplikacje/RKW/Przegladanie.aspx" Text="Powrót" />
    <hr />
    <fieldset>
        <legend>Szczegóły rejestrowanej pozycji</legend>
        <h4>Przekazano do wysłania:  <asp:Literal runat="server" ID="ldataPrzekazania" /><%--<br />Jako: <asp:Literal runat="server" ID="lTypKorespondencji" />--%></h4>
        <table class="grid">
            <tr>
                <th>
                    Znak pisma
                </th>
                <td>
                    <asp:Literal runat="server" ID="lZnakPisma" />
                </td>
            </tr>
            <tr>
                <th>
                    Wydział
                </th>
                <td>
                    <asp:Literal runat="server" ID="lWydzial" />
                </td>
            </tr>
            <tr>
                <th>
                    Pracownik
                </th>
                <td>
                    <asp:Literal runat="server" ID="lPracownik" />
                </td>
            </tr>
            <tr>
                <th>
                    Typ dokumentu
                </th>
                <td>
                    <asp:Literal runat="server" ID="lTypDokumentu" />
                </td>
            </tr>
            <tr>
                <th>
                    Adresat
                </th>
                <td>
                    <asp:Literal runat="server" ID="lNazwaAdresata" /><br />
                    <asp:Literal runat="server" ID="lUlicaAdresata" /><br />
                    <asp:Literal runat="server" ID="lKodIMiastoAdresata" />
                </td>
            </tr>
            <tr>
                <th>
                    Uwagi
                </th>
                <td>
                    <asp:TextBox runat="server" ID="tbUwagi" Style="width: 95%" />
                </td>
            </tr>
            <tr>
                <th>
                    Typ korespondencji
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlZone" Enabled='false'>
                        <asp:ListItem Value="1" Text="Europa" />
                        <asp:ListItem Value="2" Text="Pozostałe" />
                    </asp:DropDownList>
                    <asp:CheckBoxList runat="server" ID="cbAdditions" />
                </td>
            </tr>
            <tr>
            <td colspan="2" style="text-align:center">
            <asp:Button runat="server" ID="btnRegister" Text="Zarejestruj" OnClick="registerDocument" />
            </td>
            </tr>
        </table>
    </fieldset>
    <script type="text/javascript">
        var foreign = document.getElementById("ctl00_main_cbAdditions_0");
        var zones = document.getElementById("ctl00_main_ddlZone");

        foreign.onchange = function (e) {

            zones.disabled = !foreign.checked;

        }
        
    </script>
</asp:Content>
