<%@ Page Language="C#" Async="true" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="DokumentyOczekujace.aspx.cs" Inherits="Pemi.Esoda.Web.UI.DokumentyOczekujace" Title="Dokumenty oczekujące" %>
<%@ Register Src="~/Controls/ESPDocumentPreview.ascx" TagName="ESPDocPreview" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<div id="singleColumn">
<asp:LinkButton ID="lnkCheckESPForDocuments" runat="server" 
        Text="Pobież listę dokumentów elektronicznych" 
        onclick="lnkCheckESPForDocuments_Click" />
</div>
<div id="leftColumn">
<fieldset>
<legend>Lista dokumentów elektroncznych</legend>
<asp:GridView ID="gvESPDocumentsList" CssClass="grid fullWidth" runat="server" AllowPaging="True" 
        AllowSorting="True" DataSourceID="odsDocumentsESP" 
        AutoGenerateColumns="False" 
        onrowdatabound="gvESPDocumentsList_RowDataBound" 
        onrowcommand="gvESPDocumentsList_RowCommand">               
                  <Columns>
                      <asp:TemplateField HeaderText="Lp.">
                      <ItemTemplate><%# Container.DataItemIndex +1 %>. </ItemTemplate>
                      </asp:TemplateField>
                      <asp:BoundField HeaderText="Id" SortExpression="idDokumentu" DataField="idDokumentu" />
                      <asp:BoundField HeaderText="Nazwa" SortExpression="nazwa" DataField="nazwa" />
                      <asp:BoundField HeaderText="Opis" SortExpression="opis" DataField="opis" />
                      <asp:BoundField HeaderText="Status" SortExpression="status" DataField="status" />
                      <asp:TemplateField HeaderText="operacje">
                      <ItemTemplate>
                      <asp:LinkButton ID="lnkDownload" runat="server" Text="Pobierz dokument" Visible="false" CommandName="download" CommandArgument='<%# Eval("guidESP") %>' />
                      <asp:LinkButton ID="lnkPreview" runat="server" Text="Podgląd wniosku" Visible="false" CommandName="preview" CommandArgument='<%# Eval("guidESP") %>' />
                      </ItemTemplate>
                      </asp:TemplateField>
                      
    </Columns>
<EmptyDataTemplate>
Brak nowych/niewprowadzonych dokumentów.
</EmptyDataTemplate>
</asp:GridView>
    <asp:ObjectDataSource ID="odsDocumentsESP" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="GetDocumentsESPList" TypeName="Pemi.Esoda.DataAccess.DocumentDAO">
        <SelectParameters>
            <asp:Parameter Name="sortParam" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</fieldset>
</div>
<div id="rightColumn">
<uc1:ESPDocPreview ID="docPreview" runat="server" />
<asp:LinkButton ID="lnkAddDocument" runat="server" 
        Text="Dodaj do dziennika kancelaryjnego" />
</div>
</asp:Content>