var socket
var   $txt = document.getElementById('message'),
//    $user = document.getElementById('user'),
//    $welcome = document.getElementById('welcome');
//$messages = document.getElementById('messages');
 $stateLabel = document.getElementById("stateLabel"),
$welcome = document.getElementById('welcome')
let user = null;
let flItsMe = false;

if (typeof (WebSocket) !== 'undefined') {
    const HOST = window.location.host; 
    console.log(HOST);
    //socket = new WebSocket("ws://" + HOST + "/ChatASP/ChatHandler.ashx");
     socket = new WebSocket("ws://sql_jane/ChatASP/ChatHandler.ashx");
} else {
    socket = new MozWebSocket("ws://" + HOST + "/ChatASP/ChatHandler.ashx");
}

function printMessage(change) {
    //if (change.mode == "strFilter") {
    //    if (flItsMe = true)
    //    { flItsMe = false; }
    let commands = {
        //зашел пользователь;
        userStart,
        //получение сообщений
        msg,
        //количество клиентов сокета
        usersCount,
        //фильтр из AD
        strFilter
        //// История сообщений
        //History: History1,
        ////список пользователей
        //users: users,
        ////отметить пользователей в чате
        //checkUsersInChat: checkUsersInChat,
        ////фильтр с учетом регистра
        //getFilterUsers: users

    };
    
    commands[change.mode](change, user, flItsMe);
    flItsMe = false;

    //if (change.mode == "user") {
    //    user = change;
    //    $welcome.innerHTML = "Добро пожаловать, " +change.displayName + "!" ;
    //}
    //else {
    //    //var $el = document.createElement('p');
    //    //$el.innerHTML = change.displayName;
    //    //$messages.appendChild($el);
    //}
}
function userStart(messages) {
    user = messages.user;
    $welcome.innerHTML = "Добро пожаловать, " + user.displayName + "!";
    usersCount(messages, user);
    //socket.send(JSON.stringify({
    //    mode: 'msg',
    //    user,
    //    join: '' + messages.msg,
    //    ip: '' + messages.ip
    //}));
}
function updateState() {
    function disable() {
        $txt.disabled = true;

        //sendMessage.disabled = true;
        //sendButton.disabled = true;
        //closeButton.disabled = true;
    }
    function enable() {
        $txt.disabled = false;
        //sendMessage.disabled = false;
        //sendButton.disabled = false;
        //closeButton.disabled = false;
    }

    //connectionUrl.disabled = true;
    //connectButton.disabled = true;

    if (!socket) {
        disable();
    } else {
        switch (socket.readyState) {
            case WebSocket.CLOSED:
                $stateLabel.innerHTML = "Закрыт";
                disable();
                //connectionUrl.disabled = false;
                //connectButton.disabled = false;
                break;
            case WebSocket.CLOSING:
                $stateLabel.innerHTML = "Закрывается...";
                disable();
                break;
            case WebSocket.CONNECTING:
                $stateLabel.innerHTML = "Соединяется...";
                disable();
                break;
            case WebSocket.OPEN:
                $stateLabel.innerHTML = "Открыт";
                enable();
                break;
            default:
                $stateLabel.innerHTML = "Неизвестное состояние: " + htmlEscape(socket.readyState);
                disable();
                break;
        }
    }
}

socket.onmessage = function (msg) {
    updateState();
    if (msg.data) {
        console.log(msg);
        printMessage(JSON.parse(msg.data));
    }
};

socket.onclose = function (event) {
    updateState();
};
socket.onerror = updateState;
socket.onopen = updateState;
socket.ondisconect = function (event) {
    updateState();
}

//document.getElementById('send').onclick = function () {
//   // socket.send($user.value + ' : ' + $txt.value);
//    socket.send(JSON.stringify({
//        mode: 'msg',
//        user,
//        msg: '' + $txt.value
//    }));
//    $txt.value = '';
//};

form.addEventListener("submit", e => {
    e.preventDefault();
        socket.send(JSON.stringify({
        mode: 'msg',
        user,
        msg: '' + $txt.value,
        ip: user.ip
    }));
    $txt.value = '';
})



window.addEventListener('beforeunload', function () {
    if (user)
        socket.send(JSON.stringify({
            mode: 'join',
            user,
            join: 'отключился/лась ' + user.displayName,
           ip:  user.ip
        }));
});

function myFilter(e) {
    // Declare variables
    flItsMe = true;
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("myInput");
    filter = input.value; //.toUpperCase();
    if (e.keyCode == 13) {
        socket.send(JSON.stringify({
            mode: 'FindUser',
            strFilter: filter
            // ,
            // userFrom: user
        }));
        return false;
    }
}
//window.addEventListener('onload', function () {
//    if (user)
//        socket.send(JSON.stringify({
//            mode: 'msg',
//            user,
//            msg: 'подключилась/лась ' + user.displayName
//        }));
//});

