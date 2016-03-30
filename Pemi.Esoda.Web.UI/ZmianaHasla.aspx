<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="ZmianaHasla.aspx.cs" Inherits="Pemi.Esoda.Web.UI.ZmianaHasla" Title="Zmiana has³a" %>
<%@ Register Src="~/Controls/EdycjaPINu.ascx" TagName="EdycjaPinu" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<center>
<table>
<tr>
<td>    <asp:ChangePassword ID="ChangePassword1" runat="server" CancelDestinationPageUrl="~/OczekujaceZadania.aspx" ContinueDestinationPageUrl="~/OczekujaceZadania.aspx" OnChangingPassword="ChangePassword1_ChangingPassword">
        <ChangePasswordTemplate>
        
                    <fieldset>
                    <legend>Zmiana has³a</legend>
                    
                        <table border="0" cellpadding="0">
                            
                            <tr>
                                <td align="right">
                                    <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Stare has³o:</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                                        Display="Dynamic" ErrorMessage="Password is required." ToolTip="Password is required."
                                        ValidationGroup="ctl00$ChangePassword1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">Nowe has³o:</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                        Display="Dynamic" ErrorMessage="New Password is required." ToolTip="New Password is required."
                                        ValidationGroup="ctl00$ChangePassword1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Powtórz nowe has³o:</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                        Display="Dynamic" ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
                                        ValidationGroup="ctl00$ChangePassword1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                                        ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="Powtórzone has³o musi byæ identyczne jak nowe has³o"
                                        ValidationGroup="ctl00$ChangePassword1"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color: red">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    
                                        <asp:LinkButton ID="lnkChangePasswordLink" runat="server" CommandName="ChangePassword"
                                        Text="Zapisz" ValidationGroup="ctl00$ChangePassword1" />
                                </td>
                                <td>
                                   
                                        <asp:LinkButton ID="lnkCancelPasswordChange" runat="server" CommandName="Cancel"
                                        Text="Anuluj" />
                                </td>
                            </tr>
                        </table>
                        </fieldset>
        </ChangePasswordTemplate>
        <SuccessTemplate>
            <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse;">
                <tr>
                    <td>
                        <table border="0" cellpadding="0">
                           <tr>
                                <td>
                                    Has³o zosta³o pomyœlnie zmienione.</td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Button ID="ContinuePushButton" runat="server" CausesValidation="False" CommandName="Continue"
                                        Text="Continue" Visible="false" />
                                        <asp:LinkButton ID="lnkContinue" runat="server" CausesValidation="false" CommandName="Continue" Text="Kontynuuj" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </SuccessTemplate>
    </asp:ChangePassword>
</td>
<td><fieldset>
    <legend>Zmiana kodu PIN</legend>
    <uc1:EdycjaPinu ID="pinEdit" runat="server" />
    </fieldset></td>
</tr>
</table>
    
    
    </center>
</asp:Content>