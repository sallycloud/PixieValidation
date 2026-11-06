Set-Location (Join-Path $PSScriptRoot '..')

Remove-Item -Recurse -Force coverage -ErrorAction SilentlyContinue
dotnet test --collect "XPlat Code Coverage" --results-directory ./coverage/raw
dotnet reportgenerator -reports:"coverage/raw/**/coverage.cobertura.xml" -targetdir:"coverage/report" -reporttypes:"Html;TextSummary" -filefilters:"-*.g.cs"
Get-Content coverage/report/Summary.txt
