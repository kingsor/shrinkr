@echo off

if "%1%" == "" goto release
%windir%\Microsoft.NET\Framework64\v4.0.30128\MSBuild Shrinkr.build /p:Configuration=%1 /t:Full /m:2
goto end

:release
%windir%\Microsoft.NET\Framework64\v4.0.30128\MSBuild Shrinkr.build /p:Configuration=Release /t:Full /m:2

:end
pause