#!/usr/bin/env pwsh
# verify-local-workflow.ps1
# Runs the canonical local verification sequence for the EMR Simulator project.
# Run this script from the repository root: C:\Projects\Midmark\src\EmrSimulator
#
# Usage:
#   ./scripts/verify-local-workflow.ps1
#   ./scripts/verify-local-workflow.ps1 -SkipUi

param(
    [switch]$SkipUi
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$RepoRoot = $PSScriptRoot | Split-Path -Parent
$AdminUiRoot = Join-Path $RepoRoot 'src\EmrSimulator.AdminUi'

function Write-Step([string]$msg) {
    Write-Host ""
    Write-Host "==> $msg" -ForegroundColor Cyan
}

function Write-Pass([string]$msg) {
    Write-Host "[PASS] $msg" -ForegroundColor Green
}

function Write-Fail([string]$msg) {
    Write-Host "[FAIL] $msg" -ForegroundColor Red
}

$results = [System.Collections.Generic.List[PSCustomObject]]::new()

# ── Step 1: .NET build ──────────────────────────────────────────────────────
Write-Step ".NET API Build"
Push-Location $RepoRoot
try {
    & dotnet build src/EmrSimulator.Api/EmrSimulator.Api.csproj --no-incremental -v minimal
    if ($LASTEXITCODE -eq 0) {
        Write-Pass "dotnet build"
        $results.Add([PSCustomObject]@{ Step = "dotnet build"; Status = "Pass"; Evidence = "Exit 0" })
    } else {
        Write-Fail "dotnet build"
        $results.Add([PSCustomObject]@{ Step = "dotnet build"; Status = "Fail"; Evidence = "Exit $LASTEXITCODE" })
    }
} finally {
    Pop-Location
}

# ── Step 2: .NET tests ──────────────────────────────────────────────────────
Write-Step ".NET Tests"
Push-Location $RepoRoot
try {
    & dotnet test -v q
    if ($LASTEXITCODE -eq 0) {
        Write-Pass "dotnet test"
        $results.Add([PSCustomObject]@{ Step = "dotnet test"; Status = "Pass"; Evidence = "Exit 0" })
    } else {
        Write-Fail "dotnet test"
        $results.Add([PSCustomObject]@{ Step = "dotnet test"; Status = "Fail"; Evidence = "Exit $LASTEXITCODE" })
    }
} finally {
    Pop-Location
}

if (-not $SkipUi) {
    # ── Step 3: Angular build ───────────────────────────────────────────────
    Write-Step "Angular Build (from $AdminUiRoot)"
    if (-not (Test-Path (Join-Path $AdminUiRoot 'angular.json'))) {
        $msg = "angular.json not found at $AdminUiRoot — run commands from the correct directory!"
        Write-Fail $msg
        $results.Add([PSCustomObject]@{ Step = "npm run build"; Status = "Fail"; Evidence = $msg })
    } elseif (-not (Test-Path (Join-Path $AdminUiRoot 'node_modules'))) {
        $msg = "node_modules not found — run 'npm install --legacy-peer-deps' first"
        Write-Fail $msg
        $results.Add([PSCustomObject]@{ Step = "npm run build"; Status = "Blocked"; Evidence = $msg })
    } else {
        Push-Location $AdminUiRoot
        try {
            & npm run build
            if ($LASTEXITCODE -eq 0) {
                Write-Pass "npm run build"
                $results.Add([PSCustomObject]@{ Step = "npm run build"; Status = "Pass"; Evidence = "Exit 0" })
            } else {
                Write-Fail "npm run build (exit $LASTEXITCODE)"
                $results.Add([PSCustomObject]@{ Step = "npm run build"; Status = "Fail"; Evidence = "Exit $LASTEXITCODE" })
            }
        } finally {
            Pop-Location
        }
    }
}

# ── Summary ─────────────────────────────────────────────────────────────────
Write-Host ""
Write-Host "Verification Summary" -ForegroundColor White
$results | Format-Table -AutoSize

$failures = $results | Where-Object { $_.Status -ne 'Pass' }
if ($failures) {
    Write-Host "Some steps need attention. See diagnostics-log.md for triage guidance." -ForegroundColor Yellow
    exit 1
} else {
    Write-Host "All steps passed." -ForegroundColor Green
    exit 0
}
