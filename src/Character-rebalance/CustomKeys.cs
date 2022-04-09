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
    class CustomKeys
    {
        static public string Skill_Joey_CP_ExtraPot = CharacterRebalancePlugin.GUID + "_" + "Joey_CP_ExtraPot";
        
        static public string SkillExtended_Joey_CP_ExtraPot_Ex = CharacterRebalancePlugin.GUID + "_" + "Joey_CP_ExtraPot_Ex";
        // Skill_Extended class name    
        static public string ClassName_Joey_CP_ExtraPot_Ex = "Extended_Joey_CP_ExtraPot";
    }
}
