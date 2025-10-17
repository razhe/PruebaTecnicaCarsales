import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EpisodesService, Episode, PaginationInfo, EpisodeFilter } from '../../services/episodes.service';


@Component({
  selector: 'app-episodes',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './episodes.html',
  styleUrl: './episodes.css'
})
export class Episodes implements OnInit {
  episodes = signal<Episode[]>([]);
  pagination = signal<PaginationInfo | null>(null);
  isLoading = signal<boolean>(false);
  errorMessage = signal<string | null>(null);

  // Filtros
  searchName = signal<string>('');
  searchEpisode = signal<string>('');
  currentPage = signal<number>(1);

  constructor(private episodesService: EpisodesService) {}

  ngOnInit(): void {
    this.loadEpisodes();
  }

  /*
   * Carga los episodios con los filtros actuales
   */
  loadEpisodes(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    const filter: EpisodeFilter = {
      name: this.searchName() || undefined,
      episode: this.searchEpisode() || undefined,
      page: this.currentPage()
    };

    this.episodesService.getEpisodes(filter).subscribe({
      next: (response) => {
        this.episodes.set(response.results);
        this.pagination.set(response.info);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error al cargar episodios:', error);
        this.errorMessage.set('Error al cargar los episodios. Intenta nuevamente.');
        this.isLoading.set(false);
      }
    });
  }

  /*
   * Busca episodios con los filtros aplicados
   */
  search(): void {
    this.currentPage.set(1);
    this.loadEpisodes();
  }

  /*
   * Limpia los filtros y recarga
   */
  clearFilters(): void {
    this.searchName.set('');
    this.searchEpisode.set('');
    this.currentPage.set(1);
    this.loadEpisodes();
  }

  /*
   * Ir a la página anterior
   */
  previousPage(): void {
    if (this.pagination()?.prev) {
      this.currentPage.update(page => page - 1);
      this.loadEpisodes();
    }
  }

  /*
   * Ir a la página siguiente
   */
  nextPage(): void {
    if (this.pagination()?.next) {
      this.currentPage.update(page => page + 1);
      this.loadEpisodes();
    }
  }

  /*
   * Ir a una página específica
   */
  goToPage(page: number): void {
    if (page > 0 && page <= (this.pagination()?.pages || 1)) {
      this.currentPage.set(page);
      this.loadEpisodes();
    }
  }

  /*
   * Verifica si hay episodios cargados
   */
  hasEpisodes(): boolean {
    return this.episodes().length > 0;
  }

  /*
   * Obtiene el número de página actual
   */
  getCurrentPage(): number {
    return this.currentPage();
  }

  /*
   * Obtiene el total de páginas
   */
  getTotalPages(): number {
    return this.pagination()?.pages || 0;
  }

  /*
   * Verifica si puede ir a la página anterior
   */
  canGoToPrevious(): boolean {
    return this.currentPage() > 1;
  }

  /*
   * Verifica si puede ir a la página siguiente
   */
  canGoToNext(): boolean {
    const totalPages = this.pagination()?.pages || 0;
    return this.currentPage() < totalPages;
  }
}
