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

namespace Character_rebalance
{
    public class JoeyPatches
    {
        //2do. delete
        public static BepInEx.Logging.ManualLogSource logger = CharacterRebalancePlugin.logger;


        [HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromSavedData))]
        class ExtraPotSkillPatch
        {
            static void Postfix(GDESkillData __instance, ref GameObject ____Particle, ref string ____PathParticle)
            {
                if (__instance.Key == CustomKeys.Skill_Joey_CP_ExtraPot)
                {

                    __instance.ViewBuff = true;
                    __instance.Name = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(
                        GDESchemaKeys.Skill, CustomLoc.StripGuid(CustomKeys.Skill_Joey_CP_ExtraPot), CustomLoc.TermType.Name));
                    __instance.Description = "";

                    __instance.KeyID = "";
                    __instance.User = "";
                    __instance.LucyPartyDraw = "";
                    __instance.PlusSkillView = "";

                    __instance.Target = new GDEs_targettypeData("null");


                    __instance.Effect_Target = new GDESkillEffectData("null");
                    __instance.Effect_Self = new GDESkillEffectData("null");
                    __instance.Category = new GDESkillCategoryData("null");


                    // optional or required?
                    //____PathParticle = "Particle/impact";
                    //____Particle = Resources.Load<GameObject>(____PathParticle);

                    // 2do. add immage
                    //__instance.Image_0 = GDEDataManager.GetUnityObject<Sprite>(GDEItemKeys.Skill_S_Joey_7_1, "Image_0", null, GDESchemaKeys.Skill);
                    //__instance.Image_0 = new GDESkillData(GDEItemKeys.Skill_S_Joey_7_1).Image_0
                    //__instance.Image_1 = ???;


                    // list of SkillExtends which have their own GDE schema data
                    __instance.SKillExtendedItem = new List<GDESkillExtendedData>() { new GDESkillExtendedData(CustomKeys.SkillExtended_Joey_CP_ExtraPot_Ex) };
                    

                    // list of Skill_Extended classes directly associated with the skill. Should NEVER refer to the same classes as the ones in SkillExtendedItem List
                    __instance.SkillExtended = new List<string>();

                    __instance.PlusViewBuffList = new List<GDEBuffData>();
                    __instance.PlusKeyWords = new List<GDESkillKeywordData>();
                }
            }
        }

        [HarmonyPatch(typeof(GDESkillExtendedData), nameof(GDESkillExtendedData.LoadFromSavedData))]
        class ExtraPotEXtendedSkillPatch
        {
            static void Postfix(GDESkillExtendedData __instance, ref string ____PathParticle)
            {
                if (__instance.Key == CustomKeys.SkillExtended_Joey_CP_ExtraPot_Ex)
                {

                    __instance.Name = "";
                    __instance.EnforceString = "";
                    __instance.NeedCharacter = "";
                    ____PathParticle = "";

                    __instance.Des = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(
                        GDESchemaKeys.SkillExtended, CustomLoc.StripGuid(CustomKeys.SkillExtended_Joey_CP_ExtraPot_Ex), CustomLoc.TermType.Desc));

                    __instance.ClassName = CustomKeys.ClassName_Joey_CP_ExtraPot_Ex;

                }
            }
        }


        [HarmonyPatch(typeof(Extended_Joey_7))]
        class CreatePotionPatch
        {

            [HarmonyPatch(nameof(Extended_Joey_7.SkillUseSingle))]
            [HarmonyTranspiler]

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                bool injected = false;
                foreach (var ci in instructions)
                {
                    if (ci.opcode == OpCodes.Stfld && ((FieldInfo)ci.operand).Equals(AccessTools.Field(typeof(Extended_Joey_7), "OutPutSkill")) && !injected)
                    {
                        injected = true;
                        // adds our ExtraPot skill to selection of possible effects
                        yield return ci;
                        yield return new CodeInstruction(OpCodes.Ldloc_0);
                        yield return new CodeInstruction(OpCodes.Ldsfld, AccessTools.Field(typeof(CustomKeys),"Skill_Joey_CP_ExtraPot"));
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Passive_Char), "BChar"));
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Passive_Char), "BChar"));
                        yield return new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(BattleChar), "MyTeam"));
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Skill), "TempSkill", new Type[] { typeof(string), typeof(BattleChar), typeof(BattleTeam)}));
                        yield return new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<Skill>), "Add"));
                    }
                    else 
                    {
                        yield return ci;
                    }
                }
            }



        }

    }
}
