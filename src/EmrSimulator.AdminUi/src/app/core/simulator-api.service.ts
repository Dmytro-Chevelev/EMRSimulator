import { Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import {
  Appointment,
  EndpointContract,
  ImportReport,
  Order,
  Patient,
  ProviderSelection,
  RequestLog,
  Result,
  Scenario,
  SimulatorResetResult,
  VerificationEvidence
} from './models';

@Injectable({ providedIn: 'root' })
export class SimulatorApiService {
  private readonly baseUrl = '/api/v1';
  private readonly _activeProvider = signal<string>('Epic');
  private readonly _activeScenario = signal<string>('HappyPath');

  readonly activeProvider = computed(() => this._activeProvider());
  readonly activeScenario = computed(() => this._activeScenario());

  constructor(private readonly http: HttpClient) {}

  async loadActiveProvider(): Promise<void> {
    const provider = await firstValueFrom(this.http.get<ProviderSelection>(`${this.baseUrl}/providers/active`));
    this._activeProvider.set(provider.activeProvider);
  }

  async setActiveProvider(provider: string): Promise<void> {
    const value = provider.replace(/\s+/g, '');
    const providerResult = await firstValueFrom(this.http.put<ProviderSelection>(`${this.baseUrl}/providers/active/${value}`, {}));
    this._activeProvider.set(providerResult.activeProvider);
  }

  async getProviders(): Promise<ProviderSelection[]> {
    return firstValueFrom(this.http.get<ProviderSelection[]>(`${this.baseUrl}/providers`));
  }

  async getScenarios(): Promise<Scenario[]> {
    return firstValueFrom(this.http.get<Scenario[]>(`${this.baseUrl}/scenarios`));
  }

  async setScenario(scenarioType: string): Promise<void> {
    const updated = await firstValueFrom(this.http.put<Scenario>(`${this.baseUrl}/scenarios/active/${scenarioType}`, {}));
    this._activeScenario.set(updated.scenarioType);
  }

  async getPatients(): Promise<Patient[]> {
    return firstValueFrom(this.http.get<Patient[]>(`${this.baseUrl}/patients`));
  }

  async getAppointments(): Promise<Appointment[]> {
    return firstValueFrom(this.http.get<Appointment[]>(`${this.baseUrl}/appointments`));
  }

  async getOrders(): Promise<Order[]> {
    return firstValueFrom(this.http.get<Order[]>(`${this.baseUrl}/orders`));
  }

  async getResults(): Promise<Result[]> {
    return firstValueFrom(this.http.get<Result[]>(`${this.baseUrl}/results`));
  }

  async importPatients(sourceFormat: 'csv' | 'json', content: string): Promise<ImportReport> {
    return firstValueFrom(this.http.post<ImportReport>(`${this.baseUrl}/import/patients`, content, {
      headers: {
        'Content-Type': 'text/plain'
      }
    }));
  }

  async getRequestLogs(): Promise<RequestLog[]> {
    return firstValueFrom(this.http.get<RequestLog[]>(`${this.baseUrl}/request-logs`));
  }

  async getEndpointContracts(): Promise<EndpointContract[]> {
    return firstValueFrom(this.http.get<EndpointContract[]>(`${this.baseUrl}/endpoint-contracts`));
  }

  async getVerificationEvidence(endpointContractId: string): Promise<VerificationEvidence[]> {
    return firstValueFrom(this.http.get<VerificationEvidence[]>(`${this.baseUrl}/endpoint-contracts/${endpointContractId}/verification`));
  }

  async resetSimulator(): Promise<SimulatorResetResult> {
    return firstValueFrom(this.http.post<SimulatorResetResult>(`${this.baseUrl}/simulator/reset`, {}));
  }
}
