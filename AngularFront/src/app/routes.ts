import { Routes } from '@angular/router';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { PrincipalComponent } from './pages/principal/principal.component';
import { ClasePageComponent } from './pages/clase-page/clase-page.component';
import { GuardAuth } from './service/GuardAuth';
import { ClasesInscritasComponent } from './pages/clasesInscritas/clasesInscritas.component';
import { NoticiasComponent } from './pages/noticias/noticias.component';

const routeConfig: Routes = [
  {
    path: '',
    component: LoginComponent,
    title: 'Harteraphia/Login',
  },
  {
    path: 'registro',
    component: RegisterComponent,
    title: 'Harteraphia/Register',
  },
  {
    path: 'noticias',
    component: NoticiasComponent,   
    title: 'Harteraphia/Noticias '
  },
  {
    path: 'principal',
    component: PrincipalComponent,
    title: 'Harteraphia/Principal',
    canActivate : [GuardAuth]
  },
  {
    path: 'ClasePage/:id',
    component: ClasePageComponent,
    title: 'Harteraphia/Clase'
  },
  {
    path: 'ClasesUsuario/:id',
    component: ClasesInscritasComponent,
    title: 'Harteraphia/UsuarioClases'
  },
  {
      path: 'login',
      redirectTo: ''
  },
  {
    path: '**',
    component: PageNotFoundComponent,
  },
];

export default routeConfig;
