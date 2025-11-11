import { Injectable } from '@angular/core';
import { ClaseDTO } from '../models/claseDTO';
import { AlumnoDTO } from '../models/alumnoDTO';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ObjetoService {
  readonly baseUrl = 'https://localhost:7777/DanceFlowApi/Clase';

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
  const response = await fetch(`${this.baseUrl}/alumno/${id}`, {
    method: 'GET',
    headers: this.getAuthHeaders()
  });

  if (!response.ok) {
    this.toastr.error('Error al obtener el alumno');
  }

  return (await response.json()) as AlumnoDTO;
}


  async getClaseByUsuario(id: string): Promise<ClaseDTO[]> {
  try {
    const response = await fetch(`${this.baseUrl}/alumno/${id}`, {
      method: 'GET',
      headers: this.getAuthHeaders()
    });

    if (!response.ok) {
      this.toastr.error('Error al obtener las clases:');
      return [];
    }

    const data: ClaseDTO[] = await response.json();
    return data ?? [];
  } catch (error) {
    this.toastr.error('Error en la petición:');
    return [];
  }
}


  async updateClase(id: string, partialProduct: Partial<ClaseDTO>): Promise<ClaseDTO> {
    const response = await fetch(`${this.baseUrl}/${id}`, {
      method: "PATCH",
      headers: this.getAuthHeaders(),
      body: JSON.stringify(partialProduct)
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
