<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperacjeDokumentu.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.OperacjeDokumentu" %>
<ul class="opcje">
<li><asp:LinkButton runat="server" id="lnkLegalAct" Text="akty prawne" onCommand="wykonaj" commandargument="~/Dokumenty/AktyPrawne.aspx"  /></li>
<li><asp:LinkButton runat="server" id="lnkCustomForm" Text="formularz" onCommand="wykonaj" commandargument="~/Dokumenty/FormularzWidokGlowny.aspx"  /></li>
<li><asp:LinkButton runat="server" id="lnkDocDetails" Text="szczeg�y" onCommand="wykonaj" commandargument="~/Dokumenty/Dokument.aspx"  /></li>
<li><asp:LinkButton runat="server" id="lnkDocInnerItems" Text="skany/pliki" onCommand="wykonaj" commandargument="~/Dokumenty/SkladnikiDokumentu.aspx" /></li>
<li><asp:LinkButton runat="server" id="lnkDocHistory" Text="historia" onCommand="wykonaj" commandargument="~/Dokumenty/HistoriaDokumentu.aspx" /></li>
<li><asp:LinkButton runat="server" id="lnkDocActions" Text="akcje" onCommand="wykonaj" commandargument="~/Dokumenty/AkcjeDokumentu.aspx" /></li>
<li><asp:LinkButton runat="server" id="lnkDocRegisters" Text="rejestry" onCommand="wykonaj" commandargument="~/Dokumenty/RejestryDokumentu.aspx" /></li>
<li><asp:LinkButton runat="server" id="lnkDocCodes" Text="wysy�ka" onCommand="wykonaj" commandargument="~/Dokumenty/KodyDokumentu.aspx" /></li>
<li><asp:LinkButton runat="server" id="lnkDocCase" Text="sprawa" onCommand="przejdzDoSprawy" Enabled="false"  /></li>
<li><asp:LinkButton runat="server" ID="lnkDocConfirmRead" Text="[Potwierdzenie zapoznania si� z dokumentem]" OnCommand="potwierdzZapoznanie" commandargument="~/OczekujaceZadania.aspx" OnClientClick="return confirm('Na pewno potwierdzi� ?');" /></li>
<li><asp:LinkButton runat="server" id="lnkDocBack" Text="powr�t" onCommand="wykonaj" commandargument="~/OczekujaceZadania.aspx"  /></li>
</ul>
