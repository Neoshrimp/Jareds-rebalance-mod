using GameDataEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Character_rebalance.Extends
{
    public class Extended_Narhan_MentalShock : S_Control_5
    {

        public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
        {
            this.PlusSkillStat.cri = 0f;
            if (Targets[0].BuffFind(GDEItemKeys.Buff_B_Control_P, false))
            {
                this.PlusSkillStat.cri = 100f;
            }
            this.SkillBasePlus.Target_BaseDMG = (int)((float)this.BChar.GetStat.maxhp * 0.35f);
        }

		
        public override void SkillUseSingleAfter(Skill SkillD, List<BattleChar> Targets)
        {
            Targets[0].BuffAdd(GDEItemKeys.Buff_B_Control_P, BChar, false, 0, false, -1, false);
		}

        public override void SkillKill(SkillParticle SP)
        {
			List<BattleEnemy> list = new List<BattleEnemy>();
			foreach (BattleEnemy battleEnemy in BattleSystem.instance.EnemyList)
			{
				if (battleEnemy.SkillQueue.Count != 0)
				{
					list.Add(battleEnemy);
				}
			}
			if (list.Count != 0)
			{
				list.Random<BattleEnemy>().BuffAdd(GDEItemKeys.Buff_B_Control_P, BChar, false, 0, false, -1, false);
			}
			else if (BattleSystem.instance.EnemyTeam.AliveChars.Count != 0)
			{
				BattleSystem.instance.EnemyTeam.AliveChars.Random<BattleChar>().BuffAdd(GDEItemKeys.Buff_B_Control_P, BChar, false, 0, false, -1, false);
			}
		}

    }
}
