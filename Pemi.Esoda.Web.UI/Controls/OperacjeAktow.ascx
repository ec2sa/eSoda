<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacjeAktow.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.OperacjeAktow" %>
<ul class="opcje">
<li><asp:LinkButton ID="lbEdytujAkt" runat="server" onCommand="wykonaj" CommandName="EditLegalAct" Text="Edycja Aktu prawnego"/></li>
<li><asp:LinkButton ID="lblXml" runat="server" onCommand="wykonaj" CommandName="GetXml" Text="xml"/></li>
<li><asp:LinkButton ID="lblHistory" runat="server" onCommand="wykonaj" CommandName="GetHistory" Text="historia"/></li>
</ul>