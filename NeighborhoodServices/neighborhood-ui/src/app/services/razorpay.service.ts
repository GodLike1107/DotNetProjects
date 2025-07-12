import { Injectable } from '@angular/core';

declare var Razorpay: any;

@Injectable({
  providedIn: 'root',
})
export class RazorpayService {
  private key = 'rzp_test_XQPgpaX2BGkzky'; // ✅ Your Razorpay Key ID

  constructor() {}

  pay(amount: number, bookingId: number, name: string, email: string): void {
    const options: any = {
      key: this.key,
      amount: amount * 100, // Amount in paise (₹1 = 100)
      currency: 'INR',
      name: 'Neighborhood Services',
      description: `Payment for Booking ID ${bookingId}`,
      prefill: {
        name: name,
        email: email,
      },
      theme: {
        color: '#1976d2',
      },
      handler: (response: any) => {
        alert('✅ Payment successful!');
        console.log('📦 Razorpay response:', response);
        // You can send response.razorpay_payment_id to your API for verification
      },
      modal: {
        ondismiss: () => {
          alert('❌ Payment cancelled.');
        },
      },
    };

    const rzp = new Razorpay(options);
    rzp.open();
  }
}
