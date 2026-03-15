import { Component, input, output, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Appointment } from '../../models/appointment.model';

@Component({
  selector: 'app-confirmation',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    'class': 'w-full max-w-md mx-auto'
  },
  template: `
    <div class="flex flex-col items-center p-6 text-center" role="status" aria-live="polite">
      <div 
        class="w-18 h-18 bg-green-500 rounded-full flex items-center justify-center mb-6 animate-[scaleIn_0.3s_ease-out]"
        aria-hidden="true"
      >
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" class="w-9 h-9 text-white">
          <path d="M20 6L9 17l-5-5" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>
      </div>
      
      <h2 class="text-2xl font-semibold text-foreground mb-2">¡Reserva Confirmada!</h2>
      
      <p class="text-muted-foreground mb-6">
        Tu cita ha sido agendada exitosamente. Te hemos enviado un correo de confirmación.
      </p>

      <div class="w-full bg-muted/50 rounded-xl p-4 mb-6 text-left" aria-label="Detalles de la cita">
        <div class="flex justify-between py-2 border-b border-border">
          <span class="font-medium text-muted-foreground">Barbero:</span>
          <span class="text-foreground">{{ appointment()?.barberName }}</span>
        </div>
        
        <div class="flex justify-between py-2 border-b border-border">
          <span class="font-medium text-muted-foreground">Fecha:</span>
          <span class="text-foreground">{{ formatDate(appointment()?.appointmentDateTime) }}</span>
        </div>
        
        <div class="flex justify-between py-2 border-b border-border">
          <span class="font-medium text-muted-foreground">Hora:</span>
          <span class="text-foreground">{{ formatTime(appointment()?.appointmentDateTime) }}</span>
        </div>
        
        <div class="flex justify-between py-2 border-b border-border">
          <span class="font-medium text-muted-foreground">Cliente:</span>
          <span class="text-foreground">{{ appointment()?.clientName }}</span>
        </div>
        
        <div class="flex justify-between py-2 border-b border-border">
          <span class="font-medium text-muted-foreground">Teléfono:</span>
          <span class="text-foreground">{{ appointment()?.clientPhone }}</span>
        </div>
        
        <div class="flex justify-between py-2 border-b border-border">
          <span class="font-medium text-muted-foreground">Correo:</span>
          <span class="text-foreground">{{ appointment()?.clientEmail }}</span>
        </div>

        <div class="flex justify-between py-2 pt-3 border-t-2 border-dashed border-border">
          <span class="font-medium text-muted-foreground">Código de reserva:</span>
          <span class="text-primary font-bold text-lg">#{{ appointment()?.id }}</span>
        </div>
      </div>

      <button 
        class="px-6 py-2 bg-primary text-primary-foreground rounded-md hover:bg-primary/90 transition-colors font-medium"
        (click)="newBooking.emit()"
      >
        Nueva Reserva
      </button>
    </div>
  `,
  styles: []
})
export class ConfirmationComponent {
  appointment = input.required<Appointment>();
  newBooking = output<void>();

  formatDate(dateTime: string | undefined): string {
    if (!dateTime) return '';
    const date = new Date(dateTime);
    return date.toLocaleDateString('es-AR', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  formatTime(dateTime: string | undefined): string {
    if (!dateTime) return '';
    const date = new Date(dateTime);
    return date.toLocaleTimeString('es-AR', {
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}
