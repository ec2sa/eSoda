<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="SMSConfig.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.SMSConfig" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="opa1" runat="server" />
<fieldset>
<legend>Konfiguracja powiadomień sms dla interesantów </legend> 

<asp:Label runat="server" AssociatedControlID="txtSender" Text="Nadawca" />
<br />
<asp:TextBox runat="server" ID="txtSender" />
<br />
<asp:Label ID="Label1" runat="server" AssociatedControlID="txtUser" Text="Login" />
<br />
<asp:TextBox runat="server" ID="txtUser" />
<br />
<asp:Label ID="Label2" runat="server" AssociatedControlID="txtPassword" Text="Hasło" />
<br />
<asp:TextBox runat="server" ID="txtPassword" />
<br />
<asp:Label ID="Label3" runat="server" AssociatedControlID="txtTemplate" Text="Szablon wiadomości" />
<br />
<asp:TextBox runat="server" ID="txtTemplate" TextMode="MultiLine" Rows="4" Columns="40"/>
<br />

<asp:CheckBox runat="server" ID="cbIsFlash" Text="Flash SMS" TextAlign="Right" /><br />
<asp:CheckBox runat="server" ID="cbIsTest" Text="Tylko testowanie komunikacji" TextAlign="Right" /><br />
<br />
<asp:LinkButton runat="server" ID="zapisz" Text="Zapisz" OnClick="Zapisz_click" />
&nbsp;<asp:label runat="server" style="color:Green;" ID="msg" />
</fieldset>
</asp:Content>

