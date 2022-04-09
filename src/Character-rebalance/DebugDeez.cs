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
    class DebugDeez
    {
        public static BepInEx.Logging.ManualLogSource logger = CharacterRebalancePlugin.logger;

        //[HarmonyPatch]
        class dd
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.PropertyGetter(typeof(LocalizationManager), "CurrentLanguage");
            }

            static void Postfix()
            {
                var stack = new StackTrace();
                var list = new List<StackFrame>(stack.GetFrames());
                if (list.Find(f => f.GetMethod().Name == "Update") != null)
                    return;
                CharacterRebalancePlugin.logger.LogInfo("Getter");
                list.ForEach(f => logger.LogInfo(f.GetMethod()));
                logger.LogInfo("-------------------");
            }
        }


        //[HarmonyPatch]
        class dd2
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.PropertySetter(typeof(LocalizationManager), "CurrentLanguage");
            }

            static void Postfix()
            {
                logger.LogInfo("Setter");
                var stack = new StackTrace();
                new List<StackFrame>(stack.GetFrames()).ForEach(f => logger.LogInfo(f.GetMethod()));
                logger.LogInfo("-------------------");
            }
        }

        //[HarmonyPatch]
        class dd3
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(GDEDataManager), "LocalizeUpdate");
                yield return AccessTools.Method(typeof(GDEDataManager), "LocalizeArrayUpdate");

            }

            static void Postfix(string Key, string ValueName, Dictionary<string, object> LocalizeText)
            {
                Debug.Log(string.Concat(new object[]
            {
                LocalizeText["_gdeSchema"],
                "/",
                Key,
                "_",
                ValueName
            }));
            }
        }


        //[HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                
                if (__instance.Key == GDEItemKeys.Skill_S_Joey_7_1 || __instance.Key == CustomKeys.Skill_Joey_CP_ExtraPot)
                {
                    logger.LogInfo($"{__instance.Key}");
                    var newT = Traverse.Create(__instance);
                    newT.Fields().ForEach(f => {
                        var fval = newT.Field(f).GetValue();
/*                        if (fval.GetType().BaseType.Equals(typeof(IGDEData)))
                        {
                            logger.LogInfo($"{f} = {AccessTools.Field(fval.GetType(), "Key")?.GetValue(fval)}");
                        }
                        else*/
                        {
                            logger.LogInfo($"{f} = {fval}");
                        }
                    });
                }
            }
        }


        //[HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromSavedData))]
        class dd5
        {
            static void Postfix(GDESkillData __instance)
            {

                if (__instance.Key == CustomKeys.Skill_Joey_CP_ExtraPot)
                {
                    logger.LogInfo($"{__instance.Key}");
                    var newT = Traverse.Create(__instance);
                    newT.Fields().ForEach(f => {
                        var fval = newT.Field(f).GetValue();
/*                        if (fval.GetType().BaseType.Equals(typeof(IGDEData)))
                        {
                            logger.LogInfo($"{f} = {AccessTools.Field(fval.GetType(), "Key").GetValue(fval)}");
                        }
                        else*/
                        { 
                            logger.LogInfo($"{f} = {fval}");
                        }
                    });


                }
            }

/*            void NestedPrint(object fval)
            {
                
                logger.LogInfo($"{f} = ");
                if (fval.GetType().BaseType == typeof(IGDEData))
                {
                    logger.LogInfo($"key = {AccessTools.Field(fval.GetType(), "Key").GetValue(fval)}");
                }
                if (fval.GetType().BaseType == typeof(List<>))
                {
                    foreach (var v in new List(fval))
                    {
                        
                    }
                }
            }*/

        }


        //[HarmonyPatch(typeof(GDESkillExtendedData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillEXPatch
        {
            static void Postfix(GDESkillExtendedData __instance, Dictionary<string, object> dict)
            {

                if (__instance.Key == GDEItemKeys.SkillExtended_Joey_7_1_Ex)
                {
                    logger.LogInfo($"{__instance.Key}");
                    var newT = Traverse.Create(__instance);
                    newT.Fields().ForEach(f => {
                        var fval = newT.Field(f).GetValue();
                        {
                            logger.LogInfo($"{f} = {fval}");
                        }
                    });
                }
            }
        }




    }
}
