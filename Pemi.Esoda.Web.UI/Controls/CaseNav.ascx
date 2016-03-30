<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CaseNav.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.CaseNav" %>
<div id="caseNavigator" runat="server" visible="false" style="display:inline;">
<asp:LinkButton runat="server" ID="oczekujace" PostBackUrl="~/OczekujaceZadania.aspx" Text="Oczekuj¹ce zadania" />
<asp:LinkButton runat="server" ID="akta" PostBackUrl="~/Akta/AktaSpraw.aspx" Text="Akta spraw" />
 &raquo; 
<asp:LinkButton runat="server" ID="teczka" CommandName="~/Akta/AktaSpraw.aspx" Text="Teczka " visible="false" OnCommand="executeCommand" /> 
<asp:Literal runat="server" ID="separatorTeczki" Text="&raquo;" Visible="false" />
<asp:LinkButton runat="server" ID="sprawa" Text="Sprawa " CommandName="Sprawa" visible="false" OnCommand="redirectToCase" /> 
<asp:Literal runat="server" ID="separatorSprawy" Text="&raquo;" Visible="false" />
<asp:LinkButton runat="server" ID="dokument" Text="Dokument " CommandName="Dokument" visible="false" OnCommand="redirectToDocument" /> 
</div>