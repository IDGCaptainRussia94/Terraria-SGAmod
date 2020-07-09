using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;
using System.IO;
using SGAmod.NPCs.SpiderQueen;

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
			else if (sgaplayer.Redmanastar == 2 && SGAWorld.downedWraiths>3)
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

	public class TrueCopperWraithNotch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Wraith Notch");
			Tooltip.SetDefault("'Reward for beating the Copper Wraith without angering it first" +
				"\nUnlocks IDG's Starter Bags in Draken's shop");
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
			get { return ("SGAmod/Items/CopperWraithNotch"); }
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
			Tooltip.SetDefault("Allows the Strange Portal to 'move in' into a free housing in your town");
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

	public class Debug3 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Bring up the Custom UI");
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
		public override bool UseItem(Player player)
		{
			SGAmod.TryToggleUI(null);
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.PurpleSolution; }
		}

	}

	public class Debug2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Gain 100 Expertise");
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
		public override bool UseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			sgaplayer.ExpertiseCollected += 100;
			sgaplayer.ExpertiseCollectedTotal += 100;
			Main.NewText("Expertise: "+ sgaplayer.ExpertiseCollected+": Max: "+ sgaplayer.ExpertiseCollectedTotal);
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.DarkBlueSolution; }
		}

	}


	public class Debug1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-reset SGA Player Save Data");
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
		public override bool UseItem(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			sgaplayer.ExpertiseCollected = 0;
			sgaplayer.ExpertiseCollectedTotal = 0;
			sgaplayer.Redmanastar = 0;
			sgaplayer.Electicpermboost = 0;
			sgaplayer.gothellion = false;
			sgaplayer.Drakenshopunlock = false;
			sgaplayer.GenerateNewBossList();
			for (int x = 0; x < SGAWorld.questvars.Length; x++)
			{
				SGAWorld.questvars[x] = 0;
			}
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.RedSolution; }
		}

	}

	public class Debug4 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Draw A boss");
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
		public override bool UseItem(Player player)
		{
			SpriteBatch spriteBatch = new SpriteBatch(Main.instance.GraphicsDevice);

			int width = 800;
			int height = 800;

			RenderTarget2D R = new RenderTarget2D(Main.instance.GraphicsDevice, width, height, false, Main.instance.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

			Main.instance.GraphicsDevice.SetRenderTarget(R);
			Main.instance.GraphicsDevice.Clear(new Color(0, 0, 0, 0));

			Main.spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			/*SpiderQueen Spider = new SpiderQueen();

			Spider.npc.Center = new Vector2(0, 200);

			Spider.npc.velocity = new Vector2(5, 0);

			Spider.PreDraw(spriteBatch,Color.White);*/

			spriteBatch.End();

			Main.spriteBatch.Begin();

			Main.instance.GraphicsDevice.SetRenderTarget(null);

			R.SaveAsPng(File.Create(SGAmod.filePath + "/Spider.png"), width, height);
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.GreenSolution; }
		}

	}


}