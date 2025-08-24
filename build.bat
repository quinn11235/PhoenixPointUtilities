@echo off
echo Building Phoenix Point Utilities...

REM Try MSBuild first
where msbuild >nul 2>&1
if %ERRORLEVEL% == 0 (
    echo Using MSBuild...
    msbuild PhoenixPointUtilities.sln /p:Configuration=Release /p:Platform="Any CPU"
    goto :end
)

REM Try dotnet build
where dotnet >nul 2>&1
if %ERRORLEVEL% == 0 (
    echo Using dotnet...
    cd PhoenixPointUtilities
    dotnet build --configuration Release
    cd ..
    goto :end
)

echo Error: Neither MSBuild nor dotnet found in PATH
echo Please install Visual Studio Build Tools or .NET SDK
echo Or build manually in Visual Studio

:end
if exist "Dist\PhoenixPointUtilities.dll" (
    echo Build successful! Output in Dist folder.
) else (
    echo Build may have failed. Check for errors above.
)
pause