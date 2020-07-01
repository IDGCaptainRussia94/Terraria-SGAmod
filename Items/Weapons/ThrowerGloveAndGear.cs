using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using SGAmod.NPCs.Sharkvern;
using AAAAUThrowing;

namespace SGAmod.Items.Weapons
{
	class ThrowerGlove : ModItem
	{
		public static string disc = "\nMay also be worn in place of a Grapple Hook to throw grenades with the grapple key\nHowever, the grenades are slower and has a cooldown";
		public virtual int level => 0;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Grenadier's Glove");
			Tooltip.SetDefault("Throws hand grenades further, and increases their damage"+ disc);
		}

		public static int FindGrenadeToThrow(Mod mod,Player player, int level)
		{
		List<int> grenadetypes = new List<int>();
			grenadetypes.Add(ItemID.Grenade);
			grenadetypes.Add(ItemID.BouncyGrenade);
			grenadetypes.Add(ItemID.StickyGrenade);
			grenadetypes.Add(ItemID.PartyGirlGrenade);
			grenadetypes.Add(ItemID.Beenade);
			grenadetypes.Add(mod.ItemType("AcidGrenade"));
			grenadetypes.Add(mod.ItemType("ThermalGrenade"));
			if ((level != 2 && level>0) || level == 3)
			{
				grenadetypes.Add(mod.ItemType("CelestialCocktail"));
				grenadetypes.Add(ItemID.MolotovCocktail);
				grenadetypes.Add(ItemID.Bone);
				grenadetypes.Add(ItemID.Ale);
				grenadetypes.Add(ItemID.SpikyBall);
				grenadetypes.Add(mod.ItemType("ThrowableTrapSpikyball"));
			}

			if (level == 2 || level == 3)
			{
				grenadetypes.Add(ItemID.Dynamite);
				grenadetypes.Add(ItemID.BombFish);
				grenadetypes.Add(ItemID.BouncyDynamite);
				grenadetypes.Add(ItemID.StickyDynamite);
				grenadetypes.Add(ItemID.Bomb);
				grenadetypes.Add(ItemID.StickyBomb);
				grenadetypes.Add(ItemID.BouncyBomb);
			}

			for (int i = 0; i < 58; i++)
			{
				int type = grenadetypes.Find(x => x == player.inventory[i].type);
				if (type > 0 && player.inventory[i].stack > 0)
				{
					return player.inventory[i].type;
				}
			}
			return -1;


		}
		public static int FindProjectile(int proj,int weapon)
		{
			if (proj == ProjectileID.Bone)
				proj = ProjectileID.BoneGloveProj;
			if (weapon == ItemID.AleThrowingGlove || proj == ItemID.AleThrowingGlove)
				proj = ProjectileID.Ale;
			return proj;

		}

		public static int FindItem(int weapon)
		{
			if (weapon == ItemID.Ale)
				weapon = ItemID.AleThrowingGlove;
			return weapon;

		}

		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.damage = 4;
			item.shootSpeed = 5f;
			item.shoot = ProjectileID.Grenade;
			item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			item.width = 24;
			item.height = 24;
			item.maxStack = 1;
			item.knockBack = 9;
			item.consumable = false;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 60;
			item.useTime = 60;
			//item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<GrenadeNotAHook>();
			item.value = Item.buyPrice(0, 1, 50, 0);
			item.rare = 3;
		}

		public override bool CanUseItem(Player player)
		{
			return (ThrowerGlove.FindGrenadeToThrow(mod,player, level) >0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int basetype = ThrowerGlove.FindGrenadeToThrow(mod, player, level);
			//if (player.CountItem(mod.ItemType("AcidGrenade"))>0)
			//basetype = mod.ItemType("AcidGrenade");

			Vector2 basespeed = new Vector2(speedX, speedY);
			float speedbase = basespeed.Length()*player.Throwing().thrownVelocity;

			basespeed.Normalize();

			Item basetype2 = new Item();
			int itemtype = FindItem(basetype);
			basetype2.SetDefaults(itemtype);
			float baseumtli = (item.useTime/player.GetModPlayer<SGAPlayer>().ThrowingSpeed) /60f;
			if (itemtype == ItemID.Beenade)
				baseumtli = 1f;
			player.itemAnimation = (int)(basetype2.useAnimation* baseumtli);
			player.itemAnimationMax = (int)(basetype2.useAnimation* baseumtli);
			player.itemTime = (int)(basetype2.useTime* baseumtli);
			type = FindProjectile(basetype2.shoot, basetype);

			if (itemtype == mod.ItemType("ThrowableTrapSpikyball"))
				speedbase /= 1f;

			basespeed *= (basetype2.shootSpeed + speedbase);
			speedX = basespeed.X;
			speedY = basespeed.Y;
			if (type!=ProjectileID.Beenade)
			damage += (int)((float)basetype2.damage * player.Throwing().thrownDamage);

			player.ConsumeItem(basetype);

			Projectile proj = Main.projectile[Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI)];
			proj.Throwing().thrown = true;
			proj.ranged = false;
			proj.friendly = true;
			proj.hostile = false;
			proj.netUpdate = true;
			IdgProjectile.Sync(proj.whoAmI);

			return false;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/GLOOOVE_FINAL123"); }
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 8);
			recipe.AddIngredient(ItemID.BeeWax, 10);
			recipe.AddIngredient(ItemID.TatteredCloth, 5);
			recipe.AddRecipeGroup("SGAmod:Tier2Bars", 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 8);
			recipe.AddIngredient(ItemID.BeeWax, 10);
			recipe.AddIngredient(ItemID.Leather, 10);
			recipe.AddRecipeGroup("SGAmod:Tier2Bars", 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	class ThrowerGloveMK2 : ThrowerGlove
	{
		public override int level => 1;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Rioter's Glove");
			Tooltip.SetDefault("Throws hand grenades further, and increases their damage\nUpgraded to now throw Ale, Molotovs, Spiky balls, and Bones!\n" + disc);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.useStyle = 1;
			item.damage = 5;
			item.shoot = ModContent.ProjectileType<GrenadeNotAHook2>();
			item.shootSpeed = 5.5f;
			item.value = Item.buyPrice(0, 2, 50, 0);
			item.rare = 4;
			item.expert = true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/MolotovGlove"); }
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ThrowerGlove"), 1);
			recipe.AddIngredient(ItemID.AleThrowingGlove, 1);
			recipe.AddIngredient(ItemID.BoneGlove, 1);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	class ThrowerGloveDynamite : ThrowerGlove
	{
		public override int level => 2;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Demolitionist's Glove");
			Tooltip.SetDefault("Throws hand grenades further, and increases their damage\nUpgraded to now throw Bombs and Dynamite!\n" + disc);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.useStyle = 1;
			item.damage = 4;
			item.shoot = ModContent.ProjectileType<GrenadeNotAHook3>();
			item.shootSpeed = 5.5f;
			item.value = Item.buyPrice(0, 2, 50, 0);
			item.rare = 4;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/DynamiteGlove"); }
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ThrowerGlove"), 1);
			recipe.AddIngredient(ItemID.Dynamite, 10);
			recipe.AddIngredient(ItemID.StickyBomb, 20);
			recipe.AddRecipeGroup("SGAmod:Tier3Bars", 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}


	class GucciGauntlet : ThrowerGlove
	{
		public override int level => 3;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Gucci Gauntlet");
			Tooltip.SetDefault("'ah shit... here we go again'-CJ\nThrows everything previous gloves can\nGains extra powers when worn as the player gains Large Gems" + disc);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.useStyle = 1;
			item.damage = 11;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<GrenadeNotAHook4>();
			item.shootSpeed = 5.75f;
			item.useAnimation = 50;
			item.useTime = 50;
			item.value = Item.buyPrice(1, 0, 0, 0);
			item.rare = ItemRarityID.Quest;
			item.expert = true;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Weapons/IDontFeelSoGucci");
				item.GetGlobalItem<ItemUseGlow>().angleAdd = MathHelper.ToRadians(45f);
				item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Main.hslToRgb((Main.GlobalTime * 2.5f) % 1f, 0.9f, 0.75f)*MathHelper.Clamp((float)Math.Sin(((float)player.itemAnimation/ (float)player.itemAnimationMax)/2f)*3f,0f,1f);
				};
			}
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/IDontFeelSoGucci"); }
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
			{

				if (Main.LocalPlayer.ownedLargeGems[0])
					tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.Purple, "Amethyst Gem grants you a chance of Shadowflames on all your attacks")));
				if (Main.LocalPlayer.ownedLargeGems[1])
					tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.Yellow, "Topaz Gem increases your mining speed")));
				if (Main.LocalPlayer.ownedLargeGems[2])
					tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.Blue, "Saphire Gem grants unrestricted movement in water")));
				if (Main.LocalPlayer.ownedLargeGems[3])
					tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.Green, "Emerald Gem grants 1 minion slot and persistant Dryad's Blessing")));
				if (Main.LocalPlayer.ownedLargeGems[4])
					tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.Crimson, "Ruby Gem gives your attacks a very low chance to life steal")));
				if (Main.LocalPlayer.ownedLargeGems[5])
					tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.LightSlateGray, "Diamond Gem provides a minor boost to all stats")));
				if (Main.LocalPlayer.ownedLargeGems[6])
					tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.Goldenrod, "Amber Gem reduces the cost of all items in shops")));
			}

			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "When you have all 7 gems, Equip the glove and throw a 'grapple hook' to 'snap'")));
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "Snapping will erase half the enemies and hostile projectiles, with a cooldown")));
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
			{
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "Snapping will erase enemies with less than 100000 HP times the average of your damage bonuses")));
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "100000 * ((Melee+Ranged+Magic+Summon+Throwing)/5)")));
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "If an enemy's life exceeds this they will will receive the fading debuff")));
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "This debuff does massive damage over time, and fades enemies out of existance when they run out of life")));
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "Bosses will never be able to be erased noramlly")));
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "Enemies who fade out of existance do not drop anything or count as being killed")));

			}
			else
			{
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "However this power is 'Balanced', Hold the Control key for more details")));
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ThrowerGloveMK2"), 1);
			recipe.AddIngredient(mod.ItemType("ThrowerGloveDynamite"), 1);
			recipe.AddRecipeGroup("Fragment", 15);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(mod.ItemType("DrakeniteBar"), 12);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	internal class GrenadeNotAHook : ModProjectile
	{
		public virtual int level => 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/AcidGrenade"); }
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			player.GetModPlayer<SGAPlayer>().greandethrowcooldown = 120;
			int basetype = ThrowerGlove.FindGrenadeToThrow(mod, player, level);
			//if (player.CountItem(mod.ItemType("AcidGrenade"))>0)
			//basetype = mod.ItemType("AcidGrenade");

			Vector2 basespeed = (projectile.velocity/2f);
			float speedbase = basespeed.Length() * player.Throwing().thrownVelocity;
			basespeed.Normalize();

			Item basetype2 = new Item();
			basetype2.SetDefaults(ThrowerGlove.FindItem(basetype));
			player.itemTime = basetype2.useTime;
			int type = ThrowerGlove.FindProjectile(basetype2.shoot, basetype);
			basespeed *= (basetype2.shootSpeed + speedbase);
			int damage = (int)(basetype2.damage * player.Throwing().thrownDamage);

			int proj=Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, basespeed.X, basespeed.Y, type, damage, basetype2.knockBack, player.whoAmI);
			Main.projectile[proj].friendly = true;
			Main.projectile[proj].hostile = false;
			Main.projectile[proj].netUpdate = true;
			IdgProjectile.Sync(proj);

			player.ConsumeItem(basetype);

			projectile.Kill();
		}

		// Use this hook for hooks that can have multiple hooks mid-flight: Dual Hook, Web Slinger, Fish Hook, Static Hook, Lunar Hook
		public override bool? CanUseGrapple(Player player)
		{
			return (player.GetModPlayer<SGAPlayer>().greandethrowcooldown<1 && ThrowerGlove.FindGrenadeToThrow(mod, player, level) >-1);
		}

	}

	internal class GrenadeNotAHook2 : GrenadeNotAHook
	{
		public override int level => 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/AcidGrenade"); }
		}

	}

	internal class GrenadeNotAHook3 : GrenadeNotAHook
	{
		public override int level => 2;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/AcidGrenade"); }
		}

	}

	internal class GrenadeNotAHook4 : GrenadeNotAHook
	{
		public override int level => 3;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void AI()
		{
			Player owner = Main.player[projectile.owner];
			if (SGAWorld.SnapCooldown < 1)
			{
				if (owner.ownedLargeGems[0] && owner.ownedLargeGems[1] && owner.ownedLargeGems[2] && owner.ownedLargeGems[3] && owner.ownedLargeGems[4] && owner.ownedLargeGems[5] && owner.ownedLargeGems[6])
				{
					float basevalues = (owner.meleeDamage + owner.magicDamage + owner.minionDamage + owner.rangedDamage + owner.Throwing().thrownDamage)/5f;
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, ModContent.ProjectileType<GucciSnap>(), (int)(100000f*basevalues), 10f, projectile.owner);
					projectile.Kill();
					SGAWorld.SnapCooldown = 60*300;
					if (Main.netMode != NetmodeID.SinglePlayer)
					{
						ModPacket packet = SGAmod.Instance.GetPacket();
						packet.Write((int)105);
						packet.Write(60 * 300);
						packet.Send();
					}
					return;
				}
			}
			base.AI();
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
			projectile.tileCollide = false;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/AcidGrenade"); }
		}

	}

	class GucciSnap : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Snap");
		}

		public override void AI()
		{
			Player owner = Main.player[projectile.owner];
			projectile.ai[0] += 1;
			if (projectile.ai[0] == 2)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Snap").WithVolume(1.0f).WithPitchVariance(.25f), projectile.Center);
				RippleBoom.MakeShockwave(projectile.Center, 5f, 1f, 10f, 120, 1f, true);
				CombatText.NewText(new Rectangle((int)owner.position.X, (int)owner.position.Y, owner.width, owner.height), Color.Red, "snap...", true, false);
			}
			if (projectile.ai[0] == 15)
			{
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Fade").WithVolume(1.0f).WithPitchVariance(.25f), new Vector2(-1, -1));
				for(int i = 0; i < Main.maxNPCs; i += 1)
				{
					NPC npc = Main.npc[i];
					if (npc != null && npc.active)
					{
						if (!npc.friendly && !npc.townNPC && !npc.immortal && !npc.dontTakeDamage && npc.realLife<1)
						{
							if (Main.rand.Next(0, 1) == 0)
							{
								if (npc.life < projectile.damage && !npc.boss)
								{
									npc.GetGlobalNPC<SGAnpcs>().Snapped = 200;
								}
								else
								{
									IdgNPC.AddBuffBypass(npc.whoAmI,mod.BuffType("SnapFade"), (int)((float)projectile.damage / 500f));
								}
							}

						}
					}


				}


			}

		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Grenade);
			projectile.timeLeft = 300;
			projectile.aiStyle = -1;
			projectile.width = 24;
			projectile.height = 24;
			projectile.tileCollide = false;
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.HerbBag); }
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

	}

		class AcidGrenade : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Acid Grenade");
			Tooltip.SetDefault("Deals Acid Burn to all affected, but does not last long before exploding");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Grenade);
			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.damage = 65;
			item.shootSpeed = 3f;
			item.shoot = mod.ProjectileType("AcidGrenadeProj");
			item.value = Item.buyPrice(0, 0, 2, 0);
			item.rare = 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Grenade, 15);
			recipe.AddIngredient(mod.ItemType("VialofAcid"), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this,15);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			return true;
		}

	}

		class ThermalGrenade : AcidGrenade
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Thermal Grenade");
			Tooltip.SetDefault("Deals Thermal Blaze to all affected");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Grenade);
			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.damage = 72;
			item.shootSpeed = 3f;
			item.shoot = mod.ProjectileType("ThermalGrenadeProj");
			item.value = Item.buyPrice(0, 0, 2, 0);
			item.rare = 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Grenade, 15);
			recipe.AddIngredient(mod.ItemType("FieryShard"), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 15);
			recipe.AddRecipe();
		}

	}

	class AcidGrenadeProj : ModProjectile
	{

		bool hitonce = false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Acid Grenade");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Grenade);
			projectile.Throwing().thrown = true;
			projectile.timeLeft = 180;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/AcidGrenade"); }
		}

		public override bool PreKill(int timeLeft)
		{
			if (!hitonce)
			{
				projectile.width = 160;
				projectile.height = 160;
				projectile.position -= new Vector2(80, 80);
			}

			for (int i = 0; i < 100; i += 1)
			{
				float randomfloat = Main.rand.NextFloat(1f, 6f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();

				int dust = Dust.NewDust(new Vector2(projectile.Center.X - 32, projectile.Center.Y - 32), 64, 64, mod.DustType("AcidDust"));
				Main.dust[dust].scale = 2.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (projectile.velocity * (float)(Main.rand.Next(10, 20) * 0.01f)) + (randomcircle * randomfloat);
			}

			int theproj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("Explosion"), (int)((double)projectile.damage * 1f), projectile.knockBack, projectile.owner, 0f, 0f);
			Main.projectile[theproj].Throwing().thrown = projectile.magic;
			IdgProjectile.AddOnHitBuff(theproj, mod.BuffType("AcidBurn"), 120);

			projectile.velocity = default(Vector2);
			projectile.type = ProjectileID.Grenade;
			return true;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("AcidDust"));
			Main.dust[dust].scale = 0.75f;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			projectile.timeLeft -= 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!hitonce)
			{
				hitonce = true;
				projectile.position -= new Vector2(64, 64);
				projectile.width = 160;
				projectile.height = 160;
				projectile.timeLeft = 1;
			}
			//projectile.Center -= new Vector2(48,48);

			target.AddBuff(mod.BuffType("AcidBurn"), 120);
		}

	}

	class ThermalGrenadeProj : AcidGrenadeProj
	{

		bool hitonce = false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Thermal Grenade");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Grenade);
			projectile.Throwing().thrown = true;
			projectile.timeLeft = 240;
		}

	public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/ThermalGrenade"); }
		}

		public override bool PreKill(int timeLeft)
		{
			if (!hitonce)
			{
				projectile.width = 160;
				projectile.height = 160;
				projectile.position -= new Vector2(80, 80);
			}

			for (int i = 0; i < 100; i += 1)
			{
				float randomfloat = Main.rand.NextFloat(1f, 6f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();

				int dust = Dust.NewDust(new Vector2(projectile.Center.X - 32, projectile.Center.Y - 32), 64, 64, mod.DustType("HotDust"));
				Main.dust[dust].scale = 2.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (projectile.velocity * (float)(Main.rand.Next(10, 20) * 0.01f)) + (randomcircle * randomfloat);
			}

			int theproj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("Explosion"), (int)((double)projectile.damage * 1f), projectile.knockBack, projectile.owner, 0f, 0f);
			Main.projectile[theproj].Throwing().thrown = projectile.magic;
			IdgProjectile.AddOnHitBuff(theproj, mod.BuffType("ThermalBlaze"), 60 * 3);

			projectile.velocity = default(Vector2);
			projectile.type = ProjectileID.Grenade;
			return true;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"));
			Main.dust[dust].scale = 0.75f;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!hitonce)
			{
				hitonce = true;
				projectile.position -= new Vector2(64, 64);
				projectile.width = 128;
				projectile.height = 128;
				projectile.timeLeft = 1;
			}
			//projectile.Center -= new Vector2(48,48);

			target.AddBuff(mod.BuffType("ThermalBlaze"), 60*4);
		}



	}

	class CelestialCocktail : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Cosmic Cocktail");
			Tooltip.SetDefault("'Made from the sweet celestial essence of the lunar slime princess'\nExplodes violently, sending celestial projectiles in the general direction the projectile was traveling");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.MolotovCocktail);
			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.damage = 50;
			item.useTime = 40;
			item.useAnimation = 40;
			item.maxStack = 999;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("CelestialCocktailProj");
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MolotovCocktail, 75);
			recipe.AddRecipeGroup("Fragment", 3);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 2);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 75);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 10);
			recipe.AddIngredient(ItemID.Torch, 10);
			recipe.AddIngredient(ItemID.Ale, 50);
			recipe.AddRecipeGroup("Fragment", 1);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 150);
			recipe.AddRecipe();

		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			return true;
		}

	}

	class CelestialCocktailProj : ModProjectile
	{

		bool hitonce = false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Celestial Cocktail");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Grenade);
			projectile.thrown = true;
			projectile.timeLeft = 180;
		}

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/CelestialCocktail"); }
		}

		public override bool? CanHitNPC(NPC target)
		{
			int player = projectile.owner;
			return base.CanHitNPC(target);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			if (!hitonce)
			{
				projectile.width = 128;
				projectile.height = 128;
				projectile.position -= new Vector2(64, 64);
			}

			Vector2 gotohere = projectile.velocity;
			gotohere.Normalize();

			int[] projectiletype = { ProjectileID.NebulaBlaze1, ProjectileID.VortexBeaterRocket, ProjectileID.HeatRay, ProjectileID.DD2LightningBugZap };
			float[] projectiledamage = { 1f, 1f, 2.5f, 0.25f };
			int[] projectilecount = { 9, 9,12,7 };
			for (int zz = 0; zz < 4; zz += 1)
			{
				for (int i = 0; i < projectilecount[zz]; i += 1)
				{
					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(120)) * Main.rand.NextFloat(6, 12);
					int proj = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), projectiletype[zz], (int)((projectile.damage * 1f)* projectiledamage[zz]), projectile.knockBack / 10f, projectile.owner);
					Main.projectile[proj].Throwing().thrown = true;
					Main.projectile[proj].magic = false;
					Main.projectile[proj].ranged = false;
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].velocity = new Vector2(perturbedSpeed.X, perturbedSpeed.Y);
					Main.projectile[proj].hostile = false;
					if (i != 2)
					{
						Main.projectile[proj].timeLeft = 300;
						Main.projectile[proj].penetrate = 4;
					}
					if (zz==3)
					Main.projectile[proj].penetrate = 15;
					projectile.netUpdate = true;
					IdgProjectile.Sync(proj);
				}

			}

			int theproj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("SlimeBlast"), (int)((double)projectile.damage * 1.5f), projectile.knockBack, projectile.owner, 0f, 0f);
			Main.projectile[theproj].Throwing().thrown = true;

			projectile.velocity = default(Vector2);
			projectile.type = ProjectileID.Grenade;
			return true;
		}

		public override bool PreAI()
		{
			
				for (int zz = 0; zz < Main.maxNPCs; zz += 1)
				{
				NPC npc = Main.npc[zz];
				if (!npc.dontTakeDamage && !npc.townNPC  && npc.active && npc.life>0)
				{
					Rectangle rech = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
					Rectangle rech2 = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
					if (rech.Intersects(rech2))
					{
						projectile.Damage();
						projectile.Kill();

					}
				}
			}

			return true;
		}

		public override void AI()
		{

				int[] dustype = { mod.DustType("AcidDust"), mod.DustType("HotDust"), mod.DustType("MangroveDust"), mod.DustType("TornadoDust") };
			int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, dustype[Main.rand.Next(0,4)]);
			Main.dust[dust].scale = 0.75f;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			projectile.timeLeft -= 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!hitonce)
			{
				hitonce = true;
				projectile.position -= new Vector2(64, 64);
				projectile.width = 128;
				projectile.height = 128;
				projectile.timeLeft = 1;
			}
			//projectile.Center -= new Vector2(48,48);
		}

	}

	class JarateShurikens : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Jarate Shuriken");
			Tooltip.SetDefault("Throws a fan of 5 'yellow' Snowballs outwards\nThese inflict Ichored and Sodden\nRequires and consumes 5 of the item per use");
		}

		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.damage = 18;
			item.shootSpeed = 9f;
			item.shoot = mod.ProjectileType("JarateShurikensProg");
			item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			item.width = 8;
			item.height = 28;
			item.knockBack = 6;
			item.maxStack = 999;
			item.consumable = false;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 30;
			item.useTime = 30;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.value = Item.buyPrice(0, 0, 1, 0);
			item.rare = 4;
			item.ammo = AmmoID.Snowball;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int i = 0; i < 4; i += 1)
			{
				player.ConsumeItem(mod.ItemType(GetType().Name));
			}

			int numberProjectiles = 5;
			float rotation = MathHelper.ToRadians(20);
			for (int i = 0; i < numberProjectiles; i += 1)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY)).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)i / ((float)numberProjectiles - 1f)));
				int thisoned = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, Main.myPlayer);
			}


			return false;
		}

		public override bool CanUseItem(Player player)
		{
			return player.CountItem(mod.ItemType(GetType().Name))>4;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Snowball, 100);
			recipe.AddIngredient(mod.ItemType("Jarate"), 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();

		}

	}

	public class JarateShurikensProg : ModProjectile
	{

		int fakeid = ProjectileID.SnowBallFriendly;
		public int guyihit = -10;
		public int cooldown = -10;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Coin");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/JarateShurikens"); }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Ichor, 60 * 10);
			if (Main.rand.Next(0, 5) == 1)
			{
				if (Main.player[projectile.owner].GetModPlayer<SGAPlayer>().MVMBoost)
					target.AddBuff(mod.BuffType("SoddenSlow"), 60 * 4);
				target.AddBuff(mod.BuffType("Sodden"), 60 * 4);
			}
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.SnowBallFriendly);
			projectile.Throwing().thrown = true;
			projectile.ranged = false;
			projectile.tileCollide = true;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.extraUpdates = 1;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = fakeid;

			for (int num654 = 0; num654 < 16; num654++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
				int num655 = Dust.NewDust(projectile.position + Vector2.UnitX * -20f, projectile.width + 40, projectile.height, 75, -projectile.velocity.X + randomcircle.X * 4f, -projectile.velocity.Y + randomcircle.Y * 4f, 150, Color.Goldenrod, 0.8f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].noLight = true;
			}

			return true;
		}

		public override void AI()
		{


		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				Color color = projectile.GetAlpha(lightColor) * ((float)projectile.oldPos.Length / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color*0.5f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}


	}

	class SharkBait : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Shark Bait");
			Tooltip.SetDefault("'Contains many yummy snacks a Sharvern needs in their diet!'\nThrows a bag of Shark Bait, which erupts into fish\nFish may spawn hungry sharks\nProduces more fish when thrown into water\nDoubles as fishing bait");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.MolotovCocktail);
			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.damage = 50;
			item.useTime = 40;
			item.useAnimation = 40;
			item.bait = 60;
			item.maxStack = 999;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("SharkFoodProj");
			item.value = Item.buyPrice(0, 0, 5, 0);
			item.rare = 6;
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.HerbBag); }
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			return true;
		}

	}

	class SharkFoodProj : ModProjectile
	{

		bool hitonce = false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Shark Food");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Grenade);
			projectile.Throwing().thrown = true;
			projectile.timeLeft = 600;
			projectile.aiStyle = -1;
			projectile.width = 24;
			projectile.height = 24;
		}

		public override string Texture
		{
			get { return ("Terraria/Item_"+ItemID.HerbBag); }
		}

		public override bool? CanHitNPC(NPC target)
		{
			int player = projectile.owner;
			return base.CanHitNPC(target);
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return false;
		}

		public override bool PreKill(int timeLeft)
		{

			int theproj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("SharkFoodProj3"),projectile.damage, timeLeft>8000 ? 2 : 1, projectile.owner, 0f, 0f);
			return true;
		}

		public override void AI()
		{
			Tile tile = Main.tile[(int)projectile.Center.X / 16, (int)projectile.Center.Y / 16];
			if (tile != null)
			{
				if (tile.liquid > 64)
				{
					projectile.timeLeft = 9999;
					projectile.Kill();
				}
			}

			int[] dustype = {DustID.Blood};
			int dust = Dust.NewDust(new Vector2(projectile.Center.X-16, projectile.Center.Y-16), 32, 32, dustype[Main.rand.Next(0, dustype.Length)]);
			Main.dust[dust].scale = 0.75f;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			projectile.timeLeft -= 1;
			projectile.velocity.Y += 0.25f;
			projectile.velocity.X *= 0.98f;
			projectile.rotation += projectile.velocity.Length() * (float)(Math.Sign(projectile.velocity.X*1000f)/1000f)*10f;
		}

	}

	public class SharkFoodProj3 : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yum Yum!");
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.HerbBag); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 72;
			projectile.height = 24;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.Throwing().thrown = true;
			projectile.extraUpdates = 0;
			projectile.timeLeft = 60;
			projectile.tileCollide = false;
			projectile.aiStyle = -1;

		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void AI()
		{
			if (projectile.timeLeft == 30)
			{
				for (int i = 0; i < 20; i += 1)
				{
					int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33);
					Main.dust[dust2].scale = 2.5f;
					Main.dust[dust2].noGravity = false;
					Main.dust[dust2].velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-12f, 1f));
				}
				for (float xx = 5f; xx < 20f; xx += projectile.knockBack>1 ? 1f : 1.75f)
				{
					int proj2 = Projectile.NewProjectile(projectile.Center, new Vector2(Main.rand.NextFloat(-5f, 5f), -Main.rand.NextFloat(0, xx)), mod.ProjectileType("SharkFoodProj2"), projectile.damage, 0f,projectile.owner,255);
					Main.projectile[proj2].friendly = false;
					Main.projectile[proj2].hostile = true;
					Main.projectile[proj2].ai[0] = Main.rand.Next(40, 100);
					Main.projectile[proj2].netUpdate = true;
				}
				Main.PlaySound(19, (int)projectile.position.X, (int)projectile.position.Y, 1, 1f, 0.25f);
			}

			for (int i = 0; i < 3; i += 1)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-1f, 1f));
			}
		}

	}


	public class SharkFoodProj2 : RandomOceanCrap
	{

		int fakeid = ProjectileID.FrostShard;
		int fishtype = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yum Yum!");
		}

		public override bool CanDamage()
		{
			return projectile.velocity.Y > -3f && projectile.ai[1]>2;
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.velocity.Y <= -3f)
				return false;
			return base.CanHitNPC(target);
		}

		public override void AI()
		{
			base.AI();
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.ai[1] += 1;
			if (projectile.ai[1] > projectile.ai[0])
			{
				projectile.ai[1] = -999;
				int proj2 = Projectile.NewProjectile(projectile.Center+ new Vector2(Math.Sign(projectile.velocity.X) * 150,42), new Vector2(-Math.Sign(projectile.velocity.X)*12,-2.25f), mod.ProjectileType("FlyingSharkProj"), projectile.damage, 4, projectile.owner);
			}
			if (projectile.velocity.Y > 0)
				projectile.tileCollide = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.CloneDefaults(fakeid);
			projectile.width = 32;
			projectile.height = 32;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.Throwing().thrown = true;
			projectile.extraUpdates = 0;
			projectile.tileCollide = false;
			projectile.aiStyle = -1;
			int[] types = { ItemID.Fish, ItemID.Trout, ItemID.TundraTrout, ItemID.ReaverShark, ItemID.Goldfish, ItemID.Ebonkoi, ItemID.MirageFish, ItemID.PrincessFish, ItemID.FrostDaggerfish };
			fishtype = types[Main.rand.Next(types.Length)];
		}

	}

	public class FlyingSharkProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shark");
		}

		public override string Texture
		{
			get { return ("Terraria/NPC_" + NPCID.Shark); }
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.WaterBolt);
			projectile.extraUpdates = 0;
			projectile.width = 72;
			projectile.height = 42;
			projectile.aiStyle = -1;
			projectile.timeLeft = 180;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.Throwing().thrown = true;
			projectile.scale = 1f;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33);
			Main.dust[dust].scale = 2f;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-1f, 1f));

			projectile.localAI[0] += 1;
			projectile.velocity.Y += 0.1f;
			if (Collision.SolidCollision(projectile.position,projectile.width,projectile.height))
				projectile.timeLeft = Math.Min(projectile.timeLeft, 30);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = ModContent.GetTexture("Terraria/NPC_"+NPCID.Shark);
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
			Color color = projectile.GetAlpha(lightColor) * 1f; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = (int)(projectile.localAI[0] / 8f);
			timing %= 4;
			timing *= ((tex.Height) / 4);
			float yspeed = projectile.velocity.Y;
			if (Math.Abs(projectile.velocity.Y) > 2f)
			{
				yspeed = (Math.Sign(yspeed) * 2f) + yspeed / 5f;
			}
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 4), 
				color*MathHelper.Clamp((float)projectile.timeLeft/30f,0f,(float)Math.Min(projectile.localAI[0]/15f,1f)),yspeed * (0.15f* Math.Sign(projectile.velocity.X))
				, drawOrigin, projectile.scale, projectile.velocity.X<1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			return false;
		}

	}

	class NinjaBombProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Ninja Bomb");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.SmokeBomb);
			projectile.thrown = true;
			projectile.timeLeft = 200;
			projectile.damage = 0;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_"+ProjectileID.SmokeBomb); }
		}

		public override bool PreKill(int timeLeft)
		{
			for (float i = 0f; i < 12f; i += 0.1f)
			{
				Vector2 circle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
				circle=circle.SafeNormalize(Vector2.Zero);
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Smoke, circle.X*i, circle.Y * i);
				Main.dust[dust].scale = Main.rand.NextFloat(2f,6f);
				Main.dust[dust].noGravity = false;
				Main.dust[dust].alpha = 100;
				Main.dust[dust].velocity = circle * i;
			}

			for (int num172 = 0; num172 < Main.maxNPCs; num172 += 1)
			{
				NPC target = Main.npc[num172];
				float damagefalloff = 1f - ((target.Center - projectile.Center).Length() / 256f);
				if ((target.Center - projectile.Center).Length() < 256f && !target.friendly && !target.dontTakeDamage)// && ((target.modNPC!=null && target.modNPC.CanBeHitByProjectile(projectile)==true) || target.modNPC==null))
				{
					Player owner = Main.player[projectile.owner];
					//if (owner.active)
					//owner.ApplyDamageToNPC(target, (int)(projectile.damage * damagefalloff), 0f, 1, false);
					float damazz = (Main.DamageVar((float)1500) * damagefalloff);
					target.AddBuff(mod.BuffType("NinjaSmokedDebuff"),(int)damazz);
					if (Main.player[projectile.owner].SGAPly().ninjaSash > 2)
						IdgNPC.AddBuffBypass(target.whoAmI,mod.BuffType("NinjaSmokedDebuff"), (int)damazz);
				}
			}

			return true;
		}

		public override void AI()
		{

			int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Smoke);
			Main.dust[dust].scale = 2f;
			Main.dust[dust].alpha = 100;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			projectile.timeLeft -= 1;
		}

	}


}
