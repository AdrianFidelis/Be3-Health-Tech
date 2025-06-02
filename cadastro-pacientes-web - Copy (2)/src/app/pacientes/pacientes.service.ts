import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Paciente {
  id?: number;
  nome: string;
  sobrenome: string;
  dataNascimento: string;
  genero: string;
  cpf?: string;
  rg: string;
  ufRg: string;
  email: string;
  celular: string;
  telefoneFixo?: string;
  convenioId: number;
  numeroCarteirinha: string;
  validadeCarteirinha: string; // "yyyy-MM"
  ativo?: boolean;
}

@Injectable({ providedIn: 'root' })
export class PacientesService {
  private apiUrl = 'https://localhost:7139/api/Paciente';
  //private apiUrl = 'http://localhost:5087/api/Paciente';


  constructor(private http: HttpClient) {}

  listar(): Observable<Paciente[]> {
    return this.http.get<Paciente[]>(this.apiUrl);
  }

  buscarPorId(id: string): Observable<Paciente> {
    return this.http.get<Paciente>(`${this.apiUrl}/${id}`);
  }

  criar(paciente: Paciente): Observable<Paciente> {
    return this.http.post<Paciente>(this.apiUrl, paciente);
  }

  atualizar(id: string, paciente: Paciente): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, paciente);
  }

  inativar(id: string): Observable<void> {
    return this.http.patch<void>(`${this.apiUrl}/${id}/inativar`, {});
  }
}
