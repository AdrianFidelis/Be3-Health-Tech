import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'pacientes', pathMatch: 'full' },
  { path: 'pacientes', loadChildren: () => import('./pacientes/pacientes.module').then(m => m.PacientesModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
