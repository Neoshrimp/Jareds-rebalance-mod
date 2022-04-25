using BepInEx;
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

namespace Character_rebalance
{
    class CharonPatches
    {
        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {

                // soul stigma
                if (__instance.Key == GDEItemKeys.Skill_S_ShadowPriest_7)
                {

                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_ShadowPriest_7, CustomLoc.TermType.Description));

                    dict.TryGetStringList("SkillExtended", out List<string> ogSkillExtended, GDEItemKeys.Skill_S_ShadowPriest_7);
                    ogSkillExtended.Add(nameof(Extended_Charon_SoulStigma));
                    __instance.SkillExtended = ogSkillExtended;
                }
                // dark heal
                else if (__instance.Key == GDEItemKeys.Skill_S_ShadowPriest_0)
                {
                    __instance.IgnoreTaunt = false;

                }
                // forbidden flame
                else if (__instance.Key == GDEItemKeys.Skill_S_ShadowPriest_12_0)
                {
                    __instance.NotCount = false;
                }
                // absorb soul
                else if (__instance.Key == GDEItemKeys.Skill_S_ShadowPriest_8)
                {
                    __instance.Basic = true;
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_ShadowPriest_8, CustomLoc.TermType.Description));

                    __instance.SkillExtended = new List<string>() { nameof(Extended_Charon_AbsorbSoul) };
                }
                


            }
        }


        [HarmonyPatch(typeof(GDEBuffData), nameof(GDEBuffData.LoadFromDict))]
        class GdeBuffPatch
        {
            static void Postfix(GDEBuffData __instance)
            {
                // vigil of darkness
                if (__instance.Key == GDEItemKeys.Buff_B_ShadowPriest_4_T)
                {
                    __instance.TagPer = 108;
                }
            }
        }



        [HarmonyPatch(typeof(Skill_Extended), nameof(Skill_Extended.Init))]
        class EvilPhantasmPatch
        {
            static void Postfix(Skill_Extended __instance)
            {
                if (__instance is SkillExtended_ShadowPriest_5 epEx)
                {
                    if (BattleSystem.instance != null && TurnEventObserver.turnEvents.DamageTake != null)
                    {
                        if (TurnEventObserver.turnEvents.DamageTake.Find(dt => dt.User.Info.Ally && !dt.User.Dummy) != null)
                        {
                            epEx.Flag = true;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(SkillExtended_ShadowPriest_0))]
        class DarkHealPatch
        {

            public static float halfHpBonus = 80f;

            [HarmonyPatch(nameof(SkillExtended_ShadowPriest_0.Special_PointerEnter))]
            [HarmonyPatch(nameof(SkillExtended_ShadowPriest_0.DescExtended))]
            [HarmonyPatch(nameof(SkillExtended_ShadowPriest_0.SkillUseSingle))]

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {

                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Ldc_R4 && (float)ci.operand == 50f)
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_R4, halfHpBonus);
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
