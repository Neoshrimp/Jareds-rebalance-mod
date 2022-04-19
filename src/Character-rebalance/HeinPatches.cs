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
    public class HeinPatches
    {


        [HarmonyPatch(typeof(GDECharacterData), nameof(GDECharacterData.LoadFromDict))]
        class GdeCharactersPatch
        {
            static void Postfix(GDECharacterData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.Character_Hein)
                {
                    dict.TryGetVector2("ATK", out Vector2 ogATK);
                    __instance.ATK = new Vector2(16, ogATK.y);
                    dict.TryGetVector2("HIT", out Vector2 ogHIT);
                    __instance.HIT = new Vector2(89, ogHIT.y);
                }
            }
        }



        // encyclopedia calls LoadFromDict ???? ffs
        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // blood reflux
                if (__instance.Key == GDEItemKeys.Skill_S_Hein_7)
                {
                    __instance.Basic = true;
                }
                // bloodied revenge
                else if (__instance.Key == GDEItemKeys.Skill_S_Hein_11)
                {
                    __instance.NotCount = false;
                    __instance.UseAp = 3;
                    //string ogDesc = LocalizeManager.DBFile.GetTranslation(string.Concat(GDESchemaKeys.Skill, "/", GDEItemKeys.Skill_S_Hein_11, "_Description"));
                    // gets original value of the description. Changes here should never be incremental to current values as that will make them grow indefinitely 
                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_Hein_11);
                    string exDesc = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Hein_11, CustomLoc.TermType.ExtraDesc));
                        //string.Concat(GDESchemaKeys.Skill, "/", GDEItemKeys.Skill_S_Hein_11, "_ExDesc"));
                    __instance.Description = string.Concat(ogDesc, exDesc);

                }
            }
        }


        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GdeSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance)
            {
                // intimidate
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_Hein_T_4)
                {
                    __instance.DMG_Per = 55;
                }
                // eol
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Hein_9_T)
                {
                    __instance.DMG_Per = 165;
                }
                // mutilate
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Hein_T_1)
                {
                    __instance.DMG_Per = 195;
                }
                // bloodied revenge
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Hein_11_T)
                {
                    __instance.DMG_Per = 105;


                }

            }
        }

        [HarmonyPatch(typeof(Skill_Extended), nameof(Skill_Extended.Init))]
        class SkillExInitPatch
        {
            static void Postfix(Skill_Extended __instance)
            {
                if (__instance is S_Hein_11)
                {
                    __instance.SkillParticleObject = new GDESkillExtendedData(GDEItemKeys.SkillExtended_Public_1_Ex).Particle;
                }
            }
        }


        [HarmonyPatch(typeof(S_Hein_11), nameof(S_Hein_11.DamageTake))]
        class BloodiedRevengePatch
        {
            static void Postfix(S_Hein_11 __instance)
            {
                if (__instance.SkillBasePlus.Target_BaseDMG >= __instance.BChar.GetStat.maxhp * 2)
                {
                    __instance.NotCount = true;
                    __instance.APChange = -2;
                    __instance.SkillParticleOn();
                }
            }
        }

        class RagePatch
        {

            static float hpDrain = 0.3f;


            [HarmonyPatch(typeof(GDEBuffData), nameof(GDEBuffData.LoadFromDict))]
            class GdeBuffPatch
            {
                // rage
                static void Postfix(GDEBuffData __instance)
                {
                    if (__instance.Key == GDEItemKeys.Buff_B_Hein_6_S)
                    {
                        __instance.LifeTime = 2;
                        __instance.Description = __instance.Description.Replace("50%", (hpDrain * 100).ToString() + "%");
                    }
                }
            }

            [HarmonyPatch]
            class BuffPatch
            {
                static IEnumerable<MethodBase> TargetMethods()
                {
                    yield return AccessTools.Method(typeof(B_Hein_S_6), "DescExtended");
                    yield return AccessTools.Method(typeof(B_Hein_S_6), "SKillUseHand_Team");
                }


                static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
                {
                    var list = instructions.ToList();
                    foreach (var ci in instructions)
                    {
                        if (ci.opcode == OpCodes.Ldc_R4 && (float)ci.operand == 0.5f)
                        {
                            yield return new CodeInstruction(OpCodes.Ldc_R4, hpDrain);
                        }
                        else
                        {
                            yield return ci;
                        }
                    }
                }

            }
        }


        [HarmonyPatch(typeof(Extended_Hein_0), nameof(Extended_Hein_0.On))]
        class TearUpPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var list = instructions.ToList();
                int i = list.FindIndex(ci => ci.opcode == OpCodes.Ldc_R4 && (float)ci.operand == 0.35f);
                if (i != -1)
                    list[i].operand = 0.5f;
                return list.AsEnumerable();
            }

        }



        class EndOfTheLinePatch
        {
            static float critThreshold = 55f;

            [HarmonyPatch(typeof(S_Hein_9), nameof(S_Hein_9.SkillUseSingle))]
            class ThresholdPatch
            {
                static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
                {
                    var list = instructions.ToList();
                    foreach (var ci in instructions)
                    {
                        if (ci.opcode == OpCodes.Ldc_R4 && (float)ci.operand == 40f)
                        {
                            yield return new CodeInstruction(OpCodes.Ldc_R4, critThreshold);
                        }
                        else
                        {
                            yield return ci;
                        }
                    }
                }
            }


            [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
            class DescPatch
            {
                static void Postfix(GDESkillData __instance)
                {
                    if (__instance.Key == GDEItemKeys.Skill_S_Hein_9)
                        __instance.Description = __instance.Description.Replace("40%", critThreshold.ToString() + "%");
                }
            }


            // ok some biggest jank I've done to date
            // using a single static bool is fast and clean however a single field will be shared between several patched instances potentially resulting in unwanted behavior 
            //static bool targetMeetsThreshold = false;
            // using dictionary with instance => custom_value(s) mapping almost solves the problem.
            // Only question is how to remove the value from dictionary when mapped instance is no longer relevant
            // Patching some destructor method with Key from dict removal would be ideal but there doesn't seem to be any such methods in game's code
            //static Dictionary<S_Hein_9, bool> targetMeetsThreholdDict = new Dictionary<S_Hein_9, bool>();
            // ConditionalWeakTable is like a dictionary but it can 'monitor' state of its' keys and delete entries when instance is garbage collected
            // however cwt doesn't seem to have been included with game's assemblies. Maybe there's a solution to this but it doesn't work out of the box
            //static ConditionalWeakTable<S_Hein_9, RefDataStruct> targetMeetsThreholdDict = new ConditionalWeakTable<S_Hein_9, RefDataStruct>();


            struct SkillExBool
            {
                public WeakReference weakRef;
                public bool Bool;
            }

            // dictionary using WeakReference to monitor state of the relevant instance
            static Dictionary<int, SkillExBool> refDict = new Dictionary<int, SkillExBool>();

            [HarmonyPatch(typeof(Skill_Extended))]
            class SE_ParticlePatch
            {
                [HarmonyPatch(nameof(Skill_Extended.Init))]
                [HarmonyPostfix]
                static void InitPostfix(Skill_Extended __instance)
                {
                    if (__instance is S_Hein_9 Hein_9)
                    {
                        __instance.SkillParticleObject = new GDESkillExtendedData(GDEItemKeys.SkillExtended_Public_1_Ex).Particle;
                        // clear irrelevant entries
                        foreach (var kv in refDict.ToArray())
                        {
                            if (!kv.Value.weakRef.IsAlive)
                            {
                                refDict.Remove(kv.Key);
                            }
                        }
                        // HashCode should be used as a key instead of instance because that will prevent instances of being garbage collected (as they would be referenced by dictionary keys)
                        refDict.Add(Hein_9.GetHashCode(), new SkillExBool() { weakRef = new WeakReference(__instance, false), Bool = false });
                        // count should decrease after some time if GC is working correctly 
                        //Debug.Log(refDict.Count);
                    }
                }

                [HarmonyPatch(nameof(Skill_Extended.Special_PointerEnter))]
                [HarmonyPostfix]
                static void SpEnterPostfix(Skill_Extended __instance, BattleChar Char)
                {
                    if (__instance is S_Hein_9 Hein_9)
                    {

                        if (Misc.NumToPer((float)Char.GetStat.maxhp, (float)Char.HP) <= critThreshold)
                        {
                            if (refDict.TryGetValue(Hein_9.GetHashCode(), out SkillExBool outValue))
                            {
                                outValue.Bool = true;
                                refDict[Hein_9.GetHashCode()] = outValue;
                            }
                        }
                        else
                        {
                            if (refDict.TryGetValue(Hein_9.GetHashCode(), out SkillExBool outValue))
                            {
                                outValue.Bool = false;
                                refDict[Hein_9.GetHashCode()] = outValue;
                            }
                        }

                    }
                }

                [HarmonyPatch(nameof(Skill_Extended.Special_PointerExit))]
                [HarmonyPostfix]
                static void SpExitPostfix(Skill_Extended __instance)
                {
                    if (__instance is S_Hein_9 Hein_9)
                    {
                        if (refDict.TryGetValue(Hein_9.GetHashCode(), out SkillExBool outValue))
                        {
                            outValue.Bool = false;
                            refDict[Hein_9.GetHashCode()] = outValue;
                        }
                    }
                }


            }

            [HarmonyPatch(typeof(PassiveBase), nameof(PassiveBase.FixedUpdate))]
            class PB_ParticlePatch
            {
                static void Postfix(PassiveBase __instance)
                {
                    if (__instance is S_Hein_9 Hein_9)
                    {
                        refDict.TryGetValue(Hein_9.GetHashCode(), out SkillExBool outValue);
                        if (Misc.NumToPer((float)Hein_9.BChar.GetStat.maxhp, (float)Hein_9.BChar.HP) <= critThreshold || outValue.Bool)
                        {
                            Hein_9.SkillParticleOn();
                        }
                        else
                        {
                            Hein_9.SkillParticleOff();
                        }
                    }
                }
            }

        }




    }
}

