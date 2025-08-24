using PhoenixPoint.Modding;
using HarmonyLib;
using System;

namespace PhoenixPointUtilities
{
    public class PhoenixPointUtilitiesMain_Minimal : ModMain
    {
        public new static PhoenixPointUtilitiesMain_Minimal Main;

        public new PhoenixPointUtilitiesConfig Config => (PhoenixPointUtilitiesConfig)base.Config;

        private Harmony _harmony;

        public override bool CanSafelyDisable => true;

        public override void OnModEnabled()
        {
            Main = this;
            Logger.LogInfo($"Phoenix Point Utilities v{MetaData.Version} enabled.");
            
            try
            {
                _harmony = new Harmony("com.phoenix.utilities");
                _harmony.PatchAll(GetType().Assembly);
                Logger.LogInfo("Harmony patches applied successfully.");
                
                Logger.LogInfo("Note: Full functionality requires manual testing in-game.");
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
                _harmony?.UnpatchAll("com.phoenix.utilities");
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
            Logger.LogInfo("Configuration changed - restart may be required for some changes.");
        }
    }
}