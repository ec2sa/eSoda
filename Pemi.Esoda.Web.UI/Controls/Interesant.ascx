<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Interesant.ascx.cs"
    Inherits="Pemi.Esoda.Web.UI.Controls.Interesant" %>
<style type="text/css">
    .style1
    {
       
    }
</style>
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
            <td colspan="2" class="style1">
                <asp:Label ID="lblKAtegoria" runat="server" Text="Kategoria:"></asp:Label><br />
                <asp:Label ID="lblTxtKategoria" runat="server" Visible="False"></asp:Label><br />
                <asp:DropDownList ID="ddlKategoria" runat="server" OnDataBound="ddlKategoria_DataBound">
                </asp:DropDownList>
                <br />
                <asp:RequiredFieldValidator ID="rfvKategoria" runat="server" ControlToValidate="ddlKategoria"
                    Display="Dynamic" ErrorMessage="*" ValidationGroup="EditCustomer" InitialValue="-1">wymagane jest podanie kategorii interesanta dla danego 
    typu</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="style1">
                <asp:Panel ID="pnlPelnaNazwa" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblImie" runat="server" Text="ImiÍ"></asp:Label><br />
                                <asp:Label ID="lblTxtImie" runat="server" BorderStyle="Solid" BorderWidth="1px" Visible="False"></asp:Label>
                                <asp:TextBox ID="txtImie" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvImie" runat="server" ControlToValidate="txtImie"
                                    Display="Dynamic" ErrorMessage="*" ValidationGroup="EditCustomer">wymagane</asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="lblNazwisko" runat="server" Text="Nazwisko"></asp:Label><br />
                                <asp:Label ID="lblTxtNazwisko" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                    Visible="False"></asp:Label>
                                <asp:TextBox ID="txtNazwisko" runat="server" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNazwisko" runat="server" ControlToValidate="txtNazwisko"
                                    Display="Dynamic" ErrorMessage="*" ValidationGroup="EditCustomer">wymagane</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlNazwa" runat="server" Visible="True">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblNazwa" runat="server" Text="Nazwa"></asp:Label><br />
                                <asp:Label ID="lblTxtNazwa" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                    Visible="False"></asp:Label><br />
                                <asp:TextBox ID="txtNazwa" runat="server" Width="310px" MaxLength="250"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNazwa" runat="server" ControlToValidate="txtNazwa"
                                    Display="Dynamic" ErrorMessage="*" ValidationGroup="EditCustomer">wymagane</asp:RequiredFieldValidator>
                                 <br /><br />
                                 
                                  <asp:Label ID="Label1" runat="server" Text="NIP" AssociatedControlID="txtNIP"
          CssClass="etykieta" /><br />
          <asp:Label runat="server" ID="lblTxtNIP" BorderStyle="Solid" BorderWidth="1px" Visible="False"/>   
          <asp:TextBox runat="server" ID="txtNIP" MaxLength="13" />   
         
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                 <br />
           <asp:Label ID="Label3" runat="server" Text="Numer do potwierdzeÒ SMS" AssociatedControlID="txtNumerSMS"
          CssClass="etykieta" /><br />
           <strong>+48</strong><asp:Label runat="server" ID="lblTxtNumerSMS" BorderStyle="Solid" BorderWidth="1px" Visible="False"/>  
        <asp:TextBox runat="server" ID="txtNumerSMS" MaxLength="9" />
        <br />
                <asp:HiddenField ID="hfCustomerID" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center" class="style1">
                <asp:Label ID="lblAdres" runat="server" Text="Adres"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style1">
                <table>
                <tr>
                        <td align="right">
                            <asp:Label ID="lblKod" runat="server" Text="Kod"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKod" runat="server"></asp:TextBox><asp:Label ID="lblTxtKod" runat="server"
                                BorderStyle="Solid" BorderWidth="1px" Visible="False" Width="153px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblMiasto" runat="server" Text="Miasto"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMiasto" runat="server"></asp:TextBox><asp:Label ID="lblTxtMiasto"
                                runat="server" BorderStyle="Solid" BorderWidth="1px" Visible="False" Width="152px"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblUlica" runat="server" Text="Ulica"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUlica" runat="server"></asp:TextBox><asp:Label ID="lblTxtUlica"
                                runat="server" BorderStyle="Solid" BorderWidth="1px" Visible="False" Width="152px"></asp:Label>
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblNumer" runat="server" Text="Numer:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBudynek" runat="Server" Width="40px"></asp:TextBox><asp:Label
                                ID="lblTxtBudynek" runat="server" BorderStyle="Solid" BorderWidth="1px" Visible="False"></asp:Label>
                            &nbsp;/
                            <asp:TextBox ID="txtLokal" runat="server" Width="40px"></asp:TextBox><asp:Label ID="lblTxtLokal"
                                runat="server" BorderStyle="Solid" BorderWidth="1px" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label2" runat="server" Text="Poczta" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPoczta" runat="server"></asp:TextBox><asp:Label ID="lblTxtPoczta"
                                runat="server" BorderStyle="Solid" BorderWidth="1px" Visible="False" ></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" class="style1">
                <asp:LinkButton ID="lnkFind" runat="server" Text="Znajdü" OnClick="lnkFind_Click"></asp:LinkButton>
                <asp:LinkButton ID="lnkAddCustomer" runat="server" Visible="False" OnClick="lnkAddCustomer_Click"
                    ValidationGroup="EditCustomer">Wprowadü</asp:LinkButton>
                <asp:LinkButton ID="lnkUpdateCustomer" runat="server" Visible="False" OnClick="lnkUpdateCustomer_Click"
                    ValidationGroup="EditCustomer">Zapisz</asp:LinkButton>
                <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click" Visible="False">Edytuj</asp:LinkButton>
                <asp:LinkButton ID="lnkSearchMode" runat="server" OnClick="lnkSearchMode_Click">Wyszukiwanie</asp:LinkButton>
            </td>
        </tr>
    </table>
</fieldset>
