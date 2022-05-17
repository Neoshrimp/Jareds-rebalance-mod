using GameDataEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Character_rebalance.Extends
{
    public class Extended_Sizz_TimeToMove : Skill_Extended
    {
        // 2do. doesn't work with fixed
        public override void SkillUseHandBefore()
        {
            base.SkillUseHandBefore();
            Debug.Log("deez");
            var eveHolder = BattleSystem.instance.AllyTeam.AliveChars.Find(bc => bc.BuffFind(GDEItemKeys.Buff_P_Sizz_0, false));
            Debug.Log(eveHolder);
            if (eveHolder != null && ((P_Sizz_0)eveHolder.BuffReturn(GDEItemKeys.Buff_P_Sizz_0)).Stack >= 2 )
            {
                Debug.Log("deez2");

                NotCount = true;
            }
        }
    }
}
