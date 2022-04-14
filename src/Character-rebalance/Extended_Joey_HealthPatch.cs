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


class Extended_Joey_HealthPatch : S_Joey_12
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

    


}

