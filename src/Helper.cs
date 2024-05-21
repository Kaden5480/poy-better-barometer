using System.Collections.Generic;
using System.Linq;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace BetterBarometer {
    public static class Helper {
        /**
         * <summary>
         * Compares two instructions for equivalence.
         * `b` must contain at least one element, the opcode.
         * An operand can also be provided as the second element.
         * If an operand is expected, but no specific value is required for
         * equivalence, `null` can be specified instead.
         * </summary>
         * <param name="a">The actual instruction</param>
         * <param name="b">The expected instruction</param>
         * <returns>Whether the instructions are equivalent</returns>
         */
        private static bool IsInst(Instruction a, string[] b) {
            // Ignore any weird cases
            if (a == null || b == null) {
                return false;
            }
            if (b.Length < 1) {
                return false;
            }
            if (a.OpCode == null || b[0] == null) {
                return false;
            }

            // Fail if opcodes differ
            if (a.OpCode.ToString().Equals(b[0]) == false) {
                return false;
            }

            // Check when no operands were expected
            if (b.Length < 2) {
                return a.Operand == null;
            }

            // Fail if an operand was expected but not found
            if (a.Operand == null) {
                return false;
            }

            // If the expected operand is null, any operand is valid
            if (b[1] == null) {
                return true;
            }

            // Otherwise, the operands must be equal
            return a.Operand.ToString().Equals(b[1]);
        }

        /**
         * <summary>
         * Checks if a sequence (seq) can be found starting from the given offset
         * into the instructions (insts).
         * </summary>
         * <param name="insts">The instructions to search in</param
         * <param name="seq">The sequence to find</param
         * <param name="offset">The offset into the instructions to check for the sequence</param
         * <returns>Whether the sequence could be found</returns>
         */
        private static bool IsSeq(Collection<Instruction> insts, string[][] seq, int offset) {
            for (int i = 0; i < seq.Length; i++) {
                // Check out of bounds
                if (offset + i >= insts.Count) {
                    return false;
                }

                if (IsInst(insts[offset + i], seq[i]) == false) {
                    return false;
                }
            }

            return true;
        }

        /**
         * <summary>
         * Finds all instances of the provided sequence (seq) with the instructions (insts).
         * </summary>
         * <param ="insts">The instructions to search in</param>
         * <param ="seq">The sequence to find</param>
         * <returns>Indices the sequence could be found at</returns>
         */
        public static IEnumerable<int> FindSeqs(Collection<Instruction> insts, string[][] seq) {
            if (insts.Count < 1 || seq.Length < 1) {
                yield break;
            }

            for (int i = 0; i < insts.Count; i++) {
                if (IsSeq(insts, seq, i) == false) {
                    continue;
                }

                yield return i;
                i += seq.Length - 1;
            }
        }

        /**
         * <summary>
         * Finds the first instance of the provided sequence (seq) with the instructions (insts).
         * </summary>
         * <param ="insts">The instructions to search in</param>
         * <param ="seq">The sequence to find</param>
         * <returns>Index the sequence was found at</returns>
         */
        public static int FindSeq(Collection<Instruction> insts, string[][] seq) {
            return FindSeqs(insts, seq).First();
        }
    }
}
