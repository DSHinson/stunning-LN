import { Routes } from '@angular/router';
import { Main } from './pages/main/main';
import {Products } from './pages/products/products'
import { Categories} from './pages/categories/categories'

export const routes: Routes = [
  { path: '', component: Main },
  {path: 'products', component: Products},
  {path: 'categories', component: Categories}
];
