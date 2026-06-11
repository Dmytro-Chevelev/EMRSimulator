import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SimulatorApiService } from '../../core/simulator-api.service';
import { EndpointContract, RequestLog, VerificationEvidence } from '../../core/models';

@Component({
  selector: 'app-request-logs-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="toolbar">
      <div>
        <h2>Request Log Viewer</h2>
        <p class="muted">Filter request history by provider and inspect response metrics.</p>
      </div>
      <button type="button" (click)="resetSimulator()" [disabled]="loading">Reset State</button>
    </div>

    <div class="status" *ngIf="statusMessage">{{ statusMessage }}</div>

    <section class="evidence-summary" *ngIf="verificationEvidence.length > 0">
      <h3>Verification Evidence</h3>
      <ul>
        <li *ngFor="let item of verificationEvidence">
          <span [class.pass]="item.passed" [class.fail]="!item.passed">{{ item.actualStatus }}</span>
          {{ item.verificationName }} · {{ item.toolOrTestName }}
        </li>
      </ul>
    </section>

    <label class="filter">
      Provider filter
      <input [(ngModel)]="providerFilter" placeholder="epic, cerner, altera..." />
    </label>

    <table>
      <thead>
        <tr><th>Provider</th><th>Route</th><th>Method</th><th>Status</th><th>Duration (ms)</th><th>Time</th></tr>
      </thead>
      <tbody>
        <tr *ngFor="let log of filteredLogs()">
          <td>{{ log.provider }}</td>
          <td>{{ log.route }}</td>
          <td>{{ log.method }}</td>
          <td>{{ log.responseCode }}</td>
          <td>{{ log.durationMs }}</td>
          <td>{{ log.createdAtUtc }}</td>
        </tr>
      </tbody>
    </table>
  `,
  styles: [
    `
      .toolbar { display: flex; justify-content: space-between; gap: 1rem; align-items: start; }
      .muted { color: var(--muted); }
      button { border: 0; background: var(--accent); color: #f7fbf6; border-radius: 8px; padding: 0.55rem 0.9rem; cursor: pointer; }
      button:disabled { opacity: 0.55; cursor: wait; }
      .status { margin: 0.8rem 0; background: #eef7eb; border: 1px solid #c8d8bf; border-radius: 8px; padding: 0.6rem; }
      .evidence-summary { margin: 0.9rem 0; border: 1px solid #d8e2cb; border-radius: 8px; padding: 0.8rem; }
      .evidence-summary h3 { margin: 0 0 0.45rem; }
      .evidence-summary ul { margin: 0; padding-left: 1.1rem; }
      .pass { color: #1f6f43; font-weight: 700; }
      .fail { color: #9f2d20; font-weight: 700; }
      .filter { display: grid; gap: 0.3rem; max-width: 340px; margin-bottom: 0.8rem; }
      input { border: 1px solid #c8d8bf; border-radius: 8px; padding: 0.4rem; }
      table { width: 100%; border-collapse: collapse; }
      th, td { text-align: left; border-bottom: 1px solid #e1e8d9; padding: 0.4rem; font-size: 0.92rem; }
      @media (max-width: 760px) {
        .toolbar { display: grid; }
      }
    `
  ]
})
export class RequestLogsPageComponent implements OnInit {
  logs: RequestLog[] = [];
  endpointContracts: EndpointContract[] = [];
  verificationEvidence: VerificationEvidence[] = [];
  providerFilter = '';
  statusMessage = '';
  loading = false;

  constructor(private readonly api: SimulatorApiService) {}

  async ngOnInit(): Promise<void> {
    await this.loadDashboardData();
  }

  filteredLogs(): RequestLog[] {
    const token = this.providerFilter.trim().toLowerCase();
    if (!token) {
      return this.logs;
    }

    return this.logs.filter((log) => log.provider.toLowerCase().includes(token));
  }

  async resetSimulator(): Promise<void> {
    this.loading = true;
    try {
      const result = await this.api.resetSimulator();
      this.statusMessage = result.message;
      await this.loadDashboardData();
    } finally {
      this.loading = false;
    }
  }

  private async loadDashboardData(): Promise<void> {
    const [logs, endpointContracts] = await Promise.all([
      this.api.getRequestLogs(),
      this.api.getEndpointContracts()
    ]);

    this.logs = logs;
    this.endpointContracts = endpointContracts;
    this.verificationEvidence = (await Promise.all(
      endpointContracts.slice(0, 5).map((contract) => this.api.getVerificationEvidence(contract.id))
    )).flat();
  }
}
