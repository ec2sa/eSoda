<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DefinicjaZastepstw.ascx.cs" Inherits="Pemi.Esoda.Web.UI.Controls.DefinicjaZastepstw" %>
<asp:HiddenField runat="server" ID="dublerID" />
<fieldset style="margin-bottom:70px;">
<legend>Zastępstwa - kogo zastępuje</legend>
<asp:Label runat="server" ID="lblError" ForeColor="Red" />
<asp:ValidationSummary runat="server" ValidationGroup="saveGroup" />
<div style="margin: 5px;">
<table>
<tr>
<td><asp:Label ID="Label2" runat="server" AssociatedControlID="ddlDepartments" Text="Wydziały" /></td>
<td>
<asp:DropDownList runat="server" ID="ddlDepartments" 
        DataTextField="Description" DataValueField="ID" AutoPostBack="True" 
        onselectedindexchanged="ddlDepartments_SelectedIndexChanged" />
</td>

<td><asp:Label ID="Label3" runat="server" AssociatedControlID="ddlUsers" Text="Pracownik" /></td>
<td><asp:DropDownList runat="server" ID="ddlUsers" DataTextField="Description" DataValueField="ID" /></td>    
</tr>
<tr>
<td colspan="4"><asp:Label ID="Label4" runat="server" AssociatedControlID="startDate" Text="Data początkowa" />
<asp:TextBox runat="server" id="startDate" Width="75px" />
<ajax:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="startDate" />
<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="saveGroup" ControlToValidate="startDate" ErrorMessage="Data początkowa jest wymagana" Text="*" />
<asp:Label ID="Label1" runat="server" AssociatedControlID="endDate" Text="Data końcowa" />
<asp:TextBox runat="server" id="endDate" Width="75px" />
<ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="endDate" />
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="saveGroup" ControlToValidate="endDate" ErrorMessage="Data końcowa jest wymagana" Text="*" />
</td>
</tr>
<tr>
<td colspan="4" style="padding: 10px 0px 17px;"><asp:LinkButton runat="server" ID="addCover" Text="Dodaj zastępstwo" onclick="addCover_Click" ValidationGroup="saveGroup" /></td>
</tr>
</table>
</div>

<div style="margin: 5px;">
<div>Lista zastępowanych osób</div>
<asp:GridView runat="server" ID="coverList" AutoGenerateColumns="false" CssClass="grid" 
                onrowcommand="coverList_RowCommand">
<EmptyDataTemplate>Brak osób</EmptyDataTemplate>                
<Columns>
<asp:TemplateField>
<HeaderTemplate>Lp.</HeaderTemplate>
<ItemTemplate><%#Container.DataItemIndex+1 %>.</ItemTemplate>
</asp:TemplateField>
<asp:BoundField HeaderText="Wydział" DataField="userDepartment" />
<asp:BoundField HeaderText="Nazwisko" DataField="userSurname" />
<asp:BoundField HeaderText="Imię" DataField="userName" />
<asp:TemplateField>
<HeaderTemplate>Data początkowa</HeaderTemplate>
<ItemTemplate><%#Eval("startDate","{0:yyyy-MM-dd}") %></ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<HeaderTemplate>Data końcowa</HeaderTemplate>
<ItemTemplate><%#Eval("endDate","{0:yyyy-MM-dd}") %></ItemTemplate>
</asp:TemplateField>
<asp:TemplateField>
<HeaderTemplate>Akcja</HeaderTemplate>
<ItemTemplate><asp:LinkButton runat="server" ID="del" CommandName="delCover" CommandArgument='<%#Eval("CoverID") %>' Text="usuń" /></ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView>
</div>
</fieldset>