# Phoenix Point Utilities Mod

A collection of useful utilities and gameplay enhancements extracted from SuperCheatsModPlus.

## Features

### 1. Remote Control Buff
- Reduces Remote Control ability cost from 1 AP + 2 WP to 1 AP + 1 WP  
- Makes vehicle control more viable in tactical combat by reducing will point cost

### 2. Recruit Inventory Enhancement
- Allows recruits to spawn with inventory items
- Allows recruits to spawn with consumable items  
- Optional: Allows recruits to spawn with augmentations
- Configurable generation parameters

### 3. Return Fire Improvements
- **Angle Limitation**: Configure maximum angle for return fire (default 120°)
- **Per-Round Limit**: Limit return fire to X times per round (default 1)
- **Cover Step-Out**: Disable return fire when attacker steps out of cover
- **Visual Enhancement**: Emphasize return fire indicators in UI

### 4. Item Recovery
- **Always Recover All Items**: Automatically recover all dropped items from tactical missions
- Makes scavenging missions less critical but ensures no item loss

### 6. Aircraft Configuration
- **Speed Settings**: Configure speed for all aircraft types (Tiamat, Thunderbird, Manticore, Helios)
- **Capacity Settings**: Configure soldier capacity for each aircraft
- **Range Settings**: Configure maximum range for each aircraft
- Fully customizable aircraft performance

### 7. Vehicle Space Configuration
- **Armadillo Space**: Configure space Armadillo vehicles occupy in aircraft/squad (1-5, default 3)
- **Scarab Space**: Configure space Scarab vehicles occupy in aircraft/squad (1-5, default 3)
- **Aspida Space**: Configure space Aspida vehicles occupy in aircraft/squad (1-5, default 3)
- **Kaos Buggy Space**: Configure space Kaos Buggy occupies in aircraft/squad (1-5, default 3)
- Affects squad selection and aircraft capacity calculations

## Installation

1. Copy the mod folder to your Phoenix Point Mods directory
2. Enable the mod in-game through the Mods menu
3. Configure settings as desired
4. Restart game for some changes to take effect

## Configuration

All features are configurable through the in-game mod settings menu. Changes are applied immediately or on next game reload depending on the feature.

## Compatibility

This mod is designed to be compatible with other mods as it only makes targeted changes to specific game systems. Extract from SuperCheatsModPlus for better compatibility and focused functionality.

## Build Instructions

1. Ensure ModSDK is present in the parent directory
2. Use MSBuild or Visual Studio to build the project
3. Output will be generated in the Dist folder

## Credits

Functionality extracted and refined from SuperCheatsModPlus by the Phoenix Point modding community.