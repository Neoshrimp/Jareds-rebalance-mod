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
                    ogSkillExtended.Add(typeof(Extended_Charon_SoulStigma).AssemblyQualifiedName);
                    __instance.SkillExtended = ogSkillExtended;

                    dict.TryGetCustomList("PlusKeyWordsKey", out List<GDESkillKeywordData> ogPlusKeyWords);
                    ogPlusKeyWords.Add(new GDESkillKeywordData(CustomKeys.SkillKeyword_Keyword_Swiftness));
                    __instance.PlusKeyWords = ogPlusKeyWords;
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

                    __instance.SkillExtended = new List<string>() { typeof(Extended_Charon_AbsorbSoul).AssemblyQualifiedName };
                }
                // shadow pillars
                else if (__instance.Key == GDEItemKeys.Skill_S_ShadowPriest_2)
                {
                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_ShadowPriest_2);
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_ShadowPriest_2, CustomLoc.TermType.ExtraDesc)) + ogDesc;
                }
                // soul strike
                else if (__instance.Key == GDEItemKeys.Skill_S_ShadowPriest_9)
                {
                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_ShadowPriest_9);
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_ShadowPriest_9, CustomLoc.TermType.ExtraDesc)) + ogDesc + CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_ShadowPriest_9, CustomLoc.TermType.Description));

                }




            }
        }


        [HarmonyPatch(typeof(GDEBuffData), nameof(GDEBuffData.LoadFromDict))]
        class GdeBuffPatch
        {
            static void Postfix(GDEBuffData __instance, Dictionary<object, object> dict)
            {
                // vigil of darkness
                if (__instance.Key == GDEItemKeys.Buff_B_ShadowPriest_4_T)
                {
                    __instance.TagPer = 108;

                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Buff_B_ShadowPriest_4_T);
                    __instance.Description = ogDesc + CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Buff, GDEItemKeys.Buff_B_ShadowPriest_4_T, CustomLoc.TermType.ExtraDesc));


                }
            }
        }


        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GdeSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance)
            {
                // soul strike
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_ShadowPriest_9_T)
                {
                    __instance.DMG_Per = 50;
                }


            }
        }

        [HarmonyPatch(typeof(B_ShadowPriest_4_T), "Hit")]
        class VigilOfDarknessPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var list = instructions.ToList();
                int c = list.Count;

                for (int i = 0; i < c; i++)
                {

                    if (list[Math.Min(i + 4, c - 1)].opcode == OpCodes.Callvirt && ((MethodInfo)list[Math.Min(i + 4, c - 1)].operand).Equals(AccessTools.Method(typeof(BattleChar), nameof(BattleChar.BuffAdd))))
                    {
                        list[i] = new CodeInstruction(OpCodes.Ldc_I4, 500);
                    }
                    // label not marked exception
/*                    else if (list[i].opcode == OpCodes.Newobj && ((ConstructorInfo)list[i].operand).Equals(AccessTools.Constructor(typeof(NotImplementedException), new Type[] { }))
                        || list[i].opcode == OpCodes.Throw)
                    {
                        list[i] = new CodeInstruction(OpCodes.Nop);
                    }*/
                }

                return list.AsEnumerable();
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

        [HarmonyPatch(typeof(SkillExtended_ShadowPriest_2), "Init")]
        class ShadowPillarsPatch
        {
            static void Postfix(SkillExtended_ShadowPriest_2 __instance)
            {
                __instance.PlusSkillStat.Penetration = 100f;
            }
        }

        [HarmonyPatch(typeof(S_ShadowPriest_9), nameof(S_ShadowPriest_9.SkillUseSingle))]
        class SoulsStrikePatch
        {
            static bool Prefix(Skill SkillD, List<BattleChar> Targets, S_ShadowPriest_9 __instance)
            {
                int num = 0;
                foreach (BattleEnemy battleEnemy in BattleSystem.instance.EnemyList)
                {
                    foreach (Buff buff in battleEnemy.GetBuffs(BattleChar.GETBUFFTYPE.DOT, false, false))
                    {
                        battleEnemy.BuffAdd(buff.BuffData.Key, buff.Usestate_L, false, 500, false, buff.StackInfo[buff.StackInfo.Count - 1].RemainTime, false);
                        num += buff.StackNum;
                    }
                }

                foreach (BattleAlly ba in BattleSystem.instance.AllyList)
                {
                    foreach (Buff buff in ba.GetBuffs(BattleChar.GETBUFFTYPE.DOT, false, false))
                    {
                        ba.BuffAdd(buff.BuffData.Key, buff.Usestate_L, false, 500, false, buff.StackInfo[buff.StackInfo.Count - 1].RemainTime, false);
                    }

                }

                int repeats = 1;
                if (__instance?.MainThurible.ChargeNow >= 2)
                {
                    repeats = 2;
                    __instance.MainThurible.ChargeNow -= 2;
                }
                for (int r = 0; r < repeats; r++)
                {
                    for (int i = 0; i < num; i++)
                    {
                        BattleSystem.DelayInputAfter(__instance.Attack(Targets[0]));
                    }
                }
                return false;
            }
        }





    }
}
