using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using MonoMod.Utils;

namespace BetterBarometer {
    public static class Patcher {
        /**
         * <summary>
         * The DLLs this patcher targets.
         * </summary>
         */
        public static IEnumerable<string> TargetDLLs { get; } = new string[] {
            "Assembly-CSharp.dll",
        };

        /**
         * <summary>
         * Patches the barometer's height.
         * </summary>
         * <param name="barometer">The barometer class</param>
         */
        private static void PatchBarometerHeight(TypeDefinition barometer) {
            // Display the height normally (kind of)
            MethodDefinition update = barometer.FindMethod("Update");
            Collection<Instruction> insts = update.Body.Instructions;

            string[][] seq = {
                new string[] { "ldarg.0" },
                new string[] { "ldarg.0" },
                new string[] { "ldfld", "System.Single Barometer::currentMetresUp" },
                new string[] { "ldc.r4", "3.12" },
                new string[] { "mul" },
                new string[] { "stfld", "System.Single Barometer::currentMetresUp" },
            };

            int index = Helper.FindSeq(insts, seq);

            // Bulwark
            insts[index + 3].Operand = 1f;

            // ST
            insts[index + 12].Operand = 1f;
        }

        /**
         * <summary>
         * Patches the barometer's air pressure.
         * </summary>
         * <param name="barometer">The barometer class</param>
         */
        private static void PatchBarometerPressure(TypeDefinition barometer) {
            // Display the air pressure normally (kind of)
            MethodDefinition start = barometer.FindMethod("Start");
            Collection<Instruction> insts = start.Body.Instructions;

            string[][] seq = {
                new string[] { "ldarg.0" },
                new string[] { "ldc.r4", "8200" },
                new string[] { "stfld", "System.Single Barometer::maxAirPressure" },
            };

            int index = Helper.FindSeq(insts, seq);

            // ST
            insts[index + 1].Operand = 5400f;

            // Bulwark
            insts[index + 7].Operand = 5000f;
        }

        /**
         * <summary>
         * Patches the game.
         * </summary>
         * <param name="assembly">The assembly to patch</param>
         */
        public static void Patch(AssemblyDefinition assembly) {
            ModuleDefinition main = assembly.MainModule;

            PatchBarometerHeight(main.GetType("Barometer"));
            PatchBarometerPressure(main.GetType("Barometer"));
        }
    }
}
