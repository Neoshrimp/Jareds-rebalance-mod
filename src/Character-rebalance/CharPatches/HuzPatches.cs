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
    class HuzPatches
    {

        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GDESkillData_Patch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // give and take
                if (__instance.Key == GDEItemKeys.Skill_S_Queen_12)
                {
                    __instance.UseAp = 1;
                }
                // whip of healing
                else if (__instance.Key == GDEItemKeys.Skill_S_Queen_3)
                {
                    __instance.NotCount = false;
                }
                // restrained healing
                else if (__instance.Key == GDEItemKeys.Skill_S_Queen_9)
                {
                    __instance.NotCount = true;
                }
                // pain equals happiness
                else if (__instance.Key == GDEItemKeys.Skill_S_Queen_10)
                {
                    __instance.NotCount = true;
                }
            }
        }


        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GDESkillEffectData_Patch
        {
            static void Postfix(GDESkillEffectData __instance, Dictionary<string, object> dict)
            {
                // whip of healing
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_Queen_3_T)
                {
                    __instance.HEAL_Per = 60;
                }
                // crack extra hit
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Queen_6_0_T)
                {
                    __instance.DMG_Per = 66;
                }
                // restrained healing
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Queen_9_T)
                {
                    __instance.CRI = 100;
                }


            }
        }


        [HarmonyPatch(typeof(GDEBuffData), nameof(GDEBuffData.LoadFromDict))]
        class GDEBuffData_Patch
        {
            static void Postfix(GDEBuffData __instance, Dictionary<string, object> dict)
            {
                // domination
                if (__instance.Key == GDEItemKeys.Buff_B_Queen_4_T)
                {
                    __instance.LifeTime = 4;
                }
                // pain equals happiness
                else if (__instance.Key == GDEItemKeys.Buff_B_Queen_10_T)
                {
                    __instance.LifeTime = 3;
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Buff, GDEItemKeys.Buff_B_Queen_10_T, CustomLoc.TermType.Description));

                }

            }
        }




        [HarmonyPatch(typeof(S_Queen_6), nameof(S_Queen_6.DescExtended))]
        class Crack_Patch
        {

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {

                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Ldc_R4, 0.33f))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_R4, 0.66f);
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }

        }


        [HarmonyPatch(typeof(B_Queen_10_T), nameof(B_Queen_10_T.DamageTake))]
        class PainEqualsHappiness_Patch
        {
            public static bool Prefix(B_Queen_10_T __instance, BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
            {
                if (NODEF)
                {
                    resist = true;
                    if (__instance.BChar.Info.Ally == User.Info.Ally && User != BattleSystem.instance.DummyChar)
                    {
                        __instance.BChar.Heal(User, (float)(Dmg), false, false, null);
                    }
                    else
                    {
                        __instance.BChar.Heal(User, (float)(Dmg / 2), false, false, null);
                    }
                }
                return true;
            }

        }



    }
}
