<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" AutoEventWireup="true" CodeBehind="error2.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Shared.error2" Title="B��d!" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<h2>Wyst�pi� b��d w trakcie dzia�ania aplikacji</h2>
<div class="errorBox">
<p>Komunikat b��du:</p>
<p class="errorMsg" runat="server" id="errorMessage" />
<asp:LinkButton runat="server" ID="returnFromError" Text="Powr�t" OnClick="returnFromError_Click" />
</div>
</asp:Content>

