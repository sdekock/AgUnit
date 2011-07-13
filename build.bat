@echo off
set Configuration=Debug

call "%vs100comntools%\vsvars32.bat"

pushd lib\sl\StatLight\
powershell -NoProfile -Command "& { Import-Module .\psake.psm1; Invoke-psake .\default.ps1 "build-all" -parameters @{"build_configuration"='%Configuration%';} }"
popd

msbuild src\AgUnit.sln /p:Configuration=%Configuration% /t:Rebuild

pause