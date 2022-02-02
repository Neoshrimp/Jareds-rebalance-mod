using HarmonyLib;
using System.Runtime.CompilerServices;
using PItem;
using System.Collections;
using UnityEngine;
using Enchent;

namespace Equipment_rebalance
{
    class GoldenEnchantsTweaks
    {

        [HarmonyPatch(typeof(CurseEn_Rapid), "Init")]
        class FleetnessPatch
        {
            static bool Prefix(CurseEn_Rapid __instance)
            {
                EquipmentRebalancePlugin.PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.spd = 1;
                __instance.PlusStat.dod = 10f;
                return false;
            }
        }

        [HarmonyPatch(typeof(CurseEn_NightMare), "Init")]
        class NightmarePatch
        {
            static bool Prefix(CurseEn_NightMare __instance)
            {
                EquipmentRebalancePlugin.PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.atk = 3f;
                __instance.PlusStat.cri = 3f;
                return false;
            }
        }

    }
}
