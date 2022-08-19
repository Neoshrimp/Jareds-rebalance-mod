using BepInEx;
using GameDataEditor;
using HarmonyLib;
using I2.Loc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Character_rebalance
{
    public class Buff_Charon_DarkPoison : Buff, IP_SkillUse_Target
    {
        public void AttackEffect(BattleChar hit, SkillParticle SP, int DMG, bool Cri)
        {
            if (base.StackNum >= 1 && SP.SkillData.IsDamage && hit.BuffAdd(GDEItemKeys.Buff_B_ShadowPriest_1_T, base.Usestate_F, false, 0, false, -1, false) != null)
            {
                base.StackDestroy();
            }
        }

        public override string DescExtended()
        {
            return base.DescExtended().Replace("&a", (base.Usestate_F.GetStat.HIT_DOT + 100f).ToString());
        }

        public override void Init()
        {
            this.isStackDestroy = true;
            PlusStat.RES_DOT = 66;
        }
    }
}