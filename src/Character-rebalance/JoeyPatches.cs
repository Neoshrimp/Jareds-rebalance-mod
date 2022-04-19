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
    public class JoeyPatches
    {
        [HarmonyPatch(typeof(GDECharacterData), nameof(GDECharacterData.LoadFromDict))]
        class GdeCharactersPatch
        {
            static void Postfix(GDECharacterData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.Character_Joey)
                {
                    dict.TryGetVector2("DODGE", out Vector2 ogDODGE);
                    __instance.DODGE = new Vector2(8, ogDODGE.y);
                }
            }
        }

        public static float healingDroneDmg = 0.4f;
        public static float healingDroneHeal = 0.55f;

        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict, ref GameObject ____Particle, ref string ____PathParticle)
            {
                // chemical substance
                if (__instance.Key == GDEItemKeys.Skill_S_Joey_0)
                {
                    __instance.Basic = true;
                }
                // weakening smog
                else if (__instance.Key == GDEItemKeys.Skill_S_Joey_8)
                {
                    __instance.NotCount = true;
                }
                // healing drone
                else if (__instance.Key == GDEItemKeys.Skill_S_Joey_11)
                {
                    __instance.Target = new GDEs_targettypeData(GDEItemKeys.s_targettype_enemy);
                    __instance.IgnoreTaunt = true;

                    __instance.SkillExtended = new List<string>() { CustomKeys.ClassName_Joey_HealingDrone_Ex };
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(
                        GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Joey_11, CustomLoc.TermType.Description));

                    ____PathParticle = "Particle/Joey/Joey_5";
                    ____Particle = new GDESkillData(GDEItemKeys.Skill_S_Joey_5).Particle;
                }
                // health-augmenting patch
                else if (__instance.Key == GDEItemKeys.Skill_S_Joey_12)
                {
                    __instance.NotCount = false;

                    dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Skill_S_Joey_12);
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Joey_12, CustomLoc.TermType.ExtraDesc))
                        + ogDesc;

                    __instance.SkillExtended = new List<string>() { CustomKeys.ClassName_Joey_HealthPatch_Ex };
                }
                // protecting gas
                else if (__instance.Key == GDEItemKeys.Skill_S_Joey_4)
                {
                    __instance.Disposable = true;
                    __instance.NoBasicSkill = false;
                    __instance.UseAp = 2;

                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Joey_4, CustomLoc.TermType.Description));

                }
            }
        }



        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GdeSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance)
            {
                // chemical substance
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_Joey_T_2)
                {
                    __instance.DMG_Per = 50;
                }
                // weakening smog
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Joey_T_8)
                {
                    __instance.DMG_Per = 80;
                }
                // healing drone
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Joey_11_T)
                {
                    __instance.HEAL_Per = 0;
                    __instance.DMG_Per = 0;
                    __instance.DMG_Base = 1;
                }
                // health-augmenting patch
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Joey_12_T)
                {
                    __instance.HEAL_Per = 110;
                }
                // protecting gas
                else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Joey_4_T)
                {
                    __instance.HEAL_Per = 70;
                    __instance.Buffs = new List<GDEBuffData>() { new GDEBuffData(GDEItemKeys.Buff_B_Joey_T_3) };
                }
            }
        }

        [HarmonyPatch(typeof(GDEBuffData), nameof(GDEBuffData.LoadFromDict))]
        class GdeBuffPatch
        {
            static void Postfix(GDEBuffData __instance)
            {
                // weakening smog embrittlement
                if (__instance.Key == GDEItemKeys.Buff_B_Joey_T_8)
                {
                    __instance.TagPer = 100;
                }
                // protecting gas
                if (__instance.Key == GDEItemKeys.Buff_B_Joey_4_T_1)
                {
                    __instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Buff, GDEItemKeys.Buff_B_Joey_4_T_1, CustomLoc.TermType.Description));
                    __instance.LifeTime = 1;
                }

            }
        }

        [HarmonyPatch(typeof(BattleChar), nameof(BattleChar.BuffAdd))]
        class ProtectingGasBuffPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var list = instructions.ToList();
                int c = list.Count;

                for(int i = 0; i < c; i++)
                {

                    if (list[i].opcode == OpCodes.Callvirt && ((MethodInfo)list[i].operand).Equals(AccessTools.Method(typeof(Buff), nameof(Buff.SelfStackDestroy)))
                        && list[Math.Max(i - 3, 0)].operand.ToString() == "System.String Buff_B_Joey_4_T_1")
                    {
                        list[i] = new CodeInstruction(OpCodes.Pop);
                    }
                }

                return list.AsEnumerable();
            }
        }


        [HarmonyPatch(typeof(Extended_Joey_11_0), nameof(Extended_Joey_11_0.Init))]
        class HealingDroneDamageExtendedPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Ldc_R4 && (float)ci.operand == 0.5f)
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_R4, healingDroneDmg);
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(B_Joey_T_8), nameof(B_Joey_T_8.Init))]
        class WeakeningSmogDebuffPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Ldc_I4_S && (sbyte)ci.operand == -4)
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_I4_S, -10);
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }

        }



        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromSavedData))]
        class CustomSkillPatch
        {
            static void Postfix(GDESkillData __instance, ref GameObject ____Particle, ref string ____PathParticle)
            {
                if (__instance.Key == CustomKeys.Skill_Joey_CP_ExtraPot)
                {

                    __instance.ViewBuff = true;
                    __instance.Name = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(
                        GDESchemaKeys.Skill, CustomLoc.StripGuid(CustomKeys.Skill_Joey_CP_ExtraPot), CustomLoc.TermType.Name));
                    __instance.Description = "";

                    __instance.KeyID = "";
                    __instance.User = "";
                    __instance.LucyPartyDraw = "";
                    __instance.PlusSkillView = "";

                    __instance.Target = new GDEs_targettypeData("null");


                    __instance.Effect_Target = new GDESkillEffectData("null");
                    __instance.Effect_Self = new GDESkillEffectData("null");
                    __instance.Category = new GDESkillCategoryData("null");


                    // optional or required?
                    //____PathParticle = "Particle/impact";
                    //____Particle = Resources.Load<GameObject>(____PathParticle);

                    // 2do. add immage
                    //__instance.Image_0 = GDEDataManager.GetUnityObject<Sprite>(GDEItemKeys.Skill_S_Joey_7_1, "Image_0", null, GDESchemaKeys.Skill);
                    //__instance.Image_0 = new GDESkillData(GDEItemKeys.Skill_S_Joey_7_1).Image_0
                    //__instance.Image_1 = ???;


                    // list of SkillExtends which have their own GDE schema data
                    __instance.SKillExtendedItem = new List<GDESkillExtendedData>() { new GDESkillExtendedData(CustomKeys.SkillExtended_Joey_CP_ExtraPot_Ex) };


                    // list of Skill_Extended classes directly associated with the skill. Should NEVER refer to the same classes as the ones in SkillExtendedItem List
                    __instance.SkillExtended = new List<string>();

                    __instance.PlusViewBuffList = new List<GDEBuffData>();
                    __instance.PlusKeyWords = new List<GDESkillKeywordData>();
                }
                else if (__instance.Key == CustomKeys.Skill_Joey_HealingDrone_HealAllies)
                {
                    __instance.Name = "";
                    __instance.Description = "";

                    __instance.KeyID = "";
                    __instance.User = "";
                    __instance.LucyPartyDraw = "";
                    __instance.PlusSkillView = "";

                    __instance.Target = new GDEs_targettypeData(GDEItemKeys.s_targettype_all_ally);


                    __instance.Effect_Target = new GDESkillEffectData(CustomKeys.SkillEffect_Joey_HealingDrone_HealAllies_Effect);
                    __instance.Effect_Self = new GDESkillEffectData("null");
                    __instance.Category = new GDESkillCategoryData("null");

                    ____PathParticle = "Particle/Joey/Joey_4";
                    ____Particle = new GDESkillData(GDEItemKeys.Skill_S_Joey_11).Particle;

                    __instance.SKillExtendedItem = new List<GDESkillExtendedData>();
                    __instance.SkillExtended = new List<string>();
                    __instance.PlusViewBuffList = new List<GDEBuffData>();
                    __instance.PlusKeyWords = new List<GDESkillKeywordData>();

                }
            }
        }

        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromSavedData))]
        class CustomSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance)
            {
                if (__instance.Key == CustomKeys.SkillEffect_Joey_HealingDrone_HealAllies_Effect)
                {
                    __instance.DMG_Per = 0;
                    __instance.DMG_Base = 0;
                    __instance.HEAL_Per = (int)(healingDroneHeal * 100);
                    __instance.HEAL_Base = 0;

                    __instance.Buffs = new List<GDEBuffData>();
                    __instance.CRI = 0;
                    __instance.HIT = 100;
                    __instance.AP = 0;
                    __instance.HEAL_MaxHpPer = 0;
                    __instance.Horror = 0;

                    __instance.BuffPlusTagPer = new List<int>();
                    __instance.ForceHeal = false;
                    __instance.ChainHeal = false;


                }
            }
        }



        [HarmonyPatch(typeof(GDESkillExtendedData), nameof(GDESkillExtendedData.LoadFromSavedData))]
        class CustomSkillExtendedPatch
        {
            static void Postfix(GDESkillExtendedData __instance, ref string ____PathParticle)
            {
                if (__instance.Key == CustomKeys.SkillExtended_Joey_CP_ExtraPot_Ex)
                {

                    __instance.Name = "";
                    __instance.EnforceString = "";
                    __instance.NeedCharacter = "";
                    ____PathParticle = "";

                    __instance.Des = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(
                        GDESchemaKeys.SkillExtended, CustomLoc.StripGuid(CustomKeys.SkillExtended_Joey_CP_ExtraPot_Ex), CustomLoc.TermType.Description));

                    __instance.ClassName = CustomKeys.ClassName_Joey_CP_ExtraPot_Ex;

                }
            }
        }


        [HarmonyPatch(typeof(Extended_Joey_7))]
        class CreatePotionPatch
        {

            [HarmonyPatch(nameof(Extended_Joey_7.SkillUseSingle))]
            [HarmonyTranspiler]

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                bool injected = false;
                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Stfld && ((FieldInfo)ci.operand).Equals(AccessTools.Field(typeof(Extended_Joey_7), "OutPutSkill")) && !injected)
                    {
                        injected = true;
                        // adds our ExtraPot skill to selection of possible effects
                        yield return ci;
                        yield return new CodeInstruction(OpCodes.Ldloc_0);
                        yield return new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(CustomKeys),"Skill_Joey_CP_ExtraPot"));
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Passive_Char), "BChar"));
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Passive_Char), "BChar"));
                        yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(BattleChar), "MyTeam"));
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Skill), "TempSkill", new Type[] { typeof(string), typeof(BattleChar), typeof(BattleTeam)}));
                        yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<Skill>), "Add"));
                    }
                    else 
                    {
                        yield return ci;
                    }
                }
            }



        }

    }
}
