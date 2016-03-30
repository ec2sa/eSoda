<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/SingleColumn.Master"  CodeBehind="eSodaChatUsers.aspx.cs" Inherits="Pemi.Esoda.Web.UI.eSodaChatUsers" Title="eSoda komunikator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">       
    <h2>eSoda - Komunikator</h2>
    <div style="float:left; width:50%; margin-right: 10px;">
    <fieldset>
    <legend>Pracownicy</legend>
    <div style="padding: 5px; ">
        <asp:GridView runat="server" ID="Users" AutoGenerateColumns="False" 
            CssClass="grid fullWidth" AllowPaging="True" AllowSorting="True" PageSize="25" 
            DataSourceID="UserDataSource" onrowcreated="Users_RowCreated">
        <EmptyDataTemplate>Brak pracowników z aktywnym komunikatorem.</EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="Lp." ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <%#Container.DataItemIndex+1 %>.
                </ItemTemplate>                
            </asp:TemplateField>            
            <asp:BoundField DataField="UserName" HeaderText="Login" SortExpression="UserName" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="Comment" HeaderText="Imię i nazwisko" />
            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/StandardLayout/img/magglass.jpg"/>
            <ajax:PopupControlExtender runat="server" ID="PopupControlExtender1"               
               DynamicServiceMethod="GetDynamicContent"
               DynamicContextKey='<%# Eval("ProviderUserKey") %>'
               DynamicControlID="Panel1"
               TargetControlID="Image1"
               PopupControlID="Panel1"
               Position="right">
            </ajax:PopupControlExtender>
            </ItemTemplate>
            </asp:TemplateField>   
            <asp:TemplateField>
                <ItemTemplate>
                    <a href='#' onclick="window.open('eSodaChat.aspx?toGuid=<%#Eval("ProviderUserKey")%>','chat_<%#Eval("UserName") %>','width=400,height=500,toolbar=no,menubar=no,scrollbar=no,resizable=no,location=no,directories=no,minimize=no,maximize=no');"
                        id="lblChat">rozmawiaj</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>   
    </asp:GridView> 
    <asp:Panel ID="Panel1" runat="server">
        </asp:Panel>       
    </div>
    <asp:ObjectDataSource id="UserDataSource" runat="server" SelectMethod="CustomGetAllUsers"
    TypeName="Pemi.Esoda.DataAccess.GetAllUsers"></asp:ObjectDataSource>
</fieldset>        
</div>    
<div>
<fieldset>
<legend>Masz wiadomość</legend>
<div>
    <iframe frameborder="0" width="450px" src="eSodaChatNotify.aspx"></iframe>
</div>
</fieldset>
</div>   
</asp:Content>
