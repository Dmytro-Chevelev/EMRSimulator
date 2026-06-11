import { Routes } from '@angular/router';
import { ProvidersPageComponent } from './features/providers/providers-page.component';
import { ScenariosPageComponent } from './features/scenarios/scenarios-page.component';
import { DataPageComponent } from './features/data/data-page.component';
import { ImportsPageComponent } from './features/imports/imports-page.component';
import { RequestLogsPageComponent } from './features/request-logs/request-logs-page.component';
import { CompatibilityPageComponent } from './features/compatibility/compatibility-page.component';

export const routes: Routes = [
  { path: '', redirectTo: 'providers', pathMatch: 'full' },
  { path: 'providers', component: ProvidersPageComponent },
  { path: 'scenarios', component: ScenariosPageComponent },
  { path: 'data', component: DataPageComponent },
  { path: 'imports', component: ImportsPageComponent },
  { path: 'compatibility', component: CompatibilityPageComponent },
  { path: 'request-logs', component: RequestLogsPageComponent }
];
