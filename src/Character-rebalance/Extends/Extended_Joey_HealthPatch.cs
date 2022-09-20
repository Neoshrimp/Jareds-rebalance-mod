using BepInEx;
using BepInEx.Bootstrap;
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


public class Extended_Joey_HealthPatch : S_Joey_12
{

    public override void Init()
    {
        base.Init();
        if (BattleSystem.instance != null && BattleSystem.instance.AllyTeam.UsedPotionNum > 0)
        {
            this.NotCount = true;
        }

    }

    public override void FixedUpdate()
    {
        if (BattleSystem.instance != null && BattleSystem.instance.AllyTeam.UsedPotionNum > 0)
        {
            this.NotCount = true;
        }
    }

    public override string DescExtended(string desc)
    {
        if (Chainloader.PluginInfos.ContainsKey("neo.ca.gameplay.swiftnessRework"))
            return base.DescExtended(desc).Replace("Swiftness", "<b>Effortless</b>");

        return base.DescExtended(desc);
    }


}

