import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SimulatorApiService } from '../../core/simulator-api.service';
import { ImportReport } from '../../core/models';

@Component({
  selector: 'app-imports-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <h2>Import Wizard</h2>
    <p class="muted">Upload CSV or JSON-shaped rows and review accepted/rejected results.</p>

    <div class="controls">
      <label>
        Source format
        <select [(ngModel)]="sourceFormat">
          <option value="csv">CSV</option>
          <option value="json">JSON</option>
        </select>
      </label>

      <label>
        Payload
        <textarea rows="6" [(ngModel)]="payload"></textarea>
      </label>

      <button (click)="runImport()">Run Import</button>
      <p class="status">{{ statusMessage() }}</p>
    </div>

    <section *ngIf="report">
      <h3>Import Report</h3>
      <p>Accepted: {{ report.acceptedCount }} | Rejected: {{ report.rejectedCount }}</p>
      <ul>
        <li *ngFor="let row of report.rows">
          Row {{ row.rowNumber }} - {{ row.accepted ? 'Accepted' : ('Rejected: ' + row.reason) }}
        </li>
      </ul>
    </section>
  `,
  styles: [
    `
      .muted { color: var(--muted); }
      .controls { display: grid; gap: 0.7rem; max-width: 560px; }
      label { display: grid; gap: 0.35rem; }
      textarea, select { border: 1px solid #c8d8bf; border-radius: 8px; padding: 0.45rem; }
      button { width: fit-content; background: var(--accent); color: #fff; border: none; border-radius: 8px; padding: 0.45rem 0.8rem; }
      .status { color: var(--accent-2); }
    `
  ]
})
export class ImportsPageComponent {
  sourceFormat: 'csv' | 'json' = 'csv';
  payload = 'EP-3001,MRN-3001,Sam,Fields,1992-03-11,Female';
  report: ImportReport | null = null;
  readonly statusMessage = signal('Ready to import synthetic records.');

  constructor(private readonly api: SimulatorApiService) {}

  async runImport(): Promise<void> {
    this.report = await this.api.importPatients(this.sourceFormat, this.payload);
    this.statusMessage.set(`Import complete (${this.report.acceptedCount} accepted / ${this.report.rejectedCount} rejected).`);
  }
}
