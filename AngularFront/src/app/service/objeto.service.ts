import { Injectable } from '@angular/core';
import { propuestaModel } from '../models/propuestaModel';
import { CreatePropuestaModel } from '../models/CreatePropuestaModel';

@Injectable({
  providedIn: 'root'
})
export class ObjetoService {
  readonly baseUrl = 'https://localhost:7777/api/Proyecto';

  constructor() {}

  private getAuthHeaders(): { [key: string]: string } {
    const token = localStorage.getItem('token');
    return {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    };
  }

  async getAllProduct(): Promise<propuestaModel[]> {
    const response = await fetch(this.baseUrl, {
      method: 'GET',
      headers: this.getAuthHeaders()
    });
    return (await response.json()) ?? [];
  }

  async getProductById(id: number): Promise<propuestaModel | undefined> {
    const response = await fetch(`${this.baseUrl}/${id}`, {
      method: 'GET',
      headers: this.getAuthHeaders()
    });
    return (await response.json()) as propuestaModel | undefined;
  }

  async getProductByUsuario(): Promise<propuestaModel[]> {
    const response = await fetch(`${this.baseUrl}/user`, {
      method: 'GET',
      headers: this.getAuthHeaders()
    });
    return (await response.json()) ?? [];
  }

  async updateProduct(id: number, partialProduct: Partial<propuestaModel>): Promise<propuestaModel> {
    const response = await fetch(`${this.baseUrl}/${id}`, {
      method: "PATCH",
      headers: this.getAuthHeaders(),
      body: JSON.stringify(partialProduct)
    });

    return await response.json();
  }

  async createProduct(product: CreatePropuestaModel): Promise<CreatePropuestaModel> {
    const response = await fetch(this.baseUrl, {
      method: "POST",
      headers: this.getAuthHeaders(),
      body: JSON.stringify(product)
    });

    return await response.json();
  }
  
  async deleteProduct(id: number): Promise<boolean> {
    const response = await fetch(`${this.baseUrl}/${id}`, {
      method: "DELETE",
      headers: this.getAuthHeaders()
    });
  
    return response.ok; 
  }
  
}
