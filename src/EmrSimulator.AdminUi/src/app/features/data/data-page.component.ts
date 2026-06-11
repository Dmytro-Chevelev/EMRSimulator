import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SimulatorApiService } from '../../core/simulator-api.service';
import { Appointment, Order, Patient, Result } from '../../core/models';

@Component({
  selector: 'app-data-page',
  standalone: true,
  imports: [CommonModule],
  template: `
    <h2>Synthetic Data Management</h2>
    <p class="muted">Review seeded patients, appointments, orders, and results.</p>

    <section class="grid">
      <article>
        <h3>Patients ({{ patients.length }})</h3>
        <ul><li *ngFor="let p of patients">{{ p.lastName }}, {{ p.firstName }} - {{ p.mrn }}</li></ul>
      </article>
      <article>
        <h3>Appointments ({{ appointments.length }})</h3>
        <ul><li *ngFor="let a of appointments">{{ a.providerName }} - {{ a.status }}</li></ul>
      </article>
      <article>
        <h3>Orders ({{ orders.length }})</h3>
        <ul><li *ngFor="let o of orders">{{ o.orderType }} - {{ o.status }}</li></ul>
      </article>
      <article>
        <h3>Results ({{ results.length }})</h3>
        <ul><li *ngFor="let r of results">{{ r.resultType }} - {{ r.value }}</li></ul>
      </article>
    </section>
  `,
  styles: [
    `
      .muted { color: var(--muted); }
      .grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(220px, 1fr)); gap: 0.8rem; }
      article { background: var(--surface-2); border: 1px solid #d2dfc5; border-radius: 12px; padding: 0.7rem; }
      ul { padding-left: 1rem; margin: 0.4rem 0 0; }
    `
  ]
})
export class DataPageComponent implements OnInit {
  patients: Patient[] = [];
  appointments: Appointment[] = [];
  orders: Order[] = [];
  results: Result[] = [];

  constructor(private readonly api: SimulatorApiService) {}

  async ngOnInit(): Promise<void> {
    const [patients, appointments, orders, results] = await Promise.all([
      this.api.getPatients(),
      this.api.getAppointments(),
      this.api.getOrders(),
      this.api.getResults()
    ]);

    this.patients = patients;
    this.appointments = appointments;
    this.orders = orders;
    this.results = results;
  }
}
