using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using SGAmod.Projectiles;
using Idglibrary;
using System.Linq;
using AAAAUThrowing;

namespace SGAmod.Items.Weapons.Shields
{

	public class CorrodedShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corroded Shield");
			Tooltip.SetDefault("'A treasure belonging to a former adventurer you'd rather not use but it looks useful'\nAllows you to block 25% of damage from the source by pointing the shield in the general direction" + "\nBlock at the last second to 'Just Block', taking no damage\n"
				+Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 3 seconds each")+
				"\nAttack with the shield to bash-dash, gaining IFrames and hit enemies are Acid Burned\nCan only hit 5 targets, bash-dash ends prematurally after the 5th\nCan be held out like a torch and used normally by holding shift");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 25;
			item.crit = 15;
			item.melee = true;
			item.width = 54;
			item.height = 32;
			item.useTime = 60;
			item.useAnimation = 60;
			//item.reuseDelay = 120;
			item.useStyle = 1;
			item.knockBack = 5;
			item.noUseGraphic = true;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item7;
			item.shoot = mod.ProjectileType("CorrodedShieldProjDash");
			item.shootSpeed = 10f;
			item.useTurn = false;
			item.autoReuse = false;
			item.expert = true;
			item.noMelee = true;
		}

		public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
		{
			glowstick = true;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < 1;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return true;
		}


	}

	public class CorrodedShieldProj : ModProjectile, IDrawAdditive
	{
		public int blocktimer = 1;
		public virtual float BlockAngle => 0.75f;
		public virtual float BlockDamage=> 0.25f;
		public virtual bool Blocking => true;
		public Player player => Main.player[projectile.owner];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CorrodedShieldProj");
		}

		public virtual void JustBlock(int blocktime,Vector2 where, ref int damage, int damageSourceIndex)
        {


        }

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.timeLeft = 10;
			projectile.hostile = false;
			projectile.penetrate = 10;
			projectile.light = 0.5f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.melee = true;
			projectile.tileCollide = false;
			drawHeldProjInFrontOfHeldItemAndArms = true;
		}

		public override bool CanDamage()
		{
			return false;
		}

        public override bool PreAI()
        {
			blocktimer += 1;
			return true;
        }

        public override void AI()
		{
			blocktimer += 1;
			bool heldone = player.HeldItem.type != mod.ItemType("CorrodedShield") && player.HeldItem.type != mod.ItemType("CapShield");
			if (projectile.ai[0] > 0 || (player.HeldItem == null || heldone) || player.dead || player.ownedProjectileCounts[mod.ProjectileType("CapShieldToss")] > 0)
			{
				projectile.Kill();
			}
			else
			{
				if (projectile.timeLeft < 3)
					projectile.timeLeft = 3;
				Vector2 mousePos = Main.MouseWorld;

				if (projectile.owner == Main.myPlayer)
				{
					Vector2 diff = mousePos - player.Center;
					projectile.velocity = diff;
					projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
					projectile.netUpdate = true;
					projectile.Center = mousePos;
				}
				int dir = projectile.direction;
				player.ChangeDir(dir);

				Vector2 direction = (projectile.velocity);
				Vector2 directionmeasure = direction;

				player.heldProj = projectile.whoAmI;

				projectile.velocity.Normalize();

				player.bodyFrame.Y = player.bodyFrame.Height * 3;
				if (directionmeasure.Y - Math.Abs(directionmeasure.X) > 25)
					player.bodyFrame.Y = player.bodyFrame.Height * 4;
				if (directionmeasure.Y + Math.Abs(directionmeasure.X) < -25)
					player.bodyFrame.Y = player.bodyFrame.Height * 2;
				if (directionmeasure.Y + Math.Abs(directionmeasure.X) < -160)
					player.bodyFrame.Y = player.bodyFrame.Height * 5;
				player.direction = (directionmeasure.X > 0).ToDirectionInt();

				projectile.Center = player.Center + (projectile.velocity * 10f);
				projectile.velocity *= 8f;

			}
		}
		protected virtual void DrawAdd()
        {
			bool facingleft = projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.None;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);

			if (blocktimer < 30 && blocktimer > 1)
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), Main.hslToRgb((Main.GlobalTime * 3f) % 1f, 1f, 0.85f)*MathHelper.Clamp((30-blocktimer)/8f, 0f,1f), projectile.velocity.ToRotation() + (facingleft ? 0 : MathHelper.Pi), origin, projectile.scale + 0.25f, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);

		}
		public void DrawAdditive(SpriteBatch spriteBatch)
		{
			DrawAdd();
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			bool facingleft = projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.None;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), drawColor * projectile.Opacity, projectile.velocity.ToRotation() + (facingleft ? 0 : MathHelper.Pi), origin, projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);
			return false;
		}

	}

	public class CorrodedShieldProjDash : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CorrodedShieldProj");
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.timeLeft = 30;
			projectile.hostile = false;
			projectile.penetrate = 5;
			projectile.light = 0.5f;
			projectile.width = 64;
			projectile.height = 64;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("AcidBurn"), (int)(60 * 1.50));
		}

		public override void AI()
		{

			Player player = Main.player[projectile.owner];

			bool heldone= player.HeldItem.type != mod.ItemType("CorrodedShield");
			if (projectile.ai[0] > 0 || (player.HeldItem == null || heldone) || player.dead)
			{
				projectile.Kill();
			}
			else
			{
				if (projectile.ai[1] < 1)
				{
				int dir = projectile.direction;
				player.ChangeDir(dir);
					player.velocity = projectile.velocity;
					player.velocity.Y /= 2f;
					player.immune = true;
					player.immuneTime = 30;
					player.GetModPlayer<SGAPlayer>().realIFrames = 30;

				}
				player.velocity.X = projectile.velocity.X;
				projectile.Center = player.Center;



				projectile.ai[1] += 1;

			}
		}
		public override string Texture
		{
			get { return "SGAmod/Invisible"; }
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
		{
			return false;
		}

	}

	public class CapShield : CorrodedShield
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Captain America's Shield");
			Tooltip.SetDefault("Functions similarly to Corroded Shield, however allowing you to block 50% of damage instead at a larger angle\nPerforming a Just Block grants a fews seconds of Striking Moment\nCharge up to enable a powerful dash!\nThis dash may be cancelled early by unequiping the shield\nAlt Fire lets you throw the shield, which will bounce between nearby enemies\nYou cannot use your shield while it is thrown, gains +1 bounces per 30 defense\n'Stars and Stripes!'");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 150;
			item.crit = 15;
			item.width = 54;
			item.height = 32;
			item.useTime = 70;
			item.melee = true;
			item.useAnimation = 60;
			item.reuseDelay = 80;
			item.useStyle = 1;
			item.knockBack = 5;
			item.noUseGraphic = true;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = 9;
			item.UseSound = SoundID.Item7;
			item.shoot = mod.ProjectileType("CapShieldProjDash");
			item.shootSpeed = 20f;
			item.useTurn = false;
			item.autoReuse = false;
			item.expert = true;
			item.channel = true;
			item.noMelee = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CorrodedShield"), 1);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddRecipeGroup("Fragment", 15);
			recipe.AddIngredient(ItemID.LunarBar, 5);
			recipe.AddIngredient(ItemID.RedDye, 1);
			recipe.AddIngredient(ItemID.SilverDye, 1);
			recipe.AddIngredient(ItemID.BlueDye, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.channel = false;
			}
			else
			{
				item.channel = true;
			}
				return player.ownedProjectileCounts[mod.ProjectileType("CapShieldProjDash")] < 1 && player.ownedProjectileCounts[mod.ProjectileType("CapShieldToss")] < 1;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
			{
				player.itemAnimation /= 3;
				player.itemTime /= 3;
				damage = (int)(damage);
				type = mod.ProjectileType("CapShieldToss");
				int thisoned = Projectile.NewProjectile(position.X, position.Y, speedX*player.Throwing().thrownVelocity, speedY * player.Throwing().thrownVelocity, type, damage, knockBack, Main.myPlayer);
				Main.projectile[thisoned].thrown = true;
				Main.projectile[thisoned].melee = false;
				Main.projectile[thisoned].netUpdate = true;
				IdgProjectile.Sync(thisoned);
				return false;
			}
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null)
			{
				string[] thetext = tt.text.Split(' ');
				string newline = "";
				List<string> valuez = new List<string>();
				foreach (string text2 in thetext)
				{
					valuez.Add(text2 + " ");
				}
				valuez.RemoveAt(1);
				valuez.Insert(1, "Melee/Throwing ");
				foreach (string text3 in valuez)
				{
					newline += text3;
				}
				tt.text = newline;
			}

			tt = tooltips.FirstOrDefault(x => x.Name == "CritChance" && x.mod == "Terraria");
			if (tt != null)
			{
				string[] thetext = tt.text.Split(' ');
				string newline = "";
				List<string> valuez = new List<string>();
				int counter = 0;
				foreach (string text2 in thetext)
				{
					counter += 1;
					if (counter > 1)
						valuez.Add(text2 + " ");
				}
				int thecrit = Main.GlobalTime % 3f >= 1.5f ? Main.LocalPlayer.meleeCrit : Main.LocalPlayer.Throwing().thrownCrit;
				string thecrittype = Main.GlobalTime % 3f >= 1.5f ? "Melee " : "Throwing ";
				valuez.Insert(0, thecrit + "% " + thecrittype);
				foreach (string text3 in valuez)
				{
					newline += text3;
				}
				tt.text = newline;
			}
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			add -= player.meleeDamage;
			add += ((player.Throwing().thrownDamage*1.5f) + player.meleeDamage)/2f;
		}


	}

	public class CapShieldProj : CorrodedShieldProj, IDrawAdditive
	{
		public override float BlockAngle => 0.4f;
		public override float BlockDamage => 0.5f;
		public override void JustBlock(int blocktime, Vector2 where, ref int damage, int damageSourceIndex)
		{
			player.AddBuff(BuffID.ParryDamageBuff, 60 * 3);

		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CapShieldProj");
		}

	}

	public class CapShieldProjDash : CorrodedShieldProjDash
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CapShieldProjDash");
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.timeLeft = 30;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.light = 0.5f;
			projectile.width = 64;
			projectile.height = 64;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}

		public override bool CanDamage()
		{
			return projectile.ai[1]>0;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//target.AddBuff(mod.BuffType("AcidBurn"), (int)(60 * 1.50));
		}

		public override void AI()
		{

			Player player = Main.player[projectile.owner];
			if ((player.HeldItem == null || player.HeldItem.type != mod.ItemType("CapShield")) || player.dead)
			{
				projectile.Kill();
			}
			else
			{

				if (player.channel)
				{
					if (projectile.ai[0] < 150)
					{
						projectile.ai[0] += 1;
					}
					projectile.timeLeft += 1;
					for (float new1 = 0; new1 < 360; new1 = new1 + 360/10f)
					{
						if (projectile.ai[0] * (360f / 150f) >= new1) {
							float angle = new1;
							Vector2 angg = player.velocity.RotatedBy(MathHelper.ToRadians(angle))+(angle+MathHelper.ToRadians(angle)).ToRotationVector2()*10f;
							int DustID2 = Dust.NewDust(new Vector2(player.Center.X-8, player.Center.Y-8), 16, 16, DustID.AncientLight, 0, 0, 20, Color.Silver, 1f);
							Main.dust[DustID2].velocity = new Vector2(angg.X * 0.75f, angg.Y * 0.75f);
							Main.dust[DustID2].noGravity = true;
						}
						//projectile.position -= projectile.velocity;
						projectile.Center = player.Center;
					}
				}
				else
				{
					if (projectile.ai[1] == 0)
					{
						projectile.damage = (int)(projectile.damage * (1f + projectile.ai[0] / 20f));
						player.itemTime += (int)(projectile.ai[0] / 5f);
						player.itemAnimation += (int)(projectile.ai[0] / 5f);
						projectile.timeLeft += (int)(projectile.ai[0] / 3f);
					}
					if (projectile.ai[1] < (projectile.ai[0] / 5f) + 3)
					{
						if (projectile.ai[1] % 2 == 0)
						{
							Vector2 mousePos = Main.MouseWorld;

							if (projectile.owner == Main.myPlayer)
							{
								Vector2 diff = mousePos - player.Center;
								Vector2 velox = projectile.velocity;
								projectile.velocity = diff;
								projectile.velocity.Normalize();
								projectile.velocity *= velox.Length();
								projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
								projectile.netUpdate = true;
							}

							int dir = projectile.direction;
							player.ChangeDir(dir);
							player.velocity = projectile.velocity * (1 + (projectile.ai[0] / 120f));
							//player.velocity.Y = (float)Math.Pow(player.velocity.Y,0.80);
							//player.velocity.Y /= 2f;
							player.immune = true;
							player.immuneTime = 30;
							player.GetModPlayer<SGAPlayer>().realIFrames = 30;
						}
					}
					else
					{
						projectile.velocity.Y *= 0.98f;
					}
					player.velocity.X = projectile.velocity.X*(1f + (projectile.ai[0] / 120f));
					//player.velocity.Y = projectile.velocity.Y;

					for (float jj = 2; jj < 14; jj += 2)
					{
						for (float new1 = -1f; new1 < 2f; new1 = new1 + 2f)
						{
							float angle = 90;
							Vector2 velo = player.velocity;
							velo.Normalize();
							Vector2 angg = velo.RotatedBy(angle * new1);
							int DustID2 = Dust.NewDust(new Vector2(projectile.Center.X-8, projectile.Center.Y-8), 16, 16, DustID.AncientLight, 0, 0, 20, jj< 5 ? Color.White : new1 < 0 ? Color.Red : Color.Blue, 1f+(14f-jj)/14);
							Main.dust[DustID2].velocity = new Vector2(angg.X * jj, angg.Y * jj);
							Main.dust[DustID2].noGravity = true;
						}
					}

					projectile.Center = player.Center+projectile.velocity;



					projectile.ai[1] += 1;

				}

			}
		}
		public override string Texture
		{
			get { return "SGAmod/Invisible"; }
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
		{
			return false;
		}

	}

	public class CapShieldToss : ModProjectile
	{

		List<int> bouncetargets = new List<int>();
		float hittime = 200f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("MERICA!");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 64;
			projectile.height = 64;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.thrown = true;
			projectile.timeLeft = 3;
			projectile.penetrate = 20;
			aiType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = -8;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 20;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.penetrate < 10)
				return false;
			else
				return base.CanHitNPC(target);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.velocity *= -1f;
			hittime = 150f;
			projectile.ai[1] = FindClosestTarget(projectile.Center, new Vector2(0f, 0f));
			if (projectile.ai[0]>30)
			projectile.ai[0] -= 30;
			//IdgNPC.AddBuffBypass(target.whoAmI,mod.BuffType("InfinityWarStormbreaker"),300);
		}

		public int FindClosestTarget(Vector2 loc, Vector2 size)
		{
			float num170 = 1000000;
			NPC num171 = null;

			for (int num1722 = 0; num1722 < Main.maxNPCs; num1722 += 1)
			{
				int num172 = num1722;
				if (Main.npc[num172].active && !Main.npc[num172].friendly && !Main.npc[num172].townNPC && Main.npc[num172].CanBeChasedBy() && projectile.localNPCImmunity[num1722]<1)
				{
					float num173 = Main.npc[num172].position.X + (float)(Main.npc[num172].width / 2);
					float num174 = Main.npc[num172].position.Y + (float)(Main.npc[num172].height / 2);
					float num175 = Math.Abs(loc.X + (float)(size.X / 2) - num173) + Math.Abs(loc.Y + (float)(size.Y / 2) - num174);
					if (Main.npc[num172].active)
					{

						//(Collision.CanHit(new Vector2(loc.X, loc.Y), 1, 1, Main.npc[num172].position, Main.npc[num172].width, Main.npc[num172].height) || block == false)
						if (num175 < num170)
						{
							int result = 0;
							result = bouncetargets.Find(x => x == num172);
							if (result < 1)
							{
								num170 = num175;
								num171 = Main.npc[num172];
							}
						}
					}
				}
			}
			if (num170 > 900)
			{
				projectile.penetrate = 5;
				return -1;
			}

			return num171.whoAmI;

		}

		public override void AI()
		{

			Lighting.AddLight(projectile.Center, Color.Aquamarine.ToVector3() * 0.5f);

			hittime = Math.Max(1f, hittime / 1.5f);
			;
			float dist2 = 24f;

			//Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 8f;
			for (double num315 = 0; num315 < Math.PI + 0.04; num315 = num315 + Math.PI)
			{
				Vector2 thisloc = new Vector2((float)(Math.Cos((projectile.rotation + Math.PI / 2.0) + num315) * dist2), (float)(Math.Sin((projectile.rotation + Math.PI / 2.0) + num315) * dist2));
				int num316 = Dust.NewDust(new Vector2(projectile.position.X - 1, projectile.position.Y) + thisloc, projectile.width, projectile.height, DustID.AncientLight, 0f, 0f, 50, num315 < 0.01 ? Color.Blue : Color.Red, 1.5f);
				Main.dust[num316].noGravity = true;
				Main.dust[num316].velocity = thisloc / 30f;
			}

			projectile.ai[0] = projectile.ai[0] + 1;
			if (projectile.ai[0] == 1)
			{
				projectile.penetrate += (int)((float)Main.player[projectile.owner].statDefense/30f);
				projectile.ai[1] = FindClosestTarget(projectile.Center, new Vector2(0f, 0f));

			}
			projectile.velocity.Y += 0.1f;
			if ((projectile.ai[0] > 90f || (projectile.penetrate<10 && projectile.ai[0]>20)) && !Main.player[projectile.owner].dead)
			{
				Vector2 dist = (Main.player[projectile.owner].Center - projectile.Center);
				Vector2 distnorm = dist; distnorm.Normalize();
				projectile.velocity += distnorm * (5f+ ((float)projectile.timeLeft/40f));
				projectile.velocity /= 1.25f;
				//projectile.Center+=(dist*((float)(projectile.timeLeft-12)/28));
				if (dist.Length() < 80)
					projectile.Kill();

				projectile.timeLeft += 1;
			}
			projectile.timeLeft += 1;


			if (projectile.ai[1] > -1)
			{
				NPC target = Main.npc[(int)projectile.ai[1]];

				if (target != null && projectile.penetrate > 9)
				{
				projectile.Center += (projectile.DirectionTo(target.Center) * (projectile.ai[0] > 8f ? (50f * Main.player[projectile.owner].thrownVelocity)/hittime : 12f));
				}
			}

			projectile.rotation += 0.38f;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Shields/CapShield"; }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Items/Weapons/Shields/CapShield");
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}


	}


}