let hobbies = [];

function displayHobbies() {

    let list = document.getElementById("hobbyList");
    list.innerHTML = "";

    hobbies.forEach((hobby, index) => {

        let li = document.createElement("li");

        li.innerHTML = `
            ${hobby}
            <div>
                <button onclick="editHobby(${index})">Edit</button>
                <button onclick="deleteHobby(${index})">Delete</button>
            </div>
        `;

        list.appendChild(li);
    });
}

function addHobby(){

    let input = document.getElementById("hobbyInput");
    let hobby = input.value;

    if(hobby === ""){
        alert("Enter a hobby");
        return;
    }

    hobbies.push(hobby);

    input.value="";

    displayHobbies();
}

function deleteHobby(index){

    hobbies.splice(index,1);

    displayHobbies();
}

function editHobby(index){

    let newHobby = prompt("Edit Hobby", hobbies[index]);

    if(newHobby){

        hobbies[index] = newHobby;

        displayHobbies();
    }
}