using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ConditionOnTargetingUtils
{
    static public void SwiftnessUpdateManaCrytalUI(int skillAp, int overLoad)
    {
        if (skillAp >= 1 && BattleSystem.instance != null)
        {
            for (int i = BattleSystem.instance.AllyTeam.AP - skillAp; i > BattleSystem.instance.AllyTeam.AP - skillAp - overLoad; i--)
            {
                BattleSystem.instance.ActWindow.APGroup.transform.GetChild(i - 1).GetComponent<Animator>().SetBool("Using", false);
            }
        }
        
    }

    static public void ResetManaCrystalUI(int skillAp)
    {
        if (skillAp >= 1 && BattleSystem.instance != null)
        {
            for (int i = BattleSystem.instance.AllyTeam.AP; i > BattleSystem.instance.AllyTeam.AP - skillAp; i--)
            {
                BattleSystem.instance.ActWindow.APGroup.transform.GetChild(i - 1).GetComponent<Animator>().SetBool("Using", true);
            }
        }
    }

}

