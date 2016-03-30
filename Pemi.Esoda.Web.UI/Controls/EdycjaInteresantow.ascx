<%@ Control Language="C#" AutoEventWireup="true" Codebehind="EdycjaInteresantow.ascx.cs"
  Inherits="Pemi.Esoda.Web.UI.Controls.EdycjaInteresantow" %>
<fieldset>
  <legend>Interesant</legend>
  <div runat="server" id="v1">
    <asp:RadioButtonList ID="rodzajInteresanta" runat="server" OnSelectedIndexChanged="obslugaZmianyTypuInteresanta"
      CssClass="rodzajInteresanta" AutoPostBack="true">
      <asp:ListItem Value="1" Text="Osoba fizyczna" Selected="true"></asp:ListItem>
      <asp:ListItem Value="2" Text="Firma"></asp:ListItem>
      <asp:ListItem Value="3" Text="Instytucja"></asp:ListItem>
      <asp:ListItem Value="0" Text="Wszystkie"></asp:ListItem>
    </asp:RadioButtonList>
    <asp:DropDownList ID="ddlSendersList" runat="server">
    </asp:DropDownList><br />
    <asp:LinkButton ID="lnkAddSender" runat="server" OnClick="lnkAddSender_Click">Dodaj interesanta</asp:LinkButton>
    <asp:LinkButton ID="lnkEdit" runat="server" OnClick="lnkEdit_Click">Edytuj interesanta</asp:LinkButton><br />
  </div>
  <asp:FormView ID="frmSender" runat="server" OnItemInserting="frmSender_ItemInserting"
    OnItemUpdating="frmSender_ItemUpdating" OnModeChanging="frmSender_ModeChanging"
    OnItemCommand="frmSender_ItemCommand" OnDataBound="frmSender_DataBound">
    <ItemTemplate>
      &nbsp;
    </ItemTemplate>
    <InsertItemTemplate>
      <asp:RadioButtonList ID="rodzajNowegoInteresanta" runat="server" AutoPostBack="true"
        CssClass="rodzajInteresanta" OnSelectedIndexChanged="obslugaZmianyTypuNowegoInteresanta">
        <asp:ListItem Text="Osoba fizyczna" Value="1"></asp:ListItem>
        <asp:ListItem Text="Firma" Value="2"></asp:ListItem>
        <asp:ListItem Text="Instytucja" Value="3"></asp:ListItem>
      </asp:RadioButtonList>
      <div runat="server" id="daneOsoby" visible="false">
        <asp:Label ID="lblNazwisko" runat="server" Text="Nazwisko" AssociatedControlID="txtNazwisko"
          CssClass="etykieta" />
        <asp:TextBox runat="server" ID="txtNazwisko" /><br />
        <asp:Label ID="lblImie" runat="server" Text="Imiê" AssociatedControlID="txtImie"
          CssClass="etykieta" />
        <asp:TextBox runat="server" ID="txtImie" /></div>
      <div runat="server" id="daneFirmy" visible="False">
        <asp:Label ID="lblNazwa" runat="server" Text="Nazwa" AssociatedControlID="txtNazwa"
          CssClass="etykieta" />
        <asp:TextBox runat="server" ID="txtNazwa" />
        <br />
        <asp:Label ID="Label1" runat="server" Text="NIP" AssociatedControlID="txtNIP"
          CssClass="etykieta" />
        <asp:TextBox runat="server" ID="txtNIP" MaxLength="13" />
        <br />
        <asp:Label ID="Label2" runat="server" Text="Numer do potwierdzeñ SMS" AssociatedControlID="txtNumerSMS"
          CssClass="etykieta" />
        <asp:TextBox runat="server" ID="txtNumerSMS" MaxLength="9" />
        <br />
      </div>
      <fieldset>
        <legend>Adres</legend>
        <table>
          <tr>
            <td style="text-align: right;">
              <asp:Label ID="lblKod" runat="server" AssociatedControlID="txtKod" CssClass="etykieta"
                Text="Kod" />,
              <asp:Label ID="lblMiasto" runat="server" AssociatedControlID="txtMiasto" CssClass="etykieta"
                Text="Miejscowoœæ" /></td>
            <td>
              <asp:TextBox runat="server" ID="txtKod" Style="width: 4em" />,
              <asp:TextBox runat="server" ID="txtMiasto" /></td>
          </tr>
          <tr>
            <td style="text-align: right;">
              <asp:Label ID="lblUlica" runat="server" AssociatedControlID="txtUlica" CssClass="etykieta"
                Text="Ulica" />
            </td>
            <td>
              <asp:TextBox runat="server" ID="txtUlica" />
            </td>
          </tr>
          <tr>
            <td style="text-align: right;">
              <asp:Label ID="lblBudynek" runat="server" AssociatedControlID="txtBudynek" CssClass="etykieta"
                Text="Budynek" />
              /
              <asp:Label ID="lblLokal" runat="server" AssociatedControlID="txtLokal" CssClass="etykieta"
                Text="Lokal" /></td>
            <td>
              <asp:TextBox runat="server" ID="txtBudynek" Style="width: 3em" />
              /
              <asp:TextBox runat="server" ID="txtLokal" Style="width: 3em" /></td>
          </tr>
          <tr>
            <td style="text-align: right;">
              <asp:Label ID="lblPoczta" runat="server" AssociatedControlID="txtPoczta" CssClass="etykieta"
                Text="Poczta" /></td>
            <td>
              <asp:TextBox runat="Server" ID="txtPoczta" /></td>
          </tr>
        </table>
      </fieldset>
      <asp:LinkButton ID="lnkAdd" runat="server" CommandName="Insert">Dodaj</asp:LinkButton>
      <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel">Anuluj</asp:LinkButton>
    </InsertItemTemplate>
    <EditItemTemplate>
      <asp:RadioButtonList ID="rodzajNowegoInteresanta" runat="server" AutoPostBack="true"
        CssClass="rodzajInteresanta" OnSelectedIndexChanged="obslugaZmianyTypuNowegoInteresanta">
        <asp:ListItem Text="Osoba fizyczna" Value="1"></asp:ListItem>
        <asp:ListItem Text="Firma" Value="2"></asp:ListItem>
        <asp:ListItem Text="Instytucja" Value="3"></asp:ListItem>
      </asp:RadioButtonList>
      <div runat="server" id="daneOsoby" visible="false">
        <asp:Label ID="lblNazwisko" runat="server" Text="Nazwisko" AssociatedControlID="txtNazwisko"
          CssClass="etykieta" />
        <asp:TextBox runat="server" ID="txtNazwisko" Text='<%# Bind("nazwisko") %>' /><br />
        <asp:Label ID="lblImie" runat="server" Text="Imiê" AssociatedControlID="txtImie"
          CssClass="etykieta" />
        <asp:TextBox runat="server" ID="txtImie" Text='<%# Bind("imie") %>' /></div>
      <div runat="server" id="daneFirmy" visible="False">
        <asp:Label ID="lblNazwa" runat="server" Text="Nazwa" AssociatedControlID="txtNazwa"
          CssClass="etykieta" />
        <asp:TextBox runat="server" ID="txtNazwa" Text='<%# Bind("nazwa") %>' />
        <br />
        <asp:Label ID="Label1" runat="server" Text="NIP" AssociatedControlID="txtNIP"
          CssClass="etykieta" />
        <asp:TextBox runat="server" ID="txtNIP" MaxLength="13" Text='<%# Bind("nip") %>'/>
        <br />
         <asp:Label ID="Label2" runat="server" Text="Numer do potwierdzeñ SMS" AssociatedControlID="txtNumerSMS"
          CssClass="etykieta" />
        <asp:TextBox runat="server" ID="txtNumerSMS" MaxLength="9" />
        <br />
      </div>
      <fieldset>
        <legend>Adres</legend>
        <table>
          <tr>
            <td style="text-align: right;">
              <asp:Label ID="lblKod" runat="server" AssociatedControlID="txtKod" CssClass="etykieta"
                Text="Kod" />,
              <asp:Label ID="lblMiasto" runat="server" AssociatedControlID="txtMiasto" CssClass="etykieta"
                Text="Miejscowoœæ" /></td>
            <td>
              <asp:TextBox runat="server" ID="txtKod" Style="width: 4em" />,
              <asp:TextBox runat="server" ID="txtMiasto" /></td>
          </tr>
          <tr>
            <td style="text-align: right;">
              <asp:Label ID="lblUlica" runat="server" AssociatedControlID="txtUlica" CssClass="etykieta"
                Text="Ulica" />
            </td>
            <td>
              <asp:TextBox runat="server" ID="txtUlica" />
            </td>
          </tr>
          <tr>
            <td style="text-align: right;">
              <asp:Label ID="lblBudynek" runat="server" AssociatedControlID="txtBudynek" CssClass="etykieta"
                Text="Budynek" />
              /
              <asp:Label ID="lblLokal" runat="server" AssociatedControlID="txtLokal" CssClass="etykieta"
                Text="Lokal" /></td>
            <td>
              <asp:TextBox runat="server" ID="txtBudynek" Style="width: 3em" />
              /
              <asp:TextBox runat="server" ID="txtLokal" Style="width: 3em" /></td>
          </tr>
          <tr>
            <td style="text-align: right;">
              <asp:Label ID="lblPoczta" runat="server" AssociatedControlID="txtPoczta" CssClass="etykieta"
                Text="Poczta" /></td>
            <td>
              <asp:TextBox runat="Server" ID="txtPoczta" /></td>
          </tr>
        </table>
      </fieldset>
      <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update">Zapisz</asp:LinkButton>
      <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel">Anuluj</asp:LinkButton>
    </EditItemTemplate>
  </asp:FormView>
</fieldset>
