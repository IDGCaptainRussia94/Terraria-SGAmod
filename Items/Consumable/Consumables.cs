using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using Terraria.ModLoader.IO;
using System.Security.AccessControl;

namespace SGAmod.Items.Consumable
{
	public class RedManaStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Mana Star");
			Tooltip.SetDefault("'Otherwise known as the Lucky Star'\n" +
				"'Less Stable than its counterpart; it emanates heat but is thankfully only warm to the touch'" +
				"\nIncreases Max Mana by 20 each time it is used, up to 3 times depending on progression.");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			if (sgaplayer.Redmanastar < 1)
				return true;
			else if (sgaplayer.Redmanastar == 1 && SGAWorld.downedSharkvern)
				return true;
			else if (sgaplayer.Redmanastar == 2 && SGAWorld.downedWraiths > 3)
				return true;
			return false;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			SGAPlayer sgaplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			if (sgaplayer.Redmanastar < 1)
			{
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "Your magic attacks have a small chance to inflict 'On-Fire!'"));
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "Usable right away"));
			}
			if (sgaplayer.Redmanastar == 1)
			{
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "You instead inflict 'Thermal Blaze' instead of 'On Fire!'"));
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "Usable after Sharkvern is defeated."));
			}
			if (sgaplayer.Redmanastar == 2)
			{
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "You instead inflict 'Daybroken' instead of 'Thermal Blaze'"));
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "Usable after Luminite Wraith is defeated."));
			}
			if (sgaplayer.Redmanastar > 2)
			{
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "Its power is at its max, and can no longer help you gain strength"));
			}
			else
			{
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "Rare chance to permanently strip enemy immunity to the above debuff on magic hit."));
				tooltips.Add(new TooltipLine(mod, "RedStarLine", "-Permanent Upgrade-"));

			}
		}
		public override bool UseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			sgaplayer.Redmanastar += 1;
			return true;
		}
	}

	public class EnchantedBubble : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Bubble");
			Tooltip.SetDefault("'A breath of fresh air sealed within this magic bubble!'\n'Biting into this bubble restores your lungs'\nRecovers up to 100 Breath (the vanilla default)\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 90 seconds each"));
			ItemID.Sets.ItemIconPulse[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 1;
			item.value = Item.buyPrice(silver: 50);
			item.useStyle = 2;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useTurn = true;
			//item.UseSound = SoundID.Drown;
			item.consumable = true;
		}

		public override string Texture
		{
			get { return ("Terraria/Bubble"); }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((((Main.GlobalTime) * 0.116f)+(item.whoAmI * 0.179f)) % 1f, 0.8f, 0.75f);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					line.overrideColor = Color.Lerp(Color.Aqua, Color.Blue, 0.5f + (float)Math.Sin(Main.GlobalTime * 1.5f));
				}
			}
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}
		public override bool UseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			Main.PlaySound(SoundID.Drown, (int)player.Center.X, (int)player.Center.Y, 0, 1f, 0.50f);
			sgaplayer.AddCooldownStack(60 * 90, 1);
			player.breath = (int)MathHelper.Clamp(player.breath+200,0, player.breathMax);
			sgaplayer.sufficate = player.breath;
			return true;
		}
	}

	public class DivineShower : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Divine Shower Storm");
			Tooltip.SetDefault("'The heavens favor you'\nCauses all fallen stars on the ground to rain down on all active enemies; whichever is limited first\nThe entirety of 1 stack will fall over 1 enemy, but spread out the larger the stack is\nHowever this caps out at 5 per stack\nIs limited to once per night, per a long cooldown");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 1;
			item.value = 0;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useAnimation = 60;
			item.useTime = 60;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}

		public override string Texture
		{
			get { return ("Terraria/Item_"+ItemID.FallenStar); }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTime * -0.916f) % 1f, 0.8f, 0.75f);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					line.overrideColor = Color.Lerp(Color.Yellow, Color.Blue, 0.5f + (float)Math.Sin(-Main.GlobalTime * 0.5f));
				}
			}
		}

		public override bool CanUseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			int buffid = ModContent.BuffType<Buffs.StarStormCooldown>();

			return !player.HasBuff(buffid) && !Main.dayTime;
		}

		public override bool UseItem(Player player)
		{

			int buffid = ModContent.BuffType<Buffs.StarStormCooldown>();

			player.AddBuff(buffid, 60*900);

			List<Item> itemslist = new List<Item>();
			for(int i = 0; i < Main.item.Length; i += 1)
            {
				if (Main.item[i]!=null && Main.item[i].active && Main.item[i].type==ItemID.FallenStar && Main.item[i].stack<=5)
				itemslist.Add(Main.item[i]);
			}

			int itencheck = 0;
			for (int i = 0; i < Main.npc.Length; i += 1)
			{
				NPC enemy = Main.npc[i];
				bool invalid = (enemy == null || !enemy.active || enemy.dontTakeDamage || enemy.friendly || enemy.townNPC || enemy.life < 1 || enemy.immortal);
				if (itencheck < itemslist.Count && !invalid)
                {
					int stacked = itemslist[itencheck].stack;
					for (int j = 0; j < itemslist[itencheck].stack; j += 1)
					{
						Projectile.NewProjectile(new Vector2(enemy.Center.X, enemy.Center.Y - 600 + Main.rand.NextFloat(-400,0)), new Vector2(Main.rand.NextFloat(-3f,3f) * (1f+(stacked/3f)), 16f), ProjectileID.FallingStar, 2500, 8f, player.whoAmI, 1);
					}
					itemslist[itencheck].type = 0;
					itencheck += 1;
				}
			}

			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 3);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 3);
			recipe.AddIngredient(mod.ItemType("WraithFragment4"), 3);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	public class TrueCopperWraithNotch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Wraith Notch");
			Tooltip.SetDefault("'Reward for beating the Copper Wraith without angering it first'" +
				"\nUnlocks IDG's Starter Bags in Draken's shop when consumed\n-Permanent Upgrade-");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 1;
			item.value = Item.sellPrice(gold: 1);
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Consumable/TrueWraithNotch"); }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTime * 0.916f) % 1f, 0.8f, 0.75f);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					line.overrideColor = Color.Lerp(Color.Yellow, Color.Blue, 0.5f + (float)Math.Sin(Main.GlobalTime * 0.5f));
				}
			}
		}

		public override bool CanUseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			return !sgaplayer.Drakenshopunlock;
		}
		public override bool UseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			sgaplayer.Drakenshopunlock = true;
			return true;
		}
	}

	public class PortalEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Portal Essence");
			Tooltip.SetDefault("Allows the Strange Portal to 'move in' into free housing in your town");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 1;
			item.value = Item.sellPrice(gold: 1);
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/FieryRock"); }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTime * 0.916f) % 1f, 0.8f, 0.75f);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					line.overrideColor = Color.Lerp(Color.Yellow, Color.Blue, 0.5f + (float)Math.Sin(Main.GlobalTime * 0.5f));
				}
			}
		}

		public override bool CanUseItem(Player player)
		{
			if (SGAmod.anysubworld)
				return false;
			return !SGAWorld.portalcanmovein;
		}
		public override bool UseItem(Player player)
		{
			SGAWorld.portalcanmovein = true;
			return true;
		}
	}

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
			Tooltip.SetDefault("'Its ring calls forth the peddler lord himself'\nOpens a portal to bring the Traveling Merchant to your location where you used it\nUsing this item will restock the Traveling Merchant's Shop\nIs not usable if the Traveling Merchant is already in the world plus normal Traveling Merchant spawn rules\n" + Idglib.ColorText(Color.Orange, "Requires 2 Cooldown stacks, adds 300 seconds each"));
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count + 1 < player.SGAPly().MaxCooldownStacks && !NPC.travelNPC && NPC.CountNPCS(NPCID.TravellingMerchant)<1 && Main.dayTime && !Main.eclipse && Main.projectile.FirstOrDefault(type2 => type2.type == ModContent.ProjectileType<GongSummon>()) == default;
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