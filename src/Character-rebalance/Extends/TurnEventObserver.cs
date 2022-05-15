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


public struct TurnEvents
{
    public List<DamageTakeEvent> DamageTake;
}

public class DamageTakeEvent
{
    public BattleChar User;
    public int Dmg;
    public bool Cri;
    public bool resist;
    public bool NODEF; 
    public bool NOEFFECT;
    public BattleChar Target;
}


// alternatively Battle.instance.BattleLogs can be used but they don't contain as much information (i.e. if damage was pain or not)
class TurnEventObserver : LucyBuff, IP_DamageTake, IP_BattleEnd
{
    // only considers ally characters
    public void DamageTake(BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
    {

        turnEvents.DamageTake.Add(new DamageTakeEvent() { User = User, Dmg = Dmg, Cri = Cri, resist = resist, NODEF = NODEF, NOEFFECT = NOEFFECT, Target = Target});

    }

    public override void Init()
    {
        base.Init();
        // required to get picked up by BattleChar.IReturn
        AllBuff = true;
        InitTurnEvents();
    }

    public override void TurnUpdate()
    {
        base.TurnUpdate();
        if (BattleSystem.instance.TurnNum != 0)
        {
            InitTurnEvents();
        }

    }


    void InitTurnEvents()
    {
        turnEvents = new TurnEvents();
        turnEvents.DamageTake = new List<DamageTakeEvent>();
    }

    public void BattleEnd()
    {
        // resets event list at the end of the battle
        InitTurnEvents();

    }

    public static TurnEvents turnEvents;

}
