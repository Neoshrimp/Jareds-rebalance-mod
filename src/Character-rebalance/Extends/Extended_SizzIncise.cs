using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDataEditor;
using UnityEngine;

namespace Character_rebalance.Extends
{
    public class Extended_SizzIncise : Skill_Extended
    {

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {

            base.SkillUseSingle(SkillD, Targets);
            if (BattleSystem.instance != null)
            {
                /*                var eveHolder = BattleSystem.instance.AllyTeam.AliveChars.FindAll(bc => bc.BuffFind(GDEItemKeys.Buff_P_Sizz_0, false) 
                                || bc.BuffFind(GDEItemKeys.Buff_B_Sizz_10_T, false));*/
                var eh = BChar;
                P_Sizz_0 eve = (P_Sizz_0)eh.BuffReturn(GDEItemKeys.Buff_P_Sizz_0, false);
                if(eve == null)
                    eve = (B_Sizz_10)eh.BuffReturn(GDEItemKeys.Buff_B_Sizz_10_T, false);
                if (eve != null)
                {
                    Skill skill = new Skill();
                    skill.Init(new GDESkillData(GDEItemKeys.Skill_S_Sizz_P), eve.Usestate_L, this.BChar.MyTeam);
                    skill.PlusHit = true;
                    skill.ExtendedAdd(new Extended_Sizz_P());
                    Skill_Extended skill_Extended = new Skill_Extended();
                    skill_Extended.PlusSkillPerStat.Damage = 25; // incise dmg bonus
                    skill.ExtendedAdd(skill_Extended);

                    if (this.BChar.BuffFind(GDEItemKeys.Buff_B_Sizz_0_T, false))
                    {
                        Skill_Extended skill_Extended3 = new Skill_Extended();
                        skill_Extended3.PlusSkillPerStat.Damage = skill_Extended3.PlusSkillPerStat.Damage + this.BChar.BuffReturn(GDEItemKeys.Buff_B_Sizz_0_T, false).StackNum * 20;
                        skill.ExtendedAdd(skill_Extended3);
                    }

                    BattleSystem.DelayInput(eve.EveAttack(skill, Targets[0]));
                }
                        

            }
        }
    }
}
