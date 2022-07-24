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
    class TrishaPatches
    {

        [HarmonyPatch(typeof(GDECharacterData), nameof(GDECharacterData.LoadFromDict))]
        class GdeCharactersPatch
        {
            static void Postfix(GDECharacterData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.Character_Trisha)
                {
                    dict.TryGetVector2("MAXHP", out Vector2 ogMAXHP);
                    __instance.MAXHP = new Vector2(18, ogMAXHP.y);
                }
            }
        }

        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // shadow curtain
                if (__instance.Key == GDEItemKeys.Skill_S_Trisha_5)
                {
                    __instance.NoBasicSkill = false;
                    __instance.Disposable = true;


                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Trisha_5, CustomLoc.TermType.Description));

                    dict.TryGetCustomList("SKillExtendedItemKey", out List<GDESkillExtendedData> ogSKillExtendedItemKey);
                    ogSKillExtendedItemKey.RemoveAll(sei => sei.Key == GDEItemKeys.SkillExtended_Trisha_5_Ex);
                    __instance.SKillExtendedItem = ogSKillExtendedItemKey;
                }
                // shadow slash
                else if (__instance.Key == GDEItemKeys.Skill_S_Trisha_0)
                {
                    __instance.SkillExtended = new List<string>() { typeof(Extended_Trisha_ShadowSlash).AssemblyQualifiedName };

                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_Trisha_0);
                    __instance.Description = ogDesc + CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Trisha_0, CustomLoc.TermType.ExtraDesc));

                    dict.TryGetCustomList("PlusKeyWordsKey", out List<GDESkillKeywordData> ogPlusKeyWords);
                    ogPlusKeyWords.Add(new GDESkillKeywordData(CustomKeys.SkillKeyword_Keyword_Critical));
                    __instance.PlusKeyWords = ogPlusKeyWords;
                }
                // shadow dance
                else if (__instance.Key == GDEItemKeys.Skill_S_Trisha_12)
                {
                    __instance.Track = true;
                }



            }
        }




        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GdeSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance)
            {
                // shadow step
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_Trisha_6_T)
                {
                    __instance.DMG_Per = 100;
                }
                // shadow step
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Trisha_6_S)
                {
                    __instance.Buffs = new List<GDEBuffData>() { new GDEBuffData(GDEItemKeys.Buff_B_CamouflageCloak) };
                }




            }
        }

    }
}
