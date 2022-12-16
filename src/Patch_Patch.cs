using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace NeverFollowCard
{
    [HarmonyPatch(typeof(GraphicsManager), nameof(GraphicsManager.MoveViewToSlot))]
    public static class Patch_Patch
    {
        public static void Prefix(ref bool __runOriginal)
        {
            __runOriginal = false;
        }

    }
}
