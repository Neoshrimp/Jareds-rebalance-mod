namespace Character_rebalance.Extends
{
    public class Extended_HealingCoil_FixChange : Skill_Extended
    {

        public override string DescExtended(string desc)
        {
            return desc.Replace("&h", (BChar.GetStat.reg * 0.25f).ToString());
        }
        public override void AttackEffectSingle(BattleChar hit, SkillParticle SP, int DMG, int Heal)
        {
            if (DMG >= 1)
            {
                this.BChar.Heal(this.BChar, (float)DMG * 0.25f, false, false, null);
            }
            else
            {
                this.BChar.Heal(this.BChar, (float)Heal * 0.25f, false, false, null);
            }
        }
    }
}