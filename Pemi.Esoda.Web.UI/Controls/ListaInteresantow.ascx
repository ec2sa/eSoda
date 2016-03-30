<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListaInteresantow.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.ListaInteresantow" %>
<div class="ramka4">
<fieldset>
<legend>Wyniki wyszukiwania</legend>
<asp:GridView CssClass="grid fullWidth" ID="gvListaInteresantow" runat="server" 
                AutoGenerateColumns="False" DataKeyNames="id" 
                OnRowCommand="gvListaInteresantow_RowCommand" 
                OnRowDataBound="gvListaInteresantow_RowDataBound" AllowPaging="True" 
                DataSourceID="odsListaInteresantow">
<Columns>
<asp:TemplateField HeaderText="Interesant">
<ItemTemplate>
<asp:LinkButton ID="lnkEdit" ToolTip="Edycja interesanta" runat="server" CommandName="EditCustomer" CommandArgument='<%# Eval("id") %>' Text='<%# (!Eval("nazwa").ToString().Equals("")) ? Eval("nazwa") : string.Format("{0}\n{1}", Eval("nazwisko").ToString(), Eval("imie").ToString()) %>'>
</asp:LinkButton>
<asp:LinkButton ID="lnkSelect" ToolTip="Wybór interesanta" runat="server" CommandName="SelectCustomer" CommandArgument='<%# Eval("id") %>' Text='<%# (!Eval("nazwa").ToString().Equals("")) ? Eval("nazwa") : string.Format("{0}\n{1}", Eval("nazwisko").ToString(), Eval("imie").ToString()) %>'>
</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<HeaderTemplate>Typ /<br />kategoria<br />interesanta</HeaderTemplate>
<ItemTemplate>
<%# Eval("typ") %> <br /> <%# Eval("kategoria") %>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Adres">
<ItemTemplate>
<table>
<tr><td><%# Eval("ulica") %> <%# Eval("budynek") %> <%# (!Eval("lokal").ToString().Equals("")) ? string.Format(" / {0}", Eval("lokal")) : ""  %></td></tr>
<tr><td><%# Eval("kod") %> <%# Eval("miasto") %></td></tr>
</table>
</ItemTemplate>
</asp:TemplateField>
<%--<asp:TemplateField HeaderText="Akcja" Visible="false">
<ItemTemplate>
<asp:LinkButton ID="lnkEdit" runat="server" Text="edytuj" CommandName="EditCustomer" CommandArgument='<%# Eval("id") %>'></asp:LinkButton>
<asp:LinkButton ID="lnkSelect" runat="server" Text="wybierz" CommandName="SelectCustomer" CommandArgument='<%# Eval("id") %>'></asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>--%>
</Columns>
<EmptyDataTemplate>
Brak wyników wyszukiwania.<br />
<asp:LinkButton ID="lnkAddNewCustomer" runat="server" Text="WprowadŸ nowego interesanta" OnClick="lnkAddNewCustomer_Click"></asp:LinkButton>
</EmptyDataTemplate>
</asp:GridView>
<asp:ObjectDataSource ID="odsListaInteresantow" runat="server" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="FindCustomer" 
                TypeName="Pemi.Esoda.DataAccess.CustomerDAO" >
    
            </asp:ObjectDataSource>
<asp:LinkButton ID="lnkAddNewCustomer" runat="server" Text="WprowadŸ nowego interesanta" Visible="false" OnClick="lnkAddNewCustomer_Click"></asp:LinkButton>
</fieldset>
</div>