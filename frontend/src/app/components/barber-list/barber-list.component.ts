import { Component, inject, output, signal, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BarberService } from '../../services/barber.service';
import { Barber } from '../../models/barber.model';
import { LoadingSpinner } from '../loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-barber-list',
  standalone: true,
  imports: [CommonModule, LoadingSpinner],
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    'class': 'w-full p-4'
  },
  template: `
    <h2 class="text-xl font-semibold text-foreground mb-6 text-center">Selecciona tu Barbero</h2>
    
    @if (barberService.loading()) {
      <app-loading-spinner message="Cargando barberos..." />
    } @else if (barberService.error()) {
      <div class="flex flex-col items-center justify-center p-8 text-destructive" role="alert">
        <p class="mb-4">{{ barberService.error() }}</p>
        <button 
          class="px-4 py-2 bg-primary text-primary-foreground rounded-md hover:bg-primary/90 transition-colors"
          (click)="loadBarbers()"
        >
          Reintentar
        </button>
      </div>
    } @else {
      <div class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 gap-4" role="listbox" aria-label="Lista de barberos">
        @for (barber of barberService.barbers(); track barber.id) {
          <button
            class="flex flex-col items-center p-4 border-2 rounded-lg bg-card cursor-pointer transition-all hover:border-primary hover:shadow-md
                   focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2
                   disabled:opacity-50 disabled:cursor-not-allowed"
            [class.border-primary]="selectedBarberId() === barber.id"
            [class.bg-accent]="selectedBarberId() === barber.id"
            [attr.aria-selected]="selectedBarberId() === barber.id"
            role="option"
            (click)="selectBarber(barber)"
            [disabled]="!barber.active"
          >
            <div 
              class="w-16 h-16 rounded-full bg-muted text-muted-foreground flex items-center justify-center text-xl font-semibold mb-3"
              aria-hidden="true"
            >
              {{ getInitials(barber.name) }}
            </div>
            <h3 class="text-sm font-medium text-foreground text-center">{{ barber.name }}</h3>
            @if (!barber.active) {
              <span class="text-xs text-muted-foreground mt-1">No disponible</span>
            }
          </button>
        } @empty {
          <div class="col-span-full text-center p-8 text-muted-foreground">
            <p>No hay barberos disponibles en este momento.</p>
          </div>
        }
      </div>
    }
  `,
  styles: []
})
export class BarberListComponent implements OnInit {
  readonly barberService = inject(BarberService);
  
  barberSelected = output<Barber>();
  selectedBarberId = signal<number | null>(null);

  ngOnInit(): void {
    this.loadBarbers();
  }

  loadBarbers(): void {
    this.barberService.getBarbers().subscribe();
  }

  selectBarber(barber: Barber): void {
    this.selectedBarberId.set(barber.id);
    this.barberSelected.emit(barber);
  }

  getInitials(name: string): string {
    return name
      .split(' ')
      .map(n => n[0])
      .join('')
      .substring(0, 2)
      .toUpperCase();
  }
}
