import type { AlumnoDTO } from './alumnoDTO.ts';

export interface ClaseDTO{
    id_clase: string;
    nombre: string
    descripcion: string
    tipo:string; 
    fechaClase: Date
    idProfesor: string  
    alumnosInscritos: AlumnoDTO[];
}