import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, tap, throwError } from 'rxjs';
import { Appointment } from '../models/appointment.model';
import { CreateAppointmentRequest } from '../models/appointment-request.model';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5000/api/appointments';

  // Signals for state management
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);
  readonly lastCreatedAppointment = signal<Appointment | null>(null);

  /**
   * Creates a new appointment
   * @param request The appointment creation request
   */
  createAppointment(request: CreateAppointmentRequest): Observable<Appointment> {
    this.loading.set(true);
    this.error.set(null);

    return this.http.post<Appointment>(this.apiUrl, request).pipe(
      tap(appointment => {
        this.lastCreatedAppointment.set(appointment);
        this.loading.set(false);
      }),
      catchError((err: HttpErrorResponse) => {
        this.error.set(this.parseError(err));
        this.loading.set(false);
        return throwError(() => err);
      })
    );
  }

  /**
   * Parses error response from the API
   */
  private parseError(err: HttpErrorResponse): string {
    if (err.status === 0) {
      return 'No se pudo conectar con el servidor. Verifica que el backend esté funcionando.';
    }

    // Handle validation errors (400)
    if (err.status === 400 && err.error?.errors) {
      const errors = err.error.errors;
      const firstError = Object.values(errors).flat()[0];
      return String(firstError);
    }

    // Handle conflict errors (409)
    if (err.status === 409) {
      return err.error?.detail || 'El horario ya no está disponible. Por favor selecciona otro.';
    }

    if (err.status === 500) {
      return 'Error del servidor. Intenta más tarde.';
    }

    return err.error?.message || 'Ocurrió un error al crear la cita.';
  }

  /**
   * Clears the last created appointment
   */
  clearLastCreated(): void {
    this.lastCreatedAppointment.set(null);
  }
}
