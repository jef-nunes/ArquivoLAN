param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$ProjectPath = ".\Agent.csproj"
$PublishDir = ".\publish"

Write-Host ""
Write-Host "=== ArquivoLAN Agent Build & Publish ==="
Write-Host ""

if (-not (Test-Path $ProjectPath))
{
    Write-Error "Project not found: $ProjectPath"
    exit 1
}

if (Test-Path $PublishDir)
{
    Remove-Item $PublishDir -Recurse -Force
}

dotnet publish $ProjectPath `
    --configuration $Configuration `
    --output $PublishDir

if ($LASTEXITCODE -ne 0)
{
    Write-Error "Publish failed."
    exit $LASTEXITCODE
}

Write-Host ""
Write-Host "Publish completed successfully."
Write-Host "Output: $PublishDir"