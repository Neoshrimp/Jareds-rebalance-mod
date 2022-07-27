using BepInEx;
using Character_rebalance.Extends;
using DarkTonic.MasterAudio;
using GameDataEditor;
using HarmonyLib;
using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Debug = UnityEngine.Debug;


namespace Character_rebalance.CharPatches
{
    public class PhoenixPatches
    {
        [HarmonyPatch(typeof(GDECharacterData), nameof(GDECharacterData.LoadFromDict))]
        class GdeCharactersPatch
        {
            static void Postfix(GDECharacterData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.Character_Phoenix)
                {
                    dict.TryGetVector2("ATK", out Vector2 ogATK);
                    __instance.ATK = new Vector2(14, ogATK.y);

                    dict.TryGetVector2("HIT", out Vector2 ogHIT);
                    __instance.HIT = new Vector2(97, ogHIT.y);

                    dict.TryGetVector2("DODGE", out Vector2 ogDODGE);
                    __instance.DODGE = new Vector2(5, ogDODGE.y);

                    dict.TryGetVector2("RES_DEBUFF", out Vector2 ogRES_DEBUFF);
                    __instance.RES_DEBUFF = new Vector2(10, ogRES_DEBUFF.y);

                    dict.TryGetVector2("HIT_CC", out Vector2 ogHIT_CC);
                    __instance.HIT_CC = new Vector2(10, ogHIT_CC.y);

                }
            }
        }



        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GDESkillData_Patch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // nibble
                if (__instance.Key == GDEItemKeys.Skill_S_Phoenix_7)
                {
                    __instance.NotCount = true;
                }
                // blazing regeneration
                else if (__instance.Key == GDEItemKeys.Skill_S_Phoenix_1)
                {
                    __instance.UseAp = 1;
                    __instance.Target = new GDEs_targettypeData(GDEItemKeys.s_targettype_self);
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Phoenix_7, CustomLoc.TermType.Description));


                }
                // fiery wings
                else if (__instance.Key == GDEItemKeys.Skill_S_Phoenix_3)
                {
                    __instance.NotCount = false;

                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_Phoenix_3);
                    ogDesc = ogDesc.Replace("80", "25");
                    __instance.Description = ogDesc + CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Phoenix_3, CustomLoc.TermType.ExtraDesc));

                    __instance.SkillExtended = new List<string>() { typeof(Extended_Phoenix_FieryWings).AssemblyQualifiedName };
                }
                // face slap
                else if (__instance.Key == GDEItemKeys.Skill_S_Phoenix_11)
                {
                    __instance.Fatal = true;
                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_Phoenix_11);
                    __instance.Description = ogDesc.Replace("3+", "2+");
                }
                // phoenix kick
                else if (__instance.Key == GDEItemKeys.Skill_S_Phoenix_12)
                {
                    __instance.UseAp = 3;
                    __instance.Track = true;
                }
                // first hit
                else if (__instance.Key == GDEItemKeys.Skill_S_Phoenix_9)
                {
                    __instance.Track = true;
                }
                // nap time
                else if (__instance.Key == GDEItemKeys.Skill_S_Phoenix_6)
                {
                    dict.TryGetString("Description", out string ogDesc);
                    ogDesc = ogDesc.Replace("30%", "40%");
                    ogDesc = ogDesc.Replace("15%", "25%");
                    __instance.Description = ogDesc;
                }
                // eternal flame
                else if (__instance.Key == GDEItemKeys.Skill_S_Phoenix_6_1)
                {
                    dict.TryGetString("Description", out string ogDesc);
                    ogDesc = ogDesc.Replace("1", "3");
                    ogDesc = ogDesc.Replace("33%", "20%");
                    __instance.Description = ogDesc;
                }
                // legendary phoenix
                else if (__instance.Key == GDEItemKeys.Skill_S_Phoenix_4_0)
                {

                    __instance.UseAp = 1;
                }

                // undying sanctuary
                else if (__instance.Key == GDEItemKeys.Skill_S_Phoenix_4)
                {
                    dict.TryGetString("Description", out string ogDesc);
                    ogDesc = ogDesc.Replace("1", "5");
                    __instance.Description = ogDesc;
                }

            }
        }

            [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GDESkillEffectData_Patch
        {
            static void Postfix(GDESkillEffectData __instance, Dictionary<string, object> dict)
            {
                // leer
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_Phoenix_2_T)
                {

                    __instance.Buffs = new List<GDEBuffData>() { new GDEBuffData(GDEItemKeys.Buff_B_Taunt), new GDEBuffData(GDEItemKeys.Buff_B_Phoenix_2_T) };

                    __instance.BuffPlusTagPer = new List<int>() { 30 };
                }
                // face slap
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Phoenix_11_T)
                {
                    __instance.CRI = 25;
                }
                // phoenix kick
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Phoenix_12_T)
                {
                    __instance.DMG_Per = 180;
                }
                // eternal flame
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Phoenix_6_1_0_T || __instance.Key == GDEItemKeys.SkillEffect_SE_Phoenix_10_0_T)
                {
                    __instance.DMG_Per = 165;
                }
            }
        }



        [HarmonyPatch(typeof(GDEBuffData), nameof(GDEBuffData.LoadFromDict))]
        class GDEBuffData_Patch
        {
            static void Postfix(GDEBuffData __instance, Dictionary<string, object> dict)
            {
                // nibble
                if (__instance.Key == GDEItemKeys.Buff_B_Phoenix_7_T)
                {
                    __instance.LifeTime = 2;
                }
                // undying sanctuary
                else if (__instance.Key == GDEItemKeys.Buff_B_Phoenix_4_S)
                {
                    __instance.LifeTime = 3;
                }
            }
        }


        [HarmonyPatch(typeof(Character), nameof(Character.Set_AllyData))]
        class AddEfficientBread_Patch
        {
            static void Postfix(Character __instance, GDECharacterData Data)
            {
                if (Data.Key == GDEItemKeys.Character_Phoenix)
                {
                    var protect = __instance.SkillDatas.Find(sd => sd.Skill.Key == GDEItemKeys.Skill_S_DefultSkill_2);
                    if (protect != null)
                        __instance.SkillDatas.Remove(protect);
                    __instance.SkillDatas.Add(new CharInfoSkillData(GDEItemKeys.Skill_S_Phoenix_10));
                }

            }
        }


        [HarmonyPatch]
        class Nibble_Patch
        {

            static float painDmgScaling = 0.16f;

            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(S_Phoenix_7), nameof(S_Phoenix_7.DescExtended));
                yield return AccessTools.Method(typeof(S_Phoenix_7), nameof(S_Phoenix_7.SkillUseSingle));

            }


            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {

                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Ldc_R4, 0.33f))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_R4, painDmgScaling);
                    }
                    else
                    {
                        yield return ci;
                    }

                }
            }

        }


        [HarmonyPatch]
        class BlazingRegen_Patch
        {

            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(SkillExtended_Phoenix_1).
                    GetNestedTypes(BindingFlags.NonPublic).ToList().Find(t => t.Name == "<Damage>c__Iterator1"),
                    "MoveNext");

            }


            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                CodeInstruction prevCi = null;
                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Ldc_I4_1 && prevCi != null && prevCi.opcode == OpCodes.Ldc_I4_0)
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                    }
                    else
                    {
                        yield return ci;
                    }
                    prevCi = ci;

                }
            }

        }


        [HarmonyPatch(typeof(B_Phoenix_2), nameof(B_Phoenix_2.Init))]
        class WatchuLookingAt_Patch
        {
            static void Postfix(B_Phoenix_2 __instance)
            {
        		__instance.PlusPerStat.Damage = 20 * __instance.StackNum;
            }
        }


        [HarmonyPatch]
        class FaceSlap_Patch
        {

            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(S_Phoenix_11), "<Terms>m__0");
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Ldc_I4_3 )
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


        [HarmonyPatch]
        class Naptime_Patch
        {

            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(Extended_Phoenix_6), "SkillUseSingle");
                yield return AccessTools.Method(typeof(S_Phoenix_6_0), "SkillUseSingle");

            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Ldc_R4, 0.3f))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_R4, 0.4f);
                    }
                    else if (ci.Is(OpCodes.Ldc_R4, 0.2f))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_R4, 0.25f);
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }
        }



        [HarmonyPatch(typeof(S_Phoenix_6_1_0_ally), nameof(S_Phoenix_6_1_0_ally.Init))]
        class EFallyDmg_Patch
        {

            static void Postfix(S_Phoenix_6_1_0_ally __instance)
            {
                __instance.SkillBasePlus.Target_BaseDMG = -(int)((float)__instance.MySkill.TargetDamage * 0.8f);
            }
        }



        [HarmonyPatch(typeof(S_Phoenix_6_1))]
        class EternalFlame_Patch
        {
            [HarmonyPatch(nameof(S_Phoenix_6_1.ButtonSelectTerms)), HarmonyPrefix]
            static bool ButtonSelectTermsPrefix(S_Phoenix_6_1 __instance, ref bool __result)
            {
                __result = PartyInventory.InvenM.FindItem(GDEItemKeys.Item_Consume_Bread) >= 3;
                return false;
            }


            [HarmonyPatch(nameof(S_Phoenix_6_1.SkillUseSingle)), HarmonyPrefix]
            static bool SkillUseSinglePrefix(S_Phoenix_6_1 __instance)
            {
                PlayData.TSavedata.Phoenix6 = true;
                PartyInventory.InvenM.DelItem(GDEItemKeys.Item_Consume_Bread, 3);
                BattleSystem.DelayInputAfter(S_Phoenix_6_1.AllyAttack(__instance.BChar, null, true));
                return false;
            }

        }


        [HarmonyPatch]
        class EFattackRoutine_Patch
        {

            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(S_Phoenix_6_1_0).
                    GetNestedTypes(BindingFlags.NonPublic).ToList().Find(t => t.Name == "<ParticleOut_After>c__Iterator0"),
                    "MoveNext");

            }

            static IEnumerator CustomAttackEnemy(BattleChar BChar, BattleChar BeforeTarget, bool FirstShot = false)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                if (FirstShot || !(BeforeTarget == null))
                {
                    if (!(BeforeTarget != null) || (!BeforeTarget.IsDead && BeforeTarget.HP > 0))
                    {
                        yield return new WaitForSecondsRealtime(0.3f);
                        Skill Tempskill = Skill.TempSkill(GDEItemKeys.Skill_S_Phoenix_6_1_0, BChar, BChar.MyTeam);
                        Tempskill.FreeUse = true;
                        Tempskill.PlusHit = true;
                        BChar.ParticleOut(Tempskill, BattleSystem.instance.EnemyTeam.AliveChars.Random<BattleChar>());
                    }
                }
                yield break;
            }


            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                CodeInstruction prevCi = null;
                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Call, AccessTools.Method(typeof(S_Phoenix_6_1), nameof(S_Phoenix_6_1.EnemyAttack))))
                    {
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(EFattackRoutine_Patch), nameof(EFattackRoutine_Patch.CustomAttackEnemy)));
                    }
                    else
                    {
                        yield return ci;
                    }
                    prevCi = ci;

                }
            }

        }


        [HarmonyPatch(typeof(S_Phoenix_4))]
        class UndyingSanctuaryBreadUse_Patch
        {
            [HarmonyPatch(nameof(S_Phoenix_4.ButtonSelectTerms)), HarmonyPrefix]
            static bool ButtonSelectTermsPrefix(S_Phoenix_4 __instance, ref bool __result)
            {
                __result = PartyInventory.InvenM.FindItem(GDEItemKeys.Item_Consume_Bread) >= 5;
                return false;
            }


            [HarmonyPatch(nameof(S_Phoenix_4.SkillUseSingle)), HarmonyPrefix]
            static bool SkillUseSinglePrefix(S_Phoenix_4 __instance)
            {
                PartyInventory.InvenM.DelItem(GDEItemKeys.Item_Consume_Bread, 5);
                if (__instance.BChar.Info.Passive is P_Phoenix)
                {
                    (__instance.BChar.Info.Passive as P_Phoenix).ChoiceListBattle.Remove(GDEItemKeys.Skill_S_Phoenix_4);
                }
                return false;
            }

        }


    }
}
