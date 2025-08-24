#!/bin/bash

echo "Building Phoenix Point Utilities..."

# Try MSBuild first (Windows with Mono)
if command -v msbuild &> /dev/null; then
    echo "Using MSBuild..."
    msbuild PhoenixPointUtilities.sln /p:Configuration=Release /p:Platform="Any CPU"
elif command -v xbuild &> /dev/null; then
    echo "Using xbuild..."
    xbuild PhoenixPointUtilities.sln /p:Configuration=Release /p:Platform="Any CPU"
elif command -v dotnet &> /dev/null; then
    echo "Using dotnet..."
    cd PhoenixPointUtilities
    dotnet build --configuration Release
    cd ..
else
    echo "Error: No build tools found (msbuild, xbuild, or dotnet)"
    echo "Please install appropriate .NET build tools"
    exit 1
fi

if [ -f "Dist/PhoenixPointUtilities.dll" ]; then
    echo "Build successful! Output in Dist folder."
else
    echo "Build may have failed. Check for errors above."
fi