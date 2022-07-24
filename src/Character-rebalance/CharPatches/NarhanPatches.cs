using BepInEx;
using Character_rebalance.Extends;
using DarkTonic.MasterAudio;
using GameDataEditor;
using HarmonyLib;
using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Character_rebalance.CharPatches
{
	public class NarhanPatches
	{



		[HarmonyPatch(typeof(GDECharacterData), nameof(GDECharacterData.LoadFromDict))]
		class GdeCharactersPatch
		{
			static void Postfix(GDECharacterData __instance, Dictionary<string, object> dict)
			{
				if (__instance.Key == GDEItemKeys.Character_Control)
				{
					dict.TryGetVector2("ATK", out Vector2 ogATK);
					__instance.ATK = new Vector2(14, ogATK.y);
					dict.TryGetVector2("MAXHP", out Vector2 ogMAXHP);
					__instance.MAXHP = new Vector2(27, ogMAXHP.y);
					dict.TryGetVector2("HIT_CC", out Vector2 ogHIT_CC);
					__instance.HIT_CC = new Vector2(20, ogHIT_CC.y);

				}
			}
		}



		[HarmonyPatch(typeof(GDESkillData), nameof(GDESkillData.LoadFromDict))]
		class GdeSkillPatch
		{
			static void Postfix(GDESkillData __instance, Dictionary<string, object> dict)
			{
				// nightmare syndrome
				if (__instance.Key == GDEItemKeys.Skill_S_Control_0)
				{
					__instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Control_0, CustomLoc.TermType.Description));
				}
				// mental shock
				else if (__instance.Key == GDEItemKeys.Skill_S_Control_5)
				{
					__instance.Track = true;
					__instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Control_5, CustomLoc.TermType.Description));
					__instance.SkillExtended = new List<string>() { typeof(Extended_Narhan_MentalShock).AssemblyQualifiedName };

				}
				// mentalist
				else if (__instance.Key == GDEItemKeys.Skill_S_Control_12)
				{
					__instance.NoBasicSkill = false;
				}
				// trojan horse
				else if (__instance.Key == GDEItemKeys.Skill_S_Control_11)
				{
					__instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Control_11, CustomLoc.TermType.Description));

				}
				// plant spite
				else if (__instance.Key == GDEItemKeys.Skill_S_Control_2)
				{
					__instance.Track = true;
				}
				// trauma
				else if (__instance.Key == GDEItemKeys.Skill_S_Control_8)
				{
					__instance.UseAp = 0;
				}
				// paranoia
				else if (__instance.Key == GDEItemKeys.Skill_S_Control_1)
				{
					__instance.IgnoreTaunt = true;
					__instance.Description = CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Skill, GDEItemKeys.Skill_S_Control_1, CustomLoc.TermType.Description));

				}
				// phantom pain
				else if (__instance.Key == GDEItemKeys.Skill_S_Control_10)
				{
					__instance.NotCount = true;
				}
			}
		}


		[HarmonyPatch(typeof(GDESkillEffectData), nameof(GDESkillEffectData.LoadFromDict))]
		class GdeSkillEffectPatch
		{
			static void Postfix(GDESkillEffectData __instance)
			{
				// nightmare syndrome
				if (__instance.Key == GDEItemKeys.SkillEffect_SE_Control_0_T)
				{
					__instance.DMG_Per = 45;
				}
				// mental shock
				else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Control_5_T)
				{
					__instance.DMG_Per = 70;
				}
				// trojan horse
				else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Control_11_T)
				{
					__instance.DMG_Per = 80;
				}
				// plant spite
				else if (__instance.Key == GDEItemKeys.SkillEffect_SE_Control_2_T)
				{
					__instance.DMG_Per = 50;
				}


			}
		}


		[HarmonyPatch(typeof(GDEBuffData), nameof(GDEBuffData.LoadFromDict))]
		class GDEBuffData_Patch
		{
			static void Postfix(GDEBuffData __instance, Dictionary<string, object> dict)
			{
				// sleep
				if (__instance.Key == GDEItemKeys.Buff_B_Control_6_T)
				{
					dict.TryGetString("Description", out string ogDesc, GDEItemKeys.Buff_B_Control_6_T);
					__instance.Description = ogDesc + CustomLoc.MainFile.GetTranslation(CustomLoc.TermKey(GDESchemaKeys.Buff, GDEItemKeys.Buff_B_Control_6_T, CustomLoc.TermType.ExtraDesc));
				}
			}
		}



		static int WrapIndex(int pos, int shift, int count)
		{
			if (pos < 0 || pos > count || count <= 0)
			{
				Debug.Log("WrapIndex: invalid args");
				return -1;
			}
			if (count == 1)
			{
				return 0;
			}
			int res = -1;
			if (pos + shift >= 0)
			{
				res = (pos + shift) % count;
			}
			if (pos + shift < 0)
			{
				res = count - Math.Abs((pos + shift) % count);
			}
			return res;
		}

		[HarmonyPatch(typeof(S_Control_0), nameof(S_Control_0.SkillUseSingle))]
		class NightmareSyndromePatch
		{

			static IEnumerator Attack(Skill_Extended extend, BattleChar mainTarget, List<BattleChar> targets)
			{
				BattleChar prevT = null;
				foreach (var t in targets)
				{
					Skill skill2 = Skill.TempSkill(GDEItemKeys.Skill_S_Control_0_0, extend.BChar, extend.BChar.MyTeam);
					skill2.AllExtendeds[0].IsDamage = true;
					skill2.FreeUse = true;
					if (mainTarget.BuffFind(GDEItemKeys.Buff_B_Control_P, false))
					{
						skill2.AllExtendeds[0].PlusSkillStat.cri = 100f;
						mainTarget.BuffAdd(GDEItemKeys.Buff_B_Control_P, extend.BChar, false, 0, false, -1, false);
					}
					skill2.AllExtendeds[0].SkillBasePlus.Target_BaseDMG = (int)((float)extend.BChar.GetStat.maxhp * 0.45f);
					if (t.Equals(prevT))
					{
						yield return new WaitForSeconds(0.35f);
					}
					extend.BChar.ParticleOut(extend.MySkill, skill2, t);
					if (t.BuffFind(GDEItemKeys.Buff_B_Control_P, false) && !t.Equals(prevT))
					{
						t.BuffAdd(GDEItemKeys.Buff_B_Control_P, extend.BChar, false, 0, false, -1, false);
					}
					prevT = t;
				}
			}

			static bool Prefix(Skill SkillD, List<BattleChar> Targets, S_Control_0 __instance)
			{
				if (!Targets[0].Info.Ally)
				{
					if (BattleSystem.instance.EnemyList.Count == 1)
					{
						Skill skill = Skill.TempSkill(GDEItemKeys.Skill_S_Control_0_0, __instance.BChar, __instance.BChar.MyTeam);
						skill.AllExtendeds[0].IsDamage = true;
						if (Targets[0].BuffFind(GDEItemKeys.Buff_B_Control_P, false))
						{
							skill.AllExtendeds[0].PlusSkillStat.cri = 100f;
							Targets[0].BuffAdd(GDEItemKeys.Buff_B_Control_P, __instance.BChar, false, 0, false, -1, false);

						}
						skill.AllExtendeds[0].SkillBasePlus.Target_BaseDMG = (int)((float)__instance.BChar.GetStat.maxhp * 0.6f);
						__instance.BChar.ParticleOut(__instance.MySkill, skill, Targets[0]);
					}
					else
					{
						int num = 0;
						List<BattleEnemy> list = (Targets[0] as BattleEnemy).EnemyPosNum(out num);
						List<BattleChar> list2 = new List<BattleChar>();

						list2.Add(list[WrapIndex(num, -1, list.Count)]);
						list2.Add(list[WrapIndex(num, 1, list.Count)]);

						BattleSystem.DelayInput(Attack(__instance, Targets[0], list2));

					}
				}
				else if (BattleSystem.instance.AllyList.Count == 1)
				{
					Skill skill3 = Skill.TempSkill(GDEItemKeys.Skill_S_Control_0_0, __instance.BChar, __instance.BChar.MyTeam);
					skill3.AllExtendeds[0].IsDamage = true;
					if (Targets[0].BuffFind(GDEItemKeys.Buff_B_Control_P, false))
					{
						skill3.AllExtendeds[0].PlusSkillStat.cri = 100f;
						Targets[0].BuffAdd(GDEItemKeys.Buff_B_Control_P, __instance.BChar, false, 0, false, -1, false);
					}
					skill3.AllExtendeds[0].SkillBasePlus.Target_BaseDMG = (int)((float)__instance.BChar.GetStat.maxhp * 0.6f);
					__instance.BChar.ParticleOut(__instance.MySkill, skill3, Targets[0]);
				}
				else
				{
					int num2 = 0;
					for (int i = 0; i < BattleSystem.instance.AllyTeam.AliveChars.Count; i++)
					{
						if (BattleSystem.instance.AllyTeam.AliveChars[i] == Targets[0])
						{
							num2 = i;
							break;
						}
					}
					List<BattleAlly> allyList = BattleSystem.instance.AllyList;
					List<BattleChar> list3 = new List<BattleChar>();

					list3.Add(allyList[WrapIndex(num2, -1, allyList.Count)]);
					list3.Add(allyList[WrapIndex(num2, 1, allyList.Count)]);

					BattleSystem.DelayInput(Attack(__instance, Targets[0], list3));

				}
				return false;
			}
		}



		[HarmonyPatch(typeof(B_Control_6_T), nameof(B_Control_6_T.DamageTake))]
		class Sleep_Patch
		{
			static bool Prefix(B_Control_6_T __instance, BattleChar User, int Dmg, bool Cri, ref bool resist, bool NODEF = false, bool NOEFFECT = false, BattleChar Target = null)
			{
				if (!Target.BuffFind(GDEItemKeys.Buff_B_Control_P, false))
					__instance.SelfDestroy();
				return false;
			}
		}


		[HarmonyPatch(typeof(S_Control_11), nameof(S_Control_11.SkillUseSingle))]
		class S_Control_11_Patch
		{
			static void Prefix(S_Control_11 __instance, List<BattleChar> Targets)
			{
				if (!Targets[0].BuffFind(GDEItemKeys.Buff_B_Control_P, false))
					Targets[0].BuffAdd(GDEItemKeys.Buff_B_Control_P, __instance.BChar, false, 0, false, -1, false);
			}

		}




		[HarmonyPatch(typeof(S_Control_1), nameof(S_Control_1.AttackEffectSingle))]
		class Paranoia_Patch
		{
			static IEnumerator ApplyStun(Skill_Extended extend, BattleChar mainTarget, List<BattleChar> targets)
			{
				BattleChar prevT = null;
				foreach (var t in targets)
				{
					if (t.Equals(prevT) && !t.BuffFind(GDEItemKeys.Buff_B_Common_Rest))
					{
						yield return new WaitForSeconds(0.5f);
					}
					t.BuffAdd(GDEItemKeys.Buff_B_Common_Rest, extend.BChar, false, 100, false, -1, false);
					prevT = t;
				}
				yield break;
			}

			static bool Prefix(S_Control_1 __instance, BattleChar hit, SkillParticle SP, int DMG, int Heal)
			{
				if (!hit.Info.Ally)
				{
					int num = 0;
					List<BattleEnemy> list = (hit as BattleEnemy).EnemyPosNum(out num);
					List<BattleChar> targets = new List<BattleChar>();

					targets.Add(list[WrapIndex(num, -1, list.Count)]);
					targets.Add(list[WrapIndex(num, 1, list.Count)]);

					BattleSystem.DelayInput(ApplyStun(__instance, hit, targets));
					
				}
				else
				{
					int num2 = 0;
					for (int i = 0; i < BattleSystem.instance.AllyTeam.AliveChars.Count; i++)
					{
						if (BattleSystem.instance.AllyTeam.AliveChars[i] == hit)
						{
							num2 = i;
							break;
						}
					}
					List<BattleChar> aliveChars = BattleSystem.instance.AllyTeam.AliveChars;
					List<BattleChar> targets = new List<BattleChar>();

					targets.Add(aliveChars[WrapIndex(num2, -1, aliveChars.Count)]);
					targets.Add(aliveChars[WrapIndex(num2, 1, aliveChars.Count)]);
					BattleSystem.DelayInput(ApplyStun(__instance, hit, targets));


				}
				return false;
			}
		}




	}
}
