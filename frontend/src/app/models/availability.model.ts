/**
 * Time slot model for availability
 */
export interface TimeSlot {
  hour: number;
  minute: number;
  formatted: string;
}

/**
 * Availability response from API
 */
export interface AvailabilityResponse {
  barberId: number;
  date: string;
  availableSlots: string[];
  isAvailable: boolean;
}
