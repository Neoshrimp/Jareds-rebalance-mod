using GameDataEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Extended_Sizz_EveHelp : Skill_Extended
{

    public override string DescExtended(string desc)
    {
        return base.DescExtended(desc.Replace("&c", Math.Max(0, maxCastCount - castCount).ToString()));
    }

    public override void FixedUpdate()
    {
        var eveHolder = BattleSystem.instance.AllyTeam.AliveChars.Find(bc => bc.BuffFind(GDEItemKeys.Buff_P_Sizz_0, false));
        if (eveHolder != null && ((P_Sizz_0)eveHolder.BuffReturn(GDEItemKeys.Buff_P_Sizz_0)).Stack >= 2)
        {
            NotCount = true;
        }
        else
        {
            NotCount = false;
        }
    }

    public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
    {
        if (castCount < maxCastCount)
        {
            Skill skill = Skill.TempSkill(GDEItemKeys.Skill_S_Sizz_0, this.BChar, this.BChar.MyTeam);
            skill.isExcept = true;
            skill.AP = 1;
            skill.AutoDelete = 1;
            skill.NotCount = true;
            var thisExtended = (Extended_Sizz_EveHelp)skill.ExtendedFind(typeof(Extended_Sizz_EveHelp).AssemblyQualifiedName);
            if (thisExtended != null)
                thisExtended.castCount = castCount + 1;
            BattleSystem.instance.AllyTeam.Add(skill, true);
        }

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



    public int castCount = 0;
	int maxCastCount = 3;
}
