@echo off
cls
if exist %~dp0\bin\. rmdir /S /Q %~dp0bin\.
"%ProgramFiles(x86)%\MSBuild\12.0\bin\msbuild.exe" /nologo /v:q /t:Rebuild /p:OutputPath="%~dp0\bin" /p:Configuration="Release" src\BuildCs.sln
cls
scriptcs "build.csx" -- %*