import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListarPacientesComponent } from './listar-pacientes/listar-pacientes.component';
import { FormPacienteComponent } from './form-paciente/form-paciente.component';

const routes: Routes = [
  { path: '', component: ListarPacientesComponent },
  { path: 'novo', component: FormPacienteComponent },
  { path: 'editar/:id', component: FormPacienteComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PacientesRoutingModule { }
