import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ClaseComponent } from 'src/app/component/clase/clase.component';
import { ClaseDTO } from 'src/app/models/claseDTO';
import { AuthService } from 'src/app/service/Auth.service';
import { ObjetoService } from 'src/app/service/objeto.service';
import { RouterModule } from '@angular/router';

@Component(
{
  selector: 'app-principal',
  imports: [CommonModule, ClaseComponent,RouterModule],
  standalone: true,
  templateUrl: './principal.component.html',
  styleUrls: ['./principal.component.css']
})

export class PrincipalComponent{
  
  ListaClases: ClaseDTO[] = [];

  constructor(private objetoService: ObjetoService, private authService: AuthService){ 


    this.objetoService.getAllClases().then((clasesList: ClaseDTO[]) => {
      this.ListaClases = clasesList;
  });
}
}