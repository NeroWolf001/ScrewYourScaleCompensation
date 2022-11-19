using FrooxEngine;
using FrooxEngine.CommonAvatar;
using FrooxEngine.LogiX.Data;
using HarmonyLib;
using NeosModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewYourScaleCompensation
{
    public class Patch : NeosMod
    {
        public override string Name => "ScrewYourScaleCompensation";
        public override string Author => "NeroWolf & LeCloutPanda";
        public override string Version => "1.0.0";

        public override void OnEngineInit()
        {
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
                // Run this after 3 game ticks to ensure __instance isn't null
                __instance.RunInUpdates(3, () =>
                {
                    // Attach non persistent comps(They won't save with the avatar etc)
                    var copy = __instance.Slot.AttachComponent<ValueCopy<float>>(true);
                    var value = __instance.Slot.AttachComponent<ValueRegister<float>>(true);
                    // Set defaults
                    value.Value.Value = 1f;
                    copy.Persistent = false;
                    value.Persistent = false;
                    // Assign the fucking copy to ScaleCompensation value
                    copy.Source.Value = value.Value.ReferenceID;
                    copy.Target.Value = ____scaleCompensation.ReferenceID;
                });
            }
        }

        // Notes
        // We are not liable for any problems that come from usage of this mod.
        // Fuck I need a drink
        // LeCloutPanda#9456 & NeroWolf#0001
    }
}