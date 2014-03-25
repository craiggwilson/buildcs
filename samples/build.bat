@echo off
cls
if not exist bin mkdir bin >NUL
copy ..\src\ScriptCs.BuildCs\bin\Debug\BuildCs.Core.dll bin\BuildCs.Core.dll >NUL
copy ..\src\ScriptCs.BuildCs\bin\Debug\ScriptCs.Contracts.dll bin\ScriptCs.Contracts.dll >NUL
copy ..\src\ScriptCs.BuildCs\bin\Debug\ScriptCs.BuildCs.dll bin\ScriptCs.BuildCs.dll >NUL
copy ..\src\ScriptCs.BuildCs\bin\Debug\Ionic.Zip.dll bin\Ionic.Zip.dll >NUL

scriptcs "build.csx" -- %*