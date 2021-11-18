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
using Terraria.Graphics.Shaders;
using SGAmod.Buffs;
using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;

namespace SGAmod.Items.Weapons.Shields
{

	public class CorrodedShield : ModItem, IShieldItem
	{
		public Projectile GetShieldProj
		{
			get
			{
				int typez = mod.ProjectileType(Name + "Proj");
				Projectile proj = null;
				if (Main.LocalPlayer.ownedProjectileCounts[typez] > 0)
				{
					Projectile[] proj3 = Main.projectile.Where(testprog => testprog.active && testprog.owner == Main.LocalPlayer.whoAmI && testprog.type == typez).ToArray();
					if (proj3 != null && proj3.Length > 0)
						proj = proj3[0];
				}
				else
				{
					proj = new Projectile();
					proj.SetDefaults(typez);
				}
				return proj;
			}
		}
		public virtual string ShowPercentText
		{
			get
			{
				Projectile proj = GetShieldProj;
				if (proj != null)
				{
					CorrodedShieldProj proj2 = proj.modProjectile as CorrodedShieldProj;
					if (proj2 != null)
					{
						float blockpercent = (proj2.BlockDamagePublic);
						//blockpercent += Main.LocalPlayer.SGAPly().shieldDamageReduce;

						return "Blocks " + (blockpercent) * 100 + "% of damage ";
					}
				}

				return "Blocks unknown % damage ";
			}
		}
		public virtual string DamagePercent => "at a narrow angle";
		public virtual bool CanBlock => true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corroded Shield");
			Tooltip.SetDefault("'A treasure belonging to a former adventurer you'd rather not use but it looks useful'" +
				"\nAttack with the shield to bash-dash, gaining IFrames and hit enemies are Acid Burned\nCan only hit 5 targets, bash-dash ends prematurally after the 5th");
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
			item.expert = false;
			item.noMelee = true;
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			add += player.SGAPly().shieldDamageBoost;
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

		public override int ChoosePrefix(UnifiedRandom rand)
		{
			switch (rand.Next(9))
			{
				case 1:
					return PrefixID.Weak;
				case 2:
					return PrefixID.Frenzying;
				case 3:
					return PrefixID.Damaged;
				case 4:
					return PrefixID.Savage;
				case 5:
					return PrefixID.Furious;
				case 6:
					return PrefixID.Terrible;
				case 7:
					return mod.PrefixType("Busted");
				default:
					return mod.PrefixType("Defensive");
			}
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (CanBlock)
			{
				if (ShowPercentText != "none")
				{
					tooltips.Add(new TooltipLine(mod, "shieldtext", Idglib.ColorText(Color.CornflowerBlue, ShowPercentText + DamagePercent)));
					tooltips.Add(new TooltipLine(mod, "shieldtext", Idglib.ColorText(Color.CornflowerBlue, "Block at the last second to 'Just Block', taking no damage")));
					tooltips.Add(new TooltipLine(mod, "shieldtext", Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 3 seconds each")));
				}
				tooltips.Add(new TooltipLine(mod, "shieldtext", Idglib.ColorText(Color.CornflowerBlue, "Can be held out like a torch and used normally by holding shift")));
			}
		}
	}

	public class CorrodedShieldProj : ModProjectile, IDrawAdditive
	{
		public int Blocktimer => blocktimer-player.SGAPly().shieldBlockTime;
		private int blocktimer = 1;

		protected virtual float BlockAngle => 0.5f;
		protected virtual float BlockDamage => 0.25f;
		protected virtual int BlockPeriod => 30;
		protected virtual float AngleAdjust => 0f;
		protected virtual float BlockAngleAdjust => 0f;
		protected virtual float AlphaFade => 1f;
		protected virtual float HoldingDistance => 10f;

		public virtual float BlockDamagePublic
		{
			get
			{
				float boost = 0;
				if (player != null)
					boost += player.SGAPly().shieldDamageReduce*1f;

				return BlockDamage + boost;
			}
		}
		public virtual float BlockAnglePublic
		{
			get
			{
				return BlockAngle;
			}
		}
		public virtual bool Blocking => true;
		public Player player
		{
			get
			{
				if (projectile.owner >= 255)
					return Main.LocalPlayer;
				return Main.player[projectile.owner];
			}
		}
		public string ItemName => Name.Replace("Proj", "");
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CorrodedShieldProj");
		}

		public virtual void JustBlock(int blocktime, Vector2 where, ref int damage, int damageSourceIndex) { }
		public virtual void WhileHeld(Player player) { }
		public virtual bool HandleBlock(ref int damage, Player player) { return true; }
		public virtual Texture2D MyTexture => ModContent.GetTexture(Texture);

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
			bool heldone = player.HeldItem.type != mod.ItemType(ItemName);
			if (projectile.ai[0] > 0 || ((player.HeldItem == null || heldone) && projectile.timeLeft <= 10) || player.dead || (player.ownedProjectileCounts[mod.ProjectileType("CapShieldToss")] > 0 && GetType() == typeof(CapShieldProj)))
			{
				projectile.Kill();
			}
			else
			{
				SGAPlayer sgaply = player.SGAPly();
				player.SGAPly().heldShield = projectile.whoAmI;
				sgaply.heldShieldReset = 3;

				if (projectile.timeLeft < 3)
					projectile.timeLeft = 3;
				Vector2 mousePos = Main.MouseWorld;

				if (projectile.owner == Main.myPlayer)
				{
					Vector2 diff = mousePos - player.MountedCenter;
					projectile.velocity = diff.RotatedBy(BlockAngleAdjust);
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

				projectile.Center = player.MountedCenter + (projectile.velocity * HoldingDistance);
				projectile.velocity *= 8f;

			}
		}
		protected virtual void DrawAdd()
		{
			bool facingleft = projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.None;
			Texture2D texture = MyTexture;
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float facing = facingleft ? AngleAdjust : -AngleAdjust;

			float alpha = MathHelper.Clamp((30 - Blocktimer) / 8f, 0f, 1f);
			if (alpha>0f)
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), Main.hslToRgb((Main.GlobalTime * 3f) % 1f, 1f, 0.85f) * alpha, (projectile.velocity.ToRotation() + facing) + (facingleft ? 0 : MathHelper.Pi), origin, projectile.scale + 0.25f, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);

		}
		public void DrawAdditive(SpriteBatch spriteBatch)
		{
			DrawAdd();
		}
		public virtual void DrawNormal(SpriteBatch spriteBatch, Color drawColor)
		{
			bool facingleft = projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.None;
			Texture2D texture = MyTexture;
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float facing = facingleft ? AngleAdjust : -AngleAdjust;
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), drawColor * AlphaFade * projectile.Opacity, (projectile.velocity.ToRotation() + facing) + (facingleft ? 0 : MathHelper.Pi), origin, projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			DrawNormal(spriteBatch, drawColor);
			return false;
		}

	}

	public class CorrodedShieldProjDash : ModProjectile, IShieldBashProjectile
	{
		public Player player => Main.player[projectile.owner];
		public virtual int HoldType => ModContent.ItemType<CorrodedShield>();
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
			if (GetType() == typeof(CorrodedShieldProjDash))
				target.AddBuff(mod.BuffType("AcidBurn"), (int)(60 * 1.50));
		}

		public override void AI()
		{

			bool heldone = player.HeldItem.type != HoldType;
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
					//player.GetModPlayer<SGAPlayer>().realIFrames = 30;

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

	public class DankWoodShield : CorrodedShield, IShieldItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dank Wood Shield");
			Tooltip.SetDefault("");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.width = 24;
			item.height = 32;
			item.useTime = 70;
			item.damage = 0;
			item.crit = 0;
			item.value = Item.sellPrice(0, 0, 25, 0);
			item.expert = false;
			item.rare = ItemRarityID.Blue;
		}

		public override bool CanUseItem(Player player)
		{
			return false;
		}

		public override void AddRecipes()
		{
			if (GetType() == typeof(DankWoodShield))
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddIngredient(mod.ItemType("DankWood"), 30);
				recipe.AddRecipeGroup("IronBar", 6);
				recipe.AddTile(TileID.WorkBenches);
				recipe.SetResult(this, 1);
				recipe.AddRecipe();
			}
		}

	}

	public class DankWoodShieldProj : CorrodedShieldProj, IDrawAdditive
	{
		protected override float BlockDamage => 0.25f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("DankWoodShield");
		}

	}

	public class RiotShield : DankWoodShield, IShieldItem
	{
		public override string DamagePercent => "at a large angle";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Riot Shield");
			Tooltip.SetDefault("Performing a Just Block keeps the shield active for several seconds with other weapons\nSlows the player and grants knockback immunity when active");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.width = 24;
			item.height = 32;
			item.useTime = 70;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Lime;
		}
	}

	public class RiotShieldProj : DankWoodShieldProj, IDrawAdditive
	{
		protected override float BlockDamage => 0.25f;
		protected override float BlockAngle => 0.25f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("RiotShieldProj");
		}
		public override void JustBlock(int blocktime, Vector2 where, ref int damage, int damageSourceIndex)
		{
			projectile.timeLeft = 450;
			projectile.netUpdate = true;

		}
		public override void WhileHeld(Player player)
		{
			player.noKnockback = true;
			player.maxRunSpeed = Math.Max(1, player.maxRunSpeed - 20);
		}
	}

	public class Magishield : DankWoodShield, IShieldItem
	{
		public override string DamagePercent => "at a small angle";
		//		public override string DamagePercent => "Blocks "+ (100-(100f/((Main.LocalPlayer.magicDamage*2f)-1f))) + "% of damage at a small angle";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magishield");
			Tooltip.SetDefault("Use your mana as a shield, consuming mana instead\nDamage taken is reduced by Mana Costs and Magic Damage\nDoesn't work when Mana Sick or with a Mana Regen Potion\nYou do not regen mana while holding the shield out\nPerforming a Just Block spawns a Mana regenerating Nebula Booster");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.width = 24;
			item.height = 32;
			item.useTime = 70;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Pink;
		}
	}

	public class MagishieldProj : DankWoodShieldProj, IDrawAdditive
	{
		protected override float BlockDamage => 1f - (1f / ((player.magicDamage * 2) - 1f));
		protected override float BlockAngle => base.BlockAngle;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("MagishieldProj");
		}
		public override void JustBlock(int blocktime, Vector2 where, ref int damage, int damageSourceIndex)
		{
			Item.NewItem(where, Vector2.Zero, ItemID.NebulaPickup3);
		}
		public override bool HandleBlock(ref int damage, Player player)
		{
			if (player.manaCost == 0.0f)
				player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " tried to block with unlimited mana and divided by 0"), 1337, 0);
			//damage = (int)(damage / (player.magicDamage*2)-1f);
			if (!player.manaSick && !player.manaRegenBuff && player.CheckMana(new Item(), damage, true, false))
			{
				damage -= player.statMana;
				player.immune = true;
				player.immuneTime = 10;
				Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 9, 0.6f, 0.5f);
				player.manaRegenDelay = Math.Max(player.manaRegenDelay, 180);
				player.velocity -= Vector2.UnitX * player.direction * 8f;
				return false;
			}
			return true;
		}
		public override void WhileHeld(Player player)
		{
			player.manaRegenDelay = Math.Max(player.manaRegenDelay, 60);
		}
	}

	public class DiscordShield : CorrodedShield, IShieldItem
	{

		//public override string DamagePercent => "Blocks "+ (100-(100f/((Main.LocalPlayer.magicDamage*2f)-1f))) + "% of damage at a small angle";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shield of Discord");
			Tooltip.SetDefault("Left Click to teleport, same Rod of Discord rules apply\nPerforming a Just Block removes chaos state from the player");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.width = 24;
			item.height = 32;
			item.useTime = 70;
			item.useTime = 20;
			item.useAnimation = 20;
			item.damage = 0;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.UseSound = SoundID.Item7;
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ProjectileID.TruffleSpore;
			item.shootSpeed = 12f;

			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = Main.itemTexture[ItemID.RodofDiscord];
				item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Main.hslToRgb((Main.GlobalTime * 0.5f) % 1f, 0.5f, 0.65f);
				};
			}
		}

		public override bool CanUseItem(Player player)
		{
			return CanRoDTeleport(player);
		}

		public static bool CanRoDTeleport(Player player)
        {
			Vector2 vector27 = default(Vector2);
			vector27.X = (float)Main.mouseX + Main.screenPosition.X;
			if (player.gravDir == 1f)
			{
				vector27.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)player.height;
			}
			else
			{
				vector27.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
			}
			vector27.X -= player.width / 2;
			if (vector27.X > 50f && vector27.X < (float)(Main.maxTilesX * 16 - 50) && vector27.Y > 50f && vector27.Y < (float)(Main.maxTilesY * 16 - 50))
			{
				int num263 = (int)(vector27.X / 16f);
				int num264 = (int)(vector27.Y / 16f);
				if ((Main.tile[num263, num264].wall != 87 || !((double)num264 > Main.worldSurface) || NPC.downedPlantBoss) && !Collision.SolidCollision(vector27, player.width, player.height))
				{
					return true;
				}
			}
			return false;
		}

		public static void RoDTeleport(Player player,int teletype = 0)
        {
			Vector2 vector27 = default(Vector2);
			vector27.X = (float)Main.mouseX + Main.screenPosition.X;
			//Just RoD stuff
			if (player.gravDir == 1f)
			{
				vector27.Y = (float)Main.mouseY + Main.screenPosition.Y - (float)player.height;
			}
			else
			{
				vector27.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
			}


			player.Teleport(vector27, teletype);
			NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, vector27.X, vector27.Y, 1);

			//More RoD stuff
			if (player.chaosState && teletype == 1)
			{
				player.statLife -= player.statLifeMax2 / 7;
				PlayerDeathReason damageSource = PlayerDeathReason.ByOther(13);
				if (Main.rand.Next(2) == 0)
				{
					damageSource = PlayerDeathReason.ByOther(player.Male ? 14 : 15);
				}
				if (player.statLife <= 0)
				{
					player.KillMe(damageSource, 1.0, 0);
				}
				player.lifeRegenCount = 0;
				player.lifeRegenTime = 0;
			}

			player.AddBuff(BuffID.ChaosState, 360);


		}


		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			RoDTeleport(player,1);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RodofDiscord, 1);
			recipe.AddIngredient(ItemID.CrystalShard, 15);
			recipe.AddIngredient(ItemID.HallowedBar, 8);
			recipe.AddIngredient(mod.ItemType("StygianCore"), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	public class DiscordShieldProj : DankWoodShieldProj, IDrawAdditive
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("DiscordShieldProj");
		}

		public override void JustBlock(int blocktime, Vector2 where, ref int damage, int damageSourceIndex)
		{
			Player player = Main.player[projectile.owner];
			if (player != null && player.HasBuff(BuffID.ChaosState))
				player.DelBuff(player.FindBuffIndex(BuffID.ChaosState));
		}

	}

	public class EarthbreakerShield : CorrodedShield, IShieldItem
	{
		private SolarShieldProj SolarShieldGet => GetShieldProj?.modProjectile as SolarShieldProj;

		//public override string DamagePercent => "Blocks "+ (100-(100f/((Main.LocalPlayer.magicDamage*2f)-1f))) + "% of damage at a small angle";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthbreaker Shield");
			Tooltip.SetDefault("Bulwark of Gaia, defence of her wrath!\nBash-Dash throws the player down for a ground slam, impaling 5 nearby enemies\nImpaled enemies are immobile and lose 10 defence for 15 seconds\nEnemies can not be impaled again til they recover their defence\nPerforming a Just Block impales 3 nearby enemies");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.width = 24;
			item.height = 32;
			item.useTime = 70;
			item.damage = 50;
			item.useTime = 60;
			item.useAnimation = 60;
			item.melee = true;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.Pink;
			item.shoot = ModContent.ProjectileType<EarthbreakerShieldProjDash>();
			item.shootSpeed = 12f;
		}

		public override void AddRecipes()
		{
			//none
		}
	}

	public class EarthbreakerShieldProj : DankWoodShieldProj, IDrawAdditive
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("EarthbreakerShieldProj");
		}

		public override void WhileHeld(Player player)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<EarthbreakerShieldProjDash>()] > 0)
				player.maxFallSpeed += 10f;
		}

		public override void JustBlock(int blocktime, Vector2 where, ref int damage, int damageSourceIndex)
		{
			EarthbreakerShieldProjDash.Entrap(where, 200, 3, projectile.owner);
		}

	}

	public class EarthbreakerShieldProjDash : CorrodedShieldProjDash, IShieldBashProjectile
	{
		public override int HoldType => ModContent.ItemType<EarthbreakerShield>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("EarthbreakerShieldProjDash");
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.timeLeft = 100;
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
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			if (projectile.velocity.Y > 4f)
			{

				Main.PlaySound(SoundID.Item14, projectile.Center);
				for (float gg = -4f; gg < 4.22f; gg += 0.5f)
				{
					Gore.NewGore(projectile.Center + new Vector2(Main.rand.NextFloat(-32, 32), Main.rand.NextFloat(-16, 16)), new Vector2(gg, Main.rand.NextFloat(-9, -2)), Main.rand.Next(61, 64), (4f - Math.Abs(gg)) / 1.5f);
				}

				Entrap(projectile.Center, 300, 5, projectile.owner, projectile);
			}

			return false;
		}

		public static void Entrap(Vector2 where, float distance, int count, int owner, Projectile projpoj = null)
		{
			List<NPC> them = Main.npc.Where(testnpc => testnpc.active && CrackedMirrorProj.AffectNPC(testnpc) && testnpc.DistanceSQ(where) < distance * distance && !testnpc.HasBuff(ModContent.BuffType<PiercedVulnerable>())).ToList();
			if (them.Count > 0)
			{
				int limit = 0;
				them = them.OrderBy(testnpc => testnpc.DistanceSQ(where)).ToList();
				foreach (NPC npc in them)
				{
					if (limit > count)
						break;

					SGAmod.AddScreenShake(28f, 320, npc.Center);

					SoundEffectInstance sound = Main.PlaySound(SoundID.Item70, npc.Center);
					if (sound != null)
					{
						sound.Pitch += 0.50f;
					}
					int damage = projpoj?.damage ?? 0;
					int proj = Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y + EarthbreakerRockSpike.OffsetPoint), new Vector2(0, -8f), ModContent.ProjectileType<EarthbreakerRockSpike>(), damage, 0f, owner);
					Main.projectile[proj].ai[1] = npc.whoAmI;
					Main.projectile[proj].netUpdate = true;
					limit += 1;

				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			height += 64;
			fallThrough = false;

			return true;
		}

		public override void AI()
		{
			base.AI();
			Player owner = Main.player[projectile.owner];

			player.itemTime = 10;
			player.itemAnimation = 10;

			if (projectile.timeLeft < 70)
			{
				if ((projectile.timeLeft < 40 && projectile.velocity.Y > 3f) || projectile.timeLeft > 6)
					projectile.timeLeft = 5;
				//if (owner.velocity.Y<15f)
				player.velocity.Y += 0.25f;
				projectile.velocity = player.velocity;
				projectile.tileCollide = true;
			}
			else
			{
				player.velocity *= 0.9f;
			}

			player.velocity.X *= 0.99f / (1f + (projectile.ai[1] / 50f));


			int num70 = Dust.NewDust(new Vector2(owner.position.X, owner.position.Y + owner.height) + Vector2.Normalize(projectile.velocity) * 3f, owner.width, 0, 31, 0f, 0f, 100, default(Color), 1.25f);
			Main.dust[num70].velocity = Vector2.Normalize(player.velocity + new Vector2(0, 3)) * 4f;
			Main.dust[num70].velocity += (Vector2.Normalize(owner.velocity) * 2.5f) + new Vector2(Main.rand.NextFloat(-5, 5));
			Main.dust[num70].noGravity = true;
			Main.dust[num70].shader = GameShaders.Armor.GetSecondaryShader(owner.ArmorSetDye(), owner);


		}
		public override string Texture
		{
			get { return "SGAmod/Invisible"; }
		}

	}
	public class EarthbreakerRockSpike : ModProjectile
	{
		float strength => Math.Min(1f - (projectile.localAI[1] / 110f), 1f);
		public static int OffsetPoint => 64;
		public static bool ApplyPrismOnce = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthbreaker Spike");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.tileCollide = false;
			projectile.timeLeft = 300;
			projectile.hide = true;
		}
		public override string Texture
		{
			get { return "SGAmod/Invisible"; }
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsBehindNPCsAndTiles.Add(projectile.whoAmI);
		}
		public override bool PreKill(int timeLeft)
		{
			SoundEffectInstance sound = Main.PlaySound(SoundID.Item70, projectile.Center);
			if (sound != null)
			{
				sound.Pitch -= 0.50f;

			}
			return true;
		}
		public override void AI()
		{
			projectile.localAI[0] += 1;
			NPC target = Main.npc[(int)projectile.ai[1]];
			if (projectile.localAI[0] > 5)
			{
				projectile.velocity *= 0.85f;

			}
			if (target.active && target.life > 0)
			{
				SGAnpcs snpc = target.SGANPCs();
				target.AddBuff(ModContent.BuffType<PiercedVulnerable>(), 60 * 15);
				snpc.noMovement = Math.Max(snpc.noMovement, 3);
				target.Center = projectile.Center + new Vector2(0, -OffsetPoint);
				if (projectile.velocity.Y < 0 && !Collision.CanHitLine(target.Center, 16, 16, target.Center - Vector2.UnitY * 32, 8, 8))
					projectile.velocity.Y = -projectile.velocity.Y / 2f;

			}
			else
			{
				projectile.Kill();
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			if (projectile.localAI[1] < 100)
				projectile.localAI[1] = 100 + Main.rand.Next(0, 3);

			float rot = MathHelper.Pi;
			Texture2D tex = Main.extraTexture[35];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 3) / 2.5f;
			Vector2 drawPos = ((projectile.Center)) + new Vector2(0f, 4f);
			int timing = (int)(projectile.localAI[1] - 100);
			timing %= 3;
			timing *= ((tex.Height) / 3);

			for (int i = -64; i < 180; i += 32)
			{
				for (int a = -8; a < 9; a += 8)
				{
					Point loc = new Point((int)((drawPos.X + a) / 16f), (int)((drawPos.Y + (i + Math.Abs(a))) / 16f));
					Color lighting = Lighting.GetColor(loc.X, loc.Y);
					if (Lighting.GetBlackness(loc.X, loc.Y).R < 220)
					{
						spriteBatch.Draw(tex, drawPos - Main.screenPosition + new Vector2(a, i + Math.Abs(a)), new Rectangle(0, timing, tex.Width, (tex.Height) / 3), Color.Lerp(Color.Brown, Color.White, 0.5f).MultiplyRGB(lighting) * projectile.Opacity, projectile.rotation + rot, drawOrigin, projectile.scale * 1f, SpriteEffects.None, 0f);
					}
				}
			}


			return false;

		}

	}

	public class SolarShield : CorrodedShield, IShieldItem
	{
		private SolarShieldProj SolarShieldGet => GetShieldProj?.modProjectile as SolarShieldProj;
		public override string DamagePercent
		{
			get
			{
				SolarShieldProj sheld = GetShieldProj?.modProjectile as SolarShieldProj;
				if (sheld != null)
				{
					if (sheld.Level > 0)
						return sheld.Level == 1 ? "at a medium angle" : "at a large angle";
				}

				return "at a small angle";
			}
		}

		//		public override string DamagePercent => "Blocks "+ (100-(100f/((Main.LocalPlayer.magicDamage*2f)-1f))) + "% of damage at a small angle";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Shield");
			Tooltip.SetDefault("Grows in power when held, growth scales with your melee speed\nIncreases block angle and reduces damage up to 3 levels\nBlocking an attack creates a solar explosion\nPerforming a Just Block sets the shield to level 3\nGain a fiery bash-dash with atleast a level 2 shield");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.width = 24;
			item.height = 32;
			item.useTime = 70;
			item.damage = 150;
			item.useTime = 60;
			item.useAnimation = 60;
			item.melee = true;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.shoot = ModContent.ProjectileType<SolarShieldProjDash>();
			item.shootSpeed = 16f;
		}
		public override bool CanUseItem(Player player)
		{
			return SolarShieldGet.Level > 0;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			SolarShieldGet.LevelConsume();
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CorrodedShield>(), 1);
			recipe.AddIngredient(ItemID.FragmentSolar, 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	public class SolarShieldProj : DankWoodShieldProj, IDrawAdditive
	{
		protected override float BlockDamage => (Level + 1) * 0.25f;
		protected override float BlockAngle => 1f - ((Level + 1) * 0.25f);
		protected override float AngleAdjust => MathHelper.Pi * 0.05f;
		private int LevelUpTime => (int)(200f * ((float?)player.meleeSpeed ?? 1f));
		public int Level
		{
			get
			{
				if (buildUp >= MaxShieldLevel - 1)
					return 2;
				return (int)Math.Min(buildUp / (float)(LevelUpTime), 2);
			}
		}
		private int MaxShieldLevel => (LevelUpTime * 2) + 1;
		public int buildUp = 0;
		public override Texture2D MyTexture => Main.extraTexture[61 + Level];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SolarShieldProj");
		}
		public override string Texture
		{
			get { return "SGAmod/Invisible"; }
		}

		public override void JustBlock(int blocktime, Vector2 where, ref int damage, int damageSourceIndex)
		{
			HandleBlock(ref damage, player);
			buildUp = MaxShieldLevel;
		}
		public void LevelConsume()
		{
			if (buildUp > MaxShieldLevel)
				buildUp = MaxShieldLevel;

			buildUp -= LevelUpTime;
		}
		public override bool HandleBlock(ref int damage, Player player)
		{
			LevelConsume();
			if (player.HeldItem.type == mod.ItemType(Name.Replace("Proj", "")))
				projectile.damage = player.HeldItem.damage;

			int num4 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ProjectileID.SolarCounter, projectile.damage, 15f, Main.myPlayer);
			Main.projectile[num4].timeLeft = 1;
			Main.projectile[num4].Kill();

			return true;
		}
		public override void WhileHeld(Player player)
		{
			buildUp = Math.Max(buildUp + 1, -60);
		}

		protected override void DrawAdd()
		{
			DrawNormal(Main.spriteBatch, Color.White);
			base.DrawAdd();
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			//SGAUtils.DrawMoonlordHand(projectile.Center, Main.MouseWorld);
			return false;
		}

	}

	public class SolarShieldProjDash : CorrodedShieldProjDash, IShieldBashProjectile
	{
		public override int HoldType => ModContent.ItemType<SolarShield>();
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
			projectile.timeLeft = 20;
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

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int num4 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ProjectileID.SolarCounter, projectile.damage, 15f, Main.myPlayer);
			Main.projectile[num4].timeLeft = 1;
			Main.projectile[num4].Kill();
		}

		public override void AI()
		{
			base.AI();
			Player owner = Main.player[projectile.owner];
			if (projectile.velocity.LengthSquared() < 1)
				projectile.velocity = Vector2.One * 0.05f;

			int num70 = Dust.NewDust(owner.position + Vector2.Normalize(projectile.velocity) * 3f, owner.width, owner.height, 31, 0f, 0f, 100, default(Color), 1.5f);
			Main.dust[num70].velocity *= 0.2f;
			Main.dust[num70].velocity += Vector2.Normalize(owner.velocity) * 2.5f;
			Main.dust[num70].noGravity = true;
			Main.dust[num70].shader = GameShaders.Armor.GetSecondaryShader(owner.ArmorSetDye(), owner);

			for (int num78 = 0; num78 < 5; num78++)
			{
				int num79 = Dust.NewDust(owner.position, owner.width, owner.height, 6, 0f, 0f, 0, default(Color), 2.7f);
				Dust dust38 = Main.dust[num79];
				dust38.position = owner.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732) * owner.width / 2f;
				dust38.noGravity = true;
				dust38.velocity = Vector2.UnitX.RotatedByRandom(3.1415927410125732) * (Main.rand.NextFloat(1.25f, 2.75f));
				dust38.velocity += Vector2.Normalize(owner.velocity) * 5f;
				dust38.shader = GameShaders.Armor.GetSecondaryShader(owner.ArmorSetDye(), owner);
			}


		}
		public override string Texture
		{
			get { return "SGAmod/Invisible"; }
		}

	}

	public class CapShield : CorrodedShield, IShieldItem
	{
		public override string DamagePercent => "at a decent angle";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Captain America's Shield");
			Tooltip.SetDefault("Performing a Just Block grants a fews seconds of Striking Moment\nCharge up to enable a powerful bash-dash!\nThis bash-dash may be cancelled early by unequiping the shield\nAlt Fire lets you throw the shield, which will bounce between nearby enemies\nYou cannot use your shield while it is thrown, gains +1 bounces per 30 defense\n'Stars and Stripes!'");
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
			item.rare = ItemRarityID.Red;
			item.UseSound = SoundID.Item7;
			item.shoot = mod.ProjectileType("CapShieldProjDash");
			item.shootSpeed = 20f;
			item.useTurn = false;
			item.autoReuse = false;
			item.channel = true;
			item.noMelee = true;
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
				int thisoned = Projectile.NewProjectile(position.X, position.Y, speedX * player.Throwing().thrownVelocity, speedY * player.Throwing().thrownVelocity, type, damage, knockBack, Main.myPlayer);
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
			base.ModifyTooltips(tooltips);
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
			add += ((player.Throwing().thrownDamage * 1.5f) + player.meleeDamage) / 2f;
			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("DankWoodShield"), 1);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 12);
			recipe.AddRecipeGroup("Fragment", 8);
			recipe.AddIngredient(ItemID.LunarBar, 5);
			recipe.AddIngredient(ItemID.RedDye, 1);
			recipe.AddIngredient(ItemID.SilverDye, 1);
			recipe.AddIngredient(ItemID.BlueDye, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class CapShieldProj : CorrodedShieldProj, IDrawAdditive
	{
		protected override float BlockAngle => 0.4f;
		protected override float BlockDamage => 0.5f;
		public override void JustBlock(int blocktime, Vector2 where, ref int damage, int damageSourceIndex)
		{
			player.AddBuff(BuffID.ParryDamageBuff, 60 * 3);

		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CapShieldProj");
		}

	}

	public class CapShieldProjDash : CorrodedShieldProjDash, IShieldBashProjectile
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
			return projectile.ai[1] > 0;
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
					for (float new1 = 0; new1 < 360; new1 = new1 + 360 / 10f)
					{
						if (projectile.ai[0] * (360f / 150f) >= new1)
						{
							float angle = new1;
							Vector2 angg = player.velocity.RotatedBy(MathHelper.ToRadians(angle)) + (angle + MathHelper.ToRadians(angle)).ToRotationVector2() * 10f;
							int DustID2 = Dust.NewDust(new Vector2(player.Center.X - 8, player.Center.Y - 8), 16, 16, DustID.AncientLight, 0, 0, 20, Color.Silver, 1f);
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
							//player.GetModPlayer<SGAPlayer>().realIFrames = 30;
						}
					}
					else
					{
						projectile.velocity.Y *= 0.98f;
					}
					player.velocity.X = projectile.velocity.X * (1f + (projectile.ai[0] / 120f));
					//player.velocity.Y = projectile.velocity.Y;

					for (float jj = 2; jj < 14; jj += 2)
					{
						for (float new1 = -1f; new1 < 2f; new1 = new1 + 2f)
						{
							float angle = 90;
							Vector2 velo = player.velocity;
							velo.Normalize();
							Vector2 angg = velo.RotatedBy(angle * new1);
							int DustID2 = Dust.NewDust(new Vector2(projectile.Center.X - 8, projectile.Center.Y - 8), 16, 16, DustID.AncientLight, 0, 0, 20, jj < 5 ? Color.White : new1 < 0 ? Color.Red : Color.Blue, 1f + (14f - jj) / 14);
							Main.dust[DustID2].velocity = new Vector2(angg.X * jj, angg.Y * jj);
							Main.dust[DustID2].noGravity = true;
						}
					}

					projectile.Center = player.Center + projectile.velocity;



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

	public class CapShieldToss : ModProjectile, IShieldBashProjectile
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
			if (projectile.ai[0] > 30)
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
				if (Main.npc[num172].active && !Main.npc[num172].friendly && !Main.npc[num172].townNPC && Main.npc[num172].CanBeChasedBy() && projectile.localNPCImmunity[num1722] < 1)
				{
					float num173 = Main.npc[num172].position.X + (float)(Main.npc[num172].width / 2);
					float num174 = Main.npc[num172].position.Y + (float)(Main.npc[num172].height / 2);
					float num175 = Math.Abs(loc.X + (float)(size.X / 2) - num173) + Math.Abs(loc.Y + (float)(size.Y / 2) - num174);
					if (Main.npc[num172].active)
					{

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
				projectile.penetrate += (int)((float)Main.player[projectile.owner].statDefense / 30f);
				projectile.ai[1] = FindClosestTarget(projectile.Center, new Vector2(0f, 0f));

			}
			projectile.velocity.Y += 0.1f;
			if ((projectile.ai[0] > 90f || (projectile.penetrate < 10 && projectile.ai[0] > 20)) && !Main.player[projectile.owner].dead)
			{
				Vector2 dist = (Main.player[projectile.owner].Center - projectile.Center);
				Vector2 distnorm = dist; distnorm.Normalize();
				projectile.velocity += distnorm * (5f + ((float)projectile.timeLeft / 40f));
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
					projectile.Center += (projectile.DirectionTo(target.Center) * (projectile.ai[0] > 8f ? (50f * Main.player[projectile.owner].thrownVelocity) / hittime : 12f));
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

	public class AegisaltAetherstone : CorrodedShield, IShieldItem,IDevItem
	{
		public override string DamagePercent => "at a large angle";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aegisalt Aetherstone");
			Tooltip.SetDefault("Makes copies of itself when held out; covering every direction\nEach shield goes down after taking a hit, and recovers faster with Melee Speed\nAttack launches all active shields out as piercing waves\nAlt-Fire tightens the shields, allowing for a narrower wave shot\nPerforming a Just Block spawns hearts and mana stars, and resets Barrier cooldown");
			Item.staff[item.type] = true;
		}

		public (string, string) DevName()
		{
			return ("JellyBru", "other");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.width = 24;
			item.height = 32;
			item.damage = 100;
			item.useTime = 40;
			item.useAnimation = 40;
			item.melee = true;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.shoot = ModContent.ProjectileType<AegisaltAetherstoneProjDash>();
			item.shootSpeed = 16f;
			item.UseSound = default;
		}
        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
			if (player.SGAPly().devempowerment[2]>0)
				mult += 0.50f;
        }

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (Main.LocalPlayer.GetModPlayer<SGAPlayer>().devempowerment[2] > 0)
			{
				tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "--- Enpowerment bonus ---"));
				tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "Damage, shield regen rate, block percent and angle are greatly improved"));
				tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "Bash does damage at interverals"));
			}
			base.ModifyTooltips(tooltips);
		}

		public override bool AltFunctionUse(Player player)
        {
			List<Projectile> them = Main.projectile.Where(testby => testby.owner == player.whoAmI && testby.modProjectile != null && testby.modProjectile is AegisaltAetherstoneProj).ToList();
			foreach (Projectile proj2 in them)
			{
				AegisaltAetherstoneProj projha = (AegisaltAetherstoneProj)proj2.modProjectile;
				if (projha.Blocktimer < projha.TimeToCopy)
                {
					return false;
                }
			}

			return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (player.altFunctionUse != 2)
			{
				return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			}
			else
			{
				List<Projectile> them = Main.projectile.Where(testby => testby.owner == player.whoAmI && testby.modProjectile != null && testby.modProjectile is AegisaltAetherstoneProj).ToList();
				foreach (Projectile proj2 in them)
				{
					AegisaltAetherstoneProj projha = (AegisaltAetherstoneProj)proj2.modProjectile;
					projha.tightFormation = projha.tightFormation ? false : true;
					if (projha.genCopy == 0)
					{
						SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_DefenseTowerSpawn, (int)proj2.Center.X, (int)proj2.Center.Y);
						if (sound != null)
						{
							sound.Pitch = 0.8f;
						}
					}

				}
			}
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 40);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	public class AegisaltAetherstoneProj : CorrodedShieldProj, IDrawAdditive
	{
		protected override float BlockDamage => player.SGAPly().devempowerment[2] > 0 ? 0.50f : 0.25f;
		protected override float BlockAngle => disabledTimer > 0 ? 10f : (player.SGAPly().devempowerment[2] > 0 ? 0.0f : 0.25f);

		protected override float AlphaFade => MathHelper.Clamp(1f - Math.Min(disabledTimer / 20f, 0.75f), 0f, Math.Min(Blocktimer / TimeToCopy, 1f));

		protected override float HoldingDistance => MathHelper.Clamp(((Blocktimer / TimeToCopy) * 32f) + Math.Abs(genCopy * 100), 0f, 32f);

		public bool copied = false;

		public bool tightFormation = false;

		public float spreadValue = 1f;

		public float TimeToCopy => 30f * player.meleeSpeed;
		int maxCopies = 2;
		public float disabledTimer = 0;
		float AddAngleAmmount => (((MathHelper.Pi * 0.80f) / ((float)maxCopies)) * (genCopy > 0 ? 1f : -1f));

		protected override float BlockAngleAdjust => (genCopy == 0 ? 0 : angleAdd + MathHelper.Clamp(Blocktimer / TimeToCopy, 0f, 1f) * AddAngleAmmount) * spreadValue;

		public int genCopy = 0;
		public float angleAdd = 0;

		public override Texture2D MyTexture => mod.GetTexture("Items/Weapons/Shields/AegisaltAetherstoneProj");

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aegisalt Aetherstone");
		}
		public override void AI()
		{

			spreadValue += ((!tightFormation ? 1f : 0.25f) - spreadValue) / 8f;

			disabledTimer -= 6f - (player.meleeSpeed * 5f);

			if (disabledTimer > 0)
			{
				projectile.timeLeft = 5;
			}

			if (!copied && Blocktimer > TimeToCopy && Math.Abs(genCopy) < maxCopies)
			{
				copied = true;
				for (int i = -1; i < 2; i += 2)
				{
					if (genCopy == 0 || (genCopy < 0 && i < 0) || (genCopy > 0 && i > 0))
					{
						Projectile proj = Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
						if (proj != null)
						{
							((AegisaltAetherstoneProj)proj?.modProjectile).genCopy = genCopy + i;

							((AegisaltAetherstoneProj)proj?.modProjectile).angleAdd = angleAdd + (genCopy == 0 ? 0f : ((AegisaltAetherstoneProj)proj?.modProjectile).AddAngleAmmount);
							if (i == 1)
							{
								SoundEffectInstance sound = Main.PlaySound(SoundID.Item78, (int)projectile.Center.X, (int)projectile.Center.Y);
								if (sound != null)
								{
									sound.Pitch = -0.8f + Math.Abs(genCopy) / 10f;
								}
							}
						}
					}
				}
			}
			base.AI();
		}
		public override void JustBlock(int blocktime, Vector2 where, ref int damage, int damageSourceIndex)
		{
			player.SGAPly().energyShieldAmmountAndRecharge.Item3 = 0;
			player.QuickSpawnItem(ItemID.Heart);
			player.QuickSpawnItem(ItemID.Star);
		}
		public override bool HandleBlock(ref int damage, Player player)
		{
			disabledTimer = player.SGAPly().devempowerment[2]>0 ? 40 : 75;
			return true;
		}
	}

	public class AegisaltAetherstoneProjDash : CorrodedShieldProjDash, IShieldBashProjectile
	{
		public override int HoldType => ModContent.ItemType<AegisaltAetherstone>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("AegisaltAetherstoneProj");
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.timeLeft = 2;
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
			return false;
		}

		public override void AI()
		{
			Player owner = Main.player[projectile.owner];
			if (owner.SGAPly().devempowerment[2] > 0)
				projectile.localNPCHitCooldown = 4;

			foreach (Projectile proj2 in Main.projectile.Where(testby => testby.owner == projectile.owner && testby.modProjectile != null && testby.modProjectile is AegisaltAetherstoneProj))
			{
				AegisaltAetherstoneProj projha = (AegisaltAetherstoneProj)proj2.modProjectile;
				if (projha.disabledTimer < 1)
				{
					projha.disabledTimer = Math.Max(projha.disabledTimer, 75);
					Projectile.NewProjectileDirect(projectile.Center, Vector2.Normalize(proj2.velocity) * projectile.velocity.Length(), ModContent.ProjectileType<AegisaltAetherstoneProjSlashWave>(), projectile.damage, projectile.knockBack, projectile.owner);

					SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_FlameburstTowerShot, (int)projectile.Center.X, (int)projectile.Center.Y);
					if (sound != null)
					{
						sound.Pitch = 0.25f;
					}

				}
			}
		}
		public override string Texture
		{
			get { return "SGAmod/Invisible"; }
		}
	}

	public class AegisaltAetherstoneProjSlashWaveEffect
	{

		public int time = 0;
		public int timeMax = 0;
		public Vector2 position;

		public AegisaltAetherstoneProjSlashWaveEffect(int time,Vector2 pos)
		{
			this.time = 0;
			this.timeMax = time;
			position = pos;
		}

	}

	public class AegisaltAetherstoneProjSlashWave : ModProjectile, IShieldBashProjectile, IDrawAdditive
	{
		List<AegisaltAetherstoneProjSlashWaveEffect> effects = new List<AegisaltAetherstoneProjSlashWaveEffect>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("AegisaltAetherstoneProj");
		}
		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.timeLeft = 60;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.light = 0.5f;
			projectile.width = 72;
			projectile.height = 72;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.aiStyle = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 4;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (player != null)
				projectile.localNPCHitCooldown = player.SGAPly().devempowerment[2]>0 ? 4 : -1;

			projectile.ai[0] += 1;

			if (projectile.ai[0] % 4 == 0)
			{
				effects.Add(new AegisaltAetherstoneProjSlashWaveEffect(25, projectile.Center));
			}

			for (int i = 0; i < effects.Count; i += 1)
			{
				effects[i].time += 1;
				if (effects[i].time > effects[i].timeMax)
				{
					effects.RemoveAt(i);
					i -= 1;
				}
			}
			projectile.rotation = projectile.velocity.ToRotation();
		}

		public void DrawAdditive(SpriteBatch spriteBatch)
		{
			bool facingleft = projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.None;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float facing = -1;

			for (int i = 0; i < effects.Count; i += 1)
			{
				AegisaltAetherstoneProjSlashWaveEffect effectx = effects[i];
				float alpha = (1f - MathHelper.Clamp(effectx.time / ((float)effectx.timeMax),0f, 1f))*Math.Min(projectile.timeLeft / 30f, 1f);
				Main.spriteBatch.Draw(texture, effectx.position - Main.screenPosition, null, Main.hslToRgb((Main.GlobalTime * 3f) % 1f, 1f, 0.85f) * alpha, projectile.rotation, origin, (Vector2.One*projectile.scale) + new Vector2(0, 1f-alpha), effect, 0);
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White * Math.Min(projectile.timeLeft / 30f, 1f), projectile.rotation, origin, (Vector2.One * projectile.scale), SpriteEffects.None, 0);
		}

        public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Shields/AegisaltAetherstoneProjAlt"; }
		}

	}

}