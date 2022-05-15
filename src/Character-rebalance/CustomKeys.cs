using GameDataEditor;
using HarmonyLib;
using System.Collections.Generic;

namespace Character_rebalance
{
    public class CustomKeys
    {
        static public readonly string Skill_Joey_CP_ExtraPot = CharacterRebalancePlugin.GUID + "_" + "Joey_CP_ExtraPot";
        
        static public readonly string SkillExtended_Joey_CP_ExtraPot_Ex = CharacterRebalancePlugin.GUID + "_" + "Joey_CP_ExtraPot_Ex";
        // Skill_Extended class name    
        //static public readonly string ClassName_Joey_CP_ExtraPot_Ex = "Extended_Joey_CP_ExtraPot";

        //static public readonly string ClassName_Joey_HealingDrone_Ex = "Extended_Joey_HealingDrone_Ex";

        static public readonly string Skill_Joey_HealingDrone_HealAllies = CharacterRebalancePlugin.GUID + "_" + "Joey_HealingDrone_HealEAllies";

        static public readonly string SkillEffect_Joey_HealingDrone_HealAllies_Effect = CharacterRebalancePlugin.GUID + "_" + "Joey_HealingDrone_HealAllies_Effect";

        //static public readonly string ClassName_Joey_HealthPatch_Ex = "Extended_Joey_HealthPatch";

        //static public readonly string ClassName_Lian_RelentlessSwipe_Ex = "Extended_Lian_RelentlessSwipe";

        static public readonly string Buff_Lucy_TurnEventObserver = CharacterRebalancePlugin.GUID + "_" + "Lucy_TurnEventObserver";

        //static public readonly string ClassName_TurnEventObserver_Buff = "TurnEventObserver";

        //static public readonly string ClassName_Extended_Helia_Solarbolt = "Extended_Helia_Solarbolt";

        //static public readonly string ClassName_Extended_Solarbolt_APchange = "Extended_Solarbolt_APchange";

        //static public readonly string ClassName_Extended_Helia_Tears_of_the_Sun = "Extended_Helia_Tears_of_the_Sun";

        //static public readonly string ClassName_Extended_Twins_Selenelion = "Extended_Twins_Selenelion";

        //static public readonly string ClassName_Extended_Helia_Flame_Arrow = "Extended_Helia_Flame_Arrow";

        //static public readonly string ClassName_Extended_Selena_Bloody_Moon = "Extended_Selena_Bloody_Moon";
        // ok ClassName consts are redundant. Just use nameof(Type) lol
        //static public readonly string ClassName_Extended_Selena_PowerOfTheFullMoon = "Extended_Selena_PowerOfTheFullMoon";
        // should be part of universal helper mod
        static public readonly string SkillKeyword_Keyword_Swiftness = CharacterRebalancePlugin.GUID + "_" + "Keyword_Swiftness";

        static public readonly string SkillKeyword_Keyword_Critical = CharacterRebalancePlugin.GUID + "_" + "Keyword_Critical";


        public static void RegisterCustomKeys()
        {
            var dataKeysBySchemaRef = AccessTools.FieldRefAccess<GDEDataManager, Dictionary<string, HashSet<string>>>(AccessTools.Field(typeof(GDEDataManager), "dataKeysBySchema"));
            if (dataKeysBySchemaRef() == null)
                return;
            dataKeysBySchemaRef()[GDESchemaKeys.Skill].Add(Skill_Joey_CP_ExtraPot);
            dataKeysBySchemaRef()[GDESchemaKeys.SkillExtended].Add(SkillExtended_Joey_CP_ExtraPot_Ex);
            dataKeysBySchemaRef()[GDESchemaKeys.Skill].Add(Skill_Joey_HealingDrone_HealAllies);
            dataKeysBySchemaRef()[GDESchemaKeys.SkillEffect].Add(SkillEffect_Joey_HealingDrone_HealAllies_Effect);
            dataKeysBySchemaRef()[GDESchemaKeys.Buff].Add(Buff_Lucy_TurnEventObserver);

            // should be part of universal helper mod
            dataKeysBySchemaRef()[GDESchemaKeys.SkillKeyword].Add(SkillKeyword_Keyword_Swiftness);
            dataKeysBySchemaRef()[GDESchemaKeys.SkillKeyword].Add(SkillKeyword_Keyword_Critical);
        }


    }
}
