@echo off
rem
rem path to VCISdk
rem
call setpath.cmd

START "" /B "%VcInstallDir%\Common7\IDE\devenv.exe" .\vcinet.sln

