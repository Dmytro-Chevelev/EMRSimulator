import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SimulatorApiService } from '../../core/simulator-api.service';
import { ProviderSelection } from '../../core/models';

@Component({
  selector: 'app-providers-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <h2>Provider Management</h2>
    <p class="muted">Switch the active provider and keep behavior provider-specific across routes.</p>

    <div class="panel">
      <label for="provider-select">Active provider</label>
      <select id="provider-select" [(ngModel)]="selectedProvider" (change)="applyProvider()">
        <option *ngFor="let provider of providers" [value]="provider.activeProvider">{{ provider.activeProvider }}</option>
      </select>
      <p class="status">{{ statusMessage() }}</p>
    </div>

    <ul>
      <li *ngFor="let provider of providers">{{ provider.activeProvider }} - {{ provider.message }}</li>
    </ul>
  `,
  styles: [
    `
      .muted { color: var(--muted); }
      .panel { display: grid; gap: 0.5rem; max-width: 420px; }
      select { padding: 0.4rem; border-radius: 8px; border: 1px solid #c8d8bf; }
      .status { color: var(--accent-2); }
    `
  ]
})
export class ProvidersPageComponent implements OnInit {
  providers: ProviderSelection[] = [];
  selectedProvider = '';
  readonly statusMessage = signal('');

  constructor(private readonly api: SimulatorApiService) {}

  async ngOnInit(): Promise<void> {
    this.providers = await this.api.getProviders();
    await this.api.loadActiveProvider();
    this.selectedProvider = this.api.activeProvider();
    this.statusMessage.set(`Current provider: ${this.selectedProvider}`);
  }

  async applyProvider(): Promise<void> {
    await this.api.setActiveProvider(this.selectedProvider);
    this.statusMessage.set(`Switched to ${this.api.activeProvider()}`);
  }
}
