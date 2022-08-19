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

// 2do neutralize extends rather than replace them

namespace Character_rebalance
{
    [BepInPlugin(GUID, "Jared's rebalance mod", version)]
    [BepInProcess("ChronoArk.exe")]
    public class CharacterRebalancePlugin : BaseUnityPlugin
    {

        public const string GUID = "com.neo.ca.gameplay.charRebalance";
        public const string version = "1.0.0";


        private static readonly Harmony harmony = new Harmony(GUID);

        public static BepInEx.Logging.ManualLogSource logger;


        void Awake()
        {
            CustomLoc.InitLocalizationCSV("Character_rebalance.Resources.localization.csv");
            logger = Logger;
            harmony.PatchAll();
        }
        void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchAll(GUID);
        }


        [HarmonyPatch(typeof(GDEDataManager), nameof(GDEDataManager.BuildDataKeysBySchemaList))]
        class RegisterCustomKeysPatch
        {
            static void Postfix()
            {
                CustomKeys.RegisterCustomKeys();
            }
        }


        [HarmonyPatch(typeof(GDEDataManager), nameof(GDEDataManager.InitFromText))]
        class AddFullAssemblyQualifyingNamePatch
        {

            static void AddFullName()
            {
                void UpdateTypeName(string typeKey, string _gameAssemblyName, Dictionary<string, object> entity)
                {
                    var typeName = (string)entity[typeKey];
                    if (typeName != "" && !typeName.EndsWith(_gameAssemblyName))
                    {
                        entity[typeKey] = string.Concat(typeName, _gameAssemblyName);
                    }
                }

                var gameAssemblyName = typeof(FieldSystem).AssemblyQualifiedName.Remove(0, typeof(FieldSystem).FullName.Length);


                foreach (var kv in GDEDataManager.masterData)
                {
                    var entity = ((Dictionary<string, object>)GDEDataManager.masterData[kv.Key]);
                    if (entity.TryGetString("_gdeSchema", out string schema))
                    {
                        if (schema == GDESchemaKeys.Skill)
                        {
                            var SkillExtenededList = (List<object>)entity["SkillExtended"];
                            int i = 0;
                            foreach (string s in SkillExtenededList)
                            {
                                if (s != "" && !s.EndsWith(gameAssemblyName))
                                {
                                    ((List<object>)entity["SkillExtended"])[i] = string.Concat(s, gameAssemblyName);
                                }
                                i++;
                            }
                        }
                        else if (schema == GDESchemaKeys.SkillExtended)
                        {
                            UpdateTypeName("ClassName", gameAssemblyName, entity);
                        }
                        else if (schema == GDESchemaKeys.Buff)
                        {
                            UpdateTypeName("ClassName", gameAssemblyName, entity);
                        }
                        else if (schema == GDESchemaKeys.Character)
                        {
                            UpdateTypeName("PassiveClassName", gameAssemblyName, entity);
                        }
                        else if (schema == GDESchemaKeys.Enemy)
                        {
                            UpdateTypeName("AI", gameAssemblyName, entity);
                        }
                        else if (schema == GDESchemaKeys.EnemyQueue)
                        {
                            UpdateTypeName("BattleExtended", gameAssemblyName, entity);
                        }
                        else if (schema == GDESchemaKeys.CurseList)
                        {
                            // Curse. hardcoded namespace
                            // Key as type name
                        }
                        else if (schema == GDESchemaKeys.EnchantList)
                        {
                            // Enchent. hardcoded namespace
                            UpdateTypeName("ClassName", gameAssemblyName, entity);
                        }
                        else if (schema == GDESchemaKeys.Item_Active)
                        {
                            // AItem. hardcoded namespace
                            UpdateTypeName("FieldUse", gameAssemblyName, entity);
                        }
                        else if (schema == GDESchemaKeys.Item_Consume)
                        {
                            // UseItem. hardcoded namespace
                            UpdateTypeName("FieldUse", gameAssemblyName, entity);
                        }
                        else if (schema == GDESchemaKeys.Item_Equip)
                        {
                            // EItem. hardcoded namespace
                            UpdateTypeName("Equip_Script", gameAssemblyName, entity);
                        }
                        else if (schema == GDESchemaKeys.Item_Passive)
                        {
                            // PItem. hardcoded namespace
                            UpdateTypeName("passive_script", gameAssemblyName, entity);
                        }
                        else if (schema == GDESchemaKeys.Item_Potions)
                        {
                            // Potions. hardcoded namespace
                            // Key as type name
                        }
                        else if (schema == GDESchemaKeys.Item_Scroll)
                        {
                            // Scrolls. hardcoded namespace
                            // Key as type name
                        }
                        else if (schema == GDESchemaKeys.RandomEvent)
                        {
                            UpdateTypeName("Script", gameAssemblyName, entity);
                        }
                        
                    }

                }
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Call && ((MethodInfo)ci.operand).Equals(AccessTools.Method(typeof(GDEDataManager), nameof(GDEDataManager.BuildDataKeysBySchemaList))))
                    {
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(AddFullAssemblyQualifyingNamePatch), nameof(AddFullAssemblyQualifyingNamePatch.AddFullName)));
                        yield return ci;
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }

        }

        // only Common_B_EnemyTaunt uses this jank af method
        [HarmonyPatch(typeof(BattleChar), nameof(BattleChar.BuffScriptFind))]
        class BuffScriptFindFixPatch
        {
            static bool Prefix(BattleChar __instance, string key, ref bool __result)
            {   
                if (key == "null")
                {
                    __result = false;
                    return false;
                }
                foreach (Buff buff in __instance.Buffs)
                {
                    if ((buff.GetType().Name == key || buff.GetType().AssemblyQualifiedName == key) && !buff.DestroyBuff)
                    {
                        __result = true;
                        return false;
                    }
                }
                __result = false;
                return false;
            }
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
                else if (__instance.Key == CustomKeys.SkillKeyword_Keyword_Critical)
                {
                    __instance.Name = "<b>" + ScriptLocalization.Battle_Keyword.Fatal + "</b>";
                    __instance.Desc = ScriptLocalization.Battle_Keyword.Fatal_Desc;
                }
            }


        }


        [HarmonyPatch(typeof(BattleSystem), nameof(BattleSystem.BattleInit))]
        class AddObserverPatch
        {
            static void Postfix(BattleSystem __instance)
            {
                if (BattleSystem.instance.AllyTeam.LucyChar != null)
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
                    __instance.ClassName = typeof(TurnEventObserver).AssemblyQualifiedName;
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
