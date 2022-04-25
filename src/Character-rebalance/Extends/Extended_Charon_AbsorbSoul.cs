using Character_rebalance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Extended_Charon_AbsorbSoul : Skill_Extended
{
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (BattleSystem.instance != null && BChar != null && BChar is BattleAlly battleAlly)
        {
            if (battleAlly.MyBasicSkill?.buttonData?.MySkill.KeyID == MySkill.MySkill.KeyID
                && battleAlly.MyBasicSkill.buttonData.ExtendedFind(nameof(Extended_AbsorbSoul_APchange)) == null)
            {
                battleAlly.MyBasicSkill.buttonData.ExtendedAdd(new Extended_AbsorbSoul_APchange(-1));
            }
        }




    }
}

