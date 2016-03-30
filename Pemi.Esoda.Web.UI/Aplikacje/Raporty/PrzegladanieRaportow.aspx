<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" 
    AutoEventWireup="true" CodeBehind="PrzegladanieRaportow.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Aplikacje.Raporty.PrzegladanieRaportow" 
    %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>Przeglądanie dostęnych raportów</h2>
   
    <fieldset style="margin:1em 0;">
        <legend>Lista dokumentów nieprzypisanych do spraw</legend>
        <p>
            <label>Dokumenty z wydziału</label>
            <asp:DropDownList runat="server" ID="D1OUID" />
        </p>
        <p>
            <label>Zarejestrowane od</label>
            <asp:TextBox runat="server" ID="D1DaysFrom" Style="width: 4em" /> do
            <asp:TextBox runat="server" ID="D1DaysTo" Style="width: 4em" /> dni temu.
        </p>
        <p>
            <label>Format: </label>  
            <asp:DropDownList runat="server" ID="D1ReportType" />
            <asp:LinkButton runat="server" ID="ReportD1" Text="Pobierz" OnClick="ReportD1_Click" />
        </p>
    </fieldset>
    
     <fieldset style="margin:1em 0;">
        <legend>Lista spraw z kończącym się terminem realizacji</legend>
        <p>
            <label>Sprawy z wydziału</label>
            <asp:DropDownList runat="server" ID="D2OUID" />
        </p>
        <p>
            <label>Teczki z roku</label>
            <asp:TextBox runat="server" ID="D2Year" Style="width: 4em" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <label>Teczki z JRWA</label> 
            <asp:TextBox runat="server" ID="D2JRWA" Style="width: 4em" />
        </p>
        <p>
            <label>Termin upływa za</label>  
            <asp:TextBox runat="server" ID="D2DaysFrom" Style="width: 4em" /> do
            <asp:TextBox runat="server" ID="D2DaysTo" Style="width: 4em" /> dni.
        </p>
        <p>
            <label>Format: </label>  
            <asp:DropDownList runat="server" ID="D2ReportType" />
            <asp:LinkButton runat="server" ID="ReportD2" Text="Pobierz" OnClick="ReportD2_Click" />
        </p>
    </fieldset>
    
    <fieldset style="margin:1em 0;">
        <legend>Lista spraw przeterminowanych</legend>
        <p>
            <label>Sprawy z wydziału</label>
            <asp:DropDownList runat="server" ID="D3OUID" />
        </p>
        <p>
            <label>Teczki z roku</label>
            <asp:TextBox runat="server" ID="D3Year" Style="width: 4em" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <label>Teczki z JRWA</label> 
            <asp:TextBox runat="server" ID="D3JRWA" Style="width: 4em" />
        </p>
        
        <p>
            <label>Format: </label>  
            <asp:DropDownList runat="server" ID="D3ReportType" />
            <asp:LinkButton runat="server" ID="ReportD3" Text="Pobierz" OnClick="ReportD3_Click" />
        </p>
    </fieldset>
    
     <fieldset style="margin:1em 0;">
        <legend>Lista pozycji dziennika z niezdigitalizowanymi materiałami</legend>
        <p>
         <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="D4DateFrom" runat="server" />
          <ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="D4DateTo" runat="server" />
            <label>Zarejestrowane od dnia:</label>
            <asp:TextBox runat="server" ID="D4DateFrom" Style="width: 8em" /> do dnia:
            <asp:TextBox runat="server" ID="D4DateTo" Style="width: 8em" />
        </p>
        <p>
            <label>Format: </label>  
            <asp:DropDownList runat="server" ID="D4ReportType" />
            <asp:LinkButton runat="server" ID="ReportD4" Text="Pobierz" OnClick="ReportD4_Click" />
        </p>
    </fieldset>

      <fieldset style="margin:1em 0;">
        <legend>Ostatnie logowania użytkowników</legend>
        <p>
            <label>Użytkownicy z wydziału</label>
            <asp:DropDownList runat="server" ID="D5OUID" />
        </p>
        <p>
            <label>Ostatnie logowanie dawniej niż</label>
            <asp:TextBox runat="server" ID="D5Days" Style="width: 4em"  /> dni
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="D5Days" ErrorMessage="Trzeba podać wartość!" Display="Dynamic" ValidationGroup="D5" />
            <asp:CompareValidator runat="server" Operator="DataTypeCheck" Type="Integer" ControlToValidate="D5Days" ErrorMessage="Podana wartość musi być liczbą całkowitą!" Display="Dynamic"  ValidationGroup="D5" />
        </p>
        <p>
            <label>Format: </label>  
            <asp:DropDownList runat="server" ID="D5ReportType" />
            <asp:LinkButton runat="server" ID="ReportD5" Text="Pobierz" OnClick="ReportD5_Click" ValidationGroup="D5" />
        </p>
    </fieldset>


      <fieldset style="margin:1em 0;">
        <legend>Ostatnio utworzone sprawy przez użytkowników</legend>
        <p>
            <label>Użytkownicy z wydziału</label>
            <asp:DropDownList runat="server" ID="D6OUID" />
        </p>
        <p>
            <label>Ostatnie utworzenie sprawy dawniej niż</label>
            <asp:TextBox runat="server" ID="D6Days" Style="width: 4em"  /> dni
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="D6Days" ErrorMessage="Trzeba podać liczbę dni!" Display="Dynamic" ValidationGroup="D6" />
            <asp:CompareValidator ID="CompareValidator1" runat="server" Operator="DataTypeCheck" Type="Integer" ControlToValidate="D6Days" ErrorMessage="Podana wartość musi być liczbą całkowitą!" Display="Dynamic"  ValidationGroup="D6" />
        </p>
        <p>
            <label>Format: </label>  
            <asp:DropDownList runat="server" ID="D6ReportType" />
            <asp:LinkButton runat="server" ID="ReportD6" Text="Pobierz" OnClick="ReportD6_Click" ValidationGroup="D6" />
        </p>
    </fieldset>
          
</asp:Content>
