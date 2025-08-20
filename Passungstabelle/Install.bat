@setlocal enableextensions
@cd /d "%~dp0"
rem regsvr32 "Passungstabellen.comhost.dll" rem only for .net5.0 and higher

set FMWK="v4.0.30319"

"%Windir%\Microsoft.NET\Framework64\%FMWK%\regasm" /unregister "Passungstabellen.dll"
"%Windir%\Microsoft.NET\Framework64\%FMWK%\regasm" /codebase "Passungstabellen.dll"

PAUSE
