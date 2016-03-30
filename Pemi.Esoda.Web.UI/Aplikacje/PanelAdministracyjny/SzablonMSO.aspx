<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master"
    AutoEventWireup="true" CodeBehind="SzablonMSO.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.SzablonMSO"
    Title="Szablon dokumentu MS Office" %>

<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <uc1:OperacjePaneluAdministracyjnego ID="opa1" runat="server" />
    <div style="margin-bottom: 5px;">
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" /></div>
    <fieldset>
        <legend>Szablon dokumentu MS Office</legend>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" AssociatedControlID="wordTemplate" Text="Szablon dokumentu MS Office:" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="wordTemplate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel runat="server" ID="currentWordTemplate" Visible="false">
                        Bie¿¹cy szablon:
                        <asp:Label runat="server" ForeColor="Green" ID="lblWordOriginalFileName" /><asp:HiddenField
                            runat="server" ID="hfWordFileName" />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="isWordTemplateActive" Text="Aktywny" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="lnkAddWordTemplate" runat="server" OnClick="lnkAddWordTemplate_Click">zapisz szablon</asp:LinkButton>
                </td>
            </tr>
        </table>
    </fieldset>
    <br />
    <fieldset runat="server" visible="false">
        <legend><b>Zabezpieczony</b> szablon dokumentu MS Office</legend>
        <br />
        <table>
            <tr>
                <td>
                    <span><b>Zabezpieczony</b> szablon dokumentu MS Office:</span>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="wordSecureTemplate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel runat="server" ID="currentWordSecureTemplate" Visible="false">
                        Bie¿¹cy <b>zabezpieczony</b> szablon:
                        <asp:Label runat="server" ForeColor="Green" ID="lblWordSecureOriginalFileName" /></asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="lnkAddWordSecureTemplate" runat="server" OnClick="lnkAddWordSecureTemplate_Click">zapisz szablon</asp:LinkButton>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>Akty prawne</legend>
        <br />
        Bie¿¹ce ustawienia:
        <table>
            <tr>
                <td>
                    <asp:Label runat="server" ForeColor="Green" ID="lblLegalActsSettingsVersion" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton runat="server" ID="lnkCurrentSchema" OnClick="lnkCurrentSchema_Click"
                        Text="Bie¿¹ca schema" Visible="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton runat="server" ID="lnkCurrentXslt" OnClick="lnkCurrentXslt_Click"
                        Text="Bie¿¹ce przekszta³cenie XSLT" Visible="false" />
                </td>
            </tr>
        </table>
        </asp:Panel>
        <table>
            <tr>
                <td>
                    <span>Schema aktów prawnych:</span>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="fuLegalActsSchema" runat="server" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <span>XSLT aktów prawnych:</span>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="fuLegalActsXslt" runat="server" />
                </td>
            </tr>
        </table>
        <asp:LinkButton ID="lnkSaveLegalActsSettings" runat="server" OnClick="lnkSaveLegalActsSettings_Click">zapisz ustawienia aktów prawnych</asp:LinkButton>
    </fieldset>
    <br />
    <fieldset>
        <legend>Czas wa¿noœci biletu</legend>
        <div style="margin: 2px;">
            <div>
                <asp:Label ID="Label1" runat="server" Text="Domyœlny czas wa¿noœci biletu:" AssociatedControlID="ticketDuration" />
                <asp:TextBox runat="server" ID="ticketDuration" MaxLength="4" Width="30px" />
                min.</div>
            <div>
                <asp:CompareValidator runat="server" ID="TicketDurationValidotor" ControlToValidate="ticketDuration"
                    Operator="GreaterThan" ValueToCompare="0" Type="Integer" ErrorMessage="Nieprawid³owa wartoœæ!" /></div>
            <div>
                <asp:LinkButton runat="server" ID="lblSaveTicketDuration" Text="zapisz" OnClick="lblSaveTicketDuration_Click" /></div>
        </div>
    </fieldset>
    <br />
    <fieldset>
        <legend>Czas wa¿noœci przeterminowanego biletu</legend>
        <div style="margin: 2px;">
            <div>
                <asp:Label ID="Label3" runat="server" Text="Liczba dni:" AssociatedControlID="ticketLifeTime" />
                <asp:TextBox runat="server" ID="ticketLifeTime" MaxLength="4" Width="30px" /></div>
            <div>
                <asp:CompareValidator runat="server" ID="CompareValidator1" ControlToValidate="ticketLifeTime"
                    Operator="GreaterThan" ValueToCompare="0" Type="Integer" ErrorMessage="Nieprawid³owa wartoœæ!" /></div>
            <div>
                <asp:LinkButton runat="server" ID="lblSaveTicketLifeTime" Text="zapisz" OnClick="lblSaveTicketLifeTime_Click" /></div>
        </div>
    </fieldset>
</asp:Content>
