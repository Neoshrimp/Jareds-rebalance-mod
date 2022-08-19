using BepInEx;
using EItem;
using HarmonyLib;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;
using System.Reflection.Emit;

namespace Equipment_rebalance
{
    public class EquipRebalance
    {

        [HarmonyPatch(typeof(PassiveBase))]
        public class PassiveBasePatch
        {
            [HarmonyReversePatch]
            [HarmonyPatch(nameof(PassiveBase.Init))]
            [MethodImpl(MethodImplOptions.NoInlining)]
            static public void InitStub(PassiveBase instance)
            {
                return;
            }

            [HarmonyReversePatch]
            [HarmonyPatch(nameof(PassiveBase.FixedUpdate))]
            [MethodImpl(MethodImplOptions.NoInlining)]
            static public void FixedUpdateStub(PassiveBase instance)
            {
                return;
            }
        }

        [HarmonyPatch]
        class GhostBadgePatch
        {
            [HarmonyPatch(typeof(GhostBadge), "Init")]
            static bool Prefix(GhostBadge __instance)
            {
                __instance.PlusStat.dod = 8f;
                __instance.PlusStat.RES_CC = 20f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }



        [HarmonyPatch(typeof(RoseBadge), "Init")]
        class RoseBadgePatch
        {
            static bool Prefix(RoseBadge __instance)
            {
                __instance.PlusStat.cri = 4f;
                __instance.PlusStat.HIT_DOT = 15f;
                __instance.PlusStat.HIT_CC = 8f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }


        [HarmonyPatch(typeof(TargetBadge), "Init")]
        class TargetBadgePatch
        {
            static bool Prefix(TargetBadge __instance)
            {
                __instance.PlusStat.hit = 6f;
                __instance.PlusStat.RES_DEBUFF = 20f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(WoodenBat), "Init")]
        class WoodenBatPatch
        {
            static bool Prefix(WoodenBat __instance)
            {
                __instance.PlusPerStat.Damage = 24;
                __instance.PlusStat.hit = -10f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }


        [HarmonyPatch(typeof(CubicNecklace), "Init")]
        class CubicNecklacePatch
        {
            static bool Prefix(CubicNecklace __instance)
            {
                __instance.PlusPerStat.MaxHP = 15;
                __instance.PlusStat.dod = 3f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(CeremonialGloves), "Init")]
        class CeremonialGlovesPatch
        {
            static bool Prefix(CeremonialGloves __instance)
            {
                __instance.PlusPerStat.MaxHP = -15;
                __instance.PlusPerStat.Heal = 20;
                __instance.PlusStat.RES_DEBUFF = 10;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(RingofStupidman), "Init")]
        class ProtectorsRingPatch
        {
            static bool Prefix(RingofStupidman __instance)
            {
                __instance.PlusStat.def = 20f;
                __instance.PlusStat.dod = -20f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(RingofGambler), "Init")]
        class GamblersRingPatch
        {
            static bool Prefix(RingofGambler __instance)
            {
                __instance.PlusStat.dod = 10f;
                __instance.PlusStat.cri = 15f;
                __instance.PlusStat.hit = -5f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(SunCape), "Init")]
        class SunCapePatch
        {
            static bool Prefix(SunCape __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.MaxHP = 20;
                __instance.PlusStat.HIT_CC = 15f;
                __instance.PlusStat.RES_DOT = -20f;

                return false;
            }
        }

        [HarmonyPatch(typeof(CrescentCape), "Init")]
        class CrescentCapePatch
        {
            static bool Prefix(CrescentCape __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.Damage = 15;
                __instance.PlusPerStat.Heal = 15;
                __instance.PlusStat.dod = 5f;
                __instance.PlusPerStat.MaxHP = -20;

                return false;
            }
        }

        [HarmonyPatch(typeof(AmuletofAnger), "Init")]
        class AmuletOfAngerPatch
        {
            static bool Prefix(AmuletofAnger __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.def = 8f;
                __instance.PlusPerStat.MaxHP = 15;
                __instance.PlusStat.AggroPer = 50;
                __instance.PlusStat.atk = 1f;

                return false;
            }
        }

        [HarmonyPatch(typeof(AmuletofStability), "Init")]
        class AmuletOfCalmPatch
        {
            static bool Prefix(AmuletofStability __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.maxhp = 5;
                __instance.PlusStat.reg = 2f;
                __instance.PlusStat.AggroPer = -25;

                return false;
            }
        }

        [HarmonyPatch(typeof(Rustydagger), "Init")]
        class RustydaggerPatch
        {
            static bool Prefix(Rustydagger __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.atk = 1f;
                __instance.PlusStat.PlusCriDmg = 25f;
                __instance.PlusStat.PlusCriHeal = 25f;

                return false;
            }
        }

        [HarmonyPatch(typeof(BastardSword), "Init")]
        class BluntSwordPatch
        {
            static bool Prefix(BastardSword __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.Damage = 18;
                __instance.PlusStat.HEALTaken = -25f;

                return false;
            }
        }

        [HarmonyPatch(typeof(NecklaceofLife), "Init")]
        class LifeStoneNecklacePatch
        {
            static bool Prefix(NecklaceofLife __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.MaxHP = 20;
                __instance.PlusStat.RES_CC = 5f;
                __instance.PlusStat.RES_DEBUFF = 5f;
                __instance.PlusStat.RES_DOT = 5f;
                __instance.PlusStat.dod = -5f;
                return false;
            }
        }

        [HarmonyPatch(typeof(PrayersHand), "Init")]
        class HandsOfPrayerPatch
        {
            static bool Prefix(PrayersHand __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.Heal = 20;
                __instance.PlusPerStat.Damage = -25;
                __instance.PlusStat.RES_DOT = 10f;

                return false;
            }
        }


        [HarmonyPatch(typeof(CharginTarge))]
        class AssaultShieldPatch
        {
            [HarmonyPatch("Init")]
            [HarmonyPrefix]
            static bool InitPrefix(CharginTarge __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.MaxHP = 12;
                __instance.PlusStat.dod = -8f;
                __instance.PlusStat.hit = 4f;

                return false;
            }

            //bug fix
            [HarmonyPatch("FixedUpdate")]
            [HarmonyPrefix]
            static bool FixedUpdatePrefix(CharginTarge __instance)
            {
                PassiveBasePatch.FixedUpdateStub(__instance);
                if (BattleSystem.instance != null && __instance.BChar != null && __instance.BChar.GetStat.Strength)
                {
                    __instance.PlusPerStat.Damage = 15;
                }
                else
                {
                    __instance.PlusPerStat.Damage = 0;
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(RustyHammer), "Init")]
        class RustyHammerPatch
        {
            static bool Prefix(RustyHammer __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.cri = 5f;
                __instance.PlusStat.PlusCriDmg = 33f;
                __instance.PlusStat.PlusCriHeal = 33f;

                return false;
            }
        }

        [HarmonyPatch(typeof(CrossBrooch), "Init")]
        class CrossBroochPatch
        {
            static bool Prefix(CrossBrooch __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.def = 10f;
                __instance.PlusStat.cri = 35f;
                __instance.PlusStat.PlusCriDmg = -50f;
                __instance.PlusStat.RES_DEBUFF = 20f;

                return false;
            }
        }


        [HarmonyPatch(typeof(RabbitMask), "Init")]
        class DodoRabbitPatch
        {


            static bool Prefix(RabbitMask __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                if (__instance.MyItem != null)
                {
                    __instance.MyItem.Curse = new EquipCurse();

                    ItemEnchant.RandomEnchant(__instance.MyItem, string.Empty, true, false);
                    __instance.PlusStat += __instance.MyItem.Enchant.EnchantData.PlusStat;
                    __instance.PlusPerStat += __instance.MyItem.Enchant.EnchantData.PlusPerStat;

                    ItemEnchant.RandomEnchant(__instance.MyItem, string.Empty, true, false);
                    __instance.PlusStat += __instance.MyItem.Enchant.EnchantData.PlusStat;
                    __instance.PlusPerStat += __instance.MyItem.Enchant.EnchantData.PlusPerStat;

                    var goldenEchantMethod = AccessTools.Method(typeof(ItemEnchant), "RandomCurseEnchant");
                    goldenEchantMethod.Invoke(__instance, new object[] { __instance.MyItem });


                    // for whatever reason this gives MissingMethodException
                    // ItemEnchant.RandomCurseEnchant(__instance.MyItem);
                    __instance.MyItem._Isidentify = true;
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(RingofMedia), "Init")]
        class CouriersRingPatch
        {
            static bool Prefix(RingofMedia __instance)
            {

                __instance.PlusStat.dod = 15f;
                __instance.PlusStat.RES_CC = 30f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(RingofBlood), "Init")]
        class BerserkersRingPatch
        {
            static bool Prefix(RingofBlood __instance)
            {

                __instance.PlusStat.hit = -5f;
                __instance.PlusStat.cri = 10f;
                __instance.PlusPerStat.Damage = 15;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(EagleEye), "Init")]
        class EagleEyePatch
        {
            static bool Prefix(EagleEye __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.hit = 15f;
                __instance.PlusStat.cri = 4f;

                return false;
            }
        }

        [HarmonyPatch(typeof(VikingsMace), "Init")]
        class BerserkersMacePatch
        {
            static bool Prefix(VikingsMace __instance)
            {
                __instance.PlusStat.atk = 2f;
                __instance.PlusStat.HEALTaken = 33f;
                __instance.PlusStat.HIT_DEBUFF = 20f;
                __instance.PlusStat.DMGTaken = 15f;
                __instance.PlusStat.maxhp = __instance.PlusStat.maxhp + 4;

                return false;
            }
        }

        [HarmonyPatch(typeof(SceptreofLife), "Init")]
        class LifeStoneScepterPatch
        {
            static bool Prefix(VikingsMace __instance)
            {
                __instance.PlusPerStat.Heal = 20;
                __instance.PlusStat.RES_DOT = 20f;
                __instance.PlusStat.RES_CC = -15f;
                return false;
            }
        }

        [HarmonyPatch(typeof(Taegeukring), "Init")]
        class TaegukringPatch
        {
            static bool Prefix(Taegeukring __instance)
            {
                __instance.PlusStat.atk = 3f;
                __instance.PlusStat.reg = 3f;
                __instance.PlusStat.cri = -9f;
                return false;
            }
        }

        [HarmonyPatch(typeof(RingofDeath), "Init")]
        class AssassinRingPatch
        {
            static bool Prefix(RingofDeath __instance)
            {
                __instance.PlusStat.PlusCriDmg = 15f;
                __instance.PlusStat.cri = 10f;
                __instance.PlusPerStat.MaxHP = -10;
                __instance.PlusStat.HIT_DOT = 10f;
                return false;
            }
        }


        //TODO update description 
        [HarmonyPatch(typeof(EndlessScroll), "Enchent")]
        class MagicParchmentPatch
        {
            //it would probably break if 2 > magic parchments are enchanted at the same time
            static int enchantDepth = 0;
            static bool Prefix(EndlessScroll __instance)
            {
                __instance.PlusStat += __instance.MyItem.Enchant.EnchantData.PlusStat;
                __instance.PlusPerStat += __instance.MyItem.Enchant.EnchantData.PlusPerStat;
                __instance.EnchantNames += __instance.MyItem.Enchant.Name;

                __instance.MyItem.Enchant.EnchantData.PlusStat = default(Stat);
                __instance.MyItem.Enchant.EnchantData.PlusPerStat = default(PerStat);
                __instance.MyItem.Enchant.Name = __instance.EnchantNames;

                if (enchantDepth < 1)
                {
                    enchantDepth++;

                    ItemEnchant.RandomEnchant(__instance.MyItem, string.Empty, true, false);
                }

                enchantDepth = 0;

                return false;
            }

        }


        [HarmonyPatch(typeof(Featheroflife), "Init")]
        class BurningFeatherPatch
        {
            static bool Prefix(Featheroflife __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.MaxHP = -50;
                __instance.PlusStat.dod = 15f;
                __instance.PlusStat.Strength = true;

                return false;
            }
        }

        [HarmonyPatch(typeof(GuardsCertificate), "Init")]
        class GuardsDeedPatch
        {
            static bool Prefix(GuardsCertificate __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.def = 15f;
                __instance.PlusStat.atk = 2f;
                __instance.PlusStat.HIT_CC = 20f;
                __instance.PlusStat.RES_CC = 20f;
                __instance.PlusStat.dod = -10f;
                return false;
            }
        }

        [HarmonyPatch(typeof(StickofFaith), "Init")]
        class RodOfFaithPatch
        {
            static bool Prefix(StickofFaith __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.reg = 6f;
                __instance.PlusStat.atk = -3f;
                __instance.PlusStat.maxhp = 4;

                return false;
            }
        }

        [HarmonyPatch(typeof(ArmletofGambler), "Init")]
        class GamblersArmletPatch
        {
            static bool Prefix(ArmletofGambler __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.Damage = -15;
                __instance.PlusPerStat.Heal = -15;
                __instance.PlusStat.cri = 50f;
                __instance.PlusStat.PlusCriDmg = 50f;
                __instance.PlusStat.PlusCriHeal = 50f;

                return false;
            }
        }

        [HarmonyPatch(typeof(DochiHat), "Init")]
        class HedgehogHatPatch
        {
            static bool Prefix(DochiHat __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.Heal = 15;

                return false;
            }
        }


        [HarmonyPatch(typeof(LastStand), "Init")]
        class LastStandPatch
        {
            static bool Prefix(LastStand __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.atk = 1;
                __instance.PlusStat.def = 10f;
                __instance.PlusStat.RES_DEBUFF = -20f;

                return false;
            }
        }

        //TODO discuss implementation 
        //TODO change description
        [HarmonyPatch(typeof(Extended_ForbiddenLibram), nameof(Extended_ForbiddenLibram.SkillUseSingle))]
        class ForbiddenBibleExtendPatch
        {
            static bool Prefix(Extended_ForbiddenLibram __instance, Skill SkillD, List<BattleChar> Targets)
            {
                int heal = 50 * Random.Range(0, 6) - 100;
                //incorrect
                int baseHeal = __instance.MySkill.MySkill.Effect_Target.HEAL_Per;
                float mul = 0.5f * Random.Range(0, 6);
                Debug.Log(baseHeal);
                Debug.Log(mul);

                __instance.PlusSkillPerStat.Heal = Mathf.Max((int)(baseHeal * mul - baseHeal), -baseHeal);
                Debug.Log(__instance.PlusSkillPerStat.Heal);


                //__instance.PlusSkillPerStat.Heal = heal; 


                return false;
            }
        }

        [HarmonyPatch(typeof(FrozenShuriken), "Init")]
        class FrozenCommendationPatch
        {
            static bool Prefix(FrozenShuriken __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.Damage = 15;
                __instance.PlusStat.DeadImmune = 15;

                return false;
            }
        }

        [HarmonyPatch(typeof(ScalesArmor), "Init")]
        class ScaleVestPatch
        {
            static bool Prefix(ScalesArmor __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.dod = __instance.PlusStat.dod - 5f;
                __instance.PlusStat.cri = __instance.PlusStat.cri + 20f;

                return false;
            }
        }

        [HarmonyPatch(typeof(StellarHand), "Init")]
        class HandOfStarsPatch
        {
            static bool Prefix(StellarHand __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.Damage = 15;
                __instance.PlusPerStat.Heal = 10;
                __instance.PlusStat.cri = 3f;
                __instance.PlusStat.HIT_DEBUFF = 20f;
                __instance.PlusStat.HIT_DOT = -25f;
                __instance.PlusStat.hit = 3f;

                return false;
            }
        }


        [HarmonyPatch(typeof(HighPriestsLegacy), "Init")]
        class HighPriestsLegacyPatch
        {
            static bool Prefix(HighPriestsLegacy __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.Heal = 12;
                __instance.PlusStat.hit = 3f;
                __instance.PlusStat.atk = 2f;

                return false;
            }
        }

        [HarmonyPatch(typeof(TheEquability), "Init")]
        class NecktieOfTheProtectorPatch
        {
            static bool Prefix(TheEquability __instance)
            {
                __instance.PlusStat.def = 15f;
                __instance.PlusStat.hit = 5f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }


        [HarmonyPatch(typeof(OrderofSacrifice), "Init")]
        class TokenOfCommitmentPatch
        {
            static bool Prefix(OrderofSacrifice __instance)
            {
                __instance.PlusPerStat.Heal = 18;
                __instance.PlusStat.dod = 7f;
                __instance.PlusStat.RES_CC = 33f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(OrderofValiancy), "Init")]
        class TokenOfValorPatch
        {
            static bool Prefix(OrderofValiancy __instance)
            {
                __instance.PlusPerStat.Damage = 20;
                __instance.PlusStat.RES_DEBUFF = 30f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(ThePressure))]
        class BaneStonePatch
        {
            // change these to tweak stat values
            static int maxhp = 7;
            static int damagePer = 15;
            static float hit = 5f;

            [HarmonyPatch(nameof(ThePressure.Init))]
            [HarmonyPrefix]
            static bool InitPrefix(ThePressure __instance)
            {
                __instance.PlusStat.maxhp = maxhp;
                __instance.PlusPerStat.Damage = damagePer;
                __instance.PlusStat.hit = hit;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }

            [HarmonyPatch(nameof(ThePressure.FixedUpdate))]
            [HarmonyPrefix]
            static bool FixedUpdatePrefix(ThePressure __instance)
            {
                if (BattleSystem.instance != null && BattleSystem.instance.FogTurn <= BattleSystem.instance.TurnNum)
                {
                    __instance.PlusStat.maxhp = 0;
                    __instance.PlusPerStat.Damage = 0;
                    __instance.PlusStat.hit = 0f;
                }
                else
                {
                    __instance.PlusStat.maxhp = maxhp;
                    __instance.PlusPerStat.Damage = damagePer;
                    __instance.PlusStat.hit = hit;
                }
                PassiveBasePatch.FixedUpdateStub(__instance);

                return false;
            }

            [HarmonyPatch(nameof(ThePressure.BattleEnd))]
            [HarmonyPrefix]
            static bool BattleEndPrefix(ThePressure __instance)
            {
                __instance.PlusStat.maxhp = maxhp;
                __instance.PlusPerStat.Damage = damagePer;
                __instance.PlusStat.hit = hit;
                return false;
            }
        }

        [HarmonyPatch(typeof(OrderofEgis), "Init")]
        class TokenOfProtectionPatch
        {
            static bool Prefix(OrderofEgis __instance)
            {
                __instance.PlusStat.maxhp = __instance.PlusStat.maxhp + 3;
                __instance.PlusStat.def = 12f;
                __instance.PlusStat.DeadImmune = 20;
                __instance.PlusStat.HIT_CC = 30f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(OrderofHonor), "Init")]
        class TokenOfExecutionerPatch
        {
            static bool Prefix(OrderofHonor __instance)
            {
                __instance.PlusStat.atk = 1f;
                __instance.PlusStat.reg = 1f;
                __instance.PlusStat.crihit = 5;
                __instance.PlusStat.Penetration = 15f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        //TODO maybe move changes to a new item
        [HarmonyPatch(typeof(BloodyMary), "Init")]
        class BloodyMaryPatch
        {
            static bool Prefix(BloodyMary __instance)
            {
                __instance.PlusPerStat.Heal = 28;
                __instance.PlusStat.spd = -1;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(Deadeye), "Init")]
        class AssassinsEyePatch
        {
            static bool Prefix(Deadeye __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.cri = 12f;
                __instance.PlusStat.PlusCriDmg = 25f;
                __instance.PlusStat.hit = 7f;

                return false;
            }
        }

        //TODO reconsider stats
        [HarmonyPatch(typeof(Dooooooom), "Init")]
        class DemonHunterPatch
        {
            static bool Prefix(Dooooooom __instance)
            {

                __instance.PlusPerStat.Damage = 20;
                __instance.PlusStat.cri = -5f;
                __instance.PlusStat.hit = 5f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        //TODO test for unexpected behavior. Works with shadow orb
        [HarmonyPatch(typeof(DemonHunter_Ex), nameof(DemonHunter_Ex.SkillUseHand))]
        class DemonHunter_ExPatch
        {
            static bool Prefix(DemonHunter_Ex __instance)
            {
                while (BattleSystem.instance.AllyTeam.Skills.Count > 0)
                {
                    BattleSystem.instance.AllyTeam.Skills[0].MyButton.Waste();
                }

                return false;
            }
        }


        //TODO update description
        [HarmonyPatch(typeof(CrownofThorns))]
        class CrownOfThornsPatch
        {
            [HarmonyPatch("Init")]
            [HarmonyPrefix]
            static bool InitPrefix(CrownofThorns __instance)
            {

                __instance.PlusPerStat.Heal = 18;
                __instance.PlusStat.DeadImmune = 25;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }

            [HarmonyPatch("FixedUpdate")]
            [HarmonyPostfix]
            static void FixedUpdatePostfix(CrownofThorns __instance)
            {
                if (__instance.BChar != null && __instance.BChar.HP <= 0)
                {
                    __instance.PlusStat.dod = 25f;
                }
                else
                {
                    __instance.PlusStat.dod = 0f;
                }
            }
        }


        [HarmonyPatch(typeof(HolySwordKarsaga), "Init")]
        class HolySwordKarsagaPatch
        {
            static bool Prefix(HolySwordKarsaga __instance)
            {

                __instance.PlusStat.atk = 2f;
                __instance.PlusStat.reg = 2f;
                __instance.PlusStat.HIT_DEBUFF = 24f;
                __instance.PlusStat.HIT_DOT = 24f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }


        [HarmonyPatch(typeof(HandPrintofGrimReaper), "Init")]
        class ReapersHandprintPatch
        {
            static bool Prefix(HandPrintofGrimReaper __instance)
            {
                __instance.PlusStat.atk = 2f;
                __instance.PlusStat.cri = -10f;
                __instance.PlusStat.PlusCriDmg = 66f;
                __instance.PlusStat.HIT_CC = 15f;
                __instance.PlusStat.HIT_DEBUFF = 15f;
                __instance.PlusStat.HIT_DOT = 15f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(AgentSunglass), "Init")]
        class AgentsSunglassPatch
        {
            static bool Prefix(AgentSunglass __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.HitMaximum = true;
                __instance.PlusStat.hit = 25f;

                return false;
            }
        }

        [HarmonyPatch(typeof(Revenger), "Init")]
        class RevengerPatch
        {
            static bool Prefix(Revenger __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.AggroPer = 33;
                __instance.PlusStat.def = 15f;
                __instance.PlusStat.atk = 3f;
                __instance.PlusStat.dod = -15f;

                return false;
            }
        }

        [HarmonyPatch(typeof(MessiahbladesPrototype), "Init")]
        class MessiahBladesPrototypePatch
        {
            static bool Prefix(MessiahbladesPrototype __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.atk = 2f;
                __instance.PlusStat.hit = 5f;
                __instance.PlusStat.dod = 8f;

                return false;
            }
        }


        [HarmonyPatch(typeof(DolorousStroke), "Init")]
        class DolorousStrikePatch
        {
            static bool Prefix(DolorousStroke __instance)
            {
                __instance.PlusPerStat.MaxHP = 20;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(FlameDarkSword), "Init")]
        class DarkFlameSwordPatch
        {
            static bool Prefix(FlameDarkSword __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.Damage = 20;
                __instance.PlusStat.hit = 5f;
                __instance.PlusPerStat.MaxHP = -15;

                return false;
            }
        }

        [HarmonyPatch(typeof(Analyticalscope), "Init")]
        class HeavenlyWatcherPatch
        {
            static bool Prefix(Analyticalscope __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusPerStat.Damage = 12;
                __instance.PlusStat.hit = 7f;

                return false;
            }
        }

        [HarmonyPatch(typeof(GasMask), "Init")]
        class ChemicalGasTankPatch
        {
            static bool Prefix(GasMask __instance)
            {
                PassiveBasePatch.InitStub(__instance);
                __instance.PlusStat.reg = 1f;
                __instance.PlusStat.HIT_DOT = 10f;

                return false;
            }
        }

        [HarmonyPatch(typeof(FlameShieldGenerator), "Init")]
        class BurningNightPatch
        {
            static bool Prefix(FlameShieldGenerator __instance)
            {
                __instance.PlusStat.maxhp = 8;
                __instance.PlusStat.crihit = -10;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        // TODO change description 
        [HarmonyPatch(typeof(FoxOrb), "KillEffect")]
        class SealedOrbPatch
        {
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var list = instructions.ToList();
                list.ForEach(ci =>
                {
                    if (ci.opcode == OpCodes.Ldc_I4_S && ci.operand.ToString() == "15")
                        ci.operand = 13;
                });
                return list.AsEnumerable();
            }
        }


        [HarmonyPatch(typeof(FoxOrb_0), "Init")]
        class FoxOrbPatch
        {
            static bool Prefix(FoxOrb_0 __instance)
            {
                __instance.PlusPerStat.Damage = 20;
                __instance.PlusStat.dod = 8f;
                __instance.PlusStat.Penetration = 25f;
                __instance.PlusStat.hit = 4f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(VikingsBlood), "Init")]
        class BerserkersBloodPatch
        {
            static bool Prefix(VikingsBlood __instance)
            {
                __instance.PlusStat.def = 12f;
                __instance.PlusStat.cri = 8f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }


        [HarmonyPatch(typeof(PoisonousBottle), "Init")]
        class BottleOfPoisondPatch
        {
            static bool Prefix(PoisonousBottle __instance)
            {
                __instance.PlusStat.HIT_DOT = 20f;
                __instance.PlusStat.RES_DOT = 20f;
                PassiveBasePatch.InitStub(__instance);

                return false;
            }
        }

    }
}
