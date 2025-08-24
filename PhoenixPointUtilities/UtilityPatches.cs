using Base.Defs;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Common.Game;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Geoscape.Entities.PhoenixBases.FacilityComponents;
using PhoenixPoint.Geoscape.Entities.Interception.Equipments;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Weapons;
using PhoenixPoint.Tactical.Levels;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using System;

namespace PhoenixPointUtilities
{
    internal static class UtilityPatches
    {
        private static readonly DefRepository Repo = PhoenixPointUtilitiesMain.Repo;
        private static readonly SharedData Shared = PhoenixPointUtilitiesMain.Shared;
        
        internal static Dictionary<TacticalActor, int> returnFireCounter = new Dictionary<TacticalActor, int>();
        internal static KeyValuePair<bool, string> stepOutTracker = new KeyValuePair<bool, string>(false, "");

        public static void ApplyConfigChanges(PhoenixPointUtilitiesConfig config)
        {
            ApplyRemoteControlBuff(config);
            ApplyRecruitInventoryChanges(config);
            ApplyItemRecoveryChanges(config);
            ApplyAircraftChanges(config);
            ApplyVehicleBayChanges(config);
        }

        private static void ApplyRemoteControlBuff(PhoenixPointUtilitiesConfig config)
        {
            if (config.RemoteControlBuff)
            {
                try
                {
                    ApplyStatusAbilityDef remoteControl = Repo.GetAllDefs<ApplyStatusAbilityDef>()
                        .FirstOrDefault(a => a.name.Equals("ManualControl_AbilityDef"));
                    if (remoteControl != null)
                    {
                        remoteControl.ActionPointCost = 0.25f; // 1 AP out of 4 total = 0.25
                        remoteControl.WillPointCost = 1;
                    }
                }
                catch (Exception e)
                {
                    PhoenixPointUtilitiesMain.Main?.Logger?.LogWarning($"Remote Control buff failed: {e.Message}");
                }
            }
        }

        private static void ApplyRecruitInventoryChanges(PhoenixPointUtilitiesConfig config)
        {
            try
            {
                var gdlDef = Repo.GetAllDefs<GeoscapeDataLayerDef>().FirstOrDefault();
                if (gdlDef?.RecruitsGenerationParams != null)
                {
                    gdlDef.RecruitsGenerationParams.HasConsumableItems = config.RecruitGenerationHasConsumableItems;
                    gdlDef.RecruitsGenerationParams.HasInventoryItems = config.RecruitGenerationHasInventoryItems;
                    gdlDef.RecruitsGenerationParams.CanHaveAugmentations = config.RecruitGenerationCanHaveAugmentations;
                }
            }
            catch (Exception e)
            {
                PhoenixPointUtilitiesMain.Main?.Logger?.LogWarning($"Recruit inventory changes failed: {e.Message}");
            }
        }

        private static void ApplyItemRecoveryChanges(PhoenixPointUtilitiesConfig config)
        {
            if (config.AlwaysRecoverAllItemsFromTacticalMissions)
            {
                try
                {
                    var tacticalMissionTypeDefs = Repo.GetAllDefs<TacticalMissionTypeDef>();
                    foreach (var def in tacticalMissionTypeDefs)
                    {
                        if (def.DontRecoverItems)
                        {
                            def.DontRecoverItems = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    PhoenixPointUtilitiesMain.Main?.Logger?.LogWarning($"Item recovery changes failed: {e.Message}");
                }
            }
        }

        private static void ApplyAircraftChanges(PhoenixPointUtilitiesConfig config)
        {
            try
            {
                var geoVehicleDefs = Repo.GetAllDefs<GeoVehicleDef>();
                foreach (var gvDef in geoVehicleDefs)
                {
                    if (gvDef.name.Contains("Blimp") || gvDef.name.Contains("Tiamat"))
                    {
                        gvDef.BaseStats.Speed.Value = config.AircraftBlimpSpeed;
                        gvDef.BaseStats.SpaceForUnits = config.AircraftBlimpSpace;
                        gvDef.BaseStats.MaximumRange.Value = config.AircraftBlimpRange;
                    }
                    else if (gvDef.name.Contains("Thunderbird"))
                    {
                        gvDef.BaseStats.Speed.Value = config.AircraftThunderbirdSpeed;
                        gvDef.BaseStats.SpaceForUnits = config.AircraftThunderbirdSpace;
                        gvDef.BaseStats.MaximumRange.Value = config.AircraftThunderbirdRange;
                    }
                    else if (gvDef.name.Contains("Manticore"))
                    {
                        gvDef.BaseStats.Speed.Value = config.AircraftManticoreSpeed;
                        gvDef.BaseStats.SpaceForUnits = config.AircraftManticoreSpace;
                        gvDef.BaseStats.MaximumRange.Value = config.AircraftManticoreRange;
                    }
                    else if (gvDef.name.Contains("Helios"))
                    {
                        gvDef.BaseStats.Speed.Value = config.AircraftHeliosSpeed;
                        gvDef.BaseStats.SpaceForUnits = config.AircraftHeliosSpace;
                        gvDef.BaseStats.MaximumRange.Value = config.AircraftHeliosRange;
                    }
                }
            }
            catch (Exception e)
            {
                PhoenixPointUtilitiesMain.Main?.Logger?.LogWarning($"Aircraft changes failed: {e.Message}");
            }
        }

        private static void ApplyVehicleBayChanges(PhoenixPointUtilitiesConfig config)
        {
            try
            {
                VehicleSlotFacilityComponentDef vehicleBaySlotComponent = Repo.GetAllDefs<VehicleSlotFacilityComponentDef>()
                    .FirstOrDefault(ged => ged.name.Equals("E_Element0 [VehicleBay_PhoenixFacilityDef]"));
                if (vehicleBaySlotComponent != null)
                {
                    vehicleBaySlotComponent.AircraftSlots = config.VehicleBayAircraftSlots;
                    vehicleBaySlotComponent.GroundVehicleSlots = config.VehicleBayGroundVehicleSlots;
                    vehicleBaySlotComponent.AircraftHealAmount = config.VehicleBayAircraftHealAmount;
                    vehicleBaySlotComponent.VehicleHealAmount = config.VehicleBayGroundVehicleHealAmount;
                }
            }
            catch (Exception e)
            {
                PhoenixPointUtilitiesMain.Main?.Logger?.LogWarning($"Vehicle bay changes failed: {e.Message}");
            }
        }
    }

    // Return Fire Patches - Based on TFTV approach
    [HarmonyPatch(typeof(TacticalLevelController), "GetReturnFireAbilities")]
    public static class TacticalLevelController_GetReturnFireAbilities_Patch
    {
        public static void Postfix(TacticalLevelController __instance, TacticalActor attacker, TacticalActor target, ref List<TacticalAbility> __result)
        {
            var config = PhoenixPointUtilitiesMain.Main?.Config;
            if (config == null || __result == null) return;

            try
            {
                // Apply return fire limitations
                var filteredAbilities = new List<TacticalAbility>();

                foreach (var ability in __result)
                {
                    var actor = ability.TacticalActor;
                    bool shouldReturnFire = true;

                    // Check return fire counter limit
                    if (config.ReturnFireLimit > 0)
                    {
                        if (!UtilityPatches.returnFireCounter.ContainsKey(actor))
                        {
                            UtilityPatches.returnFireCounter[actor] = 0;
                        }
                        
                        if (UtilityPatches.returnFireCounter[actor] >= config.ReturnFireLimit)
                        {
                            shouldReturnFire = false;
                        }
                    }

                    // Check angle limitation
                    if (shouldReturnFire && config.ReturnFireAngle < 360f)
                    {
                        Vector3 targetDirection = (attacker.transform.position - actor.transform.position).normalized;
                        Vector3 actorForward = actor.transform.forward;
                        float angle = Vector3.Angle(actorForward, targetDirection);
                        
                        if (angle > config.ReturnFireAngle / 2f)
                        {
                            shouldReturnFire = false;
                        }
                    }

                    if (shouldReturnFire)
                    {
                        filteredAbilities.Add(ability);
                        
                        // Increment counter
                        if (config.ReturnFireLimit > 0)
                        {
                            UtilityPatches.returnFireCounter[actor]++;
                        }
                    }
                }

                __result = filteredAbilities;
            }
            catch (Exception e)
            {
                PhoenixPointUtilitiesMain.Main?.Logger?.LogError($"Return fire patch error: {e}");
            }
        }
    }

    // Reset return fire counter between turns
    [HarmonyPatch(typeof(TacticalFaction), "PlayTurnCrt")]
    public static class TacticalFaction_PlayTurnCrt_Patch
    {
        public static void Prefix(TacticalFaction __instance)
        {
            // Keep counters only for actors not belonging to the faction starting its turn
            UtilityPatches.returnFireCounter = UtilityPatches.returnFireCounter
                .Where(actor => actor.Key.TacticalFaction != __instance)
                .ToDictionary(actor => actor.Key, actor => actor.Value);
        }
    }
}