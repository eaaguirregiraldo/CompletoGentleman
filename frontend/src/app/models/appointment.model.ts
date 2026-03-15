/**
 * Appointment response from API
 */
export interface Appointment {
  id: number;
  clientName: string;
  clientPhone: string;
  clientEmail: string;
  barberId: number;
  barberName?: string;
  appointmentDateTime: string;
  status: string;
}
