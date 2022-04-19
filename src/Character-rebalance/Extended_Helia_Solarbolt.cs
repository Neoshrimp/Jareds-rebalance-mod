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


class Extended_Helia_Solarbolt: S_TW_Red_1
{
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Init()
    {
        base.Init();
        if (BChar != null && BChar is BattleAlly battleAlly)
        {
            if (battleAlly.MyBasicSkill?.buttonData?.MySkill?.KeyID == MySkill?.MySkill?.KeyID)
            {
                APChange = -2;
            }
        }

    }

}
