import { Routes } from '@angular/router';
import { Episodes } from './pages/episodes/episodes';
import { Characters } from './pages/characters/characters';
import { Locations } from './pages/locations/locations';

export const routes: Routes = [
    { path: 'episodes', component: Episodes },
    { path: 'characters', component: Characters },
    { path: 'locations', component: Locations }
];
