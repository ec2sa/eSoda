<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eSodaChat.aspx.cs" Inherits="Pemi.Esoda.Web.UI.eSodaChat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>   
    <style type="text/css">

    </style>
    <script language="javascript" type="text/javascript">
     
    function handleKeyPress(e)
    {
        var key=e.keyCode || e.which;
        
        if (key==13)
        {
            setMsg();
            $get('currentmessage').value = '';
        }
    }    
        
    function setFocus(id)
    {
        document.getElementById(id).focus();
    }
    
    function _SetChatTextScrollPosition()
    {
        var chatText = document.getElementById("ChatText");
        chatText.scrollTop = chatText.scrollHeight;
    }
                                                        
    function time()
    {
        getMsg();
        window.setTimeout(time, 1000);
    }   
    
    window.onload = function()
    {
        time();
        setFocus('currentmessage');        
    }
                                    
    function setMsg()
    {     
        var mess = $get('currentmessage').value;
        var username = $get('username').value;
        var tousername = $get('toUsername').value;
        
        var fromguid = $get('fromGuid').value;
        var toguid = $get('toGuid').value;
        
        var now = new Date();                       
        var date = String.format("{0} {1}", now.format("yyyy-MM-dd"), now.format("HH:mm:ss"));
        
        if (mess != '')
        {            
            Pemi.Esoda.Web.UI.services.ChatService.SetMessage(mess, username, tousername, fromguid, toguid, OnSuccess, OnError, null);
            $get('ret').innerHTML += String.format("<li class=\"esodachat-tobold\">{0} @ {1}</li>", username, date);
            $get('ret').innerHTML += String.format("<li class=\"esodachat-to\">{0}</li>", mess);
        }
        _SetChatTextScrollPosition();
    }
    
    function getMsg()
    {
        var username = $get('username').value;                
        var fromguid = $get('fromGuid').value;
        var toguid = $get('toGuid').value;
        
        Pemi.Esoda.Web.UI.services.ChatService.GetMessage(toguid, fromguid, getMsg_Success, OnError, null);
    }
    
    function getMsg_Success(e)
    {
        var result = eval("(" + e + ")"); 
        var msgData = new Sys.StringBuilder();
        var line;        
        
        for(i=0;i < result.length; i++)
        {
            line = String.format("<li class=\"esodachat-frombold\">{0} @ {1}</li>", result[i]['From'], result[i]['Date']);
            msgData.appendLine(line);                           
            line = String.format("<li class=\"esodachat-from\">{0}</li>", result[i]['Message']);
            msgData.appendLine(line);                           
        }
        
        $get('ret').innerHTML += msgData.toString();
        _SetChatTextScrollPosition();
        
    }
     
    function OnSuccess()
    {

    }
    
    function OnError(ex)
    {
        //alert('Error: ' + ex._message);
    }
    </script>

</head>
<body>
    <form id="form" runat="server">
    
        <asp:ScriptManager ID="ScriptManager" runat="server">
            <Services>
                <asp:ServiceReference Path="~/services/ChatService.asmx" />
            </Services>
        </asp:ScriptManager>        
    <div class="esodachat-wrap">
    <div id="ChatText" style="width: 400px; height: 400px; overflow: scroll;">
    <ul id="ret">
    </ul>
    </div>
    <asp:HiddenField runat="server" ID="username"/>
    <asp:HiddenField runat="server" ID="toUsername" />
    <asp:HiddenField runat="server" ID="fromGuid" />
    <asp:HiddenField runat="server" ID="toGuid" />
    </div>
    </form>
    <input type="text" id="currentmessage" size="50" onkeypress="handleKeyPress(event)";/>
    <input type="button" value="Send"id="btnSend" onclick="setMsg();" />
</body>
</html>
