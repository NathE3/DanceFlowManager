import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { PropuestaComponent } from 'src/app/component/propuesta/propuesta.component';
import { ClaseDTO } from 'src/app/models/claseDTO';
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

  ListaClases: ClaseDTO[] = [];

  constructor(private objetoService: ObjetoService){ 
    this.objetoService.getProductByUsuario().then((porpuestasList: ClaseDTO[]) => {
      this.ListaClases = porpuestasList;
  });
}
}