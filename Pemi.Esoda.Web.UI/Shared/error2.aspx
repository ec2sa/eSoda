<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" AutoEventWireup="true" CodeBehind="error2.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Shared.error2" Title="B³¹d!" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<h2>Wyst¹pi³ b³¹d w trakcie dzia³ania aplikacji</h2>
<div class="errorBox">
<p>Komunikat b³êdu:</p>
<p class="errorMsg" runat="server" id="errorMessage" />
<asp:LinkButton runat="server" ID="returnFromError" Text="Powrót" OnClick="returnFromError_Click" />
</div>
</asp:Content>

