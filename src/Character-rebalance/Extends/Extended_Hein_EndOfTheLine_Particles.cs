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

namespace Character_rebalance.Extends
{
    public class Extended_Hein_EndOfTheLine_Particles : Skill_Extended
    {

        public override void Init()
        {
            SkillParticleObject = new GDESkillExtendedData(GDEItemKeys.SkillExtended_Public_1_Ex).Particle;
        }

        public override void Special_PointerEnter(BattleChar Char)
        {
            if (Misc.NumToPer((float)Char.GetStat.maxhp, (float)Char.HP) <= HeinPatches.eolCritThreshold)
            {
                partOn = true;
            }
            else
            {
                partOn = false;
            }

        }

        public override void Special_PointerExit()
        {
            partOn = false;
        }

        public override void FixedUpdate()
        {
            if (Misc.NumToPer((float)BChar.GetStat.maxhp, (float)BChar.HP) <= HeinPatches.eolCritThreshold || partOn)
            {
                SkillParticleOn();
            }
            else
            {
                SkillParticleOff();
            }
        }

        bool partOn;
    }
}