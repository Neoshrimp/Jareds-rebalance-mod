using GameDataEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Character_rebalance.Extends
{
    public class Extended_Silverstein_FreezeBomb : Skill_Extended
    {
        public override void Special_PointerEnter(BattleChar Char)
        {
            base.Special_PointerEnter(Char);

            if (Char != null && Char.BuffFind(GDEItemKeys.Buff_B_SilverStein_P_1))
            {
                NotCount = true;
                SkillParticleOn();
                ConditionOnTargetingUtils.SwiftnessUpdateManaCrytalUI(MySkill.AP, MySkill.Master.Overload);
            }
            else
            {
                NotCount = false;
                SkillParticleOff();
                ConditionOnTargetingUtils.ResetManaCrystalUI(MySkill.AP);
            }
        }

        public override void Special_PointerExit()
        {
            base.Special_PointerExit();
            NotCount = false;
            SkillParticleOff();

        }

        public override void Init()
        {
            base.Init();
            this.SkillParticleObject = new GDESkillExtendedData(GDEItemKeys.SkillExtended_Public_10_Ex).Particle;

        }

    }
}
