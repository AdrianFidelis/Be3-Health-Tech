import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PacientesRoutingModule } from './pacientes-routing.module';
import { ListarPacientesComponent } from './listar-pacientes/listar-pacientes.component';
import { FormPacienteComponent } from './form-paciente/form-paciente.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    ListarPacientesComponent,
    FormPacienteComponent
  ],
  imports: [
    CommonModule,
    PacientesRoutingModule,
    ReactiveFormsModule
  ]
})
export class PacientesModule { }
