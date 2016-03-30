<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
  Codebehind="NowyDokumentLogiczny.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Akcje.NowyDokumentLogiczny"
  Title="Tworzenie nowego dokumentu" EnableEventValidation="false" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
  <h2>
    <%=Page.Title %>
  </h2>
  <asp:Label runat="server" ID="lblMessage" ForeColor="Red" />  
  <table class="grid">
    <tr>
      <th colspan="2">Utwórz nowy dokument</th>
    </tr>
    <tr>
      <th><asp:Label ID="label50" runat="server" AssociatedControlID="fkategoria" Text="Kategoria" /></th>
      <td><asp:DropDownList runat="server" ID="fkategoria" OnSelectedIndexChanged="fkategoria_SelectedIndexChanged" AutoPostBack="true" /></td>
    </tr>
    <tr>         
      
            <th><asp:Label ID="label19" runat="server" AssociatedControlID="frodzajDokumentu" Text="Rodzaj" /></th>
            <td><asp:DropDownList runat="server" ID="frodzajDokumentu" AutoPostBack="True" 
                    onselectedindexchanged="frodzajDokumentu_SelectedIndexChanged" /></td>
       
    </tr>
    <tr>
    <th><asp:Label runat="server" AssociatedControlID="txtDataPisma" Text="Data pisma" /></th>
    <td>
    <asp:TextBox runat="server" ID="txtDataPisma" />
    <ajax:CalendarExtender runat="server" TargetControlID="txtDataPisma" />
    <asp:CompareValidator runat="server" ControlToValidate="txtDataPisma" Operator="DataTypeCheck" Type="Date" ErrorMessage="z³y format" />
    </td>
    </tr>
    <tr><th><asp:Label runat="server" AssociatedControlID="txtOpis" Text="Opis" /></th>
    <td><asp:TextBox runat="server" ID="txtOpis" TextMode="MultiLine" Rows="3" Columns="40" /></td></tr>
  </table>  
  <div style="float:left">
  <div><asp:LinkButton runat="server" ID="utworz" Text="utwórz dokument" OnClick="utworz_Click" /></div>
  <div><asp:LinkButton runat="server" Visible="false" ID="createDocAndOpenWordForm" Text="utwórz dokument wraz z formularzem" OnClick="createDocAndOpenWordForm_Click" /></div>
  <div><asp:LinkButton runat="server" Visible="false" ID="createDocAndWordTemplate" Text="utwórz dokument wraz z szablonem MS Word" OnClick="createDocAndWordTemplate_Click" /></div>  
  <div><asp:LinkButton runat="server" ID="anuluj" Text="anuluj" OnClick="anuluj_Click" /></div>
  </div> 
</asp:Content>

                        
