<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="NowyRok.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.NowyRok" Title="Nowy rok" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="opa1" runat="server" />
<asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
<fieldset>
<legend>Nowy rok</legend>
<div style="margin: 3px;">
<div><asp:Label runat="server" AssociatedControlID="newYearAvailableAction" Text="Wybierz operacje:" /></div>
<div>
<asp:DropDownList runat="server" ID="newYearAvailableAction">
<asp:ListItem Text="-- wybierz --" Value="-1" />
<asp:ListItem Text="Twórz teczki na nowy rok" Value="1"/>
<asp:ListItem Text="Twórz rejestry na nowy rok" Value="2"/>
</asp:DropDownList>
<asp:LinkButton runat="server" Text="wykonaj" ID="execAction" onclick="execAction_Click" />
</div>
</div>
</fieldset>
</asp:Content>