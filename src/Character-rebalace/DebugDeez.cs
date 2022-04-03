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

    }
}
