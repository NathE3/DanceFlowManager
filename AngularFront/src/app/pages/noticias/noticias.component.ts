import { Component, OnInit } from '@angular/core';
import { SafeUrlPipe } from '../../service/safe-url.pipe';
import { CommonModule } from '@angular/common';

interface Noticia {
  titulo: string;
  fecha: string;
  contenido: string;
  videoUrl?: string; // URL de YouTube
}

@Component({
  selector: 'app-noticias',
   standalone: true,
  imports: [CommonModule, SafeUrlPipe],
  templateUrl: './noticias.component.html',
  styleUrls: ['./noticias.component.css']
})
export class NoticiasComponent implements OnInit {

  noticias: Noticia[] = [];

  constructor() { }

  ngOnInit(): void {

    this.noticias = [
      {
        titulo: 'Harteraphia celebra su 10º aniversario',
        fecha: '3 de Noviembre, 2025',
        contenido: `La escuela Harteraphia celebra una década de pasión por el baile. Con más de 500 alumnos activos y múltiples campeonatos ganados, esta academia se consolida como referente en danza contemporánea y urbana en España. ¡Ven y descubre tu ritmo con nosotros!`,
        videoUrl: 'https://www.youtube.com/embed/dQw4w9WgXcQ'
      },
      {
        titulo: 'Masterclass exclusiva con Davo, ex alumno y profesor Harteraphia',
        fecha: '15 de Octubre, 2025',
        contenido: `Davo, bailarín internacional formado en Harteraphia, regresa a su escuela para impartir una masterclass única de street dance y freestyle. Una oportunidad para aprender de los mejores y conectar con la energía que caracteriza a Harteraphia.`,
        videoUrl: 'https://www.youtube.com/embed/5qap5aO4i9A'
      },
      {
        titulo: 'Nuevos cursos de danza contemporánea y urbana',
        fecha: '1 de Octubre, 2025',
        contenido: `Estamos emocionados de anunciar los nuevos cursos de danza contemporánea y urbana para todos los niveles. Con instructores certificados y métodos innovadores, Harteraphia te brinda la oportunidad de explorar tu creatividad y mejorar tu técnica.`,
        videoUrl: 'https://www.youtube.com/embed/tgbNymZ7vqY'
      }
    ];
  }

}
