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
    public class CommonSkillPatches
    {

        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GDESkillData_Patch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // sprint
                if (__instance.Key == GDEItemKeys.Skill_S_Public_25)
                {
                    __instance.UseAp = 1;
                }
                // healing fountain
                else if (__instance.Key == GDEItemKeys.Skill_S_Public_23)
                {
                    __instance.UseAp = 0;
                }
                // medical kit
                else if (__instance.Key == GDEItemKeys.Skill_S_Public_22)
                {
                    __instance.NotCount = true;
                }
                // harden
                else if (__instance.Key == GDEItemKeys.Skill_S_Public_30)
                {
                    __instance.Disposable = true;
                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = ogDesc.Replace("10%", "25%");

                }
                // locked on
                else if (__instance.Key == GDEItemKeys.Skill_S_Public_31)
                {
                    __instance.NotCount = true;
                }
                // charged attack
                else if (__instance.Key == GDEItemKeys.Skill_S_Public_6)
                {
                    __instance.Track = true;
                }
                // double attack
                else if (__instance.Key == GDEItemKeys.Skill_S_Public_5 || __instance.Key == GDEItemKeys.Skill_S_Public_5_0)
                {
                    __instance.Fatal = true;
                }
                // reckless charge
                else if (__instance.Key == GDEItemKeys.Skill_S_Public_8)
                {
                    __instance.Track = true;
                }
                // greater heal
                else if (__instance.Key == GDEItemKeys.Skill_S_Public_16)
                {
                    __instance.NotCount = true;
                }
                // dead end
                else if (__instance.Key == GDEItemKeys.Skill_S_Public_10)
                {
                    __instance.Counting = 3;
                    __instance.UseAp = 3;

                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = ogDesc + CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Public_10, CustomLoc.TermType.ExtraDesc));

                    __instance.Disposable = true;
                }

            }
        }



        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GDESkillEffectData_Patch
        {
            static void Postfix(GDESkillEffectData __instance, Dictionary<string, object> dict)
            {
                // medical kit
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_T_22)
                {
                    __instance.HEAL_Per = 110;
                }
                // medical booklet
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_35_T)
                {
                    __instance.HEAL_Per = 105;
                }
                // fury attack
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_37_T)
                {
                    __instance.DMG_Per = 50;
                }
                // boomerang
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_36_T)
                {
                    __instance.DMG_Per = 100;
                }
                // harden
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_30_T)
                {

                    dict.TryGetIntList("BuffPlusTagPer", out List<int> ogBuffPlusTagPer);
                    ogBuffPlusTagPer.Add(100);
                    __instance.BuffPlusTagPer = ogBuffPlusTagPer;

                    dict.TryGetCustomList("Buffs", out List<GDEBuffData> ogBuffs);
                    ogBuffs.Add(new GDEBuffData(GDEItemKeys.Buff_B_MissChian_1_0_T));
                    __instance.Buffs = ogBuffs;
                }
                // breath of regen
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_29_T)
                {
                    __instance.HEAL_Per = 75;
                }
                // mask destroyer
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_32_T)
                {
                    __instance.DMG_Per = 105;
                    __instance.CRI = 15;
                }
                // surprise attack
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_T_0)
                {
                    __instance.DMG_Per = 75;
                    __instance.CRI = 100;
                }
                // strike
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_T_3)
                {
                    __instance.DMG_Per = 120;
                    __instance.HIT = 109;
                }
                // rush
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_T_2)
                {
                    __instance.DMG_Per = 90;
                }
                // extended heal
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_T_15)
                {
                    __instance.HEAL_Per = 65;
                }
                // heal
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_T_14)
                {
                    __instance.HEAL_Per = 110;
                }
                // flash heal
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_T_13)
                {
                    __instance.HEAL_Per = 95;
                    __instance.CRI = 10;
                }
                // dead end
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_T_10)
                {
                    __instance.DMG_Per = 105;
                    __instance.HIT = 93;
                }
                // quick attack
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_T_9)
                {
                    __instance.DMG_Per = 65;
                    __instance.HIT = 100;

                }
                // extended strike
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Public_T_11)
                {
                    __instance.DMG_Per = 95;
                }


            }
        }


        [HarmonyPatch(typeof(GDEBuffData), nameof(GDEBuffData.LoadFromDict))]
        class GDEBuffData_Patch
        {
            static void Postfix(GDEBuffData __instance, Dictionary<string, object> dict)
            {
                // seal
                if (__instance.Key == GDEItemKeys.Buff_B_Public_26_T)
                {
                    __instance.TagPer = 100;
                }
                // harden
                if (__instance.Key == GDEItemKeys.Buff_B_Public_23_T)
                {
                    __instance.LifeTime = 3;
                }
            }
        }



        [HarmonyPatch(typeof(GDESkillExtendedData), nameof(GDESkillExtendedData.LoadFromDict))]
        class GDESkillExtendedData_Patch
        {
            static void Postfix(GDESkillExtendedData __instance, Dictionary<string, object> dict)
            {
                // set custom skill extend for dead end
                if (__instance.Key == GDEItemKeys.SkillExtended_Public_10_Ex)
                {
                    __instance.ClassName = typeof(Extended_DeadEnd).AssemblyQualifiedName;
                }
            }
        }


        [HarmonyPatch]
        class HealingFountain_Patch
        {

            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(B_Public_23), nameof(B_Public_23.DescExtended));
                yield return AccessTools.Method(typeof(B_Public_23), nameof(B_Public_23.SkillUse));


            }


            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {

                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Ldc_R4, 55f))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_R4, 70f);
                    }
                    else if (ci.Is(OpCodes.Ldc_I4_S, 60))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_I4_S, 70);
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }
        }


        [HarmonyPatch]
        class Boomerang_Patch
        {

            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.GetDeclaredConstructors(typeof(Extended_Public_36))[0];
            }

            static void Postfix(ref int ___PlusAtk)
            {
                ___PlusAtk = 7;
            }
        }

        [HarmonyPatch]
        class Harden_Patch
        {

            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(Extended_Public_30), nameof(Extended_Public_30.DescExtended));
                yield return AccessTools.Method(typeof(Extended_Public_30), nameof(Extended_Public_30.SkillUseSingle));


            }


            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {

                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Ldc_I4_S, 10))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_I4_S, 4);
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
