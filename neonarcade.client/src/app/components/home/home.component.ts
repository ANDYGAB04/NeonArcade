import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Game } from '../../models/game.model';
import { GameService } from '../../features/games/services/game.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  standalone: false
})
export class HomeComponent implements OnInit {
  featuredGames: Game[] = [];
  newReleases: Game[] = [];
  deals: Game[] = [];
  loading = false;

  constructor(
    private gameService: GameService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadFeaturedGames();
    this.loadNewReleases();
    this.loadDeals();
  }

  loadFeaturedGames(): void {
    this.gameService.getFeaturedGames().subscribe({
      next: (games) => {
        this.featuredGames = this.mapGames(games).slice(0, 4);
      },
      error: (error) => {
        console.error('Error loading featured games:', error);
      }
    });
  }

  loadNewReleases(): void {
    this.gameService.getAllGames({ pageSize: 4, sortBy: 'releaseDate' }).subscribe({
      next: (response) => {
        this.newReleases = this.mapGames(response.items);
      },
      error: (error) => {
        console.error('Error loading new releases:', error);
      }
    });
  }

  loadDeals(): void {
    this.gameService.getDeals().subscribe({
      next: (games) => {
        this.deals = this.mapGames(games).slice(0, 4);
      },
      error: (error) => {
        console.error('Error loading deals:', error);
      }
    });
  }

  /**
   * Map backend Game model to frontend with computed properties
   */
  private mapGames(games: Game[]): Game[] {
    return games.map(game => ({
      ...game,
      imageUrl: game.coverImageUrl,
      category: game.genres?.[0] || 'Game',
      discountPercentage: game.discountPrice 
        ? Math.round(((game.price - game.discountPrice) / game.price) * 100)
        : undefined,
      finalPrice: game.discountPrice || game.price
    }));
  }

  navigateToGames(): void {
    this.router.navigate(['/games']);
  }

  navigateToDeals(): void {
    this.router.navigate(['/games'], { queryParams: { deals: true } });
  }

  navigateToGameDetails(gameId: number): void {
    this.router.navigate(['/games', gameId]);
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }
}
