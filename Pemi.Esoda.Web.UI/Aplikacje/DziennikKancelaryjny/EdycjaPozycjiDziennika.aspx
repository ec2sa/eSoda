<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
    CodeBehind="EdycjaPozycjiDziennika.aspx.cs" Inherits="Pemi.Esoda.Web.UI.EdycjaPozycjiDziennika"
    Title="Edycja pozycji dziennika kancelaryjnego" EnableEventValidation="false" %>

<%@ Register Src="~/Controls/Interesant.ascx" TagName="Interesant" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ListaInteresantow.ascx" TagName="ListaInteresantow"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ESPDocumentPreview.ascx" TagName="ESPDocPreview" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">

    <div id="singleColumn">
        <asp:Timer ID="Timer1" runat="server" Interval="100" Enabled="true" OnTick="Timer1_Tick" />
        <h2>
            Edycja pozycji rejestru
            <asp:Literal runat="server" ID="numerPozycji" /></h2>
        <asp:LinkButton ID="lnkBack" runat="server" Text="Powrót do przegl¹dania dziennika"
            OnClick="GoBack" CssClass="link" /></div>
    <hr />
<asp:Panel runat="server" ID="contentPanel">
    <div id="leftColumn" style="width: 55%!important;">
        <asp:UpdatePanel runat="server" ID="up1">
            <ContentTemplate>
                <div id="podstawoweOpcjeDziennika">
                    <asp:FormView runat="server" ID="pozycja" OnItemCommand="obslugaZdarzen" OnDataBound="podpiecieDat"
                        DefaultMode="Edit" OnPageIndexChanging="pozycja_PageIndexChanging" CssClass="fullWidth">
                        <EmptyDataTemplate>
                            Wybrana pozycja nie zawiera ¿adnych danych
                        </EmptyDataTemplate>
                        <InsertItemTemplate>
                            <fieldset class="pozycjaRejestruRW">
                                <legend>Edycja pozycji</legend>
                                <h2 runat="server" visible="false">
                                    Zarezerwowany numer: <span class="zarezerwowanyNumer">
                                        <asp:Literal runat="server" ID="zarezerwowanyNumer" /></span></h2>
                                <table>
                                    <tr>
                                        <th>
                                            <asp:Label ID="Label2" runat="server" AssociatedControlID="fdataWplywu" Text="Data wp³ywu" />
                                        </th>
                                        <th>
                                            <asp:Label ID="Label1" runat="server" AssociatedControlID="fdataPisma" Text="Data pisma" />
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Calendar runat="server" ID="fdataWplywu" Visible="false" />
                                            <asp:TextBox ID="txtDataWplywu" runat="server" Visible="true" />
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtDataWplywu"
                                                ValidationGroup="SaveGroup" Operator="DataTypeCheck" Type="Date" ErrorMessage="Niepoprawna data"
                                                Text="*" />
                                            <ajax:CalendarExtender ID="txtDataWplywu_CalendarExtender" Enabled="true" runat="server"
                                                TargetControlID="txtDataWplywu" />
                                        </td>
                                        <td>
                                            <asp:Calendar runat="server" ID="fdataPisma" Visible="false" />
                                            <asp:TextBox ID="txtDataPisma" runat="server" Visible="true" />
                                            <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="txtDataPisma"
                                                ValidationGroup="SaveGroup" Operator="DataTypeCheck" Type="Date" ErrorMessage="Niepoprawna data"
                                                Text="*" />
                                            <ajax:CalendarExtender ID="txtDataPisma_CalendarExtender" runat="server" Enabled="true"
                                                TargetControlID="txtDataPisma" />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <uc1:Interesant ID="customer" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <% 
                                            if (!IsInvoice)
                                               { %>
                                            <asp:Label ID="Label3" runat="server" AssociatedControlID="fnumerPisma" Text="Znak pisma" />
                                            <%}
                                               else
                                               { %>
                                               <asp:Label runat="server" Text="Numer faktury" AssociatedControlID="fnumerPisma" /><br />
                                               <asp:Label ID="Label7" runat="server" Text="Kwota faktury" AssociatedControlID="fKwotaFaktury" />
                                               
                                            <%} %>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="fnumerPisma" Text='' /><br />
                                             <%if (IsInvoice)
                                               { %>
                                            <asp:TextBox runat="server" ID="fKwotaFaktury" Text='' />
                                            <%} %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" AssociatedControlID="fopis" Text="Opis" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="fopis" Text='' TextMode="MultiLine" Rows="4" Width="99%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label11" runat="server">Klasyfikacja<br />dokumentu</asp:Label>
                                        </td>
                                        <td>
                                            <fieldset>
                                                <asp:Label ID="label50" runat="server" AssociatedControlID="fkategoria" Text="Kategoria" /><br />
                                                <asp:DropDownList runat="server" ID="fkategoria" OnSelectedIndexChanged="obslugaZmianyKategoriiDokumentu"
                                                    AutoPostBack="true" />
                                                <br />
                                                <asp:Label ID="label19" runat="server" AssociatedControlID="frodzajDokumentu" Text="Rodzaj" /><br />
                                                <asp:DropDownList runat="server" ID="frodzajDokumentu" />
                                                <br />
                                                <asp:TextBox runat="server" ID="frodzajWartosc" Text="" /></fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" AssociatedControlID="ftypKorespondencji" Text="Typ koresp." />
                                        </td>
                                        <td>
                                            <fieldset>
                                                <asp:DropDownList runat="server" ID="ftypKorespondencji" />
                                                <br />
                                                <asp:TextBox runat="server" ID="ftypKorespondencjiWartosc" Text="" /></fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" AssociatedControlID="fuwagi" Text="Uwagi" />
                                        </td>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" Rows="4" Width="99%" runat="server" ID="fuwagi"
                                                Text='' />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>
                                            <asp:Label ID="Label10" runat="server" AssociatedControlID="fdodatkoweMaterialy" Text="Dodatkowe niezdigitalizowane materia³y" />
                                        </td>
                                        <td>
                                       <fieldset>
                                        <asp:CheckBox runat="server" ID="cbDodatkoweMaterialy" Text="zawiera dodatkowe niezdigitalizowane materia³y"  onclick="var f=document.getElementById('ctl00_main_pozycja_fdodatkoweMaterialy');f.disabled=!this.checked;if(!this.checked) f.value='';" /> <br />
                                        <asp:TextBox runat="server" ID="fdodatkoweMaterialy" style="width:95%" enabled='false' title="lista w formacie [typ]-[numer]. np.: CD-123; DVD-0393; DVD-0232" />
                                        <asp:RegularExpressionValidator runat="server" ID="dodatkoweMaterialyValidator" ControlToValidate="fdodatkoweMaterialy" ValidationGroup="SaveGroup" ErrorMessage="niepoprawny format listy dodatkowych materia³ów!" ValidationExpression="^([a-zA-Z]{1,4}-[0-9]{1,5})+(([ ]*[;][ ]*[a-zA-Z]{1,4}-[0-9]{1,5}){0,1})+$" />
                                        </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <fieldset>
                                                <legend>Znak referenta</legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label6" runat="server" AssociatedControlID="fwydzialZnakReferenta"
                                                                Text="Wydzia³" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="fwydzialZnakReferenta" AutoPostBack="true" OnSelectedIndexChanged="obslugaZmianyWydzialu" />
                                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="fwydzialZnakReferenta"
                                                                ValidationGroup="SaveGroup" InitialValue="0" ErrorMessage="Nale¿y wybraæ wydzia³!"
                                                                Text="*" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label9" runat="server" AssociatedControlID="fpracownikZnakReferenta"
                                                                Text="Pracownik" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="fpracownikZnakReferenta" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                                <asp:LinkButton CssClass="link" runat="Server" ID="op1" CommandName="zapisz" Text="Zapisz"
                                    ValidationGroup="SaveGroup" />
                            </fieldset>
                        </InsertItemTemplate>
                        <EditItemTemplate>
                            <fieldset class="pozycjaRejestruRW">
                                <legend>Edycja pozycji </legend>
                                <table>
                                    <tr>
                                        <th>
                                            <asp:Label ID="Label2" runat="server" AssociatedControlID="fdataWplywu" Text="Data wp³ywu" />
                                        </th>
                                        <th>
                                            <asp:Label ID="Label1" runat="server" AssociatedControlID="fdataPisma" Text="Data pisma" />
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Calendar runat="server" ID="fdataWplywu" Visible="false" />
                                            <asp:TextBox ID="txtDataWplywu" runat="server" Visible="true" />
                                            <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="txtDataWplywu"
                                                ValidationGroup="SaveGroup" Operator="DataTypeCheck" Type="Date" ErrorMessage="Niepoprawna data"
                                                Text="*" />
                                            <ajax:CalendarExtender ID="txtDataWplywu_CalendarExtender" Enabled="true" runat="server"
                                                TargetControlID="txtDataWplywu" />
                                        </td>
                                        <td>
                                            <asp:Calendar runat="server" ID="fdataPisma" Visible="false" />
                                            <asp:TextBox ID="txtDataPisma" runat="server" Visible="true" />
                                            <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="txtDataPisma"
                                                ValidationGroup="SaveGroup" Operator="DataTypeCheck" Type="Date" ErrorMessage="Niepoprawna data"
                                                Text="*" />
                                            <ajax:CalendarExtender ID="txtDataPisma_CalendarExtender" runat="server" Enabled="true"
                                                TargetControlID="txtDataPisma" />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                <tr>
                                        <td colspan="2">
                                            <uc1:Interesant ID="customer" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        
                                            <%--<asp:Label ID="Label3" runat="server" AssociatedControlID="fnumerPisma" Text="Znak pisma" />                --%>         
                                       <% 
                                            if (!IsInvoice)
                                               { %>
                                            <asp:Label ID="Label3" runat="server" AssociatedControlID="fnumerPisma" Text="Znak pisma" />
                                            <%}
                                               else
                                               { %>
                                               <asp:Label ID="Label8" runat="server" Text="Numer faktury" AssociatedControlID="fnumerPisma" /><br />
                                               <asp:Label ID="Label7" runat="server" Text="Kwota faktury" AssociatedControlID="fKwotaFaktury" />
                                               
                                            <%} %>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="fnumerPisma" Text='<%# XPath("numerPisma")%>' /><br />
                                             <%if (IsInvoice)
                                               { %>
                                            <asp:TextBox runat="server" ID="fKwotaFaktury" Text='<%# XPath("kwota")%>' />
                                            <%} %>
                                                             
                                        
                                        </td>
                                        
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" AssociatedControlID="fopis" Text="Opis korespondencji" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="fopis" Text='<%# XPath("opis")%>' TextMode="MultiLine"
                                                Rows="4" Width="99%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label11" runat="server">Klasyfikacja<br />dokumentu</asp:Label>
                                        </td>
                                        <td>
                                            <fieldset>
                                                <asp:Label ID="label50" runat="server" AssociatedControlID="fkategoria" Text="Kategoria" /><br />
                                                <asp:DropDownList runat="server" ID="fkategoria" OnSelectedIndexChanged="obslugaZmianyKategoriiDokumentu"
                                                    AutoPostBack="true" />
                                                <br />
                                                <asp:Label ID="label19" runat="server" AssociatedControlID="frodzajDokumentu" Text="Rodzaj" /><br />
                                                <asp:DropDownList runat="server" ID="frodzajDokumentu" />
                                                <br />
                                                <asp:TextBox runat="server" ID="frodzajWartosc" Text='<%# XPath("klasyfikacjaDokumentu/wartosc")%>' /></fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" AssociatedControlID="ftypKorespondencji" Text="Typ korespndencji" />
                                        </td>
                                        <td>
                                            <fieldset>
                                                <asp:DropDownList runat="server" ID="ftypKorespondencji" />
                                                <br />
                                                <asp:TextBox runat="server" ID="ftypKorespondencjiWartosc" Text='<%# XPath("typKorespondencji/wartosc")%>' /></fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" AssociatedControlID="fuwagi" Text="Uwagi" />
                                        </td>
                                        <td>
                                            <asp:TextBox TextMode="MultiLine" Rows="4" Width="99%" runat="server" ID="fuwagi"
                                                Text='<%# XPath("uwagi")%>' />
                                        </td>
                                    </tr>
                                      <tr>
                                        <td>
                                            <asp:Label ID="Label10" runat="server" AssociatedControlID="fdodatkoweMaterialy" Text="Dodatkowe niezdigitalizowane materia³y" />
                                        </td>
                                        <td>
                                       <fieldset>
                                        <asp:CheckBox runat="server" ID="cbDodatkoweMaterialy" Text="zawiera dodatkowe niezdigitalizowane materia³y" Checked='<%# XPath("dodatkoweMaterialy/@zawiera")!=null && XPath("dodatkoweMaterialy/@zawiera").ToString()=="tak"?true:false%>' onclick="var f=document.getElementById('ctl00_main_pozycja_fdodatkoweMaterialy');f.disabled=!this.checked;if(!this.checked) f.value='';" /> <br />
                                        <asp:TextBox runat="server" ID="fdodatkoweMaterialy" Text='<%# XPath("dodatkoweMaterialy")%>' style="width:95%" enabled='<%# XPath("dodatkoweMaterialy/@zawiera")!=null && XPath("dodatkoweMaterialy/@zawiera").ToString()=="tak"?true:false%>' title="lista w formacie [typ]-[numer]. np.: CD-123; DVD-0393; DVD-0232" />
                                        <asp:RegularExpressionValidator runat="server" ID="dodatkoweMaterialyValidator" ControlToValidate="fdodatkoweMaterialy" ValidationGroup="SaveGroup" ErrorMessage="niepoprawny format listy dodatkowych materia³ów!" ValidationExpression="^([a-zA-Z]{1,4}-[0-9]{1,5})+(([ ]*[;][ ]*[a-zA-Z]{1,4}-[0-9]{1,5}){0,1})+$" />
                                        </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <fieldset>
                                                <legend>Znak referenta</legend>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label6" runat="server" AssociatedControlID="fwydzialZnakReferenta"
                                                                Text="Wydzia³" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="fwydzialZnakReferenta" AutoPostBack="true" OnSelectedIndexChanged="obslugaZmianyWydzialu" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="SaveGroup"
                                                                ControlToValidate="fwydzialZnakReferenta" InitialValue="0" ErrorMessage="Nale¿y wybraæ wydzia³!"
                                                                Text="*" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label9" runat="server" AssociatedControlID="fpracownikZnakReferenta"
                                                                Text="Pracownik" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="fpracownikZnakReferenta" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                                <asp:LinkButton CssClass="link" runat="Server" ID="op1" CommandName="zapisz" Text="Zapisz zmiany"
                                    ValidationGroup="SaveGroup" />&nbsp;
                                <asp:LinkButton ID="LinkButton1" runat="server" Text="Anuluj" OnClick="GoBack" CssClass="link" />
                            </fieldset>
                        </EditItemTemplate>
                    </asp:FormView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="SaveGroup"
            DisplayMode="List" />
    </div>
    <div id="rightColumn" style="width: 45%!important; margin-top: 86px; vertical-align: top;">
        <uc1:ListaInteresantow ID="customersList" runat="server" Visible="false" />
        <asp:UpdatePanel ID="up4" runat="server">
            <ContentTemplate>
                <uc1:ESPDocPreview ID="docPreview" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:Panel ID="pnlDocumentPreview" runat="server" Visible="false">
            <asp:Xml ID="xmlDocumentData" runat="server" />
            <fieldset>
                <legend>Za³¹czniki:</legend>
                <asp:Repeater ID="rptAttachments" runat="server">
                    <HeaderTemplate>
                        <table>
                            <tr>
                                <th>
                                    Lp.
                                </th>
                                <th>
                                    Nazwa
                                </th>
                                <th>
                                    Rozmiar
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Container.ItemIndex+1 %>
                            </td>
                            <td>
                                (nazwa)
                            </td>
                            <td>
                                (rozmiar)
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </fieldset>
        </asp:Panel>
    </div>
</asp:Panel>
<asp:Label runat="server" ID="lblDailyLogItemAccessDeniedInfo" ForeColor="Red" Text="Brak mo¿liwoœci edycji!" />
</asp:Content>
