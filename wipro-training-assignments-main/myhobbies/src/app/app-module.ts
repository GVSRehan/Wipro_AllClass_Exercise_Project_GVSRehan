import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Login } from './component/login/login';
import { PipesDemo } from './component/pipes-demo/pipes-demo';
import { DatesPipe } from './pipes/dates-pipe';

@NgModule({
  declarations: [App, Login, PipesDemo],
  imports: [BrowserModule, AppRoutingModule, FormsModule, DatesPipe],
  providers: [provideBrowserGlobalErrorListeners()],
  bootstrap: [App],
})
export class AppModule {}