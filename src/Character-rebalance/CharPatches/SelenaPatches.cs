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
    class SelenaPatches
    {
        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // blessing of the moon
                if (__instance.Key == GDEItemKeys.Skill_S_TW_Blue_3)
                {
                    __instance.NotCount = true;
                }
                // bloody moon
                else if (__instance.Key == GDEItemKeys.Skill_S_TW_Blue_6)
                {
                    __instance.NotCount = false;

                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_TW_Blue_6);
                    __instance.Description = ogDesc + CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_TW_Blue_6, CustomLoc.TermType.ExtraDesc));

                    dict.TryGetStringList("SkillExtended", out List<string> ogSkillExtended, GDEItemKeys.Skill_S_TW_Blue_6);
                    ogSkillExtended.Add(CustomKeys.ClassName_Extended_Selena_Bloody_Moon);
                    __instance.SkillExtended = ogSkillExtended;
                }
                // dark moon
                else if (__instance.Key == GDEItemKeys.Skill_S_TW_Blue_8)
                {
                    __instance.Track = true;
                }
                // power of the full moon
                else if (__instance.Key == GDEItemKeys.Skill_S_TW_Blue_0)
                {
                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_TW_Blue_0);
                    __instance.Description = ogDesc + CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_TW_Blue_0, CustomLoc.TermType.ExtraDesc));

                    dict.TryGetStringList("SkillExtended", out List<string> ogSkillExtended, GDEItemKeys.Skill_S_TW_Blue_0);
                    ogSkillExtended.Add(CustomKeys.ClassName_Extended_Selena_PowerOfTheFullMoon);
                    __instance.SkillExtended = ogSkillExtended;
                }
                // tears of the moon
                else if (__instance.Key == GDEItemKeys.Skill_S_TW_Blue_R0)
                {

                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_TW_Blue_R0);
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_TW_Blue_R0, CustomLoc.TermType.ExtraDesc)) + ogDesc;


                    dict.TryGetStringList("SkillExtended", out List<string> ogSkillExtended, GDEItemKeys.Skill_S_TW_Blue_R0);
                    ogSkillExtended.Add(nameof(Extended_Selena_TearsOfTheMoon));
                    __instance.SkillExtended = ogSkillExtended;
                }


            }
        }

        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GdeSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance)
            {
                // lunar ring
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_TW_Blue_7_T)
                {
                    __instance.HEAL_Per = 90;
                }
            }
        }



    }
}
