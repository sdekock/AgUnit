@echo off
set Configuration=Release

call "%vs110comntools%\vsvars32.bat"

pushd lib\sl\StatLight\
powershell -NoProfile -Command "& { Import-Module .\psake.psm1; Invoke-psake .\default.ps1 "build-all" -framework 3.5x86 -parameters @{"build_configuration"='%Configuration%';} }"
popd

msbuild src\AgUnit.vs2011.sln /p:Configuration=%Configuration% /t:Rebuild

pause