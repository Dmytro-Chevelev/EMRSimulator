import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RequestLogsPageComponent } from './request-logs-page.component';
import { SimulatorApiService } from '../../core/simulator-api.service';
import { EndpointContract, RequestLog, VerificationEvidence } from '../../core/models';

describe('RequestLogsPageComponent', () => {
  let fixture: ComponentFixture<RequestLogsPageComponent>;
  let api: jasmine.SpyObj<SimulatorApiService>;

  const requestLog: RequestLog = {
    id: 'log-1',
    provider: 'Cerner',
    route: '/api/v1/cerner/patients',
    method: 'GET',
    responseCode: 200,
    durationMs: 15,
    createdAtUtc: '2026-06-11T00:00:00Z'
  };

  const contract: EndpointContract = {
    id: 'contract-1',
    contractKey: 'cerner-patients',
    provider: 'Cerner',
    contractFamily: 'CernerMidmarkService',
    direction: 'ConnectorToSimulator',
    protocol: 'HTTP_REST',
    method: 'GET',
    pathPattern: '/api/v1/cerner/patients',
    purpose: 'List seeded database patients',
    requestContractName: 'None',
    responseContractName: 'CernerMidmarkPatientResponse',
    authRequired: false,
    acceptedSerializerVariants: 'camelCase',
    supportStatus: 'Verified',
    sourceDocument: '.docs/external-emr-endpoints.md',
    sourceAnchor: 'Cerner patient list'
  };

  const evidence: VerificationEvidence = {
    id: 'evidence-1',
    endpointContractId: contract.id,
    emrProfileId: 'profile-1',
    verificationName: 'Cerner patient list smoke',
    expectedOutcome: '200',
    actualStatus: '200',
    passed: true,
    verifiedAtUtc: '2026-06-11T00:00:00Z',
    toolOrTestName: 'Integration test'
  };

  beforeEach(async () => {
    api = jasmine.createSpyObj<SimulatorApiService>('SimulatorApiService', [
      'getRequestLogs',
      'getEndpointContracts',
      'getVerificationEvidence',
      'resetSimulator'
    ]);
    api.getRequestLogs.and.resolveTo([requestLog]);
    api.getEndpointContracts.and.resolveTo([contract]);
    api.getVerificationEvidence.and.resolveTo([evidence]);
    api.resetSimulator.and.resolveTo({ resetGeneration: 3, message: 'Reset complete' });

    await TestBed.configureTestingModule({
      imports: [RequestLogsPageComponent],
      providers: [{ provide: SimulatorApiService, useValue: api }]
    }).compileComponents();

    fixture = TestBed.createComponent(RequestLogsPageComponent);
  });

  it('loads request logs and verification evidence', async () => {
    fixture.detectChanges();
    await fixture.whenStable();
    fixture.detectChanges();

    const text = fixture.nativeElement.textContent as string;
    expect(text).toContain('Cerner');
    expect(text).toContain('/api/v1/cerner/patients');
    expect(text).toContain('Cerner patient list smoke');
  });

  it('resets simulator state and reloads logs', async () => {
    fixture.detectChanges();
    await fixture.whenStable();

    await fixture.componentInstance.resetSimulator();
    fixture.detectChanges();

    expect(api.resetSimulator).toHaveBeenCalled();
    expect(api.getRequestLogs).toHaveBeenCalledTimes(2);
    expect(fixture.nativeElement.textContent).toContain('Reset complete');
  });
});