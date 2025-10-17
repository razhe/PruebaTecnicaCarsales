import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface PaginationInfo {
  count: number;
  pages: number;
  next: string | null;
  prev: string | null;
}

export interface LocationReference {
  name: string;
  url: string;
}

export interface Character {
  id: number;
  name: string;
  status: string;
  species: string;
  type: string;
  gender: string;
  origin: LocationReference;
  location: LocationReference;
  image: string;
  episode: string[];
  url: string;
  created: string;
}

export interface PaginatedCharactersResponse {
  info: PaginationInfo;
  results: Character[];
}

export interface CharacterFilter {
  name?: string;
  status?: string;
  species?: string;
  type?: string;
  gender?: string;
  page?: number;
}

@Injectable({
  providedIn: 'root'
})
export class CharactersService {
  private readonly apiBaseUrl = 'http://localhost:5001/api/v1';

  constructor(private httpClient: HttpClient) {}

  /*
   * Obtiene todos los personajes con paginación y filtros opcionales
   */
  getCharacters(filter?: CharacterFilter): Observable<PaginatedCharactersResponse> {
    let params = new HttpParams();

    if (filter) {
      if (filter.name) {
        params = params.set('name', filter.name);
      }
      if (filter.status) {
        params = params.set('status', filter.status);
      }
      if (filter.species) {
        params = params.set('species', filter.species);
      }
      if (filter.type) {
        params = params.set('type', filter.type);
      }
      if (filter.gender) {
        params = params.set('gender', filter.gender);
      }
      if (filter.page) {
        params = params.set('page', filter.page.toString());
      }
    }

    return this.httpClient.get<PaginatedCharactersResponse>(
      `${this.apiBaseUrl}/characters`,
      { params }
    );
  }

  /*
   * Obtiene un personaje específico por ID
   */
  getCharacterById(id: number): Observable<Character> {
    return this.httpClient.get<Character>(
      `${this.apiBaseUrl}/characters/${id}`
    );
  }

  /*
   * Obtiene múltiples personajes por IDs
   */
  getMultipleCharacters(ids: number[]): Observable<Character[]> {
    const idsParam = ids.join(',');
    return this.httpClient.get<Character[]>(
      `${this.apiBaseUrl}/characters/multiple`,
      { params: new HttpParams().set('ids', idsParam) }
    );
  }
}