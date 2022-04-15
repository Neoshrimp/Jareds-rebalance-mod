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






        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.Skill_S_LianUnlock)
                {
                    if (SaveManager.IsUnlock(GDEItemKeys.Character_Lian, SaveManager.NowData.unlockList.UnlockCharacter))
                    {
                        __instance.NoDrop = false;
                    }
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
