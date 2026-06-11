# Admin UI Smoke Test

## Environment

- API: `http://localhost:5288`
- Admin UI: `http://localhost:4200`
- Browser: Headless Microsoft Edge via temporary Playwright workspace outside the repository

## Contract Results

| Contract | Route or Trigger | Status | Evidence |
|----------|------------------|--------|----------|
| C3-001 | `npm run build` | Pass | Exit 0, 6.66 seconds, dist index exists; see iteration-verification.md |
| C3-002 | `npm start` | Pass | Dev server started at `http://localhost:4200/` in 2.954 seconds; HTTP 200 |
| C3-003 | `/providers` | Pass | Route HTTP 200; browser console errors 0; `/api/v1/providers` HTTP 200 |
| C3-004 | `/scenarios` | Pass | Route HTTP 200; browser console errors 0; `/api/v1/scenarios` HTTP 200 |
| C3-005 | `/data` | Pass | Route HTTP 200; browser console errors 0; `/api/v1/patients`, `/api/v1/appointments`, `/api/v1/orders`, and `/api/v1/results` HTTP 200 |
| C3-006 | `/imports` | Pass | Route HTTP 200; browser console errors 0 |
| C3-007 | `/request-logs` | Pass | Route HTTP 200; browser console errors 0; `/api/v1/request-logs` HTTP 200 |
| C3-008 | Navigation links | Pass | Link clicks navigated to all five routes with SPA URL updates and browser console errors 0 |

## Console Errors

Headless Edge browser verification completed across all five routes. Console errors: 0.

Route results:

```text
/providers: 200, consoleErrors=0
/scenarios: 200, consoleErrors=0
/data: 200, consoleErrors=0
/imports: 200, consoleErrors=0
/request-logs: 200, consoleErrors=0
```

Navigation links discovered and verified: Providers, Scenarios, Data, Imports, Request Logs.
