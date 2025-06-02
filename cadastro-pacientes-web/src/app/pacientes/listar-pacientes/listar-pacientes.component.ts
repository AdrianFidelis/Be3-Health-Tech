import { Component, OnInit } from '@angular/core';
import { PacientesService } from '../pacientes.service';

@Component({
  selector: 'app-listar-pacientes',
  templateUrl: './listar-pacientes.component.html'
})
export class ListarPacientesComponent implements OnInit {
  pacientes: any[] = [];
  constructor(private pacientesService: PacientesService) {}
  ngOnInit() {
    this.pacientesService.listar().subscribe(res => this.pacientes = res);
  }
}
