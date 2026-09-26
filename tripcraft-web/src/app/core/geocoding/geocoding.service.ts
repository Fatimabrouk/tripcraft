import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

export interface GeocodeResult {
  displayName: string;
  latitude: number;
  longitude: number;
}

@Injectable({ providedIn: 'root' })
export class GeocodingService {
  private readonly baseUrl = 'https://nominatim.openstreetmap.org/search';

  constructor(private http: HttpClient) {}

  search(query: string): Observable<GeocodeResult[]> {
    const params = new URLSearchParams({ q: query, format: 'json', limit: '5' });
    return this.http.get<any[]>(`${this.baseUrl}?${params}`).pipe(
      map(results => results.map(r => ({
        displayName: r.display_name,
        latitude: parseFloat(r.lat),
        longitude: parseFloat(r.lon)
      })))
    );
  }
}