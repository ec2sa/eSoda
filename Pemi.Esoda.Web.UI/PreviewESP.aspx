<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreviewESP.aspx.cs" Inherits="Pemi.Esoda.Web.UI.PreviewESP" %>
<%@ Register Src="~/Controls/ESPDocumentPreview.ascx" TagName="PreviewESP" TagPrefix="esoda" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <esoda:PreviewESP ID="ctrlPreviewESP" runat="server" />
    </form>
</body>
</html>
