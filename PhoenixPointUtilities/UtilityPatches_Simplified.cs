using Base.Defs;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Geoscape.Entities.PhoenixBases.FacilityComponents;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Levels;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using System;

namespace PhoenixPointUtilities
{
    internal static class UtilityPatches_Simplified
    {
        private static readonly DefRepository Repo = GameUtl.GameComponent<DefRepository>();
        private static readonly SharedData Shared = GameUtl.GameComponent<SharedData>();
        
        internal static Dictionary<TacticalActor, int> returnFireCounter = new Dictionary<TacticalActor, int>();

        public static void ApplyConfigChanges(PhoenixPointUtilitiesConfig config)
        {
            ApplyRemoteControlBuff(config);
            // Note: Other features disabled due to compilation issues - need proper type references
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
                            if (!UtilityPatches_Simplified.returnFireCounter.ContainsKey(actor))
                            {
                                UtilityPatches_Simplified.returnFireCounter[actor] = 0;
                            }
                            
                            if (UtilityPatches_Simplified.returnFireCounter[actor] >= config.ReturnFireLimit)
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
                                UtilityPatches_Simplified.returnFireCounter[actor]++;
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
                UtilityPatches_Simplified.returnFireCounter = UtilityPatches_Simplified.returnFireCounter
                    .Where(actor => actor.Key.TacticalFaction != __instance)
                    .ToDictionary(actor => actor.Key, actor => actor.Value);
            }
        }
    }
}