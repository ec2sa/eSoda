<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="True"
    CodeBehind="HistoriaRejestru.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Rejestry.HistoriaRejestru"
    Title="Historia rejestru" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<div style="margin-bottom: 7px;"><asp:LinkButton runat="server" ID="lblGoBack" Text="Powrót" onclick="lblGoBack_Click"/></div>
<asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
<fieldset runat="server" id="fsRegistryHistory">
<legend>Historia rejestru</legend>
<table class="grid fullWidth">
<div runat="server" id="regContent"/>
</table>
</fieldset>
</asp:Content>


