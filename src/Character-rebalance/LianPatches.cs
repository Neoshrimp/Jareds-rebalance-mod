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
    class LianPatches
    {


        [HarmonyPatch(typeof(GDECharacterData), nameof(GDECharacterData.LoadFromDict))]
        class GdeCharactersPatch
        {
            static void Postfix(GDECharacterData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.Character_Lian)
                {
                    dict.TryGetVector2("ATK", out Vector2 ogATK);
                    __instance.ATK = new Vector2(15, ogATK.y);
                }
            }
        }




        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // Parry Attack
                if (__instance.Key == GDEItemKeys.Skill_S_LianUnlock)
                {
                    if (SaveManager.IsUnlock(GDEItemKeys.Character_Lian, SaveManager.NowData.unlockList.UnlockCharacter))
                    {
                        __instance.NoDrop = false;
                    }
                }
                // stunning smite
                else if (__instance.Key == GDEItemKeys.Skill_S_Lian_7)
                {
                    __instance.NotCount = false;
                    __instance.Counting = 3;
                }
                // dropdown slash
                else if (__instance.Key == GDEItemKeys.Skill_S_Lian_0)
                {
                    __instance.Track = true;
                    __instance.Effect_Self = new GDESkillEffectData(GDEItemKeys.SkillEffect_SE_Priest_4_0_T);
                }
                // relentless swipe
                else if (__instance.Key == GDEItemKeys.Skill_S_Lian_6)
                {
                    __instance.UseAp = 2;
                    __instance.SkillExtended = new List<string>() { CustomKeys.ClassName_Lian_RelentlessSwipe_Ex };
                    __instance.Description = CustomLoc.MainFile.GetTranslation(
                        CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Lian_6, CustomLoc.TermType.Description));
                }
                // bring it on
                else if (__instance.Key == GDEItemKeys.Skill_S_Lian_3)
                {
                    __instance.UseAp = 1;
                }
                // combat roar
                else if (__instance.Key == GDEItemKeys.Skill_S_Lian_4)
                {
                    __instance.UseAp = 0;
                    __instance.NotCount = true;
                }

            }
        }

        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GdeSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance)
            {
                // dropdown slash
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_Lian_0_T)
                {
                    __instance.DMG_Per = 175;
                }
            }
        }




        // in theory should add Parry Attack to skill pool on the run where Lian is unlocked but NOT tested
        [HarmonyPatch(typeof(B_Lian_P_0), nameof(B_Lian_P_0.Counter), new Type[] { typeof(BattleChar), typeof(SkillParticle), typeof(CastingSkill) })]
        class LianUnlockPatch
        {
            static void Postfix(CastingSkill CastingSkill)
            {
                if (CastingSkill.skill.MySkill.KeyID == GDEItemKeys.Skill_S_LianUnlock)
                {
                    if (SaveManager.IsUnlock(GDEItemKeys.Character_Lian, SaveManager.NowData.unlockList.UnlockCharacter))
                    {
                        SaveManager.NowData.LianUnlockNum = 0;
                    }
                    if (SaveManager.NowData.LianUnlockNum >= 10)
                    {
                        PlayData._ALLSKILLLIST.Add(new GDESkillData(GDEItemKeys.Skill_S_LianUnlock));
                    }
                }
            }
        }


    }
}
