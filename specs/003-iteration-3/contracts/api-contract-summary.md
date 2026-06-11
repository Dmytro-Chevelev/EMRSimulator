# Admin UI Acceptance Contract: Iteration 3

## Purpose

Defines the observable behavior that MUST be true after Iteration 3 for the Admin UI to be
considered functional. These contracts drive the verification checklist.

---

## Contract 1: Build exits cleanly

**ID**: `C3-001`  
**Trigger**: `npm run build` from `src/EmrSimulator.AdminUi`  
**Acceptance**:
- Exit code is 0
- Build duration is under 3 minutes and is recorded as evidence
- No TypeScript errors in output
- No Angular template errors in output
- `dist/emr-simulator-admin-ui/` directory exists and contains `index.html`

---

## Contract 2: Dev server starts

**ID**: `C3-002`  
**Trigger**: `npm start` from `src/EmrSimulator.AdminUi`  
**Acceptance**:
- Process does not exit with non-zero code within 30 seconds
- Output contains `Application bundle generation complete`
- `http://localhost:4200` returns HTTP 200

---

## Contract 3: Providers page

**ID**: `C3-003`  
**URL**: `http://localhost:4200/providers`  
**Acceptance**:
- Page renders with provider selection UI
- Zero errors in browser console
- API call to `/api/v1/providers` returns 200

---

## Contract 4: Scenarios page

**ID**: `C3-004`  
**URL**: `http://localhost:4200/scenarios`  
**Acceptance**:
- Scenario list renders
- Zero errors in browser console
- API call to `/api/v1/scenarios` returns 200

---

## Contract 5: Data page

**ID**: `C3-005`  
**URL**: `http://localhost:4200/data`  
**Acceptance**:
- Patient/appointment/order/result sections render
- Zero errors in browser console
- API calls to `/api/v1/patients`, `/api/v1/appointments`, `/api/v1/orders`, `/api/v1/results` return 200

---

## Contract 6: Imports page

**ID**: `C3-006`  
**URL**: `http://localhost:4200/imports`  
**Acceptance**:
- Import form renders without errors
- Zero errors in browser console

---

## Contract 7: Request Logs page

**ID**: `C3-007`  
**URL**: `http://localhost:4200/request-logs`  
**Acceptance**:
- Log table renders
- Zero errors in browser console
- API call to `/api/v1/request-logs` returns 200

---

## Contract 8: Navigation between pages

**ID**: `C3-008`  
**Trigger**: Click each nav link from any page  
**Acceptance**:
- All five nav items are clickable and navigate correctly
- No full page reload on navigation (SPA routing preserved)
- URL updates to match the selected route
