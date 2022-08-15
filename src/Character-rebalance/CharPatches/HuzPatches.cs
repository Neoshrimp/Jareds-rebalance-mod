using BepInEx;
using Character_rebalance.Extends;
using GameDataEditor;
using HarmonyLib;
using I2.Loc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Character_rebalance.CharPatches
{
    class HuzPatches
    {

        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GDESkillData_Patch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // give and take
                if (__instance.Key == GDEItemKeys.Skill_S_Queen_12)
                {
                    __instance.UseAp = 1;
                }
                // whip of healing
                else if (__instance.Key == GDEItemKeys.Skill_S_Queen_3)
                {
                    __instance.NotCount = false;
                }

            }
        }


        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GDESkillEffectData_Patch
        {
            static void Postfix(GDESkillEffectData __instance, Dictionary<string, object> dict)
            {
                // whip of healing
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_Queen_3_T)
                {
                    __instance.HEAL_Per = 60;
                }
            }
        }


    }
}
