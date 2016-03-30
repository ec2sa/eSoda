<%@ Page Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true"
  Codebehind="DekretacjaDokumentuWielokrotna.aspx.cs" Inherits="Pemi.Esoda.Web.UI.Akcje.DekretacjaDokumentuWielokrotna"
  Title="Dekretowanie dokumentu" %>
  <%@ Register TagName="ContextItem" TagPrefix="esoda" Src="~/Controls/DocumentContextItem.ascx" %>
  <%@ Register TagName="DocumentOperations" TagPrefix="esoda" Src="~/Controls/OperacjeDokumentu.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
    <esoda:ContextItem id="ContextItem2" runat="server"></esoda:ContextItem>
    <h2>
    <%=Page.Title %>
    </h2>
    <esoda:DocumentOperations runat="Server" ID="DocumentOperations1" />
    <hr />
    <fieldset>
        <legend>Dekretacja dokumentu</legend>        
        <table class="grid gridDokument" style="width:65%">
        <tr><th style="width:26%">Opis</th><td style="width:74%"><asp:Label runat="server" ID="lblDescription" /></td></tr>
        <tr><th>Uwagi</th><td><asp:Label runat="server" ID="lblNotice" /></td></tr>
        </table>
        <span class="RedirectMultiplyMessageWrap"><asp:Label runat="server" ID="lblMessage" CssClass="RedirectMultiplyMessage"/></span>
        <table class="RedirectMultiplyGrid">   
            <tr>
            <td colspan="4" style="background-color:#ccc;">Zarezerwuj now¹ dekretacjê</td>
            </tr>        
            <tr>
                <th>
                    <asp:Label ID="Label6" runat="server" AssociatedControlID="wydzial"
            Text="Wydzia³" />
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="wydzial" AutoPostBack="true" OnSelectedIndexChanged="obslugaZmianyWydzialu" />
                </td>
                <th>
                    <asp:Label ID="Label9" runat="server" AssociatedControlID="pracownik"
            Text="Pracownik" />
                </th>
                <td>
                    <asp:DropDownList runat="server" ID="pracownik" />
                </td>
            </tr>
            <tr>
                <th>
                    Notatka</th>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtNote" TextMode="MultiLine" Rows="4" Columns="50" />
                </td>
            </tr>
            <tr>
                <th><asp:Label ID="Label5" runat="server" AssociatedControlID="cbPaper" Text="Praca na papierze (oryginale)" /></th>
                <td><asp:CheckBox runat="server" ID="cbPaper" AutoPostBack="True" 
                        oncheckedchanged="cbPaper_CheckedChanged" /></td>
                <th><asp:Label ID="Label1" runat="server" AssociatedControlID="cbCommand" Text="Do wiadomoœci" /></th>
                <td><asp:CheckBox runat="server" ID="cbCommand" /></td>
            </tr>
            <tr>
                <th>
                    <asp:Label ID="Label2" runat="server"  Text="Opcje zaawansowane" />
                </th>
                <td colspan="3" style="background-color:#ccc; margin:0px; padding: 0px;">                                
                        <table id="AdvancedOptionsPanel" runat="server" visible="false" cellpadding="0" cellspacing="0" class="RedirectMultiplyAdvancedOptionGrid">
                            <tr>
                                <th>
                                    <asp:Label ID="Label3" runat="server" AssociatedControlID="cbAllHistory" Text="Przeka¿ dokument z pe³n¹ histori¹ wykonanych na nim dzia³añ:" />
                                </th>
                                <td>
                                    <asp:CheckBox runat="server" ID="cbAllHistory"/>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <asp:Label ID="Label4" runat="server" AssociatedControlID="cbAllScan" Text="Przeka¿ dokument ze wszystkimi wersjami skanów:" />
                                </th>
                                <td>
                                    <asp:CheckBox runat="server" ID="cbAllScan" />
                                </td>
                            </tr>                            
                        </table>                        
                        <div style="border-top: 0px solid #000; padding:7px; margin:0px; width: 100%;text-align: center;"><asp:LinkButton runat="server" ID="lnkAdvancedOptions" Text="Poka¿ opcje zaawansowane" onclick="lnkAdvancedOptions_Click" /></div>
                        <%--<div style="border-top: 1px solid #000; padding:7px; margin:0px; width: 100%;text-align: center;" ><a style="cursor:hand;" onclick="var o=document.getElementById('AdvancedOptionsPanel'); if(o.style.display=='none') o.style.display='block'; else o.style.display='none';">Poka¿ zaawansowane</a></div>--%>
                    </td>
                
            </tr>
            <tr>
                <th>
                    &nbsp;</th>
                <td colspan="3" style="text-align:center; padding: 10px;">
                    <asp:LinkButton runat="server" ID="lnkAddToRedirect" Text="Dodaj do dekretacji" 
                        onclick="lnkAddToRedirect_Click" />
                    &nbsp;<asp:LinkButton runat="server" ID="lnkCancel" Text="Anuluj" Visible=false 
                        onclick="lnkCancel_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="4" style="background-color:#ccc;">
                    Dekretacje zarezerwowane do wykonania</td>
            </tr>
            <tr>
                <th>
                    &nbsp;</th>
                <td style="text-align: center;">
                    <asp:LinkButton runat="server" ID="wykonaj" Text="Wykonaj dekretacje" />
                </td>
                <td>
                    &nbsp;</td>
                <td style="text-align: center; padding: 10px;">
                    <asp:LinkButton runat="server" ID="anuluj" Text="Anuluj dekretacje" OnClick="anuluj_Click" />
                </td>
            </tr>
            <tr><td colspan="4" style="background-color:#ccc;">Lista zadekretowanych wydzia³ów i osób</td></tr>            
        </table>             
        <asp:GridView runat="server" ID="RedirectListGridView"  CssClass="grid"
            AutoGenerateColumns="false" 
            onrowcommand="RedirectListGridView_RowCommand">
            <EmptyDataTemplate>Lista jest pusta.</EmptyDataTemplate>
        <Columns>        
        <asp:TemplateField HeaderText="Lp." ItemStyle-HorizontalAlign="Center">
        <ItemTemplate>
        <%#Container.DataItemIndex+1 %>
        </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Wydzia³" DataField="OrganizationalUnitName" />
        <asp:BoundField HeaderText="Pracownik" DataField="EmployeeName" />
        <asp:CheckBoxField HeaderText="Papier" DataField="WorkOnPaper" ReadOnly="true" ItemStyle-HorizontalAlign="Center"/>
        <asp:CheckBoxField HeaderText="Do wiad." DataField="CommandID" ReadOnly="true" ItemStyle-HorizontalAlign="Center" />        
        <asp:BoundField HeaderText="Notatka" DataField="Note" />
        <asp:TemplateField HeaderText="Akcje" ItemStyle-HorizontalAlign="Center">
        <ItemTemplate>
        <asp:LinkButton runat="server" ID="lnkEditItem" CommandArgument="<%#Container.DataItemIndex+1 %>" CommandName="selectEditItem" Text="Edytuj" />&nbsp;
        <asp:LinkButton OnClientClick="return confirm('Czy napewno chcesz usun¹æ z dekretacji?');" ID="lnkDeleteItem" runat="server" CommandArgument="<%#Container.DataItemIndex+1 %>" CommandName="selectDeleteItem" Text="Usuñ" />
        </ItemTemplate>
        </asp:TemplateField>
        </Columns>
        </asp:GridView>
    </fieldset>
</asp:Content>
