<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="SzczegolyAkcji.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Akcje.SzczegolyAkcji" Title="Szczeg�y wykonanej akcji" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<asp:LinkButton runat="server" Text="powr�t" OnClick="returnToCaller" />
<hr />
<asp:Literal runat="server" ID="emptyData" Text="Brak informacji szczeg�owych o wykonaniu akcji" Visible="false" />
<asp:Xml runat="server" ID="xml" />
</asp:Content>
