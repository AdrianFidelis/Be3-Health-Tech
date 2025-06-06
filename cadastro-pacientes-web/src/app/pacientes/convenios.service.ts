import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ConveniosService {
  private apiUrl = 'https://localhost:7139/api/Convenio';
  //private apiUrl = 'http://localhost:5087/api/Convenio';

  constructor(private http: HttpClient) {}

  listar(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
