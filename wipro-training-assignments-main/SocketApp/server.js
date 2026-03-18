const express = require('express');
const http = require('http');
const { Server } = require('socket.io');
const logger = require('./middleware/logger');
const app = express();
const server = http.createServer(app);
const io = new Server(server);
app.use(logger);
app.use(express.json());
app.use(express.static('public'));
let notifications = [];
io.on('connection', (socket) => {
    console.log("User connected:", socket.id);
    // Send existing notifications
    socket.emit('loadNotifications', notifications);
    // Admin sends notification
    socket.on('sendNotification', (data) => {
        notifications.push(data);
        // broadcast to everyone
        io.emit('newNotification', data);
    });
    socket.on('disconnect', () => {
        console.log("User disconnected");
    });
});
server.listen(3000, () => {
    console.log("Server running on port 3000");
});