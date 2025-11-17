"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .build();

connection.on("ReceiveMessage", function (user, message) {
    const msg = document.createElement("div");
    msg.classList.add("mb-2");
    msg.innerHTML = `<strong>${user}:</strong> ${message}`;
    document.getElementById("messagesList").appendChild(msg);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;
    if (message.trim() !== "") {
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
        document.getElementById("messageInput").value = "";
    }
    event.preventDefault();
});
