using GameDataEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class Extended_Selena_Bloody_Moon : Skill_Extended
{
    public override void FixedUpdate()
    {
        base.FixedUpdate();
		var found = false;
		foreach (BattleChar battleChar in BattleSystem.instance.EnemyTeam.AliveChars)
		{
			if (battleChar.BuffFind(GDEItemKeys.Buff_B_TW_Red_3_T, false))
			{
				NotCount = true;
				found = true;
			}
		}

		if (!found)
			NotCount = false;
	}

}
