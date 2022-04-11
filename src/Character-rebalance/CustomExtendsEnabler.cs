using GameDataEditor;
using HarmonyLib;
using System;
using System.Reflection;


public class CustomExtendsEnabler
{
	[HarmonyPatch(typeof(Skill_Extended))]
	class DateToExtenedPatches
	{
		static AccessTools.FieldRef<Skill_Extended, string> _DesRef = AccessTools.FieldRefAccess<Skill_Extended, string>("_Des");

		[HarmonyPatch(nameof(Skill_Extended.DataToExtended), new Type[] { typeof(GDESkillExtendedData) })]
		[HarmonyPrefix]
		static bool DataToExtendedPrefix(GDESkillExtendedData _Data, ref Skill_Extended __result)
		{
			Type customExType = Assembly.GetExecutingAssembly().GetType(_Data.ClassName);

			if (ReferenceEquals(customExType, null))
			{
				return true;
			}

			__result = (Skill_Extended)Assembly.GetExecutingAssembly().CreateInstance(customExType.Name);

			__result.Data = _Data;
			_DesRef(__result) = _Data.Des;
			if (!ReferenceEquals(_Data.Image, null))
			{
				__result.SpriteImage = Misc.CreatSprite(_Data.Image);
			}
			return false;
		}



		[HarmonyPatch(nameof(Skill_Extended.DataToExtendedC))]
		[HarmonyPrefix]
		static bool DataToExtendedC(string ClassKey, ref Skill_Extended __result)
		{
			Type customExType = Assembly.GetExecutingAssembly().GetType(ClassKey);

			if (ReferenceEquals(customExType, null))
			{
				return true;
			}
			__result = (Skill_Extended)Assembly.GetExecutingAssembly().CreateInstance(customExType.Name);
			return false;
		}


	}

}

