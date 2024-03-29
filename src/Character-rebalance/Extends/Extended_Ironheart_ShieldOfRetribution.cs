﻿using BepInEx.Bootstrap;
using GameDataEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Extended_Ironheart_ShieldOfRetribution : Skill_Extended, IP_SkillUse_Team
{

    public override string DescExtended(string desc)
    {

        var r = base.DescExtended(desc).Replace("&a", this.hitsLeft.ToString());

        if (Chainloader.PluginInfos.ContainsKey("neo.ca.gameplay.swiftnessRework"))
            return r.Replace("Swiftness", "<b>Effortless</b>");

        return r;
    }
    public override void HandInit()
    {
        base.HandInit();
        hitsLeft = 4;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (BChar.MyTeam.partybarrier.BarrierHP > 0 && !MySkill.IsNowCounting)
        {
            NotCount = true;
            SkillParticleOn();
        }
        else 
        {
            NotCount = false;
            SkillParticleOff();
        }
    }


    public override void Init()
    {
        base.Init();
        this.CountingExtedned = true;
        this.SkillParticleObject = new GDESkillExtendedData(GDEItemKeys.SkillExtended_Public_10_Ex).Particle;

    }
    public IEnumerator Attack(BattleChar target)
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Skill Temp = Skill.TempSkill(GDEItemKeys.Skill_S_Prime_11, this.BChar, this.BChar.MyTeam);
        Temp.PlusHit = true;
        Temp.FreeUse = true;
        if (!target.IsDead)
        {
            this.BChar.ParticleOut(this.MySkill, Temp, target);
        }
        else
        {
            this.BChar.ParticleOut(this.MySkill, Temp, BattleSystem.instance.EnemyList.Random<BattleEnemy>());
        }
        yield break;
    }

    public void SkillUseTeam(Skill skill)
    {
        if (this.hitsLeft >= 1 && this.MySkill.IsNowCounting && skill.IsHeal && BattleSystem.instance.EnemyList.Count != 0)
        {
            this.hitsLeft--;
            BattleSystem.DelayInput(this.Attack(MySkill.MyButton.castskill.Target));
        }
    }



    int hitsLeft = 4;
}
