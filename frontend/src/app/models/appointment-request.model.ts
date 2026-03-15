/**
 * Request model for creating an appointment
 */
export interface CreateAppointmentRequest {
  clientName: string;
  clientPhone: string;
  clientEmail: string;
  barberId: number;
  appointmentDateTime: string;
}
