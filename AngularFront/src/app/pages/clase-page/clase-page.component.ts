import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ObjetoService } from 'src/app/service/objeto.service';
import { ClaseDTO } from '../../models/claseDTO';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AlumnoDTO } from 'src/app/models/alumnoDTO';
import { AuthService } from 'src/app/service/Auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-clase-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './clase-page.component.html',
  styleUrls: ['./clase-page.component.css']
})
export class ClasePageComponent implements OnInit {
  claseId: string | null = null;
  clase: ClaseDTO = {
    id: '',
    nombre: '',
    descripcion: '',
    tipo: '',
    fechaClase: new Date(),
    idProfesor: '',
    alumnosInscritos: []
  };
  alumnoActual: AlumnoDTO | undefined; 
  isLoading: boolean = false; 
  isLoadingEliminar: boolean = false; 

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private objetoService: ObjetoService,
    private authService: AuthService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.claseId = id;
        this.cargarClase();
      }
    });

    this.cargarAlumnoDesdeToken(); 
  }

  async cargarClase() {
    if (this.claseId) {
      const objetoData = await this.objetoService.getClaseById(this.claseId);
      if (objetoData) {
        this.clase = objetoData;
      }
    }
  }

  async cargarAlumnoDesdeToken() {
    const idAlumno = this.authService.getAlumnoIdFromToken();
    if (idAlumno) {
      try {
        const alumno:AlumnoDTO = await this.objetoService.getAlumnoById(idAlumno);
        this.alumnoActual = alumno;
      } catch (error) {
        this.toastr.error('Error al cargar el alumno');
        this.toastr.warning('No se pudo cargar la información del alumno.');
      }
    }
  }

async InscribirseClase() {
  if (!this.claseId || !this.alumnoActual) return;
  if (this.isLoading) return;

  this.isLoading = true;

  try {
    const existe = this.clase.alumnosInscritos.some(a => a.id === this.alumnoActual!.id);
    if (existe) {
      this.toastr.info('El alumno ya está inscrito en esta clase.');
      return;
    }

    const resultado = await this.objetoService.anadirAlumno(this.claseId, this.alumnoActual);
    if (resultado) {
      this.clase.alumnosInscritos.push(this.alumnoActual);
      this.toastr.success('Se ha inscrito correctamente.');
    } else {
      this.toastr.warning('No se pudo inscribir al alumno.');
    }
  } catch (error) {
    this.toastr.error('Hubo un error al inscribirse a la clase.');
  } finally {
    this.isLoading = false;
  }
}

async eliminarInscripcion() {
  if (!this.claseId || !this.alumnoActual) return;
  if (this.isLoadingEliminar) return;

  const confirmacion = confirm('¿Estás seguro de que deseas eliminar tu inscripción?');
  if (!confirmacion) return;

  this.isLoadingEliminar = true;

  try {
    const resultado = await this.objetoService.eliminarAlumno(this.claseId, this.alumnoActual.id);
    if (resultado) {
      this.clase.alumnosInscritos = this.clase.alumnosInscritos.filter(a => a.id !== this.alumnoActual!.id);
      this.toastr.success('Inscripción eliminada correctamente.');
    } else {
      this.toastr.warning('No se pudo eliminar la inscripción.');
    }
  } catch (error) {
    console.error(error);
    this.toastr.error('Hubo un error al eliminar la inscripción.');
  } finally {
    this.isLoadingEliminar = false;
  }
}

  goBack() {
    this.location.back();
  }
}
