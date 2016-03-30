<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
    CodeBehind="EdycjaInteresantow.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.EdycjaInteresantow"
    Title="Interesanci" %>

<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/OperacjeInteresantow.ascx" TagName="OperacjeInteresantow"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Interesant.ascx" TagName="Interesant" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ListaInteresantow.ascx" TagName="ListaInteresantow"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
    <uc1:OperacjeInteresantow ID="OperacjeInteresantow1" runat="server" />
    <div id="leftColumn">
        <uc1:Interesant ID="customer" runat="server" />
        <asp:Panel ID="oldStylePanel" runat="Server" Visible="false">
            <hr />
            <hr />
            <hr />
            <fieldset>
                <legend>
                    <asp:Label ID="lblModeDesc" runat="server" Text="Wyszukiwanie interesantÛw"></asp:Label></legend>
                <table>
                    <tr>
                        <td colspan="2">
                            <fieldset>
                                <legend>Typ interesanta</legend>
                                <asp:RadioButtonList ID="rblTypInteresanta" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblTypInteresanta_SelectedIndexChanged"
                                    DataSourceID="dsTypInteresanta" DataTextField="nazwa" DataValueField="id" OnDataBound="rblTypInteresanta_DataBound"
                                    RepeatColumns="2">
                                </asp:RadioButtonList>
                                <asp:SqlDataSource ID="dsTypInteresanta" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
                                    SelectCommand="[Uzytkownicy].[listaTypowInteresanta]" SelectCommandType="StoredProcedure">
                                </asp:SqlDataSource>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblKAtegoria" runat="server" Text="Kategoria:"></asp:Label><br />
                            <asp:DropDownList ID="ddlKategoria" runat="server" Width="323px" OnDataBound="ddlKategoria_DataBound">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="pnlPelnaNazwa" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblImie" runat="server" Text="ImiÍ"></asp:Label><br />
                                            <asp:TextBox ID="txtImie" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNazwisko" runat="server" Text="Nazwisko"></asp:Label><br />
                                            <asp:TextBox ID="txtNazwisko" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlNazwa" runat="server" Visible="False">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNazwa" runat="server" Text="Nazwa"></asp:Label><br />
                                            <asp:TextBox ID="txtNazwa" runat="server" Width="310px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:HiddenField ID="hfCustomerID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Label ID="lblAdres" runat="server" Text="Adres"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblMiasto" runat="server" Text="Miasto"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMiasto" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblKod" runat="server" Text="Kod"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtKod" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblUlica" runat="server" Text="Ulica"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUlica" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblNumer" runat="server" Text="Numer:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBudynek" runat="Server" Width="40px"></asp:TextBox>
                                        /
                                        <asp:TextBox ID="txtLokal" runat="server" Width="40px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:LinkButton ID="lnkFind" runat="server" Text="Znajdü" OnClick="lnkFind_Click"></asp:LinkButton>
                            <asp:LinkButton ID="lnkAddCustomer" runat="server" Visible="False" OnClick="lnkAddCustomer_Click">Wprowadü</asp:LinkButton>
                            <asp:LinkButton ID="lnkUpdateCustomer" runat="server" Visible="False" OnClick="lnkUpdateCustomer_Click">Zapisz</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
    </div>
    <div id="rightColumn">
        <uc1:ListaInteresantow ID="customersList" runat="server" Visible="false" />
    </div>
</asp:Content>
