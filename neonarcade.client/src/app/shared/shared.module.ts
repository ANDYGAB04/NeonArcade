import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// Shared components will be added here as they are created
// import { HeaderComponent } from './components/header/header.component';
// import { FooterComponent } from './components/footer/footer.component';
// import { LoadingSpinnerComponent } from './components/loading-spinner/loading-spinner.component';

// Shared pipes will be added here
// import { CurrencyFormatPipe } from './pipes/currency-format.pipe';

@NgModule({
  declarations: [
    // Shared components
    // HeaderComponent,
    // FooterComponent,
    // LoadingSpinnerComponent,
    
    // Shared pipes
    // CurrencyFormatPipe
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    // Export modules
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    
    // Export shared components
    // HeaderComponent,
    // FooterComponent,
    // LoadingSpinnerComponent,
    
    // Export shared pipes
    // CurrencyFormatPipe
  ]
})
export class SharedModule { }
