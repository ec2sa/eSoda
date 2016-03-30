<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" AutoEventWireup="true" CodeBehind="ZarzadzanieAkcjami.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.ZarzadzanieAkcjami" Title="Zarz¹dzanie akcjami" %>
<%@ Register Src="../../Controls/OperacjePaneluAdministracyjnego.ascx" TagName="OperacjePaneluAdministracyjnego"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">

<uc1:OperacjePaneluAdministracyjnego ID="OperacjePaneluAdministracyjnego1" runat="server" />
    
    <asp:MultiView runat="server" ID="mv1" ActiveViewIndex="0">
      <asp:View runat="server" ID="stage1">
      <asp:GridView runat="server" ID="actions" DataSourceID="listaAkcjiDS" CssClass="grid" AutoGenerateColumns="False"
       DataKeyNames="id" OnRowCommand="actions_RowCommand" >
        <SelectedRowStyle BackColor="Red" CssClass="wybrany" />
    <AlternatingRowStyle CssClass="pozycjaNieparzysta" />
        <Columns>
        <asp:BoundField DataField="number" HeaderText="numer" SortExpression="numer" />
          <asp:BoundField DataField="nazwa" HeaderText="nazwa" SortExpression="nazwa" />
          <asp:BoundField DataField="opis" HeaderText="opis" SortExpression="opis" />
          <asp:CommandField SelectText="dodaj XSLT" ShowSelectButton="True" ></asp:CommandField>
          <asp:TemplateField>
          <ItemTemplate>
          <asp:LinkButton runat="server" CommandName="exportXslt" Text="pobierz XSLT" CommandArgument='<%# Eval("id")%>' />
          </ItemTemplate>
          </asp:TemplateField>
        </Columns>
      </asp:GridView>
      <asp:ObjectDataSource runat="server" ID="listaAkcjiDS" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.ActionsTableAdapters.ListaAkcjiDAO" UpdateMethod="Update" >
        <UpdateParameters>
          <asp:Parameter Name="idDefinicjiAkcji" Type="Object" />
          <asp:Parameter Name="xslt" Type="Object" />
        </UpdateParameters>
      </asp:ObjectDataSource>
       </asp:View>
      <asp:View runat="server" ID="stage2">
      <asp:Label ID="Label1" runat="server" Text="Wybierz plik xslt" AssociatedControlID="uploadControl" />
        <asp:FileUpload runat="server" ID="uploadControl" />
        <asp:Button ID="Button1" runat="server" OnClick="uploadXslt" Text="Zapisz" /><br />
        <asp:LinkButton runat="server" ID="powrot" Text="powrót do listy" OnClick="powrotDoListy" />
      </asp:View>
      <asp:View runat="server">
      <p class="success">Zapis wykonany poprawnie.</p>
      <asp:LinkButton runat="server" ID="LinkButton1" Text="powrót do listy" OnClick="powrotDoListy" />
      </asp:View>
      <asp:View ID="View1" runat="server">
      <p class="errorMsg">Nie uda³o siê zapisaæ pliku xslt.</p>
      <asp:LinkButton runat="server" ID="LinkButton2" Text="powrót do listy" OnClick="powrotDoListy" />
      </asp:View>
    </asp:MultiView>

</asp:Content>