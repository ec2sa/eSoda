<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" CodeBehind="importUzytkownikow.aspx.cs" Inherits="Pemi.Esoda.Web.UI.importUzytkownikow" Title="Import pracowników i ich przypisañ do wydzia³ów" %>
<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
<div id="singleColumn">
    <fieldset>
    <legend>Wska¿ plik zawieraj¹cy potrzebne dane o u¿ytkownikach i wydzia³ach</legend>
        <asp:FileUpload ID="fuplImport" runat="server" Width="600px" /><br />
        <asp:LinkButton runat="server" ID="lnkImport" Text="importuj" OnClick="importuj" /><br />
        <asp:Literal ID="litResults" runat="server"></asp:Literal>
        </fieldset>
        <br />
        </div>
 </asp:Content>