import { Routes } from '@angular/router';
import { Shell } from './core/shell'; // Corrigido: importando Shell de './core/shell/shell'
import { Parametrizacao } from './configuracoes/parametrizacao/parametrizacao'; // Corrigido: importando Parametrizacao de './configuracoes/parametrizacao/parametrizacao'
import { ClonarTasksComponent } from './pages/clonar-tasks/clonar-tasks-component';

export const routes: Routes = [
  {
    path: '',
    component: Shell,
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./pages/home/home.component').then(m => m.HomeComponent)
      }
    ]
  },
  {
    path: 'configuracoes',
    component: Shell,
    children: [
      {
        path: 'parametrizacao',
        component: Parametrizacao,
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
          import('./pages/clonar-tasks/clonar-tasks-component').then(m => m.ClonarTasksComponent),
      }
    ]
  },
];
