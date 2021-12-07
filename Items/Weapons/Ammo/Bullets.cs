using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons.Ammo
{
	public class BlazeBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blaze Bullet");
			Tooltip.SetDefault("May inflict Thermal Blaze on enemies");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/BlazeBullet"); }
		}
		public override void SetDefaults()
		{
			item.damage = 13;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 1.5f;
			item.value = 25;
			item.rare = 5;
			item.shoot = ModContent.ProjectileType <Projectiles.BlazeBullet>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 5f;                  //The speed of the projectile
			item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("WraithFragment4"), 2);
			recipe.AddIngredient(mod.ItemType("FieryShard"), 1);
			recipe.AddIngredient(ItemID.MusketBall, 50);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}

	public class AcidBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Bullet");
			Tooltip.SetDefault("High chance of inflicting Acid Burn\nAcid Burn does more damage the more defense the enemy has, and reduces their defense by 5\nAcid Rounds quickly melt away after being fired and do not go far");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/AcidBullet"); }
		}
		public override void SetDefaults()
		{
			item.damage = 8;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 1.5f;
			item.value = 25;
			item.rare = 5;
			item.shoot = ModContent.ProjectileType<Projectiles.AcidBullet>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 2.5f;                  //The speed of the projectile
			item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VialofAcid"), 1);
			recipe.AddIngredient(ItemID.MusketBall, 50);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}

	public class NoviteBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Bullet");
			Tooltip.SetDefault("Redirects when near enemies, but only once\nCosts 25 electric charge to change direction");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/NoviteBullet"); }
		}
		public override void SetDefaults()
		{
			item.damage = 6;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 0f;
			item.value = 20;
			item.rare = ItemRarityID.Green;
			item.shoot = ModContent.ProjectileType<Projectiles.NoviteBullet>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 4.5f;                  //The speed of the projectile
			item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NoviteBar>(), 1);
			recipe.AddIngredient(ItemID.MusketBall, 50);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}

	public class SeekerBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seeker Bullet");
			Tooltip.SetDefault("Does summon damage\nHomes in on the minion focused enemy\nThis includes enemies who otherwise can't be chased");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/SeekerBullet"); }
		}
        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
			flat += item.damage * player.minionDamage;
		}
        public override void SetDefaults()
		{
			item.damage = 10;
			item.summon = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;
			item.knockBack = 0f;
			item.value = 20;
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ModContent.ProjectileType<Projectiles.SeekerBullet>();
			item.shootSpeed = 6f;
			item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.AncientBattleArmorMaterial, 1);
			recipe.AddIngredient(ItemID.MusketBall, 250);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 250);
			recipe.AddRecipe();
		}
	}

	public class SoulboundBullet : SeekerBullet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soulbound Bullet");
			Tooltip.SetDefault("A bullet bound with souls, Does summon damage\nHitting an enemy focuses them for your minions to attack\nIf below 20% health, you will leech small amounts of life on hit\n"+Idglibrary.Idglib.ColorText(Color.Red,"Suffer self-damage when you miss and hit a tile"));
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/SoulboundBullet"); }
		}
		public override void SetDefaults()
		{
			item.damage = 50;
			item.summon = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;
			item.knockBack = 0f;
			item.value = 20;
			item.rare = ItemRarityID.Lime;
			item.shoot = ModContent.ProjectileType<Projectiles.SoundboundBullet>();
			item.shootSpeed = 6f;
			item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Ectoplasm, 1);
			recipe.AddIngredient(ItemID.MusketBall, 50);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}

	public class TungstenBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tungsten Bullet");
			Tooltip.SetDefault("Isn't slowed down in water");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/TungstenBullet"); }
		}
		public override void SetDefaults()
		{
			item.damage = 8;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 1.5f;
			item.value = 10;
			item.rare = 1;
			item.shoot = ModContent.ProjectileType<Projectiles.TungstenBullet>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 4.5f;                  //The speed of the projectile
			item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TungstenBar, 1);
			recipe.AddIngredient(ItemID.MusketBall, 70);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 70);
			recipe.AddRecipe();
		}
	}

	public class AimBotBullet : ModItem, IHellionDrop
	{
		int IHellionDrop.HellionDropAmmount() => 500 + Main.rand.Next(501);
		int IHellionDrop.HellionDropType() => ModContent.ItemType<AimBotBullet>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aim-Bot Bullet");
			Tooltip.SetDefault("Adjusts your aim to target the scrub nearest your mouse cursor; bullet travels instantly\nAimbot bullets can pierce 2 targets ending on the 3rd, does not cause immunity frames\nBullets do 20% increased damage after each hit they pass through\n'GIT GUD, GET LMAOBOX!'\n(disclaimer, does not function in pvp)");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return lightColor = Main.hslToRgb((Main.GlobalTime / 7f) % 1f, 0.85f, 0.45f);
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 2.0f;
			item.value = 300;
			item.rare = 10;
			item.shoot = ModContent.ProjectileType <Projectiles.AimBotBullet>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 1f;                  //The speed of the projectile
			item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Entrophite"), 30);
			recipe.AddIngredient(mod.ItemType("MoneySign"), 1);
			recipe.AddIngredient(mod.ItemType("ByteSoul"), 10);
			recipe.AddIngredient(mod.ItemType("DrakeniteBar"), 1);
			recipe.AddIngredient(ItemID.MoonlordBullet, 100);
			recipe.AddIngredient(ItemID.MeteorShot, 50);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 150);
			recipe.AddRecipe();
		}
	}
	public class PortalBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Portal Bullet");
			Tooltip.SetDefault("Portals appear at the mouse cursor which summon high velocity bullets to fly at the nearby enemies");
		}
		public override string Texture
		{
			get { return ("Terraria/Item_"+ItemID.HighVelocityBullet); }
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Texture2D inner = SGAmod.ExtraTextures[92];

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			spriteBatch.Draw(inner, position+ (new Vector2(4f,8f)*scale), null, drawColor, Main.GlobalTime, new Vector2(inner.Width / 2, inner.Height / 2), scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(Main.itemTexture[item.type], position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			return false;
		}

		public override void SetDefaults()
		{
			item.damage = 19;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 1.5f;
			item.value = 100;
			item.rare = 9;
			item.shoot = ModContent.ProjectileType<Projectiles.PortalBullet>();   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 4.5f;                  //The speed of the projectile
			item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 1);
			recipe.AddIngredient(mod.ItemType("PlasmaCell"), 1);
			recipe.AddIngredient(ItemID.HighVelocityBullet, 100);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}


	public class PrismicBullet : ModItem
	{
		public virtual int maxvalue => 2;
		public int ammotype;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lesser Prismic Bullet");
			Tooltip.SetDefault("Shots cycle through your 2nd and 3rd ammo slots while placed in your first\nDefaults to a weak musket ball\nHas a 66% to not consume the fired ammo type");
		}
		public override string Texture
		{
			get { return ("Terraria/Item_"+ItemID.SilverBullet); }
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.IntenseRainbowDye);
			shader.UseOpacity(0.5f);
			shader.UseSaturation(0.25f);
			shader.Apply(null);
			spriteBatch.Draw(Main.itemTexture[item.type], position, frame, drawColor,0, origin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			return false;
		}
		public override void SetDefaults()
		{
			item.damage = 2;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 1.5f;
			item.value = 25;
			item.rare = 5;
			item.shoot = ProjectileID.Bullet;   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 4.5f;                  //The speed of the projectile
			item.ammo = AmmoID.Bullet;
		}
		public virtual int GetAmmo(Player player,out int weapontype,bool previous = false)
		{
			bool canuse = true;
			weapontype = item.type;
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			Item newitem = new Item();
			newitem.SetDefaults(sgaplayer.ammoinboxes[sgaplayer.PrismalShots + (1)]);

			if (sgaplayer.ammoinboxes[1] == 0 || sgaplayer.ammoinboxes[1] == item.type)
				canuse = false;
			if (sgaplayer.ammoinboxes[2] == 0 || sgaplayer.ammoinboxes[2] == item.type)
				canuse = false;
			if ((sgaplayer.ammoinboxes[3] == 0 || sgaplayer.ammoinboxes[3] == item.type) && maxvalue>2)
				canuse = false;
			if (newitem.ammo != item.ammo)
				canuse = false;

			if (canuse)
			{
				if (!previous)
				{
					sgaplayer.PrismalShots += 1;
					sgaplayer.PrismalShots %= maxvalue;
				}
				ammotype = newitem.type;
				return newitem.shoot;
			}
			else
			{
				return item.shoot;
			}

		}

		public override void OnConsumeAmmo(Player player)
		{
			if (ammotype!=item.type && Main.rand.Next(maxvalue+1)<1)
			player.ConsumeItemRespectInfiniteAmmoTypes(ammotype);
		}

		public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
		{
			int nothing = 0;
		type = GetAmmo(player,out nothing);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:Tier1Bars", 1);
			recipe.AddRecipeGroup("SGAmod:Tier2Bars", 1);
			recipe.AddRecipeGroup("SGAmod:Tier4Bars", 1);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 2);
			recipe.AddIngredient(ItemID.HallowedBar, 1);
			recipe.AddIngredient(ItemID.SilverBullet, 75);
			recipe.AddIngredient(ModContent.ItemType<TungstenBullet>(), 75);
			recipe.AddTile(TileID.ImbuingStation);
			recipe.SetResult(this, 150);
			recipe.AddRecipe();
		}
	}

	public class PrismalBullet : PrismicBullet
	{
		public override int maxvalue => 3;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Bullet");
			Tooltip.SetDefault("Highly increased damage over its precursor\nCycles through all your ammo slots when placed in your first; defaults to Musket Balls\nHas a 75% to not consume the fired ammo type");
		}
		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.SilverBullet); }
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.LivingRainbowDye);
			shader.Apply(null);
			spriteBatch.Draw(Main.itemTexture[item.type], position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			return false;
		}
		public override void SetDefaults()
		{
			item.damage = 17;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 1.5f;
			item.value = 25;
			item.rare = 10;
			item.shoot = ProjectileID.Bullet;   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 4.5f;                  //The speed of the projectile
			item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 1);
			recipe.AddIngredient(mod.ItemType("PrismicBullet"), 150);
			recipe.AddTile(TileID.ImbuingStation);
			recipe.SetResult(this, 150);
			recipe.AddRecipe();
		}
	}


}
