<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master"
    AutoEventWireup="true" CodeBehind="KodyDokumentu.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Dokumenty.KodyDokumentu" %>

<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <esoda:ContextItem ID="ContextItem2" runat="server"></esoda:ContextItem>
    <h2>
        Przeglądanie kodów (DataMatrix) dokumentu</h2>
    <esoda:DocumentOperations runat="Server" ID="DocumentOperations1" />
    <hr />
    <fieldset>
        <legend>Generowanie kodów DataMatrix</legend>
        <table class="fullWidth">
            <tr>
                <td style="width: 20%; vertical-align: top; text-align: center;">
                    <h3 style="margin-bottom: 3em;">
                        Podgląd kodu</h3>
                    <img runat="server" id="imgPreview" alt="podgląd kodu datamatrix" /><br />
                    <asp:LinkButton CssClass="link" runat="server" ID="lnkGetAsPdf" Text="Pobierz PDF"
                        OnClick="lnkGetAsPdf_click" />
                </td>
                <td style="vertical-align: top;">
                    <fieldset>
                        <legend>Dane wysyłki</legend>
                        <table class="fullWidth layout">
                            <tr>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlZone" Enabled='false'>
                                        <asp:ListItem Value="1" Text="Europa" />
                                        <asp:ListItem Value="2" Text="Pozostałe" />
                                    </asp:DropDownList>
                                    <asp:CheckBoxList runat="server" ID="cbAdditions" />
                                </td>
                                <td>
                                    <asp:Label ID="Label1" CssClass="alignedLabelShort" runat="server" AssociatedControlID="txtName"
                                        Text="Nazwa adresata" />
                                    <asp:TextBox Style="width: 15em;" runat="server" ID="txtName" />
                                    <br />
                                    <asp:Label CssClass="alignedLabelShort" ID="Label2" runat="server" AssociatedControlID="txtAddress"
                                        Text="Adres (ulica, budynek, lokal)" />
                                    <asp:TextBox Style="width: 15em;" runat="server" ID="txtAddress" />
                                    <br />
                                    <asp:Label CssClass="alignedLabelShort" ID="Label3" runat="server" AssociatedControlID="txtPost"
                                        Text="Kod pocztowy, miejscowość" />
                                    <asp:TextBox Style="width: 15em;" runat="server" ID="txtPost" />
                                    <br />
                                    <asp:Label CssClass="alignedLabelShort" ID="Label4" runat="server" AssociatedControlID="txtAmount"
                                        Text="Kwota (gr.)" />
                                    <asp:TextBox Style="width: 15em;" runat="server" ID="txtAmount" />
                                    <asp:CompareValidator Display="Dynamic" ControlToValidate="txtAmount" runat="server"
                                        ID="amountValidator" Operator="DataTypeCheck" Type="Integer" ErrorMessage="podaj liczbę całkowitą!" />
                                    <br />
                                    <asp:Label CssClass="alignedLabelShort" ID="Label5" runat="server" AssociatedControlID="txtReceiving"
                                        Text="Pobranie (gr.)" />
                                    <asp:TextBox Style="width: 15em;" runat="server" ID="txtReceiving" />
                                    <asp:CompareValidator Display="Dynamic" ControlToValidate="txtReceiving" runat="server"
                                        ID="CompareValidator1" Operator="DataTypeCheck" Type="Integer" ErrorMessage="podaj liczbę całkowitą!" />
                                    <br />
                                    <asp:Label CssClass="alignedLabelShort" ID="Label6" runat="server" AssociatedControlID="txtNotes"
                                        Text="Uwagi" />
                                    <asp:TextBox Style="width: 15em;" runat="server" ID="txtNotes" />
                                    <br />
                                    <asp:Label CssClass="alignedLabelShort" ID="Label7" runat="server" AssociatedControlID="txtDepartment"
                                        Text="Dział" />
                                    <asp:TextBox runat="server" ID="txtDepartment" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset><legend>Wysyłka</legend>
                    <table class="fullWidth layout"><tr>
                    <td style="width:25%">
                    <asp:Button ID="Button2" runat="server" OnClick="btnGenerateOnly_click" Text="Wygeneruj kod" />
                     <asp:Button ID="Button1" runat="server" OnClick="btnGenerate_click" Text="Przekaż do wysyłania" />
                    
                    </td>
                    <td>
                    <h4 style="font-size:1.3em;margin:0;padding:0;">Status wysyłki:</h4>
                    <p class="statusWysylki"> <asp:Literal runat="server" ID="statusWysylki" Text="" /></p>
                    </td>
                    </tr></table>
                   
                        </fieldset>
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
    <br />
    <br />
    <fieldset>
        <legend>Historia zmian</legend>
        <asp:GridView runat="server" ID="historyGrid" AutoGenerateColumns="false" CssClass="grid">
            <Columns>
                <asp:BoundField DataField="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" HeaderText="Data" />
                <asp:TemplateField HeaderText="Kod">
                    <ItemTemplate>
                        <asp:Image runat="server" ID="imgCode" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Zawartość">
                    <ItemTemplate>
                        <asp:Literal runat="server" ID="lName" /><br />
                        <asp:Literal runat="server" ID="lAddress" /><br />
                        <asp:Literal runat="server" ID="lPost" /><br />
                        Kwota:
                        <asp:Literal runat="server" ID="lAmount" />
                        gr.&nbsp;&nbsp;&nbsp; Pobranie:
                        <asp:Literal runat="server" ID="lReceiving" />
                        gr.<br />
                        <asp:Literal runat="server" ID="lNotes" /><br />
                        <asp:Literal runat="server" ID="lDepartment" />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Status wysyłki">
                    <ItemTemplate>
                      <asp:Literal runat="server" ID="lStatusWysylki" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>
</asp:Content>
