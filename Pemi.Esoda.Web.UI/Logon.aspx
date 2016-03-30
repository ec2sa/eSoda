<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
    CodeBehind="Logon.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Logon" Title="Logowanie do systemu e-S.O.D.A." %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>
        Logowanie do systemu</h2>
    <div style="text-align: left">
    <asp:Panel ID="pnlMessageOfTheDay" runat="server" CssClass="MessageOfTheDay" Visible="false">
            <asp:Literal ID="lblMessageOfTheDay" runat="server" />
    </asp:Panel>
        <div style="margin: 25px auto; width: 300px; clear: both;">
            <asp:Panel ID="pnlLogin" runat="server">                
                <asp:Login runat="server" ID="logonBox" FailureText="Nieprawid³owa nazwa u¿ytkownika lub has³o"
                    LoginButtonType="Link" PasswordRequiredErrorMessage="Trzeba podaæ has³o" DisplayRememberMe="False"
                    UserNameRequiredErrorMessage="Trzeba podaæ nazwê u¿ytkownika" TitleText="Proszê podaæ:"
                    LoginButtonText="Zaloguj" PasswordLabelText="Has³o:" UserNameLabelText="Nazwa u¿ytkownika:"
                    OnLoggedIn="logonBox_LoggedIn" BackColor="#edd8bd" BorderColor="#000000" BorderStyle="Solid"
                    BorderWidth="1px" Font-Names="Verdana" Font-Size="10pt" OnLoginError="logonBox_LoginError"
                    OnLoggingIn="logonBox_LoggingIn">
                    <TextBoxStyle BorderStyle="Solid" BorderWidth="1px" />
                    <LoginButtonStyle CssClass="link" />
                    <LayoutTemplate>
                        <table style="border-collapse: collapse" cellspacing="0" cellpadding="1" border="0">
                            <tbody>
                                <tr>
                                    <td>
                                        <table cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="font-weight: bold; border-color: #ffffff; color: white; background-color: #dca143;"
                                                        align="center" colspan="2">
                                                        Proszê podaæ:
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Nazwa u¿ytkownika:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="UserName" runat="server" BorderWidth="1px" BorderStyle="Solid"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ToolTip="Trzeba podaæ nazwê u¿ytkownika"
                                                            Display="Dynamic" ValidationGroup="ctl02$logonBox" ErrorMessage="Trzeba podaæ nazwê u¿ytkownika"
                                                            ControlToValidate="UserName">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Has³o:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Password" runat="server" BorderWidth="1px" BorderStyle="Solid" TextMode="Password"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ToolTip="Trzeba podaæ has³o"
                                                            Display="Dynamic" ValidationGroup="ctl02$logonBox" ErrorMessage="Trzeba podaæ has³o"
                                                            ControlToValidate="Password">*</asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="color: red" align="center" colspan="2">
                                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" colspan="2">
                                                        <asp:Button ID="btnLogin" runat="server" CommandName="Login" BorderStyle="None" BackColor="Transparent"
                                                            ValidationGroup="ctl02$logonBox" Width="2px" Height="1px"></asp:Button>
                                                        <asp:LinkButton ID="LoginLinkButton" runat="server" CommandName="Login" ValidationGroup="ctl02$logonBox"
                                                            CssClass="link">Zaloguj</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                <td colspan="3" align="center">
                                 <strong>Przyk³adowe dane logowania</strong><br />
                                 nazwa u¿ytkownika: <strong>admin</strong><br />
                                 has³o: <strong>12345678</strong></td>                                
                                </tr>--%>
                                            </tbody>
                                        </table>
                                </tr>
                            </tbody>
                        </table>
                    </LayoutTemplate>
                    <TitleTextStyle BackColor="#000FFF" BorderColor="Black" Font-Bold="False" ForeColor="White" />
                </asp:Login>
            </asp:Panel>
            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Logon as admin</asp:LinkButton>
        </div>
       
    </div>
</asp:Content>
