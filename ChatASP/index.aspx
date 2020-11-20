<!DOCTYPE html>

<html>
<head>
    <title>Простой чат</title>
    <style>
        #form {
            display: flex;
        }
        input {
            outline: none;
            border: 1px solid gray;
            width: 130px;
            padding: 1em .5em;
            display: block;
            font-size: 0.9em;
            font-family: 'Consolas', monospace;
            flex: 1;
        }
    </style>
</head>
<body>

    <h3 id="welcome">Добро пожаловать в чат!</h3>
    <div>
        <label>Статус: </label>
        <label id="stateLabel">Готовимся к подключению...</label>
        <label id="userCount">0</label>
    </div>
    <p></p>

    <!--   <input type="text" id="message" />-->
    <main>

        <form id="form">
            <p class="vid"></p>
                <!--<label for="message">&gt;   </label>-->
                <input type="text" id="message" disabled ="disabled" required autofocus autocomplete="off" />
            
            <!-- <label for="message">&gt;</label> -->
        </form>

    </main>
    <button id="btnSendMessage" onclick="btnSendMessage_Click()"></button>
    <p class="vid"> <ul id="messages" ></ul></p>
    <!--<input type="button" value="Отправить" id="send" />-->
    <div id='messages'></div>

    <script type="text/javascript">

    </script>
    <script src="./lib/app.js"></script>
    <script src="./lib/appClass.js"></script>
</body>
</html>