using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace NeverFollowCard
{

    /// <summary>
    /// Handles the view movement of cards that are moved, not created.  
    /// For example, "Take Out All Items" action from containers.
    /// </summary>
    [HarmonyPatch(typeof(GraphicsManager), nameof(GraphicsManager.MoveCardToSlot))]
    internal static class GraphicsManager_MoveCardToSlot_RemoveMove_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if(Plugin.MoveOnTakeOutAll)
            {
                return instructions;
            }

            List<CodeInstruction> newCode = new(
                new CodeMatcher(instructions)
                    .MatchForward(false,
                        new CodeMatch(OpCodes.Ldarg_0),
                        new CodeMatch(OpCodes.Ldarg_1),
                        new CodeMatch(OpCodes.Callvirt),
                        new CodeMatch(OpCodes.Ldc_I4_0),
                        new CodeMatch(OpCodes.Ldc_I4_0),
                        new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(GraphicsManager), nameof(GraphicsManager.MoveViewToSlot)))
                    )
                .ThrowIfNotMatch("Did not find the MoveViewToSlot block")
                .RemoveInstructions(6)
                .InstructionEnumeration());

            return newCode;
        }
    }
}
