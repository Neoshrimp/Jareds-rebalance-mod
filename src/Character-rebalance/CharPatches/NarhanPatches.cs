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

namespace Character_rebalance.CharPatches
{
    public class NarhanPatches
    {



        [HarmonyPatch(typeof(GDECharacterData), nameof(GDECharacterData.LoadFromDict))]
        class GdeCharactersPatch
        {
            static void Postfix(GDECharacterData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.Character_Control)
                {
                    dict.TryGetVector2("ATK", out Vector2 ogATK);
                    __instance.ATK = new Vector2(14, ogATK.y);
                    dict.TryGetVector2("MAXHP", out Vector2 ogMAXHP);
                    __instance.MAXHP = new Vector2(27, ogMAXHP.y);
                    dict.TryGetVector2("HIT_CC", out Vector2 ogHIT_CC);
                    __instance.HIT_CC = new Vector2(20, ogHIT_CC.y);

                }
            }
        }



        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {

            }
        }


        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GdeSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance)
            {
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_Control_0_T)
                {
                    __instance.DMG_Per = 45;
                }
            }
        }


        [HarmonyPatch(typeof(S_Control_0), nameof(S_Control_0.SkillUseSingle))]
        class NightmareSyndromePatch
        {

            static void Prefix(List<BattleChar> Targets)
            {
                Targets.ForEach(t => Debug.Log(t.name));

            }
        }


    }
}
