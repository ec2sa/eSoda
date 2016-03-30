<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Pemi.Esoda.Web.Logs._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>eSODA Demo Logs</title>
    <style type="text/css">
        
    td { vertical-align: top;
         text-align: center;}
    
    </style>
    <meta http-equiv="refresh" content="60" />
</head>
<body>
    <form id="form1" runat="server">
    <table border="0" width="100%">
    <tr>
    <th>Login counts by IP per day</th>
    <th>Login counts per day</th>
    <th>Login counts per IP</th>
    </tr>
    <tr>
    <td><asp:GridView ID="gvLogStats" runat="server" AutoGenerateColumns="False" 
        DataSourceID="dsLogStats" BackColor="White" BorderColor="#CC9966" 
        BorderStyle="None" BorderWidth="1px" CellPadding="4">
        <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
        <RowStyle BackColor="White" ForeColor="#330099" />
        <Columns>
            <asp:BoundField DataField="data" DataFormatString="{0:yyyy-MM-dd}" 
                HeaderText="data" HtmlEncode="False" ReadOnly="True" SortExpression="data" />
            <asp:BoundField DataField="ip" HeaderText="ip" SortExpression="ip" />
            <asp:BoundField DataField="ile" HeaderText="ile" ReadOnly="True" 
                SortExpression="ile" />
        </Columns>
        <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
    </asp:GridView>
    <asp:SqlDataSource ID="dsLogStats" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>" SelectCommand="select l.dataLogowania as data, l.remoteip as ip, count(*) as ile from
( select CAST(CONVERT(nvarchar, data, 102) as smalldatetime) as dataLogowania, remoteIP from Uzytkownicy.HistoriaLogowania ) as l
group by l.dataLogowania, l.remoteip
order by data desc"> </asp:SqlDataSource></td>
    <td><asp:GridView ID="gvLogStatsPerDay" runat="server" AutoGenerateColumns="False" 
        DataSourceID="dsLogStatsPerDay" BackColor="White" BorderColor="#CC9966" 
        BorderStyle="None" BorderWidth="1px" CellPadding="4">
        <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
        <RowStyle BackColor="White" ForeColor="#330099" />
        <Columns>
            <asp:BoundField DataField="data" DataFormatString="{0:yyyy-MM-dd}" 
                HeaderText="data" HtmlEncode="False" ReadOnly="True" SortExpression="data" />
            <asp:BoundField DataField="ile" HeaderText="ile" ReadOnly="True" 
                SortExpression="ile" />
        </Columns>
        <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
    </asp:GridView>
    <asp:SqlDataSource ID="dsLogStatsPerDay" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>" SelectCommand="select l.dataLogowania as data, count(*) as ile from
( select CAST(CONVERT(nvarchar, data, 102) as smalldatetime) as dataLogowania, remoteIP from Uzytkownicy.HistoriaLogowania ) as l
group by l.dataLogowania
order by data desc"> </asp:SqlDataSource></td>
    <td><asp:GridView ID="gvLogStatsPerIP" runat="server" AutoGenerateColumns="False" 
        DataSourceID="dsLogStatsPerIP" BackColor="White" BorderColor="#CC9966" 
        BorderStyle="None" BorderWidth="1px" CellPadding="4">
        <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
        <RowStyle BackColor="White" ForeColor="#330099" />
        <Columns>
            <asp:BoundField DataField="ip" HeaderText="ip" SortExpression="ip" />
            <asp:BoundField DataField="ile" HeaderText="ile" ReadOnly="True" 
                SortExpression="ile" />
        </Columns>
        <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
    </asp:GridView>
    <asp:SqlDataSource ID="dsLogStatsPerIP" runat="server" 
        ConnectionString="<%$ ConnectionStrings:EsodaDatabase %>" SelectCommand="select l.remoteip as ip, count(*) as ile from
( select CAST(CONVERT(nvarchar, data, 102) as smalldatetime) as dataLogowania, remoteIP from Uzytkownicy.HistoriaLogowania ) as l
group by l.remoteip
order by ile desc"> </asp:SqlDataSource></td>
    </tr>  
    
    </table>
   </form>
</body>
</html>
