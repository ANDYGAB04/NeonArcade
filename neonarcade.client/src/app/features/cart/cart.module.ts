import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthGuard } from '../../core/guards/auth.guard';

// Components
import { CartComponent } from './components/cart/cart.component';

// Shared
import { SharedModule } from '../../shared/shared.module';

const routes: Routes = [
  { path: '', component: CartComponent, canActivate: [AuthGuard] }
];

@NgModule({
  declarations: [
    CartComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    RouterModule.forChild(routes)
  ]
})
export class CartModule { }
