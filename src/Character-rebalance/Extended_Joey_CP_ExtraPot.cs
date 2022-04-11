using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Character_rebalance;
using GameDataEditor;
using HarmonyLib;
using Debug = UnityEngine.Debug;


// custom Extends should be without namespace. Or AssemblyQualifying name could be use I guess?
public class Extended_Joey_CP_ExtraPot : Skill_Extended
{
    public override void Init()
    {
        base.Init();
        this.APChange = -1;
    }



    public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
    {
        base.SkillUseSingle(SkillD, Targets);
        this.MySkill.Master.MyTeam.Draw(1);
        // index out of range exception at PartyInvetory.Update
        this.MySkill.Master.MyTeam.MaxPotionNum += 1;

    }


}
