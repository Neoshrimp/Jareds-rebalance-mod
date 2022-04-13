﻿using Character_rebalance;
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

	public override string DescExtended(string desc)

	{
		string finalDesc = base.DescExtended(desc);
		finalDesc = finalDesc.Replace("&h", ((int)(this.BChar.GetStat.reg * JoeyPatches.healingDroneHeal)).ToString());
		finalDesc = finalDesc.Replace("&d", ((int)(this.BChar.GetStat.reg * JoeyPatches.healingDroneDmg)).ToString());
/*		int buffHits = hitsFromBufffs();
		finalDesc = finalDesc.Replace("&a", buffHits >= 0 ? buffHits.ToString() : "?");
		int debuffHits = hitsFromTarget(descTarget);
		finalDesc = finalDesc.Replace("&b", debuffHits >= 0 ? debuffHits.ToString() : "?");
		if (buffHits >= 0 && debuffHits >= 0)
			finalDesc = finalDesc.Replace("&c", ((int)(this.BChar.GetStat.reg * JoeyPatches.healingDroneDmg) * (buffHits + debuffHits)).ToString());
		else
			finalDesc = finalDesc.Replace("&c", "?");*/

		return finalDesc;
	}

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
		BattleSystem.DelayInputAfter(this.Effect(Targets[0], num));
		
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

	public IEnumerator Effect(BattleChar target, int num)
	{

		for (int i = 0; i < num; i++)
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.08f, 0.1f));

			// some fck gymnastics to make sure skill targets enemies after death
			Skill damageSkill = new Skill();
			damageSkill.Init(new GDESkillData(GDEItemKeys.Skill_S_Joey_11_0), this.BChar, this.BChar.MyTeam);
			damageSkill.ExtendedAdd(new Extended_Joey_11_0());

			damageSkill.isExcept = true;
			damageSkill.FreeUse = true;
			damageSkill.PlusHit = true;

			if (target.IsDead && BattleSystem.instance.EnemyTeam.AliveChars.Count != 0)
			{
				this.BChar.ParticleOut(this.MySkill, damageSkill, BattleSystem.instance.EnemyTeam.AliveChars.Random<BattleChar>());
			}
			else
			{
				this.BChar.ParticleOut(this.MySkill, damageSkill, target);
			}
		}
	}

}
