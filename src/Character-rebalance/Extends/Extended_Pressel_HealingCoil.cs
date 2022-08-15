using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Character_rebalance.Extends
{
    public class Extended_Pressel_HealingCoil : Extended_Priest_0
    {

        public override string DescExtended(string desc)
        {
            return desc.Replace("&h", ((int)(BChar.GetStat.reg * 0.25f)).ToString());
        }

        public override void FixedUpdate()
        {
            if (BattleSystem.instance != null && BChar != null && BChar is BattleAlly battleAlly)
            {
                if (battleAlly.MyBasicSkill?.buttonData?.MySkill.KeyID == MySkill.MySkill.KeyID
                    && battleAlly.MyBasicSkill.buttonData.ExtendedFind(typeof(Extended_HealingCoil_FixChange).AssemblyQualifiedName) == null)
                {

                    battleAlly.MyBasicSkill.buttonData.ExtendedAdd(new Extended_HealingCoil_FixChange());
                }
            }
            base.FixedUpdate();
        }
    }
}
