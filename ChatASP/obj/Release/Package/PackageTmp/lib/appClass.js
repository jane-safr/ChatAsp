var $txt = document.getElementById('message'),
    $user = document.getElementById('user');
$messages = document.getElementById('messages');
$userCount = document.getElementById('userCount');
//commands

function msg(messages, user) {
    //var $el = document.createElement('p');
    //$el.innerHTML = messages.msg;
    //$messages.appendChild($el);
    var $elPar = document.createElement('par');
    var $el1 = document.createElement('h5');
    
    $el1.style.margin = 0;
    $el1.style.fontSize = "12px";
    var today = new Date();
    if (messages.join)
    {
        $el1.style.color = "rgb(150, 114, 73)";
        $el1.innerHTML = messages.join + ' ' + today.toLocaleString("ru-ru") + ' ' + messages.ip;
        $elPar.appendChild($el1);
    }
    else
    {
        $el1.innerHTML = user.displayName + ' ' + today.toLocaleString("ru-ru") + ' ' + messages.ip;
        $el1.style.color = "rgb(32, 53, 238)";

        var $el = document.createElement('ch');
        $el.style.margin = "15px";
        $el.innerHTML = messages.msg;

        $elPar.appendChild($el1);
        $elPar.appendChild($el);
    }

    $messages.appendChild($elPar);
}

function usersCount(messages, user) {
    $userCount.innerHTML = "  Всего: " + messages.usersCount;
}

