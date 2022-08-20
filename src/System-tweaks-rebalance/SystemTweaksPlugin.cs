using BepInEx;
using BepInEx.Configuration;
using GameDataEditor;
using HarmonyLib;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace System_tweaks_rebalance
{
    [BepInPlugin(GUID, "System tweaks rebalance", version)]
    [BepInProcess("ChronoArk.exe")]
    public class SystemTweaksPlugin : BaseUnityPlugin
    {

        public const string GUID = "neo.ca.gameplay.systemTweaks";
        public const string version = "1.0.0";


        public static readonly Harmony harmony = new Harmony(GUID);

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



        static string ProcessTooltip(string tooltip)
        {
            return new Regex("(?!\n).*?98.*?\n").Replace(tooltip, "");
        }


        [HarmonyPatch]
        class DifficultySelectTooltipPatch
        {
            static MethodBase TargetMethod()
            {
                return AccessTools.PropertyGetter(typeof(ScriptLocalization.UI_SelectParty), "OriginDesc");
            }

            static void Postfix(ref string __result)
            {
                __result = ProcessTooltip(__result);
            }
        }


        // game needs to be reloaded for patch to take effect 
        [HarmonyPatch(typeof(PartyInventory), "Awake")]
        class ExpertIconPatch
        {
            static void Postfix(PartyInventory __instance)
            {
                if (__instance.ExpertIcon != null)
                {
                    SimpleTooltip tooltip = __instance.ExpertIcon.GetComponent<SimpleTooltip>();
                    if (tooltip != null)
                    {
                        tooltip.TooltipString = ProcessTooltip(tooltip.ToolTipString_l2.ToString());
                        tooltip.ToolTipString_l2 = null;
                    }

                }
            }
        }



        // actual logic
        [HarmonyPatch(typeof(BattleChar), nameof(BattleChar.HitPerNum))]
        class HitcapPatch
        {

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                int changeCount = 0;
                int changeLim = 4;

                foreach (CodeInstruction i in instructions)
                {
                    //maybe there's a better way for detecting this
                    if (i.opcode == OpCodes.Ldc_R4 && (float)i.operand == 98f && changeCount < changeLim)
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_R4, 100f);
                        changeCount += 1;
                    }
                    else
                    {
                        yield return i;
                    }
                }
            }
        }


        [HarmonyPatch(typeof(FieldSystem), nameof(FieldSystem.StageStart))]
        class FieldSystem_Patch
        {
            [HarmonyPostfix]
            static void AddStartingItems()
            {
                if (PlayData.TSavedata.StageNum == 0 && !PlayData.TSavedata.IsLoaded)
                {
                    GDEDataManager.GetAllDataKeysBySchema(GDESchemaKeys.Item_Scroll, out List<string> allScrollKeys);
                    foreach (var key in allScrollKeys)
                        if (PlayData.TSavedata.IdentifyItems.Find(k => k == key) == null)
                            PlayData.TSavedata.IdentifyItems.Add(key);


                }
            }

            static IEnumerable<CodeInstruction> AddItemsTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Callvirt, AccessTools.Method(typeof(UIManager), "FadeSquare_In")))
                    {
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(FieldSystem_Patch), nameof(FieldSystem_Patch.AddStartingItems)));
                        yield return ci;
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }
        }


        [HarmonyPatch(typeof(Item_Potions), nameof(Item_Potions.InputInfo))]
        class Item_Potions_Patch
        {
            static AccessTools.FieldRef<ItemBase, int> maxStackRef = AccessTools.FieldRefAccess<ItemBase, int>("maxstack");

            static void Postfix(Item_Potions __instance)
            {
                maxStackRef(__instance) = 3;
            }
        }



        [HarmonyPatch(typeof(PlayData), nameof(PlayData.init))]
        class DrawUpgradeCost_Patch
        {

            static void Postfix()
            {
                PlayData.DrawUpgradeNum[0] = 1; 
            }
        }
         

        [HarmonyPatch(typeof(B_S4_King_P2_P), nameof(B_S4_King_P2_P.FixedUpdate))]
        class TFKLeaveBasics_Patch
        {

            static bool BasicCheck(bool prevCheck, BattleAlly ba)
            {
                return prevCheck && !ba.MyBasicSkill.buttonData.MySkill.Basic;
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {

                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Call, AccessTools.Method(typeof(string), "op_Inequality", new System.Type[] { typeof(string), typeof(string) })))
                    {
                        yield return ci;
                        yield return new CodeInstruction(OpCodes.Ldloc_0);

                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(TFKLeaveBasics_Patch), nameof(TFKLeaveBasics_Patch.BasicCheck)));

                    }
                    else
                    {
                        yield return ci;
                    }

                }
            }

        }





    }
}
