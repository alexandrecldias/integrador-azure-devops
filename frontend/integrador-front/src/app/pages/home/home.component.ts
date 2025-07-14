import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="home-container">
      <h1>👋 Bem-vindo!</h1>
      <p>Selecione uma opção no menu para começar.</p>
    </div>
  `,
  styles: [`
    .home-container {
      padding: 40px;
      text-align: center;
      color: var(--primary-color);
    }

    .home-container h1 {
      font-size: 2.5rem;
      margin-bottom: 20px;
    }

    .home-container p {
      font-size: 1.2rem;
      color: var(--text-color);
    }
  `]
})
export class HomeComponent {}
