<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentItemWordUploader.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.DocumentItemWordUploader" %>
<div>
<fieldset>
<legend>Utworzenie pliku Word dla dokumentu</legend>
<div style="padding: 5px;">
<div><asp:Label runat="server" ID="lblDescription" AssociatedControlID="txtDescription">Opis</asp:Label></div>
<div><asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Columns="40" Rows="4" /></div>
<div style="padding-top: 5px;"><asp:LinkButton runat="server" id="lblOpenFile" 
        Text="utwórz plik" onclick="lblOpenFile_Click" />&nbsp;<asp:LinkButton 
        runat="server" id="lblCancel" Text="anuluj" onclick="lblCancel_Click" /></div>
</div>
</fieldset>
</div>