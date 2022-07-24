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
    class DebugDeez
    {
        public static BepInEx.Logging.ManualLogSource logger = CharacterRebalancePlugin.logger;


        /*            GDEDataManager.GetAllDataKeysBySchema("Skill", out List<string> keys);


            logger.LogInfo("{");
            foreach (var sk in keys)
            {
                logger.LogInfo($"\"{sk}\":\"{LocalizeManager.DBFile.GetTranslation(CustomLoc.TermKey("Skill", sk, CustomLoc.TermType.Name))}\",");
            }
            logger.LogInfo("}");*/


        /*            GDEDataManager.GetAllDataKeysBySchema(GDESchemaKeys.EnemyQueue, out List<string> keys);

                    foreach (var qk in keys)
                    {
                        var qd = new GDEEnemyQueueData(qk);
                        var s = "[";
                        qd.Enemys.ForEach(e => s += "\"" + e.name + "\",");
                        s += "]";

                        logger.LogInfo($"\"{qk}\":{s},");
                    }*/



        //[HarmonyPatch]
        class dd
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.PropertyGetter(typeof(LocalizationManager), "CurrentLanguage");
            }

            static void Postfix()
            {
                var stack = new StackTrace();
                var list = new List<StackFrame>(stack.GetFrames());
                if (list.Find(f => f.GetMethod().Name == "Update") != null)
                    return;
                CharacterRebalancePlugin.logger.LogInfo("Getter");
                list.ForEach(f => logger.LogInfo(f.GetMethod()));
                logger.LogInfo("-------------------");
            }
        }


        //[HarmonyPatch]
        class dd2
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.PropertySetter(typeof(LocalizationManager), "CurrentLanguage");
            }

            static void Postfix()
            {
                logger.LogInfo("Setter");
                var stack = new StackTrace();
                new List<StackFrame>(stack.GetFrames()).ForEach(f => logger.LogInfo(f.GetMethod()));
                logger.LogInfo("-------------------");
            }
        }

        //[HarmonyPatch]
        class dd3
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(GDEDataManager), "LocalizeUpdate");
                yield return AccessTools.Method(typeof(GDEDataManager), "LocalizeArrayUpdate");

            }

            static void Postfix(string Key, string ValueName, Dictionary<string, object> LocalizeText)
            {
                Debug.Log(string.Concat(new object[]
            {
                LocalizeText["_gdeSchema"],
                "/",
                Key,
                "_",
                ValueName
            }));
            }
        }


        //[HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillPatch
        {
            static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
            {
                
                if (__instance.Key.StartsWith("S_Joey_7"))
                {
                    logger.LogInfo($"{__instance.Key}: {__instance.Description}");
                    var newT = Traverse.Create(__instance);
                    newT.Fields().ForEach(f => {
                        var fval = newT.Field(f).GetValue();
                        {
                            if (fval == null)
                                fval = "is null";
                            if (fval is string && (string)fval == string.Empty)
                                fval = "\"\"";
                            logger.LogInfo($"{f} = {fval}");
                        }
                    });
                }
            }
        }


        //[HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromSavedData))]
        class dd5
        {
            static void Postfix(GDESkillData __instance)
            {

                if (__instance.Key == CustomKeys.Skill_Joey_CP_ExtraPot)
                {
                    logger.LogInfo($"{__instance.Key}");
                    var newT = Traverse.Create(__instance);
                    newT.Fields().ForEach(f => {
                        var fval = newT.Field(f).GetValue();
                        {
                            if (fval == null)
                                fval = "is null";
                            if (fval is string && (string)fval == string.Empty)
                                fval = "\"\"";
                            logger.LogInfo($"{f} = {fval}");
                        }
                    });


                }
            }


        }


        //[HarmonyPatch(typeof(GDESkillExtendedData), nameof(GDESkillExtendedData.LoadFromSavedData))]
        class whatever
        {
            static void Postfix(GDESkillExtendedData __instance)
            {

                if (__instance.Key == CustomKeys.SkillExtended_Joey_CP_ExtraPot_Ex)
                {
                    logger.LogInfo($"{__instance.Key}");
                    var newT = Traverse.Create(__instance);
                    newT.Fields().ForEach(f => {
                        var fval = newT.Field(f).GetValue();
                        {
                            if (fval == null)
                                fval = "is null";
                            if (fval is string && (string)fval == string.Empty)
                                fval = "\"\"";
                            logger.LogInfo($"{f} = {fval}");
                        }
                    });


                }
            }


        }


        //[HarmonyPatch(typeof(GDESkillExtendedData), nameof(GDESkillData.LoadFromDict))]
        class GdeSkillEXPatch
        {
            static void Postfix(GDESkillExtendedData __instance, Dictionary<string, object> dict)
            {

                if (__instance.Key.StartsWith("Joey_7"))
                {
                    logger.LogInfo($"{__instance.Key}: {__instance.Des}");
                    //__instance.Des = "suk on deeznuts";
                    var newT = Traverse.Create(__instance);
                    newT.Fields().ForEach(f =>
                    {
                        var fval = newT.Field(f).GetValue();
                        {
                            if (fval == null)
                                fval = "is null";
                            if (fval is string && (string)fval == string.Empty)
                                fval = "\"\"";
                            logger.LogInfo($"{f} = {fval}");
                        }
                    });
                }
            }
        }

        //[HarmonyPatch(typeof(Skill_Extended), nameof(Skill_Extended.Init))]
        class skExPatch
        {
            static void Postfix(Skill_Extended __instance, string ____Des, GDESkillExtendedData ___Data)
            {
                //Debug.Log(__instance == null);
                if (__instance.Data != null)
                {
                    Debug.Log(__instance.Data);
                }
                //logger.LogInfo(____Des);
            }
        }



        public static string debug_Skill_Ex = "debug_Skill_Ex";

        //[HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
        class gdeSkillPatch
        {
            static void Postfix(GDESkillData __instance)
            {
                if (__instance.Key == GDEItemKeys.Skill_S_DefultSkill_1)
                {
                    var newEx = new GDESkillExtendedData(debug_Skill_Ex);
                    __instance.SkillExtended.Add(newEx.ClassName);
                    __instance.SKillExtendedItem.Add(newEx);
                }
            }
        }

        //[HarmonyPatch(typeof(GDESkillExtendedData), nameof(GDESkillExtendedData.LoadFromSavedData))]
        class gdeSkillExPatch
        {
            static void Postfix(GDESkillExtendedData __instance)
            {
                if (__instance.Key == debug_Skill_Ex)
                {
                    __instance.Des = "heal deeznuts";
                    //__instance.ClassName = "Debug_Playground.debug_Skill_Ex,Debug-Playground";
                    __instance.ClassName = "debug_Skill_Ex";
                }
            }
        }

        public static void ObjectTraverse(object o)
        {
            var newT = Traverse.Create(o);
            newT.Fields().ForEach(f =>
            {
                var fval = newT.Field(f).GetValue();
                {
                    if (fval == null)
                        fval = "is null";
                    if (fval is string && (string)fval == string.Empty)
                        fval = "\"\"";
                    logger.LogInfo($"{f} = {fval}");
                }
            });
        }

        //[HarmonyPatch(typeof(SkillTargetTooltip), nameof(SkillTargetTooltip.InputInfo))]
        class ttPatch
        {
            static void Postfix(SkillTargetTooltip __instance)
            {

                Debug.Log(__instance.Text.font);
                ObjectTraverse(__instance.Text);
            }
        }


        //[HarmonyPatch(typeof(FieldStore), "Init")]
        class IsUnlockPatch
        {
            static void Prefix()
            {

                if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.T))
                {
                    UnityEngine.Debug.Log("is unlock");
                    SaveManager.savemanager._NowData.unlockList.PublicSkillKey.ForEach(s => UnityEngine.Debug.Log(s));
                }
            }
        }


        //[HarmonyPatch(typeof(PlayData), nameof(PlayData.DataBaseInit))]
        class ReAddCommonParryAttackPatch
        {
            static void Postfix()
            {
                Debug.Log("init");
                //PlayData._ALLSKILLLIST.RemoveAll(s => s.Category.Key == new GDESkillCategoryData(GDEItemKeys.SkillCategory_PublicSkill).Key);

                PlayData._ALLSKILLLIST.FindAll(s => s.Category.Key == GDEItemKeys.SkillCategory_PublicSkill).ForEach(s => Debug.Log(s.Key));


                //PlayData._ALLSKILLLIST.Add(new GDESkillData(GDEItemKeys.Skill_S_Public_10));
                //PlayData._ALLSKILLLIST.Add(new GDESkillData(GDEItemKeys.Skill_S_Public_11));
                //PlayData._ALLSKILLLIST.Add(new GDESkillData(GDEItemKeys.Skill_S_Public_34));





                //PlayData._ALLSKILLLIST.Add(new GDESkillData(GDEItemKeys.Skill_S_LianUnlock));
            }

        }

        //[HarmonyPatch(typeof(Extended_Public_10), "FixedUpdate")]
        class DisableDeadEnd
        {
            static bool Prefix()
            {

                return false;
            }
        }


        //[HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromSavedData))]
        class NewSkillPatch
        {
            
            static void Postfix(GDESkillData __instance, ref GameObject ____Particle, ref string ____PathParticle)
            {
                var dataKeysBySchemaRef = AccessTools.FieldRefAccess<GDEDataManager, Dictionary<string, HashSet<string>>>(AccessTools.Field(typeof(GDEDataManager), "dataKeysBySchema"));
                var nk = "newSkill";



                if (__instance.Key == nk)
                {
                    //dataKeysBySchemaRef()["Skill"].Add(nk);

                    __instance.NoDrop = false;
                    __instance.User = GDEItemKeys.Character_Joey;

                    //__instance.User = "";


                    __instance.Name = "new skill";
                    __instance.Description = "deeznuts";

                    __instance.KeyID = "";
                    __instance.LucyPartyDraw = "";
                    __instance.PlusSkillView = "";

                    __instance.Target = new GDEs_targettypeData(GDEItemKeys.s_targettype_all_ally);

                    ____PathParticle = "Particle/Joey/Joey_4";
                    ____Particle = new GDESkillData(GDEItemKeys.Skill_S_Joey_11).Particle;


                    //__instance.Effect_Target = new GDESkillEffectData(CustomKeys.SkillEffect_Joey_HealingDrone_HealAllies_Effect);
                    __instance.Effect_Target = new GDESkillEffectData(GDEItemKeys.SkillEffect_SE_Null);
                    __instance.Effect_Self = new GDESkillEffectData("null");
                    __instance.Category = new GDESkillCategoryData("null");


                    __instance.SKillExtendedItem = new List<GDESkillExtendedData>();
                    __instance.SkillExtended = new List<string>();
                    __instance.PlusViewBuffList = new List<GDEBuffData>();
                    __instance.PlusKeyWords = new List<GDESkillKeywordData>();
                    Debug.Log("new skill");
                    GDEDataManager.GetAllDataKeysBySchema(GDESchemaKeys.Skill, out List<string> skillKeys);
                    Debug.Log(skillKeys.Find(s => s == nk));

                }
            }

        }

        [HarmonyPatch(typeof(StageSystem), nameof(StageSystem.CheatChack))]
        class debugBattlePatch
        {
            static void Postfix(StageSystem __instance)
            {

                string cheatChat = PlayData.CheatChat;
                switch (cheatChat)
                {
                    case "bs":
                        __instance.CheatEnabled();
                        //first bool is normalBattle
                        FieldSystem.instance.BattleStart(new GDEEnemyQueueData(GDEItemKeys.EnemyQueue_Queue_S3_PharosLeader), __instance.StageData.BattleMap.Key, false, false, string.Empty, string.Empty);

                        break;

                }
            }
        }

    }
}
