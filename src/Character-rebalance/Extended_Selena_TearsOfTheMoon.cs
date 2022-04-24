using GameDataEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Extended_Selena_TearsOfTheMoon : Skill_Extended
{

	public override void Init()
	{
		base.Init();
		this.SkillParticleObject = new GDESkillExtendedData(GDEItemKeys.SkillExtended_Public_10_Ex).Particle;
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		bool critCondition = false;

		foreach (BattleChar battleChar in BattleSystem.instance.EnemyTeam.AliveChars)
		{
			if (battleChar.BuffFind(GDEItemKeys.Buff_B_TW_Red_3_T, false))
			{
				PlusSkillStat.cri = 100;
				critCondition = true;
				break;
			}
		}

		if (critCondition)
			SkillParticleOn();
		else
			SkillParticleOff();
	}


}

