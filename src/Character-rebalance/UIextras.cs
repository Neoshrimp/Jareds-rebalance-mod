using Character_rebalance;
using GameDataEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BepInEx;
using TMPro;
using System.Reflection;
using HarmonyLib;

namespace Character_rebalance
{
    public class UIextras
    {

        public static GameObject textTooltipTarget;
        public static void ViewTextTooltip(BattleChar target, string info)
        {
            if (textTooltipTarget != null)
            {
                UnityEngine.Object.Destroy(textTooltipTarget);
            }
            textTooltipTarget = UnityEngine.Object.Instantiate(new GameObject("textTooltip", new Type[] { typeof(TextTargetSelect), typeof(TextMeshProUGUI) }));


            if (target.Info.Ally)
            {
                textTooltipTarget.transform.SetParent(TargetSelects.ThisObject.transform);
            }
            else
            {
                textTooltipTarget.transform.SetParent(TargetSelects.TargetSelect3D.transform);
            }

            Misc.UIInit(textTooltipTarget);

            textTooltipTarget.GetComponent<TextTargetSelect>().DisplayInfo(info);
            textTooltipTarget.GetComponent<TextTargetSelect>().Init(target.GetTopPos(), target.Info.Ally, target);
        }

        public class TextTargetSelect : TargetSelect
        {
            public void DisplayInfo(string info)
            {
                textMesh = gameObject.GetComponent<TextMeshProUGUI>();

                //CharacterRebalancePlugin.gameAssembly.
                textMesh.text = info;

                Debug.Log("Mine");
                Debug.Log(textMesh.font);
                DebugDeez.ObjectTraverse(textMesh);
            }

            public TextMeshProUGUI textMesh;
        }

       

    }
}
