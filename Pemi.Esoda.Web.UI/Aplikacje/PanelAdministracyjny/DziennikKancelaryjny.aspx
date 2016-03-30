<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="DziennikKancelaryjny.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.DziennikKancelaryjny" Title="Zarzadzanie Dziennikiem Kancelaryjnym" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<uc1:OperacjePaneluAdministracyjnego ID="opa1" runat="server" />
<asp:Label runat="server" ID="ErrorMessage" ForeColor="Red" />
<fieldset>
<legend>Szczegó³y dziennika kancelaryjnego</legend>
<br />
<asp:Label ID="Label1" runat="server" Text="Importuj w³asn¹ regu³ê XSLT do transformacji XSL-FO:" />          
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:FileUpload ID="uploadXslFo" runat="server" />
        <asp:LinkButton ID="lnkAddXslFo" runat="server" onclick="lnkAddXslFo_Click" >Dodaj XSLT</asp:LinkButton>            
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="lnkAddXslFo" />
    </Triggers>
</asp:UpdatePanel>
<asp:Label ID="lblCustomXslFo" runat="server" ForeColor="Green" Text="Dziennik posiada w³asn¹ regu³ê XSLT do transformacji XSL-FO" Visible="False"/>
&nbsp;<asp:LinkButton ID="lnkRemoveXslFo" runat="server" onclick="lnkRemoveXslFo_Click" Visible="False"><span>Usuñ</span></asp:LinkButton>
<br />
<asp:LinkButton runat="server" ID="lnkSaveChanges" 
        Text="Zapisz informacje o dzienniku" onclick="lnkSaveChanges_Click" />
</fieldset>
<br />
<fieldset>
<legend>Dostêp do pozycji Dziennika Kancelaryjnego</legend>
<asp:CheckBox runat="server" ID="cbDailyLogItemAccessDenied" Text="tylko dla osób rezerwuj¹cych pozycje" /><br />
<asp:LinkButton runat="server" ID="lnkSave" Text="zapisz" onclick="lnkSave_Click" />
</fieldset>
</asp:Content>