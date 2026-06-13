param(
[string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$ProjectPath = ".\Agent\Agent.csproj"
$PublishDir = ".\publish\Agent"

Write-Host ""
Write-Host "=== ArquivoLAN Agent Build & Publish ==="
Write-Host ""

if (-not (Test-Path $ProjectPath))
{
Write-Error "Project not found: $ProjectPath"
exit 1
}

Write-Host "Cleaning publish directory..."

if (Test-Path $PublishDir)
{
Remove-Item $PublishDir -Recurse -Force
}

Write-Host "Publishing ArquivoLAN Agent..."

dotnet publish `    $ProjectPath`
-c $Configuration `
-o $PublishDir

if ($LASTEXITCODE -ne 0)
{
Write-Error "Publish failed."
exit $LASTEXITCODE
}

Write-Host ""
Write-Host "Publish completed successfully."
Write-Host "Output: $PublishDir"
Write-Host ""
