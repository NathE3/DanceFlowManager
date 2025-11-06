import { ClaseDTO } from './claseDTO';

export interface AlumnoDTO {
    id: string;
    name: string;
    apellidos: string;
    email: string;
    fechaAlta: string;      
    fechaBaja?: string;     
    telefono: number;
    clasesInscritas: ClaseDTO[];
}
