using GameDataEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Character_rebalance.Extends
{
    public class Extended_Phoenix_FieryWings: SkillExtended_Phoenix_3

    {

        public override string DescExtended(string desc)
        {
			return desc.Replace("&h", ((int)Misc.PerToNum(BChar.GetStat.atk, 40)).ToString());
		}

        public override void SkillUseSingleAfter(Skill SkillD, List<BattleChar> Targets)
		{
			BattleSystem.DelayInputAfter(CheckIfDead(Targets));
		}

		public IEnumerator CheckIfDead(List<BattleChar> Targets)
		{
			if (!Targets[0].IsDead)
			{
				using (List<BattleEnemy>.Enumerator enumerator = BattleSystem.instance.EnemyList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BattleEnemy i = enumerator.Current;
						if (Targets.Find((BattleChar a) => a == i) == null)
						{
							i.Heal(this.BChar, 25f, false, false, null);
						}
					}
				}
			}
			else
			{
				foreach (var a in BattleSystem.instance.AllyTeam.AliveChars)
				{
					a.Heal(BChar, Misc.PerToNum(BChar.GetStat.atk, 40), false, false, null);

				}
			}


			yield break;
		}
	}
}
