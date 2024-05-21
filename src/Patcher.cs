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
         * Patches the game.
         * </summary>
         * <param name="assembly">The assembly to patch</param>
         */
        public static void Patch(AssemblyDefinition assembly) {
            ModuleDefinition main = assembly.MainModule;
        }
    }
}
