using Base.Core;
using Base.Defs;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Game;
using PhoenixPoint.Common.Levels.Missions;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Entities.DifficultySystem;
using PhoenixPoint.Geoscape.Entities.PhoenixBases.FacilityComponents;
using PhoenixPoint.Modding;
using PhoenixPoint.Tactical;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Equipments;
using PhoenixPoint.Tactical.Entities.Weapons;
using HarmonyLib;
using System;
using System.Linq;

namespace PhoenixPointUtilities
{
    public class PhoenixPointUtilitiesMain_Working : ModMain
    {
        internal static readonly DefRepository Repo = GameUtl.GameComponent<DefRepository>();
        internal static readonly SharedData Shared = GameUtl.GameComponent<SharedData>();
        public static PhoenixPointUtilitiesMain_Working Main { get; private set; }

        public new PhoenixPointUtilitiesConfig Config => (PhoenixPointUtilitiesConfig)base.Config;

        public override bool CanSafelyDisable => true;

        public override void OnModEnabled()
        {
            Main = this;
            Logger.LogInfo($"Phoenix Point Utilities v{MetaData.Version} enabled.");
            
            try
            {
                HarmonyLib.Harmony harmony = (HarmonyLib.Harmony)HarmonyInstance;
                harmony.PatchAll(GetType().Assembly);
                Logger.LogInfo("Harmony patches applied successfully.");
                
                ApplyRemoteControlBuff();
                ApplyRecruitInventorySettings();
                ApplyItemRecoverySettings();
                ApplyAircraftSettings();
                ApplyVehicleBaySettings();
            }
            catch (Exception e)
            {
                Logger.LogError($"Error enabling Phoenix Point Utilities: {e}");
            }
        }

        public override void OnModDisabled()
        {
            try
            {
                HarmonyLib.Harmony harmony = (HarmonyLib.Harmony)HarmonyInstance;
                harmony.UnpatchAll(harmony.Id);
                Logger.LogInfo("Phoenix Point Utilities disabled.");
            }
            catch (Exception e)
            {
                Logger.LogError($"Error disabling Phoenix Point Utilities: {e}");
            }
            
            Main = null;
        }

        public override void OnConfigChanged()
        {
            try
            {
                ApplyRemoteControlBuff();
                ApplyRecruitInventorySettings();
                ApplyItemRecoverySettings(); 
                ApplyAircraftSettings();
                ApplyVehicleBaySettings();
                Logger.LogInfo("Configuration changes applied.");
            }
            catch (Exception e)
            {
                Logger.LogError($"Error applying config changes: {e}");
            }
        }

        private void ApplyRemoteControlBuff()
        {
            if (Config.RemoteControlBuff)
            {
                try
                {
                    ApplyStatusAbilityDef remoteControl = Repo.GetAllDefs<ApplyStatusAbilityDef>()
                        .FirstOrDefault(a => a.name.Equals("ManualControl_AbilityDef"));
                    if (remoteControl != null)
                    {
                        remoteControl.ActionPointCost = 0.25f; // 1 AP out of 4 total = 0.25
                        remoteControl.WillPointCost = 1;
                        Logger.LogInfo("Applied Remote Control buff: 1 AP + 1 WP");
                    }
                }
                catch (Exception e)
                {
                    Logger.LogWarning($"Remote Control buff failed: {e.Message}");
                }
            }
        }

        private void ApplyRecruitInventorySettings()
        {
            try
            {
                var gameDifficultyLevelDefs = Repo.GetAllDefs<GameDifficultyLevelDef>();
                foreach (GameDifficultyLevelDef gdlDef in gameDifficultyLevelDefs)
                {
                    gdlDef.RecruitsGenerationParams.HasConsumableItems = Config.RecruitGenerationHasConsumableItems;
                    gdlDef.RecruitsGenerationParams.HasInventoryItems = Config.RecruitGenerationHasInventoryItems;
                    gdlDef.RecruitsGenerationParams.CanHaveAugmentations = Config.RecruitGenerationCanHaveAugmentations;
                }
                Logger.LogInfo("Applied recruit inventory settings");
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Recruit inventory settings failed: {e.Message}");
            }
        }

        private void ApplyItemRecoverySettings()
        {
            if (Config.AlwaysRecoverAllItemsFromTacticalMissions)
            {
                try
                {
                    var defs = Repo.DefRepositoryDef.AllDefs.OfType<TacMissionTypeDef>().ToList();
                    foreach (TacMissionTypeDef def in defs)
                    {
                        if (def.DontRecoverItems)
                        {
                            def.DontRecoverItems = false;
                        }
                    }
                    Logger.LogInfo("Applied always recover all items setting");
                }
                catch (Exception e)
                {
                    Logger.LogWarning($"Item recovery settings failed: {e.Message}");
                }
            }
        }

        private void ApplyAircraftSettings()
        {
            try
            {
                var geoVehicleDefs = Repo.GetAllDefs<GeoVehicleDef>();
                foreach (var gvDef in geoVehicleDefs)
                {
                    if (gvDef.name.Contains("Blimp"))
                    {
                        gvDef.BaseStats.Speed.Value = Config.AircraftBlimpSpeed;
                        gvDef.BaseStats.SpaceForUnits = Config.AircraftBlimpSpace;
                        gvDef.BaseStats.MaximumRange.Value = Config.AircraftBlimpRange;
                    }
                    else if (gvDef.name.Contains("Thunderbird"))
                    {
                        gvDef.BaseStats.Speed.Value = Config.AircraftThunderbirdSpeed;
                        gvDef.BaseStats.SpaceForUnits = Config.AircraftThunderbirdSpace;
                        gvDef.BaseStats.MaximumRange.Value = Config.AircraftThunderbirdRange;
                    }
                    else if (gvDef.name.Contains("Manticore"))
                    {
                        gvDef.BaseStats.Speed.Value = Config.AircraftManticoreSpeed;
                        gvDef.BaseStats.SpaceForUnits = Config.AircraftManticoreSpace;
                        gvDef.BaseStats.MaximumRange.Value = Config.AircraftManticoreRange;
                    }
                    else if (gvDef.name.Contains("Helios"))
                    {
                        gvDef.BaseStats.Speed.Value = Config.AircraftHeliosSpeed;
                        gvDef.BaseStats.SpaceForUnits = Config.AircraftHeliosSpace;
                        gvDef.BaseStats.MaximumRange.Value = Config.AircraftHeliosRange;
                    }
                }
                Logger.LogInfo("Applied aircraft configuration settings");
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Aircraft settings failed: {e.Message}");
            }
        }

        private void ApplyVehicleBaySettings()
        {
            try
            {
                VehicleSlotFacilityComponentDef vehicleBaySlotComponent = Repo.GetAllDefs<VehicleSlotFacilityComponentDef>()
                    .FirstOrDefault(ged => ged.name.Equals("E_Element0 [VehicleBay_PhoenixFacilityDef]"));
                if (vehicleBaySlotComponent != null)
                {
                    vehicleBaySlotComponent.AircraftSlots = Config.VehicleBayAircraftSlots;
                    vehicleBaySlotComponent.GroundVehicleSlots = Config.VehicleBayGroundVehicleSlots;
                    vehicleBaySlotComponent.AircraftHealAmount = Config.VehicleBayAircraftHealAmount;
                    vehicleBaySlotComponent.VehicleHealAmount = Config.VehicleBayGroundVehicleHealAmount;
                    Logger.LogInfo("Applied vehicle bay configuration settings");
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Vehicle bay settings failed: {e.Message}");
            }
        }
    }
}