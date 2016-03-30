<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
  Codebehind="edycjaSprawy.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Akcje.edycjaSprawy"
  Title="Edycja danych sprawy" %>
<%@ Register Src="~/Controls/Interesant.ascx" TagName="Interesant" TagPrefix="esoda" %>
<%@ Register Src="~/Controls/ListaInteresantow.ascx" TagName="ListaInteresantow" TagPrefix="esoda" %>
<%@ Register TagPrefix="esoda" TagName="CaseContext" Src="~/Controls/CaseContextItem.ascx" %>
<%@ Register TagPrefix="esoda" TagName="OperacjeSprawy" Src="~/Controls/OperacjeSprawy.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <esoda:CaseContext runat="Server" ID="cc1" />
    <h2>
    <%=Page.Title %>
    </h2>
    <esoda:OperacjeSprawy runat="server" ID="ops1" />
    <hr />
    <div id="leftColumn">
        <fieldset>
            <legend>Edycja danych sprawy</legend>
            <asp:ValidationSummary runat="server" ValidationGroup="UpdateGroup" DisplayMode="List" />
            <asp:FormView ID="frmSprawa" runat="server" DefaultMode="Edit" CellPadding="0" 
          OnItemUpdating="frmSprawa_ItemUpdating" 
          onmodechanging="frmSprawa_ModeChanging"  >
                <EditItemTemplate>
                    <table class="grid">
                        <tr>
                            <th>
                                Sprawa (krótka treœæ)</th>
                            <td>
                                <asp:TextBox Enabled="false" ID="txtOpis" runat="server" Text='<%# XPath("rodzajSprawy") %>' TextMode="MultiLine"
                Rows="5" Columns="40"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Interesant<br />
              (Od kogo wp³ynê³a)</th>
                            <td>
                                <asp:Label ID="lblOdKogo" runat="server" Text='<%# XPath("nadawca") %>' />
                                <br />
                                <asp:LinkButton ID="lnkZmien" runat="server" Text="Zmieñ" 
                    onclick="lnkZmien_Click" />
                                <asp:HiddenField ID="hfCustomerId" runat="server" Value='<%# XPath("idNadawcy") %>' />
                                <asp:HiddenField ID="hfCaseId" runat="server" Value='<%# XPath("id") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Znak pisma</th>
                            <td>
                                <asp:TextBox ID="txtZnakPisma" runat="server" Text='<%# XPath("znakPisma") %>'></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Data pisma</th>
                            <td>
                                <asp:TextBox ID="txtDataPisma" runat="server" Text='<%# XPath("dataPisma") %>' />
                                <asp:CompareValidator runat="server" ControlToValidate="txtDataPisma" Operator="DataTypeCheck" Type="Date" ErrorMessage="B³êdna data!" Text="*" ValidationGroup="UpdateGroup" />
                                <ajax:CalendarExtender ID="txtDataPisma_CalendarExtender" runat="server" TargetControlID="txtDataPisma"/>                                
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Data wszczêcia</th>
                            <td>
                                <asp:TextBox runat="server" ID="dataRozpoczecia" Text='<%# XPath("dataRozpoczecia") %>'></asp:TextBox>                                
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="dataRozpoczecia" Operator="DataTypeCheck" Type="Date" ErrorMessage="B³êdna data!" Text="*" ValidationGroup="UpdateGroup" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="dataRozpoczecia" ErrorMessage="Nale¿y podaæ date wszczêcia." Text="*" ValidationGroup="UpdateGroup" />
                                <ajax:CalendarExtender ID="dataRozpoczecia_CalendarExtender" runat="server" TargetControlID="dataRozpoczecia"/>                                
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Data ostatecznego zakoñczenia</th>
                            <td>
                                <asp:TextBox runat="server" ID="dataZakonczenia" Text='<%# XPath("dataZakonczenia") %>'></asp:TextBox>
                                <ajax:CalendarExtender ID="dataZakonczenia_CalendarExtender" runat="server" TargetControlID="dataZakonczenia" />
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="dataZakonczenia" Operator="DataTypeCheck" Type="Date" ErrorMessage="B³êdna data!" Text="*" ValidationGroup="UpdateGroup" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Uwagi:</th>
                            <td>
                                <asp:TextBox ID="txtUwagi" runat="server" TextMode="MultiLine" Rows="5" Columns="40"
                Text='<%# XPath("uwagi") %>'></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Status:</th>
                            <td>
                                <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="dsStatus" DataTextField="nazwa"
                DataValueField="id" SelectedValue='<%# XPath("idStatusu") %>'>
                                </asp:DropDownList>
                                <br />
                                <asp:SqlDataSource ID="dsStatus" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
                SelectCommand="[Sprawy].[listaStatusow]" SelectCommandType="StoredProcedure">
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:LinkButton ValidationGroup="UpdateGroup" ID="lnkUpdate" runat="server" CommandName="Update" OnCommand="zakonczAkcje" Text="zapisz"/>
                                <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" OnCommand="zakonczAkcje">anuluj</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </asp:FormView>
        </fieldset>
    </div>
    <div id="rightColumn">
        <asp:LinkButton ID="lnkSearchAgain" runat="server" Text="Wyszukaj ponownie" 
          Visible="false" onclick="lnkSearchAgain_Click" />
        <esoda:ListaInteresantow Visible="false" ID="customersList" runat="server" />
        <asp:LinkButton ID="lnkSelectCustomer" runat="server" Text="Wybierz" 
          Visible="false" onclick="lnkSelectCustomer_Click" />
        <esoda:Interesant Visible="false" ID="customer" runat="server" />
    </div>
</asp:Content>