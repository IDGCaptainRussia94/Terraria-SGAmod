using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using System.Threading.Tasks;
using System.Threading;
using SGAmod.NPCs.Hellion;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using AAAAUThrowing;
using SGAmod.NPCs.Cratrosity;
using SGAmod.Buffs;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Items.Weapons;

namespace SGAmod
{
	public partial class SGAnpcs : GlobalNPC
	{

		public void DoTheDraw(NPC npc)
		{
			if (Snapped < 3)
				return;

			if (SnappedDrawing == null && Snapped > 2)
				SnappedDrawing = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, SurfaceFormat.HdrBlendable, DepthFormat.None);

			RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();
			//Main.spriteBatch.End();
			Main.graphics.GraphicsDevice.SetRenderTarget(SnappedDrawing);
			Main.graphics.GraphicsDevice.Clear(Color.Transparent);

			drawonce = true;

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.CreateScale(1f, 1f, 1f));

			Main.instance.DrawNPC(npc.whoAmI, true);

			drawonce = false;

			Texture2D texture = mod.GetTexture("NPCs/TPD");

			//Main.spriteBatch.Draw(texture, npc.Center-Main.screenPosition, null, Color.White, npc.spriteDirection + (npc.ai[0] * 0.4f), new Vector2(16, 16), new Vector2(Main.rand.Next(5, 20) / 4f, Main.rand.Next(5, 20) / 4f), SpriteEffects.None, 0f);

			Main.spriteBatch.End();

			Main.graphics.GraphicsDevice.SetRenderTargets(binds);


		}

		public void NinjaStashSummonProjectile(NPC npc,Player player, ref int damage, ref float knockback, ref bool crit)
        {
			SGAPlayer moddedplayer = player.GetModPlayer<SGAPlayer>();

			int ammo = 0;
			if (player.HasItem(ItemID.Shuriken))
				ammo = ItemID.Shuriken;
			if (player.HasItem(ItemID.ThrowingKnife))
				ammo = ItemID.ThrowingKnife;
			if (player.HasItem(ItemID.PoisonedKnife))
				ammo = ItemID.PoisonedKnife;
			if (player.HasItem(ItemID.FrostDaggerfish))
				ammo = ItemID.FrostDaggerfish;
			if (player.HasItem(ItemID.StarAnise))
				ammo = ItemID.StarAnise;
			if (player.HasItem(ItemID.BoneDagger))
				ammo = ItemID.BoneDagger;
			//if (player.HasItem(ModContent.ItemType<ThrowingStars>()))
			//	ammo = ModContent.ItemType<ThrowingStars>();


			if (ammo > 0)
			{
				Item itemy = new Item();
				itemy.SetDefaults(ammo);
				int shootype = itemy.shoot;

				Vector2 anglez = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, -2000));
				anglez.Normalize();

				float i = Main.rand.NextFloat(90f, 260f);

				int thisoned = Projectile.NewProjectile(npc.Center + (anglez * i), anglez * -16f, shootype, damage, 0f, Main.myPlayer);
				Main.projectile[thisoned].ranged = false;
				Main.projectile[thisoned].thrown = false;


				for (float gg = 2f; gg > 0.25f; gg -= 0.6f)
				{
					int goreIndex = Gore.NewGore(npc.Center + (anglez * i), -anglez * gg, Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex].scale = 1.5f;
				}
				moddedplayer.ninjaStashLimit += (int)(60 / player.Throwing().thrownDamage);
				player.ConsumeItem(ammo);
			}

		}

		public void OnCrit(NPC npc, Projectile projectile, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			SGAPlayer moddedplayer = player.GetModPlayer<SGAPlayer>();

			if (moddedplayer.gunslingerLegendtarget > -1)
			{
				if (npc.whoAmI == moddedplayer.gunslingerLegendtarget)
				{
					damage = (int)((float)damage * 1.50f);
				}
				else
				{
					damage = (int)((float)damage * 0.25f);
				}

			}

			if (crit)
			{
				if (moddedplayer.ninjaStashLimit < 600)
				{
					if (moddedplayer.ninjaSash > 0 && ((item != null && (item.Throwing().thrown || item.thrown)) || (projectile != null && (projectile.Throwing().thrown || projectile.thrown))))
					{
						NinjaStashSummonProjectile(npc, player, ref damage, ref knockback, ref crit);
					}
				}
			}
		}

		public void DoApoco(NPC npc, Projectile projectile, Player player, Item item, ref int damage, ref float knockback, ref bool crit,int bitBoldedEffects=7,bool always=false)
		{
			bool effectSound = (bitBoldedEffects & (1 << 1 - 1)) != 0;
			bool effectText = (bitBoldedEffects & (1 << 2 - 1)) != 0;
			bool effectShockwave = (bitBoldedEffects & (1 << 3 - 1)) != 0;

			SGAPlayer moddedplayer = player.GetModPlayer<SGAPlayer>();
			int chance = -1;
			if (projectile != null)
			{
				if (projectile.melee)
					chance = 0;
				if (projectile.ranged)
					chance = 1;
				if (projectile.magic)
					chance = 2;
				if (projectile.thrown || projectile.Throwing().thrown)
					chance = 3;
			}
			if (item != null)
			{
				if (item.melee)
					chance = 0;
				if (item.ranged)
					chance = 1;
				if (item.magic)
					chance = 2;
				if (item.thrown || item.Throwing().thrown)
					chance = 3;

			}
			if (npc != null && (always || chance > -1))
			{

				double chanceboost = 0;
				if (projectile != null)
                {
					chanceboost += projectile.GetGlobalProjectile<SGAprojectile>().extraApocoChance;
				}

				if (always || Main.rand.Next(0, 100) < (moddedplayer.apocalypticalChance[chance]+chanceboost) && crit)
				{
					if (moddedplayer.HoE && projectile != null)
					{
						float ammount = damage;
						if (moddedplayer.lifestealentropy > 0)
						{
							projectile.vampireHeal((int)((ammount * moddedplayer.apocalypticalStrength)), npc.Center);
							moddedplayer.lifestealentropy -= ammount;
						}
					}

					if (moddedplayer.ninjaSash > 2)
					{
						for (int i = 0; i < Main.maxProjectiles; i += 1)
						{
							Projectile proj = Main.projectile[i];
							if (proj.active && proj.owner == player.whoAmI)
							{
								if (proj.Throwing().thrown || proj.thrown)
									proj.SGAProj().Embue(projectile);

							}

						}

					}

					if (moddedplayer.SybariteGem)
					{
						float mul = moddedplayer.apocalypticalStrength * (((float)damage * 3f) / (float)npc.lifeMax);
						int ammount = (int)((float)npc.value * mul);


						Vector2 pos = new Vector2((int)npc.position.X, (int)npc.position.Y);
						pos += new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height));
						SGAUtils.SpawnCoins(pos, ammount, 10f + Math.Min(3f * mul, 20f));
					}

					if (moddedplayer.dualityshades)
					{
						int ammo = 0;
						for (int i = 0; i < 4; i += 1)
						{
							if (moddedplayer.ammoinboxes[i] > 0)
							{
								int ammox = moddedplayer.ammoinboxes[i];
								Item itemx = new Item();
								itemx.SetDefaults(ammox);
								if (itemx.ammo == AmmoID.Bullet)
								{
									ammo = ammox;
									break;
								}
							}
						}
						if (ammo > 0)
						{
							Item itemy = new Item();
							itemy.SetDefaults(ammo);
							int shootype = itemy.shoot;

							for (int i = 128; i < 260; i += 128)
							{
								Vector2 anglez = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
								anglez.Normalize();

								Main.PlaySound(SoundID.Item, (int)((npc.Center.X) + (anglez.X * i)), (int)((npc.Center.Y) + (anglez.Y * i)), 25, 0.5f, Main.rand.NextFloat(-0.9f, -0.25f));

								int thisoned = Projectile.NewProjectile(npc.Center + (anglez * i), anglez * -16f, shootype, (int)(damage * 1.50f * moddedplayer.apocalypticalStrength), 0f, Main.myPlayer);
								Main.projectile[thisoned].ranged = false;


								for (float gg = 4f; gg > 0.25f; gg -= 0.15f)
								{
									int dustIndex = Dust.NewDust(npc.Center + new Vector2(-8, -8) + (anglez * i), 16, 16, DustID.AncientLight, -anglez.X * gg, -anglez.Y * gg, 150, Color.Purple, 3f);
									Dust dust = Main.dust[dustIndex];
									dust.noGravity = true;
								}

								player.ConsumeItemRespectInfiniteAmmoTypes(ammo);
							}
						}
					}

					if (moddedplayer.RadSuit)
                    {
						IrradiatedExplosion(npc, (int)(damage * 1f * moddedplayer.apocalypticalStrength));

						SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_DarkMageHealImpact, (int)npc.Center.X, (int)npc.Center.Y);
						if (sound != null)
							sound.Pitch += 0.525f;

						int proj;

						if (projectile!=null)
						proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<RadioactivePool>(), (int)(damage * 0.5f * moddedplayer.apocalypticalStrength), projectile.knockBack, projectile.owner);
						else
							proj = Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<RadioactivePool>(), (int)(damage * 0.5f * moddedplayer.apocalypticalStrength), knockback, player.whoAmI);

						Main.projectile[proj].width += 80;
						Main.projectile[proj].height += 80;
						Main.projectile[proj].timeLeft += (int)(30*moddedplayer.apocalypticalStrength);
						Main.projectile[proj].Center -= new Vector2(40, 40);
						Main.projectile[proj].netUpdate = true;
					}

					if (moddedplayer.CalamityRune)
					{
						Main.PlaySound(SoundID.Item45, npc.Center);
						int boom = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("BoulderBlast"), (int)((damage * 2) * moddedplayer.apocalypticalStrength), knockback * 2f, player.whoAmI, 0f, 0f);
						Main.projectile[boom].usesLocalNPCImmunity = true;
						Main.projectile[boom].localNPCHitCooldown = -1;
						Main.projectile[boom].netUpdate = true;
						IdgProjectile.AddOnHitBuff(boom, BuffID.Daybreak, (int)(60f * moddedplayer.apocalypticalStrength));
						IdgProjectile.AddOnHitBuff(boom, mod.BuffType("EverlastingSuffering"), (int)(400f * moddedplayer.apocalypticalStrength));
					}

					damage = (int)(damage * (3f + (moddedplayer.apocalypticalStrength - 1f)));

					if (moddedplayer.magatsuSet && npc.HasBuff(ModContent.BuffType<Watched>()))
					{
						Projectile.NewProjectile(npc.Center,Vector2.Zero,ModContent.ProjectileType<Items.Armors.Magatsu.ExplosionDarkSectorEye>(),0,0);

						SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_WyvernScream, (int)projectile.Center.X, (int)projectile.Center.Y);
						if (sound != null)
							sound.Pitch = 0.925f;

						foreach (NPC enemy in Main.npc.Where(testby => testby.active && !testby.dontTakeDamage && !testby.friendly && testby != npc && (testby.Center-npc.Center).LengthSquared()<400*400))
                        {
							int damazz = Main.DamageVar(damage);
							enemy.StrikeNPC(damazz, 16, -enemy.spriteDirection, true);

							if (Main.netMode != 0)
							{
								NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, damazz, 16f, (float)1, 0, 0, 0);
							}
						}

					}

					if (effectText)
					CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), Color.DarkRed, "Apocalyptical!", true, false);
					if (SGAConfigClient.Instance.EpicApocalypticals)
					{
						if (effectShockwave)
						{
							RippleBoom.MakeShockwave(npc.Center, 8f, 1f, 10f, 60, 1f);
							if (SGAmod.ScreenShake<32)
							SGAmod.AddScreenShake(24f, 1200, player.Center);
						}
						if (effectSound)
							Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/crit_hit").WithVolume(.7f).WithPitchVariance(.25f), npc.Center);
					}


				}
			}

		}

		public void AddDamageStack(int damage,int time)
        {
			damageStacks.Add(new DamageStack(damage,time));
		}
		public void IrradiatedExplosion(NPC npc,int baseDamage)
		{
			if (IrradiatedAmmount > 0)
			{
				int proj = Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<RadioactivePool>(), (npc.boss ? 0 : baseDamage) + IrradiatedAmmount, 0, Main.player.OrderBy(playerxy => playerxy.Distance(npc.Center)).ToArray()[0].whoAmI);
				Main.projectile[proj].ai[1] = 1;
				Main.projectile[proj].timeLeft = 2;
				Main.projectile[proj].netUpdate = true;
				SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_DarkMageSummonSkeleton, (int)npc.Center.X, (int)npc.Center.Y);
				if (sound != null)
					sound.Pitch -= 0.525f;

				if (npc.HasBuff(ModContent.BuffType<RadioDebuff>()))
				npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<RadioDebuff>()));
				IrradiatedAmmount = 0;
				IrradiatedAmmount_ = 0;
			}


		}
		public void LifeSteal(NPC npc, Player player, ref int damage, ref float knockback, ref bool crit)
		{
			if (player != null)
			{
				if (player.GetModPlayer<SGAPlayer>().HasGucciGauntlet())
				{
					if (player.ownedLargeGems[4] && Main.rand.Next(0, (int)(50000f / Math.Max((float)damage, 40f))) == 1)
					{

						Projectile projectile = new Projectile();
						projectile.Center = npc.Center;
						projectile.owner = player.whoAmI;
						projectile.vampireHeal((int)(40), npc.Center);
					}
				}
			}

		}
		public void DoModifies(NPC npc, Player player, Projectile projectile, Item item, ref int sourcedamage, ref float knockback, ref bool crit)
		{
			SGAPlayer moddedplayer = player.GetModPlayer<SGAPlayer>();
			int damage = (int)(sourcedamage * damagemul);

			Projectile held = null;
			if (projectile != null)
			{

				if (crit && moddedplayer.molotovLimit>0 && projectile.Throwing().thrown)
				{
				crit = (Main.rand.Next(10) == 0);
				}

				if (player!=null && player.heldProj>=0)
				held = Main.projectile[player.heldProj];

				if (projectile.trap)
					damage += (int)(sourcedamage * (player.GetModPlayer<SGAPlayer>().TrapDamageMul-1f));
			}

			DoApoco(npc, projectile, player, item, ref damage, ref knockback, ref crit);

			if (moddedplayer != null)
			{
				if (moddedplayer.acidSet.Item1)
				{
					reducedDefense += (npc.poisoned ? 5 : 0) + (npc.venom ? 5 : 0) + (acidburn ? 5 : 0);
				}
			}

			damage += (int)(Math.Min(npc.defense, reducedDefense) / 2);

			if (Gourged)
				damage += npc.defense / 4;
			if (Sodden)
				damage += (int)((float)sourcedamage * 0.33f);

			if (moddedplayer != null)
			{
				if (moddedplayer.PrimordialSkull)
					if (npc.HasBuff(BuffID.OnFire))
						damage += (int)(sourcedamage * 0.25);

				if (npc.HasBuff(BuffID.Midas))
					damage += (int)(sourcedamage * 0.15f);
			}

			if (item != null)
            {
				if (item.pick + item.axe + item.hammer > 0)
				{
					if (player.HasBuff(ModContent.BuffType<TooltimePotionBuff>()))
					{
						knockback += 50f;
					}
				}
            }

			if (projectile != null)
			{

				SGAprojectile modeproj = projectile.GetGlobalProjectile<SGAprojectile>();
				Player P = Main.player[projectile.owner];
				bool trapdamage = false;
				if (projectile.trap)
					trapdamage = true;

				if (trapdamage)
				{
					float totaldamage = 0f;
					//damage += (int)((npc.defense * moddedplayer.TrapDamageAP) / 2.00);
					totaldamage += moddedplayer.TrapDamageAP;
					if (moddedplayer.JaggedWoodenSpike)
					{
						totaldamage += 0.4f;
						//damage += (int)((npc.defense*0.4)/2.00);
					}
					if (moddedplayer.JuryRiggedSpikeBuckler)
					{
						//damage += (int)(damage * 0.1);
						totaldamage += 0.1f;
						//damage += (int)((npc.defense * 0.1) / 2.00);
					}
					totaldamage = Math.Min(totaldamage, 1f);
					if (moddedplayer.GoldenCog)
					{
						npc.life = npc.life - (int)(damage * 0.10);
						if (Main.netMode == 2)
							NetMessage.SendData(23, -1, -1, null, npc.whoAmI, 0f, 0f, 0f, 0, 0, 0);
					}
					damage += (int)((npc.defense * totaldamage) / 2.00);
				}

				if (moddedplayer.beefield > 0 && (projectile.type == ProjectileID.Bee || projectile.type == ProjectileID.GiantBee))
				{
					damage += (int)(Math.Min(moddedplayer.beedamagemul, 10f + (moddedplayer.beedamagemul / 50f)) * 1.5f);
				}

				if (modeproj.myplayer != null)
					P = modeproj.myplayer;

				if (P != null)
				{
					if (moddedplayer.CirnoWings == true && projectile.coldDamage)
					{
						damage += (int)((float)sourcedamage * 0.20f);
					}
				}

			}

			if (moddedplayer.MisterCreeperset && item != null)
			{
				if (item.shoot < 1 && item.melee)
				{
					if (player.velocity.Y > 1)
					{
						crit = true;
					}
				}
			}

			if (moddedplayer.Blazewyrmset)
			{
				if (npc.HasBuff(mod.BuffType("ThermalBlaze")) && item.melee)
				{
					damage += (int)(sourcedamage * 0.20f);
				}
			}

			if (moddedplayer.alkalescentHeart)
			{
				damage += (int)(sourcedamage * (0f+(npc.HasBuff(ModContent.BuffType<AcidBurn>()) ? 0.15f : (npc.HasBuff(BuffID.Venom) ? 0.10f : (npc.HasBuff(BuffID.Poisoned) ? 0.05f : 0)))));
			}

			SGAPlayer sgaply = player.SGAPly();

			if (sgaply.snakeEyes.Item1)
            {
				bool falsecrit = crit;
				if (!crit && Main.rand.Next(100) == 0)
				{
					CombatText.NewText(npc.Hitbox,Color.Red,"False Crit",false,false);
					falsecrit = true;
				}
				sgaply.snakeEyes.Item2 = falsecrit ? 0 : Math.Min(sgaply.snakeEyes.Item2 + 1, 100);
				damage += (int)(sourcedamage * (0f + (sgaply.snakeEyes.Item2 / 100f)));
            }

			if ((Main.netMode < 1 || SGAmod.SkillRun > 1) && SGAmod.SkillRun > 0)
			{
				if (item != null)
					sgaply.skillMananger.OnEnemyDamage(ref damage, ref crit, ref knockback, item, null);
				if (projectile != null)
					sgaply.skillMananger.OnEnemyDamage(ref damage, ref crit, ref knockback, null, projectile);
			}

			if (petrified)
			{
				if (player != null && (item?.pick > 0 || (projectile != null && player.heldProj >= 0 && player.heldProj == projectile.whoAmI && player.HeldItem.pick > 0)))
				{
					damage = (int)(damage * 3f);
					crit = true;
					Main.PlaySound(SoundID.Tink, (int)npc.Center.X, (int)npc.Center.Y, 0, 1, 0.25f);
				}
				else
				{
					damage = (int)(damage * 0.25f);
				}
			}


			LifeSteal(npc, player, ref damage, ref knockback, ref crit);
			OnCrit(npc, projectile, player, item, ref damage, ref knockback, ref crit);

			sourcedamage = damage;

		}

		private void OnHit(NPC npc, Player player, int damage, float knockback, bool crit, Item item, Projectile projectile, bool isproj = false)
		{
			SGAPlayer moddedplayer = player.GetModPlayer<SGAPlayer>();

			if (moddedplayer.HasGucciGauntlet())
			{
				if (player.ownedLargeGems[0] && Main.rand.Next(0, 10) == 0)
					npc.AddBuff(BuffID.ShadowFlame, 60 * 15);

			}

			if (moddedplayer.flaskBuff != default)
			{
				if (Main.rand.Next(0, moddedplayer.flaskBuff.Chance) == 0)
					npc.AddBuff(moddedplayer.flaskBuff.Debuff, moddedplayer.flaskBuff.Period);
			}

			if (item != null && npc.life - damage < 1 && npc.lifeMax > 50)
			{
				if (item.type == mod.ItemType("Powerjack"))
				{
					player.HealEffect(25, false);
					player.netLife = true;
					player.statLife += 25;
					if (player.statLife > player.statLifeMax2)
					{
						player.statLife = player.statLifeMax2;
					}
					NetMessage.SendData(66, -1, -1, null, player.whoAmI, (float)25f, 0f, 0f, 0, 0, 0);
				}
			}

			if (isproj)
			{
				if (projectile.minion && moddedplayer.IDGset)
				{
					if (npc.immune[projectile.owner] > 0)
					{
						npc.immune[projectile.owner] = ((int)(npc.immune[projectile.owner] * 0.75f));

					}

					moddedplayer.digiStacks = Math.Min(moddedplayer.digiStacksMax, moddedplayer.digiStacks + (int)Math.Max((float)projectile.damage, (float)damage));
				}

				if (projectile.type == ProjectileID.CultistBossLightningOrbArc)
				{
					immunitetolightning = projectile.localNPCHitCooldown;
				}

				bool trapdamage = false;
				if (projectile != null && projectile.trap)
					trapdamage = true;


				if (trapdamage)
				{
					if (moddedplayer.JaggedWoodenSpike)
					{
						if (Main.rand.Next(0, 100) < 15)
							npc.AddBuff(mod.BuffType("MassiveBleeding"), 60 * 5);
					}
				}

				if (moddedplayer.Mangroveset && player.ownedProjectileCounts[mod.ProjectileType("MangroveOrb")] < 4)
				{
					if (crit && projectile.thrown)
					{
						player.AddBuff(BuffID.DryadsWard, 60 * 5);

						List<Projectile> itz = Idglib.Shattershots(player.Center, player.Center + (player.Center - npc.Center), new Vector2(0, 0), mod.ProjectileType("MangroveOrb"), damage, 8f, 120, 2, false, 0, false, 400);
						//itz[0].damage = 30;
						itz[0].owner = player.whoAmI; itz[0].friendly = true; itz[0].hostile = false;
						itz[1].owner = player.whoAmI; itz[1].friendly = true; itz[1].hostile = false;
						Main.projectile[itz[0].whoAmI].netUpdate = true;
						Main.projectile[itz[1].whoAmI].netUpdate = true;
						if (Main.netMode == 2 && itz[0].whoAmI < 200)
						{
							NetMessage.SendData(27, -1, -1, null, itz[0].whoAmI, 0f, 0f, 0f, 0, 0, 0);
						}

					}

				}

			}

			SGAWorld.overalldamagedone = ((int)damage) + SGAWorld.overalldamagedone;
			if (projectile != null)
			{
				if (moddedplayer.FieryheartBuff > 0 && projectile.owner == player.whoAmI && projectile.friendly)
				{
					if (!npc.buffImmune[BuffID.Daybreak] || moddedplayer.FieryheartBuff > 15)
					IdgNPC.AddBuffBypass(npc.whoAmI,189, 1 * (20+(int)(player.SGAPly().ExpertiseCollectedTotal/250f)));
				}
				if ((moddedplayer.CirnoWings) && projectile.owner == player.whoAmI)
				{
					if (isproj && (projectile.magic == true && Main.rand.Next(0,20)==0) || projectile.coldDamage)
						npc.AddBuff(BuffID.Frostburn, 4 * 60);
				}
				if ((moddedplayer.glacialStone) && projectile.owner == player.whoAmI && projectile.melee)
				{
					if (isproj && projectile.melee == true)
						npc.AddBuff(BuffID.Frostburn, Main.rand.Next(1,6) * 60);
				}
			}

			if (moddedplayer.Redmanastar > 0)
			{
				if (isproj && projectile.magic == true)
				{
					int[] buffids = { BuffID.OnFire, mod.BuffType("ThermalBlaze"), BuffID.Daybreak };
					if (projectile != null && Main.rand.Next(0, 100) < 5)
						npc.AddBuff(buffids[moddedplayer.Redmanastar - 1], 4 * 60);
					if (projectile != null && Main.rand.Next(0, 100) < 1)
						npc.buffImmune[buffids[moddedplayer.Redmanastar - 1]] = false;
				}
			}

			if (moddedplayer.SerratedTooth == true)
			{
				if (damage > npc.defense * 5)
					npc.AddBuff(mod.BuffType("MassiveBleeding"), Math.Min((int)(1f + ((float)damage - (float)npc.defense * 5f) * 0.02f) * 60, 60 * 5));
			}

			if (moddedplayer.Blazewyrmset)
			{
				if (crit && ((item != null && item.melee && item.pick + item.axe + item.hammer < 1)) || (projectile != null && projectile.melee && (player.heldProj == projectile.whoAmI || (projectile.modProjectile != null && projectile.modProjectile is IShieldBashProjectile))))
				{
					if (player.SGAPly().AddCooldownStack(12 * 60))
					{
						Main.PlaySound(SoundID.Item45, npc.Center);
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("BoulderBlast"), damage * 3, knockback * 3f, player.whoAmI, 0f, 0f);
					}

				}
			}

			if (moddedplayer.alkalescentHeart)
			{
				int[] maxcrit = { player.meleeCrit, player.rangedCrit, player.magicCrit, player.Throwing().thrownCrit };
				Array.Sort(maxcrit);
				Array.Reverse(maxcrit);
				if (crit || (projectile!=null && projectile.minion && Main.rand.Next(0,100)< maxcrit[0]))
				{
					Point point = new Point(0, 0);
					point.X = (!npc.HasBuff(BuffID.Poisoned) ? BuffID.Poisoned : (!npc.HasBuff(BuffID.Venom) ? BuffID.Venom : (!npc.HasBuff(ModContent.BuffType<AcidBurn>()) ? ModContent.BuffType<AcidBurn>() : -1)));
					if (point.X > -1)
					{
						point.Y = (point.X == ModContent.BuffType<AcidBurn>() ? 45 : point.X == BuffID.Venom ? 200 : point.X == BuffID.Poisoned ? 300 : 0);
						npc.AddBuff(point.X, point.Y);
						//if (Main.rand.Next(0, 10) == 0)
						//	player.AddBuff(point.X, point.Y/2);
					}

				}

			}

			bool hasabuff = false;

			if (npc.HasBuff(BuffID.OnFire))
				hasabuff = true;
			for (int i = 0; i < SGAmod.otherimmunes.Length; i++)
			{
				if (npc.HasBuff(SGAmod.otherimmunes[i]))
				{
					hasabuff = true;
				}
			}
			

			if (DosedInGas && hasabuff)
			{
				Combusted = 60 * 6;
				int buff = npc.FindBuffIndex(mod.BuffType("DosedInGas"));

				if ((isproj && Main.player[projectile.owner].GetModPlayer<SGAPlayer>().MVMBoost) || (item != null && player.GetModPlayer<SGAPlayer>().MVMBoost))
				{
					int prog = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Vector2.Zero.X, Vector2.Zero.Y, ProjectileID.GrenadeIII, 350, 5f, player.whoAmI);
					Main.projectile[prog].thrown = false; Main.projectile[prog].ranged = false; Main.projectile[prog].timeLeft = 2; Main.projectile[prog].netUpdate = true;
					IdgProjectile.Sync(prog);

				}


				if (buff > -1)
				{
					npc.DelBuff(buff);
					IdgNPC.AddBuffBypass(npc.whoAmI, BuffID.OnFire, 60 * 10);
				}

			}

			if (player != null && player.SGAPly().toxicity>0 && npc.HasBuff(BuffID.Stinky))
            {
				if (Main.rand.Next(0,50) == 0)
				SGAPlayer.SwearExplosion(npc.Center,player,(int)(damage*0.5f));


			}

			/*if (PinkyMinion>0)
            {
				if ((npc.life-damage)<1 && ((projectile.penetrate<5 && projectile.penetrate >= 0) || projectile == null))
				{
					NPC[] findnpc = SGAUtils.ClosestEnemies(npc.Center, 1500, checkWalls: false, checkCanChase: false)?.ToArray();
					findnpc = findnpc != null ? findnpc.Where(testby => testby.type == ModContent.NPCType<NPCs.SPinkyTrue>()).ToArray() : null;

					if (findnpc != null && findnpc.Count()>0 && findnpc[0].type == ModContent.NPCType<NPCs.SPinkyTrue>())
					Projectile.NewProjectile(npc.Center, Vector2.Normalize(findnpc[0].Center - npc.Center) * 10f, ModContent.ProjectileType<NPCs.PinkyMinionKilledProj>(), 500, 0);
				}
			}*/
		}
	}


}