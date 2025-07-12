// src/app/models/service-booking.model.ts
export interface ServiceBooking {
  id: number;
  serviceId: number;
  customerId: number;
  scheduledTime: string;
  status: string;

  service: {
    title: string;
    category: string;
    provider: {
      name: string;
    };
  };

  customer: {
    name: string;
    email: string;
  };
}
