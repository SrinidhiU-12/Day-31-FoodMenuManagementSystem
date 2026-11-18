import { Routes } from '@angular/router';

import { Home } from './components/home/home';
import { Login } from './components/login/login';
import { Register } from './components/register/register';
import { CategoryList } from './components/category-list/category-list';
import { CategoryForm } from './components/category-form/category-form';
import { FoodItemList } from './components/food-item-list/food-item-list';
import { FoodItemForm } from './components/food-item-form/food-item-form';
import { OrderList } from './components/order-list/order-list';
import { OrderItemList } from './components/order-item-list/order-item-list';
import { PaymentComponent } from './components/payment/payment';


export const routes: Routes = [
  { path: '', component: Home },
  { path: 'login', component: Login },
  { path: 'register', component: Register },

  { path: 'categories', component: CategoryList },
  { path: 'categories/add', component: CategoryForm },
  { path: 'categories/edit/:id', component: CategoryForm },

{ path: 'food-items', component: FoodItemList },
{ path: 'food-items/add', component: FoodItemForm },
{ path: 'food-items/edit/:id', component: FoodItemForm },
{ path: 'orders', component: OrderList},

{
  path: 'orders/:orderId/items',
  component: OrderItemList
}, 

{
  path: 'orders/:orderId/payment',
  component: PaymentComponent
},
];