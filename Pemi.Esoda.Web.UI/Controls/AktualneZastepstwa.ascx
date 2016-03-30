<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AktualneZastepstwa.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.AktualneZastepstwa" %>
<asp:Panel runat="server" ID="availableCover" Visible="false">
<asp:Label runat="server" AssociatedControlID="ddlAvailableCover" Text="Aktualne zastępstwa" />
<asp:DropDownList runat="server" ID="ddlAvailableCover" DataTextField="UserFullName" DataValueField="UserLogin" />
<asp:LinkButton runat="server" ID="switch" Text="przełącz" onclick="switch_Click" />
</asp:Panel>