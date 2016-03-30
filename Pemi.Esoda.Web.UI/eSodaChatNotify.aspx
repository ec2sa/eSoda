<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eSodaChatNotify.aspx.cs" Inherits="Pemi.Esoda.Web.UI.eSodaChatNotify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>eSodaChat Notify</title>
    <script language="javascript" type="text/javascript">
    
    function time()
    {
        getChatters();
        window.setTimeout(time, 1000);
    }   
    
    window.onload = function()
    {
        time();            
    }
    
    function getChatters()
    {
        var toguid = $get('toGuid').value;        
        Pemi.Esoda.Web.UI.services.ChatService.GetChatters(toguid, getChatters_Success, OnError, null);
    }   
     
    function getChatters_Success(e)
    {                
        if (e != null)
        {
            var result = eval("(" + e + ")"); 
                        
            var msgData = new Sys.StringBuilder();
            var line;                    
                for(i=0;i < result.length; i++)
                {                    
                    line = String.format("<li><a href=\"#\" onclick=\"window.open('eSodaChat.aspx?toGuid={0}', 'chat_{1}','width=400,height=500,toolbar=no,menubar=no,scrollbar=no,resizable=no,location=no,directories=no,minimize=no,maximize=no');\">{1} oczekuje na rozmowe</a></li>", result[i]["FromGuid"],result[i]["From"]);
                    msgData.appendLine(line); 
                }
                
                $get('ret').innerHTML = msgData.toString();                            
        }
        else
        {
            $get('ret').innerHTML = "<li>brak nowych wiadomości.</li>";
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
        <ul id="ret" style="font-size:0.8em; font-family: helvetica,verdana,arial,sans-serif;"></ul>
        <asp:HiddenField runat="server" ID="toGuid" />          
    </div>
    </form>
</body>
</html>
