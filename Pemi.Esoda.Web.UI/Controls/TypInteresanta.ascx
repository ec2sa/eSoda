<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TypInteresanta.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.TypInteresanta" %>
<asp:RadioButtonList ID="rblTypInteresanta" runat="server" AutoPostBack="True" DataSourceID="dsTypInteresanta" DataTextField="nazwa" DataValueField="id" OnDataBound="rblTypInteresanta_DataBound" OnSelectedIndexChanged="rblTypInteresanta_SelectedIndexChanged">
</asp:RadioButtonList>
<asp:SqlDataSource ID="dsTypInteresanta" runat="server" ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>"
       SelectCommand="[Uzytkownicy].[listaTypowInteresanta]" SelectCommandType="StoredProcedure">
</asp:SqlDataSource>