using BepInEx;
using BepInEx.Configuration;
using GameDataEditor;
using HarmonyLib;
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



    }
}
