using BepInEx.Bootstrap;
using GameDataEditor;
using SwiftnessRework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Extended_Sizz_EveHelp : Skill_Extended
{

    public override void Init()
    {
        base.Init();
        if (Chainloader.PluginInfos.ContainsKey("neo.ca.gameplay.swiftnessRework"))
            quickPlugin = true;
    }

    public override string DescExtended(string desc)
    {
        var r = base.DescExtended(desc.Replace("&c", Math.Max(0, maxCastCount - castCount).ToString()));
        if (quickPlugin)
            r = r.Replace("<b>Swiftness</b>", "<b>Effortless</b> and <b>Quick</b>");
        return r;
    }

    public override void FixedUpdate()
    {
        var eveHolder = BattleSystem.instance.AllyTeam.AliveChars.Find(bc => bc.BuffFind(GDEItemKeys.Buff_P_Sizz_0, false));
        if (eveHolder != null && ((P_Sizz_0)eveHolder.BuffReturn(GDEItemKeys.Buff_P_Sizz_0)).Stack >= 2)
        {
            NotCount = true;
            if (quickPlugin)
                SwiftnessReworkPlugin.quickManager.SetVal(this, true);
        }
        else
        {
            NotCount = false;
            if (quickPlugin)
                SwiftnessReworkPlugin.quickManager.SetVal(this, false);
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
            if(quickPlugin)
                SwiftnessReworkPlugin.quickManager.SetVal(skill, true);
            

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


    bool quickPlugin = false;
    public int castCount = 0;
	public int maxCastCount = 3;
}
