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
using SGAmod.NPCs;
using SGAmod.NPCs.Wraiths;
using SGAmod.NPCs.Cratrosity;
using SGAmod.NPCs.Murk;
using SGAmod.NPCs.Sharkvern;
using SGAmod.NPCs.SpiderQueen;
using SGAmod.NPCs.Hellion;
using CalamityMod;
using AAAAUThrowing;
using Terraria.Utilities;
using SGAmod.SkillTree;

namespace SGAmod
{

	public class ActionCooldownStack
	{
		public int timeleft;
		public int timerup;
		public int maxtime;

		public ActionCooldownStack(int timeleft)
		{
			this.timeleft = timeleft;
			this.maxtime = timeleft;
			timerup = 0;
		}

	}

		public class SGAPlayer : ModPlayer
	{
		public List<ActionCooldownStack> CooldownStacks;
		public SkillManager skillMananger;
		public bool noactionstackringofrespite = false;
		public int tf2emblemLevel = 0;
		public int ninjaSash = 0;
		public int shinobj = 0;
		public int surprised = 0;
		public float[] beserk = { 0, 0 };
		public float actionCooldownRate = 1f;
		public int previoustf2emblemLevel = 0;
		public int lockoneffect = 0;
		public int soldierboost = 0;
		public bool dualityshades = false;
		public bool gunslingerLegend = false;
		public int gunslingerLegendtarget = -1;
		public int gunslingerLegendtargettype = 0;
		public ushort FireBreath = 0;
		public int MaxCooldownStacks = 1;
		public float RevolverSpeed = 1f;
		public bool Duster = false;
		public bool Drakenshopunlock = false;
		public int DefenseFrame = 0;
		public int ReloadingRevolver = 0;
		public int CustomWings = 0;
		public int JoyrideShake = 0;
		public bool twinesoffate = false;
		public bool Walkmode = false;
		public bool MVMBoost = false;
		public bool Shieldbreak = false;
		public byte ShieldType = 0;
		public int realIFrames = 0;
		public int myammo = 0;
		public int timer = 0;
		public int beefield = 0;
		public float morespawns = 1f;
		public float SlowDownDefense = 0f;
		public float SlowDownResist = 1f;
		public int SlowDownReset = 0;
		public float damagetaken = 1f;
		public float mspeed = 1f;
		public int beefieldtoggle = 0;
		public int beefieldcounter = 0;
		public bool HeavyCrates = false;
		public bool Microtransactions = false;
		public bool MoneyMismanagement = false;
		public bool NoFly = false;
		public bool Pressured = false;
		public bool MassiveBleeding = false;
		public bool ELS = false;
		public bool ActionCooldown = false;
		public bool thermalblaze = false; public bool acidburn = false;
		public bool LifeFlower = false; public bool GeyserInABottle = false; public bool GeyserInABottleActive = false; public bool JavelinBaseBundle = false; public bool JavelinSpearHeadBundle = false; public bool PrimordialSkull = false;
		public bool MatrixBuffp = false; public bool BoosterMagnet = false; public bool HoE = false; public bool CalamityRune = false;
		public int EnhancingCharm = 0;
		public int FieryheartBuff = 0;
		public int creeperexplosion = 0;
		public bool DankShrineZone = false;
		public bool noModTeleport = false;
		private Projectile[] projectileslunarslime = new Projectile[15];

		public bool lunarSlimeHeart = false;
		public int lunarSlimeHeartdamage = 1;
		public int lunarSlimeCounter = 0;
		public bool SunderedDefense = false;

		public bool Lockedin = false;
		public bool CirnoWings = false;
		public bool SerratedTooth = false;
		private int lockedelay = 0;
		public int Novusset = 0; public int Noviteset = 0; public bool Blazewyrmset = false; public bool SpaceDiverset = false; public bool MisterCreeperset = false; public bool Mangroveset = false; public int Dankset = 0; public bool IDGset = false;
		public float SpaceDiverWings = 0f;
		public int Havoc = 0;
		public int NoFireBurn = 0;
		public int breathingdelay = 0;
		public int sufficate = 200;
		public float UseTimeMul = 1f;
		public bool Noselfdamage = false;
		public float UseTimeMulPickaxe = 1f;
		public float TrapDamageMul = 1f; public float TrapDamageAP = 0f;
		public float ThrowingSpeed = 1f; public float Thrownsavingchance = 0f;
		public Vector2 Locked = new Vector2(100, 300);
		public int electricCharge = 0; public int electricChargeMax = 0; public float electricChargeCost = 1f; public float electricChargeReducedDelay = 1f;
		public int ammoLeftInClip = 6; public int plasmaLeftInClip = 1000;
		public int ammoLeftInClipMax = 6; public int plasmaLeftInClipMax = 1000;
		public int boosterPowerLeft = 10000; public int boosterPowerLeftMax = 10000; public float boosterdelay = -500;  public float electricdelay = -500; public int boosterrechargerate = 15; public int electricrechargerate = 1;
		public int digiStacks = 0; public int digiStacksMax = 0;
		public int digiStacksCount = 16;
		public bool modcheckdelay = false;
		public bool gottf2 = false; public bool gothellion = false;
		public int floatyeffect = 0;
		public int PrismalShots = 0;
		public int devpower = 0;
		public float beedamagemul = 1f;
		public bool JaggedWoodenSpike = false; public bool JuryRiggedSpikeBuckler = false; public bool HeartGuard = false; public bool GoldenCog = false;
		public bool devpowerbool = false; public int Redmanastar = 0; public int Electicpermboost = 0;
		public int MidasIdol = 0; public bool OmegaSigil = false;
		public bool MurkyDepths = false;
		public int[] ammoinboxes = new int[4];
		public int anticipation = 0; public int anticipationLevel = 0;
		public float recoil = 0;
		public int greandethrowcooldown = 0;
		public int? resetver = 1;
		public bool nightmareplayer = false;
		public bool playercreated = false;
		public bool granteditems = false;
		public float techdamage = 1f;
		public double[] apocalypticalChance = { 0, 0, 0, 0 };
		public float apocalypticalStrength = 1f;
		public int entropycollected = 0;
		public int activestacks = 0;
		public float greedyperc = 0f;
		public float lifestealentropy = 0f;
		public int maxblink = 0;
		public int potionsicknessincreaser = 0;
		public bool EALogo = false;
		public bool demonsteppers = false;
		public bool FridgeflameCanister = false;
		public bool IceFire = false;
		public bool BIP = false;
		public float summonweaponspeed = 0f;
		public bool grippinggloves = false;
		public bool mudbuff = false;
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


		enum MessageType : byte
		{
			ClientSendInfo
		}

		public int Microtransactionsdelay = 0;

		public bool CalamityAbyss
		{
			get
			{
				/*Player player2 = (this as ModPlayer).player;
				if (ModLoader.GetMod("CalamityMod") != null)
				{
					CalamityPlayer CPlayer = player2.GetModPlayer(ModLoader.GetMod("CalamityMod"), "CalamityPlayer") as CalamityPlayer;
					Type CType = CPlayer.GetType();
					PropertyInfo CProperty = CType.GetProperty("ZoneAbyss");

					if (CProperty != null)
					{
						return !CPlayer.ZoneAbyss;
					}
				}*/
				return false;

			}
		}

		public void AddEntropy(int ammount)
		{
			entropycollected += ammount;
			while (entropycollected > 100000)
			{
				entropycollected -= 100000;
				for (int fgf = 0; fgf < 20; fgf += 1)
				{
					int type = -1;
					if (player.HasItem(ItemID.CrimtaneOre))
						type = ItemID.CrimtaneOre;
					if (player.HasItem(ItemID.DemoniteOre))
						type = ItemID.DemoniteOre;
					if (type > -1)
					{
						player.ConsumeItem(type);
						player.QuickSpawnItem(mod.ItemType("Entrophite"));
					}
				}
			}

		}

		public float GetMinionSlots
		{
			get
			{
				float ammount = 0f;
				for (int num27 = 0; num27 < Main.maxProjectiles; num27 += 1)
				{
					if (Main.projectile[num27].active && Main.projectile[num27].owner == player.whoAmI && Main.projectile[num27].minion && ProjectileID.Sets.MinionSacrificable[Main.projectile[num27].type])
						ammount += Main.projectile[num27].minionSlots;
				}
				return ammount;
			}
		}

		public bool AddCooldownStack(int time,int count=1)
		{
			if (CooldownStacks.Count+ (count-1) < MaxCooldownStacks) 
			{
				//if (player.HasBuff(mod.BuffType("CondenserBuff")))
				//	time = (int)((float)time * 1.15f);

				time = (int)((float)time * actionCooldownRate);

				for (int i = 0; i < count; i += 1)
					CooldownStacks.Add(new ActionCooldownStack(time));
				return true;
			}
			return false;

		}

		public bool ConsumeElectricCharge(int requiredcharge,int delay,bool damage=false)
		{
			if (electricCharge > requiredcharge)
			{
				electricdelay = Math.Max(delay * electricChargeReducedDelay, electricdelay);
				electricCharge -= (int)((float)requiredcharge*electricChargeCost);
				return true;
			}

			if (damage && ShieldType > 0 && electricCharge>0 && electricCharge-requiredcharge <0)
			{
				electricCharge = 0;
				electricdelay = 30;
				player.AddBuff(mod.BuffType("Shieldbreak"), 60 * 5);
			}

			return false;
		}
		public void StackAttack(ref int damage, Projectile proj)
		{

			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			if (player.HeldItem != null)
			{
				if (sgaplayer.IDGset && player.HeldItem.ranged)
				{

					int bonusattacks = (int)(((float)sgaplayer.digiStacks / (float)sgaplayer.digiStacksMax) * (float)sgaplayer.digiStacksCount);

					int ammotype = (int)player.GetModPlayer<SGAPlayer>().myammo;
					if (ammotype > 0 && bonusattacks > 0 && proj.damage > 100)
					{
						Item ammo2 = new Item();
						ammo2.SetDefaults(ammotype);
						int ammo = ammo2.shoot;
						int damageproj = proj.damage;
						float knockbackproj = proj.knockBack;
						float sppez = proj.velocity.Length();
						if (ammo2.modItem != null)
							ammo2.modItem.PickAmmo(player.HeldItem, player, ref ammo, ref sppez, ref proj.damage, ref proj.knockBack);
						int type = ammo;

						if (proj.type == mod.ProjectileType("SoldierRocketLauncherProj"))
							type = mod.ProjectileType("SoldierRocketLauncherProj");


						for (int i = 0; i < bonusattacks; i += 1)
						{
							int subtracter = sgaplayer.digiStacks -= damage;
							if (i > -1)
							{
								float angle = MathHelper.ToRadians(sgaplayer.timer + ((((float)i - 1) / (float)bonusattacks) * 360f));
								Vector2 apos = player.Center + new Vector2((float)Math.Cos(angle) * 64, (float)Math.Sin(angle) * 12);
								int probg = Projectile.NewProjectile(new Vector2(apos.X, apos.Y), proj.velocity, type, (int)(damage * 0.50f), 0f, player.whoAmI);
								Main.projectile[probg].GetGlobalProjectile<SGAprojectile>().stackedattack = true;
							}
							else
							{
								if (subtracter > 0)
								{
									//float thedamage = (sgaplayer.digiStacks / sgaplayer.digiStacksMax);
									//damage = damage + (int)thedamage;
								}
							}
							if (subtracter > 0)
							{
								sgaplayer.digiStacks = subtracter;
							}
						}
					}
				}

			}

		}

		public int IsRevolver(Item item)
		{
			if (item == null || item.IsAir)
				return 0;
			if (SGAmod.UsesClips.ContainsKey(item.type))
			{
				if (item.type == mod.ItemType("SoldierRocketLauncher"))
					return 2;
				return 1;
			}
			return 0;
		}
		public bool RefilPlasma(bool checkagain=false)
		{
			if (plasmaLeftInClip > 0)
			{
				return true;
			}

			if (plasmaLeftInClip < 1 || checkagain)
			{

				if (player.HasItem(mod.ItemType("PlasmaCell")))
				{
					player.ConsumeItem(mod.ItemType("PlasmaCell"));
					plasmaLeftInClip = Math.Min(plasmaLeftInClip+1000,plasmaLeftInClipMax);
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.LawnGreen, "Plasma Recharged!", false, false);
					if (plasmaLeftInClip<plasmaLeftInClipMax && checkagain)
					{
						RefilPlasma(true);

					}
					return true;
				}
			}
			return false;
		}

		public static void LimitProjectiles(Player player, int maxprojs, ushort[] types)
		{

			int projcount = 0;
			for (int a = 0; a < types.Length; a++)
			{
				projcount += player.ownedProjectileCounts[(int)types[a]];
			}

			Projectile removethisone = null;
			int timecheck = 99999999;

			if (projcount > maxprojs) {
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile him = Main.projectile[i];
					if (types.Any(x => x == Main.projectile[i].type)) {
						if (him.active && him.owner == player.whoAmI && him.timeLeft < timecheck) {
							removethisone = him;
							timecheck = him.timeLeft;
						} } }
				if (removethisone != null) {
					removethisone.Kill();
				} }

		}

		public bool HasGucciGauntlet()
		{
			Item minecart = player.miscEquips[4];
			if (!minecart.IsAir)
			{
				if (minecart.modItem != null)
				{
					if (minecart.type == mod.ItemType("GucciGauntlet"))
					{
						return true;
					}
				}
			}
		return false;
		}

		public void upgradetf2()
		{
			if (!gottf2 && player == Main.LocalPlayer)
			{
				Main.NewText("You have received your TF2 Emblem!", 150, 150, 150);
				player.QuickSpawnItem(mod.ItemType("TF2Emblem"), 1);
				gottf2 = true;
			}
		}

		public override void UpdateBiomes()
		{
			DankShrineZone = (SGAWorld.MoistStonecount > 15 && Main.tile[(int)(player.Center.X/16), (int)(player.Center.Y / 16)].wall==mod.WallType("SwampWall"));
		}

		public override void PlayerDisconnect(Player player)
		{
			downedHellion = 0;
		}

		public override void ResetEffects()
		{
			MVMBoost = false;
			gunslingerLegend = false;
			if (!player.HasBuff(mod.BuffType("ConsumeHellBuff")))
			FireBreath = 0;
			beserk[0]-=1;
			if (beserk[0] < 1)
			{
				beserk[0] = 0; beserk[1] = 0;
			}

			if (soldierboost>0)
			soldierboost-=1;
			surprised = Math.Max(surprised - 1, 0);
			shinobj -= 1;
			previoustf2emblemLevel = tf2emblemLevel;
			tf2emblemLevel = 0;
			ninjaSash = 0;
			RevolverSpeed = 1f;
			ReloadingRevolver = Math.Max(ReloadingRevolver - 1, 0);
			twinesoffate = false;
			Duster = false;
			dualityshades = false;
			realIFrames -= 1;
			HeavyCrates = false;
			Microtransactions = false;
			MoneyMismanagement = false;
			Lockedin = false;
			NoFly = false;
			CirnoWings = false;
			MassiveBleeding = false;
			thermalblaze = false; acidburn = false; ELS = false;
			SerratedTooth = false;
			UseTimeMul = 1f;
			UseTimeMulPickaxe = 1f;
			ThrowingSpeed = 1f;
			SpaceDiverset = false;
			potionsicknessincreaser = 0;
			Blazewyrmset = false;
			Mangroveset = false;
			IDGset = false;
			Pressured = false;
			Havoc = 0;
			SunderedDefense = false;
			lockoneffect = Math.Min(lockoneffect+1,5000);

			if (ammoLeftInClip > ammoLeftInClipMax)
			ammoLeftInClip = ammoLeftInClipMax;

			ammoLeftInClipMax = 6;
			SpaceDiverWings = 0f;
			ActionCooldown = false;
			lunarSlimeHeart = false;
			TrapDamageMul = 1f; TrapDamageAP = 0f;
			Thrownsavingchance = 0f;
			LifeFlower = false; GeyserInABottleActive = false; JavelinBaseBundle = false; JavelinSpearHeadBundle = false;
			BoosterMagnet = false;
			EnhancingCharm = 0;
			if (devpower>0)
			devpower -= 1;
			devpowerbool = false;
			MisterCreeperset = false;
			Noselfdamage = false;
			JaggedWoodenSpike = false; JuryRiggedSpikeBuckler = false; HeartGuard = false; GoldenCog = false;
			MidasIdol = 0;
			OmegaSigil = false;
			MurkyDepths = false;
			MatrixBuffp = false;
			plasmaLeftInClipMax = 1000;
			beedamagemul = 1f;
			anticipationLevel = -1;
			noModTeleport = false;
			PrimordialSkull = false;
			NoFireBurn = Math.Max(NoFireBurn-1,0);
			if (player.itemTime < 1)
			recoil = Math.Max(recoil - 0.5f, 0f);
			greandethrowcooldown = Math.Max(greandethrowcooldown - 1, 0);
			techdamage = 1f;
			HoE = false;
			CalamityRune = false;
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
			summonweaponspeed = 0f;
			SlowDownReset -= 1;
			grippinggloves = false;
			timer += 1;
			mudbuff = false;
			boosterdelay -= 1;
			digiStacks = (int)MathHelper.Clamp(digiStacks,0,digiStacksMax);
			CustomWings = 0;
			JoyrideShake -= 1;

			if (!Shieldbreak)
				electricdelay -= 1;

			if (boosterdelay < 1)
			{
				boosterPowerLeft = Math.Min(boosterPowerLeft+ boosterrechargerate, boosterPowerLeftMax);
			}
			if (electricdelay < 1)
			{
				electricCharge = Math.Min(electricCharge + electricrechargerate, electricChargeMax);
			}

			electricChargeMax = Electicpermboost;
			electricrechargerate = 0;
			electricChargeCost = 1f;
			electricChargeReducedDelay = 1f;
			boosterrechargerate = 15;
			boosterPowerLeftMax = 10000;
			Shieldbreak = false;
			ShieldType = 0;

			for (int a = 0; a < devempowerment.Length; a++)
				devempowerment[a] = Math.Max(devempowerment[a]-1,0);

			for (int i = 0; i < armorglowmasks.Length; i += 1)
			{
				armorglowmasks[i] = null;
				armorglowcolor[i] = delegate (Player player, int index)
				{
					return Color.White;
				};
			digiStacksMax = 0;
				player.breathMax = 200;
			}
			MaxCooldownStacks = 1;
			noactionstackringofrespite = false;
			actionCooldownRate = 1f;
			Noviteset = 0;
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{

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

			if (MisterCreeperset && AddCooldownStack(180*60))
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
			sgaplayer.sufficate = sufficate;
			sgaplayer.PrismalShots = PrismalShots;
			sgaplayer.plasmaLeftInClip = plasmaLeftInClip;
			sgaplayer.Redmanastar = Redmanastar;
			sgaplayer.ExpertiseCollected = ExpertiseCollected;
			sgaplayer.ExpertiseCollectedTotal = ExpertiseCollectedTotal;
			sgaplayer.entropycollected = entropycollected;
			sgaplayer.DefenseFrame = DefenseFrame;
			sgaplayer.gunslingerLegendtarget = gunslingerLegendtarget;
			sgaplayer.activestacks = activestacks;

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
			if (sgaplayer.ammoLeftInClip != ammoLeftInClip || sgaplayer.sufficate != sufficate || sgaplayer.PrismalShots != PrismalShots || sgaplayer.entropycollected != entropycollected || sgaplayer.DefenseFrame != DefenseFrame
			|| sgaplayer.plasmaLeftInClip!= plasmaLeftInClip || sgaplayer.Redmanastar != Redmanastar || sgaplayer.ExpertiseCollected != ExpertiseCollected || sgaplayer.ExpertiseCollectedTotal != ExpertiseCollectedTotal
			 || sgaplayer.gunslingerLegendtarget != gunslingerLegendtarget || sgaplayer.activestacks != activestacks)
				mismatch = true;


			if (mismatch) {
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
				packet.Write((short)ammoLeftInClip);
				packet.Write(sufficate);
				packet.Write(PrismalShots);
				packet.Write(plasmaLeftInClip);
				packet.Write((short)Redmanastar);				
				packet.Write(ExpertiseCollected);
				packet.Write(ExpertiseCollectedTotal);
				packet.Write(entropycollected);
				packet.Write((short)DefenseFrame);
				packet.Write((short)gunslingerLegendtarget);
				packet.Write((short)activestacks);
				for (int i = 54; i < 58; i++)
				{
					packet.Write(ammoinboxes[i - 54]);
				}
				packet.Send();
			}

		}

		public override bool PreItemCheck()
		{
			return true;
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

			if (MassiveBleeding) {
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
				player.lifeRegen -= 30+boost;
			}
			if (acidburn)
			{
				player.lifeRegen -= 20 + player.statDefense;
				player.statDefense -= 5;
			}

			if (Pressured && !SpaceDiverset)
			{
				player.lifeRegen -= 250;
			}

			if (IceFire)
			{
				float hasbuffs = (player.HasBuff(BuffID.OnFire) || player.HasBuff(BuffID.Frostburn) ? 0.15f : 0f);
				if (player.lifeRegen < 0)
					player.lifeRegen = (int)(player.lifeRegen * (0.80 - hasbuffs));

				player.buffImmune[BuffID.OnFire] = false;
				player.buffImmune[BuffID.Frostburn] = false;
			}
		}

		public bool DashBlink()
		{
			if (noModTeleport)
				return false;

			if (maxblink > 0 && Math.Abs(player.dashTime)>0 && player.dashDelay< 1 && player.dash > 0)
			{
				int bufftime = 0;
				if (player.HasBuff(BuffID.ChaosState))
					bufftime = player.buffTime[player.FindBuffIndex(BuffID.ChaosState)];


				if (bufftime < maxblink && (player.controlUp))
				{				
					player.Teleport(player.Center + new Vector2(player.dashTime > 0 ? -8 : 0, -20), 1);
					for (int i = 0; i < 30; i += 1) {
						if (Collision.CanHit(player.Center, 16, 16, player.Center+ new Vector2(Math.Sign(player.dashTime)*8, 0), 16, 16))
						{
							player.Center += new Vector2(Math.Sign(player.dashTime)*8,0);

						}
						else
						{
							player.Center -= new Vector2(Math.Sign(player.dashTime)*16, 0);
							break;
						}
					}
					player.Teleport(player.Center+new Vector2(player.dashTime > 0 ? -8 : 0, -20), 1);
					player.dashTime = 0;
					player.dashDelay = 5;
					player.AddBuff(BuffID.ChaosState, bufftime + 120);

				return true;
				}

			}
			return false;
		}

		public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
		{
			Item item = new Item();

			item.SetDefaults(ModLoader.GetMod("SGAmod").ItemType("IDGStartBag"), false);

			items.Add(item);

		}

		public override void NaturalLifeRegen(ref float regen)
		{
			skillMananger.NaturalLifeRegen(ref regen);
		}

		public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
		{
			skillMananger.GetHealLife(item,quickHeal, ref healValue);
		}

		public override void GetHealMana(Item item, bool quickHeal, ref int healValue)
		{
			skillMananger.GetHealMana(item, quickHeal, ref healValue);
		}

		public override void PostUpdateRunSpeeds()
		{

			if (Noviteset > 0 && electricChargeMax > 0)
			{

				if (Noviteset > 1)
				{
					if (!Walkmode)
					{
						player.moveSpeed += (float)electricCharge / 7500f;
						player.maxRunSpeed += (float)electricCharge / 10000f;
						player.runAcceleration += (float)electricCharge / 12500f;
						player.accRunSpeed += (float)electricCharge / 7500f;
					}
				}
			}

			if (SlowDownDefense > 0f)
			{
				SlowDownDefense /= SlowDownResist;
				player.moveSpeed /= 1f+SlowDownDefense;
				player.accRunSpeed /= 1f + SlowDownDefense;
				player.maxRunSpeed /= 1f + SlowDownDefense;

			}
			SlowDownDefense = 0f;
			SlowDownResist = 1f;

			if (player.HeldItem.type == mod.ItemType("Powerjack"))
			{
				player.moveSpeed *= 1.15f;
				player.accRunSpeed *= 1.15f;
				player.maxRunSpeed *= 1.15f;
			}

			skillMananger.PostUpdateRunSpeeds();

		}

		public override void PreUpdate()
		{
			downedHellion = SGAWorld.downedHellion;
			for (int i = 54; i < 58; i++)
			{

				ammoinboxes[i - 54] = player.inventory[i].type;
			}

		}

		public override void PostUpdateBuffs()
		{
			if (ELS)
			{
				if (player.lifeRegen < 0)
					player.lifeRegen = (int)(player.lifeRegen * 1.5f);
			}

			player.statManaMax2 += 20 * Redmanastar;

		}

		public override void PostUpdateEquips()
		{

			//Minecarts-

			if (gunslingerLegendtarget > -1)
			{
				NPC them = Main.npc[gunslingerLegendtarget];
				if (!them.active || them.life < 1 || them.type!= gunslingerLegendtargettype)
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
						minecart.modItem.UpdateAccessory(player,true);
					}
				}
			}

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
					player.AddBuff(BuffID.DryadsWard,2);
					player.maxMinions += 1;
				}
				if (player.ownedLargeGems[5])
				{
					SGAmod.BoostModdedDamage(player,0.05f, 5);
					player.Throwing().thrownDamage += 0.05f; player.meleeDamage += 0.05f; player.magicDamage += 0.05f; player.minionDamage += 0.10f; player.rangedDamage += 0.05f;
					player.Throwing().thrownCrit += 5; player.meleeCrit += 5; player.magicCrit += 5; player.rangedCrit += 5;
					player.lifeRegen += 2;
					player.maxRunSpeed += 0.5f;
				}
			}

			if (player.HasBuff(mod.BuffType("TechnoCurse")))
			{
				techdamage /= 2f;
			}

			if (Noviteset > 0 && electricChargeMax > 0)
			{
				if (Math.Abs(player.velocity.X) > 4f && player.velocity.Y == 0.0 && !player.mount.Active && !player.mount.Cart && electricdelay<1)
				{
					electricCharge += 10;
				}
			}

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
					IdgNPC.AddBuffBypass(player.MinionAttackTargetNPC,mod.BuffType("DigiCurse"),3);
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



			if (EnhancingCharm > 0)
			{
				for (int g = 0; g < Player.MaxBuffs; g += 1)
				{
					if (potionsicknessincreaser > 0)
					{
						if (player.buffType[g] == BuffID.PotionSickness && player.buffTime[g] > 10)
						{
							if (timer % potionsicknessincreaser == 0)
								player.buffTime[g] += 1;
						}
					}
				}

				if (timer % (EnhancingCharm) == 0)
				{
					//longerExpertDebuff
					for (int i = 0; i < Player.MaxBuffs; i += 1)
					{
						if (player.buffType[i] != BuffID.PotionSickness && player.buffType[i] != mod.BuffType("MatrixBuff") && player.buffType[i] != mod.BuffType("DragonsMight"))
						{
							ModBuff buff = ModContent.GetModBuff(player.buffType[i]);
							bool isdebuff = Main.debuff[player.buffType[i]];
							if (player.buffTime[i] > 10 && ((buff != null && ((isdebuff) || !isdebuff)) || buff == null))
							{
								player.buffTime[i] += isdebuff ? -1 : 1;
							}
						}
					}
				}
			}

			if (anticipationLevel > 0)
			{
				if (IdgNPC.bossAlive && !player.HasBuff(mod.BuffType("BossHealingCooldown")) && anticipation < 20 * anticipationLevel)
				{
					anticipation = 100;
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.Green, "Anticipated!", false, false);
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 48, player.width, player.height), Color.Green, "+" + anticipationLevel * 100 + "!", false, false);
					player.AddBuff(mod.BuffType("BossHealingCooldown"), 120 * 60);
					player.statLife += anticipationLevel * 100;
				}
				Item helditem = player.HeldItem;
				if (!player.HeldItem.IsAir)
				{
					if (helditem.Throwing().thrown)
						player.Throwing().thrownDamage += (float)(anticipation / 3000f);
					if (helditem.magic)
						player.magicDamage += (float)(anticipation / 3000f);
					if (helditem.summon)
						player.minionDamage += (float)(anticipation / 3000f);
					if (helditem.ranged)
						player.rangedDamage += (float)(anticipation / 3000f);
					if (helditem.melee)
						player.meleeDamage += (float)(anticipation / 3000f);
				}
			}

			int adderlevel = Math.Max(-1, (int)Math.Pow(anticipationLevel, 0.75));
			int[] ammounts = { 0, 150, 400, 900};
			int adder2;
			if (anticipationLevel > -1)
			adder2 = ammounts[anticipationLevel];
			else
			adder2 = -1;

			anticipation = (int)MathHelper.Clamp(anticipation + (IdgNPC.bossAlive ? (adderlevel) : -1), 0, (100+(adder2))*3);

			if (creeperexplosion > 9700)
			{
				creeperexplosion -= 1;

				if (creeperexplosion==9998)
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Creeper_fuse").WithVolume(.7f).WithPitchVariance(.25f), player.Center);

				int dustIndexsmoke = Dust.NewDust(new Vector2(player.Center.X-4, player.position.Y-6), 8, 12, 31, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dustIndexsmoke].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
				Main.dust[dustIndexsmoke].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
				Main.dust[dustIndexsmoke].noGravity = true;
				dustIndexsmoke = Dust.NewDust(new Vector2(player.Center.X, player.position.Y-6), 8, 12, 6, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dustIndexsmoke].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
				Main.dust[dustIndexsmoke].noGravity = true;


				if (creeperexplosion == 9800)
				{
					for (int xx = 64; xx < 200; xx += 48)
					{

						for (int i = 0; i < 359; i += 36)
						{
							double angles = MathHelper.ToRadians(i+(xx*4));
							float randomx = xx;//Main.rand.NextFloat(54f, 96f);
							Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles));

							int thisone = Projectile.NewProjectile(player.Center.X + (here.X * randomx) - 100, player.Center.Y + (here.Y * randomx) - 100, here.X, here.Y, mod.ProjectileType("CreepersThrowBoom2"), player.statDefense * 8, 0f, player.whoAmI, 0.0f, 0f);
							Main.projectile[thisone].timeLeft = 3;
							Main.projectile[thisone].width = 200;
							Main.projectile[thisone].height = 200;
							Main.projectile[thisone].scale = 0.001f;
							Main.projectile[thisone].netUpdate = true;
							Main.projectile[thisone].timeLeft = 2;
							Main.projectile[thisone].penetrate = 1;
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


			if (floatyeffect>-1)
floatyeffect-=1;

FieryheartBuff = FieryheartBuff-1;
beefield=beefield-1;
beefieldtoggle=beefieldtoggle-1;
Novusset-=1;
Dankset -= 1;

			breathingdelay += 1; breathingdelay%=30;
if (sufficate<0){
if (breathingdelay%5==0)
sufficate=(int)MathHelper.Clamp(sufficate+1,-200,player.breathMax-1);
}else{
if (breathingdelay%29==0) 
sufficate=(int)MathHelper.Clamp(sufficate+1,-200,player.breathMax-1);
}

			if (FireBreath > 0)
			{
				if (timer % 6 == 0)
				{
					float scalevalue = 1f+ (float)FireBreath/2f;
					int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y - 16, (Main.rand.NextFloat(5f, 6f)+(scalevalue-1f)*2f)*(float)player.direction, Main.rand.NextFloat(-0.5f,0.5f), ProjectileID.Flames, (int)((float)player.statDefense* scalevalue), 0f, player.whoAmI, 8f, 0f);
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
								whereisity = Idglib.RaycastDown(minTilePosX + 1, Math.Max(minTilePosY, 0));
								if ((whereisity - minTilePosY > 4 + (player.velocity.Y * 1)) || player.velocity.Y < 0)
								if (Collision.CanHit(player.Center, 32, 32, player.Center + new Vector2(0, -96), 32, 32))
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

				player.gills=false;

			bool isbreathing=true;

//if (SGAmod.Calamity && modcheckdelay){ isbreathing=CalamityAbyss;
//}

if (isbreathing)
player.breath=(int)MathHelper.Clamp(sufficate,-5,player.breathMax-1);
if (sufficate<1){
player.suffocating=true;
}else{
player.endurance+=((float)player.breath/(float)player.breathMax)*0.4f;
player.statDefense+=(int)(((float)player.breath/(float)player.breathMax)*100f);
}

}

if (Havoc>0){

			for (int x = 3; x < 8 + player.extraAccessorySlots; x++)
			{
            if (player.armor[x].modItem!=null){
            var myType = (player.armor[x].modItem).GetType();
            var n = myType.Namespace;
            string asastring = (string)n;
            //int ishavocitem = (asastring.Split('.').Length - 1);
            int ishavocitem = asastring.Length - asastring.Replace("HavocGear.", "").Length;
            if (ishavocitem>0){
            player.statDefense+=(Main.hardMode ? 8 : 3);

			}}}

}


if (NPC.CountNPCS(mod.NPCType("Cirno"))>0 || (SGAWorld.downedCirno==false && Main.hardMode))
player.AddBuff(mod.BuffType("NoFly"), 1, true);

			/*if (pmlcrato>0 || NPC.CountNPCS(mod.NPCType("SPinky"))>9990){player.AddBuff(mod.BuffType("Locked"), 2, true);}*/

			int pmlcrato = NPC.CountNPCS(mod.NPCType("Cratrogeddon"));
			int npctype = NPC.CountNPCS(mod.NPCType("Cratrosity")) + pmlcrato;

			if (npctype>0){
int counter=(player.CountItem(ItemID.WoodenCrate));
counter+=(player.CountItem(ItemID.IronCrate));
counter+=(player.CountItem(ItemID.GoldenCrate));
counter+=(player.CountItem(ItemID.DungeonFishingCrate));
counter+=(player.CountItem(ItemID.JungleFishingCrate));
counter+=(player.CountItem(ItemID.CorruptFishingCrate));
counter+=(player.CountItem(ItemID.HallowedFishingCrate));
counter+=(player.CountItem(ItemID.FloatingIslandFishingCrate));
if (counter>0){
player.AddBuff(mod.BuffType("HeavyCrates"), 2, true);
}}

if (HeavyCrates){
player.runAcceleration/=3f;
}

if (Lockedin){
lockedelay+=1;
if (lockedelay>30)
player.position=new Vector2(Math.Min(Math.Max(player.position.X,Locked.X),Locked.X+Locked.Y),player.position.Y);
player.position=new Vector2(player.position.X,player.position.Y);
}else{
lockedelay=0;
}

if (NoFly){
player.wingTimeMax = player.wingTimeMax/10;
}

if (CirnoWings==true){
player.buffImmune[BuffID.Chilled]=true;
player.buffImmune[BuffID.Frozen]=true;
player.buffImmune[BuffID.Frostburn]=true;
}


int losingmoney=MoneyMismanagement==true ? 2 : (Microtransactions==true ? 1 : 0);
if (losingmoney>0){
Microtransactionsdelay+=1;
if (Microtransactionsdelay%30==0){
int taketype=3;
int [] types = {ItemID.CopperCoin,ItemID.SilverCoin,ItemID.GoldCoin,ItemID.PlatinumCoin};
int copper=player.CountItem(ItemID.CopperCoin);
int silver=player.CountItem(ItemID.SilverCoin);
int gold=player.CountItem(ItemID.GoldCoin);
int plat=player.CountItem(ItemID.PlatinumCoin);
taketype = plat>0 ? 3 : (gold>0 ? 2 : (silver>0 ? 1 : 0));
player.ConsumeItem(types[taketype]);
if (losingmoney>1){
//player.Hurt(PlayerDeathReason damageSource, int Damage, int hitDirection, bool pvp = false, bool quiet = false, bool Crit = false, int cooldownCounter = -1)
player.statLife-=taketype*5;

if (player.statLife<1){player.KillMe(PlayerDeathReason.ByCustomReason(player.name + (Main.rand.Next(0,100)>66 ? " Disgraced Gaben..." : (Main.rand.Next(0,100)>50 ? " couldn't stop spending money" : " couldn't resist the sale"))), 111111, 0, false);}

}
}}


if (beefield>3){
beefieldcounter=beefieldcounter+1;
if (player.ownedProjectileCounts[181] < 5 && beefieldcounter>60)
{
beefieldcounter=0;
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
			if (beeflag==true){
int prog=Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, ProjectileID.Bee, (int)(player.GetWeaponDamage(player.armor[x])), (float)player.GetWeaponKnockback(player.armor[x], player.armor[x].knockBack)*0.01f, player.whoAmI);
SGAprojectile modeproj=Main.projectile[prog].GetGlobalProjectile<SGAprojectile>();
Main.projectile[prog].penetrate=-1;
modeproj.enhancedbees=true;
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
				player.statDefense += Buffscounter*2;

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
							if (projectileslunarslime[i]==null || !projectileslunarslime[i].active)
							{
								int prog = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("LunarSlimeProjectile"), (int)player.GetWeaponDamage(player.armor[x]), (float)player.GetWeaponKnockback(player.armor[x], player.armor[x].knockBack) * 0.01f, player.whoAmI, (float)i);
								SGAprojectile modeproj = Main.projectile[prog].GetGlobalProjectile<SGAprojectile>();
								//Main.projectile[prog].netUpdate = true;
								projectileslunarslime[i]= Main.projectile[prog];
							}
						}
					}
				}


			}		
			
			if (player.ownedProjectileCounts[mod.ProjectileType("TimeEffect")] < 1 && MatrixBuffp)
			{
				int prog = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("TimeEffect"), 1, 0, player.whoAmI);
			}

			if (OmegaSigil)
			{
				if (player.statLife > player.statLifeMax2 - 1)
				{
					for (int i = 0; i < player.GetModPlayer<SGAPlayer>().apocalypticalChance.Length; i += 1)
						player.GetModPlayer<SGAPlayer>().apocalypticalChance[i] += 3.0;
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


					if (player.ownedProjectileCounts[mod.ProjectileType("CapShieldToss")] < 1 && player.HeldItem.modItem!=null)
				{

					Dictionary<int, int> shieldtypes = new Dictionary<int, int>();
					shieldtypes.Add(mod.ItemType("CapShield"), mod.ProjectileType("CapShieldProj"));
					shieldtypes.Add(mod.ItemType("CorrodedShield"), mod.ProjectileType("CorrodedShieldProj"));
					int projtype=-1;
					if (shieldtypes.ContainsKey(player.HeldItem.type))
					{
						shieldtypes.TryGetValue(player.HeldItem.type, out projtype);
						if (projtype > 0)
						{
							if (player.ownedProjectileCounts[projtype] < 1)
								Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, projtype, player.HeldItem.damage, player.HeldItem.knockBack, player.whoAmI);
						}
					}
				}
			}


			if (Main.netMode != NetmodeID.Server)
			{
				if (CooldownStacks.Count > 0)
				{
					for (int stackindex = 0; stackindex < CooldownStacks.Count; stackindex += 1)
					{
						ActionCooldownStack stack = CooldownStacks[stackindex];
						stack.timeleft -= 1;
						stack.timerup += 1;
						if (stack.timeleft < 1)
							CooldownStacks.RemoveAt(stackindex);
					}
				}
				activestacks = CooldownStacks.Count;
			}

			skillMananger.PostUpdateEquips();

			mspeed = player.meleeSpeed;
			modcheckdelay =true;
		}

		public override void PreUpdateMovement()
		{
			if (GeyserInABottleActive && GeyserInABottle)
			{
					if (player.controlJump && !player.jumpAgainCloud)
					{
						List<Projectile> itz = Idglib.Shattershots(player.Center + new Vector2(Main.rand.Next(-15, 15),player.height), player.Center + new Vector2(0, player.height+32), new Vector2(0, 0), ProjectileID.GeyserTrap, 30, 5f, 30, 1, true, 0, false, 400);
						//itz[0].damage = 30;
						itz[0].owner = player.whoAmI;
						itz[0].friendly = true;
						itz[0].hostile = true;
						Main.projectile[itz[0].whoAmI].netUpdate = true;
						if (Main.netMode == 2 && itz[0].whoAmI < 200)
						{
							NetMessage.SendData(27, -1, -1, null, itz[0].whoAmI, 0f, 0f, 0f, 0, 0, 0);
						}

						itz = Idglib.Shattershots(player.Center + new Vector2(Main.rand.Next(-15, 15), player.height), player.Center + new Vector2(0, player.height - 180), new Vector2(0, 0), ProjectileID.GeyserTrap, 30, 10f, 30, 1, true, 0, false, 400);
						//itz[0].damage = 30;
						itz[0].owner = player.whoAmI;
						itz[0].friendly = true;
						itz[0].hostile = true;
						Main.projectile[itz[0].whoAmI].netUpdate = true;
						if (Main.netMode == 2 && itz[0].whoAmI < 200)
						{
							NetMessage.SendData(27, -1, -1, null, itz[0].whoAmI, 0f, 0f, 0f, 0, 0, 0);
						}

						GeyserInABottle = false;
						player.velocity.Y = -15;
					}


			}

		}

		public override void ModifyHitByNPC (NPC npc, ref int damage, ref bool crit)
		{
			if (PrimordialSkull)
			damage = damage / 2;
			damage=OnHit(ref damage, ref crit, npc,null);
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref bool crit)
		{
			damage=OnHit(ref damage,ref crit,null,projectile);
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (damageSource.SourceNPCIndex > -1)
			{
				NPC npc = Main.npc[damageSource.SourceNPCIndex];
				if (npc.GetGlobalNPC<SGAnpcs>().NinjaSmoked && Main.rand.Next(0, 100) < 75)
				{
					player.NinjaDodge();
					return false;
				}
			}

			if (damageSource.SourceCustomReason == (player.name + " went Kamikaze, but failed to blow up enemies"))
			return true;

			if (realIFrames > 0)
				return false;

			if (OmegaSigil && player.statLife-damage<1 && Main.rand.Next(100) <= 10)
			{
				damage = 0;
				player.NinjaDodge();
				return false;
			}
			damage = (int)(damage * damagetaken);
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
			if (creeperexplosion<9800)
			if (damageSource.SourcePlayerIndex == player.whoAmI)
			return false;
			}

			if (damageSource.SourceProjectileType == ProjectileID.GeyserTrap && (GeyserInABottleActive))
			return false;

			if (creeperexplosion > 9795)
				return false;

		if (SpaceDiverset)
		{
		if (player.breath>player.breathMax-2){
		player.immune=true;
		player.immuneTime=45;
		damage*=3;

		int lifelost=(int)(((float)damage/(float)player.statLifeMax)*100f);
		sufficate-=(lifelost+5);
		if (sufficate<0)
		sufficate=(int)MathHelper.Clamp(-(lifelost),sufficate,0);

		return false;
		}}

			if (beefield > 0)
			{
				if (Main.rand.Next(0, 10) < 5)
					player.AddBuff(BuffID.Honey, 60 * 5);
			}

			if (BIP)
				player.AddBuff(mod.BuffType("BIPBuff"), 60 * 5);

			return true;
		}

		private void damagecheck(Vector2 where,ref int damage)
		{
			Vector2 itavect = where - player.Center;
			itavect.Normalize();


			if (player.HeldItem != null && player.ownedProjectileCounts[mod.ProjectileType("CapShieldToss")] < 1)
			{
				Dictionary<int,int> shieldtypes = new Dictionary<int,int>();
				shieldtypes.Add(mod.ItemType("CapShield"), mod.ProjectileType("CapShieldProj"));
				shieldtypes.Add(mod.ItemType("CorrodedShield"),mod.ProjectileType("CorrodedShieldProj"));

				if (shieldtypes.ContainsKey(player.HeldItem.type))
				{
					int foundhim = -1;

					int xxxz = 0;
					int thetype;
					shieldtypes.TryGetValue(player.HeldItem.type,out thetype);
					for (xxxz = 0; xxxz < Main.maxProjectiles; xxxz++)
					{
						if (Main.projectile[xxxz].active && Main.projectile[xxxz].type == thetype && Main.projectile[xxxz].owner == player.whoAmI)
						{
							foundhim = xxxz;
							break;

						}
					}
						if (foundhim > -1)
						{
							Vector2 itavect2 = Main.projectile[foundhim].Center - player.Center;
							itavect2.Normalize();
							float ang1 = itavect.ToRotation();
							float ang2 = itavect2.ToRotation();
							float diff = ang1.AngleLerp(ang2,MathHelper.ToRadians(60));

						float len = ((itavect) - (itavect2)).Length();

							if (len < 0.7f)
							{
							float damageval = 0.75f;
							if (thetype == mod.ProjectileType("CapShieldProj"))
								damageval = 0.50f;
								damage = (int)(damage * damageval);
								Main.PlaySound(3, (int)player.position.X, (int)player.position.Y, 4, 0.6f, 0.5f);
								return;
							}

						}

				}

			}

		}

		public bool ChainBolt()
		{
			WeightedRandom<int> rando = new WeightedRandom<int>();

			if (ConsumeElectricCharge(750, 120))
			{

				for (int i = 0; i < Main.maxNPCs; i += 1)
				{
					if (Main.npc[i].active && !Main.npc[i].townNPC && !Main.npc[i].friendly)
					{
						if (Main.npc[i].CanBeChasedBy() && !Main.npc[i].dontTakeDamage)
						{
							float dist = Main.npc[i].Distance(player.Center);
							if (dist < 250)
							{
								rando.Add(i, 250.00 - (double)dist);
							}
						}
					}
				}

				if (rando.elements.Count > 0)
				{
					NPC luckyguy = Main.npc[rando.Get()];

					Vector2 Speed = (luckyguy.Center - player.Center);
					Speed.Normalize(); Speed *= 2f;
					int prog = Projectile.NewProjectile(player.Center.X, player.Center.Y, Speed.X, Speed.Y, mod.ProjectileType("CBreakerBolt"), 30+((int)((float)(player.statDefense * techdamage))), 3f, player.whoAmI, 3);
					IdgProjectile.Sync(prog);
					Main.PlaySound(SoundID.Item93, player.Center);
					return true;
				}
			}



			return false;
		}

		private int OnHit(ref int damage,ref bool crit,NPC npc, Projectile projectile)
		{
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
					if (npc.type==NPCID.SkeletronHand)
						player.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("HotBarCurse").Type, 60 * 20);
					if (npc.type==NPCID.SkeletronHand)
						player.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("ItemCurse").Type, 60 * 25);
				}
			}
			if (NPC.CountNPCS(mod.NPCType("Murk")) > 0 && Main.hardMode && Main.expertMode)
			{
				player.AddBuff(mod.BuffType("MurkyDepths"), damage * 5);
			}
			if (NPC.CountNPCS(mod.NPCType("TPD")) > 0 && Main.rand.Next(0,10)<(Main.expertMode ? 6 : 3))
			{
				player.AddBuff(BuffID.Electrified, 20+(damage * 2));
			}

			if (Noviteset > 2)
			ChainBolt();

			if (CirnoWings)
		{
				if (npc != null)
					if (npc.coldDamage)
						damage = (int)(damage*0.80);
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

				damagecheck(projectile.Center-projectile.velocity,ref damage);
			}
			if (npc != null)
			{
				damagecheck(npc.Center, ref damage);
			}

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

		if (SpaceDiverset)
		{
		int lifelost=(int)(((float)damage/(float)player.statLifeMax)*150f);
		sufficate-=lifelost;
		if (sufficate<0)
		sufficate=(int)MathHelper.Clamp(-(lifelost+5),sufficate,0);
		}

			if (player.HeldItem.type==mod.ItemType("Powerjack"))
			{
			damage=(int)(damage*1.20);
			}

			if (npc!=null){
				if (npc.type==NPCID.BlazingWheel && npc.life==88){
				SGAnpcs nyx=npc.GetGlobalNPC<SGAnpcs>();
				damage=(int)(damage*2.0);
			}}

			skillMananger.OnPlayerDamage(ref damage,ref crit,npc,projectile);


			return damage;
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (Main.netMode == 0)
			{
				/*if (SGAmod.SkillTestKey.JustPressed)
				{
					SGAmod.SkillUIActive = !SGAmod.SkillUIActive;
					return;
				}*/
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
					Main.PlaySound(17, -1,-1, 0, 1f, Walkmode ? -0.25f : 0.35f);

				}
			}
			if (SGAmod.GunslingerLegendHotkey.JustPressed)
			{
				if (gunslingerLegend && CooldownStacks.Count<MaxCooldownStacks)
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
							Main.PlaySound(SoundID.Item91, player.Center);
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
				if (ninjaSash>1 && AddCooldownStack(60*60))
				{
					Main.PlaySound(SoundID.Item39, player.Center);
					Vector2 tomouse = Main.MouseWorld - player.Center;
					tomouse=tomouse.SafeNormalize(Vector2.Zero);
					tomouse *= 16f;
					int thisoned = Projectile.NewProjectile(player.Center.X,player.Center.Y, tomouse.X, tomouse.Y, mod.ProjectileType("NinjaBombProj"), 0, 0f, Main.myPlayer);
					Main.projectile[thisoned].netUpdate = true;
				}
			}
		}
		public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{

			if (Lockedin)
			{
			int q = 3;
			for (q = 0; q < 2; q++)
				{

					int dust = Dust.NewDust(new Vector2(Main.rand.Next(0,100)<50 ? Locked.X : Locked.X+Locked.Y,drawInfo.position.Y), player.width + 4, player.height + 4, DustID.AncientLight, 0f, player.velocity.Y * 0.4f, 100, default(Color), 3f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].color=Main.hslToRgb((float)(Main.GlobalTime/50)%1, 0.9f, 0.65f);
					//Main.dust[dust].velocity *= 1.8f;
					//Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);
				}
				//r *= 0.1f;
				//g *= 0.2f;
				//b *= 0.7f;
				//fullBright = true;
			}
				if (MassiveBleeding){
					Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
					int dust = Dust.NewDust(new Vector2(drawInfo.position.X,drawInfo.position.Y)+randomcircle*8f, player.width + 4, player.height + 4, 5, player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 30, default(Color), 1.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].color=Main.hslToRgb(0f, 0.5f, 0.35f);
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

		public static readonly PlayerLayer WaveBeamArm = new PlayerLayer("SGAmod", "WaveBeamArm", PlayerLayer.Arms, delegate (PlayerDrawInfo drawInfo)
			{
				Player drawPlayer = drawInfo.drawPlayer;
				SGAmod mod = SGAmod.Instance;
				SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();

				//better version, from Qwerty's Mod
				Color color = drawInfo.bodyColor;
				Texture2D texture = mod.GetTexture("Items/Armors/BeamArms");
					int drawX = (int)((drawInfo.position.X+drawPlayer.bodyPosition.X+10) - Main.screenPosition.X);
					int drawY = (int)(((drawPlayer.bodyPosition.Y-4)+drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
					DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0,drawPlayer.bodyFrame.Y,drawPlayer.bodyFrame.Width,drawPlayer.bodyFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width/2,drawPlayer.bodyFrame.Height/2), 1f, (drawPlayer.direction==-1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir>0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
					Main.playerDrawData.Add(data);
			});

		public static readonly PlayerLayer SpaceDiverTank = new PlayerLayer("SGAmod", "SpaceDiverTank", PlayerLayer.BackAcc, delegate (PlayerDrawInfo drawInfo)
			{
				Player drawPlayer = drawInfo.drawPlayer;
				SGAmod mod = SGAmod.Instance;
				SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();

				//better version, from Qwerty's Mod
				Color color = drawInfo.bodyColor;

				Texture2D texture = mod.GetTexture("Items/Armors/SpaceDiverTank");
					int drawX = (int)((drawInfo.position.X+drawPlayer.bodyPosition.X+10) - Main.screenPosition.X);
					int drawY = (int)(((drawPlayer.bodyPosition.Y-4)+drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
					DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0,drawPlayer.bodyFrame.Y,drawPlayer.bodyFrame.Width,drawPlayer.bodyFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width/2,drawPlayer.bodyFrame.Height/2), 1f, (drawPlayer.direction==-1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir>0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
					data.shader = (int)drawPlayer.cWings;
					Main.playerDrawData.Add(data);
			});

		public static readonly PlayerLayer HeadGlowmask = new PlayerLayer("SGAmod", "HeadGlowmask", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();
			Color GlowColor = modply.armorglowcolor[0](drawPlayer,0);

			Color color = (Color.Lerp(drawInfo.bodyColor, GlowColor, drawPlayer.stealth*((float)drawInfo.bodyColor.A/255f)));

			if (drawPlayer.immune && !drawPlayer.immuneNoBlink && drawPlayer.immuneTime > 0)
				color = drawInfo.bodyColor*drawInfo.bodyColor.A;

			if (modply.armorglowmasks[0] != null && !drawPlayer.mount.Active)
			{
				Texture2D texture = ModContent.GetTexture(modply.armorglowmasks[0]);

				int drawX = (int)((drawInfo.position.X + drawPlayer.bodyPosition.X + 10) - Main.screenPosition.X);
				int drawY = (int)(((drawPlayer.bodyPosition.Y - 3) + drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
				DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width, drawPlayer.bodyFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
				data.shader= (int)drawPlayer.dye[0].dye;
				Main.playerDrawData.Add(data);
			}
		});

		public static readonly PlayerLayer ChestplateGlowmask = new PlayerLayer("SGAmod", "ChestplateGlowmask", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();
			Color GlowColor = modply.armorglowcolor[1](drawPlayer,1);

			Color color = (Color.Lerp(drawInfo.bodyColor, GlowColor, drawPlayer.stealth * ((float)drawInfo.bodyColor.A / 255f)));

			if (drawPlayer.immune && !drawPlayer.immuneNoBlink && drawPlayer.immuneTime > 0)
				color = drawInfo.bodyColor * drawInfo.bodyColor.A;

			if (modply.armorglowmasks[1] != null && !drawPlayer.mount.Active)
			{
				Texture2D texture = ModContent.GetTexture(modply.armorglowmasks[1]);

				int drawX = (int)((drawInfo.position.X + drawPlayer.bodyPosition.X + 10) - Main.screenPosition.X);
				int drawY = (int)(((drawPlayer.bodyPosition.Y - 3) + drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
				DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width, drawPlayer.bodyFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
				data.shader = (int)drawPlayer.dye[1].dye;
				Main.playerDrawData.Add(data);
			}
		});

		public static readonly PlayerLayer ArmsGlowmask = new PlayerLayer("SGAmod", "ArmsGlowmask", PlayerLayer.Arms, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();
			Color GlowColor = modply.armorglowcolor[2](drawPlayer,2);

			//better version, from Qwerty's Mod
			Color color = (Color.Lerp(drawInfo.bodyColor, GlowColor, drawPlayer.stealth * ((float)drawInfo.bodyColor.A / 255f)));

			if (drawPlayer.immune && !drawPlayer.immuneNoBlink && drawPlayer.immuneTime > 0)
				color = drawInfo.bodyColor * drawInfo.bodyColor.A;

			if (modply.armorglowmasks[2] != null && !drawPlayer.mount.Active)
			{
				Texture2D texture = ModContent.GetTexture(modply.armorglowmasks[2]);
				int drawX = (int)((drawInfo.position.X + drawPlayer.bodyPosition.X + 10) - Main.screenPosition.X);
				int drawY = (int)(((drawPlayer.bodyPosition.Y - 3) + drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
				DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width, drawPlayer.bodyFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
				data.shader = (int)drawPlayer.dye[1].dye;
				Main.playerDrawData.Add(data);
			}
		});

		public static readonly PlayerLayer LegsGlowmask = new PlayerLayer("SGAmod", "LegsGlowmask", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();
			Color GlowColor = modply.armorglowcolor[3](drawPlayer,3);

			Color color = (Color.Lerp(drawInfo.bodyColor, GlowColor, drawPlayer.stealth * ((float)drawInfo.bodyColor.A / 255f)));

			if (drawPlayer.immune && !drawPlayer.immuneNoBlink && drawPlayer.immuneTime > 0)
				color = drawInfo.bodyColor * drawInfo.bodyColor.A;

			if (modply.armorglowmasks[3] != null && !drawPlayer.mount.Active)
			{
				Texture2D texture = ModContent.GetTexture(modply.armorglowmasks[3]);

				int drawX = (int)((drawInfo.position.X + drawPlayer.bodyPosition.X + 10) - Main.screenPosition.X);
				int drawY = (int)(((drawPlayer.bodyPosition.Y - 3) + drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
				DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.legFrame.Y, drawPlayer.legFrame.Width, drawPlayer.legFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
				data.shader = (int)drawPlayer.dye[2].dye;
				Main.playerDrawData.Add(data);
			}
		});

		public static void drawdigistuff(PlayerDrawInfo drawInfo,bool front)
		{

			Player drawPlayer = drawInfo.drawPlayer;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();


			int bonusattacks = (int)(((float)modply.digiStacks / (float)modply.digiStacksMax) * (float)modply.digiStacksCount);

			if (bonusattacks > 0)
			{
				List<Vector2> whichone = new List<Vector2>();
				for (int i = 0; i < bonusattacks; i += 1)
				{
					float angle = MathHelper.ToRadians(modply.timer + ((((float)i - 1) / (float)bonusattacks) * 360f));
					float scaler = 1f + ((float)Math.Sin(angle) * 0.25f);
					whichone.Add(new Vector2(scaler, angle));
					//Vector2 apos = new Vector2((float)Math.Cos(angle) * 64, (float)Math.Sin(angle) * 24);
				}
				whichone=whichone.OrderBy((x) => x.X).ToList();

				if (whichone.Count > 0)
				{
					for (int a = 0; a < whichone.Count; a += 1)
					{
						Vector2 theplace = whichone[a];
						float scaler = theplace.X;

						if ((scaler >= 1f && front) || (scaler < 1f && !front))
						{
							float angle = theplace.Y;

							Vector2 apos = new Vector2((float)Math.Cos(angle) * 64, (float)Math.Sin(angle) * 12);

							Texture2D texture = Main.itemTexture[ItemID.ManaCrystal];

							int drawX = (int)((drawPlayer.Center.X + apos.X) - Main.screenPosition.X);
							int drawY = (int)((drawPlayer.MountedCenter.Y + apos.Y) - Main.screenPosition.Y);//gravDir 
							DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, Color.White, (float)0, new Vector2(texture.Width / 2, texture.Height / 2), scaler, (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
							//data.shader = (int)drawPlayer.dye[2].dye;
							Main.playerDrawData.Add(data);
						}
					}
				}

			}

		}

		public static readonly PlayerLayer DigiEffect = new PlayerLayer("SGAmod", "DigiEffect", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
		{
			SGAPlayer.drawdigistuff(drawInfo,true);
		});

		public static readonly PlayerLayer DigiEffectBack = new PlayerLayer("SGAmod", "DigiEffect", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
		{
			SGAPlayer.drawdigistuff(drawInfo, false);
		});

		public static readonly PlayerLayer AltWings = new PlayerLayer("SGAmod", "AltWings", PlayerLayer.Wings, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();

			//better version, from Qwerty's Mod
			Color color = drawInfo.bodyColor;

			if (modply.CustomWings == 1)
			{

				float angle = MathHelper.ToRadians(90f + (drawPlayer.velocity.X * 2f));

				int joy = Math.Max(0, modply.JoyrideShake);

				float nalzs = Main.rand.NextFloat(-joy, joy) / 2f;
				float nalzs2 = Main.rand.NextFloat(-joy / 1f, 0);

				Texture2D texture;
				int drawX;
				int drawY;
				Vector2 org;

				float stealth = (0.2f + drawPlayer.stealth * 0.8f) *Math.Max(0.10f,((float)drawInfo.bodyColor.A / 255f));

				for (int i = -10; i < 11; i += 20)
				{
					nalzs = Main.rand.NextFloat(-joy, joy) / 2f;
					nalzs2 = Main.rand.NextFloat(-joy / 1f, 0);
					drawX = (int)((drawPlayer.MountedCenter.X + (drawPlayer.direction * (-8 + i)) + nalzs));
					drawY = (int)((drawPlayer.MountedCenter.Y + nalzs2 - 8f));//gravDir 
					Vector2 whereat2 = (new Vector2(drawX, drawY).RotatedBy(drawPlayer.fullRotation, drawPlayer.MountedCenter));
					color = Lighting.GetColor((int)(whereat2.X / 16f), (int)(whereat2.Y / 16f)) * stealth;
					texture = Main.itemTexture[ItemID.Megashark];
					org = new Vector2(texture.Width * (drawPlayer.direction > 0 ? 0.25f : 0.25f), texture.Height / 2f);
					DrawData data2 = new DrawData(texture, whereat2 - Main.screenPosition, null, color, (float)drawPlayer.fullRotation + angle, org, 0.75f, (drawPlayer.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
					data2.shader = (int)drawPlayer.cWings;
					Main.playerDrawData.Add(data2);
				}

				nalzs = Main.rand.NextFloat(-joy, joy) / 2f;
				nalzs2 = Main.rand.NextFloat(-joy / 1f, 0);

				texture = Main.itemTexture[ItemID.ChainGun];
				drawX = (int)((drawPlayer.MountedCenter.X + (drawPlayer.direction * -8) + nalzs));
				drawY = (int)((drawPlayer.MountedCenter.Y + nalzs2 - 6f));//gravDir 
				Vector2 whereat = (new Vector2(drawX, drawY).RotatedBy(drawPlayer.fullRotation, drawPlayer.MountedCenter));

				color = Lighting.GetColor((int)(whereat.X / 16f), (int)(whereat.Y/16f), drawInfo.bodyColor) * stealth;
				org = new Vector2(texture.Width * (drawPlayer.direction > 0 ? 0.25f : 0.25f), texture.Height / 2f);
				DrawData data = new DrawData(texture, whereat - Main.screenPosition, null, color, (float)drawPlayer.fullRotation + angle, org, 0.75f, (drawPlayer.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
				data.shader = (int)drawPlayer.cWings;
				Main.playerDrawData.Add(data);

			}

		});

		public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
		{
			//drawInfo.
		}
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			//plasmaLeftInClip
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();

			if (sgaplayer.SpaceDiverset && CustomWings<1)
			{
			int wingsLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Wings"));
			int backacclayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("BackAcc"));
			if (SpaceDiverWings < 0.6f)
			layers.RemoveAt(wingsLayer);
			SpaceDiverTank.visible = true;
			layers.Insert(backacclayer, SpaceDiverTank);
			}

			if (sgaplayer.CustomWings>0)
			{
				int wingsLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Wings"));
				//layers.RemoveAt(wingsLayer);
				AltWings.visible = true;
				layers.Insert(wingsLayer+1, AltWings);
			}

			if (player.HeldItem.type==mod.ItemType("WaveBeam"))
			{
			int armLayer2 = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("HandOnAcc"));
			WaveBeamArm.visible = true;
			layers.Insert(armLayer2, WaveBeamArm);
			}

			if (IDGset)
			{
			int armLayer2 = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("MiscEffectsFront"));
			DigiEffect.visible = true;
			layers.Insert(armLayer2, DigiEffect);
				armLayer2 = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("MiscEffectsBack"));
				DigiEffectBack.visible = true;
				layers.Insert(armLayer2, DigiEffectBack);


			}

			string[] stringsz = { "Head", "Body", "Arms", "Legs"};
			PlayerLayer[] thelayer = { HeadGlowmask, ChestplateGlowmask, ArmsGlowmask, LegsGlowmask };

			for (int i = 0; i < 4; i += 1)
			{

				if (sgaplayer.armorglowmasks[i] != null)
				{
					int layer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals(stringsz[i])) + 1;
					thelayer[i].visible = true;
					layers.Insert(layer, thelayer[i]);
				}

			}

		}

		public override void SendCustomBiomes(BinaryWriter writer)
		{
			BitsByte newbim = new BitsByte();
			newbim[0] = DankShrineZone;
			writer.Write(newbim);
		}

		public override void ReceiveCustomBiomes(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();
			DankShrineZone = flags[0];
		}

		public override void UpdateBiomeVisuals()
		{
			//TheProgrammer
			player.ManageSpecialBiomeVisuals("SGAmod:ProgramSky",(SGAmod.ProgramSkyAlpha>0f || NPC.CountNPCS(mod.NPCType("SPinky"))>0) ? true : false, player.Center);
			player.ManageSpecialBiomeVisuals("SGAmod:HellionSky", (SGAmod.HellionSkyalpha > 0f || NPC.CountNPCS(mod.NPCType("Hellion"))+ NPC.CountNPCS(mod.NPCType("HellionFinal")) > 0) ? true : false, player.Center);
			player.ManageSpecialBiomeVisuals("SGAmod:CirnoBlizzard", (SGAWorld.CirnoBlizzard>0) ? true : false, player.Center);
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
			tag["entropycollected"] = entropycollected;
			tag["Drakenshopunlock"] = Drakenshopunlock;

			if (ExpertisePointsFromBosses != null)
			{

				tag["enemyvaluesTotal"] = ExpertisePointsFromBosses.Count;
				for (int i = 0; i < ExpertisePointsFromBosses.Count; i += 1)
				{
					int value = ExpertisePointsFromBosses[i];
					string tagname = "enemyvalues" + ((string)i.ToString());
					tag[tagname] = value;
					string tagname2 = "enemyvaluesPoints" + ((string)i.ToString());
					tag[tagname2] = ExpertisePointsFromBossesPoints[i];
					string tagname3 = "enemyvaluesModded" + ((string)i.ToString());
					tag[tagname3] = ExpertisePointsFromBossesModded[i];
				}

			}

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
			int? resetver2=null;
			resetver = tag.GetInt("resetver");
			if (tag.ContainsKey("nightmareplayer"))
			nightmareplayer = tag.GetBool("nightmareplayer");
			if (tag.ContainsKey("Electicpermboost"))
				Electicpermboost = tag.GetInt("Electicpermboost");

			if (tag.ContainsKey("Drakenshopunlock"))
				Drakenshopunlock = tag.GetBool("Drakenshopunlock");	
			
			if (tag.ContainsKey("entropycollected"))
				entropycollected = tag.GetInt("entropycollected");

			ExpertiseCollected = tag.GetInt("ZZZExpertiseCollectedZZZ");
			int maybeExpertiseCollected = tag.GetInt("ZZZExpertiseCollectedTotalZZZ");
			ExpertiseCollectedTotal = maybeExpertiseCollected;

			if (maybeExpertiseCollected < 1 || (!tag.ContainsKey("resetver")))
			{

				GenerateNewBossList();
			}
			else
			{
				int maxx = tag.GetInt("enemyvaluesTotal");
				if (maxx < 1)
				{
					GenerateNewBossList();

				}
				else
				{
					ExpertisePointsFromBosses = new List<int>();
					ExpertisePointsFromBossesPoints = new List<int>();
					ExpertisePointsFromBossesModded = new List<string>();
					for (int i = 0; i < maxx; i += 1)
					{
						int v1 = tag.GetInt("enemyvalues" + ((string)i.ToString()));
						int v2 = tag.GetInt("enemyvaluesPoints" + ((string)i.ToString()));
						string v3 = tag.GetString("enemyvaluesModded" + ((string)i.ToString()));

						ExpertisePointsFromBosses.Add(v1);
						ExpertisePointsFromBossesPoints.Add(v2);
						ExpertisePointsFromBossesModded.Add(v3);
					}
				}

			}




		}

		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
		{
			if (junk)
			{
				return;
			}
			if (bait.type == mod.ItemType("SharkBait"))
			{
				//hmmmm
			}
			if (DankShrineZone) {

				if (questFish == mod.ItemType("Vinefish") && Main.rand.Next(2) == 0 && DankShrineZone)
				{
					caughtType = mod.ItemType("Vinefish");
				}
				if (questFish == mod.ItemType("Rootfish") && Main.rand.Next(2) == 0 && DankShrineZone)
				{
					caughtType = mod.ItemType("Rootfish");
				}

				int chance = 10 + (player.cratePotion ? 15 : 0) + (int)Math.Min(50, power);
				if (Main.rand.Next(0, 100) < chance)
					caughtType = mod.ItemType("DankCrate");
			}
		}

		public int? FindBossEXP(int npcid,NPC npc)
		{
			int? found = -1;
			int? foundpre = -1;

			int modnpc = 0;
			if (npc != null) {
				if (npc.modNPC != null)
				{
					foundpre = ExpertisePointsFromBossesModded.FindIndex(x => (x == npc.modNPC.GetType().Name));
					//Main.NewText(foundpre);
					//Main.NewText(npc.modNPC.GetType().Name);
					if (foundpre != null && foundpre > -1)
					{
						return foundpre;
					}
				}
					
			}
			


				if (npcid == NPCID.EaterofWorldsHead || npcid == NPCID.EaterofWorldsBody || npcid == NPCID.EaterofWorldsTail)
			{
				found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.EaterofWorldsHead));
				goto gohere;
			}
			if (npcid == NPCID.DD2DarkMageT1 || npcid == NPCID.DD2DarkMageT3)
			{
				found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.DD2DarkMageT1));
				goto gohere;
			}
			if (npcid == NPCID.DD2OgreT2 || npcid == NPCID.DD2OgreT3)
			{
				found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.DD2OgreT2));
				goto gohere;
			}
			if (npcid == NPCID.GoblinSorcerer || npcid == NPCID.GoblinPeon || npcid == NPCID.GoblinThief || npcid == NPCID.GoblinWarrior || npcid == NPCID.GoblinArcher)
			{
				found = ExpertisePointsFromBosses.FindIndex(x => (x == NPCID.GoblinPeon));
				goto gohere;
			}
			goherelook:

			found = ExpertisePointsFromBosses.FindIndex(x => x == npcid);

			gohere:

			return found;

		}

		public void DoExpertiseCheck(NPC npc,bool tempc=false)
		{
			if (tempc == false)
			{
				if (npc == null)
					return;
				if (!npc.active)
					return;
				if (npc.lifeMax < 100)
					return;
			}
			if (ExpertisePointsFromBosses == null)
			{
				Main.NewText("The enemy list was somehow null... HOW?!");
				return;
			}

			if (ExpertisePointsFromBosses.Count<1)
				return;

			int npcid = npc.type;

			int? found = FindBossEXP(npcid, npc);

			if (found != null && found > -1)
			{
				int collected = ExpertisePointsFromBossesPoints[(int)found];
				if (Main.expertMode)
				{
					if (SGAWorld.NightmareHardcore > 0)
						collected = (int)(collected * (SGAWorld.NightmareHardcore == 1 ? 1.25f : 1.40f));
				}
				else
				{
					collected = (int)(collected * 0.80);
				}

				ExpertiseCollected += collected;
				ExpertiseCollectedTotal += collected;

				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.LimeGreen, "+" + collected + " Expertise", false, false);

				ExpertisePointsFromBosses.RemoveAt((int)found);
				ExpertisePointsFromBossesPoints.RemoveAt((int)found);
				ExpertisePointsFromBossesModded.RemoveAt((int)found);

				int? findagain = FindBossEXP(npcid,npc);

				if (findagain == null || findagain < 0)
				{
					if (Main.myPlayer == player.whoAmI)
						Main.NewText("You have gained Expertise! (you now have " + ExpertiseCollected + ")");


				}

			}


		}

		public void addtolist(int value,int s2ndvalue)
		{
			ExpertisePointsFromBosses.Add(value);
			ExpertisePointsFromBossesPoints.Add(s2ndvalue);
			ExpertisePointsFromBossesModded.Add("");
		}
		public void addtolistmodded(string value, int s2ndvalue)
		{
			ExpertisePointsFromBosses.Add(-1);
			ExpertisePointsFromBossesPoints.Add(s2ndvalue);
			ExpertisePointsFromBossesModded.Add(value);
		}


		public void GenerateNewBossList()
		{
			ExpertisePointsFromBosses = new List<int>();
			ExpertisePointsFromBossesPoints = new List<int>();
			ExpertisePointsFromBossesModded = new List<string>();

			//Prehardmode Bosses (+2500 total)

			addtolistmodded("CopperWraith", 100);

			addtolist(NPCID.KingSlime,100);

			addtolist(NPCID.EyeofCthulhu, 100);

			addtolistmodded("CaliburnGuardian", 75);

			for (int i = 0; i < 50; i += 1)
			{
				addtolist(NPCID.EaterofWorldsHead, 3);
			}

			addtolistmodded("CaliburnGuardian", 100);

			addtolist(NPCID.BrainofCthulhu, 150);

			addtolist(NPCID.QueenBee, 150);

			addtolistmodded("SpiderQueen", 250);

			addtolistmodded("CaliburnGuardian", 125);

			addtolist(NPCID.SkeletronHead, 200);

			addtolistmodded("BossFlyMiniboss1", 200);

			addtolistmodded("Murk", 300);

			addtolist(NPCID.WallofFlesh, 500);


			//Hardmode Bosses (+8500 total)

			addtolistmodded("CobaltWraith", 300);
			addtolistmodded("Cirno", 300);
			addtolist(NPCID.TheDestroyer, 300);
			addtolist(NPCID.SkeletronPrime, 300);
			addtolist(NPCID.Spazmatism, 150);
			addtolist(NPCID.Retinazer, 150);//1500
			addtolistmodded("SharkvernHead", 500);
			addtolist(NPCID.Plantera, 600);//2600
			addtolistmodded("Cratrosity", 700);
			addtolist(NPCID.Golem, 500);
			addtolist(NPCID.DD2Betsy, 700);
			addtolist(NPCID.CultistBoss, 500);//5000
			addtolistmodded("TPD", 800);
			addtolistmodded("Harbinger", 700);
			addtolist(NPCID.LunarTowerNebula, 250);
			addtolist(NPCID.LunarTowerSolar, 250);
			addtolist(NPCID.LunarTowerStardust, 250);
			addtolist(NPCID.LunarTowerVortex, 250);
			addtolist(NPCID.MoonLordCore, 1000);//8500

			//Post-moonlord Bosses (+7500 total)

			addtolistmodded("LuminiteWraith", 1500);
			addtolistmodded("SPinky", 1500);
			addtolistmodded("Cratrogeddon", 1500);
			addtolistmodded("Hellion", 3000);

			//Not bosses (+500 total)
			for (int i = 0; i < 75; i += 1)
			{
				addtolist(NPCID.GoblinPeon, 2);
			}

			addtolistmodded("TidalElemental", 75);
			addtolist(NPCID.Tim, 50);
			addtolist(NPCID.DoctorBones, 50);
			addtolist(NPCID.Nymph, 50);
			addtolist(NPCID.TheGroom, 25);
			addtolist(NPCID.TheBride, 25);
			addtolist(NPCID.DD2DarkMageT1, 75);

			//Not bosses: Hardmode (+2600 total)
			for (int i = 0; i < 2; i += 1)//800
			{
				addtolist(NPCID.GoblinSummoner, 50);
				addtolist(NPCID.Mothron, 75);
				addtolist(NPCID.Mimic, 50);
				addtolist(NPCID.MartianSaucerCore, 150);
				addtolist(NPCID.PirateShip, 75);
				addtolist(NPCID.PirateCaptain, 50);
			}
			//500
			addtolist(NPCID.MartianProbe, 75);
			addtolist(NPCID.Medusa, 50);
			addtolist(NPCID.Clown, 50);
			addtolist(NPCID.RuneWizard, 50);
			addtolist(NPCID.RainbowSlime, 50);
			addtolist(NPCID.Moth, 75);
			addtolist(NPCID.DD2OgreT2, 50);
			addtolist(NPCID.IceGolem, 50);
			addtolist(NPCID.SandElemental, 50);

			for (int i = 0; i < 3; i += 1)//1200
			{
				addtolist(NPCID.MourningWood, 50);
				addtolist(NPCID.Pumpking, 100);
				addtolist(NPCID.Everscream, 50);
				addtolist(NPCID.SantaNK1, 75);
				addtolist(NPCID.IceQueen, 125);
			}

			for (int i = 0; i < 100; i += 1)
			{
				//ignore this, it's filler to keep the list from running out
				addtolist(NPCID.CultistArcherWhite, 1);
			}

			//Tally-21600 Expertise

		}


	}

	public class IDGStartBag : StartBag
	{
		private List<Item> items = new List<Item>();

		public override string Texture
		{
			get
			{
				return "ModLoader/StartBag";
			}
		}

		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("IDG's Starting Bag");
			base.Tooltip.SetDefault("Some starting items couldn't fit in your inventory??\n{$CommonItemTooltip.RightClickToOpen}\n'don't mind me just reusing TMODLoader assets lol-IDG'");
		}

		public override void SetDefaults()
		{
			base.item.width = 20;
			base.item.height = 20;
			base.item.rare = 2;

		}

		public override void RightClick(Player player)
		{

			List<int> itemsbonus = new List<int>();

			int[] loot = { ItemID.TsunamiInABottle, ItemID.FartinaJar, ItemID.CloudinaBottle, SGAmod.Instance.ItemType("ThrowerPouch"), ItemID.HermesBoots, ItemID.SailfishBoots, ItemID.GrapplingHook, ItemID.SilverPickaxe, ItemID.TungstenPickaxe, ItemID.MiningHelmet };
			int[] loot2 = { ItemID.ShinePotion, ItemID.BuilderPotion, ItemID.MiningPotion, ItemID.NightOwlPotion };
			for (int zz = 0; zz < 3; zz++)
			{
				itemsbonus.Add(loot[Main.rand.Next(0, loot.Length)]);
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

}