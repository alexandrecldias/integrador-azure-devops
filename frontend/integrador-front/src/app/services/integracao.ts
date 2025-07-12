import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class IntegracaoService {
  private apiUrl = '/api/Integracao'; // Altere se a rota do backend for diferente

  constructor(private http: HttpClient) {}

  clonarTask(idWorkItemOrigem: number): Observable<{ idTaskCriada: number; mensagem: string }> {
  return this.http.post<{ idTaskCriada: number; mensagem: string }>(
    this.apiUrl + '/clonar-task',
    { idWorkItemOrigem }
  );
  }

}
