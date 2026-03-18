import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Login } from './component/login/login';
import { PipesDemo } from './component/pipes-demo/pipes-demo';

const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'pipes', component: PipesDemo }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}