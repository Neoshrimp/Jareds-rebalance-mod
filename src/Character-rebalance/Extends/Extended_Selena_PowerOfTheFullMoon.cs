using Character_rebalance;
using GameDataEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Extended_Selena_PowerOfTheFullMoon : Skill_Extended
{
    // basically needed for Somebody's Mask only
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (BattleSystem.instance != null && BChar != null && BChar is BattleAlly battleAlly)
        {
            if (battleAlly.MyBasicSkill?.buttonData?.MySkill.KeyID == MySkill.MySkill.KeyID
                && battleAlly.MyBasicSkill.buttonData.ExtendedFind(typeof(Extended_PowerOfTheFullMoon_APchange).AssemblyQualifiedName) == null)
            {

                battleAlly.MyBasicSkill.buttonData.ExtendedAdd(new Extended_PowerOfTheFullMoon_APchange(-1));
            }
        }
    }

}