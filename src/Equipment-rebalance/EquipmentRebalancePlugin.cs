using BepInEx;
using EItem;
using HarmonyLib;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;
using System.Reflection.Emit;
using GameDataEditor;
using System.Reflection;

namespace Equipment_rebalance
{
    [BepInPlugin(GUID, "Equipment rebalance", version)]
    [BepInProcess("ChronoArk.exe")]
    public class EquipmentRebalancePlugin : BaseUnityPlugin
    {

        public const string GUID = "org.who.chronoark.gameplay.eqrebalance";
        public const string version = "1.0.0";


        private static readonly Harmony harmony = new Harmony(GUID);

        public static BepInEx.Logging.ManualLogSource logger;

        void Awake()
        {
            logger = Logger;
            harmony.PatchAll();
        }
        void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchAll(GUID);
        }






        /// <summary>
        /// filthy code dupe but hopefully harmless
        /// actually unnecessary in current ca version for some reason
        /// </summary>

        //[HarmonyPatch(typeof(GDEDataManager), nameof(GDEDataManager.InitFromText))]
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


    }
}
