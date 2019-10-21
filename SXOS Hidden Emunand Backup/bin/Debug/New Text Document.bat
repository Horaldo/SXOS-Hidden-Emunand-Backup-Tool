@echo off
REM     This is the SECTOR INSPECTOR Batch File:
REM  This program was created by Daniel B. Sedory to
REM  run secinspect.exe without having to first open
REM  a Command Prompt window. This program will only
REM  save information about any drives connected to
REM  a Windows PC when run, to the file 'SIout.txt',
REM  then display the file in NOTEPAD.

if exist secinspect.exe goto okrun
echo.
echo.
echo  Did not find secinspect.exe in this folder, but
echo  we will also try looking for it in its standard
echo  Microsoft Windows location... 
echo.
echo  Press any key to continue searching for it. 
pause > nul

REM  Sector Inspector might also be located at:
REM  C:\Program Files (x86)\Windows Resource Kits\Tools\
REM  So let's look for it there too!

cd C:\Program Files (x86)\Windows Resource Kits\Tools\
if exist secinspect.exe goto okrun
echo.
echo.
echo  Did not find secinspect.exe in this folder:
echo  C:\Program Files\Windows Resource Kits\Tools\
echo  either.  You must place secinspect.exe in the
echo  same folder as SIrun.bat, or download it again
echo  from Microsoft and install it in its standard
echo  location in order for this batch file to work.
echo.
echo  Press any key to close this window. 
pause > nul
goto close

:okrun
echo -------------- >> SIout.txt
echo Date and time: >> SIout.txt
date /t >> SIout.txt
time /t >> SIout.txt
echo. >> SIout.txt

secinspect -backup PhysicalDrive6 "C:\Users\Harry\Desktop\New folder (5)\Test\RAWNAND.BIN" 16386 61071360 >> SIout.txt

echo. >> SIout.txt
echo. >> SIout.txt

start notepad SIout.txt

:close
exit
