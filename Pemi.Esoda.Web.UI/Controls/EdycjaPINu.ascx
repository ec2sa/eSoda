<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EdycjaPINu.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.EdycjaPINu" %>
<table>
<tr>
<th align="right"><asp:Label ID="lblPIN" runat="server" Text="WprowadŸ nowy PIN:" AssociatedControlID="txtPIN"></asp:Label></th>
<td><asp:TextBox ID="txtPIN" runat="server" TextMode="Password" MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator
        ID="rfvPIN" runat="server" ControlToValidate="txtPIN" Display="Dynamic" ErrorMessage="*"
        ValidationGroup="ValidatePIN">wymagane</asp:RequiredFieldValidator>
    <br />
    <asp:CustomValidator ID="cuvPIN" runat="server" ControlToValidate="txtPIN" Display="Dynamic"
        ErrorMessage="*" OnServerValidate="cuvPIN_ServerValidate" ValidationGroup="ValidatePIN"></asp:CustomValidator></td>
</tr>
<tr>
<th align="right"><asp:Label ID="lblPIN2" runat="server" Text="Powtórz PIN:" AssociatedControlID="txtPIN2"></asp:Label></th>
<td><asp:TextBox ID="txtPIN2" runat="server" TextMode="password" MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator
        ID="rfvPIN2" runat="server" ControlToValidate="txtPIN2" Display="Dynamic" ErrorMessage="*"
        ValidationGroup="ValidatePIN">wymagane</asp:RequiredFieldValidator><br />
    <asp:CompareValidator ID="cvPIN" runat="server" ControlToCompare="txtPIN" ControlToValidate="txtPIN2"
        Display="Dynamic" ValidationGroup="ValidatePIN">Wprowadzone PINy musz¹ byæ takie same</asp:CompareValidator></td>
</tr>
<tr>
<th align="right"><asp:Label ID="lblConfirm" Visible="false" runat="server" Text="PotwierdŸ PIN has³em:" AssociatedControlID="txtPINConfirm"></asp:Label></th>
<td><asp:TextBox ID="txtPINConfirm" Visible="false" runat="server" 
        TextMode="Password" MaxLength="128"></asp:TextBox><br />
    <asp:RequiredFieldValidator ID="rfvConfirmPass" runat="server" ControlToValidate="txtPINConfirm"
        Display="Dynamic" ErrorMessage="*" ValidationGroup="ValidatePIN">zmiana PINu wymaga potwierdzenia has³em</asp:RequiredFieldValidator></td>
</tr>
    <tr>
        <td align="center" colspan="2">
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
        </td>
    </tr>
<tr><td colspan="2" align="center">
<asp:LinkButton ID="lnkSavePIN" runat="server" OnClick="lnkSavePIN_Click" CommandName="UpdatePIN" ValidationGroup="ValidatePIN">Zapisz</asp:LinkButton></td>
</tr>
</table>