using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Extended_Twins_Selenelion : Skill_Extended
{
    public override void Init()
    {
        base.Init();
        this.PlusSkillStat.Penetration = 100f;
    }
}

