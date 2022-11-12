using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameDataEditor;
using HarmonyLib;
using UnityEngine;

namespace Character_rebalance
{
    public class VanillaEntityKeys
    {
        static HashSet<string> schemas = new HashSet<string>() { GDESchemaKeys.Skill, GDESchemaKeys.SkillExtended, GDESchemaKeys.Buff, 
                GDESchemaKeys.Character, GDESchemaKeys.Enemy, GDESchemaKeys.EnemyQueue, GDESchemaKeys.EnchantList, GDESchemaKeys.Item_Active, GDESchemaKeys.Item_Consume,
                GDESchemaKeys.Item_Equip, GDESchemaKeys.Item_Passive, GDESchemaKeys.Item_Potions,  GDESchemaKeys.Item_Scroll, GDESchemaKeys.RandomEvent};

        public static HashSet<string> Find()
        {
/*            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();*/

            HashSet<string> keySet = new HashSet<string>();

            var list = AccessTools.GetDeclaredFields(typeof(GDEItemKeys));//.Where(fi => schemas.Contains(fi.Name.Split('_')[0])).Select(fi => (string) fi.GetValue(null));

            foreach (var fi in list)
            {
                if (schemas.Contains(fi.Name.Split('_')[0]))
                {
                    keySet.Add((string)fi.GetValue(null));
                }

            }

/*            watch.Stop();
            Debug.Log($"ms: {watch.ElapsedMilliseconds}");
            Debug.Log(keySet.Count);*/

            return keySet;
        }


    }
}
