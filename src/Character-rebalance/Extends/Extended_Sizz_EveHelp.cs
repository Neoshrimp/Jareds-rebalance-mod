using GameDataEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Extended_Sizz_EveHelp : Skill_Extended
{

/*    public override string DescExtended(string desc)
    {
        return base.DescExtended(desc.Replace("&c", Math.Max(0, maxCastCount-castCount).ToString()));
    }*/

	//whyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy

	public override void Init()
	{
		base.Init();
	}

	public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
	{
		Skill skill = Skill.TempSkill(GDEItemKeys.Skill_S_Sizz_0, this.BChar, this.BChar.MyTeam);
		skill.isExcept = true;
		skill.AP = 1;
		skill.NotCount = true;
		skill.AutoDelete = 1;
		BattleSystem.instance.AllyTeam.Add(skill, true);
		foreach (BattleChar battleChar in this.BChar.MyTeam.AliveChars)
		{
			if (battleChar != Targets[0] && battleChar.BuffFind(GDEItemKeys.Buff_B_Sizz_0_T, false))
			{
				for (int i = 0; i < battleChar.BuffReturn(GDEItemKeys.Buff_B_Sizz_0_T, false).StackNum; i++)
				{
					Targets[0].BuffAdd(GDEItemKeys.Buff_B_Sizz_0_T, this.BChar, false, 0, false, -1, false);
				}
				battleChar.BuffReturn(GDEItemKeys.Buff_B_Sizz_0_T, false).SelfDestroy();
			}
		}
	}
	/*public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
    {
		if (castCount < maxCastCount)
		{
			Skill skill = Skill.TempSkill(GDEItemKeys.Skill_S_Sizz_0, this.BChar, this.BChar.MyTeam);
			skill.isExcept = true;
			skill.AP = 1;
			skill.AutoDelete = 1;
			var thisExtended = (Extended_Sizz_EveHelp)skill.ExtendedFind(nameof(Extended_Sizz_EveHelp));
			if(thisExtended != null)
				thisExtended.castCount = castCount + 1;
			BattleSystem.instance.AllyTeam.Add(skill, true);
		}
		Targets[0].BuffReturn(GDEItemKeys.Buff_B_Sizz_0_T, false)?.SelfDestroy();
		BattleSystem.DelayInput(Coroutine(Targets[0]));
*//*		foreach (BattleChar battleChar in this.BChar.MyTeam.AliveChars)
        {
            if (battleChar != Targets[0] && battleChar.BuffFind(GDEItemKeys.Buff_B_Sizz_0_T, false))
            {
                for (int i = 0; i < battleChar.BuffReturn(GDEItemKeys.Buff_B_Sizz_0_T, false).StackNum+1; i++)
                {
                    Targets[0].BuffAdd(GDEItemKeys.Buff_B_Sizz_0_T, this.BChar, false, 0, false, -1, false);
                }
                battleChar.BuffReturn(GDEItemKeys.Buff_B_Sizz_0_T, false).SelfDestroy();
            }
        }*//*
    }*/

	public IEnumerator Coroutine(BattleChar target)
	{
		//yield return new WaitForSeconds(0.2f);
		foreach (BattleChar battleChar in this.BChar.MyTeam.AliveChars)
		{
			if (battleChar != target && battleChar.BuffFind(GDEItemKeys.Buff_B_Sizz_0_T, false))
			{
				for (int i = 0; i < battleChar.BuffReturn(GDEItemKeys.Buff_B_Sizz_0_T, false).StackNum+1; i++)
				{
					target.BuffAdd(GDEItemKeys.Buff_B_Sizz_0_T, this.BChar, false, 0, false, -1, false);
				}
				battleChar.BuffReturn(GDEItemKeys.Buff_B_Sizz_0_T, false).SelfDestroy();
			}
		}
		yield break;
	}

    public int castCount = 0;
	int maxCastCount = 3;
}
