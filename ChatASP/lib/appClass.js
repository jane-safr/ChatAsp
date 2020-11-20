var $txt = document.getElementById('message'),
    $user = document.getElementById('user');
$messages = document.getElementById('messages');
$userCount = document.getElementById('userCount');
$tableUsers = document.getElementById("tableUsers");
$findAD = document.getElementById('findAD');
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
function strFilter(messages, user, flItsMe) {

    if (flItsMe && messages.accountName == user.accountName) {
        $findAD.innerHTML = '';
        for (let i = 0; i < messages.str.length; i++) {
            var $el1 = document.createElement('h5');
            $el1.innerHTML = messages.str[i].accountName + ' ' + messages.str[i].displayName + ' ' + messages.str[i].extensionattribute3;
            // $elPar.appendChild($el1);
            $findAD.appendChild($el1);
        }
    }


 
}

const lettersUsers = ["accountName", "displayName", "extensionattribute3"];
const createRowUsers = (accountName, displayName, extensionattribute3) => {
    const trUsers = document.createElement("tr");
    trUsers.innerHTML =
        lettersUsers
            .map(col => `<td>${col}</td>`)
            .join("");
            //.map(col => `<td><output ` + ((`${id}`).split(' ')[0] < 0 ? 'onchange="checkChat()"' : '') + (`${col}` == "id" ? 'class="id"' : '') + ` id="${col}${id}" type="text"></td>`)
            //.join("");
    $tableUsers.appendChild(trUsers);
    //lettersUsers.forEach(col => {
    //    const cell = col + id;
    //    const input = document.getElementById(cell);
    //    switch (col) {
    //        case "id":
    //            input.value = id;
    //            break;
    //        case "v":

    //            let element1 = document.createElement("input");
    //            element1.type = "checkbox";
    //            element1.name = id;
    //            element1.style = "width:15px;height:15px;"
    //            input.appendChild(element1);

    //            break;
    //        case "":
    //            if (id > 0)
    //                insImg(input, (online ? 'green' : 'red'));
    //            break;
    //        case "Login":
    //            input.value = login;
    //            break;
    //        case "ФИО":
    //            input.value = FIO;
    //            break;
    //    }
    //    cellsUsers[cell] = input;

    //});
};
