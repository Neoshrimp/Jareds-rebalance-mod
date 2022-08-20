using BepInEx;
using BepInEx.Configuration;
using GameDataEditor;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Enemy_rebalance
{
    [BepInPlugin(GUID, "Enemy rebalance tweaks", version)]
    [BepInProcess("ChronoArk.exe")]
    public class EnemyRebalancePlugin : BaseUnityPlugin
    {

        public const string GUID = "neo.ca.gameplay.enemyRebalance";
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


        [HarmonyPatch(typeof(GDEEnemyData), nameof(GDEEnemyData.LoadFromDict))]
        class GDEEnemyData_Patch
        {
            static void Postfix(GDEEnemyData __instance, Dictionary<string, object> dict)
            {

                dict.TryGetInt("maxhp", out int ogmaxhp);
                __instance.maxhp = (int)(ogmaxhp * 1.2f);

                
            }
        }



    }
}
