import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PacientesService } from '../pacientes.service';
import { ConveniosService } from '../convenios.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-form-paciente',
  templateUrl: './form-paciente.component.html'
})
export class FormPacienteComponent implements OnInit {
  form!: FormGroup;
  convenios: any[] = [];
  id?: string;

  ufs = [
    { sigla: 'AC', valor: 0 },
    { sigla: 'AL', valor: 1 },
    { sigla: 'AM', valor: 2 },
    { sigla: 'AP', valor: 3 },
    { sigla: 'BA', valor: 4 },
    { sigla: 'CE', valor: 5 },
    { sigla: 'DF', valor: 6 },
    { sigla: 'ES', valor: 7 },
    { sigla: 'GO', valor: 8 },
    { sigla: 'MA', valor: 9 },
    { sigla: 'MG', valor: 10 },
    { sigla: 'MS', valor: 11 },
    { sigla: 'MT', valor: 12 },
    { sigla: 'PA', valor: 13 },
    { sigla: 'PB', valor: 14 },
    { sigla: 'PE', valor: 15 },
    { sigla: 'PI', valor: 16 },
    { sigla: 'PR', valor: 17 },
    { sigla: 'RJ', valor: 18 },
    { sigla: 'RN', valor: 19 },
    { sigla: 'RO', valor: 20 },
    { sigla: 'RR', valor: 21 },
    { sigla: 'RS', valor: 22 },
    { sigla: 'SC', valor: 23 },
    { sigla: 'SE', valor: 24 },
    { sigla: 'SP', valor: 25 },
    { sigla: 'TO', valor: 26 }
  ];

  constructor(
    private fb: FormBuilder,
    private pacientesService: PacientesService,
    private conveniosService: ConveniosService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      nome: ['', Validators.required],
      sobrenome: ['', Validators.required],
      dataNascimento: ['', Validators.required],
      genero: [0, Validators.required],
      cpf: ['', Validators.required],
      rg: ['', Validators.required],
      ufRg: [0, Validators.required],
      email: ['', [Validators.required, Validators.email]],
      celular: [''],
      telefoneFixo: [''],
      convenioId: [0, Validators.required],
      numeroCarteirinha: ['', Validators.required],
      validadeCarteirinha: ['', Validators.required]
      // NÃO inclui ativo aqui: só vai no PUT
    }, { validators: [peloMenosUmTelefoneValidator] });

    this.id = this.route.snapshot.params['id'];
    this.conveniosService.listar().subscribe(res => this.convenios = res);

    if (this.id) {
      this.pacientesService.buscarPorId(this.id).subscribe(res => {
        this.form.patchValue({
          ...res,
          genero: Number(res.genero),
          ufRg: Number(res.ufRg),
          convenioId: Number(res.convenioId),
          dataNascimento: this.formatarDataParaInput(res.dataNascimento),
          validadeCarteirinha: this.formatarMesParaInput(res.validadeCarteirinha)
        });
      });
    }
  }

  salvar() {
    if (this.form.valid) {
      // Regra: pelo menos 1 telefone, o outro vai vazio se não preenchido
      const celular = this.form.value.celular?.trim() || null;
      const telefoneFixo = this.form.value.telefoneFixo?.trim() || null;

      const payload: any = {
        nome: this.form.value.nome,
        sobrenome: this.form.value.sobrenome,
        dataNascimento: this.formatarDataParaSalvar(this.form.value.dataNascimento),
        genero: Number(this.form.value.genero),
        cpf: aplicarMascaraCpf(this.form.value.cpf),
        rg: this.form.value.rg,
        ufRg: Number(this.form.value.ufRg),
        email: this.form.value.email,
        celular,
        telefoneFixo,
        convenioId: Number(this.form.value.convenioId),
        numeroCarteirinha: this.form.value.numeroCarteirinha,
        validadeCarteirinha: this.formatarMesParaSalvar(this.form.value.validadeCarteirinha)
      };
      console.log('CPF enviado:', this.form.value.cpf);


      // No PUT (atualizar), pode enviar ativo
      if (this.id) {
        this.pacientesService.atualizar(this.id, {
          ...payload,
          ativo: this.form.value.ativo !== undefined ? this.form.value.ativo : true
        }).subscribe(() => {
          alert('Paciente atualizado!');
          this.router.navigate(['/pacientes']);
        });
      } else {
        // No POST, NUNCA envie ativo!
        this.pacientesService.criar(payload).subscribe(() => {
          alert('Paciente cadastrado!');
          this.router.navigate(['/pacientes']);
        });
      }
    }
  }

  private formatarDataParaInput(data: string): string {
    if (!data) return '';
    if (data.length > 10) return data.substring(0, 10);
    return data;
  }

  private formatarMesParaInput(validade: string): string {
    if (!validade) return '';
    if (validade.includes('/')) {
      const [mm, yyyy] = validade.split('/');
      return `${yyyy}-${mm.padStart(2, '0')}`;
    }
    if (validade.length > 7) return validade.substring(0, 7);
    return validade;
  }

  private formatarDataParaSalvar(data: any): string {
    if (!data) return '';
    if (typeof data === 'string' && data.length === 10) {
      return new Date(data).toISOString();
    }
    if (data instanceof Date) {
      return data.toISOString();
    }
    return data;
  }

  private formatarMesParaSalvar(mes: string): string {
    if (!mes) return '';
    if (mes.length === 7 && mes.includes('-')) {
      const [yyyy, mm] = mes.split('-');
      return `${mm}/${yyyy}`;
    }
    return mes;
  }
}

// Custom validator: exige pelo menos um telefone
function peloMenosUmTelefoneValidator(group: FormGroup) {
  const celular = group.get('celular')?.value;
  const telefoneFixo = group.get('telefoneFixo')?.value;
  return (!!celular || !!telefoneFixo) ? null : { peloMenosUmTelefone: true };
}
function aplicarMascaraCpf(cpf: string): string {
  // Remove tudo que não é dígito e aplica a máscara
  cpf = cpf.replace(/\D/g, '');
  return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
}

