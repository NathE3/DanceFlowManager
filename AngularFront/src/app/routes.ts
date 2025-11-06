import { Routes } from '@angular/router';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { PrincipalComponent } from './pages/principal/prinicpal.component';
import { ClasePageComponent } from './pages/propuesta-page/propuesta-page.component';
import { AnadirComponent } from './pages/anadir/anadir.component';

const routeConfig: Routes = [
  {
    path: '',
    component: LoginComponent,
    title: 'Harteraphia Login',
  },
  {
    path: 'registro',
    component: RegisterComponent,
    title: 'Harteraphia Register',
  },
  {
    path: 'principal',
    component: PrincipalComponent,
    title: 'Harteraphia Principal',
  },
  {
    path: 'ClasePage/:id',
    component: ClasePageComponent,
    title: 'Harteraphia Clase'
  },
  {
    path: 'inscribirseClase',
    component: AnadirComponent,
    title: 'Harteraphia inscripciónClase'
  },
  {
    path: '**',
    component: PageNotFoundComponent,
  },
];

export default routeConfig;
