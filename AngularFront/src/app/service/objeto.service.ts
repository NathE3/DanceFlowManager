import { Injectable } from '@angular/core';
import { ClaseDTO } from '../models/claseDTO';

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

  async getProductById(id: string): Promise<ClaseDTO | undefined> {
    const response = await fetch(`${this.baseUrl}/${id}`, {
      method: 'GET',
      headers: this.getAuthHeaders()
    });
    return (await response.json()) as ClaseDTO | undefined;
  }

  async getProductByUsuario(): Promise<ClaseDTO[]> {
    const response = await fetch(`${this.baseUrl}/user`, {
      method: 'GET',
      headers: this.getAuthHeaders()
    });
    return (await response.json()) ?? [];
  }

  async updateProduct(id: string, partialProduct: Partial<ClaseDTO>): Promise<ClaseDTO> {
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
