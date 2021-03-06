<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="WyszukiwarkaSpraw.aspx.cs" Inherits="Pemi.Esoda.Web.UI.WyszukiwarkaSpraw" Title="Wyszukiwarka spraw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>Wyszukiwarka spraw</h2>
    <asp:Label runat="server" ID="lblMessage" ForeColor="Red" />

    <fieldset>
        <legend>Kryteria wyszukiwania</legend>
            <table>
                <tr>
                    <td style="text-align:right"><asp:Label ID="Label1" runat="server" AssociatedControlID="txtClient" Text="Interesant:" /></td>
                    <td><asp:TextBox runat="server" ID="txtClient" /></td>
                    <td style="text-align:right"><asp:Label ID="Label2" runat="server" AssociatedControlID="departments" Text="Wydzia�:" /></td>
                    <td><asp:DropDownList runat="server" ID="departments" DataTextField="Description" DataValueField="ID" /></td>
                </tr>
                <tr>
                    <td style="text-align:right"><asp:Label ID="Label3" runat="server" AssociatedControlID="startDate" Text="Data rej. dok. od:" ToolTip="Data rejestracji dokumentu" /></td>
                    <td>
                        <asp:TextBox runat="server" ID="startDate" Width="65px"/>
                        <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="startDate" runat="server" />
                        <asp:CompareValidator runat="server" ControlToValidate="startDate" ErrorMessage="z�y format" Operator="DataTypeCheck" Type="Date" />
                    </td>
                    <td style="text-align:right"><asp:Label ID="Label4" runat="server" AssociatedControlID="endDate" Text="Data rej. dok. do:" ToolTip="Data rejestracji dokumentu" /></td>
                    <td><asp:TextBox runat="server" ID="endDate" Width="65px" />
                        <ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="endDate" runat="server" />
                        <asp:CompareValidator runat="server" ControlToValidate="endDate" ErrorMessage="z�y format" Operator="DataTypeCheck" Type="Date" />
                    </td>

                </tr>
                <tr>
                    <td colspan="4">
                        <asp:LinkButton runat="server" ID="search" Text="szukaj" onclick="search_Click" />
                    </td>
                </tr>
            </table>
            <%--<asp:DropDownList runat="server" ID="clients" DataTextField="Description" DataValueField="ID" />--%>
    </fieldset>
<br />
<fieldset>
<legend>Wyniki wyszukiwania</legend>
 <div id="pager" runat="server" style="text-align:center;">
            Nawigacja :&nbsp;
            <asp:LinkButton runat="server" ID="pierwszaStrona" Text="pierwsza" CommandName="Page"
                CommandArgument="First" OnCommand="zmianaStrony" />
            &nbsp;
            <asp:LinkButton runat="server" ID="poprzedniaStrona" Text="poprzednia" CommandName="Page"
                CommandArgument="Prev" OnCommand="zmianaStrony" />
            &nbsp; <strong>aktualna</strong>&nbsp;<asp:DropDownList runat="Server" ID="nrStrony"
                AutoPostBack="true" OnSelectedIndexChanged="zmianaStronyAktualnej" />
            &nbsp;z <strong>
                <asp:Literal runat="Server" ID="liczbaStron" />
            </strong>&nbsp;
            <asp:LinkButton runat="server" ID="nastepnaStrona" Text="nast�pna" CommandName="Page"
                CommandArgument="Next" OnCommand="zmianaStrony" />
            &nbsp;
            <asp:LinkButton runat="server" ID="ostatniaStrona" Text="ostatnia" CommandName="Page"
                CommandArgument="Last" OnCommand="zmianaStrony" />
            &nbsp; Pozycje na stronie:
            <asp:DropDownList runat="server" ID="rozmiarStrony" OnSelectedIndexChanged="zmianaNumeruStrony"
                AutoPostBack="true">
                <asp:ListItem Value="5" />
                <asp:ListItem Value="10" />
                <asp:ListItem Value="15" />
                <asp:ListItem Value="20" />
                <asp:ListItem Value="25" />
                <asp:ListItem Value="30" />
                <asp:ListItem Value="50000" Text="wszystkie" />
            </asp:DropDownList>
        </div>
 <div style="margin: 5px;">
<asp:GridView runat="server" ID="searchResults" CssClass="grid" 
        AutoGenerateColumns="false" onrowcommand="searchResults_RowCommand">
<EmptyDataTemplate>Brak spraw spe�niaj�cych kryteria</EmptyDataTemplate>
<Columns>
<asp:BoundField HeaderText="Lp." DataField="NumberItem" ItemStyle-HorizontalAlign="Center" />
<asp:BoundField HeaderText="Numer sprawy" DataField="CaseNumber" />
<asp:BoundField HeaderText="Rodzaj sprawy" DataField="CaseType" />
<asp:BoundField HeaderText="Interesant" DataField="ClientName" />
<asp:BoundField HeaderText="Znak pisma" DataField="PaperSymbol" />
<asp:BoundField HeaderText="Data pisma" DataField="PaperDate" DataFormatString="{0:yyyy-MM-dd}" />
<asp:BoundField HeaderText="Data wszcz�cia sprawy" DataField="CaseStartDate" DataFormatString="{0:yyyy-MM-dd}"/>
<asp:BoundField HeaderText="Data zako�czenia sprawy" DataField="CaseEndDate" DataFormatString="{0:yyyy-MM-dd}" />
<asp:BoundField HeaderText="Uwagi" DataField="Remarks" />
    
    <asp:TemplateField>
        <ItemTemplate>
            <asp:LinkButton runat="server" ID="lblCase" CommandName="lblCase" CommandArgument='<%#Bind("CaseID") %>' Text="sprawa" />
        </ItemTemplate>
    </asp:TemplateField>
</Columns>
</asp:GridView>
 </div>
</fieldset>
</asp:Content>
