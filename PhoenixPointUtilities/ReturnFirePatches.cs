using HarmonyLib;
using PhoenixPoint.Tactical;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Weapons;
using PhoenixPoint.Tactical.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

namespace PhoenixPointUtilities
{
    internal static class ReturnFirePatches
    {
        internal static Dictionary<TacticalActor, int> returnFireCounter = new Dictionary<TacticalActor, int>();

        private static bool CanReturnFireFromAngle(TacticalActor shooter, TacticalActorBase target, float reactionAngleCos)
        {
            Vector3 forward = target.transform.forward;
            Vector3 toShooter = (shooter.Pos - target.Pos).normalized;
            float dot = Vector3.Dot(forward, toShooter);
            return dot >= reactionAngleCos;
        }

        private static bool HasReachedReturnFireLimit(TacticalActor target)
        {
            int returnFireLimit = PhoenixPointUtilitiesMain.Main.Config.ReturnFireLimit;
            if (returnFireLimit <= 0)
                return false;

            returnFireCounter.TryGetValue(target, out var currentCount);
            return currentCount >= returnFireLimit;
        }
    }

    [HarmonyPatch(typeof(TacticalFaction), "EndTurn")]
    public static class TacticalFaction_EndTurn_Patch
    {
        public static void Prefix(TacticalFaction __instance)
        {
            ReturnFirePatches.returnFireCounter = ReturnFirePatches.returnFireCounter
                .Where(tacticalActor => tacticalActor.Key.TacticalFaction != __instance)
                .ToDictionary(tacticalActor => tacticalActor.Key, tacticalActor => tacticalActor.Value);
        }
    }

    [HarmonyPatch(typeof(Weapon), "TryExecuteAbility")]
    public static class Weapon_TryExecuteAbility_Patch
    {
        public static void Postfix(TacticalAbilityTarget abilityTarget, TacticalActor ____tacticalActor)
        {
            if (abilityTarget.AttackType == AttackType.ReturnFire)
            {
                ReturnFirePatches.returnFireCounter.TryGetValue(____tacticalActor, out var currentCount);
                ReturnFirePatches.returnFireCounter[____tacticalActor] = currentCount + 1;
            }
        }
    }

    [HarmonyPatch(typeof(UIStateShoot), "CalculateReturnFirePredictions")]
    public static class UIStateShoot_CalculateReturnFirePredictions_Patch
    {
        public static bool Prefix()
        {
            return !PhoenixPointUtilitiesMain.Main.Config.NoReturnFireWhenSteppingOut;
        }
    }

    [HarmonyPatch(typeof(UIStateAbilitySelected), "CalculateReturnFirePredictions")]
    public static class UIStateAbilitySelected_CalculateReturnFirePredictions_Patch
    {
        public static bool Prefix()
        {
            return !PhoenixPointUtilitiesMain.Main.Config.NoReturnFireWhenSteppingOut;
        }
    }

    [HarmonyPatch(typeof(TacticalLevelController), "GetReturnFireAbilities")]
    public static class TacticalLevelController_GetReturnFireAbilities_Patch
    {
        public static void Postfix(TacticalLevelController __instance, ref List<ReturnFireAbility> __result, TacticalActor shooter, Weapon weapon, TacticalAbilityTarget target)
        {
            try
            {
                var config = PhoenixPointUtilitiesMain.Main.Config;
                float reactionAngleCos = (float)Math.Cos(config.ReturnFireAngle * Math.PI / 180d / 2d);

                if (__result == null || __result.Count == 0)
                    return;

                var weaponDef = weapon?.WeaponDef;
                if (target.AttackType == AttackType.ReturnFire || target.AttackType == AttackType.Overwatch || 
                    target.AttackType == AttackType.Synced || target.AttackType == AttackType.ZoneControl || 
                    (weaponDef != null && weaponDef.NoReturnFireFromTargets))
                    return;

                List<ReturnFireAbility> returnFireAbilities = __result;
                for (int i = returnFireAbilities.Count - 1; i >= 0; i--)
                {
                    TacticalActor tacticalActor = returnFireAbilities[i].TacticalActor;

                    if (config.NoReturnFireWhenSteppingOut)
                    {
                        returnFireAbilities.RemoveAt(i);
                        continue;
                    }

                    if (!ReturnFirePatches.CanReturnFireFromAngle(shooter, tacticalActor, reactionAngleCos))
                    {
                        returnFireAbilities.RemoveAt(i);
                        continue;
                    }

                    if (ReturnFirePatches.HasReachedReturnFireLimit(tacticalActor))
                    {
                        returnFireAbilities.RemoveAt(i);
                        continue;
                    }
                }
            }
            catch (Exception e)
            {
                PhoenixPointUtilitiesMain.Main.Logger.LogError($"Return fire patch error: {e}");
            }
        }
    }

    [HarmonyPatch(typeof(SpottedTargetsElement), "ShowReturnFireIcon")]
    public static class SpottedTargetsElement_ShowReturnFireIcon_Patch
    {
        public static bool Prefix(SpottedTargetsElement __instance)
        {
            if (!PhoenixPointUtilitiesMain.Main.Config.EmphasizeReturnFireHint)
                return true;

            try
            {
                __instance.ReturnFire.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
                __instance.ReturnFire.transform.Translate(new Vector3(-2f, 3f, 1f));
                __instance.StartCoroutine(Pulse(__instance.ReturnFire, Color.white, Color.red));
            }
            catch (Exception e)
            {
                PhoenixPointUtilitiesMain.Main.Logger.LogError($"Return fire UI enhancement error: {e}");
            }

            return true;
        }

        private static IEnumerator Pulse(GameObject target, Color color1, Color color2)
        {
            var image = target.GetComponent<UnityEngine.UI.Image>();
            if (image == null) yield break;

            float duration = 1.0f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = Mathf.PingPong(elapsed * 2f, 1f);
                image.color = Color.Lerp(color1, color2, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            image.color = color1;
        }
    }
}