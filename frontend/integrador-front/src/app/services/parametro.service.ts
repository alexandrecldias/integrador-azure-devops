import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'; // Importe HttpClient
import { Observable } from 'rxjs'; // Importe Observable
import { Parametro } from '../models/parametro'; // Importe a interface Parametro

@Injectable({
  providedIn: 'root'
})
export class ParametroService {
  private apiUrl = '/api/Parametro'; // URL da sua API

  constructor(private http: HttpClient) { } // Injeta o HttpClient

  /**
   * Retorna todos os parâmetros da API.
   */
  getParametros(): Observable<Parametro[]> {
    return this.http.get<Parametro[]>(this.apiUrl);
  }

/**
   * Atualiza o valor de um parâmetro específico via PUT.
   * Exemplo: salvar o parâmetro "RESPONSAVEL_TASK" com valor "alexandrecldias"
   */
   saveParametro(nomeParametro: string, valor: string): Observable<any> {
      const url = `${this.apiUrl}/${nomeParametro}`;
      const body = { valor };
      const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

      return this.http.put(url, body, { headers });
    } 
}