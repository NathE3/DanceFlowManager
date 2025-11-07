import { Component, Input } from '@angular/core';
import { ClaseDTO } from '../../models/claseDTO';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-clase',
  imports: [RouterModule, CommonModule],
  standalone: true,
  templateUrl: './clase.component.html',
  styleUrls: ['./clase.component.css']
})
export class ClaseComponent {
  @Input() clase!: ClaseDTO;
}
