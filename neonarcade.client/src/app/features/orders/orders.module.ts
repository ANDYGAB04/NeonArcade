import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../../core/guards/auth.guard';

// Components will be added here as they are created
// import { OrderHistoryComponent } from './components/order-history/order-history.component';
// import { OrderDetailComponent } from './components/order-detail/order-detail.component';
// import { CheckoutComponent } from './components/checkout/checkout.component';

const routes: Routes = [
  // { path: 'history', component: OrderHistoryComponent, canActivate: [AuthGuard] },
  // { path: ':id', component: OrderDetailComponent, canActivate: [AuthGuard] },
  // { path: 'checkout', component: CheckoutComponent, canActivate: [AuthGuard] }
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
export class OrdersModule { }
