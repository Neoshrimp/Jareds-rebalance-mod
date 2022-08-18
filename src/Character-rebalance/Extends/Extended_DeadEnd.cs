using BepInEx;
using Character_rebalance.Extends;
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


namespace Character_rebalance.Extends
{
    public class Extended_DeadEnd : Extended_Public_10, IP_Hit
    {

        public override void Init()
        {
            CountingExtedned = true;
        }
        public void Hit(SkillParticle SP, int Dmg, bool Cri)
        {
            if (MySkill.IsNowCounting && SP.TargetChar.Contains(BChar))
            {
			    BattleSystem.instance.AllyTeam.AP++;
            }
        }
    }
}