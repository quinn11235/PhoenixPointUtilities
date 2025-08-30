using HarmonyLib;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities.GameTags;
using PhoenixPoint.Common.Entities.GameTagsSharedData;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Equipments;
using PhoenixPoint.Tactical.Entities.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhoenixPointUtilities
{
    /// <summary>
    /// Harmony patches for configurable item drop chances from dying enemies
    /// </summary>
    
    // Patch the DieAbility.ShouldDestroyItem method to control item destruction rates
    [HarmonyPatch(typeof(DieAbility), "ShouldDestroyItem")]
    public static class DieAbility_ShouldDestroyItem_Patch
    {
        public static void Prefix(DieAbility __instance, TacticalItem item)
        {
            if (PhoenixPointUtilitiesMain.Main?.Config == null)
                return;

            var config = PhoenixPointUtilitiesMain.Main.Config;
            
            try
            {
                if (item.TacticalItemDef is WeaponDef wDef)
                {
                    // Handle weapon drops
                    if (!config.AllowWeaponDrops)
                    {
                        // If weapon drops are disabled, set destruction chance to 100%
                        item.TacticalItemDef.DestroyOnActorDeathPerc = 100;
                        return;
                    }
                    else
                    {
                        // Set custom weapon destruction chance
                        item.TacticalItemDef.DestroyOnActorDeathPerc = config.WeaponDestructionChance;
                    }
                }
                else
                {
                    // Handle other items (consumables, ammo, etc.)
                    item.TacticalItemDef.DestroyOnActorDeathPerc = config.ItemDestructionChance;
                }
            }
            catch (Exception e)
            {
                PhoenixPointUtilitiesMain.Main.Logger.LogWarning($"Error in item destruction patch: {e.Message}");
            }
        }
    }

    // Patch the DieAbility.DropItems method to add armor dropping functionality
    [HarmonyPatch(typeof(DieAbility), "DropItems")]
    public static class DieAbility_DropItems_Patch
    {
        public static bool Prepare()
        {
            // Only apply this patch if armor drops are enabled
            return PhoenixPointUtilitiesMain.Main?.Config?.AllowArmorDrops == true;
        }

        public static void Postfix(DieAbility __instance)
        {
            if (PhoenixPointUtilitiesMain.Main?.Config == null)
                return;

            var config = PhoenixPointUtilitiesMain.Main.Config;
            
            try
            {
                TacticalActor actor = __instance.TacticalActor;

                // Skip decoys and other special actors
                if (actor.DisplayName.Contains("decoy", StringComparison.OrdinalIgnoreCase) || 
                    __instance.AbilityDef.name.Contains("decoy", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                // Get armor items from the dying actor
                IEnumerable<TacticalItem> items = actor?.BodyState?.GetArmourItems();
                if (items?.Any() != true)
                {
                    return;
                }

                SharedData sharedData = SharedData.GetSharedDataFromGame();
                SharedGameTagsDataDef sharedGameTags = sharedData.SharedGameTags;
                GameTagDef armorTag = sharedGameTags.ArmorTag;
                GameTagDef manufacturableTag = sharedGameTags.ManufacturableTag;
                GameTagDef mountedTag = sharedGameTags.MountedTag;

                int droppedCount = 0;
                foreach (TacticalItem item in items)
                {
                    TacticalItemDef def = item.TacticalItemDef;
                    GameTagsList tags = def?.Tags;
                    
                    // Skip items that can't be manufactured or are permanent augments
                    if (tags == null || tags.Count == 0 || !tags.Contains(manufacturableTag) || def.IsPermanentAugment)
                    {
                        continue;
                    }
                    
                    // Check if this is armor or a mounted weapon
                    if (tags.Contains(armorTag) || tags.Contains(mountedTag))
                    {
                        // Roll for armor drop chance
                        int randomPercent = UnityEngine.Random.Range(0, 101);
                        bool willDrop = randomPercent > config.ArmorDestructionChance;
                        
                        if (willDrop)
                        {
                            item.Drop(sharedData.FallDownItemContainerDef, actor);
                            droppedCount++;
                        }
                    }
                }

                if (droppedCount > 0)
                {
                    PhoenixPointUtilitiesMain.Main.Logger.LogInfo($"Dropped {droppedCount} armor pieces from {actor.DisplayName}");
                }
            }
            catch (Exception e)
            {
                PhoenixPointUtilitiesMain.Main.Logger.LogWarning($"Error in armor drop patch: {e.Message}");
            }
        }
    }

    // Prevent duplicate armor recovery from squad member deaths when armor drops are enabled
    [HarmonyPatch(typeof(GeoMission), "GetDeadSquadMembersArmour")]
    public static class GeoMission_GetDeadSquadMembersArmour_Patch
    {
        public static bool Prepare()
        {
            // Only apply this patch if armor drops are enabled
            return PhoenixPointUtilitiesMain.Main?.Config?.AllowArmorDrops == true;
        }

        // Override the method to return empty collection (prevent duplicate armor recovery)
        public static bool Prefix(GeoMission __instance, ref IEnumerable<GeoItem> __result)
        {
            try
            {
                // Return empty collection to prevent automatic armor recovery
                // since we're handling it through the drop system
                __result = Enumerable.Empty<GeoItem>();
                return false; // Skip original method
            }
            catch (Exception e)
            {
                PhoenixPointUtilitiesMain.Main?.Logger?.LogWarning($"Error in squad member armor patch: {e.Message}");
                return true; // Fall back to original method
            }
        }
    }
}