import { Component, Input } from '@angular/core';
import { propuestaModel } from '../../models/propuestaModel';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-propuesta',
  imports: [RouterModule, CommonModule],
  standalone: true,
  templateUrl: './propuesta.component.html',
  styleUrls: ['./propuesta.component.css']
})
export class PropuestaComponent {
  @Input() propuesta!: propuestaModel;
}
