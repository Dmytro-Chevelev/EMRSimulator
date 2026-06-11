import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SimulatorApiService } from '../../core/simulator-api.service';
import { Scenario } from '../../core/models';

@Component({
  selector: 'app-scenarios-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <h2>Scenario Management</h2>
    <p class="muted">Set deterministic simulation outcomes and keep them synchronized with backend state.</p>

    <div class="panel">
      <label for="scenario-select">Active scenario</label>
      <select id="scenario-select" [(ngModel)]="selectedScenario" (change)="applyScenario()">
        <option *ngFor="let scenario of scenarios" [value]="scenario.scenarioType">{{ scenario.name }}</option>
      </select>
      <p class="status">{{ statusMessage() }}</p>
    </div>

    <table>
      <thead>
        <tr><th>Name</th><th>Type</th><th>Seed</th><th>Active</th></tr>
      </thead>
      <tbody>
        <tr *ngFor="let scenario of scenarios">
          <td>{{ scenario.name }}</td>
          <td>{{ scenario.scenarioType }}</td>
          <td>{{ scenario.seed }}</td>
          <td>{{ scenario.isActive ? 'Yes' : 'No' }}</td>
        </tr>
      </tbody>
    </table>
  `,
  styles: [
    `
      .muted { color: var(--muted); }
      .panel { display: grid; gap: 0.5rem; max-width: 420px; margin-bottom: 1rem; }
      select { padding: 0.4rem; border-radius: 8px; border: 1px solid #c8d8bf; }
      table { width: 100%; border-collapse: collapse; }
      th, td { text-align: left; border-bottom: 1px solid #e1e8d9; padding: 0.45rem; }
      .status { color: var(--accent-2); }
    `
  ]
})
export class ScenariosPageComponent implements OnInit {
  scenarios: Scenario[] = [];
  selectedScenario = 'HappyPath';
  readonly statusMessage = signal('');

  constructor(private readonly api: SimulatorApiService) {}

  async ngOnInit(): Promise<void> {
    this.scenarios = await this.api.getScenarios();
    const current = this.scenarios.find((s) => s.isActive);
    this.selectedScenario = current?.scenarioType ?? 'HappyPath';
    this.statusMessage.set(`Current scenario: ${this.selectedScenario}`);
  }

  async applyScenario(): Promise<void> {
    await this.api.setScenario(this.selectedScenario);
    this.scenarios = await this.api.getScenarios();
    this.statusMessage.set(`Scenario changed to ${this.selectedScenario}`);
  }
}
