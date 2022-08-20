using GameDataEditor;
using UnityEngine;
using static Equipment_rebalance.EquipRebalance;

namespace EItem
{
    public class Extended_ForbiddenBible_Item : EquipBase, IP_BattleStart_Ones //ForbiddenLibram
    {

		public int currentHealBonus = 0;
		public bool postiveHeal = true;
		public Vector2 lowRange = new Vector2(-88, -20+1);
		public Vector2 highRange = new Vector2(20, 142+1);


		public override void Init()
		{
			this.PlusStat.cri = 5f;
			this.PlusPerStat.Heal = 10;
			this.PlusStat.HIT_CC = -40f;
			base.Init();
		}

        public override string DescExtended(string desc)
        {
			return $"Healing skills in hand get <i>random</i> {(int)lowRange.x}%~{(int)highRange.y-1}% healing bonus. \n\n<color=#919191>There are patterns I can feel it. - Hein</color>";

		}

        public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (BattleSystem.instance != null)
			{
				foreach (Skill skill in BattleSystem.instance.AllyTeam.Skills)
				{
					if (skill.Master == this.BChar && skill.IsHeal && skill.ExtendedFind("Extended_ForbiddenLibram", true) == null)
					{
						skill.ExtendedAdd(Skill_Extended.DataToExtended(GDEItemKeys.SkillExtended_ForbiddenLibram_Ex));
					}
				}
			}
		}

		public void BattleStart(BattleSystem Ins)
        {
            postiveHeal = true;
            RollHeal();
        }
		public void RollHeal()
		{
			if (postiveHeal)
			{
				currentHealBonus = Random.Range((int)highRange.x, (int)highRange.y);
				postiveHeal = !postiveHeal;
			}
			else
			{
				currentHealBonus = Random.Range((int)lowRange.x, (int)lowRange.y);
				postiveHeal = !postiveHeal;
			}
			Debug.Log($"roll: {currentHealBonus}");
		}

	}
}