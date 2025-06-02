import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PacientesService {
  private apiUrl = 'http://localhost:5000/api/pacientes'; // AJUSTE para o endereço da sua API!

  constructor(private http: HttpClient) {}

  listar(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
  buscarPorId(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }
  criar(paciente: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, paciente);
  }
  atualizar(id: number, paciente: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, paciente);
  }
  inativar(id: number): Observable<any> {
    return this.http.patch<any>(`${this.apiUrl}/${id}/inativar`, {});
  }
}
