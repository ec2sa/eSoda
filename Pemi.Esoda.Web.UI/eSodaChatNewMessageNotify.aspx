<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eSodaChatNewMessageNotify.aspx.cs" Inherits="Pemi.Esoda.Web.UI.eSodaChatNewMessageNotify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>eSodaChat Notify</title>
    <style type="text/css">   
    .esodanotify {font-size: 0.7em; font-family: verdana,arial,sans-serif;}
    </style>
    <script language="javascript" type="text/javascript">
    
    function time()
    {
        isNewMessage();
        window.setTimeout(time, 1000);
    }   
    
    window.onload = function()
    {
        time();            
    }
    
    function isNewMessage()
    {
        var toguid = $get('toGuid').value;    
        if (toguid != '')    
            Pemi.Esoda.Web.UI.services.ChatService.IsNewMessage(toguid, isNewMessage_Success, OnError, null);
    }   
     
    function isNewMessage_Success(e)
    {                     
        if (e == true)
        {            
            $get('ret').style.color='#ff0000';
        }
        else
        {         
            $get('ret').style.color='#7e6f49';
        }       
    }
    
    function OnError(ex)
    {
        //alert('Error: ' + ex._message);
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="ScriptManager" runat="server">
            <Services>
                <asp:ServiceReference Path="~/services/ChatService.asmx" />
            </Services>
        </asp:ScriptManager>
        <a class="esodanotify" id="ret" target="_parent" href='<%=this.APath %>/eSodaChatUsers.aspx'>eSoda - Komunikator</a>
        <asp:HiddenField runat="server" ID="toGuid" />        
    </div>
    </form>
</body>
</html>
