@echo off
if not exist bin mkdir bin >NUL
copy ..\src\BuildCs.Core\bin\Debug\BuildCs.Core.dll bin\BuildCs.Core.dll >NUL
copy ..\src\BuildCs.Core\bin\Debug\ScriptCs.Contracts.dll bin\ScriptCs.Contracts.dll >NUL

scriptcs %1