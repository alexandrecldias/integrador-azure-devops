import { Routes } from '@angular/router';
import { Shell } from './core/shell'; // Corrigido: importando Shell de './core/shell/shell'
import { Parametrizacao } from './configuracoes/parametrizacao/parametrizacao'; // Corrigido: importando Parametrizacao de './configuracoes/parametrizacao/parametrizacao'
import { ClonarTasksComponent } from './pages/clonar-tasks/clonar-tasks-component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'configuracoes/parametrizacao',
    pathMatch: 'full'
  },
  {
    path: 'configuracoes',
    component: Shell, // Usando a classe Shell
    children: [
      {
        path: 'parametrizacao',
        component: Parametrizacao, // Usando a classe Parametrizacao
        title: 'Parametrização'
      }
    ]
  },
  {
    path: 'processamento',
    component: Shell,
    children: [
      {
        path: 'clonar-tasks',
        loadComponent: () =>
          import('./pages/clonar-tasks/clonar-tasks-component')
          .then(m => m.ClonarTasksComponent),
      }
    ]
  },
];