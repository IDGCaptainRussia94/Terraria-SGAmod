using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAAAUThrowing;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SGAmod.Effects;
using Idglibrary;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using System;
using SGAmod.Projectiles;

namespace SGAmod.Items.Armors.Vibranium
{

	[AutoloadEquip(EquipType.Head)]
	public class VibraniumMask : Weapons.Vibranium.VibraniumText
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Mask");
			Tooltip.SetDefault("25% increased melee speed\n32% increased melee damage and 25% crit chance\nMelee Apocalyptical Chance increases the more electric charge you have");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = ItemRarityID.Red;
			item.defense = 50;
		}
        public override bool DrawHead()
        {
            return false;
        }
        public override void UpdateEquip(Player player)
		{
			player.meleeSpeed += 0.25f;
			player.meleeDamage += 0.32f;
			player.meleeCrit += 25;
			SGAPlayer sgaplayer = player.GetModPlayer(mod,typeof(SGAPlayer).Name) as SGAPlayer;
			float percentCharge = (sgaplayer.electricCharge / 5000f)*1f;
			sgaplayer.apocalypticalChance[0] += percentCharge;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VibraniumBar"), 8);
			if (GetType() == typeof(VibraniumHeadgear))
				recipe.AddIngredient(ModContent.ItemType<ManaBattery>(), 2);
			if (GetType() == typeof(VibraniumHelmet))
				recipe.AddIngredient(ItemID.MechanicalLens, 1);
			if (GetType() == typeof(VibraniumMask))
			{
				recipe.AddIngredient(ModContent.ItemType<WraithFragment3>(), 6);
				recipe.AddIngredient(ModContent.ItemType<WraithFragment4>(), 10);
			}
			if (GetType() == typeof(VibraniumHood))
				recipe.AddIngredient(ModContent.ItemType<StygianCore>(), 1);
			if (GetType() == typeof(VibraniumHat))
				recipe.AddIngredient(ModContent.ItemType<PlasmaCell>(), 2);

			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class VibraniumHelmet : VibraniumMask
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Helmet");
			Tooltip.SetDefault("60% increased ranged damage and 10% crit chance\n30% reduced damage on bullet/arrow/rockets types\nWhenever ammo is consumed, you gain their damage as Electric Charge\n2% increased ranged Apocalyptical Chance");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = ItemRarityID.Red;
			item.defense = 25;
		}
		public override void UpdateEquip(Player player)
		{
			player.rangedDamage += 0.60f;
			player.rangedCrit += 10;

			player.bulletDamage -= 0.30f;
			player.rocketDamage -= 0.30f;
			player.arrowDamage -= 0.30f;

			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.apocalypticalChance[1] += 2f;
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class VibraniumHeadgear : VibraniumMask
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Headgear");
			Tooltip.SetDefault("30% increased magic damage and 12% crit chance\nHalf of your mana cost is paid as Electric Charge (by 3X the cost)\n"+Idglib.ColorText(Color.Red, "Will trigger a shield break on deplete")+"\n2% increased magic Apocalyptical Chance");
		}
		internal static bool DoMagicStuff(Player player, ref int ammount, bool pay)
		{
			bool doIt = true;
			if (player.armor[0].type == ModContent.ItemType<VibraniumHeadgear>())
			{
				doIt = player.SGAPly().ConsumeElectricCharge(10 + ammount, 30 + ammount * 2, true, pay);
				ammount = (int)(ammount * 0.50f);
			}
			return doIt;
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = ItemRarityID.Red;
			item.defense = 20;
		}
		public override void UpdateEquip(Player player)
		{
			player.magicDamage += 0.30f;
			player.magicCrit += 12;

			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.apocalypticalChance[2] += 2f;

			if (sgaplayer.ShieldType < 1)
				sgaplayer.ShieldType = 100;
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class VibraniumHood : VibraniumMask
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Hood");
			Tooltip.SetDefault("50% increased Summon damage\n+6 max minions and +2 max sentries\nSummon weapons are used 75% faster\nSummons a friendly Resonant Wisp to aid the player\nThis wisp's strength scales with your summon damage and max minions\nWhen not minion targeting, it temporarily cripples nearby enemy projectiles at a cost of Electric Charge\nThis process is faster and cheaper while holding a Summoning weapon");
		}

        public override bool Autoload(ref string name)
        {
			SGAPlayer.PostUpdateEquipsEvent += UpdateSetBonus;
			return true;
        }

		private void UpdateSetBonus(SGAPlayer sgaply)
		{
			if (GetType() == typeof(VibraniumHood))
			{
				Player player = sgaply.player;
				if (!player.dead && player.armor[0].type == ModContent.ItemType<VibraniumHood>() && player.ownedProjectileCounts[ModContent.ProjectileType<VibraniumWispMinion>()] < 1)
				{
					Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<VibraniumWispMinion>(), 100, 2f, player.whoAmI);
				}
			}

		}

        public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = ItemRarityID.Red;
			item.defense = 10;
		}
		public override bool DrawHead()
		{
			return true;
		}
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;

			player.minionDamage += 0.50f;
			player.maxMinions += 6;
			player.maxTurrets += 2;
			sgaplayer.summonweaponspeed += 0.75f;

		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class VibraniumHat : VibraniumHood
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Hat");
			Tooltip.SetDefault("40% increased throwing damage and 15% crit chance\n+30% Throwing velocity and attack speed\nGravitate consumable throwing items from a long range towards you\nThese items spawn magnetic fields to attack enemies\nThe range of both is increased by your Throwing Velocity\n2% increased throwing Apocalyptical Chance");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = ItemRarityID.Red;
			item.defense = 30;
		}
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;

			player.Throwing().thrownDamage += 0.40f;
			player.Throwing().thrownCrit += 15;
			player.Throwing().thrownVelocity += 0.30f;
			sgaplayer.ThrowingSpeed += 0.30f;
			sgaplayer.apocalypticalChance[3] += 2.0;
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class VibraniumChestplate : Weapons.Vibranium.VibraniumText
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Breastplate");
			Tooltip.SetDefault("10% faster item use times\n+1% (1.5% summon) increased damage and 0.5% crit chance per 1000 Electric Charge\nGrants 25% increased radiation resistance and 500% increased recover rate\n+5000 Max Electric Charge and +5 Recharge Rate");
		}
        public override bool Autoload(ref string name)
        {
			SGAPlayer.PreUpdateMovementEvent += VibraniumHoverInPlace;
			return true;
		}
        public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = ItemRarityID.Red;
			item.defense = 30;
		}
		const float DamageMul = 1000f * 100f;
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.UseTimeMul += 0.10f;

			float percentCharge = sgaplayer.electricCharge / DamageMul;
			player.BoostAllDamage(percentCharge, (int)(percentCharge*50));
			player.minionDamage += percentCharge * 0.5f;

			player.SGAPly().electricChargeMax += 5000;
			player.SGAPly().electricrechargerate += 5;
			player.GetModPlayer<IdgPlayer>().radresist += 0.25f;
			player.GetModPlayer<IdgPlayer>().radationRecover += 0.04f;
		}		
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.LocalPlayer;
			float percentCharge = player.SGAPly().electricCharge / DamageMul;
			tooltips.Add(new TooltipLine(mod, "VibraniumChestplateDamageBoost", "Bonus Damage: "+ (percentCharge*100f).ToString("0.00")+"%"));
			tooltips.Add(new TooltipLine(mod, "VibraniumChestplateDamageBoost", "Bonus Crit: " + (int)(percentCharge * 50f) + "%"));
		}
		public static void VibraniumHoverInPlace(SGAPlayer sgaplayer)
		{
			Player player = sgaplayer.player;
			List<Projectile> platforms = Main.projectile.Where(testby => testby.type == ModContent.ProjectileType<VibraniumPlatform>() && testby.owner == player.whoAmI && testby.ai[0] < 1).ToList();
			if (platforms.Count > 0)
			{
				Projectile platfrom = platforms[0];
				if (!player.justJumped && player.velocity.Y >= 0)
				{
					player.velocity.Y = 0;
					player.fallStart = (int)(player.position.Y / 16f);
					player.gfxOffY = 0;
				}
				//player.powerrun = true;
			}
		}

			public static void VibraniumSetBonus(SGAPlayer sgaplayer)
		{
			Player player = sgaplayer.player;

			if (sgaplayer.ShieldType == 0)
				sgaplayer.ShieldType = 100;

			//DoPlatformHere
			if (sgaplayer.vibraniumSetPlatform && player.velocity.Y>=0 && player.ownedProjectileCounts[ModContent.ProjectileType<VibraniumPlatform>()]< 1 && !sgaplayer.Shieldbreak && !player.controlDown && sgaplayer.ConsumeElectricCharge(500,120,true))
            {
				Projectile.NewProjectile(player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<VibraniumPlatform>(), 0, 0, player.whoAmI);
				SoundEffectInstance sound = Main.PlaySound(SoundID.Item78, (int)player.MountedCenter.X, (int)player.MountedCenter.Y);
				if (sound != null)
					sound.Pitch = 0.95f;
			}

			List<Projectile> platforms = Main.projectile.Where(testby => testby.type == ModContent.ProjectileType<VibraniumPlatform>() && testby.owner == player.whoAmI && testby.ai[0] < 1).ToList();
			if (platforms.Count > 0)
			{
				player.powerrun = true;
				//player.slippy2 = true;
			}

			if (!sgaplayer.Shieldbreak && sgaplayer.ConsumeElectricCharge(300, 200, false,false))
			{
				if (sgaplayer.vibraniumSetWall && player.ownedProjectileCounts[ModContent.ProjectileType<VibraniumWall>()] < 1 && !sgaplayer.Shieldbreak && sgaplayer.ConsumeElectricCharge(300, 200, true))
				{
					for (float f = -MathHelper.PiOver4; f <= MathHelper.PiOver4+ (MathHelper.TwoPi / 32f); f += MathHelper.TwoPi / 32f)
					{
						float angle = f + (Main.MouseWorld - player.MountedCenter).ToRotation();
						int proj = Projectile.NewProjectile(player.MountedCenter, angle.ToRotationVector2() * 8f, ModContent.ProjectileType<VibraniumWall>(), 24 + Main.rand.Next(8), 0, player.whoAmI);
						Main.projectile[proj].localAI[1] = Main.rand.NextFloat(-0.5f, 0.5f);
						Main.projectile[proj].ai[1] = angle;
						Main.projectile[proj].netUpdate = true;
						SoundEffectInstance sound = Main.PlaySound(SoundID.Item52, (int)player.MountedCenter.X, (int)player.MountedCenter.Y);
						if (sound != null)
							sound.Pitch = -0.95f;
					}
				}
            }
            else
            {
				sgaplayer.vibraniumSetWall = false;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VibraniumBar"), 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class VibraniumLeggings : Weapons.Vibranium.VibraniumText
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Leggings");
			Tooltip.SetDefault("15% increased movement speed\n20% increased acceleration and max running speed\n25% reduced Electric Consumption and Recharge Delay\nImmunity to low and Distorted Gravity");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = ItemRarityID.Red;
			item.defense = 20;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed *= 1.15f;
			player.accRunSpeed *= 1.2f;
			player.maxRunSpeed *= 1.2f;
			player.SGAPly().electricChargeReducedDelay *= 0.75f;
			player.SGAPly().electricChargeCost *= 0.75f;
			player.gravity = Player.defaultGravity;
			player.buffImmune[BuffID.VortexDebuff] = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VibraniumBar"), 6);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
    public class VibraniumPlatform : ModProjectile
    {
        Effect effect => SGAmod.TrailEffect;
		NoiseGenerator noise;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vibranium Platform");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override bool CanDamage()
        {
            return false;
        }
        public override string Texture => "Terraria/Item_"+ItemID.GemLockRuby;
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.penetrate = 8;
            projectile.alpha = 40;
            projectile.timeLeft = 15;
            projectile.light = 0.75f;
            projectile.ignoreWater = true;
        }
        public override void AI()
		{
			if (noise == default)
            {
				noise = new NoiseGenerator(projectile.Center.GetHashCode());
			}
			Player owner = Main.player[projectile.owner];
			projectile.localAI[0] += 1;
			if (Math.Abs(owner.velocity.Y) > 1f || projectile.ai[0] > 0 || owner.justJumped || !owner.SGAPly().vibraniumSetPlatform || owner.controlDown || owner.SGAPly().Shieldbreak || (!owner.SGAPly().ConsumeElectricCharge(3, 16, true) && projectile.ai[0] < 0))
			{
				projectile.ai[0] += 1;
			}
			else
			{
				projectile.Center = new Vector2(owner.Center.X, owner.Center.Y);
				projectile.timeLeft = 15;
			}
		}

		float ScaleProperty => MathHelper.Clamp(projectile.localAI[0] / 10f, 0f, (projectile.timeLeft+1) / 15f);
		float ScalePropertyColor2 => MathHelper.Clamp(1.5f-(projectile.localAI[0] / 30f), 0f, 1f);

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Player owner = Main.player[projectile.owner];
			List<Vector2> vects = new List<Vector2>();

            for (int i = -96; i <= 96; i += 8)
            {
				float scalar = 1f + Math.Max(Math.Abs(i / 300f) - 0.05f, 0f);
				double scaleoffset = (noise.Noise((int)projectile.Center.X + i,0)*48.00)*(scalar-ScaleProperty);
				vects.Add(new Vector2(projectile.Center.X + i, projectile.Center.Y+(float)scaleoffset));
            }

			for (int j = 0; j < (ScalePropertyColor2>0 ? 2 : 1); j += 1)
			{

				TrailHelper trail = new TrailHelper("BasicEffectPass", j == 0 ? SGAmod.PlatformTex : Main.magicPixel);
				trail.projsize = new Vector2(0, owner.height / 2);
				trail.trailThickness = 4;
				trail.doFade = false;
				if (j == 0)
				{
					trail.color = delegate (float percent)
					{
						return Color.White * MathHelper.Clamp((float)Math.Sin((-0.2f + percent * 1.40f) * MathHelper.Pi) * 3f, 0f, 1f);
					};
                }
                else
                {
					trail.color = delegate (float percent)
					{
						return Color.Lerp(Color.OrangeRed,Color.Purple,0.50f+(float)Math.Sin(((percent*160f)+owner.SGAPly().timer)/10f)*0.50f) * MathHelper.Clamp(((float)Math.Sin((-0.2f + percent * 1.40f) * MathHelper.Pi) * 3f), 0f, 1f) * ScalePropertyColor2;
					};
				}
				trail.coordMultiplier = new Vector2(1f, (96 * 2) / 16f);
				trail.coordOffset = new Vector2(0, projectile.Center.X / 16);
				trail.strength = ScaleProperty;
				trail.trailThicknessIncrease = 0;
				trail.DrawTrail(vects, projectile.Center);
			}

			Texture2D texture = Main.projectileTexture[mod.ProjectileType(this.GetType().Name)];
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

            //spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, origin, new Vector2(1f, 1f), projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }
    }

	public class VibraniumWispMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium's Wisp");
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Starfury);
			aiType = -1;
			projectile.aiStyle = -1;
			projectile.tileCollide = false;
			projectile.melee = false;
			//projectile.minion = true;
			//projectile.minionSlots = 0;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.FallingStar; }
		}

        public override bool CanDamage()
        {
			return false;
        }

        public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.Starfury;
			return true;
		}
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			SGAPlayer sgaply = player.SGAPly();

			projectile.localAI[0] += 1f;

			if (!player.dead && player.armor[0].type == ModContent.ItemType<VibraniumHood>())
            {
				bool playerheldsummon = !player.HeldItem.IsAir && player.HeldItem.summon;

				projectile.timeLeft = 3;
				projectile.damage = (int)((25f*player.minionDamage) * (1f+(player.maxMinions/2f)));

				projectile.ai[0] += 1;

				Vector2 gothere = player.MountedCenter + new Vector2(0, -player.gravDir * 64f);
				if (player.MinionAttackTargetNPC >= 0)
				{
					NPC target = Main.npc[player.MinionAttackTargetNPC];
					gothere = target.Center - Vector2.Normalize(target.Center - player.Center) * 256f;

					if (projectile.ai[0] % (playerheldsummon ? 6 : 10) == 0)
					{
						int index = 0;
						int dist = 400 * 400;

						foreach (NPC npctarget in Main.npc.Where(testby => testby.active && !testby.dontTakeDamage && testby.chaseable && !testby.friendly && (testby.Center - projectile.Center).LengthSquared() < dist).OrderBy(testby => testby.type == player.MinionAttackTargetNPC ? -100 : (testby.Center - player.Center).LengthSquared()))
						{
							if (index > player.maxTurrets)
								continue;

							Vector2 there = npctarget.Center - projectile.Center;

							Projectile.NewProjectile(projectile.Center, ((npctarget.position+new Vector2(Main.rand.Next(0,npctarget.width), Main.rand.Next(0, npctarget.height))) - projectile.Center), ModContent.ProjectileType<VibraniumZapEffect>(), 0, 0);
							SoundEffectInstance snd = Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 91);

							int damazz = (Main.DamageVar((float)projectile.damage));
							npctarget.StrikeNPC(damazz, 0f,player.direction);
							player.addDPS(damazz);

							if (Main.netMode != 0)
							{
								NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npctarget.whoAmI, damazz, 16f, (float)1, 0, 0, 0);
							}

							if (snd != null)
							{
								snd.Pitch = 0.50f;
							}
							index += 1;
						}
					}


				}
				else
				{

					int index = 0;
					int dist = 400 * 400;

					if (projectile.ai[0] % (playerheldsummon ? 5 : 20) == 0)
					{
						foreach (Projectile proj in Main.projectile.Where(testby => testby.active && !testby.friendly && testby.hostile && (testby.Center - player.Center).LengthSquared() < dist && testby.GetGlobalProjectile<SGAprojectile>().damageReduceTime <= 0).OrderBy(testby => (testby.Center - player.Center).LengthSquared()))
						{

							if (!sgaply.ConsumeElectricCharge((int)(proj.damage * (playerheldsummon ? 0.75f : 1.5f)), 60) || index > player.maxTurrets)
								continue;

							SGAprojectile sgaproj = proj.GetGlobalProjectile<SGAprojectile>();
							sgaproj.damageReduce = 10f;
							sgaproj.damageReduceTime = 180;

							Vector2 there = proj.Center - projectile.Center;

							Projectile.NewProjectile(projectile.Center, proj.Center - projectile.Center, ModContent.ProjectileType<VibraniumZapEffect>(), 0, 0);
							SoundEffectInstance snd = Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 91);

							if (snd != null)
							{
								snd.Pitch = 0.50f;
							}

							index += 1;

						}
					}

				}

				Vector2 finalvectormovement = gothere - projectile.Center;
				if (finalvectormovement.Length() > 24f)
				{

					projectile.velocity += (finalvectormovement / 400f) + Vector2.Normalize(finalvectormovement) *0.075f;
				}
				projectile.velocity *= 0.95f;

				return;
            }

			//projectile.timeLeft = 1;
			//projectile.active = false;

			
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			NPCs.ResonantWisp.DrawResonantWisp(projectile,projectile.Center,projectile.localAI[0],spriteBatch,lightColor);
			return false;
        }
    }

	public class VibraniumZapEffect : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium's Zap");
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Starfury);
			aiType = -1;
			projectile.aiStyle = -1;
			projectile.timeLeft = 8;
			projectile.tileCollide = false;
			projectile.melee = false;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_591"; }
		}

		public override bool CanDamage()
		{
			return false;
		}
		public override void AI()
		{
			projectile.position -= projectile.velocity;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];

			spriteBatch.Draw(tex, projectile.Center-Main.screenPosition, null, Color.White*MathHelper.Clamp(projectile.timeLeft/8f,0f,1f), projectile.velocity.ToRotation()+MathHelper.PiOver2, new Vector2(tex.Width / 2f, tex.Height), new Vector2(1f, projectile.velocity.Length()/tex.Height), SpriteEffects.None, 0);

			return false;
		}
	}


		public class VibraniumWall : ModProjectile
	{
		Player Owner;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Wall");
		}
		public override bool CanDamage()
		{
			return false;
		}
		public override string Texture => "SGAmod/Projectiles/VibraniumWall";
		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = 8;
			projectile.alpha = 40;
			projectile.timeLeft = 45;
			projectile.light = 0.75f;
			projectile.ignoreWater = true;
		}
		public virtual void Orbit()
        {
			float dist = 80 + (float)Math.Sin((projectile.localAI[0]+(projectile.whoAmI*67f))/20f)*projectile.damage;
			Vector2 pos = Owner.MountedCenter + (projectile.ai[1].ToRotationVector2()* dist);
			projectile.Center += Owner.velocity+(pos - projectile.Center)/8f;
			SGAmod.vibraniumCounter = 3;

		}
		public virtual bool DoCheck()
        {
			Owner = Main.player[projectile.owner];
			return Owner.SGAPly().vibraniumSet && Owner.SGAPly().vibraniumSetWall;
		}
		public override void AI()
		{
			projectile.rotation += projectile.localAI[1];

			Player owner = Main.player[projectile.owner];
			projectile.localAI[0] += 1;
			if (projectile.ai[0] > 0 || !DoCheck())
			{
				projectile.ai[0] += 1;
			}
			else
			{
				Orbit();
				projectile.timeLeft = 45;
			}
		}
		float ScaleProperty => MathHelper.Clamp(projectile.localAI[0] / 10f, 0f, (projectile.timeLeft + 1) / 45f);
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player owner = Main.player[projectile.owner];

			Texture2D texture = Main.projectileTexture[mod.ProjectileType(this.GetType().Name)];
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White* ScaleProperty, projectile.rotation, origin, new Vector2(1f, 1f), SpriteEffects.None, 0f);
			return false;
		}
	}

	public class VibraniumThrownExplosion : Explosion
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magnetic Explosion");
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 30;
			projectile.Throwing().thrown = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}
        public override bool CanDamage()
        {
			return false;
        }

        public override void AI()
        {
			projectile.ai[0] += 1;

			if (projectile.ai[0] == 3)
            {
				Player owner = Main.player[projectile.owner];
				NPC enemy = null;
				List<NPC> npcx = SGAUtils.ClosestEnemies(projectile.Center, 320*owner.Throwing().thrownVelocity, projectile.Center);
				if (npcx != null && npcx.Count>0)
				enemy = npcx[0];

				if (enemy != null)
				{
					Vector2 aimvelo = Vector2.Normalize(enemy.Center - projectile.Center)*32f;
					int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, aimvelo.X, aimvelo.Y, ProjectileID.MagnetSphereBolt, projectile.damage, 0, projectile.owner, 0f, 0f);
					Main.projectile[proj].magic = false;
					Main.projectile[proj].netUpdate = true;
				}
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Texture2D star = Main.itemTexture[ModContent.ItemType<StygianCore>()];
			float colorfade = MathHelper.Clamp(projectile.ai[0] / 5f, 0f, Math.Min(projectile.timeLeft / 10f, 1f));
			spriteBatch.Draw(star, projectile.Center-Main.screenPosition, null, Color.Red* colorfade, 0, star.Size()/2f, projectile.timeLeft / 80f, SpriteEffects.None, 0f);
			colorfade = MathHelper.Clamp(projectile.ai[0] / 20f, 0f, Math.Min(projectile.timeLeft / 30f, 1f));
			spriteBatch.Draw(star, projectile.Center - Main.screenPosition, null, Color.Blue * colorfade, 0, star.Size() / 2f, 0.25f+(projectile.timeLeft / 160f), SpriteEffects.None, 0f);

			return false;
        }

	}
}