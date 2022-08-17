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
    public class LucyCardsPatches
    {


        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GDESkillData_Patch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // trick show
                if (__instance.Key == GDEItemKeys.Skill_S_Lucy_18)
                {
                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = ogDesc + CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Lucy_18, CustomLoc.TermType.ExtraDesc));
                }
                // dark silhouette 
                else if (__instance.Key == GDEItemKeys.Skill_S_Lucy_17)
                {
                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = ogDesc.Replace(" 1 ", " 2 ");

                }
                // acceleration circle
                else if (__instance.Key == GDEItemKeys.Skill_S_Lucy_15)
                {
                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = ogDesc + CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Lucy_15, CustomLoc.TermType.ExtraDesc));

                    dict.TryGetStringList("SkillExtended", out List<string> ogSkillExtended);
                    ogSkillExtended.Add(typeof(SkillEn_BattleStartDraw).AssemblyQualifiedName);
                    __instance.SkillExtended = ogSkillExtended;
                }
                // search more
                else if (__instance.Key == GDEItemKeys.Skill_S_LucyD_9)
                {
                    __instance.UseAp = 1;
                }
                // flag of combat
                else if (__instance.Key == GDEItemKeys.Skill_S_Lucy_14)
                {
                    __instance.UseAp = 2;
                }
                // teamwork
                else if (__instance.Key == GDEItemKeys.Skill_S_Lucy_7)
                {
                    __instance.UseAp = 2;
                }
                // auto focus
                else if (__instance.Key == GDEItemKeys.Skill_S_Lucy_9)
                {
                    __instance.UseAp = 1;
                }
                // money power
                else if (__instance.Key == GDEItemKeys.Skill_S_Lucy_20)
                {
                    __instance.UseAp = 1;
                }
                // potential
                else if (__instance.Key == GDEItemKeys.Skill_S_Lucy_1)
                {
                    __instance.UseAp = 1;
                }
                // possibilities
                else if (__instance.Key == GDEItemKeys.Skill_S_Lucy_8)
                {
                    __instance.UseAp = 0;
                }
                // accelerate
                else if (__instance.Key == GDEItemKeys.Skill_S_Lucy_0)
                {
                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = ogDesc.Replace(" 1.", " 2.");

                }
                // apply magic
                if (__instance.Key == GDEItemKeys.Skill_S_Lucy_21)
                {
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Lucy_21, CustomLoc.TermType.Description));
                }
                // suggestion
                if (__instance.Key == GDEItemKeys.Skill_S_Lucy_23)
                {
                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = ogDesc.Replace(" 0 ", " 3 less ");
                }

            }
        }


        [HarmonyPatch(typeof(S_Lucy_18), nameof(S_Lucy_18.Del2))]
        class TrickShow_Patch
        {
            static bool Prefix(SkillButton Mybutton)
            {
                Skill skill = Mybutton.Myskill.CloneSkill(true, Mybutton.Myskill.Master, null, true);
                Skill skill2 = Mybutton.Myskill.CloneSkill(true, Mybutton.Myskill.Master, null, true);
                Mybutton.Myskill.Master.MyTeam.Add(skill, true);
                Mybutton.Myskill.Master.MyTeam.Add(skill2, true);
                return false;
            }

        }


        [HarmonyPatch]
        class DarkSilhouette_Patch
        {

            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(S_Lucy_17), nameof(S_Lucy_17.DescExtended));
                yield return AccessTools.Method(typeof(B_Lucy_17_T), nameof(B_Lucy_17_T.DescExtended));
                yield return AccessTools.Method(typeof(B_Lucy_17_T).
                    GetNestedTypes(BindingFlags.NonPublic).ToList().Find(t => t.Name == "<Attack>c__Iterator0"),
                    "MoveNext");

            }


            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {

                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Ldfld, AccessTools.Field(typeof(TempSaveData), nameof(TempSaveData.StageNum))))
                    {
                        yield return ci;
                        yield return new CodeInstruction(OpCodes.Ldc_I4_2);
                        yield return new CodeInstruction(OpCodes.Mul);

                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }

        }


        [HarmonyPatch(typeof(Extended_Lucy_0_1), nameof(Extended_Lucy_0_1.Init))]
        class Accelerate_Patch
        {

            static void Postfix(Extended_Lucy_0_1 __instance)
            {
                __instance.APChange = -2;
            }
        }



        [HarmonyPatch(typeof(S_Lucy_21), nameof(S_Lucy_21.SkillTargetSelectExcept))]
        class ApplyMagic_Patch1
        {
            static bool Prefix(Skill ExceptSkill, ref bool __result)
            {
                __result =  ExceptSkill.Enforce;
                return false;
            }
        }

        [HarmonyPatch(typeof(S_Lucy_21), nameof(S_Lucy_21.SkillTargetSingle))]
        class ApplyMagic_Patch2
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {

                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Ldc_I4_2)
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_I4_3);

                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }
        }


        [HarmonyPatch(typeof(S_Lucy_23_0), nameof(S_Lucy_23_0.FixedUpdate))]
        class Suggestion_Patch
        {
            static bool Prefix(S_Lucy_23_0 __instance)
            {
                __instance.APChange = -3;
                return false;
            }

        }





    }
}
