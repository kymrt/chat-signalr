
var WebChat = {
    InitialAll: function () {
        WebChat.WebSocket.Connect();
        WebChat.dcGet("chat-message").addEventListener("keyup", function (event) {
            event.preventDefault();
            if (event.keyCode === 13) {
                WebChat.dcGet("chat-send").click();
            }
        });
        WebChat.dcGet("login-nickname").addEventListener("keyup", function (event) {
            event.preventDefault();
            if (event.keyCode === 13) {
                WebChat.dcGet("button-login").click();
            }
        });
    },
    WebSocket: {
        Connection: null,
        Connect: function () {
            WebChat.WebSocket.Connection = new signalR.HubConnectionBuilder().withUrl("/websocket").build();

            WebChat.WebSocket.Connection.on("Login", function (state) {
                if (state) {
                    WebChat.dcGet("login-section").style.display = "none";
                    WebChat.dcGet("chat-section").style.display = "block";
                    var message = "Hello!"; 
                    WebChat.WebSocket.Connection.invoke("Message", message);
                    WebChat.dcGet("chat-messages").scrollTop = WebChat.dcGet("chat-messages").scrollHeight;
                }
            });

            WebChat.WebSocket.Connection.on("Logout", function () {
                WebChat.dcGet("login-section").style.display = "block";
                WebChat.dcGet("chat-section").style.display = "none";
            });

            WebChat.WebSocket.Connection.on("MessageMine", function (id, nickname, message, time) {
                WebChat.dcGet("chat-messages").insertAdjacentHTML("beforeend", WebChat.MakeBubbleMine(id, nickname, message, time));
                WebChat.dcGet("chat-messages").scrollTop = WebChat.dcGet("chat-messages").scrollHeight;
            });

            WebChat.WebSocket.Connection.on("MessageOther", function (id, nickname, message, time) {
                WebChat.dcGet("chat-messages").insertAdjacentHTML("beforeend", WebChat.MakeBubbleOther(id, nickname, message, time));                

                WebChat.dcGet("chat-messages").scrollTop = WebChat.dcGet("chat-messages").scrollHeight;
            });

            WebChat.WebSocket.Connection.on("Error", function (message) {
                alert(message);
            });

            WebChat.WebSocket.Connection.start().then(function () {
                console.log("Connected!");
            });
        },
        Request: {
            Login: function () {
                var nickname = WebChat.dcGet("login-nickname").value;
                WebChat.WebSocket.Connection.invoke("Login", nickname, Request.UserHostAddress);
            },
            Logout: function () {
                WebChat.WebSocket.Connection.invoke("Logout");
            },
            Message: function () {
                var message = WebChat.dcGet("chat-message").value;
                WebChat.WebSocket.Connection.invoke("Message", message);
                WebChat.dcGet("chat-message").value = "";
            }
        },
    },
    dcGet: function (dom) {
        return document.getElementById(dom);
    },
    MakeBubbleOther: function (id, nickname, message, time) {
        return "<div id=\"chat-message-" + id + "\" class=\"other messages\"><div class=\"message\"><strong>" + nickname + ":</strong> " + message + " <sub>" + time + "</sub></div></div>";
    },
    MakeBubbleMine: function (id, nickname, message, time) {
        return "<div id=\"chat-message-" + id + "\" class=\"mine messages\"><div class=\"message\"><strong>" + nickname + ":</strong> " + message + " <sub>" + time + "</sub></div></div>";
    }
}