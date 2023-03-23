using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace NeverFollowCard
{

    /// <summary>
    /// Handles the view movement when cards are created.
    /// For example, fiber created from snakegrass.
    /// </summary>
    [HarmonyPatch(typeof(GameManager), MethodType.Enumerator, new Type[] {
            typeof(CardData ),
            typeof(SlotInfo ),
            typeof(CardData ),
            typeof(InGameCardBase ),
            typeof(bool ),
            typeof(TransferedDurabilities ),
            typeof(List<CollectionDropsSaveData> ),
            typeof(List<StatTriggeredActionStatus> ),
            typeof(ExplorationSaveData ),
            typeof(BlueprintSaveData ),
            typeof(Vector3 ),
            typeof(bool ),
            typeof(SpawningLiquid ),
            typeof(bool ),
            typeof(bool ),
            typeof(Vector2Int ),
            typeof(CardData ),
            typeof(int ),
            typeof(int ),
            typeof(bool ),
            typeof(string ),
            })]
    [HarmonyPatch("AddCard")]
    [HarmonyPriority(500)]

    internal static class GameManager_AddCard_RemoveMove_Patch
    {

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {

            List<CodeInstruction> code = new(instructions);

            AccessTools.Method(typeof(GraphicsManager), nameof(GraphicsManager.MoveViewToSlot));
            List<CodeInstruction> newCode = new(
                new CodeMatcher(code)
                    .MatchForward(false,
                        new CodeMatch(OpCodes.Ldloc_1),
                        new CodeMatch(OpCodes.Ldfld),
                        new CodeMatch(OpCodes.Ldarg_0),
                        new CodeMatch(OpCodes.Ldfld),
                        new CodeMatch(OpCodes.Callvirt),
                        new CodeMatch(OpCodes.Ldc_I4_0),
                        new CodeMatch(OpCodes.Ldc_I4_0),
                        new CodeMatch(OpCodes.Callvirt,
                            AccessTools.Method(typeof(GraphicsManager), nameof(GraphicsManager.MoveViewToSlot)))
                    )
                    .ThrowIfNotMatch("Did not find the MoveViewToSlot block")
                    .RemoveInstructions(8)
                    .InstructionEnumeration()
            );

            //foreach (CodeInstruction instruction in newCode)
            //{
            //    Plugin.LogInfo(instruction.ToString());
            //}

            return newCode;

        }
    }
}
