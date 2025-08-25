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

        // Vehicle Bay Configuration
        [ConfigField(text: "Aircraft Slots", description: "Number of aircraft slots in vehicle bay")]
        public int VehicleBayAircraftSlots = 2;
        
        [ConfigField(text: "Ground Vehicle Slots", description: "Number of ground vehicle slots in vehicle bay")]
        public int VehicleBayGroundVehicleSlots = 2;
        
        [ConfigField(text: "Aircraft Heal Amount", description: "Aircraft healing amount per hour")]
        public int VehicleBayAircraftHealAmount = 100;
        
        [ConfigField(text: "Ground Vehicle Heal Amount", description: "Ground vehicle healing amount per hour")]
        public int VehicleBayGroundVehicleHealAmount = 100;
    }
}