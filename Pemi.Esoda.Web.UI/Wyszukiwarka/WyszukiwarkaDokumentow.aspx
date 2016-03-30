<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="WyszukiwarkaDokumentow.aspx.cs" Inherits="Pemi.Esoda.Web.UI.WyszukiwarkaDokumentow" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <h2>Wyszukiwarka dokumentów</h2>
    <asp:Label runat="server" ID="lblMessage" ForeColor="Red" />
    
    <fieldset>
        <legend>Kryteria wyszukiwania</legend>
        <table>
            
            <tr>
                <td>
                    Kategoria dokumentu:
                </td>
                <td>
                    <asp:DropDownList ID="ddlCategories" runat="server" OnSelectedIndexChanged="ddlCategories_SelectedIndexChanged" AutoPostBack="true" />
                </td>
                <td>
                    Rodzaj dokumentu:
                </td>
                <td>
                    <asp:DropDownList ID="ddlTypes" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Numer z dziennika kancelaryjnego:
                </td>
                <td>
                    <asp:TextBox ID="tbDocumentNumber" runat="server" />
                </td>
                <td>
                    Numer systemowy:
                </td>
                <td>
                    <asp:TextBox ID="tbSystemNumber" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Interesant:
                </td>
                <td>
                    <asp:TextBox ID="tbClientName" runat="server" />
                </td>
                <td>
                    Znak Pisma:
                </td>
                <td>
                    <asp:TextBox ID="tbMark" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Data utworzenia (od):
                </td>
                <td>
                    <asp:TextBox runat="server" ID="tbDateFrom" />
                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="tbDateFrom" />
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="tbDateFrom" Operator="DataTypeCheck" Type="Date" ErrorMessage="Błędny format daty" />
                </td>
                <td>
                    Data utworzenia (do):
                </td>
                <td>
                    <asp:TextBox ID="tbDateTo" runat="server" />
                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="tbDateTo" />
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="tbDateTo" Operator="DataTypeCheck" Type="Date" ErrorMessage="Błędny format daty" />
                </td>
            </tr>
            <tr>
                <td>Aktualny status:</td>
                <td colspan="3">
                    <%--<asp:DropDownList ID="ddlStatuses" runat="server" />--%>
                    <asp:CheckBoxList ID="cblStatuses" runat="server" RepeatColumns="3" />
                </td>
            </tr>
            <tr>
                <td>Szukana fraza:</td>
                <td colspan="3">
                    <asp:TextBox ID="tbText" runat="server" TextMode="MultiLine" Columns="20" Rows="3" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td colspan="3">
                    Szukaj frazy w:
                    <asp:CheckBox ID="cbSearchDescription" runat="server" Text="opisie" />
                    <asp:CheckBox ID="cbSearchContent" runat="server" Text="treści skanów" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:LinkButton runat="server" ID="lbtnSearch" Text="Szukaj" onclick="lbtnSearch_Click" />
                </td>
            </tr>
        </table>
    
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
            <asp:GridView runat="server" ID="searchResults" CssClass="grid" AutoGenerateColumns="false" onrowcommand="searchResults_RowCommand">
                <EmptyDataTemplate>Brak dokumentów spełniających kryteria</EmptyDataTemplate>
                <Columns>
                    <%--<asp:BoundField HeaderText="Lp." DataField="NumberItem" ItemStyle-HorizontalAlign="Center" />--%>
                    <asp:BoundField HeaderText="Nr dk" DataField="DocumentNumber" />
                    <asp:BoundField HeaderText="Nr sys." DataField="SystemNumber" />
                    <asp:BoundField HeaderText="Data utw." DataField="CreationDate" DataFormatString="{0:dd-MM-yyyy}" />
                    <asp:BoundField HeaderText="Kategoria" DataField="DocumentCategory" />
                    <asp:BoundField HeaderText="Rodzaj" DataField="DocumentType" />
                    <asp:BoundField HeaderText="Interesant" DataField="ClientName" />
                    <asp:BoundField HeaderText="Znak pisma" DataField="Mark" />
                    <asp:BoundField HeaderText="Status" DataField="DocumentStatus" />
                    <asp:BoundField HeaderText="Właściciel dokumentu / Numer sprawy" DataField="Owner" />
                    <asp:BoundField HeaderText="Znaleziono w opisie" DataField="FoundInDescription" />
                    <asp:BoundField HeaderText="Znaleziono w skanach" DataField="FoundInContent" />
                    <%--<asp:HyperLinkField runat="server" Target="_blank" Text="dokument" 
                         DataNavigateUrlFields="SystemNumber,IsInContent,IsInDescription,FoundInContent"
                         DataNavigateUrlFormatString="~/Dokumenty/Dokument.aspx?ID={0}&c={1}&d={2}&w={3}" />   
                      --%>   
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink runat="server" Target="_blank" Text="dokument" 
                             NavigateUrl='<%# FormatDocURL((int)Eval("SystemNumber"),(bool)Eval("IsInContent"),(bool)Eval("IsInDescription"),(string)Eval("FoundInContent")) %>' />   
                        
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
         </div>
    </fieldset>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
</asp:Content>
