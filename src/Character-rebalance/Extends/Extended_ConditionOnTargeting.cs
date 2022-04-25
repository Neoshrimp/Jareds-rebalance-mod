using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


// 2do. test with pharos leader
public class Extended_ConditionOnTargeting : Skill_Extended
{
    public void SwiftnessUpdateManaCrytalUI()
    {
        if (this.MySkill.AP >= 1 && BattleSystem.instance != null)
        {
            for (int i = this.MySkill.MyTeam.AP - MySkill.AP; i > this.MySkill.MyTeam.AP - this.MySkill.AP - MySkill.Master.Overload; i--)
            {
                BattleSystem.instance.ActWindow.APGroup.transform.GetChild(i - 1).GetComponent<Animator>().SetBool("Using", false);
            }
        }
    }

    public void ResetManaCrystalUI()
    {
        if (this.MySkill.AP >= 1 && BattleSystem.instance != null)
        {
            for (int i = this.MySkill.MyTeam.AP; i > this.MySkill.MyTeam.AP - this.MySkill.AP; i--)
            {
                BattleSystem.instance.ActWindow.APGroup.transform.GetChild(i - 1).GetComponent<Animator>().SetBool("Using", true);
            }
        }
    }

}

