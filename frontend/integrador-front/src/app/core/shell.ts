import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterOutlet, CommonModule],
  templateUrl: './shell.html', // Corrigido para shell.html
  styleUrl: './shell.css' // Corrigido para shell.css
})
export class Shell { // A classe se chama Shell
  isMenuOpen = true; // Para controlar a abertura/fechamento do menu (opcional)
  isConfiguracoesSubmenuOpen = true; // Nova propriedade para o submenu de Configurações
  isProcessamentoSubmenuOpen = false;

  toggleConfiguracoesSubmenu() {
    this.isConfiguracoesSubmenuOpen = !this.isConfiguracoesSubmenuOpen;
  }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }

  toggleProcessamentoSubmenu() {
    this.isProcessamentoSubmenuOpen = !this.isProcessamentoSubmenuOpen;
  }
}