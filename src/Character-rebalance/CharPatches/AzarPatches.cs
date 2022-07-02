using BepInEx;
using BepInEx.Configuration;
using GameDataEditor;
using HarmonyLib;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using TileTypes;
using System.Reflection.Emit;
using System.Reflection;
using I2.Loc;

namespace Character_rebalance
{
    [BepInPlugin(GUID, "Azar Patch", version)]
    [BepInProcess("ChronoArk.exe")]
    public class RareSkillsPlugin : BaseUnityPlugin
    {
        public const string GUID = "org.windy.chronoark.cardmod.skillbalance";
        public const string version = "1.1.0";
        private static readonly Harmony harmony = new Harmony(GUID);

        void Awake()
        {
            harmony.PatchAll();
        }
        void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchAll(GUID);
        }

        // modify gdata.json
        [HarmonyPatch(typeof(GDEDataManager), nameof(GDEDataManager.InitFromText))]
        class GdataJsonPatch
        {

            // dont change method signature or traspiler will break
            static void GdataModification()
            {
                // Accuracy +2%
                ((GDEDataManager.masterData["Azar"] as Dictionary<string, object>)["HIT"] as Dictionary<string, object>)["x"] = 98;
                ((GDEDataManager.masterData["Azar"] as Dictionary<string, object>)["HIT"] as Dictionary<string, object>)["y"] = 117;

                // Illusion Sword: Damage 40% -> 45%
                (GDEDataManager.masterData["SE_Azar_T_P"] as Dictionary<string, object>)["DMG_Per"] = 45;

                // Crescent Slash: Damage 135 -> 150%, Crit +50%
                (GDEDataManager.masterData["SE_Azar_10_T"] as Dictionary<string, object>)["DMG_Per"] = 150;
                (GDEDataManager.masterData["SE_Azar_10_T"] as Dictionary<string, object>)["CRI"] = 50;

                // Shining Aura : Cost 1 -> 0
                (GDEDataManager.masterData["S_Azar_11"] as Dictionary<string, object>)["UseAp"] = 0;

                // Illusion Flash : Given Tracking
                (GDEDataManager.masterData["S_Azar_3"] as Dictionary<string, object>)["Track"] = true;

                // Storming Blade : Given Tracking
                (GDEDataManager.masterData["S_Azar_5"] as Dictionary<string, object>)["Track"] = true;

                // Veiled Sword: Cost 1 -> 0, 2 -> 3 stacks, Given Basic
                (GDEDataManager.masterData["S_Azar_4"] as Dictionary<string, object>)["UseAp"] = 0;
                (GDEDataManager.masterData["S_Azar_4"] as Dictionary<string, object>)["Basic"] = true;

                // First Scroll: Damage 95 -> 105%
                (GDEDataManager.masterData["SE_Azar_T_0"] as Dictionary<string, object>)["DMG_Per"] = 105;

                // Illusion Sword's Calling: Cost 1 -> 0, Basic
                (GDEDataManager.masterData["S_Azar_13"] as Dictionary<string, object>)["UseAp"] = 0;
                (GDEDataManager.masterData["S_Azar_13"] as Dictionary<string, object>)["Basic"] = true;

                // Sword of Infinity: Cost 2 -> 1
                (GDEDataManager.masterData["S_Azar_9"] as Dictionary<string, object>)["UseAp"] = 1;

                // Dancing Sword: Given Swiftness
                (GDEDataManager.masterData["S_Azar_12"] as Dictionary<string, object>)["NotCount"] = true;

                // Blade Starfall: Countdown 2 -> 1
                (GDEDataManager.masterData["S_Azar_2"] as Dictionary<string, object>)["Counting"] = 1;
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var newInsts = new List<CodeInstruction>();
                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Call && ((MethodInfo)ci.operand).Equals(AccessTools.Method(typeof(GDEDataManager), nameof(GDEDataManager.BuildDataKeysBySchemaList))))
                    {
                        newInsts.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GdataJsonPatch), nameof(GdataJsonPatch.GdataModification))));
                        newInsts.Add(ci);
                    }
                    else
                    {
                        newInsts.Add(ci);
                    }
                }
                return newInsts.AsEnumerable();
            }

        }

        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                // Fantasy : exclude after 2 uses
                if (__instance.Key == GDEItemKeys.Skill_S_Azar_7)
                {
                    List<GDESkillExtendedData> list = new List<GDESkillExtendedData>();
                    GDESkillExtendedData a = new GDESkillExtendedData("Trisha_5_Ex");
                    a.Des = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.SkillExtended, GDEItemKeys.Skill_S_Azar_7, CustomLoc.TermType.Description));
                    a.Name = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.SkillExtended, GDEItemKeys.Skill_S_Azar_7, CustomLoc.TermType.Name));

                    list.Add(a);
                    __instance.SKillExtendedItem = list;
                }
            }
        }

        [HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
        class GdeSkillEffectPatch
        {
            static void Postfix(GDESkillEffectData __instance)
            {

                // Veiled Sword : 2 -> 3 stacks
                if (__instance.Key == GDEItemKeys.SkillEffect_SE_Azar_T_4)
                {
                    __instance.Buffs = new List<GDEBuffData>() { new GDEBuffData(GDEItemKeys.Buff_B_Azar_T_4), new GDEBuffData(GDEItemKeys.Buff_B_Azar_T_4), new GDEBuffData(GDEItemKeys.Buff_B_Azar_T_4) };
                }
            }
        }

        //Fantasy - Exclude after 2 uses
        [HarmonyPatch(typeof(Trisha_5_Ex))]
        class Exclude_Patch
        {


            [HarmonyPatch(nameof(Trisha_5_Ex.FixedUpdate))]
            [HarmonyPrefix]
            static bool Prefix(Trisha_5_Ex __instance)
            {
                __instance.BuffIconStackNum = __instance.UseNum;
                // if fantasy
                if (__instance.MySkill.MySkill.KeyID == GDEItemKeys.Skill_S_Azar_7)
                {
                    if (__instance.UseNum == 0)
                    {
                        __instance.MySkill.Disposable = true;
                        //Debug.Log("Here");
                    }
                }
                // if shadow curtain
                else
                {
                    if (__instance.UseNum == 1)
                    {
                        __instance.MySkill.Disposable = true;
                    }
                }
                return false;
            }

            [HarmonyPatch(nameof(Trisha_5_Ex.Init))]
            [HarmonyPostfix]
            static void Postfix2(Trisha_5_Ex __instance)
            {
                // if fantasy
                if (__instance.MySkill.MySkill.KeyID == GDEItemKeys.Skill_S_Azar_7)
                {
                    __instance.UseNum = 2;
                }
            }
        }

    }
}