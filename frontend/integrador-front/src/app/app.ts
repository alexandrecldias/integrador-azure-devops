import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  // templateUrl: './app.html',
  template: '<router-outlet></router-outlet>', // <--- ALTERE AQUI!
  styleUrl: './app.css'
})
export class AppComponent  {
  title = signal('integrador-front'); // 'title' agora é um Signal
}
