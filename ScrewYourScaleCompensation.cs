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
        public override string Version => "1.0.1";

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
                __instance.RunInUpdates(3, () =>
                {
                    if (!config.GetValue(enabled)) return;

                    var valueUserOverride = __instance.Slot.AttachComponent<ValueUserOverride<float>>();
                    valueUserOverride.Target.Value = ____scaleCompensation.ReferenceID;
                    valueUserOverride.Default.Value = 1f;
                    valueUserOverride.Persistent = false;
                });
            }
        }

        // Notes
        // We are not liable for any problems that come from usage of this mod.
        // Fuck I need a drink
        // LeCloutPanda#9456 & NeroWolf#0001
    }
}