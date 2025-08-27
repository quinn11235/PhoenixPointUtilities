# PhoenixPointUtilities

Phoenix Point utility mod with 5 working features: Remote Control buff, Recruit inventory, Item recovery, Aircraft config, Vehicle bay settings.

## Features

- **Remote Control Buff** - Reduces ability cost from 1 AP + 2 WP to 1 AP + 1 WP
- **Recruit Inventory** - Allows recruits to spawn with items, consumables, augmentations
- **Item Recovery** - Always recover all dropped items from tactical missions
- **Aircraft Configuration** - Speed, capacity, range settings for all aircraft (Tiamat, Thunderbird, Manticore, Helios)
- **Vehicle Bay Settings** - Configure space vehicles occupy (Armadillo, Scarab, Aspida, Kaos Buggy: 1-5 slots, default 3)

## Configuration

### Per feature toggles and settings
- Remote Control: AP/WP cost reduction
- Recruit Inventory: Item spawn rates, augmentation inclusion
- Item Recovery: Auto-recovery toggle
- Aircraft: Individual speed/capacity/range per aircraft type
- Vehicle Space: Space requirements (1-5 per vehicle type)

### Usage
1. Enable mod in Phoenix Point Mods menu
2. Configure features in mod settings
3. Some changes require game restart

## Technical Details

- Extracted from SuperCheatsModPlus for focused functionality
- Targeted changes to specific game systems for compatibility
- Uses Harmony patches for non-destructive modifications
- Safe to enable/disable mid-campaign for most features

## Installation

Extract to `Documents/My Games/Phoenix Point/Mods/` directory.

## Build

Requires ModSDK in parent directory. Output in `Dist/` folder.