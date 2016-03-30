<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WykonanieCzynnosci.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Akcje.WykonanieCzynnosci" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" Title="Wykonanie czynności" %>
<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <esoda:ContextItem ID="ContextItem2" runat="server"></esoda:ContextItem>
    <h2>
        <%=Page.Title %>
    </h2>
    <esoda:DocumentOperations runat="Server" ID="DocumentOperations1" />
    <hr />
    <fieldset>
        <table class="grid fullWidth">
            <tr>
                <th>
                    Opis czynności
                </th>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="description" style="width:99%" MaxLength="250" />
                </td>
            </tr>
        </table>
        <asp:LinkButton runat="server" ID="save" Text="zapisz zmiany" OnClick="saveClick" />
        <asp:LinkButton runat="server" ID="cancel" Text="anuluj" OnClick="cancelClick" />
    </fieldset>
</asp:Content>
