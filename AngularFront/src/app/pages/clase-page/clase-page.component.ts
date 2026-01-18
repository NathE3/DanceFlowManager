import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location, CommonModule } from '@angular/common'; 
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ObjetoService } from '../../service/objeto.service';
import { AuthService } from '../../service/Auth.service';
import { ClaseDTO } from '../../models/claseDTO';
import { AlumnoDTO } from '../../models/alumnoDTO';

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
        this.cargarAlumnoDesdeToken();
      }
    });
  }

  async cargarClase() {
    if (!this.claseId) return;

    const objetoData = await this.objetoService.getClaseById(this.claseId);
    if (objetoData) {
      this.clase = objetoData;
      if (!Array.isArray(this.clase.alumnosInscritos)) {
        this.clase.alumnosInscritos = [];
      }
    }
  }

  async cargarAlumnoDesdeToken() {
    const idAlumno = this.authService.getAlumnoIdFromToken();
    if (!idAlumno) {
      this.toastr.error('No se pudo obtener el ID del alumno desde el token');
      return;
    }

    try {
      const alumno: AlumnoDTO = await this.objetoService.getAlumnoById(idAlumno);
      this.alumnoActual = alumno;
      console.log('Alumno cargado:', this.alumnoActual);
    } catch (error) {
      console.error('Error cargando alumno:', error);
      this.toastr.error('No se pudo cargar la información del alumno');
    }
  }

  async InscribirseClase() {
    if (!this.claseId || !this.alumnoActual) {
      this.toastr.error("Faltan datos para inscribirse");
      return;
    }

    if (this.isLoading) return;
    this.isLoading = true;

    try {
      const existe = this.clase.alumnosInscritos.some(a => a.id === this.alumnoActual!.id);
      if (existe) {
        this.toastr.info('El alumno ya está inscrito.');
        return;
      }

      const resultado = await this.objetoService.anadirAlumno(this.claseId, this.alumnoActual.id);
      console.log("Resultado backend:", resultado);

      if (resultado) {
        this.clase.alumnosInscritos.push(this.alumnoActual);
        this.toastr.success('Inscripción correcta.');
      } else {
        this.toastr.warning('No se pudo inscribir.');
      }

    } catch (error) {
      console.error('Error al inscribirse:', error);
      this.toastr.error('Error al inscribirse.');
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

