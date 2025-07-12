import { Component } from '@angular/core';
import { IntegracaoService } from '../../services/integracao';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr'; // certifique-se que isso está no topo
import { ToastrModule } from 'ngx-toastr'; // certifique-se que isso está no topo


@Component({
  selector: 'app-clonar-tasks',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule, ToastrModule],
    templateUrl: './clonar-tasks-component.html',
    styleUrls: ['./clonar-tasks-component.css']

})
export class ClonarTasksComponent {
  form: FormGroup;
  idWorkItemOrigem: number = 0;
  mensagem: string = '';
  sucesso = false;
  carregando = false;

  constructor(
    private integracaoService: IntegracaoService,
    private toastr: ToastrService, // ✅ injeta o Toastr
    private fb: FormBuilder // ⬅️ injeta o FormBuilder
  ) {
    this.form = this.fb.group({
      idTask: [null, Validators.required]
    });
  }

clonarTask() {
  if (this.form.invalid) {
    console.log('Formulário inválido, não vai clonar.');
    return;
  }
  console.log('Clonar Task disparado para ID:', this.form.value.idTask);

  this.carregando = true;
  const id = this.form.value.idTask;

  this.integracaoService.clonarTask(id).subscribe({
    next: (res) => {
      this.sucesso = true;
      console.log('Sucesso na clonagem, resposta:', res);
      this.toastr.success(
        `ID ${res.idTaskCriada} - ${res.mensagem}`,
        'Clonagem bem-sucedida'
      );
      this.form.reset();
      this.carregando = false;
      console.log('Toast de sucesso exibido e formulário resetado.');
    },
    error: (err: any) => {
      this.sucesso = false;
      const erroMsg = 'Erro: ' + (err?.error || 'Erro inesperado');
      console.error('Erro na clonagem, mensagem:', erroMsg, err);
      this.toastr.error(erroMsg, 'Erro ao clonar');
      this.carregando = false;
      console.log('Toast de erro exibido.');
    }
  });
}
}

