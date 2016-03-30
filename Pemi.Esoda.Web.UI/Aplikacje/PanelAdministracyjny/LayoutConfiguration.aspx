<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" AutoEventWireup="true" CodeBehind="LayoutConfiguration.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.LayoutConfiguration" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="opa1" runat="server" />

<fieldset>
<legend>Konfiguracja szaty graficznej</legend>

<asp:Label runat="server" AssociatedControlID="uploadLogo1" Text="Logo 1 (lewe)" /><br />
<asp:FileUpload runat="server" ID="uploadLogo1" /><asp:Button ID="Button1" runat="server" Text="Zapisz" CommandName="saveLogo1" OnCommand="setConfig"/> 
<asp:Button ID="Button3" runat="server" Text="Usuń" CommandName="deleteLogo1" OnCommand="setConfig"/>
<br />
<asp:Label ID="Label1" runat="server" AssociatedControlID="uploadLogo2" Text="Logo 2 (prawe)" /><br />
<asp:FileUpload runat="server" ID="uploadLogo2" /><asp:Button ID="Button2" runat="server" Text="Zapisz" CommandName="saveLogo2" OnCommand="setConfig"/>
<asp:Button ID="Button4" runat="server" Text="Usuń" CommandName="deleteLogo2" OnCommand="setConfig"/>
<br />
<asp:Label ID="Label2" runat="server" AssociatedControlID="bgColor" Text="Kolor tła (format: #rrggbb)" /><br />
<asp:TextBox runat="server" ID="bgColor" /><asp:Button runat="server" Text="Zapisz" CommandName="saveBG" OnCommand="setConfig"/>
<asp:Button ID="Button5" runat="server" Text="Usuń" CommandName="deleteBG" OnCommand="setConfig"/>
<br />
<br />
<asp:Button runat="server" Text="Przywróć ustawienia domyślne" CommandName="setDefault"  OnCommand="setConfig" />
&nbsp;<asp:label runat="server" style="color:Green;" ID="msg" />
</fieldset>
</asp:Content>
