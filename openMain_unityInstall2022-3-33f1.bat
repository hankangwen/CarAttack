@echo off
setlocal
cd /d %~dp0

start C:\UnityInstall\2022.3.33f1\Editor\Unity.exe -projectpath . -logfile Library\Editor.log

endlocal