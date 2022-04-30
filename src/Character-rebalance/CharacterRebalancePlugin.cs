using BepInEx;
using BepInEx.Configuration;
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
using System.Runtime.CompilerServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Character_rebalance
{
    [BepInPlugin(GUID, "", version)]
    [BepInProcess("ChronoArk.exe")]
    public class CharacterRebalancePlugin : BaseUnityPlugin
    {

        public const string GUID = "com.neo.ca.gameplay.charRebalance";
        public const string version = "1.0.0";


        private static readonly Harmony harmony = new Harmony(GUID);

        public static BepInEx.Logging.ManualLogSource logger;


        void Awake()
        {
            CustomLoc.InitLocalizationCSV();
            logger = Logger;
            harmony.PatchAll();
        }
        void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchAll(GUID);
        }

        [HarmonyPatch(typeof(GDESkillKeywordData), nameof(GDESkillKeywordData.LoadFromSavedData))]
        class CustomKeywordTooltips
        {
            static void Postfix(GDESkillKeywordData __instance)
            {
                if (__instance.Key == CustomKeys.SkillKeyword_Keyword_Swiftness)
                {
                    __instance.Name = "<b>" + ScriptLocalization.Battle_Keyword.Quick + "</b>";
                    if (SaveManager.NowData.GameOptions.Difficulty == 1)
                    {
                        __instance.Desc = ScriptLocalization.Battle_Keyword.Quick_Desc_Casual;
                    }
                    else
                    {
                        __instance.Desc = ScriptLocalization.Battle_Keyword.Quick_Desc;
                    }

                }
            }


        }


        [HarmonyPatch(typeof(BattleSystem), nameof(BattleSystem.BattleInit))]
        class AddObserverPatch
        {
            static void Postfix(BattleSystem __instance)
            {
                if(BattleSystem.instance.AllyTeam.LucyChar != null)
                {
                    BattleSystem.instance.AllyTeam.LucyChar.BuffAdd(CustomKeys.Buff_Lucy_TurnEventObserver, BattleSystem.instance.AllyTeam.LucyChar, true, 0, false, -1, false);
                }
            }
        }

        // removes black bar under Lucy's portrait if she doesn't have any visible buffs
        [HarmonyPatch(typeof(LucyM), "Update")]
        class BattleLucyUIPatch
        {
            static void Postfix(LucyM __instance)
            {
                if (BattleSystem.instance?.AllyTeam?.LucyChar != null)
                {
                    if ((BattleSystem.instance.AllyTeam.LucyChar.GetBuffs(BattleChar.GETBUFFTYPE.ALL, false, false)).Count == 0)
                    {
                        __instance.Ani.SetBool("On", false);
                    }
                }
            }
        }


        [HarmonyPatch(typeof(GDEBuffData), nameof(GDEBuffData.LoadFromSavedData))]
        class CreateTurnEventObserverBuffPatch
        {
            static void Postfix(GDEBuffData __instance, ref string ____PathAllyBuffEffect, ref string ____PathEnemyBuffEffect)
            {
                if (__instance.Key == CustomKeys.Buff_Lucy_TurnEventObserver)
                {
                    __instance.ClassName = CustomKeys.ClassName_TurnEventObserver_Buff;
                    __instance.Cantdisable = true;
                    __instance.Hide = true;

                    __instance.MaxStack = 1;

                    __instance.BuffSoundEffect = "";
                    __instance.Icon = new Texture2D(1, 1);

                    __instance.Name = "Turn Event Observer";
                    __instance.Description = "";

                    ____PathAllyBuffEffect = "";
                    ____PathEnemyBuffEffect = "";


                    __instance.Tick = new GDESkillEffectData("null");
                    __instance.BuffTag = new GDEBuffTagData("null");

                }
            }
        }


    }
}
