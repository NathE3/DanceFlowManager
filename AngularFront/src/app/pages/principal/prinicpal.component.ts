import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { PropuestaComponent } from 'src/app/component/propuesta/propuesta.component';
import { propuestaModel } from 'src/app/models/propuestaModel';
import { ObjetoService } from 'src/app/service/objeto.service';

@Component(
{
  selector: 'app-principal',
  imports: [CommonModule, PropuestaComponent, RouterLink],
  standalone: true,
  templateUrl: './principal.component.html',
  styleUrls: ['./principal.component.css']
})

export class PrincipalComponent{

  porpuestasList: propuestaModel[] = [];

  constructor(private objetoService: ObjetoService){ 
    this.objetoService.getProductByUsuario().then((porpuestasList: propuestaModel[]) => {
      this.porpuestasList = porpuestasList;
  });
}
}