using FrooxEngine;
using FrooxEngine.CommonAvatar;
using HarmonyLib;
using NeosModLoader;

namespace ScrewYourScaleCompensation
{
    public class ScrewYourScaleCompensation : NeosMod
    {
        public override string Name => "ScrewYourScaleCompensation";
        public override string Author => "NeroWolf & LeCloutPanda";
        public override string Version => "1.0.2";

        public static ModConfiguration config;

        [AutoRegisterConfigKey]
        public static ModConfigurationKey<bool> enabled = new ModConfigurationKey<bool>("Enabled", "", () => true);

        public override void OnEngineInit()
        {
            config = GetConfiguration();
            config.Save();

            Harmony harmony = new Harmony($"dev.{Author}.{Name}");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(AvatarAudioOutputManager))]
        static class PatchAvatarAudioOutputManager
        {
            [HarmonyPatch("OnAwake")]
            [HarmonyPostfix]
            static void FixScaleCompensation(AvatarAudioOutputManager __instance, Sync<float> ____scaleCompensation)
            {
                __instance.RunInUpdates(0, () =>
                {
                    if (!config.GetValue(enabled)) return;
                    ____scaleCompensation.OverrideForUser(__instance.LocalUser, 1f).Persistent = false;
                });
            }
        }

        // Notes
        // We are not liable for any problems that come from usage of this mod.
        // LeCloutPanda#9456 & NeroWolf#0001
    }
}
