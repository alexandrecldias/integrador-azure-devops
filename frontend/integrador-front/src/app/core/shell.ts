import { Component, OnInit } from '@angular/core'; // Adicionado OnInit para clareza
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatExpansionModule } from '@angular/material/expansion';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive,
    RouterOutlet,
    CommonModule,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatListModule,
    MatExpansionModule
  ],
  templateUrl: './shell.html',
  styleUrl: './shell.css'
})
export class Shell implements OnInit { // Implementa OnInit para indicar o uso do lifecycle hook
  isMenuOpen: boolean = true; // Tipo explícito para clareza
  isMobile: boolean = false; // Tipo explícito para clareza
  isConfiguracoesSubmenuOpen: boolean = true; // Tipo explícito para clareza
  isProcessamentoSubmenuOpen: boolean = true; // Tipo explícito para clareza

  // Usar o construtor para injeção de dependências, se necessário.
  // constructor() {}

  ngOnInit(): void {
    this.detectarMobile();
    this.aplicarTemaSalvo();

    // Recomenda-se desinscrever de event listeners globais em ngOnDestroy
    // para evitar vazamento de memória em aplicações maiores.
    window.addEventListener('resize', this.detectarMobile.bind(this));
  }

  /**
   * Detecta se o dispositivo é móvel baseado na largura da janela
   * e ajusta o estado do menu.
   */
  detectarMobile(): void { // Tipo de retorno explícito
    this.isMobile = window.innerWidth <= 768;
    // O menu só deve fechar automaticamente no modo mobile.
    // Se o menu foi aberto manualmente pelo usuário no desktop, ele deve permanecer aberto.
    if (this.isMobile) {
      this.isMenuOpen = false;
    } else {
      // No desktop, por padrão, o menu pode começar aberto ou seguir a última preferência.
      // Manter a lógica atual de isMenuOpen = !this.isMobile faz ele abrir no desktop.
      this.isMenuOpen = true; // Ou pode ser uma preferência salva
    }
  }

  /**
   * Aplica o tema salvo no localStorage ao corpo do documento.
   */
  aplicarTemaSalvo(): void { // Tipo de retorno explícito
    const temaSalvo = localStorage.getItem('dark-theme');
    if (temaSalvo === 'true') {
      document.body.classList.add('dark-theme');
    } else {
      // Garantir que, se não houver tema salvo ou for 'false', o tema escuro não esteja aplicado.
      document.body.classList.remove('dark-theme');
    }
  }

  /**
   * Alterna o estado de abertura/fechamento do menu principal.
   * Se o menu for fechado, os submenus também são fechados.
   */
  toggleMenu(): void { // Tipo de retorno explícito
    this.isMenuOpen = !this.isMenuOpen;

    if (!this.isMenuOpen) {
      this.isConfiguracoesSubmenuOpen = false;
      this.isProcessamentoSubmenuOpen = false;
    }
  }

  /**
   * Controla a abertura do menu ao passar o mouse, apenas em modo não-móvel.
   * @param hovering Indica se o mouse está sobre o menu.
   */
  onHoverMenu(hovering: boolean): void { // Tipo de retorno explícito e tipo para o parâmetro
    if (!this.isMobile) {
      this.isMenuOpen = hovering;
    }
  }

  /**
   * Alterna o estado de abertura/fechamento do submenu de Configurações.
   */
  toggleConfiguracoesSubmenu(): void { // Tipo de retorno explícito
    this.isConfiguracoesSubmenuOpen = !this.isConfiguracoesSubmenuOpen;
  }

  /**
   * Alterna o estado de abertura/fechamento do submenu de Processamento.
   */
  toggleProcessamentoSubmenu(): void { // Tipo de retorno explícito
    this.isProcessamentoSubmenuOpen = !this.isProcessamentoSubmenuOpen;
  }

  /**
   * Alterna entre o tema claro e escuro e salva a preferência no localStorage.
   */
  toggleTheme(): void { // Tipo de retorno explícito
    document.body.classList.toggle('dark-theme');
    const isDark = document.body.classList.contains('dark-theme');
    localStorage.setItem('dark-theme', String(isDark));
  }
}