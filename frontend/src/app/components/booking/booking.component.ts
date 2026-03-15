import { Component, inject, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StateService, BookingStep } from '../../services/state.service';
import { Barber } from '../../models/barber.model';
import { Appointment } from '../../models/appointment.model';
import { BarberListComponent } from '../barber-list/barber-list.component';
import { TimeSlotSelector } from '../time-slot-selector/time-slot-selector.component';
import { AppointmentFormComponent } from '../appointment-form/appointment-form.component';
import { ConfirmationComponent } from '../confirmation/confirmation.component';

@Component({
  selector: 'app-booking',
  standalone: true,
  imports: [
    CommonModule,
    BarberListComponent,
    TimeSlotSelector,
    AppointmentFormComponent,
    ConfirmationComponent
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    'class': 'min-h-screen bg-background'
  },
  template: `
    <div class="flex flex-col min-h-screen">
      <header class="bg-primary text-primary-foreground p-8 text-center shadow-md">
        <h1 class="text-3xl font-bold mb-2">BarberBook</h1>
        <p class="text-lg opacity-90">Reserva tu turno con tu barbero favorito</p>
      </header>

      <main class="flex-1 max-w-2xl mx-auto w-full p-6">
        @if (state.currentStep() === 'barber-selection') {
          <app-barber-list 
            (barberSelected)="onBarberSelected($event)" 
          />
        }

        @if (state.currentStep() === 'datetime-selection') {
          <div class="mb-6">
            <button 
              class="text-primary font-medium hover:underline flex items-center gap-1"
              (click)="goBack()"
              aria-label="Volver a selección de barbero"
            >
              ← Volver
            </button>
          </div>
          
          <div class="flex items-center gap-2 bg-accent/50 p-3 rounded-md mb-6">
            <span class="text-muted-foreground font-medium">Barbero seleccionado:</span>
            <span class="font-semibold text-primary">{{ state.selectedBarber()?.name }}</span>
          </div>

          <app-time-slot-selector
            [barberId]="state.selectedBarber()!.id"
            [date]="state.selectedDate()"
            (timeSelected)="onTimeSelected($event)"
          />

          @if (state.isTimeSelected()) {
            <div class="mt-8 p-6 bg-accent/30 rounded-xl text-center">
              <p class="mb-4 text-foreground">
                <strong>Fecha:</strong> {{ formatDate(state.selectedDate()!) }}
                <strong class="ml-2">Hora:</strong> {{ state.selectedTime() }}
              </p>
              <button 
                class="px-6 py-2 bg-green-600 text-white rounded-md hover:bg-green-700 transition-colors font-medium"
                (click)="proceedToClientInfo()"
              >
                Continuar →
              </button>
            </div>
          }
        }

        @if (state.currentStep() === 'client-info') {
          <div class="mb-6">
            <button 
              class="text-primary font-medium hover:underline flex items-center gap-1"
              (click)="goBack()"
              aria-label="Volver a selección de fecha"
            >
              ← Volver
            </button>
          </div>

          <div class="bg-muted/50 p-4 rounded-md mb-6">
            <div class="flex justify-between py-2">
              <span class="font-medium text-muted-foreground">Barbero:</span>
              <span class="font-semibold">{{ state.selectedBarber()?.name }}</span>
            </div>
            <div class="flex justify-between py-2">
              <span class="font-medium text-muted-foreground">Fecha y Hora:</span>
              <span class="font-semibold">
                {{ formatDate(state.selectedDate()!) }} a las {{ state.selectedTime() }}
              </span>
            </div>
          </div>

          <app-appointment-form
            [barberId]="state.selectedBarber()!.id"
            [date]="state.selectedDate()!"
            [time]="state.selectedTime()!"
            (appointmentCreated)="onAppointmentCreated($event)"
          />
        }

        @if (state.currentStep() === 'confirmation') {
          <app-confirmation
            [appointment]="state.createdAppointment()!"
            (newBooking)="startNewBooking()"
          />
        }
      </main>
    </div>
  `,
  styles: []
})
export class BookingComponent {
  readonly state = inject(StateService);

  onBarberSelected(barber: Barber): void {
    this.state.setBarber(barber);
    // Set default date to today
    const today = new Date().toISOString().split('T')[0];
    this.state.setDate(today);
    this.state.nextStep();
  }

  onTimeSelected(time: string): void {
    this.state.setTime(time);
  }

  proceedToClientInfo(): void {
    this.state.nextStep();
  }

  onAppointmentCreated(appointment: Appointment): void {
    this.state.setCreatedAppointment(appointment);
    this.state.nextStep();
  }

  startNewBooking(): void {
    this.state.reset();
  }

  goBack(): void {
    this.state.previousStep();
  }

  formatDate(dateStr: string): string {
    const date = new Date(dateStr);
    return date.toLocaleDateString('es-AR', {
      weekday: 'short',
      day: 'numeric',
      month: 'short'
    });
  }
}
