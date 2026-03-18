const express = require("express");
const fs = require("fs");
const path = require("path");
const events = require("events");

const app = express();
const eventEmitter = new events.EventEmitter();

app.use(express.static(path.join(__dirname, "../public")));
app.use(express.json());

const folderPath = path.join(__dirname, "../files");


// Event: server started
eventEmitter.on("serverStarted", (port) => {
  console.log(`Server started on port ${port}`);
});

// Event: file created
eventEmitter.on("fileCreated", (file) => {
  console.log(`File created: ${file}`);
});

// Event: file read
eventEmitter.on("fileRead", (file) => {
  console.log(`File read: ${file}`);
});

// Event: file updated
eventEmitter.on("fileUpdated", (file) => {
  console.log(`File updated: ${file}`);
});

// Event: file renamed
eventEmitter.on("fileRenamed", (oldName, newName) => {
  console.log(`File renamed from ${oldName} to ${newName}`);
});

// Event: file deleted
eventEmitter.on("fileDeleted", (file) => {
  console.log(`File deleted: ${file}`);
});


// CREATE FILE
app.post("/create", (req, res) => {

  const filePath = path.join(folderPath, "demo.txt");

  if (fs.existsSync(filePath)) {
    return res.send("File already exists");
  }

  fs.writeFile(filePath, "Hello this is a new file", (err) => {
    if (err) return res.send("Error creating file");

    eventEmitter.emit("fileCreated", "demo.txt");

    res.send("File created successfully");
  });

});


// READ FILE
app.get("/read", (req, res) => {

  const filePath = path.join(folderPath, "demo.txt");

  fs.readFile(filePath, "utf8", (err, data) => {
    if (err) return res.send("File not found");

    eventEmitter.emit("fileRead", "demo.txt");

    res.send(data);
  });

});


// UPDATE FILE
app.post("/update", (req, res) => {

  const filePath = path.join(folderPath, "demo.txt");

  fs.appendFile(filePath, "\nNew line added", (err) => {
    if (err) return res.send("Error updating file");

    eventEmitter.emit("fileUpdated", "demo.txt");

    res.send("File updated");
  });

});


// RENAME FILE
app.post("/rename", (req, res) => {

  const oldPath = path.join(folderPath, "demo.txt");
  const newPath = path.join(folderPath, "newDemo.txt");

  fs.rename(oldPath, newPath, (err) => {
    if (err) return res.send("Rename failed");

    eventEmitter.emit("fileRenamed", "demo.txt", "newDemo.txt");

    res.send("File renamed");
  });

});


// DELETE FILE
app.delete("/delete", (req, res) => {

  const filePath = path.join(folderPath, "newDemo.txt");

  fs.unlink(filePath, (err) => {
    if (err) return res.send("Delete failed");

    eventEmitter.emit("fileDeleted", "newDemo.txt");

    res.send("File deleted");
  });

});


// START SERVER
const PORT = 3000;

app.listen(PORT, () => {
  eventEmitter.emit("serverStarted", PORT);
});