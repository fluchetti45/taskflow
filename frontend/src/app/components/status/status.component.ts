import { Component, inject, Input } from '@angular/core';
import { Status } from '../../models/status.model';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { StatusService } from '../../services/status.service';

@Component({
  selector: 'app-status',
  imports: [ReactiveFormsModule, DatePipe],
  templateUrl: './status.component.html',
  styleUrl: './status.component.css'
})
export class StatusComponent {
  @Input() status! : Status;
  private _statusService = inject(StatusService);

  handleDelete(roleId : number) {
    this._statusService.deleteStatus(roleId)
  }
  
}
