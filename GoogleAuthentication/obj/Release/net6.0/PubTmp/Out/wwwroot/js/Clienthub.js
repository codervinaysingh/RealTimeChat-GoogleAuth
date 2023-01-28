"use strict"

var connection = new signalR.HubConnectionBuilder().withUrl("/ConectedHub").build();
var selectedUser = ''
connection.on("ReceiveMessage", function (res) {
    debugger;
    var result = JSON.parse(res)
    console.log(result)
    var ul = document.getElementById("message_list").innerHTML
    var li = document.createElement("li");
    //li.innerHTML = result[0];
    var user = document.getElementById("txt_userId").innerHTML
    var len = (Object.keys(result).length )- 1;
    console.log("len",len)
    var lastMessage = result[len];
    document.getElementById(lastMessage.UserName).innerHTML = lastMessage.Message +'<span class="fa-stack fa-3x" data-count="1">';
    //document.getElementById("li").remove()
    bindChat(result,user);
});
function bindChat(result, userName) {
    document.getElementById("message_list").innerHTML = '';
    var liBuilder = '';
    for (var i = 0; i < result.length; i++) {
        liBuilder += ` <li class="${result[i].UserName == userName ? 'text-start list-unstyled' : 'text-end list-unstyled'} ">${result[i].Message}-${result[i].CreatedDate}</li>`
    }
    document.getElementById("message_list").innerHTML = liBuilder
}

connection.start().then(function (res) {
    document.getElementById("btn_send").disabled = false;
    getAllUser();
}).catch(function (err) {
    return console.error(err.toString());
})



document.getElementById("btn_send").addEventListener("click", function (event) {
    var user = document.getElementById("txt_userId").innerHTML
    var message = document.getElementById("txt_message").value
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    document.getElementById("txt_message").value = '';
    var li = document.createElement("li");
    li.className = 'text-end list-unstyled';
    li.textContent = message;
    document.getElementById("message_list").appendChild(li);
    event.preventDefault();
})
async function getAllUser() {
    var res = await fetch('chat/GetALLUser', {
        method: 'GET',
    })
    const result = await res.json();
    console.log("user", result)
    document.getElementById("user_list").innerHTML = ''

    var htmlBuilder = ''
    for (var i = 0; i < result.length; i++) {
        htmlBuilder += ` <div class="form-control m-2" onclick="clickUser('${result[i].userName}')" >
                        <div>
                            <h6>${result[i].userName}</h6>
                            <span id='${result[i].userName}'>${result[i].msg} </span>

                        </div>
                    </div>`;
    }

    document.getElementById("user_list").innerHTML = htmlBuilder;
}
async function clickUser(username) {
    selectedUser = username;
    document.getElementById("txt_userId").innerHTML = username
    var res = await fetch(`chat/GetALLChat?selectUser=${username}`, {
        method: 'GET',
    })
    const result = await res.json();
    bindChat(result, username);
}