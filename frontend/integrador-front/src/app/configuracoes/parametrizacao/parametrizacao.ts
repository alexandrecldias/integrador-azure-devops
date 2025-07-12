import { Component, OnInit } from '@angular/core'; // Importe OnInit
import { CommonModule } from '@angular/common'; // Para ngFor e ngIf
import { FormsModule } from '@angular/forms'; // Para ngModel (edição bidirecional)

import { ParametroService } from '../../services/parametro.service'; // Importe o serviço
import { Parametro } from '../../models/parametro'; // Importe a interface

@Component({
  selector: 'app-parametrizacao',
  standalone: true,
  imports: [CommonModule, FormsModule], // Adicione CommonModule e FormsModule
  templateUrl: './parametrizacao.html',
  styleUrl: './parametrizacao.css'
})
export class Parametrizacao implements OnInit { // Implemente OnInit
  parametros: Parametro[] = []; // Array para armazenar os dados da API
  isLoading = true; // Para mostrar um indicador de carregamento

  constructor(private parametroService: ParametroService) { } // Injeta o serviço

  ngOnInit(): void {
    this.loadParametros(); // Carrega os parâmetros ao inicializar o componente
  }

  loadParametros(): void {
    this.isLoading = true;
    this.parametroService.getParametros().subscribe({
      next: (data) => {
        this.parametros = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Erro ao carregar parâmetros:', err);
        this.isLoading = false;
        // Você pode adicionar uma mensagem de erro na UI aqui
      }
    });
  }

  // Futuras funções para Salvar e Voltar
  salvar(): void {
    console.log('Botão Salvar clicado!', this.parametros);
    // Aqui virá a lógica de chamar a API de salvar
  }

  voltar(): void {
    console.log('Botão Voltar clicado!');
    // Aqui virá a lógica de navegação ou descarte de alterações
    // Ex: this.router.navigate(['/']); // Se tiver injetado Router
  }
}