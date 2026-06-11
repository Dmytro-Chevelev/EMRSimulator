import { CommonModule } from '@angular/common';
import { Component, OnInit, computed, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { EndpointContract, VerificationEvidence } from '../../core/models';
import { SimulatorApiService } from '../../core/simulator-api.service';

@Component({
  selector: 'app-compatibility-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="toolbar">
      <div>
        <h2>External EMR Compatibility</h2>
        <p class="muted">Native endpoint coverage, protocol families, verification evidence, and simulator reset.</p>
      </div>
      <button type="button" (click)="resetSimulator()" [disabled]="loading()">Reset State</button>
    </div>

    <div class="status" *ngIf="statusMessage()">{{ statusMessage() }}</div>

    <section class="filters">
      <label>
        Provider
        <select [(ngModel)]="providerFilter">
          <option value="">All</option>
          <option *ngFor="let provider of providers()" [value]="provider">{{ provider }}</option>
        </select>
      </label>

      <label>
        Protocol
        <select [(ngModel)]="protocolFilter">
          <option value="">All</option>
          <option *ngFor="let protocol of protocols()" [value]="protocol">{{ protocol }}</option>
        </select>
      </label>

      <label>
        Search
        <input [(ngModel)]="searchTerm" placeholder="route, operation, source..." />
      </label>
    </section>

    <div class="summary">
      <span>{{ filteredContracts().length }} endpoints</span>
      <span>{{ verifiedCount() }} verified</span>
      <span>{{ nativeProtocolCount() }} native protocol families</span>
    </div>

    <table>
      <thead>
        <tr>
          <th>Provider</th>
          <th>Family</th>
          <th>Protocol</th>
          <th>Endpoint / Action</th>
          <th>Status</th>
          <th>Evidence</th>
        </tr>
      </thead>
      <tbody>
        <tr *ngFor="let contract of filteredContracts()" [class.selected]="selectedContract()?.id === contract.id">
          <td>{{ contract.provider }}</td>
          <td>{{ contract.contractFamily }}</td>
          <td>{{ contract.protocol }}</td>
          <td>
            <strong>{{ contract.method || 'MSG' }}</strong>
            {{ contract.pathPattern || contract.actionName }}
            <small>{{ contract.purpose }}</small>
          </td>
          <td>{{ contract.supportStatus }}</td>
          <td><button type="button" class="secondary" (click)="selectContract(contract)">Open</button></td>
        </tr>
      </tbody>
    </table>

    <aside class="evidence" *ngIf="selectedContract() as contract">
      <h3>{{ contract.contractKey }}</h3>
      <dl>
        <dt>Source</dt><dd>{{ contract.sourceDocument }} {{ contract.sourceAnchor }}</dd>
        <dt>Serializers</dt><dd>{{ contract.acceptedSerializerVariants }}</dd>
        <dt>Auth</dt><dd>{{ contract.authRequired ? 'Required' : 'Optional' }}</dd>
      </dl>

      <h4>Evidence</h4>
      <p class="muted" *ngIf="evidence().length === 0">No evidence has been recorded for this endpoint yet.</p>
      <ul>
        <li *ngFor="let item of evidence()">
          <span [class.pass]="item.passed" [class.fail]="!item.passed">{{ item.actualStatus }}</span>
          {{ item.verificationName }} · {{ item.toolOrTestName }}
        </li>
      </ul>
    </aside>
  `,
  styles: [
    `
      .toolbar { display: flex; justify-content: space-between; gap: 1rem; align-items: start; }
      h2, h3, h4 { margin: 0 0 0.35rem; }
      .muted { color: var(--muted); }
      button { border: 0; background: var(--accent); color: #f7fbf6; border-radius: 8px; padding: 0.55rem 0.9rem; cursor: pointer; }
      button:disabled { opacity: 0.55; cursor: wait; }
      button.secondary { background: #edf4e6; color: var(--text); border: 1px solid #c8d8bf; }
      .status { margin: 0.8rem 0; background: #eef7eb; border: 1px solid #c8d8bf; border-radius: 8px; padding: 0.6rem; }
      .filters { display: grid; grid-template-columns: repeat(3, minmax(0, 1fr)); gap: 0.8rem; margin: 1rem 0; }
      label { display: grid; gap: 0.35rem; color: var(--muted); }
      select, input { border: 1px solid #c8d8bf; border-radius: 8px; padding: 0.45rem; background: #fff; color: var(--text); }
      .summary { display: flex; gap: 0.6rem; flex-wrap: wrap; margin-bottom: 0.7rem; }
      .summary span { background: #f4f8ef; border: 1px solid #dce8d3; border-radius: 8px; padding: 0.45rem 0.65rem; }
      table { width: 100%; border-collapse: collapse; }
      th, td { text-align: left; border-bottom: 1px solid #e1e8d9; padding: 0.45rem; font-size: 0.9rem; vertical-align: top; }
      td small { display: block; color: var(--muted); margin-top: 0.2rem; }
      tr.selected { background: #f6faf1; }
      .evidence { margin-top: 1rem; border: 1px solid #d8e2cb; border-radius: 8px; padding: 0.9rem; }
      dl { display: grid; grid-template-columns: max-content 1fr; gap: 0.35rem 0.8rem; margin: 0.6rem 0 1rem; }
      dt { color: var(--muted); }
      dd { margin: 0; }
      ul { margin: 0; padding-left: 1.1rem; }
      .pass { color: #1f6f43; font-weight: 700; }
      .fail { color: #9f2d20; font-weight: 700; }

      @media (max-width: 760px) {
        .toolbar { display: grid; }
        .filters { grid-template-columns: 1fr; }
        table { display: block; overflow-x: auto; }
      }
    `
  ]
})
export class CompatibilityPageComponent implements OnInit {
  providerFilter = '';
  protocolFilter = '';
  searchTerm = '';
  readonly contracts = signal<EndpointContract[]>([]);
  readonly evidence = signal<VerificationEvidence[]>([]);
  readonly selectedContract = signal<EndpointContract | undefined>(undefined);
  readonly loading = signal(false);
  readonly statusMessage = signal('');

  readonly providers = computed(() => Array.from(new Set(this.contracts().map((contract) => contract.provider))).sort());
  readonly protocols = computed(() => Array.from(new Set(this.contracts().map((contract) => contract.protocol))).sort());
  readonly verifiedCount = computed(() => this.contracts().filter((contract) => contract.supportStatus === 'Verified').length);
  readonly nativeProtocolCount = computed(() => new Set(this.contracts().map((contract) => contract.protocol)).size);

  constructor(private readonly api: SimulatorApiService) {}

  async ngOnInit(): Promise<void> {
    this.loading.set(true);
    try {
      this.contracts.set(await this.api.getEndpointContracts());
    } finally {
      this.loading.set(false);
    }
  }

  filteredContracts(): EndpointContract[] {
    const search = this.searchTerm.trim().toLowerCase();
    return this.contracts().filter((contract) => {
      const providerMatch = !this.providerFilter || contract.provider === this.providerFilter;
      const protocolMatch = !this.protocolFilter || contract.protocol === this.protocolFilter;
      const searchMatch = !search || [contract.contractKey, contract.pathPattern, contract.actionName, contract.purpose, contract.sourceDocument]
        .filter(Boolean)
        .some((value) => value!.toLowerCase().includes(search));

      return providerMatch && protocolMatch && searchMatch;
    });
  }

  async selectContract(contract: EndpointContract): Promise<void> {
    this.selectedContract.set(contract);
    this.evidence.set(await this.api.getVerificationEvidence(contract.id));
  }

  async resetSimulator(): Promise<void> {
    this.loading.set(true);
    try {
      const result = await this.api.resetSimulator();
      this.statusMessage.set(result.message);
    } finally {
      this.loading.set(false);
    }
  }
}