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
    public class SilversteinPatches
    {
        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // enhanced mark
                if (__instance.Key == GDEItemKeys.Skill_S_SilverStein_11)
                {
                    __instance.UseAp = 1;
                }
                // freeze bomb
                else if (__instance.Key == GDEItemKeys.Skill_S_SilverStein_6)
                {
                    dict.TryGetStringList("SkillExtended", out List<string> ogSkillExtended);
                    ogSkillExtended.Add(typeof(Extended_Silverstein_FreezeBomb).AssemblyQualifiedName);
                    __instance.SkillExtended = ogSkillExtended;

                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_SilverStein_6, CustomLoc.TermType.ExtraDesc)) + ogDesc;

                    dict.TryGetCustomList("PlusKeyWordsKey", out List<GDESkillKeywordData> ogPlusKeyWords);
                    ogPlusKeyWords.Add(new GDESkillKeywordData(CustomKeys.SkillKeyword_Keyword_Swiftness));
                    __instance.PlusKeyWords = ogPlusKeyWords;

                }
                // quick fire
                else if (__instance.Key == GDEItemKeys.Skill_S_SilverStein_7)
                {
                    __instance.NotCount = true;

                }
                // overcharged shot
                else if (__instance.Key == GDEItemKeys.Skill_S_SilverStein_4)
                {
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_SilverStein_4, CustomLoc.TermType.Description));

                }
            }
        }

        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GdeSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance, Dictionary<string, object> dict)
            {
                // quick fire
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_SilverStein_7_T)
                {
                    __instance.DMG_Per = 65;

                }
                // rapid shot
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_SilverStein_10_T)
                {
                    __instance.DMG_Per = 30;
                }


            }
        }


        [HarmonyPatch(typeof(P_SilverStein), nameof(P_SilverStein.AttackEffect))]
        class SilverFixedSkillMarkPatch
        {

            static bool CheckIfSilverFixed(Skill skill)
            {
                return skill.BasicSkill && skill.Master.Info.KeyData != GDEItemKeys.Character_SilverStein;
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Ldfld, AccessTools.Field(typeof(Skill), nameof(Skill.BasicSkill))))
                    {
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SilverFixedSkillMarkPatch), nameof(SilverFixedSkillMarkPatch.CheckIfSilverFixed)));
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }
        }

        [HarmonyPatch]
        class RapidShotExtraDmgPatch
        {
            static float extraDmg = 0.65f;

            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(S_SilverStein_10), "DescExtended");
                yield return AccessTools.Method(typeof(S_SilverStein_10), "Special_PointerEnter");
                yield return AccessTools.Method(typeof(S_SilverStein_10), "SkillUseSingle");

                yield return AccessTools.Method(typeof(S_SilverStein_10_0), "DescExtended");
                yield return AccessTools.Method(typeof(S_SilverStein_10_0), "Special_PointerEnter");
                yield return AccessTools.Method(typeof(S_SilverStein_10_0), "SkillUseSingle");

            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Ldc_R4, 0.6f))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_R4, extraDmg);
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Extended_SilverStein_4))]
        class OverchargedShotPatch
        {
            static float extraScaling = 1.5f;

            static float AddExtraScaling()
            {
                foreach (BattleChar battleChar in BattleSystem.instance.EnemyTeam.AliveChars)
                {
                    if (battleChar.BuffFind(GDEItemKeys.Buff_B_SilverStein_P_1, false))
                    {
                        return extraScaling;
                    }
                }
                return 1f;
            }

            [HarmonyPatch(nameof(Extended_SilverStein_4.DescExtended))]

            static void Prefix(ref string desc, Extended_SilverStein_4 __instance)
            {
                desc = desc.Replace("&b", ((int)(extraScaling * 0.8 * __instance.BChar.GetStat.atk)).ToString());
            }


            [HarmonyPatch(nameof(Extended_SilverStein_4.TurnEnd))]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Mul)
                    {
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(OverchargedShotPatch), nameof(OverchargedShotPatch.AddExtraScaling)));
                        yield return ci;
                        yield return new CodeInstruction(OpCodes.Mul);

                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }


        }

        [HarmonyPatch(typeof(FieldSystem), nameof(FieldSystem.NextStage))]
        class OverchargedShotExtraDmgResetPatch
        {
            static void Postfix()
            {
                PlayData.TSavedata.SilverStein_4_Rare = 0;
            }
        }



    }
}
