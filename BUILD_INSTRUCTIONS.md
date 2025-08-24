# Phoenix Point Utilities - Build Instructions

## Prerequisites
- Visual Studio 2019 or later (or MSBuild tools)
- .NET Framework 4.7.2 or later

## Building the Mod

### Option 1: Visual Studio
1. Open `PhoenixPointUtilities.sln` in Visual Studio
2. Build → Build Solution (or press Ctrl+Shift+B)
3. Built files will be in the `Dist` folder

### Option 2: MSBuild Command Line
```cmd
cd "D:\PP Modding\PhoenixPointUtilities"
msbuild PhoenixPointUtilities.sln /p:Configuration=Release
```

### Option 3: dotnet CLI (if supported)
```cmd
cd "D:\PP Modding\PhoenixPointUtilities\PhoenixPointUtilities"
dotnet build --configuration Release
```

## Expected Output Files
After building, the `Dist` folder should contain:
- `PhoenixPointUtilities.dll` - Main mod assembly
- `PhoenixPointUtilities.pdb` - Debug symbols
- `meta.json` - Mod metadata

## Installation
1. Copy the entire contents of the `Dist` folder to your Phoenix Point Mods directory
2. Enable the mod in-game through the Mods menu
3. Configure settings as desired

## Troubleshooting
- Ensure ModSDK folder contains all required Phoenix Point assemblies
- Check that .NET Framework 4.7.2 is installed
- Verify Phoenix Point installation path is correct

## Manual Compilation Alternative
If automated build fails, you can manually compile using:
1. Reference the assemblies in ModSDK folder
2. Compile all .cs files in the PhoenixPointUtilities folder
3. Target .NET Framework 4.7.2
4. Output to Dist folder