using GameDataEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Extended_Trisha_ShadowSlash : Skill_Extended
{	
	public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
	{
		if (Targets[0].HP == Targets[0].GetStat.maxhp)
		{
			Skill skill = Skill.TempSkill(GDEItemKeys.Skill_S_Trisha_0, this.BChar, this.BChar.MyTeam);
			skill.isExcept = true;
			skill.AutoDelete = 1;
			this.BChar.MyTeam.Add(skill, true);
		}
		else if (BattleSystem.instance.EnemyList.Count == 1 && extraCopiesVsSingleTarget < maxRecastsVsSingleTarget)
		{
			Skill skill = Skill.TempSkill(GDEItemKeys.Skill_S_Trisha_0, this.BChar, this.BChar.MyTeam);
			skill.isExcept = true;
			skill.AutoDelete = 1;
			var thisExtended = (Extended_Trisha_ShadowSlash)skill.ExtendedFind(typeof(Extended_Trisha_ShadowSlash).AssemblyQualifiedName);
			if (thisExtended != null)
				thisExtended.extraCopiesVsSingleTarget = extraCopiesVsSingleTarget + 1; 
			this.BChar.MyTeam.Add(skill, true);
		}

		if (BattleSystem.instance.EnemyList.Count == 1)
		{
			Fatal = true;
		}
	}
    public override string DescExtended(string desc)
    {
        return base.DescExtended(desc.Replace("&a", Math.Max(0, maxRecastsVsSingleTarget - extraCopiesVsSingleTarget).ToString()));
    }


    public int extraCopiesVsSingleTarget = 0;
	int maxRecastsVsSingleTarget = 1;
}
