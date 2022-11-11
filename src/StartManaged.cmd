@echo off
rem
rem path to VCISdk
rem
call setpath.cmd

"%VcInstallDir%\Common7\IDE\devenv.exe" .\vcinet.sln

