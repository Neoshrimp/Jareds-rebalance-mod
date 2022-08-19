using HarmonyLib;
using System.Runtime.CompilerServices;
using PItem;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;
using System.Reflection.Emit;

namespace PItem.Equipment_rebalance
{
    public class Extended_Pinwheel : WeatherVane, IP_BattleEnd
    {
        public void BattleEnd()
        {
            MyItem.UseStackNum = 0;
        }
    }
}