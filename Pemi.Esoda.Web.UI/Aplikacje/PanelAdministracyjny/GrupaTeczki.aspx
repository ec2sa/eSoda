<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="GrupaTeczki.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.GrupaTeczki" Title="Podteczki" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="opa1" runat="server" />
<div style="margin-bottom: 5px;"><asp:LinkButton runat="server" PostBackUrl="~/Aplikacje/PanelAdministracyjny/edycjaTeczek.aspx" Text="Powrót" /></div>
<asp:Label runat="server" ID="lblMessage" ForeColor="Red" />

<fieldset>
<legend>Szczegó³y wybranej teczki</legend>
            <asp:GridView ID="gvBriefcaseList" DataSourceID="BriefcaseDataSource" CssClass="grid fullWidth" GridLines="None" 
            runat="server" AutoGenerateColumns="False"             
            OnRowDataBound="gvBriefcaseList_RowDataBound" >
                <AlternatingRowStyle CssClass="pozycjaNieparzysta" />
                <Columns>                    
                    <asp:BoundField DataField="tytul" HeaderText="Tytu³" />
                    <asp:BoundField DataField="symbolJRWA" HeaderText="JRWA" />
                    <asp:BoundField DataField="skrot" HeaderText="Kom. org." />
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Rodzaje spraw
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Literal ID="litCaseKindsList" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="rok" HeaderText="Rok" SortExpression="rok" />                    
                </Columns>
                <EmptyDataTemplate>
                    Brak wybranej teczki.
                </EmptyDataTemplate>                
            </asp:GridView>
</fieldset>            
            <asp:ObjectDataSource ID="BriefcaseDataSource" runat="server" SelectMethod="GetBriefcaseDetails" TypeName="Pemi.Esoda.DataAccess.BriefcaseDAO">            
            </asp:ObjectDataSource>
<fieldset>
<br />
<legend>Podteczki</legend>
<br />
<asp:Panel runat="server" ID="panelGroup">
<div>Wybierz podteczki:</div>
<asp:CheckBoxList runat="server" ID="BriefcaseGroup" DataTextField="Name"  
        DataValueField="ID" onprerender="BriefcaseGroup_PreRender"/>
<asp:LinkButton runat="server" ID="SaveChanges" Text="Zapisz" 
        onclick="SaveChanges_Click" />
</asp:Panel>
<div runat="server" id="dBriefcaseGroupEmpty" visible="false">Brak teczek, które mog¹ tworzyæ podteczki.</div>
</fieldset>
</asp:Content>