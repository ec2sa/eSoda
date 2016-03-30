<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="DniWolneOdPracy.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.DniWolneOdPracy" Title="Dni wolne od pracy" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="opa1" runat="server" />
<asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
<div style="padding: 5px;float: left;">
<fieldset>
<legend>Dzieñ wolny od pracy</legend>
<div style="padding: 5px 5px 50px 5px;">
<div><asp:ValidationSummary runat="server" ID="ValidationSummary" ValidationGroup="changeGroup" DisplayMode="List" /></div>
<div>Dzieñ wolny:</div>
<div>
<asp:TextBox runat="server" ID="holidayDate" />
<asp:RequiredFieldValidator runat="server" ID="holidayDateRequiredValidator" ValidationGroup="changeGroup" ControlToValidate="holidayDate" ErrorMessage="data wymagana" Text="*" />
<asp:CompareValidator runat="server" ID="holidayDateCompareValidator" Type="Date" Operator="DataTypeCheck" ValidationGroup="changeGroup" ControlToValidate="holidayDate" ErrorMessage="z³y format daty" Text="*" />
<ajax:CalendarExtender runat="server" ID="CalendarExtender" TargetControlID="holidayDate" ></ajax:CalendarExtender>
</div>
<div>Opis:</div>
<div><asp:TextBox runat="server" ID="description" TextMode="MultiLine" Rows="5" Columns="30" /></div>
<div><%--<asp:LinkButton runat="server" ID="delete" Text="usuñ" OnClick="delete_Click" />&nbsp;--%><asp:LinkButton runat="server" ID="new" Text="nowy" OnClick="new_Click" />&nbsp;<asp:LinkButton runat="server" ID="save" Text="zapisz" onclick="save_Click" ValidationGroup="changeGroup"/></div>
<asp:HiddenField runat="server" ID="holidayDateID" />
</div>
</fieldset>
</div>
<div style="padding: 5px; float: left;">
<fieldset>
<legend>Dni wolne od pracy</legend>
<div style="padding: 5px">
<div><span>Dni wolne w roku:</span> <asp:DropDownList runat="server" ID="availableYears" AutoPostBack="True" 
        ontextchanged="availableYears_TextChanged" DataTextField="Description" DataValueField="ID" /></div>
<asp:GridView runat="server" ID="holidayDateList" CssClass="grid" 
        AutoGenerateColumns="false" onrowcommand="holidayDateList_RowCommand">
<EmptyDataTemplate>Brak</EmptyDataTemplate>
<Columns>
<asp:TemplateField HeaderText="Lp." ItemStyle-HorizontalAlign="Center">
<ItemTemplate>
<%#Container.DataItemIndex+1 %>.
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Data">
<ItemTemplate>
<asp:Label runat="server" ID="date" Text='<%#Bind("Date","{0:yyyy-MM-dd}") %>' />
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="Description" HeaderText="Opis" />
<asp:TemplateField>
<ItemTemplate><asp:LinkButton runat="server" ID="hdID" CommandName="hdID" CommandArgument='<%#Bind("ID") %>' Text="wybierz" /></ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</div>
</fieldset>
</div>
</asp:Content>