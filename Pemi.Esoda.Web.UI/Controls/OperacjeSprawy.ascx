<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacjeSprawy.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.OperacjeSprawy" %>
<ul class="opcje">
<li><asp:LinkButton runat="server" id="lnkCaseDetails" Text="szczegó³y" onCommand="wykonaj" commandargument="~/Sprawy/Sprawa.aspx"  /></li>
<li><asp:LinkButton runat="server" id="lnkCaseDocuments" Text="dokumenty" onCommand="wykonaj" commandargument="~/Sprawy/DokumentySprawy.aspx" /></li>
<li><asp:LinkButton runat="server" id="lnkCaseHistory" Text="historia" onCommand="wykonaj" commandargument="~/Sprawy/HistoriaSprawy.aspx" /></li>
<li><asp:LinkButton runat="server" id="lnkCaseActions" Text="akcje" onCommand="wykonaj" commandargument="~/Sprawy/AkcjeSprawy.aspx" /></li>
<li><asp:LinkButton runat="server" id="lnkCaseRegistry" Text="rejestry" OnCommand="wykonaj" CommandArgument="~/Sprawy/RejestrySprawy.aspx" /></li>
<li><asp:LinkButton runat="server" id="lnkCaseBack" Text="powrót" onCommand="wykonaj" commandargument="~/OczekujaceZadania.aspx"  /></li>
<li><asp:HyperLink runat="server" id="lnkCaseMetrics" Text="metryka sprawy" NavigateUrl="~/Aplikacje/Raporty/MetrykaSprawy.aspx"  /></li>
</ul>