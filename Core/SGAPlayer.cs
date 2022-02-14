using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Idglibrary;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Shaders;
using SGAmod.Items.Weapons;
using SGAmod.NPCs;
using SGAmod.NPCs.Wraiths;
using SGAmod.NPCs.Cratrosity;
using SGAmod.NPCs.Murk;
using SGAmod.NPCs.Sharkvern;
using SGAmod.NPCs.SpiderQueen;
using SGAmod.NPCs.Hellion;
using SGAmod.Items.Consumables;
using CalamityMod;
using AAAAUThrowing;
using Terraria.Utilities;
using SGAmod.SkillTree;
using SGAmod.Dimensions;
using SGAmod.Items.Accessories;
using SGAmod.Buffs;
using SGAmod.Items.Weapons.Technical;

namespace SGAmod
{

	public partial class SGAPlayer : ModPlayer
	{
		public List<ActionCooldownStack> CooldownStacks;
		public List<(int, float)> DoTStack = new List<(int, float)>();

		public SkillManager skillMananger;
		public int surprised = 0;
		public float[] beserk = { 0, 0 };
		public int previoustf2emblemLevel = 0;
		public int lockoneffect = 0;
		public int gunslingerLegendtarget = -1;
		public int gunslingerLegendtargettype = 0;
		public bool Duster = false;
		public bool Drakenshopunlock = false; public bool benchGodFavor = false;
		public int DefenseFrame = 0;
		public int ReloadingRevolver = 0;
		public int CustomWings = 0;
		public int JoyrideShake = 0;
		public bool Walkmode = false;
		public bool Shieldbreak = false;
		public bool invincible = false;
		public int ShieldType = 0;
		public int ShieldTypeDelay = 0;
		public int realIFrames = 0;
		public int myammo = 0;
		public int timer = 0;
		public int beefield = 0;
		public int ninjaStashLimit = 0;
		public int molotovLimit = 0;
		public float morespawns = 1f;
		public int midasMoneyConsumed = 0;
		public int manaBoost = 0;
		public float DoTResist = 1f;
		public int potionFatigue = 0;
		public byte invertedTime = 0;
		public (int, byte) skylightLightInfused = (0, 0);
		public (int, int) PolarityHarbPower = (0, 0);


		//For holding Trap Weapons
		public float SlowDownDefense = 0f;
		public float SlowDownResist = 1f;
		public int SlowDownReset = 0;
		public int heldShield = -1;
		public int heldShieldReset = 0;

		//Potion related
		public float trueMeleeDamage = 1f;
		public float triggerFinger = 1f;
		public int intimacy = 0;
		public int toxicity = 0;
		public bool IceFire = false;

		public float mspeed = 1f;
		public int beefieldtoggle = 0;
		public int beefieldcounter = 0;
		public bool HeavyCrates = false;
		public int uncraftBoost = 0;
		public bool Microtransactions = false;
		public bool MoneyMismanagement = false;
		public bool lavaBurn = false;
		public bool NoFly = false;
		public Vector2 drowningIncrementer = Vector2.Zero;
		public bool permaDrown = false;
		public bool Pressured = false;
		public bool MassiveBleeding = false;
		public bool ELS = false;
		public bool ActionCooldown = false;
		public bool thermalblaze = false; public bool acidburn = false; public int badLifeRegen = 0;
		public int FieryheartBuff = 0;
		public int creeperexplosion = 0;
		public bool DankShrineZone = false;
		public byte ShadowSectorZone = 0;
		public DarkSector ShadowSector = default;

		public bool noModTeleport = false;
		private Projectile[] projectileslunarslime = new Projectile[15];
		public static Dictionary<int, int> ShieldTypes = new Dictionary<int, int>();
		public int lunarSlimeHeartdamage = 1;
		public int lunarSlimeCounter = 0;
		public bool SunderedDefense = false;
		public int watcherDebuff = 0;

		//Accessory related
		public bool CirnoWings = false;
		public bool manaUnchained = false;
		public bool SerratedTooth = false;
		public int grippinggloves = 0; public int grippingglovestimer = 0;
		public bool vibraniumSetPlatform = false; public bool vibraniumSetWall = false;
		public bool mudbuff = false; public bool alkalescentHeart = false; public bool jabALot = false; public bool NoHitCharm = false; public int NoHitCharmTimer = 0;
		public int Havoc = 0;
		public int Novusset = 0; public int Noviteset = 0; public bool Blazewyrmset = false; public bool SpaceDiverset = false; public bool MisterCreeperset = false; public bool Mangroveset = false; public int Dankset = 0; public bool IDGset = false; public bool jellybruSet = false; public bool vibraniumSet = false; public (bool, float, bool, float) valkyrieSet = (false, 0, false, 0); public (bool, bool) acidSet = (false, false); public (int, int) illuminantSet = (0, 0); public (bool, bool) jungleTemplarSet = (false, false); public bool magatsuSet = false; public bool desertSet = false; public (bool, int) mandalaSet = (false, 0);
		public float SpaceDiverWings = 0f;
		public int novusBoost = 0;
		public int gamePadAutoAim = 0;
		public int tidalCharm = 0;
		public bool personaDeck = false;
		public bool lunarSlimeHeart = false;
		public bool LifeFlower = false; public bool GeyserInABottle = false; public bool GeyserInABottleActive = false; public bool JavelinBaseBundle = false; public bool JavelinSpearHeadBundle = false; public bool PrimordialSkull = false;
		public bool MatrixBuffp = false; public bool BoosterMagnet = false; public bool HoE = false; public bool CalamityRune = false; public bool RadSuit = false;
		public int EnhancingCharm = 0;
		public bool noactionstackringofrespite = false;
		public bool enchantedShieldPolish = false; public bool diesIraeStone = false; public bool magusSlippers = false; public bool airTank = false; public bool murkyCharm = false;
		public int tf2emblemLevel = 0;
		public int ninjaSash = 0;
		public int shinobj = 0;
		public int soldierboost = 0;
		public bool voidEmbrancers = false;
		public bool transformerAccessory = false;
		public bool gravBoots = false;
		public bool undyingValor = false;
		public (bool,int) bustlingFungus = (false,0);
		public bool highStakesSet = false;
		public int liquidGambling = 0;
		public bool experimentalPathogen = false;
		public bool concussionDevice = false;
		public float concussionDeviceEffectiveness = 0f;
		public FlaskOfBlaze flaskBuff = default;
		public (bool, int) snakeEyes = (false, 0);
		public (float, float) auraBoosts = (0, 0);
		public bool russianRoulette = false;
		public bool dualityshades = false;
		public bool gunslingerLegend = false;
		public bool YoyoTricks = false;
		public bool twinesoffate = false;
		public bool MVMBoost = false;
		public bool SybariteGem = false;
		public bool restorationFlower = false;
		public bool tpdcpu = false;
		public byte cobwebRepellent = 0;
		public bool aversionCharm = false;
		public byte avariceRing = 0;
		public int devpower = 0;
		public bool EALogo = false;
		public bool graniteMagnet = false;
		public bool demonsteppers = false;
		public bool FridgeflameCanister = false;
		public bool terraDivingGear = false;
		public bool glacialStone = false;
		public bool rustedBulwark = false;
		public bool novusStackBoost = false;
		public bool BIP = false;
		public bool refractor = false;
		public int phaethonEye = 0;
		public bool armorToggleMode = false;
		public bool bustedSpawningGear = false;

		public bool Lockedin = false;
		private int lockedelay = 0;
		public int NoFireBurn = 0;
		public int breathingdelay = 0;
		public int sufficate = 200;
		public int finalGem = 0;
		public int sandStormTimer = 0;
		public int postLifeRegenBoost = 0;

		//Stat Related
		public float UseTimeMul = 1f;
		public bool noLifeRegen = false;
		public int drownRate = 0;
		public ushort FireBreath = 0;
		public int MaxCooldownStacks = 1;
		public float RevolverSpeed = 1f;
		public int forcedMiningSpeed = 100000;
		public float damagetaken = 1f;
		public float knockbackTaken = 1f;
		public int consumeCurse = 0;
		public float shieldDamageReduce = 0f;
		public float shieldDamageBoost = 0f;
		public float actionCooldownRate = 1f;
		public int shieldBlockTime = 0;
		public float shieldBlockAngle = 0;
		public int disabledAccessories = 0;
		public int centerOverrideTimer = 0;
		public static int centerOverrideTimerIsActive = 0;
		public Vector2 centerOverridePosition = default;

		public string MoneyCollected => (midasMoneyConsumed / (float)Item.buyPrice(1)) + " platinum collected";

		public float ActionCooldownRate
		{
			get
			{
				return actionCooldownRate;
			}
			set
			{
				if (value < 0.50f)
				{
					actionCooldownRate = 0.50f - (float)Math.Pow(-(actionCooldownRate + 0.50f), 0.75f);
				}

				actionCooldownRate = value;// Math.Max(0.5f, actionCooldownRate);
			}
		}


		public bool Noselfdamage = false;
		public float UseTimeMulPickaxe = 1f;
		public float summonweaponspeed = 1f;
		public float TrapDamageMul = 1f; public float TrapDamageAP = 0f;
		public float ThrowingSpeed = 1f; public float Thrownsavingchance = 0f;
		public Vector2 Locked = new Vector2(100, 300);
		public int electricCharge = 0; public int electricChargeMax = 0; public float electricChargeCost = 1f; public float electricChargeReducedDelay = 1f;
		public int ammoLeftInClip = 6; public int plasmaLeftInClip = 1000;
		public float electricdelay = -500; public int boosterrechargerate = 15; public int electricrechargerate = 1; public float electricRechargeRateMul = 1f;
		public int ammoLeftInClipMaxLastHeld = 0;
		public int ammoLeftInClipMaxStack = 0;
		public int ammoLeftInClipMaxAddedAmmo = 0;
		public int ammoLeftInClipMax = 6;
		public float energyShieldReservation = 0f;
		public (int, int, int) energyShieldAmmountAndRecharge = (0, 0, 0);
		public (int, int) GetEnergyShieldAmmountAndRecharge => ((int)(energyShieldAmmountAndRecharge.Item1 * techdamage), (int)(energyShieldAmmountAndRecharge.Item2 * techdamage));


		public int plasmaLeftInClipMax = 1000;
		public const int plasmaLeftInClipMaxConst = 1000; public const int boosterPowerLeftMaxConst = 10000;
		public int boosterPowerLeft = 10000; public int boosterPowerLeftMax = 10000; public float boosterdelay = -500;
		public int digiStacks = 0; public int digiStacksMax = 0;
		public int digiStacksCount = 16;
		public bool modcheckdelay = false;
		public bool gottf2 = false; public bool gothellion = false;
		public int floatyeffect = 0;
		public int PrismalShots = 0;
		public float beedamagemul = 1f;
		public bool JaggedWoodenSpike = false; public bool JuryRiggedSpikeBuckler = false; public bool HeartGuard = false; public bool GoldenCog = false;
		public bool devpowerbool = false; public int Redmanastar = 0; public int Electicpermboost = 0;
		public int MidasIdol = 0; public bool OmegaSigil = false; public bool starCollector = false;
		public bool MurkyDepths = false;
		public int[] ammoinboxes = new int[4];
		public int anticipation = 0; public int anticipationLevel = 0;
		public float recoil = 0;
		public int greandethrowcooldown = 0;
		public int? resetver = 1;
		public int claySlowDown = 0;
		public bool nightmareplayer = false;
		public bool playercreated = false;
		public bool granteditems = false;

		//tech damage
		public float techdamage = 1f;

		//apocalyptical related
		public double[] apocalypticalChance = { 0, 0, 0, 0 };
		public float apocalypticalStrength = 1f;
		public float lifestealentropy = 0f;

		public int entropyCollected = 0;
		public int activestacks = 0;
		public float greedyperc = 0f;
		public int maxblink = 0;
		public int potionsicknessincreaser = 0;

		public bool dragonFriend = false;
		public Point benchGodItem = new Point(-1, -1);
		public string[] armorglowmasks = new string[4];
		public int[] devempowerment = { 0, 0, 0, 0 };
		public Func<Player, int, Color>[] armorglowcolor = {delegate (Player player,int index)
		{
			return Color.White;
		},
			delegate (Player player,int index)
		{
			return Color.White;
		},
			delegate (Player player,int index)
		{
			return Color.White;
		},
			delegate (Player player,int index)
		{
			return Color.White;
		}
	};

		public List<int> ExpertisePointsFromBosses;
		public List<string> ExpertisePointsFromBossesModded;
		public List<int> ExpertisePointsFromBossesPoints;
		public int ExpertiseCollected = 0;
		public int ExpertiseCollectedTotal = 0;

		public int downedHellion = 0;

		public int Microtransactionsdelay = 0;

		public int manifestedWeaponType = 0;

		public bool EnergyDepleted => GetEnergyShieldAmmountAndRecharge.Item1 < 1 && energyShieldAmmountAndRecharge.Item3 > 0;

		public override void ResetEffects()
		{
			invincible = false;
			manifestedWeaponType = 0;
			watcherDebuff = 0;
			MVMBoost = false;
			gunslingerLegend = false;
			if (!player.HasBuff(mod.BuffType("ConsumeHellBuff")))
				FireBreath = 0;
			beserk[0] -= 1;
			if (beserk[0] < 1)
			{
				beserk[0] = 0; beserk[1] = 0;
			}
			if (invertedTime > 0)
				invertedTime -= 1;

			if (soldierboost > 0)
				soldierboost -= 1;
			if (gamePadAutoAim > 0)
				gamePadAutoAim -= 1;
			if (novusBoost > 0)
				novusBoost -= 1;

			badLifeRegen = 0;
			noLifeRegen = false;
			uncraftBoost = Math.Max(uncraftBoost - 1, 0);
			surprised = Math.Max(surprised - 1, 0);
			tidalCharm = (int)MathHelper.Clamp(tidalCharm - Math.Sign(tidalCharm), -1000, 1000);
			shinobj -= 1;

			heldShieldReset -= 1;
			if (heldShieldReset < 1)
				heldShield = -1;
			finalGem -= 1;

			claySlowDown = Math.Max(claySlowDown - 1, 0);

			shieldDamageReduce = 0f;
			shieldDamageBoost = 0f;


			energyShieldReservation = 0f;
			if (energyShieldAmmountAndRecharge.Item1 > energyShieldAmmountAndRecharge.Item2)
			{
				energyShieldAmmountAndRecharge.Item1 = energyShieldAmmountAndRecharge.Item2;
			}
			energyShieldAmmountAndRecharge.Item2 = 0;

			if (!Shieldbreak || energyShieldAmmountAndRecharge.Item3 > 1)
				energyShieldAmmountAndRecharge.Item3 -= 1;

			disabledAccessories = Math.Max(disabledAccessories - 1, 0);
			centerOverrideTimer = Math.Max(centerOverrideTimer - 1, -300);
			if (centerOverrideTimer < 1)
			{
				centerOverridePosition = player.MountedCenter;
			}
			avariceRing = 0;
			manaUnchained = false;
			snakeEyes = (false, snakeEyes.Item2);
			enchantedShieldPolish = false;
			diesIraeStone = false;
			starCollector = false;
			magusSlippers = false;
			transformerAccessory = false;
			previoustf2emblemLevel = tf2emblemLevel;
			tf2emblemLevel = 0;
			ninjaSash = 0;
			RevolverSpeed = 1f;
			intimacy = 0;
			toxicity = 0;
			consumeCurse = 0;
			russianRoulette = false;
			if (ReloadingRevolver > 0)
				ReloadingRevolver -= 1;
			if (molotovLimit > 0)
				molotovLimit -= 1;

			if (grippingglovestimer < 1)
			{
				grippingglovestimer = 0;
				grippinggloves = 0;
			}
			else
			{
				grippingglovestimer -= 1;
			}

			gravBoots = false;
			voidEmbrancers = false;
			twinesoffate = false;
			jabALot = false;
			glacialStone = false;
			terraDivingGear = false;
			Duster = false;
			flaskBuff = default;
			dualityshades = false;
			forcedMiningSpeed = 100000;
			realIFrames -= 1;
			HeavyCrates = false;
			Microtransactions = false;
			MoneyMismanagement = false;
			Lockedin = false;
			NoFly = false;
			airTank = false;
			murkyCharm = false;
			permaDrown = false;
			trueMeleeDamage = 1f;
			knockbackTaken = 1f;
			triggerFinger = 1f;
			CirnoWings = false;
			MassiveBleeding = false;
			lavaBurn = false;
			thermalblaze = false; acidburn = false; ELS = false;
			SerratedTooth = false;
			aversionCharm = false;
			SybariteGem = false;
			personaDeck = false;
			highStakesSet = false;
			undyingValor = false;
			refractor = false;

			if (liquidGambling > 0)
				liquidGambling--;

			experimentalPathogen = false;
			concussionDevice = false;
			if (bustlingFungus.Item1)
			{
				bustlingFungus = (false, player.velocity.Length()>0.05f ? 0 : bustlingFungus.Item2+1);
			}
			else
			{
				bustlingFungus = (false, 0);
			}
			concussionDeviceEffectiveness = 0f;
			UseTimeMul = 1f;
			UseTimeMulPickaxe = 1f;
			ThrowingSpeed = 1f;
			SpaceDiverset = false;
			acidSet = (false, false);
			jungleTemplarSet = (false, false);
			desertSet = false;
			potionsicknessincreaser = 0;
			Blazewyrmset = false;
			Mangroveset = false;
			IDGset = false;
			jellybruSet = false;
			vibraniumSet = false;
			mandalaSet = (false, mandalaSet.Item2);
			sandStormTimer = Math.Max(sandStormTimer - 1, 0);

			shieldBlockTime = 0;
			shieldBlockAngle = 0;

			if (valkyrieSet.Item1 || valkyrieSet.Item2 > 0)
			{
				if ((!valkyrieSet.Item1 || valkyrieSet.Item3) && valkyrieSet.Item2 > 0)
				{
					valkyrieSet.Item2 -= 0.02f;
					valkyrieSet.Item2 /= 1.1f;
				}
				valkyrieSet.Item4 += ((valkyrieSet.Item3 ? 0.15f : 1f) - valkyrieSet.Item4) * 0.20f;
			}

			valkyrieSet.Item1 = false;
			valkyrieSet.Item3 = false;

			magatsuSet = false;

			illuminantSet = (0, illuminantSet.Item2);
			novusStackBoost = false;
			Pressured = false;
			Havoc = 0;
			graniteMagnet = false;
			SunderedDefense = false;
			lockoneffect = Math.Min(lockoneffect + 1, 5000);


			if (!NoHitCharm)
			{
				if (NoHitCharmTimer > 999999)
				{
					player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " attempted to break free of Cataclysm"), 666666, 0);
				}

				NoHitCharmTimer = Math.Min(NoHitCharmTimer + 1, 181);
			}

			NoHitCharm = false;

			if (ammoLeftInClip > ammoLeftInClipMax)
				ammoLeftInClip = ammoLeftInClipMax;

			ammoLeftInClipMaxStack = 0;
			auraBoosts = (0, 1f);
			SpaceDiverWings = 0f;
			ActionCooldown = false;
			lunarSlimeHeart = false;
			alkalescentHeart = false;
			rustedBulwark = false;
			TrapDamageMul = 1f; TrapDamageAP = 0f;
			Thrownsavingchance = 0f;
			LifeFlower = false; GeyserInABottleActive = false; JavelinBaseBundle = false; JavelinSpearHeadBundle = false; restorationFlower = false;
			BoosterMagnet = false;
			EnhancingCharm = 0;
			if (devpower > 0)
				devpower -= 1;
			devpowerbool = false;
			MisterCreeperset = false;
			Noselfdamage = false;
			JaggedWoodenSpike = false; JuryRiggedSpikeBuckler = false; HeartGuard = false; GoldenCog = false;
			MidasIdol = 0;
			YoyoTricks = false;
			OmegaSigil = false;
			tpdcpu = false;
			cobwebRepellent = 0;
			if (phaethonEye > 0)
				phaethonEye -= 1;
			MurkyDepths = false;
			MatrixBuffp = false;
			plasmaLeftInClipMax = 1000;
			beedamagemul = 1f;
			anticipationLevel = -1;
			noModTeleport = false;
			PrimordialSkull = false;
			NoFireBurn = Math.Max(NoFireBurn - 1, 0);
			if (player.itemTime < 1)
				recoil = Math.Max(recoil - 0.5f, 0f);
			greandethrowcooldown = Math.Max(greandethrowcooldown - 1, 0);
			techdamage = 1f;
			HoE = false;
			CalamityRune = false;
			RadSuit = false;
			for (int a = 0; a < apocalypticalChance.Length; a++)
				apocalypticalChance[a] = 0;
			apocalypticalStrength = 1f;
			greedyperc = 0f;
			lifestealentropy = Math.Min(lifestealentropy + 0.20f, 500);
			maxblink = 0;
			EALogo = false;
			demonsteppers = false;
			IceFire = false;
			FridgeflameCanister = false;
			BIP = false;
			morespawns = 1f;
			damagetaken = 1f;
			drownRate = 0;
			summonweaponspeed = 1f;
			SlowDownReset -= 1;
			timer += 1;
			mudbuff = false;
			boosterdelay -= 1;
			digiStacks = (int)MathHelper.Clamp(digiStacks, 0, digiStacksMax);
			CustomWings = 0;
			JoyrideShake -= 1;
			DoTResist = 1f;

			if (!Shieldbreak)
				electricdelay -= 1;

			boosterPowerLeft = Math.Min(boosterPowerLeft + (boosterdelay < 1 ? boosterrechargerate : 0), boosterPowerLeftMax);

			electricCharge = Math.Min(electricCharge + (electricdelay < 1 ? (int)(electricrechargerate * electricRechargeRateMul) : 0), electricChargeMax);

			electricChargeMax = Electicpermboost;
			electricrechargerate = 0;
			electricRechargeRateMul = 1f;
			electricChargeCost = 1f;
			electricChargeReducedDelay = 1f;
			boosterrechargerate = 15;
			boosterPowerLeftMax = 10000;
			Shieldbreak = false;

			if (ShieldTypeDelay > 0)
				ShieldTypeDelay -= 1;
			else
				ShieldType = 0;

			for (int a = 0; a < devempowerment.Length; a++)
				devempowerment[a] = Math.Max(devempowerment[a] - 1, 0);

			for (int i = 0; i < armorglowmasks.Length; i += 1)
			{
				armorglowmasks[i] = null;
				armorglowcolor[i] = delegate (Player player, int index)
				{
					return Color.White;
				};
			}

			digiStacksMax = 0;
			player.breathMax = 200;
			MaxCooldownStacks = 1;
			noactionstackringofrespite = false;
			actionCooldownRate = 1f;
			Noviteset = 0;
			if (player.breath >= player.breathMax || (!SGAConfig.Instance.DrowningChange && !SGAmod.DRMMode))
				drowningIncrementer.X = 0;
			else if (player.breath < 1)
				drowningIncrementer.X += 1;

			drowningIncrementer.Y = Math.Max(0, (drowningIncrementer.X - 300) / 60);
		}

		public override void UpdateBiomes()
		{
			ShadowSectorZone = (byte)Math.Max(ShadowSectorZone - 1, 0);
			foreach (DarkSector sector in DimDingeonsWorld.darkSectors)
			{
				if (sector.PlayerInside(player))
				{
					ShadowSectorZone = 5;
					ShadowSector = sector;
					SGADimPlayer dimplayer = player.GetModPlayer<SGADimPlayer>();
					dimplayer.noLightGrow = 180;
					dimplayer.lightSize += (int)((400f - (float)dimplayer.lightSize) / 600f);
					if (dimplayer.lightSize < 600)
						player.AddBuff(BuffID.Blackout, 120);
					break;
				}
			}

			DankShrineZone = (SGAWorld.MoistStonecount > 15 && Main.tile[(int)(player.Center.X / 16), (int)(player.Center.Y / 16)].wall == mod.WallType("SwampWall"));
		}

		public override void PlayerDisconnect(Player player)
		{
			downedHellion = 0;
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (NoHitCharm)
			{
				return true;
			}

			if (damageSource.SourceCustomReason == (player.name + " went Kamikaze, but failed to blow up enemies"))
				return true;

			if (creeperexplosion > 9795)
				return false;

			if (LifeFlower && !player.HasBuff(BuffID.PotionSickness))
			{
				bool potionsickness = player.HasBuff(BuffID.PotionSickness);
				if (player.QuickHeal_GetItemToUse() == null)
					potionsickness = true;
				else
					player.QuickHeal();
				return potionsickness;
			}

			if (MisterCreeperset && AddCooldownStack(180 * 60))
			{
				creeperexplosion = 10000;
				player.statLife = 1;
				//player.AddBuff(mod.BuffType("ActionCooldown"), 60 * 60);
				return false;
			}

			return true;
		}

		public override void clientClone(ModPlayer clientClone)
		{
			SGAPlayer sgaplayer = clientClone as SGAPlayer;
			sgaplayer.ammoLeftInClip = ammoLeftInClip;
			sgaplayer.ammoLeftInClipMax = ammoLeftInClipMax;
			sgaplayer.ammoLeftInClipMaxLastHeld = ammoLeftInClipMaxLastHeld;
			sgaplayer.ammoLeftInClipMaxAddedAmmo = ammoLeftInClipMaxAddedAmmo;
			sgaplayer.sufficate = sufficate;
			sgaplayer.PrismalShots = PrismalShots;
			sgaplayer.plasmaLeftInClip = plasmaLeftInClip;
			sgaplayer.Redmanastar = Redmanastar;
			sgaplayer.ExpertiseCollected = ExpertiseCollected;
			sgaplayer.ExpertiseCollectedTotal = ExpertiseCollectedTotal;
			sgaplayer.entropyCollected = entropyCollected;
			sgaplayer.DefenseFrame = DefenseFrame;
			sgaplayer.gunslingerLegendtarget = gunslingerLegendtarget;
			sgaplayer.activestacks = activestacks;
			sgaplayer.dragonFriend = dragonFriend;
			sgaplayer.armorToggleMode = armorToggleMode;
			sgaplayer.midasMoneyConsumed = midasMoneyConsumed;

			for (int i = 54; i < 58; i++)
			{

				sgaplayer.ammoinboxes[i - 54] = ammoinboxes[i - 54];
			}
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			bool mismatch = false;
			SGAPlayer sgaplayer = clientPlayer as SGAPlayer;

			for (int i = 54; i < 58; i++)
			{
				if (sgaplayer.ammoinboxes[i - 54] != ammoinboxes[i - 54])
				{
					mismatch = true;
					break;
				}
			}
			if (sgaplayer.ammoLeftInClipMax != ammoLeftInClipMax ||
			sgaplayer.ammoLeftInClipMaxLastHeld != ammoLeftInClipMaxLastHeld ||
			sgaplayer.ammoLeftInClipMaxAddedAmmo != ammoLeftInClipMaxAddedAmmo)
				mismatch = true;
			if (sgaplayer.ammoLeftInClip != ammoLeftInClip || sgaplayer.sufficate != sufficate || sgaplayer.PrismalShots != PrismalShots || sgaplayer.entropyCollected != entropyCollected || sgaplayer.DefenseFrame != DefenseFrame
			|| sgaplayer.plasmaLeftInClip != plasmaLeftInClip || sgaplayer.Redmanastar != Redmanastar || sgaplayer.ExpertiseCollected != ExpertiseCollected || sgaplayer.ExpertiseCollectedTotal != ExpertiseCollectedTotal
			 || sgaplayer.gunslingerLegendtarget != gunslingerLegendtarget || sgaplayer.activestacks != activestacks || sgaplayer.dragonFriend != dragonFriend || sgaplayer.armorToggleMode != armorToggleMode || sgaplayer.midasMoneyConsumed != midasMoneyConsumed)
				mismatch = true;


			if (mismatch)
			{
				SendClientChangesPacket();
			}
		}


		private void SendClientChangesPacket()
		{

			if (Main.netMode == 1)
			{
				ModPacket packet = SGAmod.Instance.GetPacket();
				packet.Write(500);
				packet.Write(player.whoAmI);
				packet.Write((byte)ammoLeftInClip);
				packet.Write((byte)ammoLeftInClipMax);
				packet.Write((byte)ammoLeftInClipMaxLastHeld);
				packet.Write((byte)ammoLeftInClipMaxAddedAmmo);

				packet.Write(sufficate);
				packet.Write(PrismalShots);
				packet.Write(plasmaLeftInClip);
				packet.Write((short)Redmanastar);
				packet.Write(ExpertiseCollected);
				packet.Write(ExpertiseCollectedTotal);
				packet.Write(entropyCollected);
				packet.Write((short)DefenseFrame);
				packet.Write((short)gunslingerLegendtarget);
				packet.Write((short)activestacks);
				packet.Write(dragonFriend);
				packet.Write(midasMoneyConsumed);

				for (int i = 54; i < 58; i++)
				{
					packet.Write(ammoinboxes[i - 54]);
				}
				packet.Send();
			}

		}

		public override bool ModifyNurseHeal(NPC nurse, ref int health, ref bool removeDebuffs, ref string chatText)
		{
			return base.ModifyNurseHeal(nurse, ref health, ref removeDebuffs, ref chatText);
		}


		public override void UpdateBadLifeRegen()
		{

			if (NoFireBurn > 0)
			{
				if (player.HasBuff(BuffID.OnFire))
				{
					player.lifeRegen += 15;
				}
			}

			if (MassiveBleeding)
			{
				if (player.lifeRegen > 0)
					player.lifeRegen = 0;
				player.lifeRegenTime = 0;
				player.lifeRegen -= 10;
			}
			if (thermalblaze)
			{
				int boost = 0;
				if (player.HasBuff(BuffID.Oiled))
					boost = 50;
				player.lifeRegen -= 30 + boost;
			}
			if (acidburn)
			{
				player.lifeRegen -= 15 + (int)(player.statDefense*0.90f);
				player.statDefense -= 5;
			}

			if ((ShieldType < 1 && Shieldbreak) || (Pressured && !SpaceDiverset))
			{
				player.lifeRegen -= 250;
			}

			if (NoHitCharm && realIFrames < 1 && (player.immune || player.immuneTime > 0))
			{
				player.lifeRegenTime = 0;
				player.lifeRegen = -500;

				if (player.statLife < 1)
					player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " couldn't dodge fate"), 10000, player.direction);
				return;
			}
			player.lifeRegen -= badLifeRegen;

			float dot = 0f;

			if (DoTStack.Count > 0)
			{
				int count = 1;
				foreach ((int, float) stack in DoTStack)
				{
					count += 1;
					dot += stack.Item2;
				}
				dot *= 1f + ((count-1) / 3f);
				float scalepercemn = (Math.Min(0.50f+(DoTResist/2f), 1f));
				player.lifeRegen -= (int)(dot/scalepercemn);
				DoTStack = DoTStack.Select(testby => (testby.Item1 - 1, testby.Item2)).Where(testby => testby.Item1 > 0).ToList();
			}
		}

		public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
		{
			Item item = new Item();

			item.SetDefaults(ModLoader.GetMod("SGAmod").ItemType("IDGStartBag"), false);

			items.Add(item);

		}

		public override void NaturalLifeRegen(ref float regen)
		{
			if ((Main.netMode < 1 || SGAmod.SkillRun > 1) && SGAmod.SkillRun > 0)
				skillMananger.NaturalLifeRegen(ref regen);
		}

		public bool IsRestPotion(Item item)
		{
			return (item.type == ItemID.RestorationPotion || item.type == ItemID.LesserRestorationPotion || item.type == ItemID.StrangeBrew);
		}

		public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
		{
			if (restorationFlower && IsRestPotion(item))
				healValue = (int)((float)healValue * 1.50);

			if ((Main.netMode < 1 || SGAmod.SkillRun > 1) && SGAmod.SkillRun > 0)
				skillMananger.GetHealLife(item, quickHeal, ref healValue);
		}

		public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
		{
			if (restorationFlower && IsRestPotion(item))
				healValue = (int)((float)healValue * 1.50);

			if ((Main.netMode < 1 || SGAmod.SkillRun > 1) && SGAmod.SkillRun > 0)
				skillMananger.GetHealMana(item, quickHeal, ref healValue);
		}

		public override void PostUpdateRunSpeeds()
		{
			//SGAUtils.UpdateCoins(Main.LocalPlayer.bank,15);

			if (Noviteset > 0 && electricChargeMax > 0)
			{

				if (Noviteset > 1)
				{
					if (!Walkmode)
					{
						player.maxRunSpeed += (float)electricCharge / 5000f;
						player.runAcceleration += (float)electricCharge / 150000f;
					}
				}
			}

			if (SlowDownDefense > 0f)
			{
				SlowDownDefense /= SlowDownResist;
				player.moveSpeed /= 1f + SlowDownDefense;
				player.accRunSpeed /= 1f + SlowDownDefense;
				player.maxRunSpeed /= 1f + SlowDownDefense;

			}
			SlowDownDefense = 0f;
			SlowDownResist = 1f;

			if (player.HeldItem.type == ModContent.ItemType<Items.Tools.Powerjack>())
			{
				player.moveSpeed *= 1.15f;
				player.accRunSpeed *= 1.15f;
				player.maxRunSpeed *= 1.15f;
			}

			if (player.HeldItem.type == ModContent.ItemType<NoviteKnife>())
			{
				player.moveSpeed *= 1.20f;
				player.accRunSpeed *= 1.20f;
				player.maxRunSpeed *= 1.20f;
				player.jumpSpeedBoost += 1f;
			}

			if ((Main.netMode < 1 || SGAmod.SkillRun > 1) && SGAmod.SkillRun > 0)
				skillMananger.PostUpdateRunSpeeds();

		}

		public override void PreUpdate()
		{
			if (player.immuneTime > 0 && !bustedSpawningGear && Main.myPlayer == player.whoAmI)
			{
				if (Main.ActivePlayerFileData.GetPlayTime().TotalSeconds < 2)
				{
					foreach (Item item in player.inventory)
					{

						//item.Prefix(TrapPrefix.GetBustedPrefix != null ? (byte)TrapPrefix.GetBustedPrefix : (byte)PrefixID.Legendary);

						if (item.prefix == (byte)TrapPrefix.GetBustedPrefix)
						{
							bustedSpawningGear = true;
							break;
						}
					}
					if (bustedSpawningGear == true)
					{
						Item.NewItem(player.Center, ModContent.ItemType<AntiBustedPickaxe>(), prefixGiven: PrefixID.Legendary);
					}
				}
			}


			if (Main.netMode != NetmodeID.Server)
			{
				if (benchGodItem.X > -100)
					SGAmod.craftBlockPanel.ItemPanel.item = new Item();

				if (benchGodItem.X > -1)
				{
					SGAmod.craftBlockPanel.ItemPanel.item.SetDefaults(benchGodItem.X);
					SGAmod.craftBlockPanel.ItemPanel.item.stack = benchGodItem.Y;
				}
				benchGodItem.X = -100;

				Filter manshad = Filters.Scene["SGAmod:ScreenWave"];
				int perception = ModContent.BuffType<CleansedPerception>();
				if (player == Main.LocalPlayer && (player.HeldItem.type == ModContent.ItemType<Debug1>() || player.HasBuff(perception)))
				{
					float alpha = 1;
					if (player.HasBuff(perception))
						alpha = MathHelper.Clamp(player.buffTime[player.FindBuffIndex(perception)] / 1200f, 0f, 1f);

					//if (timer%60==0)
					//Main.NewText("Test!");

					if (!manshad.IsActive())
					{
						Filters.Scene.Activate("SGAmod:ScreenWave", player.Center, new object[0]);
						Overlays.Scene.Activate("SGAmod:SGAHUD");
						//Main.NewText("Turn on! Test!");

					}
					else
					{
						ScreenShaderData shader = manshad.GetShader();
						shader.UseIntensity(26f).UseProgress((Main.GlobalTime * 0.1f) % 1f).UseOpacity(alpha).UseColor(0.02f, 0.02f, 0f).UseTargetPosition(player.Center).UseDirection(new Vector2(1f, Main.GlobalTime * 0.1f))
							.UseImageScale(new Vector2(Main.GlobalTime * -0.4f, Main.GlobalTime * 0.7f), 0);
					}

				}
				else
				{
					if (manshad.IsActive())
					{
						//Main.NewText("Turn off! Test!");
						manshad.Deactivate();
					}
					Overlays.Scene.Deactivate("SGAmod:SGAHUD");
				}
			}

			if (CooldownStacks == null)
				CooldownStacks = new List<ActionCooldownStack>();
			if (skillMananger == null)
				skillMananger = new SkillManager(player);

			downedHellion = SGAWorld.downedHellion;
			for (int i = 54; i < 58; i++)
			{

				ammoinboxes[i - 54] = player.inventory[i].type;
			}

		}

		public override void PostUpdateBuffs()
		{
			if (postLifeRegenBoost > 0)
            {
				player.lifeRegen += postLifeRegenBoost;
				postLifeRegenBoost = 0;
			}

			if (player.HasBuff(ModContent.BuffType<Buffs.StarStormCooldown>()))
			{
				if (!Main.dayTime)
					player.buffTime[player.FindBuffIndex(ModContent.BuffType<Buffs.StarStormCooldown>())] += 1;
			}

			player.statManaMax2 += 20 * Redmanastar;

		}

		public delegate void PostUpdateEquipsDelegate(SGAPlayer player);
		public static event PostUpdateEquipsDelegate PostUpdateEquipsEvent;

		public static event PostUpdateEquipsDelegate PostCharmsUpdateEquipsEvent;

		public static event PostUpdateEquipsDelegate PostPostUpdateEquipsEvent;

		//Really should do more event stuff like this ^

		public static void PostPostUpdateEquips(Player player)//COMPLETELY after everything else (well, after all other modded accessories)
		{
			SGAPlayer sgaply = player.SGAPly();
			PostPostUpdateEquipsEvent?.Invoke(sgaply);

			SGAGlobalItem.VanillaArmorSetBonus(player);

			if (sgaply.ELS)
			{
				if (player.lifeRegen < 0)
					sgaply.DoTResist += 0.50f;
					//player.lifeRegen = (int)(player.lifeRegen * 1.5f);
			}

			//Main.NewText("TextTewst");
			if (sgaply.finalGem > 0)
			{
				for (int index = 0; index < 7; index += 1)
				{
					player.ownedLargeGems[index] = true;
				}
			}

			if (sgaply.NoFly)
			{
				player.wingTimeMax = player.wingTimeMax / 10;
			}

			if (sgaply.HeavyCrates)
			{
				player.runAcceleration /= 3f;
			}
			if (player.HasBuff(ModContent.BuffType<TechnoCurse>()))
			{
				sgaply.techdamage = Math.Max(sgaply.techdamage - 0.50f, 0);
			}

			DoPotionFatigue(sgaply);
		}

		public override void PostUpdateEquips()
		{
			//Minecarts-

			//player.powerrun = true;

			if (!player.HeldItem.IsAir)
			{
				TrapDamageItems stuff = player.HeldItem.GetGlobalItem<TrapDamageItems>();
				if (stuff.misc == 3)
				{
					shieldDamageReduce += 0.05f;
				}
			}

			PostUpdateEquipsEvent?.Invoke(this);

			if (player.SGAPly().manifestedWeaponType > 0)
			{
				if (player.inventory[player.selectedItem].IsAir && player.selectedItem < 58)
				{
					Item newItem = new Item();
					newItem.SetDefaults(player.SGAPly().manifestedWeaponType);
					player.inventory[player.selectedItem] = newItem;
					//Main.NewText(player.inventory[player.selectedItem].type);
				}
			}

			if (ninjaSash > 0)
				ninjaStashLimit = Math.Max(ninjaStashLimit - 1, 0);

			if (player.HasBuff(BuffID.Lovestruck))
			{
				if (intimacy > 0)
					player.aggro += 250 * intimacy;
				int maxdamage = 0;
				foreach (Player player2 in Main.player.Where(player2 => player2.active && player.whoAmI != player2.whoAmI && player.SGAPly().intimacy > 0 && player.Distance(player.MountedCenter) < 600))
				{
					maxdamage = Math.Max(maxdamage, (int)(player2.lifeRegen / 5));
				}
				player.lifeRegen += maxdamage;
			}

			if (toxicity > 0 && player.HasBuff(BuffID.Stinky))
				player.aggro -= 400 * toxicity;

			if (gunslingerLegendtarget > -1)
			{
				NPC them = Main.npc[gunslingerLegendtarget];
				if (!them.active || them.life < 1 || them.type != gunslingerLegendtargettype)
					gunslingerLegendtarget = -1;
			}

			Item minecart = player.miscEquips[2];
			if (!minecart.IsAir)
			{
				if (minecart.modItem != null)
				{
					var myType = (minecart.modItem).GetType();
					var n = myType.Namespace;
					string asastring = (string)n;
					int ischarm = asastring.Length - asastring.Replace(".Charms", "").Length;
					if (ischarm > 0)
					{
						minecart.modItem.UpdateAccessory(player, true);
					}
				}
			}

			PostCharmsUpdateEquipsEvent?.Invoke(this);

			if (HasGucciGauntlet())
			{
				if (player.ownedLargeGems[1])
					UseTimeMulPickaxe += 0.25f;
				if (player.ownedLargeGems[2])
				{
					player.accFlipper = true;
					player.ignoreWater = true;
				}
				if (player.ownedLargeGems[3])
				{
					player.AddBuff(BuffID.DryadsWard, 2);
					player.maxMinions += 1;
				}
				if (player.ownedLargeGems[5])
				{
					player.BoostAllDamage(0.05f, 5);
					player.lifeRegen += 2;
					player.maxRunSpeed += 0.5f;
				}
			}

			if (player.HeldItem.type == ModContent.ItemType<CrateBossWeaponMeleeOld>())
				player.goldRing = true;

			string[] buffTier = { "", "RadiationOne", "RadiationTwo", "RadiationThree" };

			if (player.HeldItem.modItem != null && player.HeldItem.modItem is IRadioactiveItem)
			{
				int buffID = (player.HeldItem.modItem as IRadioactiveItem).RadioactiveHeld() - (grippinggloves > 1 ? 1 : 0);
				int indexer = (int)MathHelper.Clamp(buffID, 0, buffTier.Length - 1);

				if (indexer > 0)
					player.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff(buffTier[indexer]).Type, 2);
			}

			ShuffleYourFeetElectricCharge();

			DashBlink();

			if (!granteditems)
			{
				if (nightmareplayer && !player.HasItem(mod.ItemType("Nightmare")))
				{
					player.QuickSpawnItem(mod.ItemType("Nightmare"));
				}
				granteditems = true;
			}

			if (IDGset)
			{
				if (player.HasMinionAttackTargetNPC)
				{
					IdgNPC.AddBuffBypass(player.MinionAttackTargetNPC, mod.BuffType("DigiCurse"), 3);
				}
			}


			if (Dankset > 0)
			{
				bool underground = (int)((double)((player.position.Y + (float)player.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;
				if (!underground && Main.dayTime)
					player.lifeRegen += 3;
			}
			if (Main.netMode != 1)
			{
				//Overlays.Scene.Activate("SGAmod:SGAHUD");
			}

			CharmingAmuletCode();

			if (player.manaRegenBuff && (SGAConfig.Instance.ManaPotionChange || SGAmod.DRMMode))
				player.statManaMax2 = Math.Max(player.statManaMax2 - 50, 0);

			if (creeperexplosion > 9700)
			{
				creeperexplosion -= 1;

				if (creeperexplosion == 9998)
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Creeper_fuse").WithVolume(.7f).WithPitchVariance(.25f), player.Center);

				int dustIndexsmoke = Dust.NewDust(new Vector2(player.Center.X - 4, player.position.Y - 6), 8, 12, 31, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dustIndexsmoke].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
				Main.dust[dustIndexsmoke].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
				Main.dust[dustIndexsmoke].noGravity = true;
				dustIndexsmoke = Dust.NewDust(new Vector2(player.Center.X, player.position.Y - 6), 8, 12, 6, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dustIndexsmoke].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
				Main.dust[dustIndexsmoke].noGravity = true;


				if (creeperexplosion == 9800)
				{
					for (int xx = 64; xx < 200; xx += 48)
					{

						for (int i = 0; i < 359; i += 36)
						{
							double angles = MathHelper.ToRadians(i + (xx * 4));
							float randomx = xx;//Main.rand.NextFloat(54f, 96f);
							Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles));

							int thisone = Projectile.NewProjectile(player.Center.X + (here.X * randomx) - 100, player.Center.Y + (here.Y * randomx) - 100, here.X, here.Y, mod.ProjectileType("CreepersThrowBoom2"), player.statDefense * 8, 0f, player.whoAmI, 0.0f, 0f);
						}

					}

				}


			}

			if (creeperexplosion < 9798 && creeperexplosion > 2000)
			{
				creeperexplosion = 0;
				Noselfdamage = false;
				PlayerDeathReason reason = PlayerDeathReason.ByCustomReason(player.name + " went Kamikaze, but failed to blow up enemies");
				reason.SourcePlayerIndex = -111;
				reason.SourceNPCIndex = 0;
				player.KillMe(reason, 1337000, player.direction);
			}


			if (floatyeffect > -1)
				floatyeffect -= 1;

			FieryheartBuff = FieryheartBuff - 1;
			beefield = beefield - 1;
			beefieldtoggle = beefieldtoggle - 1;
			Novusset -= 1;
			Dankset -= 1;

			breathingdelay += 1; breathingdelay %= 30;
			if (sufficate < 0)
			{
				if (breathingdelay % 5 == 0)
					sufficate = (int)MathHelper.Clamp(sufficate + 1, -200, player.breathMax - 1);
			}
			else
			{
				if (breathingdelay % 29 == 0)
					sufficate = (int)MathHelper.Clamp(sufficate + 1, -200, player.breathMax - 1);
			}
			if (beserk[0] > 0 || permaDrown)
				sufficate = player.breath;

			if (FireBreath > 0)
			{
				if (timer % 6 == 0)
				{
					float scalevalue = 1f + (float)FireBreath / 2f;
					int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y - 16, (Main.rand.NextFloat(5f, 6f) + (scalevalue - 1f) * 2f) * (float)player.direction, Main.rand.NextFloat(-0.5f, 0.5f), ProjectileID.Flames, (int)((float)player.statDefense * scalevalue), 0f, player.whoAmI, 8f, 0f);
					Main.projectile[thisone].usesLocalNPCImmunity = true;
					Main.projectile[thisone].localNPCHitCooldown = 20;
					Main.projectile[thisone].ranged = false;
					Main.projectile[thisone].penetrate += ((int)scalevalue - 1);

				}
			}


			//if (this.grappling[0] == -1 && this.carpet && !this.jumpAgainCloud && !this.jumpAgainSandstorm && !this.jumpAgainBlizzard && !this.jumpAgainFart && !this.jumpAgainSail && !this.jumpAgainUnicorn && this.jump == 0 && this.velocity.Y != 0f && this.rocketTime == 0 && this.wingTime == 0f && !this.mount.Active)
			//{
			if (SpaceDiverWings > 0 && boosterPowerLeft > 50)
			{
				float spacediverwingstemp = Math.Max(SpaceDiverWings, 1f);
				if (player.controlJump && player.velocity.Y != 0f)
				{
					bool pressdownonly = (!player.controlLeft && !player.controlRight);



					if (SpaceDiverset)
					{
						int dust = Dust.NewDust(new Vector2(player.Center.X - 12, player.Center.Y + 18), 24, 8, 27);
						Main.dust[dust].scale = 1.5f;
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						Main.dust[dust].velocity = (randomcircle / 2f) + new Vector2(0, player.wingTime > 0 ? 12 : 3);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);

					}

					if (player.controlDown)
					{
						boosterPowerLeft -= 50;
						boosterdelay = 90f;
						if (player.controlLeft && player.wingTime > 0)
							player.velocity.X -= ((float)spacediverwingstemp / 2f);
						if (player.controlRight && player.wingTime > 0)
							player.velocity.X += ((float)spacediverwingstemp / 2f);
						if (pressdownonly || player.wingTime < 1)
						{
							player.velocity.Y += 0.025f;
							int minTilePosX = (int)(player.Center.X / 16.0) - 1;
							int minTilePosY = (int)((player.Center.Y + 32f) / 16.0) - 1;
							int whereisity;
							//whereisity = Idglib.RaycastDown(minTilePosX + 1, (int)MathHelper.Clamp(minTilePosY, 0,Main.maxTilesY));
							//if ((whereisity - minTilePosY > 4 + (player.velocity.Y * 1)) || player.velocity.Y < 0)
							if (Collision.CanHitLine(player.Center, 32, 32, player.Center + new Vector2(0, (Vector2.Normalize(player.velocity).Y * 64f) + (player.velocity.Y * 2f)), 32, 32))
								player.position.Y += 8 + (player.velocity.Y * 2);
						}
						else
						{
							if (player.wingTime > 0)
								player.velocity.Y /= 2f;
						}

						player.velocity.X /= 1.02f;

						int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 27);
						Main.dust[dust].scale = 1.25f;
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						Main.dust[dust].velocity = (randomcircle / 3f) - player.velocity;
						Main.dust[dust].velocity.Normalize();
						Main.dust[dust].velocity *= 2f;
						Main.dust[dust].noGravity = true;
						Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);

						if (breathingdelay % 3 == 0)
						{
							float pitcher = -0.99f + ((float)player.wingTime / (float)player.wingTimeMax);
							pitcher = MathHelper.Clamp(pitcher, -0.9f, 1f);
							Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 75, 0.25f, pitcher);
						}

					}

				}
			}


			if (SpaceDiverset)
			{
				player.AddBuff(mod.BuffType("Pressured"), 180);

				player.gills = false;

				bool isbreathing = beserk[1] <= 0 && !permaDrown;

				//if (SGAmod.Calamity && modcheckdelay){ isbreathing=CalamityAbyss;
				//}

				if (isbreathing)
					player.breath = (int)MathHelper.Clamp(sufficate, -5, player.breathMax - 1);
				if (sufficate < 1)
				{
					player.suffocating = true;
				}
				else
				{
					player.endurance += ((float)player.breath / (float)player.breathMax) * 0.4f;
					player.statDefense += (int)(((float)player.breath / (float)player.breathMax) * 100f);
				}

			}

			if (Havoc > 0)
			{

				for (int x = 3; x < 8 + player.extraAccessorySlots; x++)
				{
					if (player.armor[x].modItem != null)
					{
						var myType = (player.armor[x].modItem).GetType();
						var n = myType.Namespace;
						string asastring = (string)n;
						//int ishavocitem = (asastring.Split('.').Length - 1);
						int ishavocitem = asastring.Length - asastring.Replace("HavocGear.", "").Length;
						if (ishavocitem > 0)
						{
							player.statDefense += (Main.hardMode ? 8 : 3);

						}
					}
				}

			}


			if (NPC.CountNPCS(mod.NPCType("Cirno")) > 0 || (SGAWorld.downedCirno == false && Main.hardMode && (SGAConfig.Instance.NegativeWorldEffects || SGAmod.DRMMode)))
				player.AddBuff(mod.BuffType("NoFly"), 1, true);

			/*if (pmlcrato>0 || NPC.CountNPCS(mod.NPCType("SPinky"))>9990){player.AddBuff(mod.BuffType("Locked"), 2, true);}*/

			int pmlcrato = NPC.CountNPCS(mod.NPCType("Cratrogeddon"));
			int npctype = NPC.CountNPCS(mod.NPCType("Cratrosity")) + pmlcrato;

			if (npctype > 0)
			{
				int counter = (player.CountItem(ItemID.WoodenCrate));
				counter += (player.CountItem(ItemID.IronCrate));
				counter += (player.CountItem(ItemID.GoldenCrate));
				counter += (player.CountItem(ItemID.DungeonFishingCrate));
				counter += (player.CountItem(ItemID.JungleFishingCrate));
				counter += (player.CountItem(ItemID.CorruptFishingCrate));
				counter += (player.CountItem(ItemID.HallowedFishingCrate));
				counter += (player.CountItem(ItemID.FloatingIslandFishingCrate));
				counter += (player.CountItem(ModContent.ItemType<HavocGear.Items.DankCrate>()));
				if (counter > 0)
				{
					player.AddBuff(mod.BuffType("HeavyCrates"), 2, true);
				}
			}

			if (SGAConfig.Instance.HeavyInventory || SGAmod.DRMMode)
            {
				bool allfilled = true;
				for(int i = 0; i < 50; i += 1)
                {
					if (player.inventory[i].IsAir)
                    {
						allfilled = false;
                    }
                }

				if (allfilled)
				player.AddBuff(ModContent.BuffType<HeavyInventory>(), 2, true);
			}

			if (CirnoWings == true)
			{
				player.buffImmune[BuffID.Chilled] = true;
				player.buffImmune[BuffID.Frozen] = true;
				player.buffImmune[BuffID.Frostburn] = true;
				player.buffImmune[ModContent.BuffType<NoFly>()] = true;
			}


			int losingmoney = MoneyMismanagement == true ? 2 : (Microtransactions == true ? 1 : 0);
			if (losingmoney > 0)
			{
				Microtransactionsdelay += 1;
				if (Microtransactionsdelay % 30 == 0)
				{
					int taketype = 3;
					int[] types = { ItemID.CopperCoin, ItemID.SilverCoin, ItemID.GoldCoin, ItemID.PlatinumCoin };
					int copper = player.CountItem(ItemID.CopperCoin);
					int silver = player.CountItem(ItemID.SilverCoin);
					int gold = player.CountItem(ItemID.GoldCoin);
					int plat = player.CountItem(ItemID.PlatinumCoin);
					taketype = plat > 0 ? 3 : (gold > 0 ? 2 : (silver > 0 ? 1 : 0));
					player.ConsumeItem(types[taketype]);
					if (losingmoney > 1)
					{
						//player.Hurt(PlayerDeathReason damageSource, int Damage, int hitDirection, bool pvp = false, bool quiet = false, bool Crit = false, int cooldownCounter = -1)
						player.statLife -= taketype * 5;

						if (player.statLife < 1) { player.KillMe(PlayerDeathReason.ByCustomReason(player.name + (Main.rand.Next(0, 100) > 66 ? " Disgraced Gaben..." : (Main.rand.Next(0, 100) > 50 ? " couldn't stop spending money" : " couldn't resist the sale"))), 111111, 0, false); }

					}
				}
			}


			if (beefield > 3)
			{
				beefieldcounter = beefieldcounter + 1;
				if (player.ownedProjectileCounts[181] < 5 && beefieldcounter > 60)
				{
					beefieldcounter = 0;
					bool beeflag = false;
					int x = 3;
					for (x = 3; x < 8 + player.extraAccessorySlots; x++)
					{
						if (player.armor[x].type == mod.ItemType("PortableHive") || player.armor[x].type == mod.ItemType("DevPower"))
						{
							beeflag = true;
							//if (1f + (player.armor[x].damage * 0.05f)>beedamagemul)
							//beedamagemul = 1f+(player.armor[x].damage*0.05f);
							break;
						}
					}
					if (beeflag == true)
					{
						int prog = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ProjectileID.Bee, (int)(player.GetWeaponDamage(player.armor[x])), (float)player.GetWeaponKnockback(player.armor[x], player.armor[x].knockBack) * 0.01f, player.whoAmI);
						SGAprojectile modeproj = Main.projectile[prog].GetGlobalProjectile<SGAprojectile>();
						Main.projectile[prog].penetrate = -1;
						modeproj.enhancedbees = true;
					}
				}

			}
			if (twinesoffate)
			{
				if (player.ownedProjectileCounts[mod.ProjectileType("TwineOfFate")] < 1)
				{
					int prog = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("TwineOfFate"), 0, 0, player.whoAmI);
				}
				if (player.ownedProjectileCounts[mod.ProjectileType("TwineOfFateClothier")] < 1)
				{
					int prog = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("TwineOfFateClothier"), 0, 0, player.whoAmI);
				}
			}


			if (lunarSlimeHeart)
			{
				int Buffscounter = 0;
				for (int z = 0; z < Player.MaxBuffs; z++)
				{

					if (player.buffType[z] > 0)
						Buffscounter += Main.debuff[player.buffType[z]] ? 4 : 1;

				}
				player.statDefense += Buffscounter * 1;

				lunarSlimeHeartdamage = (int)((float)(player.statDefense * (player.minionDamage + player.rangedDamage + player.meleeDamage + player.Throwing().thrownDamage + player.magicDamage)));

				lunarSlimeCounter = lunarSlimeCounter + 1;
				if (player.ownedProjectileCounts[mod.ProjectileType("LunarSlimeProjectile")] < 8)
				{
					bool beeflag = false;
					int x = 3;
					for (x = 3; x < 8 + player.extraAccessorySlots; x++)
					{
						if (player.armor[x].type == mod.ItemType("LunarSlimeHeart"))
						{
							beeflag = true;
							break;
						}
					}
					if (beeflag == true)
					{
						for (int i = 0; i < 7; i++)
						{
							if (projectileslunarslime[i] == null || !projectileslunarslime[i].active)
							{
								int prog = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("LunarSlimeProjectile"), (int)player.GetWeaponDamage(player.armor[x]), (float)player.GetWeaponKnockback(player.armor[x], player.armor[x].knockBack) * 0.01f, player.whoAmI, (float)i);
								SGAprojectile modeproj = Main.projectile[prog].GetGlobalProjectile<SGAprojectile>();
								//Main.projectile[prog].netUpdate = true;
								projectileslunarslime[i] = Main.projectile[prog];
							}
						}
					}
				}

			}

			if (player.ownedProjectileCounts[mod.ProjectileType("TimeEffect")] < 1 && MatrixBuffp)
			{
				int prog = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("TimeEffect"), 1, 0, player.whoAmI);
			}

			if (player.ownedProjectileCounts[ModContent.ProjectileType<DrakenSummonProj>()] < 1 && player.HeldItem.type == ModContent.ItemType<DragonCommanderStaff>())
			{
				int prog = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<DrakenSummonProj>(), 1, 0, player.whoAmI);
			}

			if (player.ownedProjectileCounts[ModContent.ProjectileType<FistOfMoonlordProjectile>()] < 1 && player.HeldItem.type == ModContent.ItemType<FistOfMoonlord>())
			{
				int prog = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ModContent.ProjectileType<FistOfMoonlordProjectile>(), 1, 0, player.whoAmI);
			}

			if (player.statLife > player.statLifeMax2 - 1)
			{

				if (OmegaSigil)
				{
					for (int i = 0; i < player.GetModPlayer<SGAPlayer>().apocalypticalChance.Length; i += 1)
						player.GetModPlayer<SGAPlayer>().apocalypticalChance[i] += 3.0;
				}

				if (SybariteGem)
				{
					player.AddBuff(BuffID.Midas, 60 * 5);
				}

			}

			if (player.HeldItem != null)
			{
				if (SGAmod.NonStationDefenses.ContainsKey(player.HeldItem.type))
				{
					int projtype;
					SGAmod.NonStationDefenses.TryGetValue(player.HeldItem.type, out projtype);
					if (player.ownedProjectileCounts[projtype] < 1)
						Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, projtype, player.HeldItem.damage, player.HeldItem.knockBack, player.whoAmI);
				}

				int index = player.SGAPly().heldShield;
				if (index >= 0)
				{
					if (Main.projectile[index].active)
					{
						Items.Weapons.Shields.CorrodedShieldProj myShield = (Main.projectile[player.SGAPly().heldShield].modProjectile as Items.Weapons.Shields.CorrodedShieldProj);
						if (myShield != null)
							myShield.WhileHeld(player);
					}
				}
				else
				{

					if (player.ownedProjectileCounts[mod.ProjectileType("CapShieldToss")] < 1 && player.HeldItem.modItem != null)
					{
						int projtype = -1;
						if (SGAPlayer.ShieldTypes.ContainsKey(player.HeldItem.type))
						{
							SGAPlayer.ShieldTypes.TryGetValue(player.HeldItem.type, out projtype);
							if (projtype > 0)
							{
								if (player.ownedProjectileCounts[projtype] < 1)
								{
									Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, projtype, player.HeldItem.damage, player.HeldItem.knockBack, player.whoAmI);
									if (proj != null && proj.modProjectile != null && player.HeldItem != null && player.HeldItem.modItem is LaserMarker heldmarker)
									{
										LaserMarkerProj marker = ((LaserMarkerProj)proj.modProjectile);
										SGAmod.GemColors.TryGetValue(heldmarker.gemType, out Color color);
										((LaserMarkerProj)proj.modProjectile).gemColor = color;
									}
								}
							}
						}
					}
				}
			}

			DoCooldownUpdate();


			if ((Main.netMode < 1 || SGAmod.SkillRun > 1) && SGAmod.SkillRun > 0)
				skillMananger.PostUpdateEquips();

			mspeed = player.meleeSpeed;
			player.manaRegenBonus += manaBoost;
			manaBoost = 0;
			modcheckdelay = true;
		}

		public delegate void PreUpdateMovementDelegate(SGAPlayer player);
		public static event PreUpdateMovementDelegate PreUpdateMovementEvent;

		public override void PreUpdateMovement()
		{
			PreUpdateMovementEvent?.Invoke(this);
			if (GeyserInABottleActive && GeyserInABottle)
			{
				if (player.controlJump && !player.jumpAgainCloud)
				{
					List<Projectile> itz = Idglib.Shattershots(player.Center + new Vector2(Main.rand.Next(-15, 15), player.height), player.Center + new Vector2(0, player.height + 32), new Vector2(0, 0), ProjectileID.GeyserTrap, 30, 5f, 30, 1, true, 0, false, 400);
					//itz[0].damage = 30;
					itz[0].owner = player.whoAmI;
					itz[0].friendly = true;
					itz[0].hostile = true;
					Main.projectile[itz[0].whoAmI].netUpdate = true;
					if (Main.netMode == 2 && itz[0].whoAmI < 200)
					{
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, itz[0].whoAmI, 0f, 0f, 0f, 0, 0, 0);
					}

					itz = Idglib.Shattershots(player.Center + new Vector2(Main.rand.Next(-15, 15), player.height), player.Center + new Vector2(0, player.height - 180), new Vector2(0, 0), ProjectileID.GeyserTrap, 30, 10f, 30, 1, true, 0, false, 400);
					//itz[0].damage = 30;
					itz[0].owner = player.whoAmI;
					itz[0].friendly = true;
					itz[0].hostile = true;
					Main.projectile[itz[0].whoAmI].netUpdate = true;
					if (Main.netMode == 2 && itz[0].whoAmI < 200)
					{
						NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, itz[0].whoAmI, 0f, 0f, 0f, 0, 0, 0);
					}

					GeyserInABottle = false;
					player.velocity.Y = -15;
				}


			}

		}

		public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
		{
			if (PrimordialSkull)
				damage = damage / 2;
			damage = OnHit(ref damage, ref crit, npc, null);
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref bool crit)
		{
			damage = OnHit(ref damage, ref crit, null, projectile);
		}

		public override void UpdateDead()
		{

				if (DoTStack.Count > 0)
					DoTStack.Clear();

			NoHitCharmTimer = 0;
			if (NoHitCharm && !IdgNPC.bossAlive)
			{
				NoHitCharmTimer = 0;
				player.respawnTimer = Math.Min(90, player.respawnTimer);
			}
		}
		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			if (NoHitCharm)
			{
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.Red, "Finished...", true, false);
				if (SGAConfigClient.Instance.EpicApocalypticals)
				{
					RippleBoom.MakeShockwave(player.Center, 8f, 1f, 10f, 60, 1f);
					Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, player.Center);
				}
			}
		}

		public delegate void PreHurtDelegate(SGAPlayer player, bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource);
		public static event PreHurtDelegate PreHurtEvent;

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (invincible)
				return false;

			if (damageSource.SourceCustomReason == (player.name + " went Kamikaze, but failed to blow up enemies"))
				return true;

			if (realIFrames > 0)
				return false;

			if (highStakesSet)
			{
				if (damageSource.SourcePlayerIndex == player.whoAmI)
				{
					damage /= 4;
				}
			}

			if (PreHurtEvent != null)
			PreHurtEvent.Invoke(this, pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
			SGAnpcs.PlayersGotHit();

			if (damageSource.SourceNPCIndex > -1)
			{
				NPC npc = Main.npc[damageSource.SourceNPCIndex];
				if (ShieldDamageCheck(npc.Center, ref damage, npc.whoAmI + 1))
					return false;

				if (!NoHitCharm && npc.GetGlobalNPC<SGAnpcs>().NinjaSmoked && Main.rand.Next(0, 100) < 75)
				{
					player.NinjaDodge();
					return false;
				}
			}

			if (damageSource.SourceProjectileIndex > -1)
			{
				if (ShieldDamageCheck(Main.projectile[damageSource.SourceProjectileIndex].Center, ref damage, -(damageSource.SourceProjectileIndex - 1)))
					return false;

				if (!NoHitCharm && Main.projectile[damageSource.SourceProjectileIndex].trap && aversionCharm && AddCooldownStack(60 * 30))
				{
					player.NinjaDodge();
					return false;
				}
			}

			if (highStakesSet)
			{
				damage = (int)(damage * Main.rand.NextFloat(0.50f, 2f));
			}

			if (NoHitCharm)
			{
				damage = player.statLifeMax2 * 3;
				return true;
			}

			if (phaethonEye > 6 && player.statLife - damage < 1 && AddCooldownStack(180 + (damage * 5)))
			{
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Items.Accessories.PhaethonEyeProcEffect>(), 0, 0, player.whoAmI);
				player.NinjaDodge();
				return false;
			}

			if (OmegaSigil && player.statLife - damage < 1 && Main.rand.Next(100) <= 10)
			{
				damage = 0;
				player.NinjaDodge();
				return false;
			}
			damage = (int)(damage * damagetaken);

			if (player.HasBuff(ModContent.BuffType<DrakenDefenseBuff>()))
			{
				damage = (int)Math.Pow(damage, 0.90);

			}

			if (anticipation > 0)
			{
				anticipation = (int)(anticipation / 2);
			}

			if (MurkyDepths)
			{
				damage = (int)(damage * 1.5);
			}

			if (Noselfdamage)
			{
				if (creeperexplosion < 9800)
					if (damageSource.SourcePlayerIndex == player.whoAmI && AddCooldownStack(60 * 60))
						return false;
			}

			if (damageSource.SourceProjectileType == ProjectileID.GeyserTrap && (GeyserInABottleActive))
				return false;

			if (creeperexplosion > 9795)
				return false;

			if (SpaceDiverset)
			{
				if (player.breath > player.breathMax - 2)
				{
					player.immune = true;
					player.immuneTime = 45;
					damage *= 3;

					int lifelost = (int)(((float)damage / (float)player.statLifeMax) * 100f);
					sufficate -= (lifelost + 5);
					if (sufficate < 0)
						sufficate = (int)MathHelper.Clamp(-(lifelost), sufficate, 0);

					return false;
				}
			}

			if (beefield > 0)
			{
				if (Main.rand.Next(0, 10) < 5)
				{
					player.honeyWet = true;
					player.wet = true;
					foreach (Player player2 in Main.player.Where(playertest => playertest.active && !playertest.dead && player.IsAlliedPlayer(playertest) && playertest.Distance(player.Center) < 160))
						player2.AddBuff(BuffID.Honey, 60 * 5);
				}
			}

			if (BIP)
				player.AddBuff(mod.BuffType("BIPBuff"), 60 * 10);

			if (TakeShieldHit(ref damage))
				return false;

			player.GetModPlayer<Items.Weapons.Almighty.CataNukePlayer>().Charge /= 2;

			return true;
		}

		private int OnHit(ref int damage, ref bool crit, NPC npc, Projectile projectile)
		{
			//StackDebuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationThree").Type, 60 * 15);

			if (SGAmod.OverpoweredMod > 0)
			{
				crit = true;
				damage = damage + (int)(damage * SGAmod.OverpoweredMod);

			}
			if (Hellion.GetHellion() != null)
			{
				Hellion hell = Hellion.GetHellion();
				if (hell.army.Count > 0)
				{
					player.AddBuff(Idglib.Instance.BuffType("LimboFading"), 60 * 4);

					if (hell.phase < 3)
						player.AddBuff(BuffID.BrokenArmor, 60 * 3);
					if (hell.phase < 5)
					{
						player.AddBuff(BuffID.WitheredArmor, 60 * 5);
						player.AddBuff(BuffID.WitheredWeapon, 60 * 3);
					}

				}

				if (npc != null)
				{
					if (npc.type == NPCID.SkeletronHand)
						player.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("HotBarCurse").Type, 60 * 20);
					if (npc.type == NPCID.SkeletronHand)
						player.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("ItemCurse").Type, 60 * 25);
				}
			}
			if (NPC.CountNPCS(mod.NPCType("Murk")) > 0 && Main.hardMode && Main.expertMode)
			{
				//player.AddBuff(mod.BuffType("MurkyDepths"), damage * 5);
			}
			if (NPC.CountNPCS(mod.NPCType("TPD")) > 0 && Main.rand.Next(0, 10) < (Main.expertMode ? 6 : 3))
			{
				player.AddBuff(BuffID.Electrified, 20 + (damage * 2));
			}
			if (NPC.CountNPCS(ModContent.NPCType<SPinkyTrue>()) > 0 && npc != null && npc.type <= NPCID.BlueSlime && Main.expertMode)
			{
				player.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationTwo").Type, 60 * 10);
			}
			if (Noviteset > 2)
				ChainBolt();

			if (CirnoWings)
			{
				if (npc != null)
					if (npc.coldDamage)
						damage = (int)(damage * 0.80);
				if (projectile != null)
					if (projectile.coldDamage)
						damage = (int)(damage * 0.80);
			}

			if (IceFire)
			{
				if (npc != null)
					if (npc.coldDamage)
						damage = (int)(damage * 0.75);
				if (projectile != null)
					if (projectile.coldDamage)
						damage = (int)(damage * 0.75);
			}

			if (projectile != null)
			{
				if (Duster)
					if (projectile.type == ProjectileID.Bullet || projectile.type == ProjectileID.BulletDeadeye || projectile.type == ProjectileID.BulletSnowman || projectile.type == ProjectileID.SniperBullet)
						damage = (int)((float)damage * 0.50f);

				//damagecheck(projectile.Center - projectile.velocity, ref damage);
			}
			/*if (npc != null)
			{
				damagecheck(npc.Center, ref damage);
			}*/

			if (SpaceDiverset)
			{
				int lifelost = (int)(((float)damage / (float)player.statLifeMax) * 150f);
				sufficate -= lifelost;
				if (sufficate < 0)
					sufficate = (int)MathHelper.Clamp(-(lifelost + 5), sufficate, 0);
			}

			if (player.HeldItem.type == ModContent.ItemType<Items.Tools.Powerjack>())
			{
				damage = (int)(damage * 1.20);
			}

			if (npc != null)
			{
				if (npc.type == NPCID.BlazingWheel && npc.life == 88)
				{
					SGAnpcs nyx = npc.GetGlobalNPC<SGAnpcs>();
					damage = (int)(damage * 2.0);
				}
			}

			if ((Main.netMode < 1 || SGAmod.SkillRun > 1) && SGAmod.SkillRun > 0)
				skillMananger.OnPlayerDamage(ref damage, ref crit, npc, projectile);


			return damage;
		}

		public override void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			AfterTheHit(npc, null, damage, crit);
		}

		public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
		{
			AfterTheHit(null, proj, damage, crit);
		}

		public static void SwearExplosion(Vector2 location, Player player, int damage)
		{
			SGAPlayer sga = player.SGAPly();
			foreach (NPC npc2 in Main.npc.Where(npc2 => npc2.active && npc2.Distance(location) < 400 * sga.toxicity))
			{
				float power = (sga.toxicity * 3f) + player.thorns + (npc2.HasBuff(BuffID.Stinky) ? 2 : 0);
				npc2.StrikeNPC((int)(damage * power), power, npc2.direction);
				if (Main.netMode != 0)
				{
					NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc2.whoAmI, (int)(damage * power), 16f, (float)1, 0, 0, 0);
				}
			}
			string[] aBunchOfSwearWords = { "Shit", "Fuck", "Bitch", "Cunt", "Asshole" };//no, I'm not using the N-word here, I have stadards unlike the low IQ people this item mimics
			CombatText.NewText(new Rectangle((int)location.X, (int)location.Y, 0, 0), Color.Red, aBunchOfSwearWords[Main.rand.Next(aBunchOfSwearWords.Length)], true, true);
			Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_BetsyScream, location);
			if (sound != null)
				sound.Pitch -= 0.75f;

		}

		public delegate void AfterTheHitDelegate(SGAPlayer player, NPC npc, Projectile projectile, int damage, bool crit);
		public static event AfterTheHitDelegate AfterTheHitEvent;

		public void AfterTheHit(NPC npc, Projectile projectile, int damage, bool crit)
		{

			if (AfterTheHitEvent != null)
			AfterTheHitEvent.Invoke(this, npc == null ? default : npc, projectile == null ? default : projectile, damage, crit);

			if (MisterCreeperset)
			{
				Vector2 myspeed = new Vector2(0, 0);
				if (npc != null)
				{
					myspeed = npc.Center - player.Center;
					myspeed.Normalize();
				}
				if (projectile != null)
				{
					myspeed = projectile.Center - player.Center;
					myspeed.Normalize();
				}
				myspeed *= 20f;
				int prog = Projectile.NewProjectile(player.Center.X, player.Center.Y, myspeed.X, myspeed.Y, ProjectileID.Grenade, 1000, 10f, player.whoAmI);
				Main.projectile[prog].Throwing().thrown = true; Main.projectile[prog].ranged = false; Main.projectile[prog].netUpdate = true;
				IdgProjectile.Sync(prog);

			}

			if (toxicity > 0)
			{
				if (player.HasBuff(BuffID.Stinky))
				{
					SwearExplosion(player.MountedCenter, player, damage);
				}

			}

		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{

			//Main.NewText(player.headFrame.Width);
			if (Main.netMode == 0)
			{
				/*if (SGAmod.SkillTestKey.JustPressed)
				{
					SGAmod.SkillUIActive = !SGAmod.SkillUIActive;
					return;
				}*/
			}
			if (SGAmod.ToggleRecipeHotKey.JustPressed)
			{
				if (acidSet.Item1)
				{
					Items.Armors.Acid.AcidHelmet.ActivateHungerOfFames(this);
				}
				if (valkyrieSet.Item1)
				{
					Items.Armors.Valkyrie.ValkyrieHelm.ActivateRagnorok(this);
				}
				if (magatsuSet)
				{
					Items.Armors.Magatsu.MagatsuHood.ActivateDecoy(this);
				}
				if (jungleTemplarSet.Item1)
				{
					Items.Armors.JungleTemplar.JungleTemplarHelmet.ActivatePrecurserPower(this);
				}
				if (mandalaSet.Item1)
				{
					Items.Armors.Mandala.MandalaHood.SwapModes(this);
				}
				if (desertSet)
				{
					Items.Armors.Desert.DesertHelmet.ActivateSandySwiftness(this);
				}

				if (vibraniumSet)
				{
					if (player.controlTorch)
					{
						if (vibraniumSetWall)
							vibraniumSetWall = false;
						else
							vibraniumSetWall = true;
					}
					else
					{
						if (vibraniumSetPlatform)
							vibraniumSetPlatform = false;
						else
							vibraniumSetPlatform = true;

						Main.PlaySound(SoundID.Zombie, (int)player.Center.X, (int)player.Center.Y, 68, 1f, vibraniumSetPlatform ? -0.25f : 0.35f);
					}
				}
				Items.Armors.Engineer.EngineerArmorPlayer EAP = player.GetModPlayer<Items.Armors.Engineer.EngineerArmorPlayer>();
				if (EAP.EngieArmor())
				{
					EAP.ToggleEngieArmor();
				}

				if (Main.netMode != NetmodeID.Server)
				{
					SGAmod.RecipeIndex += 1;
				}
			}

			if (Main.netMode != NetmodeID.Server && SGAmod.ToggleGamepadKey.JustPressed)
			{
				if (Main.LocalPlayer.SGAPly().gamePadAutoAim > 0)
				{
					LockOnHelper.CycleUseModes();
					Main.PlaySound(22, -1, -1, 0, 1f, 1.5f);
				}
			}

			if (SGAmod.CollectTaxesHotKey.JustPressed)
			{
				if (EALogo)
				{
					if (player.taxMoney > Item.buyPrice(0, 1, 0, 0))
					{
						player.taxMoney -= Item.buyPrice(0, 1, 0, 0);
						player.QuickSpawnItem(ItemID.GoldCoin);
					}
				}
			}
			if (SGAmod.WalkHotKey.JustPressed)
			{
				Walkmode = Walkmode ? false : true;
				if (Main.LocalPlayer == player)
				{
					Main.PlaySound(17, -1, -1, 0, 1f, Walkmode ? -0.25f : 0.35f);

				}
			}
			if (SGAmod.GunslingerLegendHotkey.JustPressed)
			{
				if (gunslingerLegend && AddCooldownStack(60 * 30, testOnly: true))
				{
					float dist = 999999;
					int theone = -1;
					int theonetype = -1;
					for (int num172 = 0; num172 < Main.maxNPCs; num172 += 1)
					{
						NPC target = Main.npc[num172];
						if (target.active && !target.townNPC && !target.dontTakeDamage && target.CanBeChasedBy())// && ((target.modNPC!=null && target.modNPC.CanBeHitByProjectile(projectile)==true) || target.modNPC==null))
						{
							float dit = target.Distance(Main.MouseWorld);
							if (dit < dist)
							{
								dist = dit;
								theone = target.whoAmI;
								theonetype = target.type;
							}

						}

					}
					if (theone > -1)
					{
						if (AddCooldownStack(60 * 30))
						{
							Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/P5Targeted").WithVolume(1f).WithPitchVariance(.10f), player.Center);
							gunslingerLegendtarget = theone;
							gunslingerLegendtargettype = theonetype;
							lockoneffect = 0;
							//Effects I guess
						}
					}
				}
			}
			if (SGAmod.NinjaSashHotkey.JustPressed)
			{
				if (ninjaSash > 1 && AddCooldownStack(60 * 60))
				{
					Main.PlaySound(SoundID.Item39, player.Center);
					Vector2 tomouse = Main.MouseWorld - player.Center;
					tomouse = tomouse.SafeNormalize(Vector2.Zero);
					tomouse *= 16f;
					int thisoned = Projectile.NewProjectile(player.Center.X, player.Center.Y, tomouse.X, tomouse.Y, ModContent.ProjectileType<NinjaBombProj>(), 0, 0f, Main.myPlayer);
					Main.projectile[thisoned].netUpdate = true;
				}
			}
		}
		public override void ModifyNursePrice(NPC nurse, int health, bool removeDebuffs, ref int price)
		{
			if (Hellion.GetHellion() != null)
			{
				price = int.MaxValue - 10;
			}
		}
        public override void PostNurseHeal(NPC nurse, int health, bool removeDebuffs, int price)
        {
			string[] strs = new string[] { "Stay close, I obviously can't work on you from a distance", "Alright, for the next new seconds I can quickly patch your wounds", "Make sure you keep any unwanted aggressives away in the meantime", "It's not instant but that's just life" };
			Main.npcChatText = strs[Main.rand.Next(strs.Length)];
			//stuff
		}
        public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{

			if (tidalCharm > 0)
			{
				Vector2 position2 = player.position;

				if (true)
				{

					if (Main.rand.Next(3) == 0)
					{
						int num52 = Dust.NewDust(position2 - new Vector2(2, 1), player.width + 4, player.height + 2, 211, 0f, 0f, 210, Color.Blue, 1.7f);
						Dust dust;
						if (Main.rand.Next(2) == 0)
						{
							dust = Main.dust[num52];
							dust.alpha += 25;
						}
						if (Main.rand.Next(2) == 0)
						{
							dust = Main.dust[num52];
							dust.alpha += 25;
						}
						Main.dust[num52].noLight = true;
						Main.dust[num52].noGravity = true;
						dust = Main.dust[num52];
						dust.velocity *= 0.2f;
						Dust dust9 = Main.dust[num52];
						dust9.velocity.Y = dust9.velocity.Y + 0.2f;
						dust = Main.dust[num52];
						dust.velocity += player.velocity / 3f;
						Main.playerDrawDust.Add(num52);
					}
					else
					{
						int num53 = Dust.NewDust(position2 - new Vector2(4, 4), player.width + 8, player.height + 8, 211, 0f, 0f, 100, Color.DeepSkyBlue, 0.8f);
						Dust dust;
						if (Main.rand.Next(2) == 0)
						{
							dust = Main.dust[num53];
							dust.alpha += 25;
						}
						if (Main.rand.Next(2) == 0)
						{
							dust = Main.dust[num53];
							dust.alpha += 25;
						}
						Main.dust[num53].noLight = true;
						Main.dust[num53].noGravity = true;
						dust = Main.dust[num53];
						dust.velocity *= 0.2f;
						Dust dust10 = Main.dust[num53];
						dust10.velocity.Y = dust10.velocity.Y + 1f;
						dust = Main.dust[num53];
						dust.velocity += player.velocity / 3f;
						Main.playerDrawDust.Add(num53);
					}
				}
			}

			if (Lockedin)
			{
				int q = 3;
				for (q = 0; q < 2; q++)
				{

					int dust = Dust.NewDust(new Vector2(Main.rand.Next(0, 100) < 50 ? Locked.X : Locked.X + Locked.Y, drawInfo.position.Y), player.width + 4, player.height + 4, DustID.AncientLight, 0f, player.velocity.Y * 0.4f, 100, default(Color), 3f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].color = Main.hslToRgb((float)(Main.GlobalTime / 50) % 1, 0.9f, 0.65f);
					//Main.dust[dust].velocity *= 1.8f;
					//Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);
				}
				//r *= 0.1f;
				//g *= 0.2f;
				//b *= 0.7f;
				//fullBright = true;
			}
			if (MassiveBleeding)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				int dust = Dust.NewDust(new Vector2(drawInfo.position.X, drawInfo.position.Y) + randomcircle * 8f, player.width + 4, player.height + 4, 5, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 30, default(Color), 1.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Main.hslToRgb(0f, 0.5f, 0.35f);
				//Main.dust[dust].velocity *= 1.8f;
				//Main.dust[dust].velocity.Y -= 0.5f;
				Main.playerDrawDust.Add(dust);
			}

			if (SunderedDefense)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y) + randomcircle * (1.2f * (float)player.width), player.width + 4, player.height + 4, mod.DustType("TornadoDust"), player.velocity.X * 0.4f, (player.velocity.Y - 7f) * 0.4f, 30, default(Color) * 1f, 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Main.hslToRgb(0f, 0.5f, 0.35f);
				Main.playerDrawDust.Add(dust);
			}

			if (thermalblaze)
			{
				if (Main.rand.Next(4) == 0 && drawInfo.shadow == 0f)
				{
					int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, ModContent.DustType<Dusts.HotDust>(), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default(Color), 1f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);
				}
				r *= 0.1f;
				g *= 0.2f;
				b *= 0.7f;
				fullBright = true;
			}

			if (acidburn)
			{
				if (Main.rand.Next(4) == 0 && drawInfo.shadow == 0f)
				{
					int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, ModContent.DustType<Dusts.AcidDust>(), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default(Color), 1f);
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);
				}
				r *= 0.1f;
				g *= 0.7f;
				b *= 0.1f;
				fullBright = true;
			}


			if (Blazewyrmset)
			{
				if (Main.rand.Next(8) == 0 && drawInfo.shadow == 0f)
				{
					int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, ModContent.DustType<Dusts.HotDust>(), player.velocity.X * 0.8f, player.velocity.Y * 0.8f, 200, default(Color), 0.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);
				}
			}

		}

		public override void SendCustomBiomes(BinaryWriter writer)
		{
			BitsByte newbim = new BitsByte();
			newbim[0] = DankShrineZone;
			newbim[1] = ShadowSectorZone > 0;
			writer.Write(newbim);
		}

		public override void ReceiveCustomBiomes(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			DankShrineZone = flags[0];
			ShadowSectorZone = flags[1] ? (byte)5 : (byte)0;
		}


		public override Texture2D GetMapBackgroundImage()
		{
			if (DankShrineZone)
			{
				return mod.GetTexture("swamp_map_background");
			}
			return null;
		}

		public override void UpdateBiomeVisuals()
		{
			//TheProgrammer
			player.ManageSpecialBiomeVisuals("SGAmod:ProgramSky", (SGAmod.ProgramSkyAlpha > 0f || NPC.CountNPCS(mod.NPCType("SPinkyTrue")) > 0) ? true : false, player.Center);
			player.ManageSpecialBiomeVisuals("SGAmod:HellionSky", (SGAmod.HellionSkyalpha > 0f || NPC.CountNPCS(mod.NPCType("Hellion")) + NPC.CountNPCS(mod.NPCType("HellionFinal")) > 0) ? true : false, player.Center);
			player.ManageSpecialBiomeVisuals("SGAmod:SwirlingVortex", SPinkyTrue.VortexEffect, SPinkyTrue.PinkyBossLoc);


			player.ManageSpecialBiomeVisuals("SGAmod:CirnoBlizzard", (SGAWorld.CirnoBlizzard > 0) ? true : false, player.Center);


			ScreenShaderData shad = Filters.Scene["SGAmod:CirnoBlizzard"].GetShader();
			if (SGAWorld.CirnoBlizzard > 0)
				shad.UseOpacity((float)(SGAWorld.CirnoBlizzard / 1000f));
			else
				shad.UseOpacity(0f);
		}

		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();

			if (playercreated == false)
			{
				if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
				{
					if (SGAmod.NightmareUnlocked)
					{
						Main.PlaySound(29, -1, -1, 105, 1f, -0.6f);
						nightmareplayer = true;
					}
				}
			}
			playercreated = true;
			tag["playercreated"] = playercreated;
			tag["gottf2"] = gottf2;
			tag["gothellion"] = gothellion;
			tag["devpower"] = devpowerbool;
			tag["devpowerint"] = devpower;
			tag["Redmanastar"] = Redmanastar;
			tag["Electicpermboost"] = Electicpermboost;

			tag["ZZZExpertiseCollectedZZZ"] = ExpertiseCollected;
			tag["ZZZExpertiseCollectedTotalZZZ"] = ExpertiseCollectedTotal;
			tag["resetver"] = resetver;
			tag["nightmareplayer"] = nightmareplayer;
			tag["entropycollected"] = entropyCollected;
			tag["Drakenshopunlock"] = Drakenshopunlock;
			tag["benchGodFavor"] = benchGodFavor;
			tag["dragonFriend"] = dragonFriend;
			tag["midasMoneyConsumed"] = midasMoneyConsumed;
			if (!SGAmod.craftBlockPanel.ItemPanel.item.IsAir && benchGodFavor)
			{
				tag["BenchGodItemX"] = SGAmod.craftBlockPanel.ItemPanel.item.type;
				tag["BenchGodItemY"] = SGAmod.craftBlockPanel.ItemPanel.item.stack;
			}

			SaveExpertise(ref tag);

			//ExpertisePointsFromBosses = null;
			//ExpertisePointsFromBossesPoints = null;
			return tag;
		}

		public override void Load(TagCompound tag)
		{
			CooldownStacks = new List<ActionCooldownStack>();
			skillMananger = new SkillManager(player);

			playercreated = true;

			ExpertiseCollected = 0;
			ExpertiseCollectedTotal = 0;
			gottf2 = tag.GetBool("gottf2");
			gothellion = tag.GetBool("gothellion");
			devpowerbool = tag.GetBool("devpower");
			devpower = tag.GetInt("devpowerint");
			Redmanastar = tag.GetInt("Redmanastar");

			resetver = tag.GetInt("resetver");
			if (tag.ContainsKey("nightmareplayer"))
				nightmareplayer = tag.GetBool("nightmareplayer");
			if (tag.ContainsKey("Electicpermboost"))
				Electicpermboost = tag.GetInt("Electicpermboost");

			if (tag.ContainsKey("Drakenshopunlock"))
				Drakenshopunlock = tag.GetBool("Drakenshopunlock");

			if (tag.ContainsKey("entropycollected"))
				entropyCollected = tag.GetInt("entropycollected");

			if (tag.ContainsKey("dragonFriend"))
				dragonFriend = tag.GetBool("dragonFriend");

			if (tag.ContainsKey("midasMoneyConsumed"))
				midasMoneyConsumed = tag.GetInt("midasMoneyConsumed");

			//SGAmod.craftBlockPanel.ItemPanel.item.TurnToAir();

			benchGodItem.X = -1;
			if (tag.ContainsKey("benchGodFavor"))
				benchGodFavor = tag.GetBool("benchGodFavor");

			if (benchGodFavor && tag.ContainsKey("BenchGodItemX"))
			{
				benchGodItem.X = tag.GetInt("BenchGodItemX");
				benchGodItem.Y = tag.GetInt("BenchGodItemY");
			}

			LoadExpertise(tag);

		}

		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
		{
			if (bait.type == ModContent.ItemType<Items.Tools.UniversalBait>())
			{
				int fish = Items.Tools.UniversalBait.GetFish();
				if (fish > -1)
				{
					caughtType = fish;
					return;
				}
			}
			if (junk)
			{
				return;
			}
			if (bait.type == mod.ItemType("SharkBait"))
			{
				//hmmmm
			}
			if (DankShrineZone)
			{

				if (questFish == mod.ItemType("Vinefish") && Main.rand.Next(2) == 0)
				{
					caughtType = mod.ItemType("Vinefish");
				}
				if (questFish == mod.ItemType("Rootfish") && Main.rand.Next(2) == 0)
				{
					caughtType = mod.ItemType("Rootfish");
				}

				int chance = 10 + (player.cratePotion ? 15 : 0) + (int)Math.Min(50, power);
				if (Main.rand.Next(0, 100) < chance)
					caughtType = mod.ItemType("DankCrate");

			}
			/*if (player.ZoneSkyHeight)
			{
				int chance = 2 + (int)Math.Min(20, power/5);
				if (Main.rand.Next(0, 100) < chance)
					caughtType = mod.ItemType("StarCollector");
			}*/

		}
	}

	public class IDGStartBag : StartBag
	{
		private List<Item> items = new List<Item>();

		public override string Texture
		{
			get
			{
				return "SGAmod/Items/Consumables/derg_bag";
			}
		}

		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("IDG's Starting Bag");
			base.Tooltip.SetDefault("Some starting items couldn't fit in your inventory??\n{$CommonItemTooltip.RightClickToOpen}");
		}

		public override void SetDefaults()
		{
			base.item.width = 20;
			base.item.height = 20;
			base.item.rare = 2;

		}

		public override void RightClick(Player player)
		{

			List<int> loot = new List<int>();

			loot.Add(Main.rand.NextBool() ? ItemID.TsunamiInABottle : (Main.rand.NextBool() ? ItemID.FartinaJar : ItemID.CloudinaBottle));
			loot.Add(Main.rand.NextBool() ? ItemID.SailfishBoots : ItemID.HermesBoots);
			loot.Add(SGAmod.Instance.ItemType("ThrowerPouch")); loot.Add(ItemID.GrapplingHook); loot.Add(ItemID.MiningHelmet);
			loot.Add(Main.rand.NextBool() ? ItemID.TungstenPickaxe : ItemID.SilverPickaxe);

			List<int> itemsbonus = new List<int>();

			//int[] loot = { ItemID.TsunamiInABottle, ItemID.FartinaJar, ItemID.CloudinaBottle, SGAmod.Instance.ItemType("ThrowerPouch"), ItemID.HermesBoots, ItemID.SailfishBoots, ItemID.GrapplingHook, ItemID.SilverPickaxe, ItemID.TungstenPickaxe, ItemID.MiningHelmet };
			int[] loot2 = { ItemID.ShinePotion, ItemID.BuilderPotion, ItemID.MiningPotion, ItemID.NightOwlPotion };
			for (int zz = 0; zz < 3; zz++)
			{
				int index = Main.rand.Next(0, loot.Count);
				itemsbonus.Add(loot[index]);
			}

			Item item3 = new Item();
			item3.SetDefaults(ItemID.LifeCrystal, false);
			((IDGStartBag)item.modItem).AddItem(item3);

			Item item4 = new Item();
			item4.SetDefaults(SGAmod.Instance.ItemType("BossHints"), false);
			((IDGStartBag)item.modItem).AddItem(item4);

			for (int k = 0; k < itemsbonus.Count; k++)
			{
				Item item2 = new Item();
				item2.SetDefaults(itemsbonus[k], false);
				((IDGStartBag)item.modItem).AddItem(item2);
			}
			itemsbonus.Clear();

			for (int zz = 0; zz < 8; zz++)
			{
				itemsbonus.Add(loot2[Main.rand.Next(0, loot2.Length)]);
			}
			for (int gg = 0; gg < 7; gg += 3)
			{
				for (int k = 0; k < itemsbonus.Count; k++)
				{
					if (Main.rand.Next(gg) < 1)
					{
						Item item2 = new Item();
						item2.SetDefaults(itemsbonus[k], false);
						item2.stack = Main.rand.Next(1, 2);
						((IDGStartBag)item.modItem).AddItem(item2);
					}
				}
			}


			foreach (Item current in this.items)
			{
				int number = Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, current.type, current.stack, false, (int)current.prefix, false, false);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(21, -1, -1, null, number, 1f, 0f, 0f, 0, 0, 0);
				}
			}
		}

		private void AddItem(Item item)
		{
			this.items.Add(item);
		}

	}
	public class AntiBustedPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Consolation Prize");
			Tooltip.SetDefault("A stroke of bad RNG has given you this stronger pickaxe! (can chop too!)");
		}

		public override string Texture => "Terraria/Item_"+ItemID.PlatinumPickaxe;

        public override Color? GetAlpha(Color lightColor)
        {
			return Main.DiscoColor;
        }

        public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.PlatinumAxe);
			item.CloneDefaults(ItemID.PlatinumPickaxe);
			item.useAnimation = (int)(item.useAnimation/1.50f);
			item.useTime = (int)(item.useTime / 1.50f);
		}

	}


}