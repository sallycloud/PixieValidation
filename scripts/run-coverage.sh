#!/usr/bin/env bash
cd "$(dirname "$0")/.."

rm -rf coverage
dotnet test --collect "XPlat Code Coverage" --results-directory ./coverage/raw
dotnet reportgenerator -reports:"coverage/raw/**/coverage.cobertura.xml" -targetdir:"coverage/report" -reporttypes:"Html;TextSummary" -filefilters:"-*.g.cs"
cat coverage/report/Summary.txt
