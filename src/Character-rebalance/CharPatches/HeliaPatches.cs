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
    class HeliaPatches
    {
        [HarmonyPatch(typeof(GDECharacterData), nameof(GDECharacterData.LoadFromDict))]
        class GdeCharactersPatch
        {
            static void Postfix(GDECharacterData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.Character_TW_Red)
                {
                    dict.TryGetVector2("HIT_DOT", out Vector2 ogHIT_DOT);
                    __instance.HIT_DOT = new Vector2(0, ogHIT_DOT.y);
                    dict.TryGetVector2("HIT_CC", out Vector2 ogHIT_CC);
                    __instance.HIT_CC = new Vector2(0, ogHIT_CC.y);
                    dict.TryGetVector2("HIT_DEBUFF", out Vector2 ogHIT_DEBUFF);
                    __instance.HIT_DEBUFF = new Vector2(0, ogHIT_DEBUFF.y);
                }
            }
        }


        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // blessing of the stars
                if (__instance.Key == GDEItemKeys.Skill_S_TW_Red_2)
                {
                    __instance.NotCount = true;
                }
                // flame eruption
                else if (__instance.Key == GDEItemKeys.Skill_S_TW_Red_5)
                {
                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = ogDesc.Replace("3", "2");
                }
                // solarbolt
                else if (__instance.Key == GDEItemKeys.Skill_S_TW_Red_1)
                {
                    __instance.SkillExtended = new List<string> { CustomKeys.ClassName_Extended_Helia_Solarbolt };

                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_TW_Red_1);
                    __instance.Description = ogDesc + CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_TW_Red_1, CustomLoc.TermType.ExtraDesc));
                }
                // tears of the sun
                else if (__instance.Key == GDEItemKeys.Skill_S_TW_Red_R0)
                {
                    __instance.SkillExtended = new List<string> { CustomKeys.ClassName_Extended_Helia_Tears_of_the_Sun };

                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_TW_Red_R0);
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_TW_Red_R0, CustomLoc.TermType.ExtraDesc)) + ogDesc;

                    dict.TryGetCustomList("PlusKeyWordsKey", out List<GDESkillKeywordData> ogPlusKeyWords);
                    ogPlusKeyWords.Add(new GDESkillKeywordData(CustomKeys.SkillKeyword_Keyword_Swiftness));
                    __instance.PlusKeyWords = ogPlusKeyWords;

                }
                // Selenelion
                else if (__instance.Key == GDEItemKeys.Skill_S_TW_Red_R0_0)
                {
                    __instance.Except = false;
                    __instance.Disposable = true;

                    dict.TryGetStringList("SkillExtended", out List<string> ogSkEx, GDEItemKeys.Skill_S_TW_Red_R0_0);
                    ogSkEx.Add(CustomKeys.ClassName_Extended_Twins_Selenelion);
                    __instance.SkillExtended = ogSkEx;

                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_TW_Red_R0_0);
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_TW_Red_R0_0, CustomLoc.TermType.ExtraDesc)) + ogDesc;
                }
                // flame arrow
                else if (__instance.Key == GDEItemKeys.Skill_S_TW_Red_0)
                {
                    __instance.NoBasicSkill = false;
                    __instance.SkillExtended = new List<string>() { CustomKeys.ClassName_Extended_Helia_Flame_Arrow };
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_TW_Red_0, CustomLoc.TermType.Description));
                }
            }
        }

        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GdeSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance)
            {
                // sun ring
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_TW_Red_4_T)
                {
                    __instance.DMG_Per = 80;
                }
                // solar bolt
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_TW_Red_1_T)
                {
                    __instance.DMG_Per = 170;
                }
                // selenelion
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_TW_Red_R0_0_T)
                {
                    __instance.DMG_Base = 60;
                }
                // flame arrow
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Tw_Red_0_T)
                {
                    __instance.DMG_Per = 80;
                }


            }
        }



        [HarmonyPatch(typeof(S_TW_Red_5))]
        class FlameEruptionPatch
        {

            [HarmonyPatch(nameof(S_TW_Red_5.SkillUseSingle))]
            [HarmonyPatch(nameof(S_TW_Red_5.AttackEffectSingle))]

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Ldc_I4_3)
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_I4_2);
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
