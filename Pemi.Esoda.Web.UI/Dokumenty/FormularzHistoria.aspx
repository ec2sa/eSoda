<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" AutoEventWireup="true" CodeBehind="FormularzHistoria.aspx.cs" Inherits="Pemi.Esoda.Web.UI.FormularzHistoria" Title="Historia danych formularza" %>
<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<%@ Register TagName="DocPreview" TagPrefix="esoda" Src="~/Controls/ESPDocumentPreview.ascx" %>

<%@ Register src="../Controls/OperacjeFormularza.ascx" tagname="OperacjeFormularza" tagprefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">

<esoda:ContextItem id="ContextItem1" runat="server"></esoda:ContextItem>
<h2>Historia danych formularza</h2>
<esoda:DocumentOperations runat="Server" ID="opcjeDokumentu1" />
<hr />
<uc1:OperacjeFormularza ID="OperacjeFormularza" runat="server" />    
<asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
<asp:GridView runat="server" ID="historyList"  CssClass="grid" 
        AutoGenerateColumns="false" onrowcommand="historyList_RowCommand">
<EmptyDataTemplate>Brak historii</EmptyDataTemplate>
<Columns>        
        <asp:TemplateField HeaderText="Lp." ItemStyle-HorizontalAlign="Center">
        <ItemTemplate>
        <%#Container.DataItemIndex+1 %>
        </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Data" DataField="Date" />
        <asp:BoundField HeaderText="U¿ytkownik" DataField="Username" />
        <asp:TemplateField>
        <ItemTemplate>
        <asp:LinkButton runat="server" ID="lblXml" Text="xml" CommandArgument='<%#Eval("ItemID") %>' CommandName="selectedItem" />
        </ItemTemplate>
        </asp:TemplateField>
</Columns>
</asp:GridView>
</asp:Content>
