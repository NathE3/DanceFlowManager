import { Injectable } from '@angular/core';
import { ClaseDTO } from '../models/claseDTO';
import { AlumnoDTO } from '../models/alumnoDTO';

@Injectable({
  providedIn: 'root'
})
export class ObjetoService {
  readonly baseUrl = 'https://localhost:7777/DanceFlowApi/Clase';

  constructor() {}

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

  async getClaseById(id: string): Promise<ClaseDTO | undefined> {
    const response = await fetch(`${this.baseUrl}/${id}`, {
      method: 'GET',
      headers: this.getAuthHeaders()
    });
    return (await response.json()) as ClaseDTO | undefined;
  }

  async getAlumnoById(id: string): Promise<AlumnoDTO | undefined> {
  const response = await fetch(`${this.baseUrl}/alumnos/${id}`, {
    method: 'GET',
    headers: this.getAuthHeaders()
  });

  if (!response.ok) {
    console.error('Error al obtener el alumno:', response.statusText);
    return undefined;
  }

  return (await response.json()) as AlumnoDTO;
}

  async getClaseByUsuario(): Promise<ClaseDTO[]> {
    const response = await fetch(`${this.baseUrl}/user`, {
      method: 'GET',
      headers: this.getAuthHeaders()
    });
    return (await response.json()) ?? [];
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
