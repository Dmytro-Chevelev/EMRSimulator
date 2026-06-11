#!/usr/bin/env pwsh
# collect-diagnostic.ps1
# Records a structured diagnostic entry per contracts/verification-contract.md
# Run from any directory.
#
# Usage:
#   ./scripts/collect-diagnostic.ps1 `
#     -Id "D006-example" `
#     -Command "npm run build" `
#     -Cwd "src/EmrSimulator.AdminUi" `
#     -ErrorSignature "Cannot find module 'xyz'" `
#     -LikelyCause "Package removed during iterative npm install" `
#     -NextAction "Run npm install --legacy-peer-deps from project root" `
#     [-Status Resolved]

param(
    [Parameter(Mandatory)] [string]$Id,
    [Parameter(Mandatory)] [string]$Command,
    [Parameter(Mandatory)] [string]$Cwd,
    [Parameter(Mandatory)] [string]$ErrorSignature,
    [Parameter(Mandatory)] [string]$LikelyCause,
    [Parameter(Mandatory)] [string]$NextAction,
    [ValidateSet('Open','Resolved')] [string]$Status = 'Open'
)

Set-StrictMode -Version Latest

$DiagnosticsFile = Join-Path $PSScriptRoot '..\specs\002-next-iteration\verification\diagnostics-log.md'

$timestamp = (Get-Date -Format 'yyyy-MM-dd HH:mm:ss')

$entry = @"

---

## $Id — (added $timestamp)

| Field | Value |
|-------|-------|
| ID | ``$Id`` |
| Command | $Command |
| CWD | $Cwd |
| Error Signature | $ErrorSignature |
| Likely Cause | $LikelyCause |
| Next Action | $NextAction |
| **Status** | **$Status** |
"@

Add-Content -Path $DiagnosticsFile -Value $entry
Write-Host "Diagnostic '$Id' appended to $DiagnosticsFile" -ForegroundColor Cyan
