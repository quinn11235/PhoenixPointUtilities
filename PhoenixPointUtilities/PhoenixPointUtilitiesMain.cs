using Base.Defs;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Game;
using PhoenixPoint.Modding;
using HarmonyLib;
using System;

namespace PhoenixPointUtilities
{
    public class PhoenixPointUtilitiesMain : ModMain
    {
        internal static readonly DefRepository Repo = GameUtl.GameComponent<DefRepository>();
        internal static readonly SharedData Shared = GameUtl.GameComponent<SharedData>();
        public static PhoenixPointUtilitiesMain Main { get; private set; }

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
                
                OnConfigChanged();
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
                UtilityPatches_Simplified.ApplyConfigChanges(Config);
                Logger.LogInfo("Configuration changes applied.");
            }
            catch (Exception e)
            {
                Logger.LogError($"Error applying config changes: {e}");
            }
        }
    }
}