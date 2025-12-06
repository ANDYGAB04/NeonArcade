import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';

// Components will be added here as they are created
// import { GameListComponent } from './components/game-list/game-list.component';
// import { GameDetailComponent } from './components/game-detail/game-detail.component';

const routes: Routes = [
  // { path: '', component: GameListComponent },
  // { path: ':id', component: GameDetailComponent }
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
export class GamesModule { }
