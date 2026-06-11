$ErrorActionPreference = 'Stop'

$commandDirectory = if ([string]::IsNullOrWhiteSpace($env:INIT_CWD)) { (Get-Location).Path } else { $env:INIT_CWD }
$angularJsonPath = Join-Path -Path $commandDirectory -ChildPath 'angular.json'

if (-not (Test-Path -Path $angularJsonPath -PathType Leaf)) {
    Write-Error "Admin UI commands must be run from src/EmrSimulator.AdminUi, where angular.json is located. Current directory: $commandDirectory"
    exit 1
}
