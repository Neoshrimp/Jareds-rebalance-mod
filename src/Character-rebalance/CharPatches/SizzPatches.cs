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
                    __instance.NotCount = true;

                    __instance.SkillExtended = new List<string>() {nameof(Extended_Sizz_EveHelp)};

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

        [HarmonyPatch(typeof(Buff), "Init")]
        class dd
        {
            static void Postfix(Buff __instance)
            {
                if (__instance is B_Sizz_0_T ca)
                {
                    Debug.Log(ca.StackNum);
                }
            }
        }


    }
}
