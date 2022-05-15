using Character_rebalance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ExtendedExtra_Ironheart_InnocentArmor : Skill_Extended
{
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (BattleSystem.instance != null && BChar != null && BChar is BattleAlly battleAlly)
        {
            if (battleAlly.MyBasicSkill?.buttonData?.MySkill.KeyID == MySkill.MySkill.KeyID
                && battleAlly.MyBasicSkill.buttonData.ExtendedFind(typeof(Extended_InnocentArmor_APchange).AssemblyQualifiedName) == null)
            {

                battleAlly.MyBasicSkill.buttonData.ExtendedAdd(new Extended_InnocentArmor_APchange(-1));
            }
        }
    }
}
