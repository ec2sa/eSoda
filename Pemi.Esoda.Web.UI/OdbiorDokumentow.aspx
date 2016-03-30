<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="OdbiorDokumentow.aspx.cs" Inherits="Pemi.Esoda.Web.UI.OdbiorDokumentow" Title="Dokumenty do odbioru" %>
<%@ Register src="~/Controls/ScanPreviewControl.ascx" tagname="ScanPreviewControl" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<center>
<div style="width: 400px;">
<fieldset>
<legend>Na Pani/Pana wydzia³</legend>
<asp:CheckBox ID="ckbAllWydzial" runat="server" Text="Zaznacz / odznacz wszystkie" 
        AutoPostBack="true" oncheckedchanged="ckbAllWydzial_CheckedChanged" />
<asp:GridView ID="gvDokumentyWydzialu" runat="server" DataKeyNames="id" CssClass="grid" AutoGenerateColumns="False" OnRowDataBound="gvDokumentyWydzialu_RowDataBound">
<Columns>
<asp:TemplateField>
<HeaderTemplate>Numer dziennika kancelaryjnego</HeaderTemplate>
<ItemTemplate><%# Eval("nr") %></ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<HeaderTemplate>Znak pisma</HeaderTemplate>
<ItemTemplate><%# Eval("znakPisma") %></ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<HeaderTemplate>Wybierz</HeaderTemplate>
<ItemTemplate><asp:Label ID="lblDocReceived" runat="server" Text="Odebrany" ></asp:Label><asp:CheckBox ID="ckbRecvDoc" runat="server" AutoPostBack="true" Checked="false" ToolTip="Do odbioru" /> </ItemTemplate>
</asp:TemplateField>
</Columns>
<EmptyDataTemplate>
Brak dokumentów do odbioru dla Pani/Pana wydzia³u.
</EmptyDataTemplate>
</asp:GridView>
</fieldset>
<br />
<fieldset>
<legend>Na Pani¹/Pana</legend>
<asp:CheckBox ID="ckbAllPracownik" runat="server" 
        Text="Zaznacz / odznacz wszystkie" AutoPostBack="true" 
        oncheckedchanged="ckbAllPracownik_CheckedChanged" />
<asp:GridView ID="gvDokumentyPracownika" runat="server" DataKeyNames="id" CssClass="grid" AutoGenerateColumns="False" OnRowDataBound="gvDokumentyPracownika_RowDataBound">
<Columns>
<asp:TemplateField>
<HeaderTemplate>Numer dziennika kancelaryjnego</HeaderTemplate>
<ItemTemplate><%# Eval("nr") %></ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<HeaderTemplate>Znak pisma</HeaderTemplate>
<ItemTemplate><%# Eval("znakPisma") %></ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<ItemTemplate>
<asp:LinkButton runat="server" ID="previewScanLink" Text="Podgl¹d" CommandArgument='<%# Eval("id") %>' CommandName="preview" OnCommand="previewScanCommand" />
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<HeaderTemplate>Wybierz</HeaderTemplate>
<ItemTemplate><asp:Label ID="lblDocReceived" runat="server" Text="Odebrany"></asp:Label><asp:CheckBox ID="ckbRecvDoc" runat="server" Checked="false" ToolTip="Do odbioru" /> </ItemTemplate>
</asp:TemplateField>
</Columns>
<EmptyDataTemplate>
Brak dokumentów do odbioru dla Pani/Pana.
</EmptyDataTemplate>
</asp:GridView>
</fieldset>

<table>
<tr><td><asp:Label ID="lblEnterPin" runat="server" Text="WprowadŸ numer PIN:"></asp:Label></td><td><asp:TextBox ID="txtPIN" runat="server" Width="100px" TextMode="Password"></asp:TextBox></td><td><asp:LinkButton ID="lnkConfirmReceive" runat="server" Text="Potwierdzam odbiór" OnClick="lnkConfirmReceive_Click"></asp:LinkButton></td></tr>
    <tr>
        <td align="center" colspan="3">
            <asp:Label ID="lblError" runat="server" Visible="False"></asp:Label></td>
    </tr>
</table>
</div>
<asp:LinkButton runat="server" ID="openScan" Text="Pokaz" style="display:none" />
<ajax:ModalPopupExtender 
runat="server" 
RepositionMode="None"
ID="scanPreviewModal" 
TargetControlID="openScan" 
PopupControlID="scanPreviewPanel" 
OkControlID="closeScanPreview"
 BackgroundCssClass="modalBackground"
 PopupDragHandleControlID="scanPreviewPanel"  />
<asp:Panel runat="server" ID="scanPreviewPanel" CssClass="modalPopup">
<asp:LinkButton runat="server" ID="closeScanPreview" Text="Zamknij" />
<hr />
<uc1:ScanPreviewControl runat="server" ID="scan" />
</asp:Panel>
</center>
</asp:Content>
