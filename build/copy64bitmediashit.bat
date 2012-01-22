REM There's a fancy bug in Visual Studio 2010 that pertinently
REM refuses to compile the whole damn load to x86 if every sub
REM project is set to x86 (the main app will refuse to compile)
REM Microsoft provided some back-handed solutions (blah-blahblah)
REM See: http://blogs.msdn.com/b/visualstudio/archive/2010/06/19/resgen-exe-error-an-attempt-was-made-to-load-a-program-with-an-incorrect-format.aspx
REM SO:
REM Since we're always compiling "Any CPU", the architecture specific dll will be copied to "plugins".
REM We'll be handling this separately in the WIX project.

@echo off

echo Param 1 %1 >> .\output.log

if "%programfiles(x86)%XXX"=="XXX" goto 32BIT
echo 64-bit Windows installed >> .\output.log
copy "%1thirdparty\MediaInfo\x64\MediaInfo.dll" %LOCALAPPDATA%\convendro\plugins\MediaInfo.dll
goto END

:32BIT
echo 32-bit Windows installed >> .\output.log
copy "%1thirdparty\MediaInfo\x86\MediaInfo.dll" %LOCALAPPDATA%\convendro\plugins\MediaInfo.dll
:END