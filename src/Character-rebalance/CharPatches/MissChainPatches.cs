using Character_rebalance.Extends;
using GameDataEditor;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;

namespace Character_rebalance.CharPatches
{
    class MissChainPatches
    {

        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // zoom
                if (__instance.Key == GDEItemKeys.Skill_S_MissChain_0)
                {
                    __instance.SkillExtended = new List<string> { typeof(Extended_Chain_Zoom).AssemblyQualifiedName };
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_MissChain_0, CustomLoc.TermType.Description));
                }
                // bazzuum
                else if (__instance.Key == GDEItemKeys.Skill_S_MissChain_2)
                {
                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = ogDesc.Replace("33%", "50%");

                }
                // engine burner
                else if (__instance.Key == GDEItemKeys.Skill_S_MissChain_8)
                {
                    __instance.IgnoreTaunt = false;
                }
                // combined arms
                else if (__instance.Key == GDEItemKeys.Skill_S_MissChain_9)
                {
                    __instance.NotCount = false;
                }
                // dismantle armor
                else if (__instance.Key == GDEItemKeys.Skill_S_MissChain_13)
                {
                    __instance.Track = true;
                }
                // leave this to me
                else if (__instance.Key == GDEItemKeys.Skill_S_MissChain_7)
                {
                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = ogDesc.Replace("3", "2");
                }

            }
        }




        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GdeSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance, Dictionary<string, object> dict)
            {
                // zoom
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_MissChain_T_0)
                {
                    __instance.DMG_Per = 115;
                }
                // shred
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_MissChain_11_T)
                {
                    __instance.DMG_Per = 85;
                }
                // playing with fire
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_MissChain_T_4)
                {
                    __instance.DMG_Per = 55;
                    __instance.HIT = 95;
                }
                // leave this to me
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_MissChain_T_7)
                {
                    __instance.DMG_Per = 80;

                    dict.TryGetCustomList("Buffs", out List<GDEBuffData> ogBuffsList);
                    ogBuffsList.RemoveAt(0);
                    __instance.Buffs = ogBuffsList;

                    dict.TryGetIntList("BuffPlusTagPer", out List<int> ogBuffPlusTagPer);
                    ogBuffPlusTagPer.RemoveAt(0);
                    __instance.BuffPlusTagPer = ogBuffPlusTagPer;
                }
                // combined arms
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_MissChain_9_T)
                {
                    __instance.DMG_Per = 125;
                }
            }
        }

        [HarmonyPatch(typeof(Extended_MissChain_2), nameof(Extended_MissChain_2.FixedUpdate))]
        class BazzuumPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Ldc_R4, 33))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_R4, 50f);
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }
        }

    }
}
