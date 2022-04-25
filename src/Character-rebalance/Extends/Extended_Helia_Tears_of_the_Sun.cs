using Character_rebalance;
using GameDataEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// 2do. maybe fix bug with multiple Tears of the same kind
public class Extended_Helia_Tears_of_the_Sun : S_TW_Red_R0
{


    public override void Init()
    {
        base.Init();
        this.SkillParticleObject = new GDESkillExtendedData(GDEItemKeys.SkillExtended_Public_10_Ex).Particle;

    }

    public override void FixedUpdate()
    {
        if (MySkill.MyButton != null)
        {
            if (!MySkill.MyButton.AlreadyWasted && !flag2)
            {
                var skills = BattleSystem.instance.AllyTeam.Skills;
                int sun_i = skills.FindIndex(s => s == MySkill);
                int moon_i = -1;
                if (sun_i != -1)
                {
                    if (skills[Math.Max(sun_i - 1, 0)].MySkill.KeyID == GDEItemKeys.Skill_S_TW_Blue_R0)
                    {
                        moon_i = Math.Max(sun_i - 1, 0);
                    }
                    else if (skills[Math.Min(sun_i + 1, skills.Count - 1)].MySkill.KeyID == GDEItemKeys.Skill_S_TW_Blue_R0)
                    {
                        moon_i = Math.Min(sun_i + 1, skills.Count - 1);
                    }

                    if (moon_i != -1)
                    {
                        flag2 = true;
                        BattleSystem.DelayInput(Effect(sun_i, moon_i));
                    }
                }
            }
        }


        if (BChar.BuffFind(GDEItemKeys.Buff_B_TW_Blue_2_T, false))
        {

            this.PlusSkillStat.cri = 100;
            SkillParticleOn();
        }
        else
        {
            this.PlusSkillStat.cri = 0;
            SkillParticleOff();
        }
    }

}


