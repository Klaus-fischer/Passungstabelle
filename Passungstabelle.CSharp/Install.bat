@setlocal enableextensions
@cd /d "%~dp0"
rem only for .net5.0 and higher
regsvr32 "Passungstabelle.CSharp.comhost.dll" 

set FMWK="v4.0.30319"

rem "%Windir%\Microsoft.NET\Framework64\%FMWK%\regasm" /unregister "Passungstabelle.CSharp.dll"
rem "%Windir%\Microsoft.NET\Framework64\%FMWK%\regasm" /codebase "Passungstabelle.CSharp.dll"

PAUSE
