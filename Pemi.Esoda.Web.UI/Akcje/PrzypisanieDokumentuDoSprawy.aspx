<%@ Page Culture="pl-PL" Language="C#" MasterPageFile="~/MasterPages/SingleColumnNoUpdatePanel.Master" AutoEventWireup="true"
    CodeBehind="PrzypisanieDokumentuDoSprawy.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Akcje.PrzypisanieDokumentuDoSprawy"
    Title="Przypisanie dokumentu do sprawy" %>

<%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
<%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<%@ Register TagName="Interesant" TagPrefix="esoda" Src="~/Controls/Interesant.ascx" %>
<%@ Register TagName="ListaInteresantow" TagPrefix="esoda" Src="~/Controls/ListaInteresantow.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <esoda:ContextItem ID="ContextItem2" runat="server"></esoda:ContextItem>
    <h2>
        Przypisanie dokumentu do sprawy</h2>
    <esoda:DocumentOperations runat="Server" ID="DocumentOperations1" />
    <hr />
    <div id="leftColumn">
    <fieldset>    
        <legend>Opis sprawy</legend>
        <div>
        <asp:Label runat="server" ID="lblAvailableYear" AssociatedControlID="dostepneLata" Text="Wybierz rok:" /></br>
        <asp:DropDownList runat="server" ID="dostepneLata" AutoPostBack="True" 
                DataTextField="Description" DataValueField="Id" 
                onselectedindexchanged="dostepneLata_SelectedIndexChanged" />
        </div>      
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="label7" runat="server" AssociatedControlID="teczka" Text="Oznaczenie teczki" /><br />
            <asp:DropDownList runat="server" ID="teczka" AutoPostBack="true" />
            <asp:RequiredFieldValidator ID="rfvTeczka" runat="server" ValidationGroup="AssignDocument" ControlToValidate="teczka" InitialValue="0" Display="Dynamic" ErrorMessage="wymagane" Text="wybierz teczkê" />    
        </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="dostepneLata" EventName="SelectedIndexChanged" />
        </Triggers>
        </asp:UpdatePanel>
          
        
        <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <br />              
        <asp:Label ID="Label1" runat="server" AssociatedControlID="numerSprawy" Text="Numer sprawy" /><br />                
        <asp:DropDownList runat="server" ID="numerSprawy" />                
            <asp:RequiredFieldValidator ID="rfvSprawa" runat="server" 
                ControlToValidate="numerSprawy" Display="Dynamic" ErrorMessage="wymagane" 
                InitialValue="0" Text="wybierz sprawê" ValidationGroup="AssignDocument" />
        <br />
        </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="teczka" EventName="SelectedIndexChanged" />
        </Triggers>
        </asp:UpdatePanel>
        
        <div runat="server" id="opcjeNowejSprawy">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="Label2" runat="server" AssociatedControlID="rodzajSprawy" Text="Rodzaj sprawy" /><br />
            <asp:DropDownList runat="server" ID="rodzajSprawy" />
        </ContentTemplate>
        <Triggers><asp:AsyncPostBackTrigger ControlID="teczka" EventName="SelectedIndexChanged" /></Triggers>
        
        </asp:UpdatePanel>        
            <br />
            <asp:Label ID="Label3" runat="server" AssociatedControlID="opisSprawy" Text="Opis" /><br />
            <asp:TextBox TextMode="MultiLine" Rows="4" Columns="40" runat="server" ID="opisSprawy" /><br />
            <asp:Label ID="Label4" runat="server" AssociatedControlID="dataPisma" Text="Data pisma" /><br />
            
                <asp:TextBox runat="server" ID="dataPisma" />
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                ControlToValidate="dataPisma" ErrorMessage="*" Operator="DataTypeCheck" Display="Dynamic" 
                Type="Date" ValidationGroup="AssignDocument">b³êdny format daty (RRRR-MM-DD)</asp:CompareValidator><br />
                    <ajax:CalendarExtender ID="dataPisma_CalendarExtender" runat="server" TargetControlID="dataPisma" PopupPosition="TopLeft" FirstDayOfWeek="Monday" Format="yyyy-MM-dd" />    
            <asp:Label ID="Label5" runat="server" AssociatedControlID="znakPisma" Text="Znak pisma" /><br />
            <asp:TextBox runat="server" ID="znakPisma" /><br />
            <asp:Label ID="Label6" runat="server" AssociatedControlID="interesant" Text="Interesant" /><br />
            <asp:TextBox runat="server" ID="interesant" Visible="false" />
            <asp:UpdatePanel ID="up3" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>
            <asp:Label ID="lblInteresant" runat="server" Text="(brak)" />
             <asp:HiddenField ID="hfCustomerId" runat="server" />
            </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="customersList" EventName="SelectCustomer" />
            <asp:AsyncPostBackTrigger ControlID="lnkSelectCustomer" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>            
           
            <asp:LinkButton ID="lnkZmien" runat="server" Text="Zmieñ" 
                onclick="lnkZmien_Click" />
           
            <asp:CustomValidator ID="cmpCustomerId" runat="server" 
                ErrorMessage="*" Text="wybierz interesanta" Display="Dynamic"
                ValidationGroup="AssignDocument" 
                onservervalidate="cmpCustomerId_ServerValidate" />
        </div>
        <asp:LinkButton ID="przypiszDoIstniejacej" runat="server" 
            ValidationGroup="AssignDocument" Text="Przypisz do wybranej sprawy" 
            onclick="przypiszDoIstniejacej_Click" />&nbsp;
        <asp:LinkButton ID="przypiszDoNowej" runat="server" 
            ValidationGroup="AssignDocument" Text="Przypisz do nowej sprawy" 
            onclick="przypiszDoNowej_Click" />
    </fieldset>
   </div>
   <div id="rightColumn">
   <asp:UpdatePanel ID="up1" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
   <ContentTemplate>
  <asp:LinkButton ID="lnkSearchAgain" runat="server" Text="Wyszukaj ponownie" 
          Visible="false" onclick="lnkSearchAgain_Click" />
  <esoda:ListaInteresantow Visible="false" ID="customersList" runat="server" />
  <asp:LinkButton ID="lnkSelectCustomer" runat="server" Text="Wybierz" 
          Visible="false" onclick="lnkSelectCustomer_Click" />
  <esoda:Interesant Visible="false" ID="customer" runat="server" />  
  </ContentTemplate>
  <Triggers>
  <asp:AsyncPostBackTrigger ControlID="lnkZmien" EventName="Click" />
  </Triggers>
  </asp:UpdatePanel>
  </div>
</asp:Content>
