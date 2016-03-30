<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="True"
    CodeBehind="ListaRejestrow.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Rejestry.ListaRejestrow"
    Title="Lista i przeglądanie zawartości dostępnych rejestrów" %>

<%--<%@ Register Src="../Controls/KryteriaWyszukiwaniaWRejestrze.ascx" TagName="KryteriaWyszukiwaniaWRejestrze"
  TagPrefix="uc1" %>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>
        <%=Page.Title%>
    </h2>
    <div style="width: 20%; float: left;">
            <asp:LinkButton ID="lnkRegistry" Visible="false" runat="server" Text="Powrót" PostBackUrl="~/Aplikacje/PanelAdministracyjny/listaRejestrow.aspx" ToolTip="Zarządzanie rejestrami" />

            <% if (Roles.RoleExists("RejestryCentralne")) { %>
            
           
            <table class="grid">
           <tr><th>Rejestry centralne</th></tr>
             <tr><td><asp:LinkButton ID="LinkButton1" runat="server" CommandName="1" OnCommand="goToRC" Text="Rejestr Decyzji" /></td></tr>
             <tr><td><asp:LinkButton ID="LinkButton2" runat="server" CommandName="2" OnCommand="goToRC" Text="Rejestr Postanowień" /></td></tr>
             <tr><td><asp:LinkButton ID="LinkButton3" runat="server" CommandName="3" OnCommand="goToRC" Text="Rejestr Umów" /></td></tr>
             <tr><td><asp:LinkButton ID="LinkButton4" runat="server" CommandName="4" OnCommand="goToRC" Text="Rejestr Zarządzeń" /></td></tr>
            
            </table>
            <asp:HyperLink runat="server" NavigateUrl="~/Rejestry/RejestrCentralny.aspx" Text="Rejestry Centralne" />
            <%} %>
        <h3>
            Lista rejestrów</h3>
        <asp:Label ID="lbl1" runat="server" AssociatedControlID="rok" Text="Wybierz rok" />
        <asp:DropDownList runat="Server" ID="rok" AutoPostBack="True" />
        <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="true">
            <asp:ListItem Selected="True" Value="True">Mojego wydziału</asp:ListItem>
            <asp:ListItem Value="False">Innych wydziałów</asp:ListItem>
        </asp:RadioButtonList>
        <asp:GridView GridLines="None" ID="GridView1" runat="server"
            AllowSorting="True" AutoGenerateColumns="False" CssClass="grid fullWidth" DataKeyNames="id"
            DataSourceID="ObjectDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            OnSorted="GridView1_Sorted" onrowdatabound="GridView1_RowDataBound" 
            ondatabinding="GridView1_DataBinding">
            <EmptyDataTemplate>
                Brak dostępnych rejestrów</EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True"
                    SortExpression="id" Visible="False"></asp:BoundField>
                <asp:ButtonField Visible="false" DataTextField="nazwa" HeaderText="Nazwa rejestru" SortExpression="nazwa"
                    CommandName="Select" />
                    <asp:TemplateField HeaderText="Nazwa rejestru" SortExpression="nazwa">
                    <ItemTemplate>
                    <asp:LinkButton ID="lnkRegSelect" runat="server" CommandName="Select"><asp:Literal ID="litRegName" runat="server" /></asp:LinkButton>                    
                    </ItemTemplate>
                    </asp:TemplateField>
            </Columns>
            <AlternatingRowStyle CssClass="pozycjaNieparzysta" />
            <SelectedRowStyle CssClass="wybrany" />
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
            
            TypeName="Pemi.Esoda.Web.UI.Rejestry.RejestryDSTableAdapters.RejestryTableAdapter" 
            InsertMethod="Insert" 
            UpdateMethod="Update">
               
                  <UpdateParameters>
                      <asp:Parameter Name="id" Type="Int32" />
                      <asp:Parameter Name="nazwa" Type="String" />
                      <asp:Parameter Name="rok" Type="Int32" />
                      <asp:Parameter Name="aktualny" Type="Boolean" />
                     
            </UpdateParameters>
            <SelectParameters>
                <asp:ControlParameter ControlID="rok" Name="rok" PropertyName="SelectedValue" 
                    Type="Int32" />
                <asp:Parameter Name="userId" Type="Object"></asp:Parameter>
                <asp:ControlParameter ControlID="RadioButtonList1" Name="mojegoWydzialu" 
                    PropertyName="SelectedValue" Type="Boolean"></asp:ControlParameter>
                <asp:Parameter DefaultValue="false" Name="nieaktywne" Type="Boolean" />
            </SelectParameters>
                
                  <InsertParameters>
                      <asp:Parameter Name="wydzialGlowny" Type="Int32" />
                      <asp:Parameter Name="idDefinicji" Type="Int32" />
                      <asp:Parameter Name="nazwa" Type="String" />
                      <asp:Parameter Name="jrwaId" Type="Int32" />
                      <asp:Parameter Name="wydzialy" Type="Object" />
                      <asp:Parameter Name="wpisy" Type="Object" />
                      <asp:Parameter Name="xslt" Type="Object" />
                      <asp:Parameter Name="rok" Type="Int32" />
                      <asp:Parameter Name="entryDate" Type="Boolean" />
                      <asp:Parameter Name="creatingUser" Type="Boolean" />
                      <asp:Parameter Name="numerKolejnejPozycji" Type="Int32" />
                      <asp:Parameter Name="aktualny" Type="Boolean" />
                      <asp:Parameter Name="archiwalny" Type="Boolean" />
                     
            </InsertParameters>
        </asp:ObjectDataSource>
        <fieldset runat="server" id="wyszukiwanie" visible="false">
            <legend>Wyszukiwanie</legend>
            <%--<uc1:KryteriaWyszukiwaniaWRejestrze ID="kryteria" runat="server"  />--%>
            <div runat="server" id="kryteria">
            </div>
            <asp:LinkButton runat="server" ID="szukaj" Text="szukaj" OnClick="szukaj_Click" />
            <asp:LinkButton runat="server" ID="wyczysc" Text="wyczyść" OnClick="wyczysc_Click" />
        </fieldset>
        <br />
        <fieldset runat="server" id="printpdf" visible="false">
        <legend>Drukuj rejestr</legend>
        <br />
        <a runat="server" id="printPdfLink"/>                
        </fieldset>
    </div>
    <div style="width: 79%; float: right;">
        <h3>
            Przeglądanie:
            <asp:Literal runat="server" ID="registryName" /></h3>
                 <div id="pager">
Nawigacja (strony rejestru):&nbsp; 
                    <asp:LinkButton runat="server" ID="pierwszaStrona" Text="pierwsza" CommandName="Page" CommandArgument="First" OnCommand="zmianaStrony" />
                    &nbsp;
                    <asp:LinkButton runat="server" ID="poprzedniaStrona" Text="poprzednia" CommandName="Page" CommandArgument="Prev" OnCommand="zmianaStrony"  />
                    &nbsp;
 <strong>aktualna</strong>&nbsp;<asp:DropDownList runat="Server" ID="nrStrony" AutoPostBack="true" OnSelectedIndexChanged="zmianaNumeruStrony" />
                    &nbsp;z <strong>
                    <asp:Literal runat="Server" ID="liczbaStron" />
                    </strong>&nbsp;
                    <asp:LinkButton runat="server" ID="nastepnaStrona" Text="następna" CommandName="Page" CommandArgument="Next" OnCommand="zmianaStrony"  />
                    &nbsp;
                    <asp:LinkButton runat="server" ID="ostatniaStrona" Text="ostatnia" CommandName="Page" CommandArgument="Last" OnCommand="zmianaStrony"  />
                    &nbsp;
 Pozycje na stronie:
                    <asp:DropDownList runat="server" ID="rozmiarStrony" OnSelectedIndexChanged="zmianaNumeruStrony" AutoPostBack="true">
                        <asp:ListItem Value="5" />
                        <asp:ListItem Value="10" selected="true"/>
                        <asp:ListItem Value="15" />
                        <asp:ListItem Value="20" />
                        <asp:ListItem Value="25" />
                        <asp:ListItem Value="30" />
                        <asp:ListItem Value="50000" Text="wszystkie" />
                    </asp:DropDownList>
                </div>
        <div class="ramka">
            <table class="grid fullWidth">
               <asp:Xml runat="server" ID="registryContent" /></table>
        </div>
    </div>
</asp:Content>



