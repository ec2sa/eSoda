<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" AutoEventWireup="true" CodeBehind="EdycjaRejestru.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Akcje.EdycjaRejestru" Title="Edycja pozycji rejestru" %>
<%@ Register src="../Controls/ObjectDetailsView.ascx" tagname="ObjectDetailsView" tagprefix="uc1" %>
<%@ Register src="../Controls/ScanPreviewControl.ascx" tagname="ScanPreviewControl" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<h1>Dodawanie/Edycja pozycji rejestru</h1>
<asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
<table><tr>
<td style="width:50%;vertical-align: top;">
<fieldset id="fs" runat="server" class="registryEditor">
<legend>
Dodawanie / Edycja pozycji rejestru
</legend>
    <asp:Panel runat="server" id="fieldContainer">    
    </asp:Panel>
</fieldset>
</td>
<td style="vertical-align: top;"><fieldset class="registryEditor">
<legend>Szczegóły <%=this.ObjectType %></legend>
    <uc1:ObjectDetailsView ID="ObjectDetailsViewC" runat="server" />
    <hr />
    <uc1:ScanPreviewControl ID="ScanPreviewControl1" runat="server" />
</fieldset></td>
</tr></table>



<br />
<asp:LinkButton ID="lblSave" runat="server" CssClass="link" Text="Zapisz" ValidationGroup="registryItem" OnClick="saveItem" />
<asp:LinkButton ID="lblCancel" runat="server" CssClass="link" Text="Anuluj" 
        onclick="lblCancel_Click" />
</asp:Content>

