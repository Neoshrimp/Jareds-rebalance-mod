/*using BepInEx;
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
    class SizzPatches
    {


        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // thread of life
                if (__instance.Key == GDEItemKeys.Skill_S_Sizz_9)
                {
                    __instance.UseAp = 1;
                }
                // incise
                else if (__instance.Key == GDEItemKeys.Skill_S_Sizz_1)
                {

                }
                // eve help
                else if (__instance.Key == GDEItemKeys.Skill_S_Sizz_0)
                {
                    Debug.Log("RELOAD");
                    __instance.NotCount = true;

                    //__instance.SkillExtended = new List<string>() { nameof(Extended_Sizz_EveHelp) };

                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Sizz_0, CustomLoc.TermType.Description));


                }



            }
        }




        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GdeSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance)
            {
                // eve help
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_Sizz_0_T)
                {
                    __instance.HEAL_Per = 45;
                }

            }
        }

        *//*        [HarmonyPatch(typeof(Buff), "Init")]
                class dd
                {
                    static void Postfix(Buff __instance)
                    {
                        if (__instance is B_Sizz_0_T ca)
                        {
                            Debug.Log(ca.StackNum);
                        }
                    }
                }*//*

        //[HarmonyPatch(typeof(Buff), "Init")]
        class dd2
        {
            static void Postfix(Buff __instance)
            {
                if (__instance is B_Sizz_0_T ca)
                {
                    Debug.Log("deez");

                    Debug.Log(ca.StackNum);
                }

                if (__instance is B_Sizz_1_S ca2)
                {
                    Debug.Log("deez2");

                    Debug.Log(ca2.StackNum);
                }
            }
        }

        [HarmonyPatch(typeof(Extended_S_Sizz_0), nameof(Extended_S_Sizz_0.SkillUseSingle))]
        //[HarmonyDebug]
        class EveHelpPatch
        {
            // full change to il directly works regardless how DataToBuff is patched
            //[HarmonyTranspiler]
            static IEnumerable<CodeInstruction> FullCallTranspiler(IEnumerable<CodeInstruction> instructions)
            {

                //Debug.Log(Assembly.GetExecutingAssembly().FullName);

                yield return new CodeInstruction(OpCodes.Ldarg_2);
                yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(List<BattleChar>), "Item"));
                yield return new CodeInstruction(OpCodes.Ldstr, "B_Sizz_0_T");
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Passive_Char), "BChar"));
                yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                yield return new CodeInstruction(OpCodes.Ldc_I4_M1);
                yield return new CodeInstruction(OpCodes.Ldc_I4_0);

                yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(BattleChar), "BuffAdd")); // this calls DataToBuff eventually

                foreach (var ci in instructions)
                {
                    yield return ci;
                }
            }

            static void AddBuff(BattleChar target, BattleChar user)
            {
                target.BuffAdd(GDEItemKeys.Buff_B_Sizz_0_T, user, false, 0, false, -1, false);
                Debug.Log(Assembly.GetExecutingAssembly().FullName);
            }

            // calling auxiliary method from il produces a bug if identity transpiler is present
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> MethodCallTranspiler(IEnumerable<CodeInstruction> instructions)
            {

                //Debug.Log(Assembly.GetExecutingAssembly().FullName);
                yield return new CodeInstruction(OpCodes.Ldarg_2);
                yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(List<BattleChar>), "Item"));
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Passive_Char), "BChar"));

                yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(EveHelpPatch), "AddBuff")); 

                foreach (var ci in instructions)
                {
                    yield return ci;
                }
            }




            // this doesn't
            //[HarmonyPrefix]
            static bool MyPrefix(Extended_S_Sizz_0 __instance, List<BattleChar> Targets)
            {


                Targets[0].BuffAdd(GDEItemKeys.Buff_B_Sizz_0_T, __instance.BChar, false, 0, false, -1, false);

                Skill skill = Skill.TempSkill(GDEItemKeys.Skill_S_Sizz_0, __instance.BChar, __instance.BChar.MyTeam);
                skill.isExcept = true;
                skill.AP = 1;
                skill.NotCount = true;
                skill.AutoDelete = 1;
                BattleSystem.instance.AllyTeam.Add(skill, true);


                return false;
            }
        }

        // big if true. dataToBuff patch somehow disregards code in buff extended completely
        [HarmonyPatch(typeof(B_Sizz_0_T), nameof(B_Sizz_0_T.DescExtended))]
        class diedesc
        {
            static bool Prefix()
            {
                return false;
            }
        }


    }
}
*/