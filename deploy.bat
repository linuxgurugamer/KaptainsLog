
@echo off

set H=R:\KSP_1.3.1_dev
set GAMEDIR=KaptainsLog

echo %H%

copy /Y "%1%2" "GameData\%GAMEDIR%\Plugins"
rem copy /Y %GAMEDIR%.version GameData\%GAMEDIR%

mkdir "%H%\GameData\%GAMEDIR%"
xcopy  /E /y GameData\%GAMEDIR% "%H%\GameData\%GAMEDIR%"

xcopy  /E /y GameData\DMagicUtilities "%H%\GameData\DMagicUtilities"

