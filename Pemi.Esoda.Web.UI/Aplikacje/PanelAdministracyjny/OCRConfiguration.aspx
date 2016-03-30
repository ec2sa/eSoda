<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="OCRConfiguration.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.OCRConfiguration" Title="Konfiguracja OCR" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="opa1" runat="server" />
    <fieldset>
        <legend>Konfiguracja OCR</legend>
        <table>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblServiceRunning" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Godzina rozpoczęcia skanowania (np. 18:00)</td>
                <td><asp:TextBox ID="tbOCRStart" runat="server" /></td>
            </tr>
            <tr>
                <td>Godzina zakończenia skanowania (np. 24:00)</td>
                <td><asp:TextBox ID="tbOCREnd" runat="server" /></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton runat="server" ID="zapisz" Text="Zapisz" OnClick="Zapisz_click" />
                    &nbsp;
                    <asp:label runat="server" style="color:Green;" ID="msg" />
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>Logi OCR</legend>
        
        Pokaż logi z ostatnich <asp:TextBox ID="tbDays" runat="server" Text="7" /> dni
        <asp:LinkButton ID="lbtnShow" runat="server" Text="Pokaż" OnClick="lbtnShow_Click" />
        
        <asp:GridView runat="server" ID="gvLogs" CssClass="grid" AutoGenerateColumns="false">
            <EmptyDataTemplate>Brak logów</EmptyDataTemplate>
            <Columns>
                <asp:BoundField HeaderText="Data logu" DataField="LogDate" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Przeskanowane skany" DataField="OCRed" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Przeskanowane strony" DataField="ScansPagesOCRed" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Niemożliwe do zeskanowania" DataField="UnOCRable" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Pozostałe do zeskanowania" DataField="ScansRemainedToOCR" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Wszystkie" DataField="Total" ItemStyle-HorizontalAlign="Center" />
            </Columns>
        </asp:GridView>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
