using PhoenixPoint.Modding;

namespace PhoenixPointUtilities
{
    public class PhoenixPointUtilitiesConfig : ModConfig
    {
        [ConfigField(text: "Remote Control Buff",
        description: "Remote Control Costs 1 AP And 1 WP To Use (instead of 1 AP and 2 WP)")]
        public bool RemoteControlBuff = true;

        [ConfigField(text: "Recruit Generation Has Inventory Items",
        description: "Allows For Recruits To Come With Inventory Items")]
        public bool RecruitGenerationHasInventoryItems = true;

        [ConfigField(text: "Recruit Generation Has Consumable Items",
        description: "Allows For Recruits To Come With Consumable Items")]
        public bool RecruitGenerationHasConsumableItems = true;

        [ConfigField(text: "Recruit Generation Can Have Augmentations",
        description: "Allows For Recruits To Come With Augmentations")]
        public bool RecruitGenerationCanHaveAugmentations = false;

        [ConfigField(text: "Return Fire Angle",
        description: "Maximum angle in which return fire is possible at all. Vanilla didn't check at all, returned fire for 360 degrees.")]
        public float ReturnFireAngle = 120f;

        [ConfigField(text: "Return Fire Limit",
        description: "Limit the ability to return fire to X times per round, vanilla default is unlimited.")]
        public int ReturnFireLimit = 1;

        [ConfigField(text: "No Return Fire When Stepping Out Of Cover",
        description: "Disables return fire when the attacker side-steps from full cover.")]
        public bool NoReturnFireWhenSteppingOutOfCover = false;

        [ConfigField(text: "Emphasize Return Fire Hint",
        description: "Enlarges and animates the small return fire icon indicating the target's ability to return fire.")]
        public bool EmphasizeReturnFireHint = false;

        [ConfigField(text: "Always Recover All Items From Tactical Missions",
        description: "Recover All Dropped Items, Makes scavenging missions somewhat pointless")]		
        public bool AlwaysRecoverAllItemsFromTacticalMissions = false;

        [ConfigField(text: "Aircraft Tiamat Speed",
        description: "Speed of the Tiamat aircraft")]
        public float AircraftBlimpSpeed = 250f;

        [ConfigField(text: "Aircraft Thunderbird Speed",
        description: "Speed of the Thunderbird aircraft")]
        public float AircraftThunderbirdSpeed = 380f;

        [ConfigField(text: "Aircraft Manticore Speed",
        description: "Speed of the Manticore aircraft")]
        public float AircraftManticoreSpeed = 500f;

        [ConfigField(text: "Aircraft Helios Speed",
        description: "Speed of the Helios aircraft")]
        public float AircraftHeliosSpeed = 650f;

        [ConfigField(text: "Aircraft Tiamat Space",
        description: "Number of soldiers the Tiamat can carry")]
        public int AircraftBlimpSpace = 8;

        [ConfigField(text: "Aircraft Thunderbird Space",
        description: "Number of soldiers the Thunderbird can carry")]
        public int AircraftThunderbirdSpace = 7;

        [ConfigField(text: "Aircraft Manticore Space",
        description: "Number of soldiers the Manticore can carry")]
        public int AircraftManticoreSpace = 6;

        [ConfigField(text: "Aircraft Helios Space",
        description: "Number of soldiers the Helios can carry")]
        public int AircraftHeliosSpace = 5;

        [ConfigField(text: "Aircraft Tiamat Range",
        description: "Maximum range of the Tiamat aircraft")]
        public float AircraftBlimpRange = 4000f;

        [ConfigField(text: "Aircraft Thunderbird Range",
        description: "Maximum range of the Thunderbird aircraft")]
        public float AircraftThunderbirdRange = 3000f;

        [ConfigField(text: "Aircraft Manticore Range",
        description: "Maximum range of the Manticore aircraft")]
        public float AircraftManticoreRange = 2500f;

        [ConfigField(text: "Aircraft Helios Range",
        description: "Maximum range of the Helios aircraft")]
        public float AircraftHeliosRange = 3500f;

        [ConfigField(text: "Vehicle Bay Aircraft Slots",
        description: "Modify The Amount Of Aircraft Slots Vehicle Bay Can Hold")]
        public int VehicleBayAircraftSlots = 2;

        [ConfigField(text: "Vehicle Bay Ground Vehicle Slots",
        description: "Modify The Amount Of Ground Vehicle Slots Vehicle Bay Can Hold")]
        public int VehicleBayGroundVehicleSlots = 2;

        [ConfigField(text: "Vehicle Bay Aircraft Heal Amount",
        description: "Amount of HP aircraft heal per hour in Vehicle Bay")]
        public int VehicleBayAircraftHealAmount = 48;

        [ConfigField(text: "Vehicle Bay Ground Vehicle Heal Amount",
        description: "Amount of HP ground vehicles heal per hour in Vehicle Bay")]
        public int VehicleBayGroundVehicleHealAmount = 48;
    }
}