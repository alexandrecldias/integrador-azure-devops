import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'; // Importe HttpClient
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

  // Futuramente, adicionaremos métodos para salvar (PUT/POST) aqui
  // saveParametro(parametro: Parametro): Observable<Parametro> {
  //   return this.http.put<Parametro>(`${this.apiUrl}/${parametro.id}`, parametro);
  // }
}