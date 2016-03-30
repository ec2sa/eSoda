<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
    CodeBehind="SzczegolyPozycjiDziennika.aspx.cs" Inherits="Pemi.Esoda.Web.UI.SzczegolyPozycjiDziennika"
    Title="Szczegó³y pozycji dziennika" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>
        Szczegó³y pozycji rejestru
        <asp:Literal runat="server" ID="numerPozycji" /></h2>
    <asp:LinkButton ID="LinkButton1" runat="server" Text="Powrót do przegl¹dania dziennika"
        OnClick="GoBack" CssClass="link" />
    <asp:LinkButton ID="LinkButton2" runat="server" Text="Edycja pozycji dziennika" OnClick="Edit"
        CssClass="link" />
    <hr />
    <asp:Panel runat="server" ID="contentPanel">
        <fieldset class="registryItem">
            <legend>Podgl¹d pozycji</legend>
            <asp:Xml runat="Server" ID="szczegolyPozycji"></asp:Xml>
        </fieldset>
        <fieldset class="historyItems">
            <legend>Historia pozycji</legend>
            <asp:GridView GridLines="None" runat="server" ID="historiaPozycji" CssClass="listaPozycjiRejestru grid"
                AutoGenerateColumns="false" AlternatingRowStyle-CssClass="pozycjaNieparzysta">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Data i czas<br />
                            edycji</HeaderTemplate>
                        <ItemTemplate>
                            <%#XPath("@data")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Data<br />
                            [W]p³ywu / [P]isma</HeaderTemplate>
                        <ItemTemplate>
                            <strong>W:</strong><%# XPath("dataWplywu")%><br />
                            <strong>P:</strong><%# XPath("dataPisma")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <%if (!IsInvoice)
                              { %>
                            Znak pisma
                            <%}
                              else
                              { %>
                            <strong>[N]</strong>umer faktury<br />
                            <strong>[K]</strong>wota
                            <%} %>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%if (!IsInvoice)
                              { %>
                            <%#XPath("numerPisma")%>
                            <%}
                              else
                              { %>
                            <strong>N:</strong><%#XPath("numerPisma")%><br />
                            <strong>K:</strong><%#XPath("kwota")%><br />
                            <%} %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <%if (!IsInvoice)
                              { %>
                            Interesant
                            <%}
                              else
                              { %>
                            Nadawca pisma
                            <%} %>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%#XPath("nadawca")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Typ koresp./<br />
                            numer</HeaderTemplate>
                        <ItemTemplate>
                            <%# XPath("typKorespondencji/rodzaj")%><br />
                            <%# XPath("typKorespondencji/wartosc","{0}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Klasyfikacja dokumentu<br />
                            [K]ategoria [R]odzaj [N]umer
                        </HeaderTemplate>
                        <ItemTemplate>
                            <strong>K:</strong>
                            <%#XPath("klasyfikacjaDokumentu/kategoria") %><br />
                            <strong>R:</strong>
                            <%#XPath("klasyfikacjaDokumentu/rodzaj")%><br />
                            <strong>N:</strong>
                            <%#XPath("klasyfikacjaDokumentu/wartosc")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Znak Referenta<br />
                            [W]ydzia³<br />
                            [P]racownik
                        </HeaderTemplate>
                        <ItemTemplate>
                            W:
                            <%#XPath("znakReferenta/wydzial") %>
                            <br />
                            P:
                            <%#XPath("znakReferenta/pracownik") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Dodatkowe niezdigitalizowane materia³y
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# XPath("dodatkoweMaterialy/@zawiera")!=null && XPath("dodatkoweMaterialy/@zawiera").ToString()=="tak"?"Zawiera: ":"Nie zawiera."%>
                            <%#XPath("dodatkoweMaterialy")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField>
                        <HeaderTemplate>
                            Skany</HeaderTemplate>
                        <ItemTemplate>
                            <%#XPath("skany") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
    </asp:Panel>
    <asp:Label runat="server" ID="lblDailyLogItemAccessDeniedInfo" ForeColor="Red" Text="Brak mo¿liwoœci podgl¹du!" />
</asp:Content>
