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
using SGAmod.Dimensions;

namespace SGAmod.Items.Consumable
{

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
			get { return "Terraria/CoolDown"; }
		}

	}

	public class Debug4 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug-Dark Sector Awakener");
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
			new DarkSector((int)player.Center.X/16, (int)player.Center.Y / 16);
			return true;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.Darkness; }
		}

	}


}