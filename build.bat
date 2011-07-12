@echo off
if "%ProgramFiles(x86)%" == "" set ProgramFiles(x86)=%ProgramFiles%

set ReSharperPath="%ProgramFiles(x86)%\JetBrains\ReSharper\v6.0\Bin"
set Configuration=Debug

echo Building against ReSharper in %ReSharperPath%
call "%vs100comntools%\vsvars32.bat"
pushd lib\sl\StatLight\
powershell -NoProfile -Command "& { Import-Module .\psake.psm1; Invoke-psake .\default.ps1 "build-all" -parameters @{"build_configuration"='%Configuration%';} }"
popd
msbuild src\AgUnit.sln /p:Configuration=%Configuration%;ReferencePath=%ReSharperPath% /t:Rebuild

pause