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

class Extended_Lian_RelentlessSwipe : Skill_Extended, IP_DamageTake
{

    // 2do. check encyclopedia char nullpo
    public void DamageTake(BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
    {
        if (!NODEF)
        {
            BonusOn();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (bonusActive)
            base.SkillParticleOn();
        else
            base.SkillParticleOff();
    }

    public override void Init()
    {
        base.Init();
        this.SkillParticleObject = new GDESkillExtendedData(GDEItemKeys.SkillExtended_Public_10_Ex).Particle;
        // makes sure IP_DamageTake is found while skill is on count down
        CountingExtedned = true;


        if (BattleSystem.instance != null && TurnEventObserver.turnEvents.DamageTake != null)
        {
            if (TurnEventObserver.turnEvents.DamageTake.Find(dte => !dte.NODEF) != null)
            {
                BonusOn();
            }
        }


    }

    public override void TurnUpdate()
    {
        base.TurnUpdate();
        APChange = 0;
        this.PlusSkillStat.cri = 0;
        bonusActive = false;

    }

    void BonusOn()
    {
        APChange = 1;
        this.PlusSkillStat.cri = 60;
        bonusActive = true;
    }

    bool bonusActive = false;

}

