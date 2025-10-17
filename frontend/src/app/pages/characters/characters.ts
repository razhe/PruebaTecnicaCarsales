import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CharactersService, Character, PaginationInfo, CharacterFilter } from '../../services/characters.service';

@Component({
  selector: 'app-characters',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './characters.html',
  styleUrl: './characters.css'
})
export class Characters implements OnInit {
  characters = signal<Character[]>([]);
  pagination = signal<PaginationInfo | null>(null);
  isLoading = signal<boolean>(false);
  errorMessage = signal<string | null>(null);

  // Filtros
  searchName = signal<string>('');
  searchStatus = signal<string>('');
  searchSpecies = signal<string>('');
  searchType = signal<string>('');
  searchGender = signal<string>('');
  currentPage = signal<number>(1);

  statusOptions = ['Alive', 'Dead', 'unknown'];
  genderOptions = ['Female', 'Male', 'Genderless', 'unknown'];

  constructor(private charactersService: CharactersService) {}

  ngOnInit(): void {
    this.loadCharacters();
  }

  /*
   * Carga los personajes con los filtros actuales
   */
  loadCharacters(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    const filter: CharacterFilter = {
      name: this.searchName() || undefined,
      status: this.searchStatus() || undefined,
      species: this.searchSpecies() || undefined,
      type: this.searchType() || undefined,
      gender: this.searchGender() || undefined,
      page: this.currentPage()
    };

    this.charactersService.getCharacters(filter).subscribe({
      next: (response) => {
        this.characters.set(response.results);
        this.pagination.set(response.info);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error al cargar personajes:', error);
        this.errorMessage.set('Error al cargar los personajes. Intenta nuevamente.');
        this.isLoading.set(false);
      }
    });
  }

  /*
   * Busca personajes con los filtros aplicados
   */
  search(): void {
    this.currentPage.set(1);
    this.loadCharacters();
  }

  /*
   * Limpia los filtros y recarga
   */
  clearFilters(): void {
    this.searchName.set('');
    this.searchStatus.set('');
    this.searchSpecies.set('');
    this.searchType.set('');
    this.searchGender.set('');
    this.currentPage.set(1);
    this.loadCharacters();
  }

  /*
   * Ir a la página anterior
   */
  previousPage(): void {
    if (this.pagination()?.prev) {
      this.currentPage.update(page => page - 1);
      this.loadCharacters();
    }
  }

  /*
   * Ir a la página siguiente
   */
  nextPage(): void {
    if (this.pagination()?.next) {
      this.currentPage.update(page => page + 1);
      this.loadCharacters();
    }
  }

  /*
   * Ir a una página específica
   */
  goToPage(page: number): void {
    if (page > 0 && page <= (this.pagination()?.pages || 1)) {
      this.currentPage.set(page);
      this.loadCharacters();
    }
  }

  /*
   * Verifica si hay personajes cargados
   */
  hasCharacters(): boolean {
    return this.characters().length > 0;
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