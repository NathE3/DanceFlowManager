import { Component } from '@angular/core';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms'; 
import { ObjetoService } from 'src/app/service/objeto.service';
import { CreatePropuestaModel } from 'src/app/models/CreatePropuestaModel';

@Component({
  selector: 'app-anadir',
  standalone: true,
  imports: [FormsModule], 
  templateUrl: './anadir.component.html',
  styleUrls: ['./anadir.component.css']
})
export class AnadirComponent { 
  propuesta: CreatePropuestaModel = { nombre: '', descripcion: '', tipo: '', estado: '' };

  constructor(private objetoService: ObjetoService) {} 

  async crearObjeto() 
  {
    if (!this.propuesta.nombre.trim() || !this.propuesta.descripcion.trim()) {
      alert('Rellene todos los campos, por favor.');
      return;
    }
   
    this.propuesta.estado = 'Pendiente';

    try {
      await this.objetoService.createProduct(this.propuesta).then(result =>{
        alert('Proyecto creado exitosamente');
      
      });
     // if (resultado) {
       // console.log('Producto creado:', resultado);
        alert('Producto creado exitosamente');
    //  } else {
        alert('Error al crear el producto.');
      }catch (error: any) {
      console.error('Error al crear el producto:', error);
     alert('Error al conectar con el servidor.');
   // }           
      }  
 }
     goBack() 
   {
      window.history.back();
   }
}
