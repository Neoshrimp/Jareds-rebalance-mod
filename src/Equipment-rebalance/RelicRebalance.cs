using HarmonyLib;
using System.Runtime.CompilerServices;
using PItem;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;
using System.Reflection.Emit;
using System.Reflection;
using System;
using PItem.Equipment_rebalance;

namespace Equipment_rebalance
{
    class RelicRebalance
    {


        

        [HarmonyPatch(typeof(GDESkillExtendedData), nameof(GDESkillExtendedData.LoadFromDict))]
        class GDESkillExtendedData_Patch
        {
            static void Postfix(GDESkillExtendedData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.SkillExtended_Memoryfragment_Ex)
                {
                    dict.TryGetString("Des", out string ogDesc);
                    __instance.Des = ogDesc.Replace(" 2 ", " 1 ");
                }
            }
        }


        [HarmonyPatch(typeof(PassiveItemBase), nameof(PassiveItemBase.ShinyEffect))]
        class PIBReversePatch
        {
            [HarmonyReversePatch]
            [MethodImpl(MethodImplOptions.NoInlining)]
            static public void ShinyEffectStub(PassiveItemBase instance) { return; }
        }


        [HarmonyPatch(typeof(ShadowOrb), nameof(ShadowOrb.SKillUseHand_Team))]
        class ShadowOrbPatch
        {
            // this might not work if source of discard is a source of player playing a skill
            static bool Prefix(Skill skill, ShadowOrb __instance)
            {
                int skillInhand = BattleSystem.instance.AllyTeam.Skills.Contains(skill) ? 1 : 0;

                if (BattleSystem.instance.AllyTeam.Skills.Count-skillInhand == 0)
                {
                    // perhaps BattleSystem.DelayInput?
                    BattleSystem.DelayInputAfter(DrawWhenEmpty(__instance));
                }
                return false;
            }

            static public IEnumerator DrawWhenEmpty(ShadowOrb instance)
            {
                if (BattleSystem.instance.AllyTeam.Skills.Count == 0)
                {
                    PIBReversePatch.ShinyEffectStub(instance);
                    BattleSystem.instance.AllyTeam.Draw();
                }
                yield return null;
                yield break;
            }
        }


        [HarmonyPatch(typeof(Memoryfragment_Ex), nameof(Memoryfragment_Ex.Init))]
        class Memoryfragment_Ex_Patch
        {

            static void Postfix(Memoryfragment_Ex __instance)
            {
                __instance.APChange = -1;
            }
        }


        [HarmonyPatch(typeof(WeatherVane), nameof(WeatherVane.SKillUseHand_Team))]
        class WeatherVane_Patch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {

                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Ldc_I4_6)
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_I4_8);
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }
        }

        [HarmonyPatch]
        class ManaBattery_Patch
        {


            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(ManaBattery), nameof(ManaBattery.Init));
                yield return AccessTools.Method(typeof(ManaBattery), nameof(ManaBattery.Turn));

            }


            static void Postfix(ManaBattery __instance)
            {
                __instance.MyItem.UseStackNum = 99;
            }
        }


        [HarmonyPatch(typeof(Buff), nameof(Buff.FixedUpdate))]
        class ThornyStem_Patch
        {

            static bool IsCCBuff(Buff buff)
            {
                if (buff.BuffData != null && buff.BuffData.BuffTag != null && buff.BuffData.BuffTag.Key == GDEItemKeys.BuffTag_CrowdControl)
                {
                    return true;
                }
                return false;
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {

                bool injected = false;
                var ciEnum = instructions.GetEnumerator();
                while (ciEnum.MoveNext())
                {
                    if (!injected && ciEnum.Current.Is(OpCodes.Ldflda, AccessTools.Field(typeof(PassiveBase), nameof(PassiveBase.PlusStat))))
                    {
                        injected = true;
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ThornyStem_Patch), nameof(ThornyStem_Patch.IsCCBuff)));
                        // skip instruction
                        ciEnum.MoveNext();
                    }
                    else
                    {
                        yield return ciEnum.Current;
                    }
                }
            }
                
        }



    }
}
