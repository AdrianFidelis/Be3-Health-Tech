import { Component, OnInit } from '@angular/core';
import { PacientesService } from '../pacientes.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-form-paciente',
  templateUrl: './form-paciente.component.html'
})
export class FormPacienteComponent implements OnInit {
  form: FormGroup;
  id?: number;
  constructor(
    private fb: FormBuilder,
    private pacientesService: PacientesService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.form = this.fb.group({
      nome: ['', Validators.required],
      sobrenome: ['', Validators.required],
      // Adicione os outros campos depois
    });
  }
  ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    if (this.id) {
      this.pacientesService.buscarPorId(this.id).subscribe(res => this.form.patchValue(res));
    }
  }
  salvar() {
    if (this.id) {
      this.pacientesService.atualizar(this.id, this.form.value).subscribe(() => this.router.navigate(['/pacientes']));
    } else {
      this.pacientesService.criar(this.form.value).subscribe(() => this.router.navigate(['/pacientes']));
    }
  }
}
