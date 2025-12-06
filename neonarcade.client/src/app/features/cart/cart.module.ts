import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../../core/guards/auth.guard';

// Components will be added here as they are created
// import { CartPageComponent } from './components/cart-page/cart-page.component';

const routes: Routes = [
  // { path: '', component: CartPageComponent, canActivate: [AuthGuard] }
];

@NgModule({
  declarations: [
    // Components will be declared here
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [
    // Exported components will be listed here
  ]
})
export class CartModule { }
