import { Component } from '@angular/core';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {

  title = 'Login';
  username = "";
  password = "";

  myhobbies = ['Angular', 'TypeScript', 'JavaScript', 'chess'];

  onLogin() {
    let login = { username: this.username, password: this.password };
    console.log(login);
  }

}