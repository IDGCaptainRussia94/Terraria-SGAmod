#define DefineHellionUpdate

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.NPCs.Hellion;
using Terraria.GameContent.Events;

namespace SGAmod.Items.Consumable
{

	public class BaseBossSummon : ModItem
	{
		public override bool Autoload(ref string name)
		{
			return GetType()!=typeof(BaseBossSummon);
		}

		public override bool CanUseItem(Player player)
		{
			if (SGAmod.anysubworld)
			{
				if (player == Main.LocalPlayer)
					Main.NewText("This cannot be used outside the normal folds of reality...", 75, 75, 80);

				return false;
			}
			return base.CanUseItem(player);
		}


	}

		public class WraithCoreFragment3 : WraithCoreFragment
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Wraith Core Fragment");
			Tooltip.SetDefault("Summons forth the third and final of the Wraiths, who has stolen your ability to make Luminite Bars (and also the Ancient Manipulator from the Cultist)");
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Consumable/LunarCore"; }
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 8;
		}

		public override bool CanUseItem(Player player)
		{
			if (SGAWorld.downedWraiths == 3 && !NPC.downedMoonlord)
			{
				item.consumable = false;
			} else {
				item.consumable = true;
			}
			return base.CanUseItem(player);
		}

		public override bool UseItem(Player player)
		{
			if (item.consumable == false) {
				if (player == Main.LocalPlayer)
					Main.NewText("Our time has not yet come", 25, 25, 80);
				return false;
			} else {
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("LuminiteWraith"));
				Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("Fragment", 4);
			recipe.AddIngredient(null, "WraithCoreFragment2", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class WraithCoreFragment2 : WraithCoreFragment
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empowered Wraith Core Fragment");
			Tooltip.SetDefault("Summons forth the second of the Wraiths, who has stolen your ability to make a hardmode anvil and craft anything at one as well.");
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Consumable/EmpoweredCore"; }
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.rare = 5;
		}

		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("CobaltWraith"));
			Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:Tier1HardmodeOre", 10);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 5);
			recipe.AddIngredient(null, "WraithCoreFragment", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
	public class WraithCoreFragment : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wraith Core Fragment");
			Tooltip.SetDefault("Summons forth the first of the Wraiths, who watches you craft bars with envy...");
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Consumable/BasicCore"; }
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 2;
			item.useAnimation = 2;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 0;
			item.rare = 1;
			item.UseSound = SoundID.Item1;
		}

		public override bool CanUseItem(Player player)
		{
			if (Main.netMode==NetmodeID.Server)
			SGAmod.Instance.Logger.Warn("DEBUG SERVER: item canuse");
			if (Main.netMode == NetmodeID.MultiplayerClient)
				SGAmod.Instance.Logger.Warn("DEBUG CLIENT: item canuse");
			if (!NPC.AnyNPCs(mod.NPCType("CopperWraith")) && !NPC.AnyNPCs(mod.NPCType("CobaltWraith")) && !NPC.AnyNPCs(mod.NPCType("LuminiteWraith")))
			{
				return base.CanUseItem(player);
			} else {
				return false;
			}
		}
		public override bool UseItem(Player player)
		{
			if (Main.netMode == NetmodeID.Server)
				SGAmod.Instance.Logger.Warn("DEBUG SERVER: item used");
			if (Main.netMode == NetmodeID.MultiplayerClient)
				SGAmod.Instance.Logger.Warn("DEBUG CLIENT: item used");

			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("CopperWraith"));
			Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:Tier1Ore", 10);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class ConchHorn : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Conch Horn");
			Tooltip.SetDefault("'It's call pierces the depths of the ocean.' \nSummons the Sharkvern");
		}
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 99;
			item.rare = 3;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item44;
			item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ZoneBeach && !NPC.AnyNPCs(mod.NPCType("SharkvernHead"))) {
				return base.CanUseItem(player);
			} else {
				if (player == Main.LocalPlayer)
					Main.NewText("The couch blows but no waves are shaken by its ring...", 100, 100, 250);
				return false;

			}
		}

		public override bool UseItem(Player player)
		{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SharkvernHead"));
				Main.PlaySound(SoundID.Roar, player.position, 0);
				return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Seashell, 1);
			recipe.AddIngredient(ItemID.SharkFin, 1);
			recipe.AddIngredient(ItemID.SoulofLight, 5);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class AcidicEgg : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acidic Egg");
			Tooltip.SetDefault("'No words for this...' \nSummons the Spider Queen\nRotten Eggs drop from spiders");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RottenEgg, 1);
			recipe.AddIngredient(ItemID.Cobweb, 25);
			recipe.AddRecipeGroup("SGAmod:EvilBossMaterials", 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 99;
			item.rare = 2;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item44;
			item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			bool underground = (int)((double)((player.position.Y + (float)player.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;
			;
			if (underground && !NPC.AnyNPCs(mod.NPCType("SpiderQueen")))
			{
				return base.CanUseItem(player);
			}
			else
			{
				if (player == Main.LocalPlayer)
					Main.NewText("There are no spiders here, try using it underground", 30, 200, 30);
				return false;

			}
		}
		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SpiderQueen"));
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}


	}

	public class RoilingSludge : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Roiling Sludge");
			Tooltip.SetDefault("'Ew, Gross!' \nSummons the Murk");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 5);
			recipe.AddIngredient(ItemID.MudBlock, 10);
			recipe.AddIngredient(ItemID.Gel, 30);
			recipe.AddIngredient(ItemID.Bone, 5);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 99;
			item.rare = 2;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item44;
			item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ZoneJungle && !NPC.AnyNPCs(mod.NPCType("Murk")) && !NPC.AnyNPCs(mod.NPCType("BossFlyMiniboss1"))) {
				return base.CanUseItem(player);
			} else {
				if (player == Main.LocalPlayer)
					Main.NewText("There is a lack of mud and sludge for Murk to even exist here...", 40, 180, 60);
				return false;

			}
		}

		public override bool UseItem(Player player)
		{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType(SGAWorld.downedMurk == 0 ? "BossFlyMiniboss1" : "Murk"));
				Main.PlaySound(SoundID.Roar, player.position, 0);
				return true;
		}


	}

	public class Prettygel : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminous Gel");
			Tooltip.SetDefault("Makes pinky very JELLLLYYYYY");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 2;
			item.useAnimation = 2;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 0;
			item.rare = 9;
			item.UseSound = SoundID.Item1;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (SGAmod.Calamity)
				tooltips.Add(new TooltipLine(mod, "NoU", "Summoning this boss with automatically disable Revengence and Death Modes"));
		}

		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(mod.NPCType("SPinky")) && !NPC.AnyNPCs(50) && !Main.dayTime)
			{
				return base.CanUseItem(player);
			} else {
				if (player == Main.LocalPlayer)
					Main.NewText("this gel shimmers only in moonlight...", 100, 40, 100);
				return false;
			}
		}

		public override bool UseItem(Player player)
		{
			if (item.consumable == true) {
				SGAmod.CalamityNoRevengenceNoDeathNoU();
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SPinky"));
				Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
				//player.GetModPlayer<SGAPlayer>().Locked=new Vector2(player.Center.X-2000,4000);
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 3);
			recipe.AddIngredient(3111, 10); //pink gel
			recipe.AddTile(220); //Soldifier
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 5);
			recipe.AddIngredient(mod.ItemType("IlluminantEssence"), 3); //pink gel
			recipe.AddIngredient(mod.ItemType("MurkyGel"), 20); //pink gel
			recipe.AddTile(220); //Soldifier
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class Nineball : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nineball");
			Tooltip.SetDefault("Summons the strongest ice fairy");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 0;
			item.rare = 9;
			item.UseSound = SoundID.Item1;
		}

		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(mod.NPCType("Cirno")))
			{
				if (!Main.dayTime || !player.ZoneSnow)
				{
					item.consumable = false;
				}
				else
				{
					item.consumable = true;
				}
				return base.CanUseItem(player);
			}
			else
			{
				return false;
			}
		}
		public override bool UseItem(Player player)
		{
			if (item.consumable == false)
			{
				if (player == Main.LocalPlayer)
					Main.NewText("It's power lies in the snow biome during the day", 50, 50, 250);
			}
			else
			{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Cirno"));
				Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SoulofNight, 2);
			recipe.AddIngredient(ItemID.SoulofLight, 2);
			recipe.AddIngredient(mod.ItemType("FrigidShard"), 9);
			recipe.AddIngredient(mod.ItemType("IceFairyDust"), 9);
			recipe.AddTile(TileID.IceMachine); //IceMachine
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class CaliburnCompess : BaseBossSummon
	{
		private float effect = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn Compass");
			Tooltip.SetDefault("While held, it points to Caliburn Alters in your world\nCan be used in hardmode to fight a stronger Caliburn spirit\nNon-Consumable");

		}
		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(mod.NPCType("CaliburnGuardianHardmode")) && player.GetModPlayer<SGAPlayer>().DankShrineZone && Main.hardMode)
			{
				return base.CanUseItem(player);
			}
			else
			{
				if (player == Main.LocalPlayer)
					Main.NewText("The compass points the way to a shrine...", 0, 75, 0);
				return false;
			}
		}

		public override bool UseItem(Player player)
		{
			if (Main.hardMode && player.GetModPlayer<SGAPlayer>().DankShrineZone)
			{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("CaliburnGuardianHardmode"));
				Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			}
			return true;
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = 2;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 4;
		}
	}
	public class Mechacluskerf : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mechanical Clusterfuck");
			Tooltip.SetDefault("Summons the Twin-Prime-Destroyers\nIt is highly encourged you do not fight this before late hardmode...");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 2;
			item.useAnimation = 2;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 0;
			item.rare = 9;
			item.UseSound = SoundID.Item1;
		}

		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(mod.NPCType("TPD")) && !NPC.AnyNPCs(50))
			{
				if (Main.dayTime)
				{
					item.consumable = false;
				}
				else
				{
					item.consumable = true;
				}
				return base.CanUseItem(player);
			}
			else
			{
				return false;
			}
		}
		public override bool UseItem(Player player)
		{
			if (item.consumable == false || Main.dayTime)
			{
				if (player == Main.LocalPlayer)
					Main.NewText("Their terror only rings at night", 150, 5, 5);
			}
			else
			{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("TPD"));
				Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(544, 1);
			recipe.AddIngredient(556, 1);
			recipe.AddIngredient(557, 1);
			recipe.AddIngredient(547, 3);
			recipe.AddIngredient(548, 3);
			recipe.AddIngredient(549, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class TruelySusEye : BaseBossSummon
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Truly Suspicious Looking Eye");
			Tooltip.SetDefault("Summons the Servants of the lord of the moon...\nOnly useable after Tier 3 Old One's Army event and Martians are beaten" +
				"\nUse at Night\nDoesn't work online");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (SGAmod.Calamity)
				tooltips.Add(new TooltipLine(mod, "NoU", "Summoning this boss with automatically disable Revengence and Death Modes"));
		}
		public override bool CanUseItem(Player player)
	{
			if ((DD2Event.DownedInvasionT3 && NPC.downedMartians) && !Main.dayTime && Main.netMode<1)
			{
				if (NPC.CountNPCS(mod.NPCType("Harbinger"))<1 && NPC.CountNPCS(NPCID.MoonLordFreeEye) < 1)
				{
					return base.CanUseItem(player);
				}
				else
				{
					return false;
				}
			}
			else
			{
				Main.NewText("No...", 0, 0, 75);
				return false;
			}
	}
	public override bool UseItem(Player player)
	{
			SGAmod.CalamityNoRevengenceNoDeathNoU();
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Harbinger"));
		//Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
		return true;
	}

	public override void AddRecipes()
	{
		ModRecipe recipe = new ModRecipe(mod);
		//recipe.AddIngredient(ItemID.LunarBar, 10);
		recipe.AddIngredient(ItemID.Ectoplasm, 5);
		recipe.AddIngredient(ItemID.SuspiciousLookingEye, 1);
		recipe.AddTile(TileID.CrystalBall);
		recipe.SetResult(this);
		recipe.AddRecipe();
	}

	public override void SetDefaults()
	{
		item.rare = 9;
		item.maxStack = 999;
		item.consumable = true;
		item.width = 40;
		item.height = 40;
		item.useTime = 30;
		item.useAnimation = 30;
		item.useStyle = 4;
		item.noMelee = true; //so the item's animation doesn't do damage
		item.value = 0;
		item.UseSound = SoundID.Item8;
	}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Aqua;

		}

	public override string Texture
	{
		get { return ("Terraria/Item_"+ItemID.SuspiciousLookingEye); }
	}


}

#if DefineHellionUpdate
	public class HellionSummon : BaseBossSummon
	{
		public static ModItem instance;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reality's Sunder");
		}

		public override bool ConsumeItem(Player player)
		{
			return false;
		}

		public override bool Autoload(ref string name)
		{
			instance = this;
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (SGAWorld.downedHellion < 2)
			{
				TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Material" && x.mod == "Terraria");
				if (tt != null)
				{
					int index = tooltips.FindIndex(here => here == tt);
					tooltips.RemoveAt(index);
				}
			}
			else
			{
				tooltips.Add(new TooltipLine(mod, "Nmxx", "Useable in crafting"));
			}

			if (SGAWorld.downedSPinky && SGAWorld.downedCratrosityPML && SGAWorld.downedWraiths>3)
			{
				if (SGAWorld.downedHellion > 0)
					tooltips.Add(new TooltipLine(mod, "Nmxx", "Hold 'left control' while you use the item to skip Hellion Core, this costs 25 Souls of Byte"));
				if (SGAWorld.downedHellion < 2)
				{
					if (SGAWorld.downedHellion == 0)
					{
						tooltips.Add(new TooltipLine(mod, "Nmxx", "'Well done " + SGAmod.userName + ". Yes, I know your real name behind that facade you call " + Main.LocalPlayer.name + ".'"));
						tooltips.Add(new TooltipLine(mod, "Nmxx", "'And thanks to your Dragon's signal, I have found my way to your world, this one tear which will let me invade your puny little " + Main.worldName + "'"));
						tooltips.Add(new TooltipLine(mod, "Nmxx", "'Spend what little time you have left meaningful, if you were expecting to save him, I doubt it'"));
						tooltips.Add(new TooltipLine(mod, "Nmxx", "'But let us not waste anymore time, come, face me'"));
					}
					else
					{
						tooltips.Add(new TooltipLine(mod, "Nmxx", "'Getting closer, I guess now I'll just have to use more power to stop you'"));
						tooltips.Add(new TooltipLine(mod, "Nmxx", "'But enough talk, lets finish this'"));
					}
				}
				else
				{
					tooltips.Add(new TooltipLine(mod, "Nmxx", "'Hmp, very Well done " + SGAmod.userName + ", you've bested me, this time"));
					tooltips.Add(new TooltipLine(mod, "Nmxx", "But next time you won't be so lucky..."));
					tooltips.Add(new TooltipLine(mod, "Nmxx", "My tears have stablized..."));
					tooltips.Add(new TooltipLine(mod, "Nmxx", "Enjoy your fancy reward, you've earned that much..."));
					tooltips[0].text += " (Stablized)";
				}
				tooltips.Add(new TooltipLine(mod, "Nmxx", "Tears a hole in the bastion of reality to bring forth the Paradox General, Helen 'Hellion' Weygold"));
				tooltips.Add(new TooltipLine(mod, "Nmxx", "Non Consumable"));


					foreach (TooltipLine line in tooltips)
				{
					string text = line.text;
					string newline = "";
					for (int i = 0; i < text.Length; i += 1)
					{
						newline += Idglib.ColorText(Color.Lerp(Color.White, Main.hslToRgb((Main.rand.NextFloat(0, 1)) % 1f, 0.75f, Main.rand.NextFloat(0.25f, 0.5f)),MathHelper.Clamp(0.5f+(float)Math.Sin(Main.GlobalTime*2f)/1.5f,0.2f,1f)), text[i].ToString());


					}
					line.text = newline;
				}
			}
			else
			{
				tooltips = new List<TooltipLine>();
			}

		}

		public override bool CanUseItem(Player player)
		{
			if (Hellion.GetHellion()==null && !IdgNPC.bossAlive && SGAWorld.downedSPinky && SGAWorld.downedCratrosityPML && SGAWorld.downedWraiths > 3 && NPC.CountNPCS(mod.NPCType("HellionMonolog"))<1)
			{
				if (!Main.expertMode)
				{
					Hellion hell = new Hellion();
					hell.HellionTaunt("What makes you think I'm going to challenge a NORMAL difficulty player? You shouldn't even have this item, cheater...");
					return false;
				}
				return base.CanUseItem(player);
			}
			else
			{
				return false;
			}
		}
		public override bool UseItem(Player player)
		{
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && player.CountItem(mod.ItemType("ByteSoul")) > 24 && SGAWorld.downedHellion > 0)
			{
				for (int i = 0; i < 25; i += 1)
				{
					player.ConsumeItem(mod.ItemType("ByteSoul"));
				}
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Hellion"));
			}
			else
			{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("HellionCore"));
			}
			//Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(mod.ItemType("WraithCoreFragment3"), 1);
			recipe.AddIngredient(mod.ItemType("RoilingSludge"), 1);
			recipe.AddIngredient(mod.ItemType("Mechacluskerf"), 1);
			recipe.AddIngredient(mod.ItemType("Nineball"), 1);
			recipe.AddIngredient(mod.ItemType("AcidicEgg"), 1);
			recipe.AddIngredient(mod.ItemType("Prettygel"), 1);
			recipe.AddIngredient(mod.ItemType("ConchHorn"), 1);
			recipe.AddIngredient(mod.ItemType("CosmicFragment"), 1);
			recipe.AddIngredient(mod.ItemType("MoneySign"), 10);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 20);


			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void SetDefaults()
		{
			item.rare = 12;
			item.maxStack = 1;
			item.consumable = false;
			item.width = 40;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 0;
			item.rare = -1;
			item.expert = true;
			item.UseSound = SoundID.Item8;
		}

		public override string Texture
		{
			get { return ("Terraria/Extra_19"); }
		}

		private static void drawit(Vector2 where, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI, Matrix zoomitz)
		{

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, zoomitz);

			int width = 32; int height = 32;

			Texture2D beam = new Texture2D(Main.graphics.GraphicsDevice, width, height);
			var dataColors = new Color[width * height];


			///


			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					float dist = (new Vector2(x, y) - new Vector2(width / 2, height / 2)).Length();
					if (Main.rand.NextFloat(dist, 32)<16f)
					{
						float alg = ((-Main.GlobalTime + ((float)(dist) / 4f)) / 2f);
						dataColors[x + y * width] = Main.hslToRgb(alg % 1f, 0.75f, 0.5f);
					}
				}
			}


			///


			beam.SetData(0, null, dataColors, 0, width * height);
			spriteBatch.Draw(beam, where + new Vector2(2, 2), null, Color.White, 0, new Vector2(beam.Width / 2, beam.Height / 2), scale * 2f, SpriteEffects.None, 0f);



		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			float gg = 0f;
			drawit(position + new Vector2(11, 11), spriteBatch, drawColor, drawColor, ref gg, ref scale, 1, Main.UIScaleMatrix);
			return false;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{


			drawit(item.Center - Main.screenPosition, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}

	}
#endif


}
