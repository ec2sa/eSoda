<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="PowiadomienieSMS.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Akcje.PowiadomienieSMS" %>
<%@ Register TagPrefix="esoda" TagName="CaseContext" Src="~/Controls/CaseContextItem.ascx" %>
<%@ Register TagPrefix="esoda" TagName="OperacjeSprawy" Src="~/Controls/OperacjeSprawy.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
 <esoda:CaseContext runat="Server" ID="cc1" />
    <h2>
    <%=Page.Title %>
    </h2>
    <esoda:OperacjeSprawy runat="server" ID="ops1" />
    <hr />
    <fieldset>
    <legend>Wysłanie powiadomienia SMS do interesanta</legend>
    <asp:Label runat="server" AssociatedControlID="txtNumerSMS" Text="Numer telefonu (9 cyfr, bez spacji)" /><br />
    <strong>+48</strong><asp:TextBox runat="server" ID="txtNumerSMS" MaxLength="9" />
    <br />
    <asp:Label runat="server" AssociatedControlID="txtMessage" Text="Treść wiadomości (maks. 160 znaków)" /><br />
    <asp:TextBox runat="server" ID="txtMessage" TextMode="MultiLine" Rows="4" Columns="40" MaxLength="160" />
    <br />
    <asp:LinkButton runat="server" ID="wyslij" Text="Wyślij SMS" OnClick="wyslij_click" />
    
    </fieldset>
</asp:Content>
