import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthGuard } from '../../core/guards/auth.guard';

// Components
import { OrdersListComponent } from './components/orders-list/orders-list.component';
import { OrderDetailsComponent } from './components/order-details/order-details.component';
import { CheckoutComponent } from './components/checkout/checkout.component';

// Shared
import { SharedModule } from '../../shared/shared.module';

const routes: Routes = [
  { path: '', component: OrdersListComponent, canActivate: [AuthGuard] },
  { path: 'checkout', component: CheckoutComponent, canActivate: [AuthGuard] },
  { path: ':id', component: OrderDetailsComponent, canActivate: [AuthGuard] }
];

@NgModule({
  declarations: [
    OrdersListComponent,
    OrderDetailsComponent,
    CheckoutComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    RouterModule.forChild(routes)
  ]
})
export class OrdersModule { }
