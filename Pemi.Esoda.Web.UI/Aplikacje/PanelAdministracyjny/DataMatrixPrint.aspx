<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="DataMatrixPrint.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.DataMatrixPrint" Title="Konfiguracja strony wydruku kodu kreskowego"%>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="opa1" runat="server" />
<fieldset>
<legend>Konfiguracja strony wydruku kodu kreskowego</legend> 

<asp:Label ID="Label1" runat="server" AssociatedControlID="txtPageWidth" Text="Szerokość strony (w mm)" />
<br />
<asp:TextBox runat="server" ID="txtPageWidth" />
<br />
<asp:Label ID="Label2" runat="server" AssociatedControlID="txtPageHeight" Text="Wysokość strony (w mm)" />
<br />
<asp:TextBox runat="server" ID="txtPageHeight" />

<br /><br />
<asp:Label ID="Label5" runat="server" AssociatedControlID="txtPositionTop" Text="Umieszczenie kodu na stronie:" />
<br /><br />
<asp:Label ID="Label3" runat="server" AssociatedControlID="txtPositionTop" Text="Odległość od górnej krawędzi (w mm)" />
<br />
<asp:TextBox runat="server" ID="txtPositionTop" />
<br /><asp:Label ID="Label4" runat="server" AssociatedControlID="txtPositionLeft" Text="Odległość od lewej krawędzi (w mm)" />
<br />
<asp:TextBox runat="server" ID="txtPositionLeft" />
<br />
<asp:LinkButton runat="server" ID="zapisz" Text="Zapisz" OnClick="Zapisz_click" />
&nbsp;<asp:label runat="server" style="color:Green;" ID="msg" />
</fieldset>
</asp:Content>

