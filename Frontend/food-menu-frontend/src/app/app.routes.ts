import { Routes } from '@angular/router';

import { Home } from './components/home/home';
import { Login } from './components/login/login';
import { Register } from './components/register/register';
import { CategoryList } from './components/category-list/category-list';
import { CategoryForm } from './components/category-form/category-form';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'login', component: Login },
  { path: 'register', component: Register },

  { path: 'categories', component: CategoryList },
  { path: 'categories/add', component: CategoryForm },
  { path: 'categories/edit/:id', component: CategoryForm }
];