<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
    Codebehind="EdycjaSkanowPozycjiDziennika.aspx.cs" Inherits="Pemi.Esoda.Web.UI.EdycjaSkanowPozycjiDziennika"
    Title="Edycja skanów" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
  <h2>Edycja skanów skojarzonych z pozycj¹ rejestru <asp:Literal runat="server" ID="numerPozycji" /></h2>
<asp:UpdatePanel runat="server" ID="up1"><ContentTemplate>
 <asp:LinkButton ID="LinkButton1" runat="server" Text="Powrót do przegl¹dania dziennika" OnClick="GoBack" CssClass="link" />
    &nbsp;
    <hr />
    <asp:Panel runat="server" ID="contentPanel">
    <asp:GridView GridLines="None" runat="server" ID="wybranaPozycja" AutoGenerateColumns="false" CssClass="grid fullWidth">
        <Columns>
            <asp:BoundField HeaderText="Nr" DataField="numer" />
            <asp:BoundField HeaderText="Data otrzymania" DataField="dataWplywu" />
            <asp:BoundField HeaderText="Data koresp." DataField="dataPisma" />
            <asp:BoundField HeaderText="Nr koresp." DataField="numerPisma" />
            <asp:BoundField HeaderText="Interesant" DataField="nadawca" />
            <asp:BoundField HeaderText="Opis koresp." DataField="opis" />          
            <%--<asp:TemplateField>
                <HeaderTemplate>
                    Znak referenta
                </HeaderTemplate>
                <ItemTemplate>
                    <%#XPath("pracownikOdbierajacy")%>
                </ItemTemplate>
            </asp:TemplateField>--%>
        </Columns>
    </asp:GridView>
    <hr />
    <div id="leftColumn">
          <h3>
                    Lista skanów skojarzonych z pozycj¹ rejestru</h3>
                    <div class="ramka">
                    <asp:Repeater runat="server" ID="listaSkanow" OnItemCommand="obslugaListySkanow">
                    <ItemTemplate>
                    <div class="skanInfo" runat="server" id="skanInfo">
                    <table>
                    <tr><td rowspan="2" style="width: 60px">
                    <asp:ImageButton ID="wyborSkanu" CommandName="wybierzSkan" CommandArgument='<%#Eval("ID") %>' runat="server" ToolTip="szczegó³owe informacje o skanie" ImageUrl='<%# Eval("ID","~/Image.aspx?id={0}") %>' />
                    <br />
                    <asp:Literal ID="czyGlowny" runat="Server" Text='G³ówny' Visible='<%# Eval("IsMain")%>' />
                    <br />
                    <asp:HyperLink runat="server" ID="pobierz" NavigateUrl='<%# Eval("ID","~/download.aspx?id={0}&pn=0") %>' Text="pobierz ca³y skan" /><hr />
                    <asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl='<%# Eval("ID","~/download.aspx?id={0}&pn=1") %>' Text="pobierz pierwsz¹ stronê" />
                    </td>
                    <th style="width: 8em;">Nazwa orygina³u</th>
                    <td><asp:Literal ID="nazwaSkanu" runat="Server" Text='<%# Eval("OriginalName")%>' /></td>
                    </tr>
                    <tr>
                    <th>Krótki opis</th>
                    <td><asp:Literal ID="opisSkanu" runat="Server" Text='<%# Eval("Description")%>' /></td>
                    </tr>
                    
                    </table>
                    </div>
                    </ItemTemplate>
                    </asp:Repeater>
                    	</div>
    </div>
    <div id="rightColumn">
     <asp:LinkButton runat="server" ID="nowySkan" Text="Dodaj nowy skan" CssClass="link" OnClick="dodajNowySkan" />
                &nbsp;
                <asp:LinkButton runat="server" ID="usuwanieSkanu" Text="Usuñ skan" CssClass="link" OnClick="usunSkan" />
                <hr />
                <div id="opcjeWybranejPozycji" runat="server" visible="false">
                <asp:CheckBox runat="server" ID="isMainItem" Text="G³ówny?" /><br />
                <asp:Label ID="Label1" runat="server" AssociatedControlID="opisElementu" Text="Krótki opis" />
                <asp:TextBox runat="server" ID="opisElementu" TextMode="MultiLine" Rows="5" Columns="40" /><br />
                <asp:LinkButton runat="Server" ID="zapisanieZmian" Text="Zapisz zmiany" OnClick="zapiszZmiany" />
                <hr />
                <h3>
                    Podgl¹d pierwszej strony skanu</h3>
                <asp:Image runat="server" ID="podgladSkanu" />
                </div>
    </div>
    </asp:Panel>
    <asp:Label runat="server" ID="lblDailyLogItemAccessDeniedInfo" ForeColor="Red" Text="Brak mo¿liwoœci edycji!" />
   </ContentTemplate></asp:UpdatePanel>
</asp:Content>
