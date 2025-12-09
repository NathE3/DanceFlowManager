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
  console.log("➡ CLICK INSCRIBIR", this.claseId);

  // 1️⃣ Validar que exista el ID de la clase
  if (!this.claseId) {
    this.toastr.error("No se obtuvo el ID de la clase");
    return;
  }

  // 2️⃣ Cargar el alumno logueado desde el token
  await this.cargarAlumnoDesdeToken();

  if (!this.alumnoActual) {
    this.toastr.error("No se pudo cargar el alumno desde el token");
    return;
  }

  // 3️⃣ Validar que la clase exista
  if (!this.clase) {
    this.toastr.error("No se pudo cargar la información de la clase");
    return;
  }

  // 4️⃣ Asegurarse de que alumnosInscritos sea un array
  if (!Array.isArray(this.clase.alumnosInscritos)) {
    this.clase.alumnosInscritos = [];
  }

  this.isLoading = true;

  try {
    // 5️⃣ Comprobar si el alumno ya está inscrito
    const existe = this.clase.alumnosInscritos.some(
      a => a?.id === this.alumnoActual?.id
    );

    if (existe) {
      this.toastr.info('El alumno ya está inscrito.');
      return;
    }

    // 6️⃣ Llamar al backend para añadir al alumno
    const resultado = await this.objetoService.anadirAlumno(this.claseId, this.alumnoActual);
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
