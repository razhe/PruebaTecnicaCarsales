import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

export interface PaginationInfo {
  count: number;
  pages: number;
  next: string | null;
  prev: string | null;
}

export interface Episode {
  id: number;
  name: string;
  airDate: string;
  episode: string;
  characters: string[];
  url: string;
  created: string;
}

export interface PaginatedEpisodesResponse {
  info: PaginationInfo;
  results: Episode[];
}

export interface EpisodeFilter {
  name?: string;
  episode?: string;
  page?: number;
}

@Injectable({
  providedIn: 'root'  
})
export class EpisodesService {
  private readonly apiBaseUrl = environment.apiBaseUrl;

  constructor(private httpClient: HttpClient) {}

  /*
   * Obtiene todos los episodios con paginación y filtros opcionales
   */
  getEpisodes(filter?: EpisodeFilter): Observable<PaginatedEpisodesResponse> {
    let params = new HttpParams();

    if (filter) {
      if (filter.name) {
        params = params.set('name', filter.name);
      }
      if (filter.episode) {
        params = params.set('episode', filter.episode);
      }
      if (filter.page) {
        params = params.set('page', filter.page.toString());
      }
    }

    return this.httpClient.get<PaginatedEpisodesResponse>(
      `${this.apiBaseUrl}/episodes`,
      { params }
    );
  }

  /*
   * Obtiene un episodio específico por ID
   */
  getEpisodeById(id: number): Observable<Episode> {
    return this.httpClient.get<Episode>(
      `${this.apiBaseUrl}/episodes/${id}`
    );
  }

  /*
   * Obtiene múltiples episodios por IDs
   */
  getMultipleEpisodes(ids: number[]): Observable<Episode[]> {
    const idsParam = ids.join(',');
    return this.httpClient.get<Episode[]>(
      `${this.apiBaseUrl}/episodes/multiple`,
      { params: new HttpParams().set('ids', idsParam) }
    );
  }
}