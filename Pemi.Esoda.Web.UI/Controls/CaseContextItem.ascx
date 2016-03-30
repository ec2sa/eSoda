<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CaseContextItem.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.CaseContextItem" %>
<div id="sprawaContent">
<h1>Sprawa:&nbsp;<span runat="server" id="oznaczenieSprawy"></span></h1>
</div>
<asp:GridView GridLines="None" runat="Server" ID="opisZadania" AutoGenerateColumns="false" class="fullWidth" ShowHeader="false">
<EmptyDataTemplate>Brak skojarzonego zadania</EmptyDataTemplate>
<Columns>
<asp:TemplateField>
<ItemTemplate>
<table class="grid gridSprawa fullWidth">
<tr>
<th>Rok</th>
<th>Referent</th>
<th>Symbol kom. org.</th>
<th>Oznaczenie teczki</th>
<th>Tytu³ teczki wg wykazu akt</th>
</tr>
<tr>
<td><%# XPath("rok") %></td>
<td><%# XPath("referent") %></td>
<td><%# XPath("wydzial") %></td>
<td><%# XPath("numerTeczki") %></td>
<td><%# XPath("tytulTeczki") %></td>
</tr>
</table>
<table class="grid gridSprawa fullWidth">
<tr>
<th rowspan="2">Lp.</th>
<th rowspan="2">Sprawa<br />(krótka treœæ)</th>
<th colspan="2">Interesant</th>
<th colspan="2">Data</th>
<th rowspan="2">Uwagi<br />(sposób za³atwienia)</th>
</tr>
<tr>
<th>Znak pisma</th>
<th>Z dnia</th>
<th>Wszczêcia sprawy</th>
<th>Ostatecznego<br /> za³atwienia</th>
</tr>

<tr>
<td rowspan="2"><%# XPath("lp") %></td>
<td rowspan="2"><%# XPath("rodzajSprawy")%></td>
<td colspan="2" style="text-align:center;"><%# (XPath("nadawca") != null) ? XPath("nadawca") : "&nbsp;"%></td>
<td rowspan="2"><%# XPath("dataRozpoczecia")%></td>
<td rowspan="2"><%# XPath("dataZakonczenia")%></td>
<td rowspan="2"><%# XPath("uwagi") %></td>
</tr>
<tr>
<td><%# (!XPath("znakPisma").ToString().Equals("")) ? XPath("znakPisma") : "&nbsp;"%></td>
<td><%# (!XPath("dataPisma").Equals("")) ? DateTime.Parse(XPath("dataPisma").ToString()).ToString("yyyy-MM-dd") : "&nbsp;"%></td>
</tr>

</table>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
