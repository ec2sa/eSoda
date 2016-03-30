<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
    CodeBehind="HistoriaDokumentuPozycjiDziennika.aspx.cs" Inherits="Pemi.Esoda.Web.UI.HistoriaDokumentuPozycjiDziennika"
    Title="Historia dokumentu" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>
        Historia dokumentu pozycja dziennika
        <asp:Literal runat="server" ID="numerPozycji" /></h2>
    <asp:LinkButton ID="LinkButton1" runat="server" Text="Powrót do przegl¹dania dziennika"
        OnClick="GoBack" CssClass="link" />
    <asp:LinkButton ID="LinkButton2" runat="server" Text="Edycja pozycji dziennika" OnClick="Edit"
        CssClass="link" />
    <hr />
    <asp:Panel runat="server" ID="contentPanel">
    <fieldset>
        <legend>Historia dokumentu pozycji dziennika</legend>
        <asp:Label runat="server" ID="lblError" ForeColor="Red" />
        <div style="margin:5px;">
        <asp:GridView GridLines="None" runat="server" ID="lista" AutoGenerateColumns="false"
            CssClass="grid fullWidth">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Data</HeaderTemplate>
                    <ItemTemplate>
                        <%# XPath("substring(data,1,10)") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Czas</HeaderTemplate>
                    <ItemTemplate>
                        <%# XPath("substring(data,12,8)")%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Pracownik</HeaderTemplate>
                    <ItemTemplate>
                        <%# XPath("pracownik") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Status po</HeaderTemplate>
                    <ItemTemplate>
                        <%# XPath("status") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        Opis</HeaderTemplate>
                    <ItemTemplate>
                        <%# XPath("opis") %></ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField>
<HeaderTemplate>Szczegó³y</HeaderTemplate>
<ItemTemplate><asp:LinkButton ID="LinkButton3" runat="server" Text="poka¿" CommandName="details" CommandArgument='<%# XPath("id") %>' /></ItemTemplate>
</asp:TemplateField>--%>
            </Columns>
        </asp:GridView>
        </div>
    </fieldset>
    </asp:Panel>
    <asp:Label runat="server" ID="lblDailyLogItemAccessDeniedInfo" ForeColor="Red" Text="Brak mo¿liwoœci podgl¹du!" />
</asp:Content>
