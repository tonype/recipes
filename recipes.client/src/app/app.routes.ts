import { Routes } from '@angular/router';
import { RecipeListComponent } from './features/recipes/recipe-list/recipe-list.component';
import { RecipeCreateComponent } from './features/recipes/recipe-create/recipe-create.component';

export const routes: Routes = [
  { path: '', redirectTo: '/recipes', pathMatch: 'full' },
  { path: 'recipes', component: RecipeListComponent },
  { path: 'recipes/new', component: RecipeCreateComponent }
];
