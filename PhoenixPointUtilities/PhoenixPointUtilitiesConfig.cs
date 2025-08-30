using PhoenixPoint.Modding;

namespace PhoenixPointUtilities
{
    public class PhoenixPointUtilitiesConfig : ModConfig
    {
        // Remote Control Feature
        [ConfigField(text: "Remote Control Buff", description: "Reduce Remote Control cost from 2 WP to 1 WP")]
        public bool RemoteControlBuff = true;

        // Recruit Inventory Features  
        [ConfigField(text: "Recruits Have Inventory Items", description: "Allow recruits to spawn with inventory items")]
        public bool RecruitGenerationHasInventoryItems = true;
        
        [ConfigField(text: "Recruits Have Consumable Items", description: "Allow recruits to spawn with consumable items")]
        public bool RecruitGenerationHasConsumableItems = true;
        
        [ConfigField(text: "Recruits Can Have Augmentations", description: "Allow recruits to spawn with augmentations")]
        public bool RecruitGenerationCanHaveAugmentations = false;

        // Return Fire Features
        [ConfigField(text: "Return Fire Angle", description: "Maximum angle in which return fire is possible (degrees, default 120)")]
        public int ReturnFireAngle = 120;
        
        [ConfigField(text: "Return Fire Limit", description: "Limit return fire to X times per round (0 = unlimited, default 1)")]
        public int ReturnFireLimit = 1;
        
        [ConfigField(text: "No Return Fire When Stepping Out", description: "Disable return fire when attacker steps out of cover")]
        public bool NoReturnFireWhenSteppingOut = false;
        
        [ConfigField(text: "Emphasize Return Fire Hint", description: "Enlarge and animate return fire indicators")]
        public bool EmphasizeReturnFireHint = true;

        // Item Recovery Feature
        [ConfigField(text: "Always Recover All Items", description: "Automatically recover all items from tactical missions")]
        public bool AlwaysRecoverAllItemsFromTacticalMissions = true;

        // Aircraft Configuration - Tiamat (Blimp)
        [ConfigField(text: "Tiamat Speed", description: "Aircraft speed for Tiamat")]
        public float AircraftBlimpSpeed = 500f;
        
        [ConfigField(text: "Tiamat Capacity", description: "Soldier capacity for Tiamat")]
        public int AircraftBlimpSpace = 8;
        
        [ConfigField(text: "Tiamat Range", description: "Maximum range for Tiamat")]
        public float AircraftBlimpRange = 3000f;

        // Aircraft Configuration - Thunderbird
        [ConfigField(text: "Thunderbird Speed", description: "Aircraft speed for Thunderbird")]
        public float AircraftThunderbirdSpeed = 700f;
        
        [ConfigField(text: "Thunderbird Capacity", description: "Soldier capacity for Thunderbird")]
        public int AircraftThunderbirdSpace = 6;
        
        [ConfigField(text: "Thunderbird Range", description: "Maximum range for Thunderbird")]
        public float AircraftThunderbirdRange = 2500f;

        // Aircraft Configuration - Manticore
        [ConfigField(text: "Manticore Speed", description: "Aircraft speed for Manticore")]
        public float AircraftManticoreSpeed = 600f;
        
        [ConfigField(text: "Manticore Capacity", description: "Soldier capacity for Manticore")]
        public int AircraftManticoreSpace = 8;
        
        [ConfigField(text: "Manticore Range", description: "Maximum range for Manticore")]
        public float AircraftManticoreRange = 4000f;

        // Aircraft Configuration - Helios
        [ConfigField(text: "Helios Speed", description: "Aircraft speed for Helios")]
        public float AircraftHeliosSpeed = 800f;
        
        [ConfigField(text: "Helios Capacity", description: "Soldier capacity for Helios")]
        public int AircraftHeliosSpace = 4;
        
        [ConfigField(text: "Helios Range", description: "Maximum range for Helios")]
        public float AircraftHeliosRange = 2000f;

        // Vehicle Space Configuration
        [ConfigField(text: "Armadillo Space", description: "Space Armadillo vehicles occupy in aircraft/squad (1-5)")]
        public int VehicleSpaceArmadillo = 3;
        
        [ConfigField(text: "Scarab Space", description: "Space Scarab vehicles occupy in aircraft/squad (1-5)")]
        public int VehicleSpaceScarab = 3;
        
        [ConfigField(text: "Aspida Space", description: "Space Aspida vehicles occupy in aircraft/squad (1-5)")]
        public int VehicleSpaceAspida = 3;
        
        [ConfigField(text: "Kaos Buggy Space", description: "Space Kaos Buggy occupies in aircraft/squad (1-5)")]
        public int VehicleSpaceKaos = 3;

        // Drop Chance Configuration
        [ConfigField(text: "Item Destruction Chance", description: "Base chance for items to be destroyed when dropped by a dying enemy (0-100%)")]
        public int ItemDestructionChance = 80;

        [ConfigField(text: "Allow Weapon Drops", description: "Allow weapons to drop from dying enemies")]
        public bool AllowWeaponDrops = true;

        [ConfigField(text: "Weapon Destruction Chance", description: "Chance for weapons to be destroyed when dropped (0-100%)")]
        public int WeaponDestructionChance = 60;
        
        [ConfigField(text: "Allow Armor Drops", description: "Allow armor to drop from dying enemies")]
        public bool AllowArmorDrops = true;

        [ConfigField(text: "Armor Destruction Chance", description: "Chance for armor to be destroyed when dropped (0-100%)")]
        public int ArmorDestructionChance = 70;
    }
}