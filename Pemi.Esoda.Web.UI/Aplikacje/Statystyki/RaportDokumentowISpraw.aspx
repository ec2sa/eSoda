<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="RaportDokumentowISpraw.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.Statystyki.RaportDokumentowISpraw" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<h2>Raport dokumentów i spraw dla wybranego wydziału i pracownika</h2>
<div id="printContent">
<fieldset>
<legend>Parametry raportu</legend>
<asp:Label runat="server" AssociatedControlID="ddlWydzialy" Text="Wydział" /><br />
<asp:DropDownList id="ddlWydzialy" runat="server" DataSourceID="wydzialyDS" DataValueField="ID" DataTextField="Description" AutoPostBack="true" />
<asp:CompareValidator runat="server" ControlToValidate="ddlWydzialy" ValueToCompare="0" Operator="NotEqual" Text="Trzeba wybrać wydział." Display="Dynamic" />
<br />
<asp:Label ID="Label1" runat="server" AssociatedControlID="ddlPracownicy" Text="Pracownik" /><br />
<asp:DropDownList ID="ddlPracownicy" runat="server" DataSourceID="pracownicyDS" DataValueField="ID" DataTextField="Description" /><br />
<h4>Zakres dat dla dokumentów:</h4>
<asp:Label runat="server" AssociatedControlID="txtDataOd" Text="Data początkowa" /><br />
<asp:TextBox runat="server"  ID="txtDataOd" />
<ajax:CalendarExtender ID="txtDataOd_CalendarExtender" Enabled="true" runat="server"
                                                TargetControlID="txtDataOd" /><br />
<asp:Label ID="Label2" runat="server" AssociatedControlID="txtDataOd" Text="Data końcowa" /><br />
<asp:TextBox runat="server"  ID="txtDataDo" />
<ajax:CalendarExtender ID="txtDataDo_CalendarExtender" Enabled="true" runat="server"
                                                TargetControlID="txtDataDo" /><br />
                                                
<br />
<asp:LinkButton runat="server" Text="Generuj raport" OnClick="generujRaport" id="lbGenerujRaport"/>                                                
 </fieldset>
 <fieldset>
 <legend>Zawartość raportu</legend>
 <asp:GridView ID="raportGrid" runat="server" AllowPaging="false" AllowSorting="false" CssClass="grid fullWidth" AutoGenerateColumns="false">
 <Columns>
 <asp:BoundField DataField="Pracownik" HeaderText="Pracownik" />
 <asp:BoundField DataField="DokumentyUtworzone" HeaderText="Dokumenty utworzone" />
 <asp:BoundField DataField="DokumentyWplywajace" HeaderText="Dokumenty otrzymane" />
 <asp:TemplateField>
 <HeaderTemplate>Sprawy zakończone / w tym po terminie realizacji (w bieżącym roku)</HeaderTemplate>
 <ItemTemplate>
<%# Eval("Zamkniete") %> / <%# Eval("ZamknietePoTerminie") %></ItemTemplate>
 </asp:TemplateField>
  <asp:TemplateField>
 <HeaderTemplate>Sprawy otwarte / w tym po terminie realizacji (w bieżącym roku)</HeaderTemplate>
 <ItemTemplate><%# Eval("Otwarte") %> / <%# Eval("OtwartePoTerminie") %></ItemTemplate>
 </asp:TemplateField>
 </Columns>
 </asp:GridView>
 </fieldset>
</div>
<asp:ObjectDataSource runat="server" ID="wydzialyDS" 
    DataObjectTypeName="Pemi.Esoda.DTO.SimpleLookupDTO" 
    TypeName="Pemi.Esoda.Tasks.EditRegistryItemTask"
    SelectMethod="GetOrganizationalUnits" />
    <asp:ObjectDataSource runat="server" ID="pracownicyDS"
    DataObjectTypeName="Pemi.Esoda.DTO.SimpleLookupDTO" 
    TypeName="Pemi.Esoda.Tasks.EditRegistryItemTask"
    SelectMethod="GetEmployees">
    <SelectParameters>
    <asp:ControlParameter ControlID="ddlWydzialy" PropertyName="SelectedValue" DefaultValue="0" Name="organizationalUnitId" />
    </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

