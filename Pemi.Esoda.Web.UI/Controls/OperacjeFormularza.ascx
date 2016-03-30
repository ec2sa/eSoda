<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacjeFormularza.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.OperacjeFormularza" %>
<ul class="opcje">
<li><asp:LinkButton ID="lblESodaForm" runat="server" onCommand="wykonaj" Text="formularz eSoda"/></li>
<li><asp:LinkButton ID="lblWordForm" runat="server" onCommand="wykonaj" Text="formularz MS Word"/></li>
<li><asp:LinkButton ID="lblWordEditForm" runat="server" onCommand="wykonaj" Text="edycja formularza MS Word"/></li>
<li><asp:LinkButton ID="lblXml" runat="server" onCommand="wykonaj" Text="xml"/></li>
<li><asp:LinkButton ID="lblHistory" runat="server" onCommand="wykonaj" Text="historia"/></li>
<li><asp:LinkButton ID="lblepuap" runat="server" onCommand="wykonaj" Text="nadaj [skrytka ePUAP]" CommandName="nadajepuap"/></li>
</ul><br /><br />
<asp:Label runat="server" ID="lblMessage"/>