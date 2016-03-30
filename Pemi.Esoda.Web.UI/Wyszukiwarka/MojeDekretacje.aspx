<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="MojeDekretacje.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Wyszukiwarka.MojeDekretacje" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<h2>Moje dekretacje</h2>
<asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
<fieldset>
<legend>Kryteria wyszukiwania</legend>
<table>
<tr>
<td><asp:Label ID="Label1" runat="server" AssociatedControlID="txtClient" Text="Interesant" /></td>
<td><asp:TextBox runat="server" ID="txtClient" /></td>
<td><asp:Label ID="Label2" runat="server" AssociatedControlID="txtNr" Text="Numer dziennika kancelaryjnego" /></td>
<td><asp:TextBox runat="server" ID="txtNr" /></td>
</tr>
<tr>
<td><asp:Label ID="Label3" runat="server" AssociatedControlID="startDate" Text="data dekr. od" ToolTip="Data dekretacji dokumentu" /></td>
<td>
<asp:TextBox runat="server" ID="startDate" Width="65px"/>
<ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="startDate" runat="server" />
<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="startDate" ErrorMessage="zły format" Operator="DataTypeCheck" Type="Date" />
</td>
<td><asp:Label ID="Label4" runat="server" AssociatedControlID="endDate" Text="data dekr. do" ToolTip="Data dekretacji dokumentu" /></td>
<td><asp:TextBox runat="server" ID="endDate" Width="65px" />
<ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="endDate" runat="server" />
<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="endDate" ErrorMessage="zły format" Operator="DataTypeCheck" Type="Date" />
</td>

</tr>
<tr>
<td colspan="4"><asp:LinkButton runat="server" ID="search" Text="szukaj" onclick="search_Click" /></td>
</tr>
</table>
<%--<asp:DropDownList runat="server" ID="clients" DataTextField="Description" DataValueField="ID" />--%>
</fieldset>
<br />
<fieldset>
<legend>Wyniki wyszukiwania</legend>
 <div id="pager" runat="server">
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
            <asp:LinkButton runat="server" ID="nastepnaStrona" Text="następna" CommandName="Page"
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
<EmptyDataTemplate>Brak dekretacji spełniających kryteria</EmptyDataTemplate>
<Columns>
<asp:BoundField HeaderText="Lp." DataField="OrdinalNumber" ItemStyle-HorizontalAlign="Center" />
<asp:BoundField HeaderText="Nr sys." DataField="DocumentID" />

<asp:TemplateField HeaderText="Nr dz.">
<ItemTemplate>
<%# Eval("RegistryNumber")%><%#Eval("IsInvoice")%>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField HeaderText="Interesant" DataField="SenderName" />
<asp:BoundField HeaderText="Status" DataField="Status" />
<asp:BoundField HeaderText="Znak pisma" DataField="DocumentSignature" />
<asp:BoundField HeaderText="Rodzaj" DataField="DocumentType" />
<asp:TemplateField>
<HeaderTemplate>
<strong>[W]</strong>ydział<br />
<strong>[P]</strong>racownik
</HeaderTemplate>
<ItemTemplate>
<strong>[W] </strong><%#Eval("CurrentDepartment")%><br />
<strong>[P] </strong><%#Eval("CurrentEmployee")%>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField HeaderText="Data dekr." DataField="DecretationDate" DataFormatString="{0:yyyy-MM-dd}"/>
<asp:BoundField HeaderText="Data utw." DataField="CreationDate" DataFormatString="{0:yyyy-MM-dd}" />
<asp:TemplateField>

<ItemTemplate>
<asp:LinkButton runat="server" ID="lblCase" CommandName="lblCase" CommandArgument='<%#Bind("DocumentID") %>' Text="Dokument" />
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
 </div>
</fieldset>
</asp:Content>
