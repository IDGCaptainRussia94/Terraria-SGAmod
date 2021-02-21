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
using SGAmod.Items.Weapons;
using SGAmod.HavocGear.Items.Weapons;
using Terraria.Utilities;
using SGAmod.Items.Consumable;
using Terraria.DataStructures;
using SGAmod.Buffs;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Dusts;
using SGAmod.Items.Accessories;

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
			grenadetypes.Add(mod.ItemType("SwarmGrenade"));
			grenadetypes.Add(mod.ItemType("ThermalGrenade"));
			if ((level != 2 && level>0) || level == 3)
			{
				grenadetypes.Add(mod.ItemType("CelestialCocktail"));
				grenadetypes.Add(mod.ItemType("SludgeBomb"));
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
			player.itemAnimation = (int)(basetype2.useAnimation * baseumtli);
			player.itemAnimationMax = (int)(basetype2.useAnimation * baseumtli);
			player.itemTime = (int)(basetype2.useTime* baseumtli);
			type = FindProjectile(basetype2.shoot, basetype);

			if (itemtype == mod.ItemType("ThrowableTrapSpikyball"))
				speedbase /= 1f;

			basespeed *= (basetype2.shootSpeed + speedbase);
			speedX = basespeed.X;
			speedY = basespeed.Y;
			if (type!=ProjectileID.Beenade)
			damage += (int)((float)basetype2.damage * player.Throwing().thrownDamage);

			damage += (int)(basetype2.type == ItemID.MolotovCocktail || basetype2.type == ItemID.AleThrowingGlove || basetype2.type == ItemID.Bone || basetype2.type == ItemID.SpikyBall ? damage * 2f : 0);


			if (TrapDamageItems.SavingChanceMethod(player,true))
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
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddIngredient(ItemID.BeeWax, 4);
			recipe.AddIngredient(ItemID.Leather, 6);
			recipe.AddRecipeGroup("SGAmod:Tier2Bars", 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddIngredient(ItemID.BeeWax, 4);
			recipe.AddIngredient(ItemID.TatteredCloth, 2);
			recipe.AddRecipeGroup("SGAmod:Tier2Bars", 4);
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
			Tooltip.SetDefault("Throws hand grenades further, and increases their damage\nUpgraded to now throw Ale, Molotovs, Spiky balls, and Bones! And iamproving their damage\n" + disc);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.useStyle = 1;
			item.damage = 6;
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
			recipe.AddIngredient(ItemID.AncientCloth, 8);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 12);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddTile(TileID.MythrilAnvil);
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
			recipe.AddIngredient(ItemID.Dynamite, 8);
			recipe.AddIngredient(ItemID.StickyBomb, 16);
			recipe.AddRecipeGroup("SGAmod:Tier3Bars", 8);
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
			item.crit = 10;
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
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "Snapping will erase half the enemies, with a cooldown")));
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
			{
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "Snapping will erase enemies with less than 100000 HP times the average of your damage bonuses")));
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "100000 * ((Melee+Ranged+Magic+Summon+Throwing)/5)")));
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "If an enemy's life exceeds this they will will receive the fading debuff")));
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "This debuff does massive damage over time, and fades enemies out of existance when they run out of life")));
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "Bosses affected will only receive the fading debuff")));
			tooltips.Add(new TooltipLine(mod, "GucciGlove", Idglib.ColorText(Color.White, "Enemies who fade out of existance do not drop anything (unless a boss) or count as being killed")));

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

			damage += (int)(basetype2.type == ItemID.MolotovCocktail || basetype2.type == ItemID.AleThrowingGlove || basetype2.type == ItemID.Bone || basetype2.type == ItemID.SpikyBall ? damage * 1.5f : 0);

			int proj=Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, basespeed.X, basespeed.Y, type, damage, basetype2.knockBack, player.whoAmI);
			Main.projectile[proj].friendly = true;
			Main.projectile[proj].hostile = false;
			Main.projectile[proj].netUpdate = true;
			IdgProjectile.Sync(proj);

			if (TrapDamageItems.SavingChanceMethod(player, true))
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

	}

	internal class GrenadeNotAHook3 : GrenadeNotAHook
	{
		public override int level => 2;
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
					if (Main.dedServ)
					{
						ModPacket packet = SGAmod.Instance.GetPacket();
						packet.Write((ushort)MessageType.Snapped);
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
							if (Main.rand.Next(0, 2) == 0)
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
			item.thrown = false;
			item.damage = 60;
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

			int theproj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("Explosion"), (int)((double)projectile.damage * 1.5f), projectile.knockBack, projectile.owner, 0f, 0f);
			Main.projectile[theproj].Throwing().thrown = true;
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
			item.thrown = false;
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
			Main.projectile[theproj].Throwing().thrown = true;
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

	class SludgeBomb : AcidGrenade
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Sludge Bomb");
			Tooltip.SetDefault("Explodes into sludge that sticks to walls and damage enemies\nEnemies near the sludge get Oiled, Confused, and Dank Slowed\nDank Slow only applies to enemies not immune to poison");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Grenade);
			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.thrown = false;
			item.damage = 80;
			item.shootSpeed = 4f;
			item.shoot = mod.ProjectileType("SludgeBombProj");
			item.value = Item.buyPrice(0, 0, 2, 0);
			item.rare = 5;
		}
		public override void AddRecipes()
		{
			//nil
		}

	}

	class SludgeBombProj : AcidGrenadeProj
	{

		bool hitonce = false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Sludge Bomb");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Grenade);
			projectile.Throwing().thrown = true;
			projectile.timeLeft = 240;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/SludgeBomb"); }
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

				int dust = Dust.NewDust(new Vector2(projectile.Center.X - 32, projectile.Center.Y - 32), 64, 64, 184, 0, 0, 100, new Color(30, 30, 30, 20), 1f);
				Main.dust[dust].scale = 2.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (projectile.velocity * (float)(Main.rand.Next(10, 20) * 0.01f)) + (randomcircle * randomfloat);
			}
			for (int i = 0; i < 10; i += 1)
			{
				Projectile proj = Projectile.NewProjectileDirect(projectile.Center, Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(2f, 10f), ModContent.ProjectileType<SludgeProj>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(3, 30));
			}

			projectile.velocity = default(Vector2);
			projectile.type = ProjectileID.Grenade;
			return true;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 184,0,0,100, new Color(30, 30, 30, 20), 1f);
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
		}
	}

	public class SludgeProj : ModProjectile
	{

		float scalePercent => MathHelper.Clamp(projectile.timeLeft / 60f, 0f, Math.Min(projectile.localAI[0] / 10f, 1f));

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.timeLeft = 400;
			projectile.light = 0.1f;
			projectile.extraUpdates = 0;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/MudBlob"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ew Gross!");
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void AI()
		{

			if (projectile.localAI[1] < 10)
			{
				projectile.localAI[1] = Main.rand.Next(3)+100;
			}
			projectile.localAI[0] += 1;

			Point16 point = new Point16((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16);

			if (projectile.localAI[0] > projectile.ai[0] && WorldGen.InWorld(point.X, point.Y))
			{
				if (Main.tile[point.X, point.Y].wall > 0) 
				{
					if (projectile.localAI[0] < 10000)
					{
						projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
						projectile.localAI[0] += 10000;
					}

					projectile.velocity /= 1.25f;
					projectile.rotation += (projectile.velocity.X + projectile.velocity.Y) / 8f;

					projectile.scale = Math.Min(projectile.scale + (4 - projectile.scale) / 3f, 3f);


					foreach (NPC enemy in Main.npc.Where(npctest => npctest.active && !npctest.friendly && !npctest.immortal && !npctest.boss && !npctest.noGravity && !npctest.noTileCollide &&
					npctest.Distance(projectile.Center)< 32 * projectile.scale))
					{
						if (!enemy.buffImmune[BuffID.Poisoned])
						enemy.AddBuff(ModContent.BuffType<DankSlow>(), 20);
						enemy.AddBuff(BuffID.Oiled, 60*1);
						enemy.AddBuff(BuffID.Confused, 3);
						enemy.SGANPCs().nonStackingImpaled = projectile.damage;
					}

					for (int num654 = 0; num654 < 1 + (projectile.localAI[0]<10003 ? 10 : 0); num654++)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
						int num655 = Dust.NewDust(projectile.position + Vector2.UnitX * -20f, projectile.width + 40, projectile.height+40, 184, projectile.velocity.X + randomcircle.X * 4f, projectile.velocity.Y + randomcircle.Y * 4f, 200, new Color(30, 30, 30, 20), 1f);
						Main.dust[num655].noGravity = true;
					}

					return;
                }
                else
                {
					if (projectile.localAI[0]>8000)
					projectile.Kill();
                }
			}
				projectile.rotation = projectile.velocity.ToRotation()+MathHelper.PiOver2;
				projectile.velocity.Y += 0.25f;

				int num126 = Dust.NewDust(projectile.position - new Vector2(2, 2), Main.rand.Next(projectile.width + 6), Main.rand.Next(projectile.height + 6), 184, 0, 0, 140, new Color(30, 30, 30, 20), 1f);
				Main.dust[num126].noGravity = true;
				Main.dust[num126].velocity = projectile.velocity * 0.5f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			Texture2D tex = SGAmod.ExtraTextures[107+ (int)projectile.localAI[1]%3];
			if (projectile.localAI[0] < 9000)
            {
				tex = Main.projectileTexture[projectile.type];
				spriteBatch.Draw(tex, drawPos, new Rectangle(0,(projectile.whoAmI*(tex.Height/3))%tex.Height, tex.Width, tex.Height/3), (lightColor.MultiplyRGB(Color.Brown) * 0.75f)* scalePercent, projectile.rotation,new Vector2(tex.Width,tex.Height/3f)/2f, projectile.scale, SpriteEffects.None, 0f);
				return false;
			}
			spriteBatch.Draw(tex, drawPos, null, lightColor.MultiplyRGB(Color.Brown) * 0.5f* scalePercent, projectile.rotation, tex.Size()/2f, projectile.scale, SpriteEffects.None, 0f);
			return false;
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
			item.thrown = false;
			item.damage = 40;
			item.useTime = 60;
			item.useAnimation = 60;
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
			projectile.thrown = false;
			projectile.Throwing().thrown = false;
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

			Main.player[projectile.owner].SGAPly().molotovLimit = 60;

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
			item.height = 8;
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
			for (int i = 0; i < 5; i += 1)
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
			recipe.AddIngredient(ItemID.Snowball, 150);
			recipe.AddIngredient(mod.ItemType("Jarate"), 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 150);
			recipe.AddRecipe();

		}

	}

	public class JarateShurikensProg : ModProjectile
	{

		protected int fakeid = ProjectileID.SnowBallFriendly;
		public int guyihit = -10;
		public int cooldown = -10;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jarate Shuriken");
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
			projectile.magic = false;
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

	class UraniumSnowballs : JarateShurikens, IRadioactiveItem
	{
		public int RadioactiveHeld() => 2;
		public int RadioactiveInventory() => 1;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Uranium Filled Snowballs");
			Tooltip.SetDefault("'Excuse me? Yeah, this is a thing'\nSnowballs leave behind Irradiated pools, infecting enemies\nEnemies killed while Irradiated explode, crits force an explosion\nRight click to throw an empowered shot that sticks to enemies\n"+ Idglib.ColorText(Color.Orange, "Requires 2 Cooldown stacks, adds 20 seconds each")
				+"\n"+ Idglib.ColorText(Color.Red,"You suffer Radiation 2 while holding these")+"\n"+Idglib.ColorText(Color.Red,"Radiation 1 if only in inventory"));
		}

        public override string Texture => "Terraria/Item_"+ItemID.Snowball;

        public override void SetDefaults()
		{
			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			item.width = 8;
			item.height = 8;
			item.knockBack = 6;
			item.maxStack = 999;
			item.damage = 72;
			item.crit = 15;
			item.consumable = true;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 30;
			item.useTime = 30;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.value = Item.buyPrice(0, 0, 1, 0);
			item.rare = 4;
			item.shootSpeed = 25f;
			item.shoot = mod.ProjectileType("UraniumSnowballsProg");
			item.ammo = AmmoID.Snowball;
		}

        public override Color? GetAlpha(Color lightColor)
        {
			return Color.Lerp(Color.Lime, Color.Green, 0.50f + (float)Math.Sin(Main.GlobalTime * 4f) / 2f);
        }
        public override void UpdateInventory(Player player)
        {
			player.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationOne").Type, 60*3);
		}
        public override void HoldItem(Player player)
        {
			//if (!player.HasAccessoryEquipped(ModContent.ItemType<HandlingGloves>()) || !player.HasAccessoryEquipped(ModContent.ItemType<BlinkTechGear>()))
		}

        public override bool AltFunctionUse(Player player)
        {
            return player.SGAPly().AddCooldownStack(60*20,2);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			int numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(0);
			//for (int i = 0; i < numberProjectiles; i += 1)
			//{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY)).RotatedBy(MathHelper.Lerp(-rotation, rotation, Main.rand.NextFloat()));
				int thisoned = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, Main.myPlayer);
				Main.projectile[thisoned].Throwing().thrown = true;
				Main.projectile[thisoned].ranged = false;
			if (player.altFunctionUse == 2)
            {
				Main.projectile[thisoned].ai[1] = 1;
				SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_DrakinShot, (int)position.X, (int)position.Y);
				if (sound != null)
					sound.Pitch -= 0.525f;
			}
			Main.projectile[thisoned].netUpdate = true;
			//}

			return false;
		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Snowball, 50);
			recipe.AddIngredient(ItemID.LunarOre, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}

	}

	public class UraniumSnowballsProg : JarateShurikensProg
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Uranium Filled Snowballs");
		}
		public override string Texture => "Terraria/Item_" + ItemID.Snowball;
		int hitnpc = -1;
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (crit)
			{
				target.SGANPCs().IrradiatedExplosion(target, damage);
			}
			hitnpc = target.whoAmI;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.SnowBallFriendly);
			projectile.Throwing().thrown = false;
			projectile.ranged = true;
			projectile.magic = false;
			projectile.tileCollide = true;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.extraUpdates = 0;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = fakeid;

			for (float num654 = 0; num654 < 8; num654+=0.25f)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); randomcircle *= (float)(num654 / 10.00);
				int num655 = Dust.NewDust(projectile.position + Vector2.UnitX * -6f, projectile.width + 12, projectile.height+12, 75, 0,0, 150, Color.Lime, 1.8f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].noLight = true;
				Main.dust[num655].velocity = new Vector2(randomcircle.X * 12f, randomcircle.Y * 12f);
			}

			if (projectile.ai[1] < 1)
			{
				int proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<RadioactivePool>(), projectile.damage, projectile.knockBack, projectile.owner);
			}
			else
			{
				int proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<RadioactivePool>(), projectile.damage*2, projectile.knockBack, projectile.owner);
				Main.projectile[proj].width += 160;
				Main.projectile[proj].height += 160;
				Main.projectile[proj].timeLeft = 600;
				Main.projectile[proj].Center -= new Vector2(80, 80);
				if (hitnpc > -1)
                {
					//Main.projectile[proj].ai[1] = -(hitnpc+1);
				}
				Main.projectile[proj].netUpdate = true;
			}

			SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_SkyDragonsFuryShot, (int)projectile.Center.X, (int)projectile.Center.Y);
			if (sound != null)
				sound.Pitch -= 0.525f;

			return true;
		}

		public override void AI()
		{
			if (projectile.ai[1] > 0)
			{
				projectile.localAI[1] += 1;

				if (projectile.localAI[1]%3==0)
				{
					int proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<RadioactivePool>(), projectile.damage, projectile.knockBack, projectile.owner);
					Main.projectile[proj].timeLeft = 50;
				}
			}

			for (int num654 = 0; num654 < 2; num654++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
				int num655 = Dust.NewDust(projectile.Center + Main.rand.NextVector2Circular(16,16), 0, 0, 75, randomcircle.X * 3f, randomcircle.Y * 3f, 150, Color.Lime, 1f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].noLight = true;
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = projectile.oldPos.Length-1; k > 0; k-=1)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				Color color = Color.Lime * (1f-((float)k / ((float)projectile.oldPos.Length)));
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color * 0.75f, projectile.rotation, drawOrigin, projectile.scale+0.5f, SpriteEffects.None, 0f);
			}

			Texture2D texture = SGAmod.ExtraTextures[96];
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.Lime*0.50f, 0, texture.Size() / 2f, 0.5f, SpriteEffects.None, 0f);

			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center-Main.screenPosition, null, Color.GreenYellow, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}

	public class RadioactivePool : SludgeProj
	{

		float scalePercent => MathHelper.Clamp(projectile.timeLeft / 60f, 0f, Math.Min(projectile.localAI[0] / 10f, 1f));

		public override void SetDefaults()
		{
			projectile.width = 128;
			projectile.height = 128;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.magic = false;
			projectile.timeLeft = 160;
			projectile.light = 0.1f;
			projectile.extraUpdates = 0;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pool of Radioactive Sludge");
		}

		public override bool CanDamage()
		{
			return projectile.ai[1]>0;
		}

        public override void AI()
		{
			if (projectile.localAI[1] < 10)
			{
				projectile.localAI[1] = Main.rand.Next(3) + 100;
			}
			projectile.localAI[0] += 1;

			/*if (projectile.ai[1] < 0)
			{
				NPC npc = Main.npc[(int)(-projectile.ai[1] + 1)];
				if (npc!=null && npc.active)
				{
					projectile.Center = npc.Center;
                }
                else
                {
					projectile.ai[1] = 0;
                }
            }*/

			Point16 point = new Point16((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16);

			float boomspeed = projectile.ai[1] > 0 ? 8 : 0;

			if (projectile.timeLeft > 30 && boomspeed<1)
			{
				foreach (NPC enemy in Main.npc.Where(npctest => npctest.active && !npctest.friendly && !npctest.immortal &&
				npctest.Distance(projectile.Center) < projectile.width * projectile.scale))
				{
					SGAnpcs sganpc = enemy.SGANPCs();
					sganpc.nonStackingImpaled = projectile.damage * 3;
					sganpc.impaled += projectile.damage/2;
					sganpc.IrradiatedAmmount = Math.Min(sganpc.IrradiatedAmmount + 1, projectile.damage * 3);
					enemy.AddBuff(mod.BuffType("RadioDebuff"), 60 * 18);
				}
			}

			for (int num654 = 0; num654 < 4+ boomspeed * 4; num654++)
			{
				if (boomspeed > 0 || Main.rand.Next(160) < projectile.timeLeft)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
					int num655 = Dust.NewDust(projectile.Center + Main.rand.NextVector2Circular(projectile.width, projectile.width), 0, 0, 184, projectile.velocity.X + randomcircle.X * (4f), projectile.velocity.Y + randomcircle.Y * (4f), 150, Color.Lime, 0.5f);
					Main.dust[num655].noGravity = true;
				}
			}

			for (int num654 = 1; num654 < 3 + boomspeed*4; num654++)
			{
				if (boomspeed > 0 || Main.rand.Next(160) < projectile.timeLeft)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
					int num655 = Dust.NewDust(projectile.Center + Main.rand.NextVector2Circular(projectile.width/2, projectile.width/2), 0, 0, ModContent.DustType<RadioDust>(), projectile.velocity.X + randomcircle.X * (2f), projectile.velocity.Y + randomcircle.Y * (2f), boomspeed > 0 ? 140 : 220, boomspeed > 0 ? Color.Orange : Color.Lime, 0.5f + (boomspeed / 20f));
					Main.dust[num655].noGravity = true;
				}
			}

			if (Main.rand.Next(24) < projectile.timeLeft)
			{
				int num126 = Dust.NewDust(projectile.Center + Main.rand.NextVector2Circular(projectile.width/2, projectile.width/2), 0, 0, 184, 0, 0, 140, new Color(30, 30, 30, 20), 1f);
				Main.dust[num126].noGravity = true;
				Main.dust[num126].velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-6f, 1f));
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
	}


	class SharkBait : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Shark Bait");
			Tooltip.SetDefault("'Contains many yummy snacks a Sharvern needs in their diet!'\nThrows a bucket of Shark Bait, which erupts into fish\nFish may spawn hungry sharks\nProduces more fish when thrown into water\nDoubles as fishing bait");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.MolotovCocktail);
			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.thrown = false;
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
			get { return ("SGAmod/Items/Weapons/SharkBait"); }
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
			projectile.rotation += projectile.velocity.Length() * (float)(Math.Sign(projectile.velocity.X))*0.01f;
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
				for (float xx = 5f; xx < 20f; xx += projectile.knockBack>1 ? 0.85f : 1.75f)
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

			Texture2D tex = Main.npcTexture[ModContent.NPCType<SharvernMinion>()];
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
	class RockProj : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Rock");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Grenade);
			projectile.Throwing().thrown = true;
			projectile.timeLeft = 300;
			projectile.aiStyle = -1;
			projectile.width = 6;
			projectile.height = 6;
			projectile.penetrate = 1;
		}

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/RockProj"); }
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			for (int i = 0; i < 8; i += 1)
			{
				int dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 8, 8, 1);
				Main.dust[dust].scale = 0.75f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(2f, 2f);
			}
			SoundEffectInstance sound = Main.PlaySound(SoundID.Tink, (int)projectile.Center.X, (int)projectile.Center.Y);
				if (sound!=null)
				sound.Pitch -= 0.525f;

			//			SoundEffectInstance sound = Main.PlaySound(SoundID.Tink, (int)projectile.Center.X, (int)projectile.Center.Y);
			//if (sound!=null)
			//	sound.Pitch -= 0.525f;
			return true;
		}

		public override void AI()
		{
			projectile.velocity.Y += 0.30f;
			projectile.rotation += projectile.velocity.Length() * (float)(Math.Sign(projectile.velocity.X * 1000f) / 1000f) * 10f;
		}

	}


}

namespace SGAmod.HavocGear.Items.Weapons
{


	class SwarmGrenade : AcidGrenade
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Gnat-ade");
			Tooltip.SetDefault("Explodes into a swarm of seeking flies");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Grenade);
			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.thrown = false;
			item.damage = 35;
			item.shootSpeed = 4f;
			item.shoot = mod.ProjectileType("SwarmGrenadeProj");
			item.value = Item.buyPrice(0, 0, 5, 0);
			item.rare = 5;
		}

        public override void AddRecipes()
        {
            //Nil
        }
    }


	class SwarmGrenadeProj : AcidGrenadeProj
	{

		bool hitonce = false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Swarm Grenade");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Grenade);
			projectile.Throwing().thrown = true;
			projectile.timeLeft = 320;
		}

		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Items/Weapons/SwarmGrenade"); }
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

				int dust = Dust.NewDust(new Vector2(projectile.Center.X - 32, projectile.Center.Y - 32), 64, 64, DustID.Buggy);
				Main.dust[dust].scale = 0.25f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (projectile.velocity * (float)(Main.rand.Next(10, 20) * 0.01f)) + (randomcircle * randomfloat);
			}

			for(int i = 0; i < 8; i += 1)
            {
				double angle = (i / 8.0) * (Math.PI * 2.0);
				Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 4f;
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, velocity.X, velocity.Y, ModContent.ProjectileType<FlyProjectileThrown>(), (int)((double)projectile.damage), projectile.knockBack/5f, projectile.owner, 0f, 0f);
			}

			int theproj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("Explosion"), (int)((double)projectile.damage * 0.5f), projectile.knockBack, projectile.owner, 0f, 0f);
			Main.projectile[theproj].Throwing().thrown = true;
			Main.projectile[theproj].netUpdate = true;
			projectile.type = ProjectileID.Grenade;
			return true;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Buggy);
			Main.dust[dust].scale = 0.35f;
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
		}

	}

	public class FlyProjectileThrown : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fly");
			//ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
		}

		public override string Texture => "SGAmod/NPCs/Dank/Fly";

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.friendly = true;
			projectile.Throwing().thrown = true;
			projectile.timeLeft = 300;
			projectile.penetrate = 5;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 8;
		}

        public override bool CanDamage()
        {
            return projectile.penetrate>1;
        }

        public override void AI()
		{

			if (projectile.timeLeft > 30 && projectile.penetrate < 2)
				projectile.timeLeft = 30;

			Player player = Main.player[base.projectile.owner];
			projectile.localAI[0] += 1;


			if (projectile.localAI[0] % 20 == 0)
			{

				List<NPC> closestnpcs = SGAUtils.ClosestEnemies(projectile.Center, 300f);

				NPC target = closestnpcs?[0];//Closest

				//projectile.ai[0] = 0;

				if (target != null && target.active && target.life > 0 && Vector2.Distance(target.Center, projectile.Center) < 600)
				{
					projectile.ai[0] = target.whoAmI+1;
						projectile.netUpdate = true;
				}
			}

			if ((projectile.localAI[0]+(projectile.whoAmI*7)) % 60 == 0 || (int)projectile.ai[1] == 0)
			{
				float left = 0.40f + (1f-(projectile.timeLeft/300f))*0.20f;
				projectile.ai[1] = Main.rand.NextFloat(-MathHelper.Pi * left, MathHelper.Pi * left);
				projectile.netUpdate = true;
			}

			if (projectile.ai[0] > 0)
            {
				NPC target = Main.npc[(int)projectile.ai[0]-1];
				if (target != null && target.active && target.life > 0)
                {
					Vector2 difference = target.Center - projectile.Center;

					projectile.velocity += Vector2.Normalize(difference).RotatedBy(projectile.ai[1])/3f;
					projectile.velocity /= 1.03f;

					if (projectile.velocity.Length() > 16)
                    {
						projectile.velocity.Normalize(); projectile.velocity *= 16f;
					}



				}

			}




		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texa = Main.projectileTexture[projectile.type];
			int frame = texa.Height / 4;
			float alpha = MathHelper.Clamp(projectile.localAI[0] / 10f, 0f, Math.Min(projectile.timeLeft/30f,1f));
			spriteBatch.Draw(texa, projectile.Center - Main.screenPosition, new Rectangle(0,((int)((projectile.localAI[0] /3f)% 4)) * frame, texa.Width, frame), lightColor * alpha, projectile.rotation, new Vector2(texa.Width, frame) / 2f, projectile.scale, projectile.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{

			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}

			projectile.velocity /= 1.25f;
			return false;
		}
	}

}