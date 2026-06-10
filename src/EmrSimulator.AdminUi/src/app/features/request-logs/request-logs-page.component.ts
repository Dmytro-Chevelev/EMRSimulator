import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SimulatorApiService } from '../../core/simulator-api.service';
import { RequestLog } from '../../core/models';

@Component({
  selector: 'app-request-logs-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <h2>Request Log Viewer</h2>
    <p class="muted">Filter request history by provider and inspect response metrics.</p>

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
      .muted { color: var(--muted); }
      .filter { display: grid; gap: 0.3rem; max-width: 340px; margin-bottom: 0.8rem; }
      input { border: 1px solid #c8d8bf; border-radius: 8px; padding: 0.4rem; }
      table { width: 100%; border-collapse: collapse; }
      th, td { text-align: left; border-bottom: 1px solid #e1e8d9; padding: 0.4rem; font-size: 0.92rem; }
    `
  ]
})
export class RequestLogsPageComponent implements OnInit {
  logs: RequestLog[] = [];
  providerFilter = '';

  constructor(private readonly api: SimulatorApiService) {}

  async ngOnInit(): Promise<void> {
    this.logs = await this.api.getRequestLogs();
  }

  filteredLogs(): RequestLog[] {
    const token = this.providerFilter.trim().toLowerCase();
    if (!token) {
      return this.logs;
    }

    return this.logs.filter((log) => log.provider.toLowerCase().includes(token));
  }
}
