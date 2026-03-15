import { Injectable, signal, computed } from '@angular/core';
import { Barber } from '../models/barber.model';
import { Appointment } from '../models/appointment.model';

/**
 * Booking step enumeration
 */
export type BookingStep = 'barber-selection' | 'datetime-selection' | 'client-info' | 'confirmation';

/**
 * Application state model
 */
export interface AppState {
  selectedBarber: Barber | null;
  selectedDate: string | null;
  selectedTime: string | null;
  currentStep: BookingStep;
}

@Injectable({
  providedIn: 'root'
})
export class StateService {
  // Core state signals
  readonly selectedBarber = signal<Barber | null>(null);
  readonly selectedDate = signal<string | null>(null);
  readonly selectedTime = signal<string | null>(null);
  readonly currentStep = signal<BookingStep>('barber-selection');
  readonly clientData = signal<{
    name: string;
    phone: string;
    email: string;
  } | null>(null);
  readonly createdAppointment = signal<Appointment | null>(null);

  // Computed values
  readonly isBarberSelected = computed(() => this.selectedBarber() !== null);
  readonly isDateSelected = computed(() => this.selectedDate() !== null);
  readonly isTimeSelected = computed(() => this.selectedTime() !== null);
  readonly isClientInfoComplete = computed(() => {
    const data = this.clientData();
    return data !== null && !!data.name && !!data.phone && !!data.email;
  });
  readonly canProceedToDateSelection = computed(() => this.isBarberSelected());
  readonly canProceedToClientInfo = computed(() => 
    this.isBarberSelected() && this.isDateSelected() && this.isTimeSelected()
  );

  /**
   * Sets the selected barber
   */
  setBarber(barber: Barber): void {
    this.selectedBarber.set(barber);
  }

  /**
   * Sets the selected date
   */
  setDate(date: string): void {
    this.selectedDate.set(date);
    // Reset time when date changes
    this.selectedTime.set(null);
  }

  /**
   * Sets the selected time slot
   */
  setTime(time: string): void {
    this.selectedTime.set(time);
  }

  /**
   * Sets client information
   */
  setClientData(data: { name: string; phone: string; email: string }): void {
    this.clientData.set(data);
  }

  /**
   * Sets the created appointment
   */
  setCreatedAppointment(appointment: Appointment): void {
    this.createdAppointment.set(appointment);
  }

  /**
   * Advances to the next step
   */
  nextStep(): void {
    const current = this.currentStep();
    const steps: BookingStep[] = [
      'barber-selection',
      'datetime-selection', 
      'client-info',
      'confirmation'
    ];
    const currentIndex = steps.indexOf(current);
    if (currentIndex < steps.length - 1) {
      this.currentStep.set(steps[currentIndex + 1]);
    }
  }

  /**
   * Goes back to the previous step
   */
  previousStep(): void {
    const current = this.currentStep();
    const steps: BookingStep[] = [
      'barber-selection',
      'datetime-selection',
      'client-info',
      'confirmation'
    ];
    const currentIndex = steps.indexOf(current);
    if (currentIndex > 0) {
      this.currentStep.set(steps[currentIndex - 1]);
    }
  }

  /**
   * Sets a specific step
   */
  goToStep(step: BookingStep): void {
    this.currentStep.set(step);
  }

  /**
   * Resets all state for a new booking
   */
  reset(): void {
    this.selectedBarber.set(null);
    this.selectedDate.set(null);
    this.selectedTime.set(null);
    this.currentStep.set('barber-selection');
    this.clientData.set(null);
    this.createdAppointment.set(null);
  }
}
