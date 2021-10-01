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
using CalamityMod.Projectiles.Ranged;
using SGAmod.Buffs;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Items.Weapons.Shields;

namespace SGAmod
{
	public class ActionCooldownStack
	{
		public int timeleft;
		public int timerup;
		public int maxtime;
		public int index;

		public ActionCooldownStack(int timeleft,int index)
		{
			this.timeleft = timeleft;
			this.maxtime = timeleft;
			this.index = index;
			timerup = 0;
		}
	}

	public partial class SGAPlayer : ModPlayer
	{

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
			entropyCollected += ammount;
			while (entropyCollected > Items.EntropyTransmuter.MaxEntropy)
			{
				entropyCollected -= Items.EntropyTransmuter.MaxEntropy;
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

		public void ShieldRecharge()
        {

        }

		public void StartShieldRecharge()
		{
			SoundEffectInstance sound = Main.PlaySound(SoundID.Zombie, (int)player.Center.X, (int)player.Center.Y, 71);
			if (sound != null)
				sound.Pitch += 0.5f;
		}

		public void ShieldDepleted()
        {
			CauseShieldBreak(60 * 7);
		}

		(int, int) shieldAmmounts = (5, 7);

		public bool TakeShieldHit(ref int damage)
		{

			int takenshielddamage = (int)(jellybruSet ? damage*Math.Max(player.manaCost, 0.10f) : damage);

			if (GetEnergyShieldAmmountAndRecharge.Item1 > takenshielddamage)
            {

				if (!player.immune)
				{
					SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 93);
					if (sound != null)
						sound.Pitch = MathHelper.Clamp(-0.8f + ((GetEnergyShieldAmmountAndRecharge.Item1 / (float)GetEnergyShieldAmmountAndRecharge.Item2) * 1.60f),-0.75f,0.80f);

					energyShieldAmmountAndRecharge.Item3 = 60 * (tpdcpu ? shieldAmmounts.Item1 : shieldAmmounts.Item2);
					energyShieldAmmountAndRecharge.Item1 -= takenshielddamage;

					//Main.NewText(takenshielddamage);

					player.immune = true;
					player.immuneTime = 20;
				}

				return true;
			}
			damage -= GetEnergyShieldAmmountAndRecharge.Item1;

			if (GetEnergyShieldAmmountAndRecharge.Item1 > 0)
			{
				ShieldDepleted();
				energyShieldAmmountAndRecharge.Item1 = 0;
			}
			return false;
		}

		public void StackDebuff(int type,int time)
        {
			player.AddBuff(ModContent.BuffType<PlaceHolderDebuff>(), time);
			if (player.FindBuffIndex(ModContent.BuffType<PlaceHolderDebuff>()) >= 0)
            {
				player.buffType[player.FindBuffIndex(ModContent.BuffType<PlaceHolderDebuff>())] = type;
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

		public void RestoreBreath(int ammount, bool texteffect = true)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			Main.PlaySound(SoundID.Drown, (int)player.Center.X, (int)player.Center.Y, 0, 1f, 0.50f);
			player.breath = (int)MathHelper.Clamp(player.breath + ammount, 0, player.breathMax);
			sgaplayer.sufficate = player.breath;
			if (texteffect)
				CombatText.NewText(player.Hitbox, Color.Aqua, "+" + (ammount / 20) + " bubbles");

		}

		public bool AddCooldownStack(int time, int count = 1)
		{
			if (illuminantSet.Item1>4 && Main.rand.Next(4) == 0)
            {
				return true;
            }
			if (CooldownStacks.Count + (count - 1) < MaxCooldownStacks)
			{
				//if (player.HasBuff(mod.BuffType("CondenserBuff")))
				//	time = (int)((float)time * 1.15f);

				time = (int)((float)time * ActionCooldownRate);

				for (int i = 0; i < count; i += 1)
					CooldownStacks.Add(new ActionCooldownStack(time, CooldownStacks.Count));
				return true;
			}
			return false;
			
		}

		public void AddElectricCharge(int ammount)
        {
			electricCharge += ammount;
		}

		public void CauseShieldBreak(int time)
        {
			if (!tpdcpu)
			{
				player.AddBuff(ModContent.BuffType<ShieldBreak>(), time);
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.Aquamarine, "Shield Break!", true, false);
				SoundEffectInstance sound = Main.PlaySound(SoundID.NPCHit, (int)player.Center.X, (int)player.Center.Y, 53);
				if (sound != null)
					sound.Pitch -= 0.5f;

				for (int i = 0; i < 20; i += 1)
				{
					int dust = Dust.NewDust(new Vector2(player.Center.X - 4, player.Center.Y - 8), 8, 16, 269);
					Main.dust[dust].scale = 0.50f;
					Main.dust[dust].noGravity = false;
					Main.dust[dust].velocity = Main.rand.NextVector2Circular(6f, 6f);
				}
			}
		}

		public bool ConsumeElectricCharge(int requiredcharge, int delay, bool damage = false,bool consume = true)
		{
			int newcharge = (int)Math.Max(requiredcharge * electricChargeCost,1);
			if (electricCharge > newcharge)
			{
				if (consume)
				{
					electricdelay = Math.Max(delay * electricChargeReducedDelay, electricdelay);
					electricCharge -= newcharge;
				}
				return true;
			}
			else
			{
				if (damage && ShieldType > 0 && electricCharge > 0)
				{
					electricCharge = 0;
					electricdelay = 30;
					CauseShieldBreak(60 * 5);
				}
			}

			return false;
		}

		public bool HandleFluidDisplacer(int tier)
        {
			if (tidalCharm < 0 && ConsumeElectricCharge(tier, 60* tier, true))
				return true;


			return false;


		}
		public bool ConsumeAmmoClip(bool doConsume = true,int ammoCheck = 1)
        {
			if (ammoLeftInClip >= ammoCheck)
			{
				if (doConsume)
				ammoLeftInClip -= ammoCheck;

				return true;
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
		public bool RefilPlasma(bool checkagain = false)
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
					plasmaLeftInClip = plasmaLeftInClipMax;//Math.Min(plasmaLeftInClip + 1000, plasmaLeftInClipMax);
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.LawnGreen, "Plasma Recharged!", false, false);
					if (plasmaLeftInClip < plasmaLeftInClipMax && checkagain)
					{
						RefilPlasma(true);

					}
					return true;
				}
			}
			return false;
		}

		public static void LimitProjectiles(Player player, int maxprojs, int[] types)
		{

			int projcount = 0;
			for (int a = 0; a < types.Length; a++)
			{
				projcount += player.ownedProjectileCounts[(int)types[a]];
			}

			Projectile removethisone = null;
			int timecheck = 99999999;

			if (projcount > maxprojs)
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile him = Main.projectile[i];
					if (types.Any(x => x == Main.projectile[i].type))
					{
						if (him.active && him.owner == player.whoAmI && him.timeLeft < timecheck)
						{
							removethisone = him;
							timecheck = him.timeLeft;
						}
					}
				}
				if (removethisone != null)
				{
					removethisone.Kill();
				}
			}

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

		public void UpgradeTF2()
		{
			if (!gottf2 && player == Main.LocalPlayer)
			{
				Main.NewText("You have received your TF2 Emblem!", 150, 150, 150);
				player.QuickSpawnItem(mod.ItemType("TF2Emblem"), 1);
				gottf2 = true;
			}
		}

		public int ActionStackOverflow(ActionCooldownStack stack,int stackIndex)
		{
			if (MaxCooldownStacks <= stackIndex && stack.timerup % 2 == 0)
				return 0;

				return 1;
		}

		public void DoCooldownUpdate()
		{
			if (Main.netMode != NetmodeID.Server)//ugh
			{

				if (CooldownStacks.Count > 0)
				{
					for (int stackindex = 0; stackindex < CooldownStacks.Count; stackindex += 1)
					{
						ActionCooldownStack stack = CooldownStacks[stackindex];
						stack.timeleft -= ActionStackOverflow(stack, stackindex);
						stack.timerup += 1;
						if (stack.timeleft < 1)
							CooldownStacks.RemoveAt(stackindex);
					}
				}
				activestacks = CooldownStacks.Count;
			}

		}

		private bool ShieldJustBlock(int blocktime,Projectile shield, Vector2 where, ref int damage, int damageSourceIndex)
        {
			if (blocktime < 30 && AddCooldownStack(60 * 3))
			{
				for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.Pi / 10f)
				{
					Vector2 randomcircle = Main.rand.NextVector2CircularEdge(2f, 4f).RotatedBy(shield.velocity.ToRotation());
					int dust = Dust.NewDust(shield.Center, 0, 0, 27);
					Main.dust[dust].scale = 1.5f;
					Main.dust[dust].velocity = randomcircle * 3f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].shader = GameShaders.Armor.GetShaderFromItemId(ItemID.AcidDye);
				}
				player.GetModPlayer<SGAPlayer>().realIFrames = 30;
				Main.PlaySound(3, (int)player.position.X, (int)player.position.Y, 4, 1f, -0.5f);

				(shield.modProjectile as Items.Weapons.Shields.CorrodedShieldProj).JustBlock(blocktime, where, ref damage, damageSourceIndex);

				if (enchantedShieldPolish)
                {
					player.statMana = Math.Min(player.statMana + damage, player.statManaMax2);
					player.ManaEffect(damage);
				}

				if (damageSourceIndex > 0)
				{
					if (rustedBulwark)
					{
						RustBurn.ApplyRust(Main.npc[damageSourceIndex - 1], (2 + damage) * 20);
					}

					if (diesIraeStone)
					{
						float empty = 5f;
						bool emptyCrit = true;
						Main.npc[damageSourceIndex - 1].SGANPCs().DoApoco(Main.npc[damageSourceIndex - 1], shield, player, null, ref damage, ref empty, ref emptyCrit, 4, true);
					}
				}

				return true;
			}
			return false;

		}

		protected bool ShieldDamageCheck(Vector2 where, ref int damage,int damageSourceIndex)
		{
			Vector2 itavect = where - player.Center;
			itavect.Normalize();


			if (player.SGAPly().heldShield>=0 && player.ownedProjectileCounts[mod.ProjectileType("CapShieldToss")] < 1)
			{
				int heldShield = player.SGAPly().heldShield;

				//if (SGAPlayer.ShieldTypes.ContainsKey(player.HeldItem.type))
				//{
					int foundhim = -1;

					int xxxz = 0;
					int thetype;
					/*SGAPlayer.ShieldTypes.TryGetValue(player.HeldItem.type, out thetype);
					Projectile proj = null;
					for (xxxz = 0; xxxz < Main.maxProjectiles; xxxz++)
					{
						if (Main.projectile[xxxz].active && Main.projectile[xxxz].type == thetype && Main.projectile[xxxz].owner == player.whoAmI)
						{
							foundhim = xxxz;
							proj = Main.projectile[xxxz];
							break;
						}
					}*/
					Projectile proj = Main.projectile[heldShield];
				if (proj.active)
				{
					foreach (Projectile proj2 in Main.projectile.Where(testby => testby.modProjectile != null && testby.modProjectile is CorrodedShieldProj))
					{
						proj = proj2;
						foundhim = heldShield;

						if (foundhim > -1)
						{
							CorrodedShieldProj modShieldProj = proj.modProjectile as CorrodedShieldProj;
							if (modShieldProj == null)
								return false;
							int blocktime = modShieldProj.blocktimer;
							bool blocking = modShieldProj.Blocking;
							if (proj == null || blocktime < 2 || !blocking)
								continue;// return false;



							Vector2 itavect2 = Main.projectile[foundhim].Center - player.Center;
							itavect2.Normalize();
							Vector2 ang1 = Vector2.Normalize(proj.velocity);
							float diff = Vector2.Dot(itavect, ang1);


							if (diff > (proj.modProjectile as CorrodedShieldProj).BlockAnglePublic)
							{
								if (ShieldJustBlock(blocktime, proj, where, ref damage, damageSourceIndex))
									return true;

								float damageval = 1f - modShieldProj.BlockDamagePublic;
								damage = (int)(damage * damageval);

								Main.PlaySound(3, (int)player.position.X, (int)player.position.Y, 4, 0.6f, 0.5f);

								if (!NoHitCharm && !(proj.modProjectile as CorrodedShieldProj).HandleBlock(ref damage, player))
									return true;

								continue;// return false;
							}
						}
					}
				}

			}
			return false;
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
					int prog = Projectile.NewProjectile(player.Center.X, player.Center.Y, Speed.X, Speed.Y, mod.ProjectileType("CBreakerBolt"), 30 + ((int)((float)(player.statDefense * techdamage))), 3f, player.whoAmI, 3);
					IdgProjectile.Sync(prog);
					Main.PlaySound(SoundID.Item93, player.Center);
					return true;
				}
			}



			return false;
		}

		public bool DashBlink()
		{
			if (noModTeleport || maxblink < 1)
				return false;



			int previousdash = player.dash;
			player.dash = 1;

			if (Math.Abs(player.dashTime) > 0 && player.dashDelay < 1 && player.dash > 0 && (player.controlLeft || player.controlRight))
			{
				int bufftime = 0;
				if (player.HasBuff(BuffID.ChaosState))
					bufftime = player.buffTime[player.FindBuffIndex(BuffID.ChaosState)];


				if (bufftime < maxblink && (player.controlUp))
				{
					player.Teleport(player.Center + new Vector2(player.dashTime > 0 ? -8 : 0, -20), 1);
					for (int i = 0; i < 30; i += 1)
					{
						if (Collision.CanHitLine(player.Center, 16, 16, player.Center + new Vector2(Math.Sign(player.dashTime) * 8, 0), 16, 16))
						{
							player.Center += new Vector2(Math.Sign(player.dashTime) * 8, 0);

						}
						else
						{
							player.Center -= new Vector2(Math.Sign(player.dashTime) * 16, 0);
							break;
						}
					}
					player.Teleport(player.Center + new Vector2(player.dashTime > 0 ? -8 : 0, -20), 1);
					player.dashTime = 0;
					player.dashDelay = 5;
					player.AddBuff(BuffID.ChaosState, bufftime + 120);

					return true;
				}

				player.dash = previousdash;

			}
			return false;
		}

		public void ShuffleYourFeetElectricCharge()
		{
			if (Noviteset > 0 && electricChargeMax > 0)
			{
				if (Math.Abs(player.velocity.X) > 4f && player.velocity.Y == 0.0 && !player.mount.Active && !player.mount.Cart)
				{
					electricrechargerate += 10;
				}
			}
		}

		public void CharmingAmuletCode()
		{
			if (EnhancingCharm > 0 || player.manaRegenBuff)
			{
				for (int g = 0; g < Player.MaxBuffs; g += 1)
				{
					if (player.manaRegenBuff && (SGAConfig.Instance.ManaPotionChange || SGAWorld.NightmareHardcore>0))
					{
						if (player.buffType[g] == BuffID.ManaSickness && player.buffTime[g] > 3)
						{
							if (timer % 4 > 0)
								player.buffTime[g] += 1;
						}
					}
					if (EnhancingCharm>0)
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
				}
				if (EnhancingCharm>0)
				{
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
			int[] ammounts = { 0, 150, 400, 900 };
			int adder2;
			if (anticipationLevel > -1)
				adder2 = ammounts[anticipationLevel];
			else
				adder2 = -1;

			anticipation = (int)MathHelper.Clamp(anticipation + (IdgNPC.bossAlive ? (adderlevel) : -1), 0, (100 + (adder2)) * 3);
		}

		public void FlaskEffects(Rectangle rect,Vector2 speed)
		{
			if (flaskBuff != default)
			{
				flaskBuff.FlaskEffect(rect, speed);
			}
		}
	}

}