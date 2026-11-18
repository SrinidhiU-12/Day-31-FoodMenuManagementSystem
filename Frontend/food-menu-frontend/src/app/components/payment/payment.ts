import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Payment } from '../../models/payment';
import { PaymentService } from '../../services/payment';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './payment.html',
  styleUrl: './payment.css'
})
export class PaymentComponent implements OnInit {

  orderId = 0;
  paymentMethod = 'CashOnDelivery';
  payment: Payment | null = null;

  isLoading = false;
  isSubmitting = false;

  successMessage = '';
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private paymentService: PaymentService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.orderId = Number(
      this.route.snapshot.paramMap.get('orderId')
    );

    this.loadPayment();
  }

  loadPayment(): void {
    this.isLoading = true;

    this.paymentService.getByOrderId(this.orderId).subscribe({
      next: response => {
        this.payment = response;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: error => {
        this.payment = null;
        this.isLoading = false;

        if (error.status !== 404) {
          this.errorMessage = 'Unable to load payment details.';
        }

        this.cdr.detectChanges();
      }
    });
  }

  createPayment(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.orderId || this.orderId <= 0) {
      this.errorMessage = 'Invalid order ID.';
      return;
    }

    this.isSubmitting = true;

    this.paymentService.create(
      this.orderId,
      this.paymentMethod
    ).subscribe({
      next: response => {
        this.payment = response;
        this.successMessage =
          'Payment record created successfully.';
        this.isSubmitting = false;
        this.cdr.detectChanges();
      },
      error: error => {
        this.errorMessage =
          error.error?.message || 'Unable to create payment.';
        this.isSubmitting = false;
        this.cdr.detectChanges();
      }
    });
  }
}