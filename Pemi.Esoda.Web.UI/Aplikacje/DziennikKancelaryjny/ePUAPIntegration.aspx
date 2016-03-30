<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master"
    AutoEventWireup="true" CodeBehind="ePUAPIntegration.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.DziennikKancelaryjny.ePUAPIntegration" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>
        Sprawdzanie skrytki ePUAP</h2>
    <br />
    <asp:LinkButton ID="lnkBack" runat="server" Text="Powrót do przeglądania dziennika"
        OnClick="GoBack" CssClass="link" />
    <asp:LinkButton runat="server" ID="lbtnRefresh" Text="Odśwież" OnClick="lbtnRefresh_Click" />
    <asp:LinkButton runat="server" ID="lbtnGetFirstDocument" Text="Pobierz pierwszy dokument w kolejce"
        OnClick="lbtnGetFirstDocument_Click" />
    <br />
    <br />
    <asp:Label runat="server" ID="errorMsg" Text="" CssClass="errorMsg" />
    <br />
    <asp:Label runat="server" ID="lblBoxStatus" Text="" />
    <br />
    <br />
    <asp:GridView runat="server" ID="documentsGrid" CssClass="grid fullWidth" AutoGenerateColumns="false"
        OnRowCommand="documentsGrid_RowCommand">
        <Columns>
            <asp:BoundField HeaderText="Interesant" DataField="DocumentSenderName" />
            <asp:BoundField HeaderText="Data nadania" DataField="DocumentSendDate" />
            <asp:BoundField HeaderText="Nazwa pliku" DataField="DocumentName" />
            <asp:BoundField HeaderText="Typ pliku" DataField="DocumentType" />
            <asp:TemplateField HeaderText="Zawartość">
                <ItemTemplate>
                    <a href="ePUAPIntegrationFileDownload.aspx">Pobierz</a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:ButtonField HeaderText="Potwierdzenie" ButtonType="Link" CommandName="Confirm"
                Text="Potwierdź" />
            <asp:ButtonField HeaderText="Usuwanie" ButtonType="Link" CommandName="Delete" Text="Usuń"
                Visible="false" />
        </Columns>
    </asp:GridView>
</asp:Content>
