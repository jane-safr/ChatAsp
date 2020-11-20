var socket,
    $txt = document.getElementById('message'),
    $user = document.getElementById('user'),
    $messages = document.getElementById('messages');
var stateLabel = document.getElementById("stateLabel");

if (typeof (WebSocket) !== 'undefined') {
    const HOST = window.location.host + window.location.pathname; 
    socket = new WebSocket("ws://" + HOST + "/ChatHandler.ashx");
    // socket = new WebSocket("ws://sql_jane/ChatASP/ChatHandler.ashx");
} else {
    socket = new MozWebSocket("ws://sql_jane/ChatASP/ChatHandler.ashx");
}

function updateState() {
    function disable() {
        //sendMessage.disabled = true;
        //sendButton.disabled = true;
        //closeButton.disabled = true;
    }
    function enable() {
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
                stateLabel.innerHTML = "Closed";
                disable();
                //connectionUrl.disabled = false;
                //connectButton.disabled = false;
                break;
            case WebSocket.CLOSING:
                stateLabel.innerHTML = "Closing...";
                disable();
                break;
            case WebSocket.CONNECTING:
                stateLabel.innerHTML = "Connecting...";
                disable();
                break;
            case WebSocket.OPEN:
                stateLabel.innerHTML = "Open";
                enable();
                break;
            default:
                stateLabel.innerHTML = "Unknown WebSocket State: " + htmlEscape(socket.readyState);
                disable();
                break;
        }
    }
}
socket.onmessage = function (msg) {
    updateState();
    var $el = document.createElement('p');
    $el.innerHTML = msg.data;
    $messages.appendChild($el);
};

socket.onclose = function (event) {
    updateState();
    alert('Мы потеряли её. Пожалуйста, обновите страницу');
};
socket.onerror = updateState;

document.getElementById('send').onclick = function () {
    socket.send($user.value + ' : ' + $txt.value);
    $txt.value = '';
};