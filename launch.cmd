START cmd /k "cd src/CaseManagement.CMMN.Host && dotnet run"
START cmd /k "cd src/CaseManagement.Identity && dotnet run"
START cmd /k "cd src/CaseManagement.Website && npm run start"
echo Applications are running ...