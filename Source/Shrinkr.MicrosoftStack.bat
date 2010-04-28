@echo off

if "%1%" == "" goto debug
%windir%\Microsoft.NET\Framework64\v4.0.30128\MSBuild Shrinkr.build /p:Configuration=%1 /t:Full /m:2
goto end

:debug
%windir%\Microsoft.NET\Framework64\v4.0.30128\MSBuild Shrinkr.build /p:Configuration=debug /t:Full /m:2

:end
pause