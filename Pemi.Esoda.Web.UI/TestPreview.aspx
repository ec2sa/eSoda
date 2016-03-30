<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleColumn.Master" AutoEventWireup="true" CodeBehind="TestPreview.aspx.cs" Inherits="Pemi.Esoda.Web.UI.TestPreview" %>

<%@ Register src="Controls/ScanPreviewControl.ascx" tagname="ScanPreviewControl" tagprefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="server">
<h2></h2>
    <uc1:ScanPreviewControl ID="ScanPreviewControl1" runat="server" />

</asp:Content>

