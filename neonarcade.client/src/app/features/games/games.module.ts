import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';

// Components
import { GamesListComponent } from './components/games-list.component';
import { GameDetailsComponent } from './components/game-details/game-details.component';

// Shared
import { SharedModule } from '../../shared/shared.module';

const routes: Routes = [
  { path: '', component: GamesListComponent },
  { path: ':id', component: GameDetailsComponent }
];

@NgModule({
  declarations: [
    GamesListComponent,
    GameDetailsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    RouterModule.forChild(routes)
  ]
})
export class GamesModule { }
