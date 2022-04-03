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
        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromSavedData))]
        class ExtraPotSkillPatch
        {
            static void Postfix(GDESkillData __instance)
            {
                if (__instance.Key == CustomGdeKeys.Skill_Joey_CP_ExtraPot)
                {
                    __instance.ViewBuff = true;
                    __instance.Name = CustomLocalization.MainFile.GetTranslation(CustomLocalization.TermKey(GDESchemaKeys.Skill, CustomGdeKeys.Skill_Joey_CP_ExtraPot, CustomLocalization.TermType.Name));
                }


            }
        }

        [HarmonyPatch(typeof(Extended_Joey_7))]
        class CreatePotionPatch
        {

            [HarmonyPatch(nameof(Extended_Joey_7.SkillUseSingle))]
            [HarmonyPrefix]
            static bool SkillUseSinglePrefix(Extended_Joey_7 __instance, Skill SkillD, List<BattleChar> Targets, ref Skill ___OutPutSkill)
            {

                //var deez = Skill.TempSkill(CustomGdeKeys.Skill_Joey_CP_ExtraPot, __instance.BChar, __instance.BChar.MyTeam);
                var whatever = new GDESkillData(CustomGdeKeys.Skill_Joey_CP_ExtraPot);
                List<Skill> list = new List<Skill>();
                ___OutPutSkill = Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_Final, __instance.BChar, __instance.BChar.MyTeam);
                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_0, __instance.BChar, __instance.BChar.MyTeam));
                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_1, __instance.BChar, __instance.BChar.MyTeam));
                //list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_2, __instance.BChar, __instance.BChar.MyTeam));
                /*                list.Add(Skill.TempSkill(GDEItemKeys.Skill_S_Joey_7_3, __instance.BChar, __instance.BChar.MyTeam));
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
