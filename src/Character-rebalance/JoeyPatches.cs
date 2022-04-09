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
    class JoeyPatches
    {
        //2do. delete
        public static BepInEx.Logging.ManualLogSource logger = CharacterRebalancePlugin.logger;


        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromSavedData))]
        class ExtraPotSkillPatch
        {
            static void Postfix(GDESkillData __instance, ref GameObject ____Particle, ref string ____PathParticle)
            {
                if (__instance.Key == CustomKeys.Skill_Joey_CP_ExtraPot)
                {
                    __instance.ViewBuff = true;
                    __instance.Name = CustomLocalization.MainFile.GetTranslation(CustomLocalization.TermKey(
                        GDESchemaKeys.Skill, CustomKeys.Skill_Joey_CP_ExtraPot, CustomLocalization.TermType.Name));
                    __instance.Description = "";

                    __instance.Target = new GDEs_targettypeData("null");

                    __instance.Target = new GDEs_targettypeData(GDEItemKeys.s_targettype_ally);

                    __instance.Effect_Target = new GDESkillEffectData("null");
                    __instance.Effect_Self = new GDESkillEffectData("null");
                    __instance.Category = new GDESkillCategoryData("null");


                    // optional or required?
                    /*                    ____PathParticle = "Particle/impact";
                                        ____Particle = Resources.Load<GameObject>(____PathParticle);*/


                    __instance.SKillExtendedItem = new List<GDESkillExtendedData>() { new GDESkillExtendedData(CustomKeys.SkillExtended_Joey_CP_ExtraPot_Ex) };

                    // is actually a list of GDESkillExtendedData.ClassName
                    __instance.SkillExtended = new List<string>();
                    __instance.SKillExtendedItem.ForEach(se => __instance.SkillExtended.Add(se.ClassName));

                    __instance.PlusViewBuffList = new List<GDEBuffData>();
                    __instance.PlusKeyWords = new List<GDESkillKeywordData>();
                }
            }
        }

        [HarmonyPatch(typeof(GDESkillExtendedData), nameof(GDESkillData.LoadFromSavedData))]
        class ExtraPotEXtendedSkillPatch
        {
            static void Postfix(GDESkillExtendedData __instance)
            {
                if (__instance.Key == CustomKeys.SkillExtended_Joey_CP_ExtraPot_Ex)
                {

                    //__instance.Image = null;

                    // 2do make description add itself
                    // maybe it's a buff description?
                    __instance.Des = "deeznuts";

                    __instance.ClassName = CustomKeys.ClassName_Joey_CP_ExtraPot_Ex;

                    


                }
            }

        }


        // 2do. use transpiler
        [HarmonyPatch(typeof(Extended_Joey_7))]
        class CreatePotionPatch
        {

            [HarmonyPatch(nameof(Extended_Joey_7.SkillUseSingle))]
            [HarmonyPrefix]
            static bool SkillUseSinglePrefix(Extended_Joey_7 __instance, Skill SkillD, List<BattleChar> Targets, ref Skill ___OutPutSkill)
            {

                
                List<Skill> list = new List<Skill>();
                ___OutPutSkill = Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_Final, __instance.BChar, __instance.BChar.MyTeam);

                list.Add(Skill.TempSkill(CustomKeys.Skill_Joey_CP_ExtraPot, __instance.BChar, __instance.BChar.MyTeam));

                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_0, __instance.BChar, __instance.BChar.MyTeam));
                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_1, __instance.BChar, __instance.BChar.MyTeam));
/*                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_2, __instance.BChar, __instance.BChar.MyTeam));
                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_3, __instance.BChar, __instance.BChar.MyTeam));
                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_4, __instance.BChar, __instance.BChar.MyTeam));
                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_5, __instance.BChar, __instance.BChar.MyTeam));
                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_6, __instance.BChar, __instance.BChar.MyTeam));
                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_7, __instance.BChar, __instance.BChar.MyTeam));
                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_8, __instance.BChar, __instance.BChar.MyTeam));
                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_9, __instance.BChar, __instance.BChar.MyTeam));*/
                for (int i = 0; i < 2; i++)
                {
                    BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(list.Random(3), new SkillButton.SkillClickDel(__instance.Del), ScriptLocalization.System_SkillSelect.Joey_7, false, false, true, false, true));
                }
                return false;
            }
        }

    }
}
