import { Component, OnInit } from '@angular/core';
import { PacientesService } from '../pacientes.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-listar-pacientes',
  templateUrl: './listar-pacientes.component.html'
})
export class ListarPacientesComponent implements OnInit {
  pacientes: any[] = [];
  loading = false;

  constructor(
    private pacientesService: PacientesService,
    private router: Router
  ) {}

  ngOnInit() {
    this.buscarPacientes();
  }

  buscarPacientes(): void {
    this.loading = true;
    this.pacientesService.listar().subscribe({
      next: (res) => {
        this.pacientes = res;
        this.loading = false;
      },
      error: () => {
        alert('Erro ao buscar pacientes!');
        this.loading = false;
      }
    });
  }

  editar(id: string): void {
    this.router.navigate(['/pacientes/editar', id]);
  }

  inativar(paciente: any): void {
  if (confirm('Deseja realmente inativar este paciente?')) {
    // Manda tudo igual, só altera o ativo para false
    const atualizado = { ...paciente, ativo: false };
    this.pacientesService.atualizar(paciente.id, atualizado).subscribe(() => {
      alert('Paciente inativado!');
      this.buscarPacientes();
    });
  }
}

ativar(paciente: any): void {
  if (confirm('Deseja ativar este paciente?')) {
    const atualizado = { ...paciente, ativo: true };
    this.pacientesService.atualizar(paciente.id, atualizado).subscribe(() => {
      alert('Paciente ativado!');
      this.buscarPacientes();
    });
  }
}
}
