<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ChatASP.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <style>
        #form {
            display: flex;
        }
        input {
            outline: none;
            border: 1px solid gray;
            width: 215px;
            padding: 1em .5em;
            display: block;
            font-size: 0.9em;
            font-family: 'Consolas', monospace;
            flex: 1;
        }
            #btnSendMessage {
                width: 100px;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <h3 id="welcome">Добро пожаловать в чат!</h3>
    <div>
        <label>Статус: </label>
        <label id="stateLabel">Готовимся к подключению...</label>
        <label id="userCount">0</label>
    </div>
    <p></p>

    <!--   <input type="text" id="message" />-->
    <main>

            <p class="vid">
                <asp:Table ID="Table1" runat="server">
                </asp:Table>
                <asp:Table ID="Table2" runat="server" Width="321px">
                </asp:Table>
        </p>
                <!--<label for="message">&gt;   </label>-->
                <input type="text" id="message" disabled ="disabled" required autofocus autocomplete="off" />
            
            <!-- <label for="message">&gt;</label> -->
        </form>

    </main>

    </form>
</body>
</html>
