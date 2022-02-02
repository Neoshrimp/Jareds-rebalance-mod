using HarmonyLib;
using System.Runtime.CompilerServices;
using PItem;
using System.Collections;
using UnityEngine;


namespace Equipment_rebalance
{
    class RelicRebalance
    {

        [HarmonyPatch(typeof(PassiveItemBase), nameof(ShadowOrb.ShinyEffect))]

        class PIBReversePatch
        {
            [HarmonyReversePatch]
            [MethodImpl(MethodImplOptions.NoInlining)]
            static public void ShinyEffectStub(PassiveItemBase instance) { return; }
        }


        [HarmonyPatch(typeof(ShadowOrb), nameof(ShadowOrb.SKillUseHand_Team))]
        class ShadowOrbPatch
        {
            static bool Prefix(Skill skill, ShadowOrb __instance)
            {
                if (BattleSystem.instance.AllyTeam.Skills.Count <= 1)
                {
                    // perhaps BattleSystem.DelayInput?
                    BattleSystem.DelayInputAfter(DrawWhenEmpty(__instance));
                }
                return false;
            }

            static public IEnumerator DrawWhenEmpty(ShadowOrb instance)
            {
                if (BattleSystem.instance.AllyTeam.Skills.Count == 0)
                {
                    PIBReversePatch.ShinyEffectStub(instance);
                    BattleSystem.instance.AllyTeam.Draw();
                }
                yield return null;
                yield break;
            }
        }


    }
}
