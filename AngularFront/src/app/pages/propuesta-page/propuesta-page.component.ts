import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ObjetoService } from 'src/app/service/objeto.service';
import { ClaseDTO } from '../../models/claseDTO';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AlumnoDTO } from 'src/app/models/alumnoDTO';

@Component({
  selector: 'app-propuesta-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './propuesta-page.component.html',
  styleUrls: ['./propuesta-page.component.css']
})
export class ClasePageComponent implements OnInit {
  claseId: string | null = null;
  clase: ClaseDTO = { id_clase: '', nombre: '', descripcion: '',tipo:'',fechaClase: new Date(),idProfesor: '',alumnosInscritos:[] };
  alumnoActual : AlumnoDTO | undefined

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private objetoService: ObjetoService
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.claseId = id;
        this.cargarClase();
      }
    });
  }

  async cargarClase() {
    if (this.claseId) {
      const objetoData = await this.objetoService.getProductById(this.claseId);
      if (objetoData) {
        this.clase = objetoData;
      }
    }
  }

  
async InscribirseClase() {
  if (this.claseId && this.alumnoActual) {
    try {
      const existe = this.clase.alumnosInscritos.some(a => a.id === this.alumnoActual.id);
      if (!existe) {
        this.clase.alumnosInscritos.push(this.alumnoActual);
      }

      await this.objetoService.updateProduct(this.claseId, this.clase);
      alert('Se ha inscrito correctamente.');
    } catch (error) {
      console.error('Hubo un error al inscribirse a la clase.', error);
      alert('Hubo un error al inscribirse a la clase.');
    }
  }
}


 
async eliminarInscripcion() {
  if (this.claseId && this.alumnoActual) {
    const confirmacion = confirm('¿Estás seguro de que deseas eliminar tu inscripción?');
    if (confirmacion) {
      try {
        this.clase.alumnosInscritos = this.clase.alumnosInscritos.filter(a => a.id !== this.alumnoActual.id);

        await this.objetoService.updateProduct(this.claseId, this.clase);
        alert('Inscripción eliminada correctamente.');
        this.location.back();
      } catch (error) {
        console.error('Error al eliminar la inscripción', error);
        alert('Hubo un error al eliminar la inscripción.');
      }
    }
  }
}

  goBack() {
    this.location.back();
  }
}
