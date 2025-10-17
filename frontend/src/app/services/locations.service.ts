import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface PaginationInfo {
  count: number;
  pages: number;
  next: string | null;
  prev: string | null;
}

export interface Location {
  id: number;
  name: string;
  type: string;
  dimension: string;
  residents: string[];
  url: string;
  created: string;
}

export interface PaginatedLocationsResponse {
  info: PaginationInfo;
  results: Location[];
}

export interface LocationFilter {
  name?: string;
  type?: string;
  dimension?: string;
  page?: number;
}

@Injectable({
  providedIn: 'root'
})
export class LocationsService {
  private readonly apiBaseUrl = 'http://localhost:5001/api/v1';

  constructor(private httpClient: HttpClient) {}

  /*
   * Obtiene todas las ubicaciones con paginación y filtros opcionales
   */
  getLocations(filter?: LocationFilter): Observable<PaginatedLocationsResponse> {
    let params = new HttpParams();

    if (filter) {
      if (filter.name) {
        params = params.set('name', filter.name);
      }
      if (filter.type) {
        params = params.set('type', filter.type);
      }
      if (filter.dimension) {
        params = params.set('dimension', filter.dimension);
      }
      if (filter.page) {
        params = params.set('page', filter.page.toString());
      }
    }

    return this.httpClient.get<PaginatedLocationsResponse>(
      `${this.apiBaseUrl}/locations`,
      { params }
    );
  }

  /*
   * Obtiene una ubicación específica por ID
   */
  getLocationById(id: number): Observable<Location> {
    return this.httpClient.get<Location>(
      `${this.apiBaseUrl}/locations/${id}`
    );
  }

  /*
   * Obtiene múltiples ubicaciones por IDs
   */
  getMultipleLocations(ids: number[]): Observable<Location[]> {
    const idsParam = ids.join(',');
    return this.httpClient.get<Location[]>(
      `${this.apiBaseUrl}/locations/multiple`,
      { params: new HttpParams().set('ids', idsParam) }
    );
  }
}