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

namespace Character_rebalance.CharPatches
{
    class IronheartPatches
    {
        [HarmonyPatch(typeof(GDECharacterData), nameof(GDECharacterData.LoadFromDict))]
        class GdeCharactersPatch
        {
            static void Postfix(GDECharacterData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.Character_Prime)
                {
                    dict.TryGetVector2("MAXHP", out Vector2 ogMAXHP);
                    __instance.MAXHP = new Vector2(29, ogMAXHP.y);
                }
            }
        }

        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // high energy emission
                if (__instance.Key == GDEItemKeys.Skill_S_Prime_13)
                {
                    __instance.NotCount = true;
                }
                // duel
                else if (__instance.Key == GDEItemKeys.Skill_S_Prime_6)
                {
                    __instance.NotCount = false;
                }
                // absolute defense
                else if (__instance.Key == GDEItemKeys.Skill_S_Prime_10)
                {
                    __instance.NoBasicSkill = true;
                    __instance.UseAp = 1;
                }
                // double armor
                else if (__instance.Key == GDEItemKeys.Skill_S_Prime_8)
                {
                    __instance.UseAp = 0;
                }
                // shield of retribution
                else if (__instance.Key == GDEItemKeys.Skill_S_Prime_11)
                {
                    __instance.SkillExtended = new List<string>() { nameof(Extended_Ironheart_ShieldOfRetribution) };
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Prime_11, CustomLoc.TermType.Description));

                    dict.TryGetCustomList("PlusKeyWordsKey", out List<GDESkillKeywordData> ogPlusKeyWords);
                    ogPlusKeyWords.Add(new GDESkillKeywordData(CustomKeys.SkillKeyword_Keyword_Swiftness));
                    __instance.PlusKeyWords = ogPlusKeyWords;
                }
                // innocent armor
                else if (__instance.Key == GDEItemKeys.Skill_S_Prime_3)
                {
                    dict.TryGetStringList("SkillExtended", out List<string> ogSkEx, GDEItemKeys.Skill_S_Prime_3);
                    ogSkEx.Add(nameof(ExtendedExtra_Ironheart_InnocentArmor));
                    __instance.SkillExtended = ogSkEx;

                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_Prime_3);
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Prime_3, CustomLoc.TermType.ExtraDesc)) + ogDesc;
                }
            }
        }

        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GdeSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance)
            {
                // charge of faith
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_Prime_12_T)
                {
                    __instance.Buffs = new List<GDEBuffData>() { new GDEBuffData(GDEItemKeys.Buff_B_Taunt), new GDEBuffData(GDEItemKeys.Buff_B_Taunt) };
                }
            }
        }

        //[HarmonyPatch(typeof(GDEBuffData), nameof(GDEBuffData.LoadFromDict))]
        class GdeBuffPatch
        {
            static void Postfix(GDEBuffData __instance)
            {
                // shield of retribution
                if (__instance.Key == GDEItemKeys.Buff_B_Prime_11_T)
                {
                    __instance.LifeTime = 3;
                }
            }
        }




    }
}
