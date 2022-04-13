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


namespace Character_rebalance
{
    public class CustomKeys
    {
        static public readonly string Skill_Joey_CP_ExtraPot = CharacterRebalancePlugin.GUID + "_" + "Joey_CP_ExtraPot";
        
        static public readonly string SkillExtended_Joey_CP_ExtraPot_Ex = CharacterRebalancePlugin.GUID + "_" + "Joey_CP_ExtraPot_Ex";
        // Skill_Extended class name    
        static public readonly string ClassName_Joey_CP_ExtraPot_Ex = "Extended_Joey_CP_ExtraPot";

        static public readonly string ClassName_Joey_HealingDrone_Ex = "Extended_Joey_HealingDrone_Ex";

        static public readonly string Skill_Joey_HealingDrone_HealAllies = CharacterRebalancePlugin.GUID + "_" + "Joey_HealingDrone_HealEAllies";

        static public readonly string SkillEffect_Joey_HealingDrone_HealAllies_Effect = CharacterRebalancePlugin.GUID + "_" + "Joey_HealingDrone_HealAllies_Effect";


    }
}
