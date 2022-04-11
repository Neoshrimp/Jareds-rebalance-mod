using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class debug_Skill_Ex : Skill_Extended
{

/*    public override string ExtendedDes()
    {
        return this.Data.Des;
    }*/
    public override void SkillUseSingleAfter(Skill SkillD, List<BattleChar> Targets)
    {
        base.SkillUseSingleAfter(SkillD, Targets);
        //Debug.Log("deez: " + Targets[0].Info.KeyData);
        Targets[0].HP = 0;
    }

}
