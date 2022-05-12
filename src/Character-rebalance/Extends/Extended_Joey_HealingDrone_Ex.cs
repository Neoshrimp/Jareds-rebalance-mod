using Character_rebalance;
using GameDataEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Extended_Joey_HealingDrone_Ex : Skill_Extended
{
	public string hitCountLoc = "";

/*	public override string DescExtended(string desc)

	{
		string finalDesc = base.DescExtended(desc);
		finalDesc = finalDesc.Replace("&h", ((int)(this.BChar.GetStat.reg * JoeyPatches.healingDroneHeal)).ToString());
		finalDesc = finalDesc.Replace("&d", ((int)(this.BChar.GetStat.reg * JoeyPatches.healingDroneDmg)).ToString());

		return finalDesc;
	}
*/
    public override void Special_PointerEnter(BattleChar Char)
    {
        base.Special_PointerEnter(Char);
		
		UIextras.ViewTextTooltip(Char, hitCountLoc + (hitsFromBufffs() + hitsFromTarget(Char)).ToString());
    }

    public override void Init()
    {
        base.Init();
		hitCountLoc = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Joey_11, CustomLoc.TermType.Tooltip));
	}


    public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
	{
		var healTargets = new List<BattleChar>();
		foreach (BattleAlly battleAlly in BattleSystem.instance.AllyList)
		{
			healTargets.Add((BattleChar)battleAlly);
		}
		Skill healSkill = Skill.TempSkill(CustomKeys.Skill_Joey_HealingDrone_HealAllies, this.BChar, this.BChar.MyTeam);
		healSkill.isExcept = true;
		healSkill.FreeUse = true;
		this.BChar.ParticleOut(this.MySkill, healSkill, healTargets);
		
		int num = hitsFromBufffs()+hitsFromTarget(Targets[0]);
		
		for (int i = 0; i < num; i++)
		{
			BattleSystem.DelayInputAfter(this.Effect(Targets[0]));
		}

		
	}
	int hitsFromBufffs()
	{
		if (BattleSystem.instance == null)
			return 0;
		int num = 0;
		foreach (BattleAlly battleAlly in BattleSystem.instance.AllyList)
		{
			num += battleAlly.GetBuffs(BattleChar.GETBUFFTYPE.BUFF, false, false).Count;
		}
		//counts Lucy's buffs as well
		num += BattleSystem.instance.AllyTeam.LucyChar.GetBuffs(BattleChar.GETBUFFTYPE.BUFF, false, false).Count;
		num = (int)Math.Ceiling(num / 2f);
		return num;
	}

	int hitsFromTarget(BattleChar target)
	{
		if (target == null)
			return 0;
		return target.GetBuffs(BattleChar.GETBUFFTYPE.ALLDEBUFF, false, false).Count;
	}

	public IEnumerator Effect(BattleChar target)
	{

		//yield return new WaitForSecondsRealtime(0.1f);
		yield return new WaitForSeconds(0.1f);

		// ok so problem probably was double adding same skill extend
		Skill damageSkill = Skill.TempSkill(GDEItemKeys.Skill_S_Joey_11_0, this.BChar, this.BChar.MyTeam);

		damageSkill.FreeUse = true;
		damageSkill.PlusHit = true;


        if (!target.IsDead)
        {
			this.BChar.ParticleOut(this.MySkill, damageSkill, target);
		}
		else
        {
			this.BChar.ParticleOut(this.MySkill, damageSkill, BattleSystem.instance.EnemyTeam.AliveChars.Random<BattleChar>());
        }


	}

}

