<%@ Control Language="C#" AutoEventWireup="true" Codebehind="NumeracjaSpraw.ascx.cs"
  Inherits="Pemi.Esoda.Web.UI.Controls.NumeracjaSpraw" %>

<fieldset><legend>Dostosowanie numerowania spraw</legend>
  <table class="numeracjaSpraw"><thead><tr><th><asp:Label ID="Label0" runat="server"
    AssociatedControlID="prefix">Symbol wydzia³u</asp:Label></th>
    <th>&nbsp;</th>
    <th><asp:Label ID="Label1" runat="server" AssociatedControlID="jrwa">JRWA</asp:Label></th>
    <th><asp:Label ID="Label2" runat="server" AssociatedControlID="suffix">Dodatkowe oznaczenie</asp:Label></th>
    <th>&nbsp;</th>
    <th>Numer kolejny . rok</th>
    <td rowspan="3"><asp:LinkButton ID="LinkButton1" runat="server" Text="Zastosuj" OnClick="LinkButton1_Click" /></td>
  </tr>
  </thead>
    <tbody>
    <tr class="numberRow">
    <td><asp:TextBox runat="server" ID="prefix" MaxLength="20" /></td>
      <td>.</td>
      <td><asp:TextBox runat="server" ID="jrwa" ReadOnly="true" /></td>
      <td><asp:TextBox runat="server" ID="suffix" MaxLength="10" /></td>
      <td>.</td>
      <td><asp:Label runat="server" ID="numberAndYear" Text="1.2008" /></td>
      
    </tr>
      <tr><td>&nbsp;</td>
        <td>&nbsp;</td>
        <td><asp:CheckBox AutoPostBack="true" runat="server" ID="disableJRWA" Text="wy³¹cz" OnCheckedChanged="disableJRWA_CheckedChanged" /></td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td><asp:Label ID="Label3" runat="server" AssociatedControlID="firstNumber" Text="Numeruj od:" /><br /><asp:TextBox runat="server" ID="firstNumber" /></td>
      </tr>
    </tbody>
  </table>
  <fieldset><legend>Podgl¹d numeracji</legend>
<div runat="server" id="generatedNumbers">
</div>
</fieldset>
</fieldset>


