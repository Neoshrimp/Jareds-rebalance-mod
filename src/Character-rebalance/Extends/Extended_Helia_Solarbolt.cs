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
using Character_rebalance;
using Debug = UnityEngine.Debug;


public class Extended_Helia_Solarbolt: S_TW_Red_1
{

    public override void FixedUpdate()
    {
        base.FixedUpdate();


        if (BattleSystem.instance != null && BChar != null && BChar is BattleAlly battleAlly)
        {
            if (battleAlly.MyBasicSkill?.buttonData?.MySkill.KeyID == MySkill.MySkill.KeyID
                && battleAlly.MyBasicSkill.buttonData.ExtendedFind(CustomKeys.ClassName_Extended_Solarbolt_APchange) == null)
            {

                battleAlly.MyBasicSkill.buttonData.ExtendedAdd(new Extended_Solarbolt_APchange(-2));
            }
        }
    }


    
    // UI doesn't update perfectly with this. Very minor issue just gonna rephrase the description 
/*    public override void Init()
    {
        base.Init();


        if (BattleSystem.instance == null)
        {
            var ownerCW = UIManager.inst?.CharstatUI.GetComponent<CharStatV3>().CWindows.Find(cw => cw?.AllyCharacter.Info.KeyData == BChar.Info.KeyData);
            if (ownerCW != null)
            {

                if (ownerCW?.BasicSkillView?.buttonData?.MySkill?.KeyID == MySkill.MySkill.KeyID 
                    && ownerCW.BasicSkillView.buttonData.ExtendedFind(CustomKeys.ClassName_Extended_Solarbolt_APchange) == null)
                {
                    ownerCW.BasicSkillView.buttonData.ExtendedAdd(new Extended_Solarbolt_APchange(-2));
                }
            }
        }

    }*/

}
