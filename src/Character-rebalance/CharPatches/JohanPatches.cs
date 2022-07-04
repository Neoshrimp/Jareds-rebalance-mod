using Character_rebalance.Extends;
using GameDataEditor;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;

namespace Character_rebalance.CharPatches
{
    class JohanPatches
    {

        // Modify gdata.json
        [HarmonyPatch(typeof(GDEDataManager), nameof(GDEDataManager.InitFromText))]
        class GdataJsonPatch
        {
            static private void GdataModification()
            {


                // Rain of Arrows: Scaling 33% -> 55%
                (GDEDataManager.masterData["SE_Mement_8_T"] as Dictionary<string, object>)["DMG_Per"] = 55;

                // Sonic Blow: Added Swiftness
                (GDEDataManager.masterData["S_Mement_3"] as Dictionary<string, object>)["NotCount"] = true;

                // accuracy +4%
                ((GDEDataManager.masterData[GDEItemKeys.Character_Mement] as Dictionary<string, object>)["HIT"] as Dictionary<string, object>)["x"] = 100;
                ((GDEDataManager.masterData[GDEItemKeys.Character_Mement] as Dictionary<string, object>)["HIT"] as Dictionary<string, object>)["y"] = 119;

                // evade +5%
                ((GDEDataManager.masterData[GDEItemKeys.Character_Mement] as Dictionary<string, object>)["CRI"] as Dictionary<string, object>)["x"] = 5;
                ((GDEDataManager.masterData[GDEItemKeys.Character_Mement] as Dictionary<string, object>)["CRI"] as Dictionary<string, object>)["y"] = 15;

                // crit +5%
                ((GDEDataManager.masterData[GDEItemKeys.Character_Mement] as Dictionary<string, object>)["DODGE"] as Dictionary<string, object>)["x"] = 5;
                ((GDEDataManager.masterData[GDEItemKeys.Character_Mement] as Dictionary<string, object>)["DODGE"] as Dictionary<string, object>)["y"] = 15;


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

        static float imitateDmgReduction = 65f;

        [HarmonyPatch(typeof(GDESkillExtendedData), nameof(GDESkillExtendedData.LoadFromDict))]
        class GDESkillExtendedPatch
        {
            static void Postfix(GDESkillExtendedData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.SkillExtended_Mement_4_Ex)
                {
                    dict.TryGetString("Des", out string ogDesc);
                    __instance.Des = ogDesc.Replace("80%", imitateDmgReduction.ToString() + "%");
                }
            }
        }

        //Imitate Buff: -80% -> -50%, change description
        [HarmonyPatch(typeof(S_Mement_4))]
        class Imitate_Patch
        {
            [HarmonyPatch(nameof(S_Mement_4.SkillUseSingle))]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Ldc_R4, 80f))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_R4, imitateDmgReduction);
                    }
                    else
                    {
                        yield return ci;
                    }
                }
            }
        }

        //Change Imitate Desc
        [HarmonyPatch(typeof(Skill_Extended))]
        class ImitateDesc_Patch
        {
            [HarmonyPatch(nameof(Skill_Extended.DescExtended))]
            [HarmonyPostfix]
            static void DescExtendedPostfix(ref string __result, Skill_Extended __instance, string desc)
            {
                if (__instance is S_Mement_4)
                {
                    __result = desc.Replace("80%", imitateDmgReduction.ToString() + "%");
                }
            }
        }

    }
}
