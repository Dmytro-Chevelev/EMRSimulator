export interface ProviderSelection {
  activeProvider: string;
  message: string;
}

export interface Scenario {
  id: string;
  name: string;
  scenarioType: string;
  isActive: boolean;
  seed: string;
}

export interface Patient {
  id: string;
  externalPatientId: string;
  mrn: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  gender: string;
  phone?: string;
  email?: string;
}

export interface Appointment {
  id: string;
  patientId: string;
  startTimeUtc: string;
  endTimeUtc: string;
  providerName: string;
  status: string;
}

export interface Order {
  id: string;
  patientId: string;
  orderType: string;
  status: string;
  placedAtUtc: string;
}

export interface Result {
  id: string;
  patientId: string;
  orderId?: string;
  resultType: string;
  value: string;
  resultedAtUtc: string;
}

export interface RequestLog {
  id: string;
  provider: string;
  route: string;
  method: string;
  responseCode: number;
  durationMs: number;
  createdAtUtc: string;
}

export interface ImportRow {
  rowNumber: number;
  accepted: boolean;
  reason?: string;
}

export interface ImportReport {
  sourceFormat: string;
  acceptedCount: number;
  rejectedCount: number;
  rows: ImportRow[];
}
