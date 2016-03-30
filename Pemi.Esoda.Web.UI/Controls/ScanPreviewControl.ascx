<%@ Control EnableViewState="true" Language="C#" AutoEventWireup="true" CodeBehind="ScanPreviewControl.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.ScanPreviewControl" %>
<fieldset>
<legend>Podgląd skanu</legend>
<asp:UpdatePanel runat="server" ID="upScanPreview"><ContentTemplate>
<asp:LinkButton runat="server" ID="btnPrev" Text="&laquo;&laquo;" 
        ToolTip="Poprzednia strona" onclick="btnPrev_Click" />&nbsp;
        &nbsp;<asp:Literal runat="server" ID="currentPage" />
        &nbsp;
 <asp:LinkButton runat="server" ID="btnNext" Text="&raquo;&raquo;" 
        ToolTip="Następna strona" onclick="btnNext_Click" />
 <hr />
 <h4><asp:Label runat="server" ID="noPreviewLabel" Text="Brak podglądu skanu." Visible="false"/></h4>
 <div>
<asp:Image style="display:block;" runat="server" ID="imagePreview" />
</div>
</ContentTemplate></asp:UpdatePanel>
</fieldset>