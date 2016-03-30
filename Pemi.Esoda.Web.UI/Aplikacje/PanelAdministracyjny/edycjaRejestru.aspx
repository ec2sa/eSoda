<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="edycjaRejestru.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.edycjaRejestru" Title="Szczegóły rejestru" %>
<%@ Register Src="~/Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracjnego" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/OperacjeRejestrow.ascx" TagName="OperacjeRejestrow" TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <uc1:OperacjePaneluAdministracjnego ID="OperacjePanelu" runat="server" />
    <uc1:OperacjeRejestrow ID="OperacjeRejestrow1" runat="server" Visible="true" />
    <div id="leftColumn">
        <asp:FormView ID="frmRejestr" runat="server">
            <ItemTemplate>
                <fieldset>
                    <legend>Szczegóły rejestru</legend>
                </fieldset>
            </ItemTemplate>
            <InsertItemTemplate>
                <fieldset>
                    <legend>Dodawanie nowego rejestru</legend>
                </fieldset>
            </InsertItemTemplate>
            <EditItemTemplate>
                <fieldset>
                    <legend>Edycja rejestru</legend>
                </fieldset>
            </EditItemTemplate>
        </asp:FormView>
        <fieldset>
            <legend>Szczegóły rejestru</legend>
            <asp:Label ID="lblZarzadzanieRejestrem" runat="server" Text="Zarządzanie rejestrem:" />
            <asp:CheckBox ID="ckbActive" runat="server" Checked="true" Text="Aktywny" />
            <asp:CheckBox ID="ckbArchive" runat="server" Checked="false" Text="Archiwalny" />
            <br />
            <hr />
            <asp:CheckBox ID="ckbShowEntryDate" runat="server" Checked="false" Text="Pokaż datę wpisu" />
            <br />
            <asp:CheckBox ID="ckbShowAddingUser" runat="server" Checked="false" Text="Pokaż dane tworzącego" />
            <hr />
            <asp:Label runat="server" ID="lblRok" AssociatedControlID="txtRok" Text="Rok:" />
            <asp:TextBox runat="server" ID="txtRok" Width="45px"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRok" ErrorMessage="wymagane" ValidationGroup="Rejestr" />
            <asp:CompareValidator runat="server" ControlToValidate="txtRok" Operator="DataTypeCheck" Type="Integer" ErrorMessage="zły format roku" ValidationGroup="Rejestr" />            
            <br />
            <asp:Label ID="lblNazwaRejestru" runat="server" Text="Nazwa rejestru:" />
            <asp:TextBox ID="txtNazwaRejestru" runat="server" Width="191px" 
                MaxLength="80" />
            <asp:RequiredFieldValidator ID="rfvNazwaRejestru" runat="server" 
        ErrorMessage="*" ValidationGroup="Rejestr" 
        ControlToValidate="txtNazwaRejestru">wymagane</asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lnlJRWA" runat="server" Text="JRWA:" />
            <asp:TextBox ID="txtJRWA" 
        runat="server" Width="350px" />
            <asp:RequiredFieldValidator ID="rfvJRWARejestru" runat="server" 
        Display="Dynamic" ErrorMessage="*" ValidationGroup="Rejestr" 
        ControlToValidate="txtJRWA">wymagane</asp:RequiredFieldValidator>
            <br />
            <asp:LinkButton ID="lnkSelectJRWA" runat="server" Text="Wybierz JRWA..." 
        onclick="lnkSelectJRWA_Click" />
            <asp:CheckBox ID="ckbSetJRWA" runat="server" AutoPostBack="true" Checked="false" Text="wyłącz" 
        oncheckedchanged="ckbSetJRWA_CheckedChanged" />
            <asp:HiddenField ID="hfJRWAId" runat="server" />
            <br />
            <hr />
            <asp:Label ID="lblTypWpisu" runat="server" Text="Typ przechowywanych wpisów:" />
            <asp:DropDownList ID="ddlTypWpisu" runat="server" AutoPostBack="true"
        onselectedindexchanged="ddlTypWpisu_SelectedIndexChanged">
                <asp:ListItem Value="-1" Selected="True">- wybierz -</asp:ListItem>
                <asp:ListItem Text="Dokumenty" Value="doc" />
                <asp:ListItem Text="Sprawy" Value="case" />
                
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rvfTypWpisow" runat="server" ErrorMessage="*" 
        ControlToValidate="ddlTypWpisu" ValidationGroup="Rejestr" InitialValue="-1">wymagane</asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lblRodzjeWpisow" runat="server" Text="Rodzaje przechowywanych wpisów" />
            <br />
            <asp:ListBox ID="lstRodzajeWpisow" runat="server" />
            <asp:CustomValidator ID="cusWpisy" runat="server" Display="Dynamic" ErrorMessage="*" 
        onservervalidate="cusWpisy_ServerValidate" ValidationGroup="Rejestr">określ rodzaje wpisów przechowywanych w rejestrze</asp:CustomValidator>
            <asp:Panel ID="pnlDocTypes" runat="server" Visible="False">
                <asp:Label ID="lblDocCat" runat="server" Text="Kategoria dokumentu:" />
                <asp:DropDownList ID="ddlDocCat" runat="server" 
        onselectedindexchanged="ddlDocCat_SelectedIndexChanged" AutoPostBack="true" />
                <br />
                <asp:Label ID="lblDocType" runat="server" Text="Typ dokumentu:" />
                <asp:DropDownList ID="ddlDocType" runat="server" />
                <br />
            </asp:Panel>
            <asp:Panel ID="pnlCaseTypes" runat="server" Visible="False">
                <asp:Label ID="lblCaseType" runat="server" Text="Wybierz rodzaj sprawy" AssociatedControlID="ddlCaseTypes" />
                <asp:DropDownList ID="ddlCaseTypes" runat="server" />
            </asp:Panel>
            <asp:LinkButton ID="lnkDodajRodzaj" runat="server" Text="Dodaj do listy" 
        onclick="lnkDodajRodzaj_Click" />
            <br />
            <asp:LinkButton ID="lnkUsunRodzaj" runat="server" Text="Usuń rodzaj z listy" 
        onclick="lnkUsunRodzaj_Click" />
            <hr />
            <asp:Label ID="lblMainGroup" runat="server" Text="Wydział prowadzący rejestr:" AssociatedControlID="ddlMainGroup" />
            <asp:DropDownList ID="ddlMainGroup" runat="server" />
            <asp:RequiredFieldValidator ID="rfvMainGroup" runat="server" 
        ControlToValidate="ddlMainGroup" ValidationGroup="Rejestr" Display="Dynamic" 
        ErrorMessage="*" InitialValue="-1" >wymagane</asp:RequiredFieldValidator>
            <hr />
            <asp:Label ID="lblReadableGroups" runat="server" Text="Wydziały o prawie podglądu rejestru:" />
            <br />
            <asp:ListBox ID="lstRedableGroups" runat="server" />
            <br />
            <asp:Label ID="lblWydzial" runat="server" Text="Wybierz wydział:" />
            <br />
            <asp:DropDownList ID="ddlListaWydzialow" runat="server" />
            <br />
            <asp:LinkButton ID="lnkDodajWydzial" runat="server" Text="Dodaj wydział" 
        onclick="lnkDodajWydzial_Click" />
            <br />
            <asp:LinkButton ID="lnkUsunWydzial" runat="server" Text="Usuń wybrany wydział" 
        onclick="lnkUsunWydzial_Click" />
            <hr />
            <asp:Label ID="lblImport" runat="server" Text="Importuj własną regułę XSLT:" />
            <asp:UpdatePanel ID="upUploadXSLT" runat="server">
                <ContentTemplate>
                    <asp:FileUpload ID="fuplXSLT" runat="server" />
                    <asp:LinkButton ID="lnkDodajXSLT" runat="server" onclick="lnkDodajXSLT_Click" >Dodaj XSLT</asp:LinkButton>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lnkDodajXSLT" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:Label ID="lblCustomXSLT" runat="server" 
        Text="Rejestr posiada własną regułę XSLT" Visible="False"></asp:Label>
&nbsp;<asp:LinkButton ID="lnkRemoveXSLT" runat="server" 
        onclick="lnkRemoveXSLT_Click" Visible="False"><span>Usuń</span></asp:LinkButton>
            <br />
            <hr />
            <asp:Label ID="Label1" runat="server" Text="Importuj własną regułę XSLT do transformacji XSL-FO:" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:FileUpload ID="uploadXslFo" runat="server" />
                    <asp:LinkButton ID="lnkAddXslFo" runat="server" onclick="lnkAddXslFo_Click" >Dodaj XSLT</asp:LinkButton>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lnkAddXslFo" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:Label ID="lblCustomXslFo" runat="server" Text="Rejestr posiada własną regułę XSLT do transformacji XSL-FO" Visible="False"></asp:Label>
            &nbsp;<asp:LinkButton ID="lnkRemoveXslFo" runat="server" onclick="lnkRemoveXslFo_Click" Visible="False"><span>Usuń</span></asp:LinkButton>
            <br />
            <asp:CustomValidator ID="cvDefinicjaRejestru" runat="server" Display="Dynamic" 
        ErrorMessage="*" onservervalidate="cvDefinicjaRejestru_ServerValidate" 
        ValidationGroup="Rejestr">nie utworzono definicji rejestru</asp:CustomValidator>
            <br />
            <asp:LinkButton ID="lnkZapiszRejestr" runat="server" 
        Text="Zapisz informacje o rejestrze" onclick="lnkZapiszRejestr_Click" 
        ValidationGroup="Rejestr" />
        </fieldset>
    </div>
    <div id="rightColumn">
        <asp:Panel ID="pnlJRWA" runat="server" Visible="False" HorizontalAlign="Left">
            <fieldset>
                <legend>Drzewo JRWA</legend>
                <asp:LinkButton ID="lnkAddJRWA" runat="server" OnClick="lnkAddJRWA_Click" Enabled="False" Visible="False">Wstaw do teczki</asp:LinkButton>
                <br />
                <asp:LinkButton ID="lnkCloseJRWA" runat="server" OnClick="lnkCloseJRWA_Click">Ukryj</asp:LinkButton>
                <br />
                <asp:TreeView ID="tvJRWA" runat="server" AutoGenerateDataBindings="False" DataSourceID="dsxmlJRWA" OnSelectedNodeChanged="tvJRWA_SelectedNodeChanged" ImageSet="Arrows">
                    <DataBindings>
                        <asp:TreeNodeBinding DataMember="jrwaItem" TextField="opis" ValueField="id" />
                    </DataBindings>
                    <SelectedNodeStyle CssClass="treeSelectedNode"/>
                </asp:TreeView>
                <asp:XmlDataSource ID="dsxmlJRWA" runat="server" XPath="jrwaTree/jrwaItem" EnableCaching="false">
                </asp:XmlDataSource>
            </fieldset>
        </asp:Panel>
    </div>
</asp:Content>