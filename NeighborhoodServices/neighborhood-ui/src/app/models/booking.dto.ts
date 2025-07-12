export interface BookingDto {
  id: number;
  serviceTitle: string;
  providerName: string;
  scheduledTime: string; // Use `string` if it's ISO format from backend (e.g., "2025-06-22T14:30:00Z")
  status: string;
  customerName: string;
  customerEmail: string;
}
