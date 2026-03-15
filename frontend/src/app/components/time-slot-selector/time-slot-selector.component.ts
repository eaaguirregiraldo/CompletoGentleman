import { Component, inject, input, output, signal, OnInit, OnChanges, SimpleChanges, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BarberService } from '../../services/barber.service';
import { AvailabilityResponse } from '../../models/availability.model';
import { LoadingSpinner } from '../loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-time-slot-selector',
  standalone: true,
  imports: [CommonModule, LoadingSpinner],
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    'class': 'w-full'
  },
  template: `
    <h3 class="text-lg font-medium text-foreground mb-4">Selecciona un horario</h3>
    
    @if (barberService.loading()) {
      <app-loading-spinner message="Cargando horarios disponibles..." />
    } @else if (barberService.error()) {
      <div class="flex flex-col items-center justify-center p-6 text-destructive" role="alert">
        <p class="mb-4">{{ barberService.error() }}</p>
        <button 
          class="px-4 py-2 bg-primary text-primary-foreground rounded-md hover:bg-primary/90 transition-colors"
          (click)="loadAvailability()"
        >
          Reintentar
        </button>
      </div>
    } @else if (!availability()?.isAvailable) {
      <div class="text-center p-6 text-muted-foreground">
        <p>El barbero no tiene horarios disponibles para esta fecha.</p>
        <p class="text-sm text-muted-foreground/70 mt-1">Intenta seleccionar otra fecha.</p>
      </div>
    } @else {
      <div class="flex items-center gap-4 mb-6">
        <label for="date-input" class="font-medium text-muted-foreground">Fecha:</label>
        <input
          type="date"
          id="date-input"
          class="px-3 py-2 border border-input bg-background rounded-md text-foreground focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2"
          [value]="selectedDate()"
          [min]="minDate"
          (change)="onDateChange($event)"
          aria-label="Seleccionar fecha"
        />
      </div>

      @if (availability()?.availableSlots?.length) {
        <div 
          class="grid grid-cols-3 sm:grid-cols-4 md:grid-cols-5 gap-3" 
          role="listbox" 
          aria-label="Horarios disponibles"
        >
          @for (slot of availability()!.availableSlots; track slot) {
            <button
              class="px-3 py-2 border-2 border-input bg-background rounded-md text-foreground cursor-pointer transition-all hover:border-primary hover:bg-accent
                     focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2"
              [class.border-primary]="selectedTime() === slot"
              [class.bg-primary]="selectedTime() === slot"
              [class.text-primary-foreground]="selectedTime() === slot"
              [attr.aria-selected]="selectedTime() === slot"
              role="option"
              (click)="selectTime(slot)"
            >
              {{ formatTime(slot) }}
            </button>
          } @empty {
            <p class="col-span-full text-center text-muted-foreground py-4">No hay horarios disponibles para esta fecha.</p>
          }
        </div>
      } @else if (selectedDate()) {
        <p class="text-center text-muted-foreground py-4">No hay horarios disponibles para esta fecha.</p>
      }
    }
  `,
  styles: []
})
export class TimeSlotSelector implements OnInit, OnChanges {
  readonly barberService = inject(BarberService);

  barberId = input.required<number>();
  date = input<string | null>(null);
  
  timeSelected = output<string>();
  
  availability = signal<AvailabilityResponse | null>(null);
  selectedTime = signal<string | null>(null);
  selectedDate = signal<string>('');

  // Minimum date is today
  readonly minDate: string;

  constructor() {
    const today = new Date();
    this.minDate = today.toISOString().split('T')[0];
    this.selectedDate.set(this.minDate);
  }

  ngOnInit(): void {
    if (this.date()) {
      this.selectedDate.set(this.date()!);
    }
    this.loadAvailability();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['date'] && this.date()) {
      this.selectedDate.set(this.date()!);
      this.selectedTime.set(null);
      this.loadAvailability();
    }
  }

  loadAvailability(): void {
    const dateToLoad = this.selectedDate();
    if (dateToLoad && this.barberId()) {
      this.barberService.getAvailability(this.barberId(), dateToLoad).subscribe({
        next: (response) => {
          this.availability.set(response);
        },
        error: () => {
          this.availability.set(null);
        }
      });
    }
  }

  onDateChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedDate.set(input.value);
    this.selectedTime.set(null);
    this.loadAvailability();
  }

  selectTime(time: string): void {
    this.selectedTime.set(time);
    this.timeSelected.emit(time);
  }

  formatTime(time: string): string {
    // Time comes as "HH:mm" format
    return time;
  }
}
