﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameDataEditor;
using BepInEx.Bootstrap;

public class Extended_Charon_SoulStigma : Skill_Extended
{
    public override void Special_PointerEnter(BattleChar Char)
    {
        base.Special_PointerEnter(Char);

        if (Char != null && Char.BuffFind(GDEItemKeys.Buff_B_ShadowPriest_1_T))
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

    public override string DescExtended(string desc)
    {
        if (Chainloader.PluginInfos.ContainsKey("neo.ca.gameplay.swiftnessRework"))
            return base.DescExtended(desc).Replace("Swiftness", "<b>Effortless</b>");

        return base.DescExtended(desc);
    }

    public override void Init()
    {
        base.Init();
        this.SkillParticleObject = new GDESkillExtendedData(GDEItemKeys.SkillExtended_Public_10_Ex).Particle;

    }

}
