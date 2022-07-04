using GameDataEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Character_rebalance.Extends
{
    public class Extended_Chain_Zoom : MissChainPBase
    {
        public override void FireOn()
        {
            base.FireOn();
            this.IgnoreTaunt = true;
            base.SkillBuffAdd(new GDEBuffData(GDEItemKeys.Buff_B_MissChain_0_T), true, 0);

        }

        public override void FireOff()
        {
            base.FireOff();
            this.IgnoreTaunt = false;
            TargetBuff.Clear();

        }
    }
}
