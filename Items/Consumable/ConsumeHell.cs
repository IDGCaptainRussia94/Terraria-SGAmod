using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;

namespace SGAmod.Items.Consumable
{
	public class ConsumeHell : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Consumable Hell");
			Tooltip.SetDefault("A Red Devil's favorite, eating it will turn your mouth into a literal flame thrower\nThis lasts for 1 minute, cannot be cancelled\nSeveral peppers may be consumed in succession to increase the power of the fire breath\nDeals non-class damage, does not crit\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 90 seconds each"));
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}

		public override bool UseItem(Player player)
		{
			player.SGAPly().AddCooldownStack(60 * 90, 1);
			player.SGAPly().FireBreath += 1;
			player.AddBuff(mod.BuffType("ConsumeHellBuff"), 60 * 60);
			return true;
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 5;
			item.value = 15000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item2;
			item.consumable = true;
			//item.buffType = mod.BuffType("ConsumeHellBuff");
			//item.buffTime = 60*60;
		}
	}

	public class EnergizerBattery : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energizer Battery");
			Tooltip.SetDefault("'Keeping going and going...'\nInstantly restores 20% of your Max Electric Charge on use\nIncreases your Max Electric Charge by 200 per use (up to 2000)\nAfter Sharkvern, this is raised 3000\nAfter Luminite Wraith, this is raised again to 5000\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 40 seconds each"));
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}

		public override bool UseItem(Player player)
		{
			int max = 2000 + (SGAWorld.downedSharkvern ? 1000 : 0) + (SGAWorld.downedWraiths>3 ? 2000 : 0);
			if (player.SGAPly().Electicpermboost < max)
			player.SGAPly().Electicpermboost += 200;
			player.SGAPly().AddCooldownStack(60 * 40, 1);
			player.SGAPly().electricCharge += (int)((float)player.SGAPly().electricChargeMax*0.20f);
			return true;
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 2;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item2;
			item.consumable = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VialofAcid"), 9);
			recipe.AddIngredient(mod.ItemType("Biomass"), 6);
			recipe.AddIngredient(ItemID.Bunny, 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
	}

	public class Gong : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eastern Gong");
			Tooltip.SetDefault("'Its ring calls forth the peddler lord himself'\nOpens a portal to bring the Traveling Merchant to your location where you used it\nUsing this item will restock the Traveling Merchant's Shop\nIs not usable if the Traveling Merchant is already in the world plus normal Traveling Merchant spawn rules\n" + Idglib.ColorText(Color.Orange, "Requires 2 Cooldown stack, adds 300 seconds each"));
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count + 1 < player.SGAPly().MaxCooldownStacks && !NPC.travelNPC && NPC.CountNPCS(NPCID.TravellingMerchant)<1 && Main.dayTime && !Main.eclipse;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.SGAPly().AddCooldownStack(60 * 300, 2);
			return true;
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 1;
			item.rare = 5;
			item.value = 50000;
			item.useStyle = 2;
			item.noUseGraphic = true;
			item.useAnimation = 120;
			item.useTime = 120;
			item.shoot = mod.ProjectileType("GongSummon");
			item.useTurn = true;
			item.UseSound = SoundID.Item1;
		}
	}

	public class GongSummon : ModProjectile
	{

		int chargeuptime = 100;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("GOOONGGG!");
		}

		public override bool? CanHitNPC(NPC target) { return false; }

		public override string Texture
		{
			get { return ("SGAmod/Items/Consumable/Gong"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.timeLeft = 220;
			projectile.tileCollide = false;
			projectile.magic = true;
			aiType = 0;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 gohere = new Vector2(0, -160+(160 / ((projectile.ai[0]/25f)+1)));

			Texture2D tex = ModContent.GetTexture("SGAmod/Extra_49");

			float scaleeffect = MathHelper.Clamp(((float)150f-projectile.timeLeft) / 80f,0f,Math.Min((float)projectile.timeLeft / 30f,1f));

			for (float valez = 0.1f; valez < 10f; valez += 0.5f)
			{
				spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * 0.25f, (Main.GlobalTime + valez * 3f) * 2f, tex.Size() / 2f, (0.5f + (valez / 15f)) * scaleeffect, SpriteEffects.FlipVertically, 0f);
			}

			scaleeffect = MathHelper.Clamp(((float)120f - projectile.timeLeft) / 30f, 0f, Math.Min((float)projectile.timeLeft / 30f, 1f));
			tex = Main.extraTexture[34];

			for (float valez = 0.1f; valez < 10f; valez += 0.2f)
			{
				spriteBatch.Draw(tex, projectile.Center-Main.screenPosition, null, Main.hslToRgb(((Main.GlobalTime + valez * 3f) * 2f)%1f,0.8f,0.75f)*0.05f, (Main.GlobalTime + valez*3f)*2f, tex.Size() / 2f, (0.25f+(valez/10f))* scaleeffect, SpriteEffects.FlipVertically, 0f);
			}

			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center+ gohere - Main.screenPosition, null, Color.White*Math.Min((float)projectile.timeLeft / 80f,1f), 0, Main.projectileTexture[projectile.type].Size()/2f, new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (player == null)
				projectile.Kill();
			if (player.dead)
				projectile.Kill();
			player.itemTime = 6;
			player.itemAnimation = 6;
			projectile.ai[0] += 1;

			Vector2 gohere = new Vector2(0, -160 + (160 / ((projectile.ai[0] / 25f) + 1)));

			if (projectile.timeLeft == 150)
			{
				RippleBoom.MakeShockwave(projectile.Center+ gohere, 8f, 2f, 20f, 100, 0.5f, true);
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 35, 1f, 0.25f);

			}

			if (projectile.timeLeft == 50)
			{
				Chest.SetupTravelShop();
				NetMessage.SendTravelShop(-1);

				if (NPC.CountNPCS(NPCID.TravellingMerchant) < 1)
					NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, NPCID.TravellingMerchant);
				else
					Main.npc[(NPC.FindFirstNPC(NPCID.TravellingMerchant))].Center = projectile.Center;
			}
		}
	}
}