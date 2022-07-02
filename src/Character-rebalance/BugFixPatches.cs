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
    public class BugFixPatches
    {
        [HarmonyPatch(typeof(PartyInventory), "Update")]
        class PotionSpriteIndexOutOfRangeFix
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {

                var list = instructions.ToList();


                for (int i=0; i < list.Count; i++)
                {
                    if (list[i].opcode == OpCodes.Sub && i + 1 < list.Count && list[i + 1].opcode == OpCodes.Callvirt 
                        && ((MethodInfo)list[i + 1].operand).Equals(AccessTools.Method(typeof(List<Sprite>), "get_Item")))
                    {
                        // makes sure index doesn't go out of bounds
                        yield return list[i];
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PartyInventory), "BattlePotionSprites"));
                        yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<Sprite>), "get_Count"));
                        yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                        yield return new CodeInstruction(OpCodes.Sub);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Math), "Min", new Type[] { typeof(int), typeof(int) }));
                    }
                    else
                    {
                        yield return list[i];
                    }

                }
            }
        }


        [HarmonyPatch(typeof(B_ShadowPriest_8_T), nameof(B_ShadowPriest_8_T.SelfdestroyPlus))]
        class AbosrbSoulBugfixPatch
        {
            static bool Prefix(B_ShadowPriest_8_T __instance)
            {
                if (__instance.MainThurible != null)
                {
                    GameObject gameObject = Misc.UIInst(__instance.BChar.BattleInfo.EffectView);
                    if (__instance.BChar.Info.Ally)
                    {
                        gameObject.transform.position = __instance.BChar.GetPos();
                    }
                    else
                    {
                        gameObject.transform.position = __instance.BChar.GetTopPos();
                    }
                    gameObject.GetComponent<EffectView>().TextOut(__instance.BChar.Info.Ally, ScriptLocalization.CharText_ShadowPriest.Charge);
                }
                return false;
            }
        }


        [HarmonyPatch(typeof(Buff), nameof(Buff.StackDestroy))]
        class AbosrbSoulBugfixBuffPatch
        {
            static void Prefix(Buff __instance)
            {
                if (__instance is B_ShadowPriest_8_T absSoul)
                {
                    if (absSoul.MainThurible != null)
                        absSoul.MainThurible.ChargeNow++;
                }
            }

        }

        // 2do double patching
        [HarmonyPatch(typeof(P_Sizz_0), nameof(P_Sizz_0.EveAttackFree))]
        class InciseBuffDmgPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Ldc_I4_S, 40))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_I4_S, 25);
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
