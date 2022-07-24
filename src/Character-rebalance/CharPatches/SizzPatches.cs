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
using Character_rebalance.Extends;


// 2do fix regen buff
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
                    __instance.SkillExtended = new List<string>() { typeof(Extended_Sizz_Incise).AssemblyQualifiedName };
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Sizz_1, CustomLoc.TermType.Description));
                }
                // eve help
                else if (__instance.Key == GDEItemKeys.Skill_S_Sizz_0)
                {
                    __instance.SkillExtended = new List<string>() { typeof(Extended_Sizz_EveHelp).AssemblyQualifiedName };
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Sizz_0, CustomLoc.TermType.Description));

                    dict.TryGetCustomList("PlusKeyWordsKey", out List<GDESkillKeywordData> ogPlusKeyWords);
                    ogPlusKeyWords.Add(new GDESkillKeywordData(CustomKeys.SkillKeyword_Keyword_Swiftness));
                    __instance.PlusKeyWords = ogPlusKeyWords;


                }
                // time to move
                else if (__instance.Key == GDEItemKeys.Skill_S_Sizz_6)
                {
                    __instance.NotCount = true;
                }
                // sacrifice
                else if (__instance.Key == GDEItemKeys.Skill_S_Sizz_4)
                {
                    __instance.Track = true;
                }
                // marionette
                else if (__instance.Key == GDEItemKeys.Skill_S_Sizz_10)
                {
                    __instance.UseAp = 0;
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
                // patch up
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Sizz_3_TBT)
                {
                    __instance.HEAL_Per = 30;
                }
                // time to move
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Sizz_6_T)
                {
                    __instance.HEAL_Per = 65;
                }
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Sizz_6_1_T)
                {
                    __instance.HEAL_Per = 35;
                }
                // sacrifice
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Sizz_4_T)
                {
                    __instance.DMG_Per = 135;
                }



            }
        }

        [HarmonyPatch(typeof(GDEBuffData), nameof(GDEBuffData.LoadFromDict))]
        class GdeBuffPatch
        {
            // patch up
            static void Postfix(GDEBuffData __instance)
            {
                if (__instance.Key == GDEItemKeys.Buff_B_Sizz_3_T)
                {

                    __instance.ClassName = typeof(Extended_Sizz_PatchUpBuff).AssemblyQualifiedName;
                    __instance.MaxStack = 1;
                }
            }
        }




    }
}
