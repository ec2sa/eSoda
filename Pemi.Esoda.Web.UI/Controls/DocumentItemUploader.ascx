<%@ Control Language="C#" AutoEventWireup="true" Codebehind="DocumentItemUploader.ascx.cs"
  Inherits="Pemi.Esoda.Web.UI.Controls.DocumentItemUploader" %>
<div id="documentItemUploader">
  <asp:MultiView runat="server" ID="views" ActiveViewIndex="0">
    <asp:View runat="server" ID="documentItemUploading">
      <table class="grid fullWidth">
        <tr>
          <th>Dodawanie pliku do dokumentu (etap 1 z 2) </th>
        </tr>
        <tr>
          <td>
            <asp:FileUpload runat="server" ID="fileToUpload" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="fileToUpload"
              Display="Dynamic" ErrorMessage="Trzeba wybraæ plik!" ValidationGroup="view1"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
          <td>
            <asp:LinkButton runat="Server" ID="pobierz" Text="pobierz" OnClick="pobierz_Click"
              ValidationGroup="view1" />
            <asp:LinkButton runat="server" ID="anuluj" Text="anuluj" OnClick="anuluj_Click" />
          </td>
        </tr>
        <tr>
          <td>Maksymalny rozmiar pliku:
            <asp:Literal runat="server" ID="maxFileSize" Text="10 MB" /></td>
        </tr>
      </table>
    </asp:View>
    <asp:View runat="server" ID="documentItemUploaded">
      <table class="grid fullWidth">
        <tr>
          <th>Dodawanie pliku do dokumentu (etap 2 z 2)</th>
        </tr>
        <tr>
          <td>
            <asp:Literal runat="server" ID="uploadResult" /></td>
        </tr>
        <tr>
          <td>Rozmiar pliku:
            <asp:Literal runat="server" ID="uploadedFileSize" />
            Typ:
            <asp:Literal runat="server" ID="mimeType" />
          </td>
        </tr>
        <tr>
          <td>
            <fieldset>
            <legend>Dodatkowe informacje o dodawanym pliku</legend>
              <asp:Label ID="label1" runat="Server" AssociatedControlID="fileName" Text="Nazwa pliku:" />
              <asp:TextBox runat="server" ID="fileName" />
              <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="fileName"
                Display="Dynamic" ErrorMessage="Trzeba podaæ nazwê pliku!" ValidationGroup="view2"></asp:RequiredFieldValidator><br />
              <asp:CheckBox runat="server" Text="Uaktualnij istniej¹cy plik o takiej nazwie" ID="forceUpdate" /><br />
              <asp:Label runat="server" Text="Opis" AssociatedControlID="fileDescription" />
              <asp:TextBox TextMode="MultiLine" Columns="40" Rows="4" runat="server" ID="fileDescription" />
            </fieldset>/
          </td>
        </tr>
        <tr>
          <td>
            <asp:LinkButton runat="Server" ID="zapisz" Text="zapisz" OnClick="zapisz_Click" ValidationGroup="view2" />
            <asp:LinkButton runat="server" ID="LinkButton2" Text="anuluj" OnClick="anuluj_Click" />
            <asp:LinkButton runat="server" ID="changeFile" Text="pobranie innego pliku" OnClick="changeFile_Click" />
          </td>
        </tr>
      </table>
    </asp:View>
  </asp:MultiView>
</div>
