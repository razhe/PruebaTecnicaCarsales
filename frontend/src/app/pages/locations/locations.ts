import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LocationsService, Location, PaginationInfo, LocationFilter } from '../../services/locations.service';

@Component({
  selector: 'app-locations',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './locations.html',
  styleUrl: './locations.css'
})
export class Locations implements OnInit {
  locations = signal<Location[]>([]);
  pagination = signal<PaginationInfo | null>(null);
  isLoading = signal<boolean>(false);
  errorMessage = signal<string | null>(null);

  // Filtros
  searchName = signal<string>('');
  searchType = signal<string>('');
  searchDimension = signal<string>('');
  currentPage = signal<number>(1);

  constructor(private locationsService: LocationsService) {}

  ngOnInit(): void {
    this.loadLocations();
  }

  /**
   * Carga las ubicaciones con los filtros actuales
   */
  loadLocations(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    const filter: LocationFilter = {
      name: this.searchName() || undefined,
      type: this.searchType() || undefined,
      dimension: this.searchDimension() || undefined,
      page: this.currentPage()
    };

    this.locationsService.getLocations(filter).subscribe({
      next: (response) => {
        this.locations.set(response.results);
        this.pagination.set(response.info);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error al cargar ubicaciones:', error);
        this.errorMessage.set('Error al cargar las ubicaciones. Intenta nuevamente.');
        this.isLoading.set(false);
      }
    });
  }

  /*
   * Busca ubicaciones con los filtros aplicados
   */
  search(): void {
    this.currentPage.set(1);
    this.loadLocations();
  }

  /*
   * Limpia los filtros y recarga
   */
  clearFilters(): void {
    this.searchName.set('');
    this.searchType.set('');
    this.searchDimension.set('');
    this.currentPage.set(1);
    this.loadLocations();
  }

  /*
   * Ir a la página anterior
   */
  previousPage(): void {
    if (this.pagination()?.prev) {
      this.currentPage.update(page => page - 1);
      this.loadLocations();
    }
  }

  /*
   * Ir a la página siguiente
   */
  nextPage(): void {
    if (this.pagination()?.next) {
      this.currentPage.update(page => page + 1);
      this.loadLocations();
    }
  }

  /*
   * Ir a una página específica
   */
  goToPage(page: number): void {
    if (page > 0 && page <= (this.pagination()?.pages || 1)) {
      this.currentPage.set(page);
      this.loadLocations();
    }
  }

  /*
   * Verifica si hay ubicaciones cargadas
   */
  hasLocations(): boolean {
    return this.locations().length > 0;
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