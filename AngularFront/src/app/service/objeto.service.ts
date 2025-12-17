import { Injectable } from '@angular/core';
import { ClaseDTO } from '../models/claseDTO';
import { AlumnoDTO } from '../models/alumnoDTO';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ObjetoService {
  readonly baseUrl = 'https://localhost:7777/DanceFlowApi/Clase';
  readonly baseUrl2 = 'https://localhost:7777/DanceFlowApi/Alumno';

  constructor(private toastr: ToastrService ) { }

  private getAuthHeaders(): { [key: string]: string } {
    const token = localStorage.getItem('token');
    return {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    };
  }

  async getAllClases(): Promise<ClaseDTO[]> {
    const response = await fetch(this.baseUrl, {
      method: 'GET',
      headers: this.getAuthHeaders()
    });
    return (await response.json()) ?? [];
  }

  async getClaseById(id: string): Promise<ClaseDTO> {
    const response = await fetch(`${this.baseUrl}/${id}`, {
      method: 'GET',
      headers: this.getAuthHeaders()
    });
    return (await response.json()) as ClaseDTO;
  }

 async getAlumnoById(id: string): Promise<AlumnoDTO> {
  const response = await fetch(`${this.baseUrl2}/${id}`, {
    method: 'GET',
    headers: this.getAuthHeaders()
  });

  if (!response.ok) {
    this.toastr.error('Error al obtener el alumno');
  }

  return (await response.json()) as AlumnoDTO;
}


async getClaseByUsuario(alumnoId: string): Promise<ClaseDTO[]> {
  try {
    const response = await fetch(`${this.baseUrl}/alumno/${alumnoId}`, {
      method: "GET",
      headers: {
        ...this.getAuthHeaders(),
        "Content-Type": "application/json"
      }
    });

    if (!response.ok) {
      throw new Error(`Error HTTP: ${response.status}`);
    }

    const data = await response.json();
    return data as ClaseDTO[];

  } catch (error) {
    console.error("Error obteniendo clases por usuario:", error);
    throw error;
  }
}


async anadirAlumno(idClase: string, idAlumno: string): Promise<boolean> {
  try {
    const response = await fetch(`${this.baseUrl}/${idClase}/anadir-alumno/${idAlumno}`, {
      method: "POST",
      headers: {
        ...this.getAuthHeaders()
      }
    });

    if (!response.ok) {
      this.toastr.error(`Error HTTP: ${response.status}`);
      return false;
    }

    const result = await response.json();
    return Boolean(result);
  } catch (error) {
    this.toastr.error("Error en la petición");
    return false;
  }
}


async eliminarAlumno(idClase: string, idAlumno: string): Promise<boolean> {
  try {
    const response = await fetch(`${this.baseUrl}/${idClase}/eliminar-alumno/${idAlumno}`, {
      method: "DELETE",
      headers: {
        ...this.getAuthHeaders()
      }
    });

    if (!response.ok) {
      this.toastr.error(`Error HTTP: ${response.status}`);
      return false;
    }

    const result = await response.json();
    return Boolean(result);
  } catch (error) {
    this.toastr.error("Error en la petición");
    return false;
  }
}



  async updateClase(id: string, partialClase: Partial<ClaseDTO>): Promise<ClaseDTO> {
    const response = await fetch(`${this.baseUrl}/${id}`, {
      method: "PUT",
      headers: this.getAuthHeaders(),
      body: JSON.stringify(partialClase)
    });

    return await response.json();
  }
  
  async deleteProduct(id: string): Promise<boolean> {
    const response = await fetch(`${this.baseUrl}/${id}`, {
      method: "DELETE",
      headers: this.getAuthHeaders()
    });
  
    return response.ok; 
  }
  
}
