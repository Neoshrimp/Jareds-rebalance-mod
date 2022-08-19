using HarmonyLib;
using System.Runtime.CompilerServices;
using PItem;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;
using System.Reflection.Emit;
using System;

namespace Equipment_rebalance
{
    public class PotionRebalace
    {



        [HarmonyPatch(typeof(GDEItem_PotionsData), nameof(GDEItem_PotionsData.LoadFromDict))]
        class GDEItem_PotionsData_Patch
        {
            static void Postfix(GDEItem_PotionsData __instance, Dictionary<string, object> dict)
            {
                if (__instance.Key == GDEItemKeys.Item_Potions_Potion_Clone)
                {
                    dict.TryGetString("Description", out string ogDesc);
                    __instance.Description = ogDesc.Replace(" 2", " 1");
                }
                else if (__instance.Key == GDEItemKeys.Item_Potions_Potion_Fairy)
                {
                    __instance.Description = "Display 5 regular skills belonging to any of the investigators. Select one skill and cast it on a random target.";
                }
                
            }
        }


        [HarmonyPatch(typeof(S_Potion_Clone), nameof(S_Potion_Clone.SkillTargetSingle))]
        class S_Potion_Clone_Patch
        {
            static bool Prefix(List<Skill> Targets)
            {
                BattleSystem.instance.AllyTeam.Add(Targets[0].CloneSkill(true, Targets[0].Master, null, false), true);
                return false;

            }
        }


        [HarmonyPatch(typeof(S_Potion_Fairy), nameof(S_Potion_Fairy.SkillUseSingle))]
        class FairyPotion_Patch
        {

            static void PickSkill2Cast(BattleChar caster, List<GDESkillData> skillPool)
            {

                var skills2select = skillPool.Random(5).ConvertAll<Skill>(sd => Skill.TempSkill(sd.Key, caster, caster.MyTeam));
                BattleSystem.DelayInput(BattleSystem.I_OtherSkillSelect(skills2select, new SkillButton.SkillClickDel(FairyPotion_Patch.SkillSelect), 
                    "select deeznuts"));
            }

            public static void SkillSelect(SkillButton Mybutton)
            {
                Mybutton.Myskill.FreeUse = true;
                BattleSystem.DelayInput(BattleSystem.instance.SkillRandomUseIenum(Mybutton.Myskill.Master, Mybutton.Myskill, false, false, false));
            }



            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
            {
                bool skillListInited = false;
                foreach (var ci in instructions)
                {
                    if (ci.Is(OpCodes.Callvirt, AccessTools.Method(typeof(List<BattleChar>), "get_Item", new Type[] { typeof(int) })))
                    {
                        skillListInited = true;

                        yield return ci;
                        yield return new CodeInstruction(OpCodes.Ldloc_0);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(FairyPotion_Patch), nameof(FairyPotion_Patch.PickSkill2Cast)));

                    }
                    else if(!skillListInited)
                    {
                        yield return ci;
                    }
                }
                yield return new CodeInstruction(OpCodes.Ret);
            }

        }







    }
}
