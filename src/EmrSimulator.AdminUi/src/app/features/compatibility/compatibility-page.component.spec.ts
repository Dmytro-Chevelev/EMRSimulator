import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CompatibilityPageComponent } from './compatibility-page.component';
import { SimulatorApiService } from '../../core/simulator-api.service';
import { EndpointContract, VerificationEvidence } from '../../core/models';

describe('CompatibilityPageComponent', () => {
  let fixture: ComponentFixture<CompatibilityPageComponent>;
  let api: jasmine.SpyObj<SimulatorApiService>;

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
      'getEndpointContracts',
      'getVerificationEvidence',
      'resetSimulator'
    ]);
    api.getEndpointContracts.and.resolveTo([contract]);
    api.getVerificationEvidence.and.resolveTo([evidence]);
    api.resetSimulator.and.resolveTo({ resetGeneration: 2, message: 'Reset complete' });

    await TestBed.configureTestingModule({
      imports: [CompatibilityPageComponent],
      providers: [{ provide: SimulatorApiService, useValue: api }]
    }).compileComponents();

    fixture = TestBed.createComponent(CompatibilityPageComponent);
  });

  it('loads endpoint coverage and verification evidence', async () => {
    fixture.detectChanges();
    await fixture.whenStable();
    fixture.detectChanges();

    fixture.componentInstance.selectContract(contract);
    await fixture.whenStable();
    fixture.detectChanges();

    const text = fixture.nativeElement.textContent as string;
    expect(text).toContain('Cerner');
    expect(text).toContain('/api/v1/cerner/patients');
    expect(text).toContain('Cerner patient list smoke');
  });

  it('resets simulator state from coverage view', async () => {
    fixture.detectChanges();
    await fixture.whenStable();

    await fixture.componentInstance.resetSimulator();
    fixture.detectChanges();

    expect(api.resetSimulator).toHaveBeenCalled();
    expect(fixture.nativeElement.textContent).toContain('Reset complete');
  });
});