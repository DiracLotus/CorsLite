"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/displayHub").build();

//Disable send button until connection is established
document.getElementById("lemonTea").disabled = true;
document.getElementById("coffeeTime").disabled = true;
document.getElementById("chocolate").disabled = true;

connection.on("ReceiveMessage", function (message) {
    console.log(message);
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${message.text}`;
});

connection.start().then(function () {
    document.getElementById("lemonTea").disabled = false;
    document.getElementById("coffeeTime").disabled = false;
    document.getElementById("chocolate").disabled = false;

}).catch(function (err) {
    return console.error(err.toString());
});
