using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Character_rebalance.Extends
{
    public class Extended_Sizz_PatchUpBuff : B_Sizz_3_T
    {
        public override void BuffStat()
        {
            base.BuffStat();
            this.PlusStat.RES_DEBUFF = (float)(10 * base.StackNum);
            this.PlusStat.RES_DOT = (float)(10 * base.StackNum);
            this.PlusStat.RES_CC = (float)(10 * base.StackNum);
        }
    }
}
