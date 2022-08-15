namespace Character_rebalance.Extends
{
    internal class Extended_Pressel_HolyLight : PriestPBase
    {
		public override void FixedUpdate()
		{
			if (PassiveDraw)
			{
				NotCount = true;
			}
			else
			{
				NotCount = false;
			}
			base.FixedUpdate();
		}
	}
}