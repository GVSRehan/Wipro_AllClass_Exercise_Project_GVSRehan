const express = require("express");
const http = require("http");
const { Server } = require("socket.io");
const fs = require("fs");
const path = require("path");

const app = express();
const server = http.createServer(app);
const io = new Server(server);

app.use(express.json());
app.use(express.static("public"));

const dbPath = path.join(__dirname, "Database/db.json");

function readDB() {
    return JSON.parse(fs.readFileSync(dbPath));
}

function writeDB(data) {
    fs.writeFileSync(dbPath, JSON.stringify(data, null, 2));
}

io.on("connection", (socket) => {

    const userId = socket.handshake.query.userId;

    const db = readDB();

    const user = db.users.find(u => u.id === userId);

    if(!user){
        console.log("Unknown user");
        return;
    }

    console.log("User connected:", user.firstName);

    socket.emit("loadNotifications", db.notifications);

    socket.on("sendNotification", (data) => {

        if(user.roleId !== "71x48gz3u05" && user.roleId !== "admin2"){
            console.log("Only admin can send notifications");
            return;
        }

        const db = readDB();

        db.notifications.push(data);

        writeDB(db);

        io.emit("newNotification", data);

        console.log("Notification saved");

    });

    socket.on("disconnect", () => {
        console.log(user.firstName,"disconnected");
    });

});

server.listen(3000, () => {
    console.log("Server running on http://localhost:3000");
});