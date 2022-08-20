using HarmonyLib;
using System.Runtime.CompilerServices;
using PItem;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;
using System.Reflection.Emit;
using System.Reflection;
using System;
using PItem.Equipment_rebalance;
using EItem;

namespace Equipment_rebalance
{
    public class ItemDataPatches
    {

        // does not modify masterdata
        [HarmonyPatch]
        class ItemDescription_Patch
        {

            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.PropertyGetter(typeof(ItemBase), nameof(ItemBase.GetDescription));
            }
                

            static AccessTools.FieldRef<ItemBase, Dictionary<string, object>> dicRef = AccessTools.FieldRefAccess<ItemBase, Dictionary<string, object>>("KeyDictionary");

            static void Postfix(ItemBase __instance, ref string __result)
            {

                var data = dicRef(__instance);
                var key = __instance.itemkey;
                if (data == null)
                    __result = "";
                // relics
                if (key == GDEItemKeys.Item_Passive_Memoryfragment)
                {
                    __result = ((string)data["Description"]).Replace(" 2 ", " 1 ");
                }
                else if (key == GDEItemKeys.Item_Passive_WeatherVane)
                {
                    __result = ((string)data["Description"]).Replace(" 6 ", " 8 ");
                }
                else if (key == GDEItemKeys.Item_Passive_ManaBattery)
                {
                    __result = ((string)data["Description"]).Split('\n')[0];
                }
                // potions
                else if (key == GDEItemKeys.Item_Potions_Potion_Clone)
                {
                    __result = ((string)data["Description"]).Replace(" 2", " 1");
                }
                else if (key == GDEItemKeys.Item_Potions_Potion_Fairy)
                {
                    __result = "Display 5 regular skills belonging to any of the investigators. Select one skill and cast it on a random target.";
                }
                // items
                else if (key == GDEItemKeys.Item_Equip_EndlessScroll)
                {
                    __result = data["Description"] + " \nAny enchantments are applied <b>twice</b>. ";
                }
                else if (key == GDEItemKeys.Item_Equip_CrownofThorns)
                {
                    __result = data["Description"] + " \nGain 25% <b>dodge</b> bonus while at Death's Door.";
                }
                else if (key == GDEItemKeys.Item_Equip_FoxOrb)
                {
                    __result = ((string)data["Description"]).Replace("15%", "13%");

                }
            }
        }

        [HarmonyPatch(typeof(GDEDataManager), nameof(GDEDataManager.InitFromText))]
        class GDEMasterData_Patch
        {

            static void ModifyData()
            {

                var entity = ((Dictionary<string, object>)GDEDataManager.masterData[GDEItemKeys.Item_Passive_WeatherVane]);
                // fckn hardcoded name spaces
                entity["passive_script"] = typeof(Extended_Pinwheel).AssemblyQualifiedName.Substring("PItem.".Length);

                entity = ((Dictionary<string, object>)GDEDataManager.masterData[GDEItemKeys.Item_Equip_ForbiddenLibram]);
                entity["Equip_Script"] = typeof(Extended_ForbiddenBible_Item).AssemblyQualifiedName.Substring("EItem.".Length);


            }


            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Call && ((MethodInfo)ci.operand).Equals(AccessTools.Method(typeof(GDEDataManager), nameof(GDEDataManager.BuildDataKeysBySchemaList))))
                    {
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GDEMasterData_Patch), nameof(GDEMasterData_Patch.ModifyData)));
                        yield return ci;
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }

        }



        [HarmonyPatch(typeof(GDESkillExtendedData), nameof(GDESkillExtendedData.LoadFromDict))]
        class GDESkillExtendedData_Patch
        {
            static void Postfix(GDESkillExtendedData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.SkillExtended_ForbiddenLibram_Ex)
                {
                    __instance.Des = "Additive heal bonus \n-88%~142%";
                }
            }
        }



        // because of course item code is fucking special and works with masterdata directly
        // modifies masterdata
        //[HarmonyPatch(typeof(ItemBase), nameof(ItemBase.GetItem), new Type[] { typeof(string) })]
        /*        class ItemData_Patch
                {

                    static AccessTools.FieldRef<ItemBase, Dictionary<string, object>> dicRef = AccessTools.FieldRefAccess<ItemBase, Dictionary<string, object>>("KeyDictionary");

                    static void ChangeItemData(string key, Dictionary<string, object> data)
                    {
                        if (data == null)
                            return;
                        // relics
                        if (key == GDEItemKeys.Item_Passive_Memoryfragment)
                        {
                            data["Description"] = ((string)data["Description"]).Replace(" 2 ", " 1 ");
                        }
                        else if (key == GDEItemKeys.Item_Passive_WeatherVane)
                        {
                            data["Description"] = ((string)data["Description"]).Replace(" 6 ", " 8 ");
                        }
                        else if (key == GDEItemKeys.Item_Passive_ManaBattery)
                        {
                            data["Description"] = ((string)data["Description"]).Split('\n')[0];
                        }
                        // potions
                        else if (key == GDEItemKeys.Item_Potions_Potion_Clone)
                        {
                            data["Description"] = ((string)data["Description"]).Replace(" 2", " 1");
                        }
                        else if (key == GDEItemKeys.Item_Potions_Potion_Fairy)
                        {
                            data["Description"] = "Display 5 regular skills belonging to any of the investigators. Select one skill and cast it on a random target.";
                        }
                        // items
                        else if (key == GDEItemKeys.Item_Equip_EndlessScroll)
                        {
                            Debug.Log("endless deeznuts");
                            data["Description"] = data["Description"] + " deeznuts";
                        }

                    }


                    static void Postfix(string key, ref ItemBase __result)
                    {
                        ChangeItemData(key, dicRef(__result));
                    }

                }*/




    }
}
