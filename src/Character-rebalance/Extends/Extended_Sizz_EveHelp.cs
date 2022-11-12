using BepInEx.Bootstrap;
using GameDataEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using Character_rebalance;

public class Extended_Sizz_EveHelp : Skill_Extended
{

    public override void Init()
    {
        base.Init();
        if (Chainloader.PluginInfos.ContainsKey("neo.ca.gameplay.swiftnessRework"))
        { 
            quickPlugin = true;

            quickManagerFI = AccessTools.Field(Type.GetType("SwiftnessRework.SwiftnessReworkPlugin, SwiftnessRework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"), "quickManager");
            var qm = Type.GetType("SwiftnessRework.QuickManager, SwiftnessRework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            //var fm = Type.GetType("SwiftnessRework.FieldManager, SwiftnessRework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            setValMethod = AccessTools.Method(qm, "SetVal");
            //generics: new Type[] { typeof(bool) }
            setValMethod.GetParameters().ToList().ForEach(pi => Debug.Log(pi.ParameterType));
            //Debug.Log();
            if (ReferenceEquals(quickManagerFI, null) || ReferenceEquals(setValMethod, null))
                quickPlugin = false;
        }
    }

    public override string DescExtended(string desc)
    {
        var r = base.DescExtended(desc.Replace("&c", Math.Max(0, maxCastCount - castCount).ToString()));
        if (quickPlugin)
        {
            //r = r.Replace("Swiftness", "<b>Effortless</b>");

            r = r.Replace("<b>Swiftness</b>", "<b>Effortless</b> and <b>Quick</b>");

        }
        return r;
    }

    public override void FixedUpdate()
    {
        var eveHolder = BattleSystem.instance.AllyTeam.AliveChars.Find(bc => bc.BuffFind(GDEItemKeys.Buff_P_Sizz_0, false));
        if (eveHolder != null && ((P_Sizz_0)eveHolder.BuffReturn(GDEItemKeys.Buff_P_Sizz_0)).Stack >= 2)
        {
            NotCount = true;


            if (quickPlugin)
                setValMethod.Invoke(quickManagerFI.GetValue(null), new object[] { this, true });
            // 2do proper quick integration
            /*            if (quickPlugin)
                            SwiftnessRework.SwiftnessReworkPlugin.quickManager.SetVal(this, true);*/
        }
        else
        {
            NotCount = false;

            if (quickPlugin)
                setValMethod.Invoke(quickManagerFI.GetValue(null), new object[] { this, false });
            // 2do proper quick integration
            /*            if (quickPlugin)
                            SwiftnessRework.SwiftnessReworkPlugin.quickManager.SetVal(this, false);*/
        }
    }

    public override void SkillUseSingle(Skill SkillD, List<BattleChar> Targets)
    {
        if (castCount < maxCastCount)
        {
            Skill skill = Skill.TempSkill(GDEItemKeys.Skill_S_Sizz_0, this.BChar, this.BChar.MyTeam);
            skill.isExcept = true;
            skill.AP = 1;
            skill.AutoDelete = 1;
            skill.NotCount = true;

            if (quickPlugin)
                setValMethod.Invoke(quickManagerFI.GetValue(null), new object[] { skill, true });
            // 2do proper quick integration
            /*            if(quickPlugin)
                            SwiftnessRework.SwiftnessReworkPlugin.quickManager.SetVal(skill, true);*/


            var thisExtended = (Extended_Sizz_EveHelp)skill.ExtendedFind(typeof(Extended_Sizz_EveHelp).AssemblyQualifiedName);
            if (thisExtended != null)
                thisExtended.castCount = castCount + 1;
            BattleSystem.instance.AllyTeam.Add(skill, true);
        }

        foreach (BattleChar battleChar in this.BChar.MyTeam.AliveChars)
        {
            if (battleChar != Targets[0] && battleChar.BuffFind(GDEItemKeys.Buff_B_Sizz_0_T, false))
            {
                for (int i = 0; i < battleChar.BuffReturn(GDEItemKeys.Buff_B_Sizz_0_T, false).StackNum; i++)
                {
                    Targets[0].BuffAdd(GDEItemKeys.Buff_B_Sizz_0_T, this.BChar, false, 0, false, -1, false);
                }
                battleChar.BuffReturn(GDEItemKeys.Buff_B_Sizz_0_T, false).SelfDestroy();
            }
        }
    }


    bool quickPlugin = false;
    MethodInfo setValMethod;
    FieldInfo quickManagerFI;
    public int castCount = 0;
	public int maxCastCount = 3;
}
