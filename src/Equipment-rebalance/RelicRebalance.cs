using HarmonyLib;
using System.Runtime.CompilerServices;
using PItem;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Equipment_rebalance
{
    class RelicRebalance
    {

        [HarmonyPatch(typeof(PassiveItemBase), nameof(PassiveItemBase.ShinyEffect))]

        class PIBReversePatch
        {
            [HarmonyReversePatch]
            [MethodImpl(MethodImplOptions.NoInlining)]
            static public void ShinyEffectStub(PassiveItemBase instance) { return; }
        }


        [HarmonyPatch(typeof(ShadowOrb), nameof(ShadowOrb.SKillUseHand_Team))]
        class ShadowOrbPatch
        {
            // this might not work if source of discard is a source of player playing a skill
            static bool Prefix(Skill skill, ShadowOrb __instance)
            {
                int skillInhand = BattleSystem.instance.AllyTeam.Skills.Contains(skill) ? 1 : 0;

                if (BattleSystem.instance.AllyTeam.Skills.Count-skillInhand == 0)
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
