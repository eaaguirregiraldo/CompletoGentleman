import { Component, inject, input, output, signal, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppointmentService } from '../../services/appointment.service';
import { CreateAppointmentRequest } from '../../models/appointment-request.model';

@Component({
  selector: 'app-appointment-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    'class': 'w-full'
  },
  template: `
    <h3 class="text-lg font-medium text-foreground mb-4">Datos del Cliente</h3>
    
    <form 
      [formGroup]="form" 
      (ngSubmit)="onSubmit()"
      class="flex flex-col gap-4"
    >
      <div class="flex flex-col gap-2">
        <label for="clientName" class="font-medium text-muted-foreground text-sm">
          Nombre completo <span class="text-destructive">*</span>
        </label>
        <input
          type="text"
          id="clientName"
          formControlName="clientName"
          class="px-3 py-2 border border-input bg-background rounded-md text-foreground focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2
                 [&.ng-invalid.ng-touched]:border-destructive"
          [class.border-destructive]="isFieldInvalid('clientName')"
          aria-required="true"
          aria-describedby="clientName-error"
        />
        @if (isFieldInvalid('clientName')) {
          <span id="clientName-error" class="text-destructive text-xs" role="alert">
            @if (form.get('clientName')?.errors?.['required']) {
              El nombre es requerido
            } @else if (form.get('clientName')?.errors?.['minlength']) {
              Mínimo 2 caracteres
            } @else if (form.get('clientName')?.errors?.['maxlength']) {
              Máximo 100 caracteres
            }
          </span>
        }
      </div>

      <div class="flex flex-col gap-2">
        <label for="clientPhone" class="font-medium text-muted-foreground text-sm">
          Teléfono <span class="text-destructive">*</span>
        </label>
        <input
          type="tel"
          id="clientPhone"
          formControlName="clientPhone"
          class="px-3 py-2 border border-input bg-background rounded-md text-foreground focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2
                 [&.ng-invalid.ng-touched]:border-destructive"
          [class.border-destructive]="isFieldInvalid('clientPhone')"
          aria-required="true"
          aria-describedby="clientPhone-error"
          placeholder="Ej: +54 11 1234 5678"
        />
        @if (isFieldInvalid('clientPhone')) {
          <span id="clientPhone-error" class="text-destructive text-xs" role="alert">
            @if (form.get('clientPhone')?.errors?.['required']) {
              El teléfono es requerido
            } @else if (form.get('clientPhone')?.errors?.['pattern']) {
              Formato de teléfono inválido
            }
          </span>
        }
      </div>

      <div class="flex flex-col gap-2">
        <label for="clientEmail" class="font-medium text-muted-foreground text-sm">
          Correo electrónico <span class="text-destructive">*</span>
        </label>
        <input
          type="email"
          id="clientEmail"
          formControlName="clientEmail"
          class="px-3 py-2 border border-input bg-background rounded-md text-foreground focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2
                 [&.ng-invalid.ng-touched]:border-destructive"
          [class.border-destructive]="isFieldInvalid('clientEmail')"
          aria-required="true"
          aria-describedby="clientEmail-error"
          placeholder="Ej: nombre@ejemplo.com"
        />
        @if (isFieldInvalid('clientEmail')) {
          <span id="clientEmail-error" class="text-destructive text-xs" role="alert">
            @if (form.get('clientEmail')?.errors?.['required']) {
              El correo es requerido
            } @else if (form.get('clientEmail')?.errors?.['email']) {
              Formato de correo inválido
            }
          </span>
        }
      </div>

      @if (appointmentService.error()) {
        <div class="p-3 bg-destructive/10 text-destructive rounded-md text-sm text-center" role="alert">
          {{ appointmentService.error() }}
        </div>
      }

      <div class="mt-2">
        <button 
          type="submit" 
          class="w-full px-4 py-2 bg-primary text-primary-foreground rounded-md hover:bg-primary/90 transition-colors font-medium
                 disabled:opacity-50 disabled:cursor-not-allowed"
          [disabled]="form.invalid || appointmentService.loading()"
        >
          @if (appointmentService.loading()) {
            <span class="flex items-center justify-center gap-2">Confirmando...</span>
          } @else {
            Confirmar Reserva
          }
        </button>
      </div>
    </form>
  `,
  styles: []
})
export class AppointmentFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  readonly appointmentService = inject(AppointmentService);

  barberId = input.required<number>();
  date = input.required<string>();
  time = input.required<string>();
  
  appointmentCreated = output<any>();

  form!: FormGroup;

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.form = this.fb.group({
      clientName: ['', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(100)
      ]],
      clientPhone: ['', [
        Validators.required,
        Validators.pattern(/^[\+]?[(]?[0-9]{1,3}[)]?[-\s\.]?[0-9]{1,4}[-\s\.]?[0-9]{1,4}[-\s\.]?[0-9]{1,9}$/)
      ]],
      clientEmail: ['', [
        Validators.required,
        Validators.email
      ]]
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.form.get(fieldName);
    return field ? field.invalid && field.touched : false;
  }

  onSubmit(): void {
    if (this.form.invalid) {
      // Mark all fields as touched to show errors
      Object.keys(this.form.controls).forEach(key => {
        this.form.get(key)?.markAsTouched();
      });
      return;
    }

    const formValue = this.form.value;
    
    // Combine date and time into ISO string
    const dateTimeStr = `${this.date()}T${this.time()}:00`;

    const request: CreateAppointmentRequest = {
      clientName: formValue.clientName,
      clientPhone: formValue.clientPhone,
      clientEmail: formValue.clientEmail,
      barberId: this.barberId(),
      appointmentDateTime: dateTimeStr
    };

    this.appointmentService.createAppointment(request).subscribe({
      next: (appointment) => {
        this.appointmentCreated.emit(appointment);
      },
      error: (err) => {
        // Error is handled in the service and displayed in template
        console.error('Error creating appointment:', err);
      }
    });
  }
}
