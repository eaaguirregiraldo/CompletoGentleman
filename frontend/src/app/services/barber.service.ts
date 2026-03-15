import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, tap, throwError, of } from 'rxjs';
import { Barber } from '../models/barber.model';
import { AvailabilityResponse } from '../models/availability.model';

@Injectable({
  providedIn: 'root'
})
export class BarberService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5000/api/barbers';

  // Signals for state management
  readonly barbers = signal<Barber[]>([]);
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  /**
   * Fetches all active barbers from the API
   */
  getBarbers(): Observable<Barber[]> {
    this.loading.set(true);
    this.error.set(null);

    return this.http.get<Barber[]>(this.apiUrl).pipe(
      tap(barbers => {
        this.barbers.set(barbers);
        this.loading.set(false);
      }),
      catchError((err: HttpErrorResponse) => {
        this.error.set(this.getErrorMessage(err));
        this.loading.set(false);
        return throwError(() => err);
      })
    );
  }

  /**
   * Fetches availability for a specific barber on a given date
   * @param barberId The barber ID
   * @param date The date to check (YYYY-MM-DD format)
   */
  getAvailability(barberId: number, date: string): Observable<AvailabilityResponse> {
    this.loading.set(true);
    this.error.set(null);

    const url = `${this.apiUrl}/${barberId}/availability?date=${date}`;

    return this.http.get<AvailabilityResponse>(url).pipe(
      tap(response => {
        this.loading.set(false);
      }),
      catchError((err: HttpErrorResponse) => {
        this.error.set(this.getErrorMessage(err));
        this.loading.set(false);
        return throwError(() => err);
      })
    );
  }

  /**
   * Helper to parse error messages from HTTP responses
   */
  private getErrorMessage(err: HttpErrorResponse): string {
    if (err.status === 0) {
      return 'No se pudo conectar con el servidor. Verifica que el backend esté funcionando.';
    }
    if (err.status === 404) {
      return 'Recurso no encontrado.';
    }
    if (err.status === 500) {
      return 'Error del servidor. Intenta más tarde.';
    }
    return err.error?.message || 'Ocurrió un error inesperado.';
  }
}
