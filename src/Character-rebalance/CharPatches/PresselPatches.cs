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
    public class PresselPatches
    {


        [HarmonyPatch(typeof(GDECharacterData), nameof(GDECharacterData.LoadFromDict))]
        class GDECharacterData_Patch
        {
            static void Postfix(GDECharacterData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.Character_Priest)
                {
                    dict.TryGetVector2("HIT", out Vector2 ogHIT);
                    __instance.HIT = new Vector2(97, ogHIT.y);
                    dict.TryGetVector2("MAXHP", out Vector2 ogMAXHP);
                    __instance.MAXHP = new Vector2(22, ogMAXHP.y);
                }
            }
        }


        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GDESkillData_Patch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // healing coil
                if (__instance.Key == GDEItemKeys.Skill_S_Priest_0)
                {
                    __instance.SkillExtended = new List<string>() { typeof(Extended_Pressel_HealingCoil).AssemblyQualifiedName };
                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Priest_0, CustomLoc.TermType.ExtraDesc))
                        + ogDesc;
                }
                // holy light
                else if (__instance.Key == GDEItemKeys.Skill_S_Priest_1)
                {

                    __instance.SkillExtended = new List<string>() { typeof(Extended_Pressel_HolyLight).AssemblyQualifiedName };

                    __instance.NotCount = false;
                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Priest_1, CustomLoc.TermType.ExtraDesc))
                        + ogDesc;
                }
                // first class to heaven
                else if (__instance.Key == GDEItemKeys.Skill_S_Priest_2)
                {
                    __instance.Track = true;
                }
                // shining pillars
                else if (__instance.Key == GDEItemKeys.Skill_S_Priest_10)
                {
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Priest_10, CustomLoc.TermType.Description));


                }


            }
        }


        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GDESkillEffectData_Patch
        {
            static void Postfix(GDESkillEffectData __instance, Dictionary<string, object> dict)
            {
                // holy light
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_Priest_1_T)
                {
                    __instance.CRI = 15;
                    __instance.HEAL_Per = 85;
                }
                // kill it
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Priest_3_T)
                {
                    __instance.DMG_Per = 80;
                }
            }
        }



        [HarmonyPatch]
        class BlinkingHeal_Patch
        {


            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(Extended_Priest_4), nameof(Extended_Priest_4.DescExtended));
                yield return AccessTools.Method(typeof(Extended_Priest_4), nameof(Extended_Priest_4.FixedUpdate));

            }


            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {

                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Ldc_R4, 0.2f))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_R4, 0.3f);
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }

        }



        [HarmonyPatch(typeof(Extended_Priest_10), nameof(Extended_Priest_10.SkillUseSingle))]
        class ShiningPillar_Patch
        {
            static bool Prefix(Extended_Priest_10 __instance, Skill SkillD, List<BattleChar> Targets)
            {
                List<BattleChar> globalChar = BattleSystem.instance.GetGlobalChar();
                globalChar.Remove(Targets[0]);
                foreach (BattleChar battleChar in globalChar)
                {
                    battleChar.Heal(__instance.BChar, 50, 0, 0);
                }
                return false;
            }

        }




    }
}
