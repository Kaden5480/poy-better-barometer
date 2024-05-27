using System.Collections.Generic;

using HarmonyLib;


#if BepInEx
using BepInEx;

namespace BetterBarometer {
    [BepInPlugin("com.github.Kaden5480.poy-better-barometer", "Better Barometer", PluginInfo.PLUGIN_VERSION)]
    public class Plugin: BaseUnityPlugin {
        /**
         * <summary>
         * Executes when the plugin is being loaded.
         * </summary>
         */
        public void Awake() {
            Harmony.CreateAndPatchAll(typeof(PatchBarometerHeight));
            Harmony.CreateAndPatchAll(typeof(PatchBarometerPressure));
        }


#elif MelonLoader
using MelonLoader;

[assembly: MelonInfo(typeof(BetterBarometer.Plugin, "Better Barometer", "0.1.0", "Kaden5480"))]
[assembly: MelonGame("TraipseWare", "Peaks of Yore")]

namespace BetterBarometer {
    public class Plugin: MelonMod {

#endif

    [HarmonyPatch(typeof(Barometer), "Update")]
    static class PatchBarometerHeight {
        static IEnumerable<CodeInstruction> Transpiler(
            IEnumerable<CodeInstruction> insts
        ) {
            foreach (CodeInstruction inst in insts) {
                if (inst.LoadsConstant(3.12f) || inst.LoadsConstant(3.125f)) {
                    inst.operand = 1f;
                }

                yield return inst;
            }
        }
    }

    [HarmonyPatch(typeof(Barometer), "Start")]
    static class PatchBarometerPressure {
        static IEnumerable<CodeInstruction> Transpiler(
            IEnumerable<CodeInstruction> insts
        ) {
            foreach (CodeInstruction inst in insts) {
                if (inst.LoadsConstant(8200f)) {
                    inst.operand = 5400f;
                }

                if (inst.LoadsConstant(6000f)) {
                    inst.operand = 5000f;
                }

                yield return inst;
            }
        }
    }
}
