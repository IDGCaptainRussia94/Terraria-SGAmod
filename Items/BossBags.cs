using Microsoft.Xna.Framework;
using SGAmod.HavocGear.Items.Accessories;
using SGAmod.HavocGear.Items.Weapons;
using SGAmod.Items.Weapons;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items
{
	public class MurkBossBag : ModItem
	{
		public override void SetDefaults()
		{

			item.maxStack = 999;
			item.consumable = true;
			item.width = 24;
			item.height = 24;

			item.rare = 9;
			item.expert = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}

		public override int BossBagNPC
		{
			get
			{
				return mod.NPCType("Murk");
			}
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{

			int random = Main.rand.Next(7);
			if (random == 6)
			{
				player.QuickSpawnItem(ModContent.ItemType<SwarmGun>());
			}
			if (random == 5)
			{
				player.QuickSpawnItem(ModContent.ItemType<GnatStaff>(), 1);
			}
			if (random == 4)
			{
				player.QuickSpawnItem(ModContent.ItemType<SwarmGrenade>(), Main.rand.Next(40, 100));
			}
			if (random == 3)
			{
				player.QuickSpawnItem(ModContent.ItemType<Mudmore>());
			}
			if (random == 2)
			{
				player.QuickSpawnItem(ModContent.ItemType<MurkFlail>());
			}
			if (random == 1)
			{
				player.QuickSpawnItem(ModContent.ItemType<Mossthorn>());
			}
			if (random == 0)
			{
				player.QuickSpawnItem(ModContent.ItemType<Landslide>());
			}
			player.QuickSpawnItem(ModContent.ItemType<MudAbsorber>());
			player.QuickSpawnItem(ModContent.ItemType<MurkyGel>(), Main.rand.Next(50, 70));
		}
	}
	public class SharkvernBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 24;
			item.height = 24;
			item.rare = 11;
			item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override int BossBagNPC
		{
			get
			{
				return mod.NPCType("SharkvernHead");
			}
		}

		public override void OpenBossBag(Player player)
		{

			List<int> types = new List<int>();
			types.Insert(types.Count, ItemID.SharkFin);
			types.Insert(types.Count, ItemID.Seashell);
			types.Insert(types.Count, ItemID.Starfish);
			types.Insert(types.Count, ItemID.SoulofFlight);
			types.Insert(types.Count, ItemID.Coral);

			/*
			for (int f = 0; f < (Main.expertMode ? 150 : 75); f = f + 1)
			{
				player.QuickSpawnItem(types[Main.rand.Next(0, types.Count)]);
			}*/

			DropHelper.DropFixedItemQuanity(types.ToArray(), Main.expertMode ? 150 : 75,Vector2.Zero,player);

			player.TryGettingDevArmor();
			int lLoot = (Main.rand.Next(0, 4));
			player.QuickSpawnItem(mod.ItemType("SerratedTooth"));
			if (lLoot == 0)
			{
				player.QuickSpawnItem(mod.ItemType("SkytoothStorm"));
			}
			if (lLoot == 1)
			{
				player.QuickSpawnItem(mod.ItemType("Jaws"));
			}
			if (lLoot == 2)
			{
				player.QuickSpawnItem(mod.ItemType("SnappyShark"));
				player.QuickSpawnItem(mod.ItemType("SharkTooth"), 150);
			}
			if (lLoot == 3)
			{
				player.QuickSpawnItem(mod.ItemType("SharkBait"), Main.rand.Next(60, 150));
			}
			player.QuickSpawnItem(mod.ItemType("SharkTooth"), Main.rand.Next(100, 200));
		}
	}
}


namespace SGAmod.Items
{
	public class PhaethonBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 32;
			item.height = 32;
			item.expert = true;
			item.rare = -12;
		}

		public override int BossBagNPC
		{
			get
			{
				return ModContent.NPCType<Dimensions.NPCs.SpaceBoss>();
			}
		}

		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(ModContent.ItemType<StarMetalMold>());
			player.QuickSpawnItem(ModContent.ItemType<Accessories.PhaethonEye>(), 1);
		}
	}	
	public class SpiderBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 32;
			item.height = 32;
			item.expert = true;
			item.rare = -12;
		}

		public override int BossBagNPC
		{
			get
			{
				return mod.NPCType("SpiderQueen");
			}
		}

		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("VialofAcid"), Main.rand.Next(35, 60));
			player.QuickSpawnItem(mod.ItemType("AlkalescentHeart"), 1);
			if (Main.rand.Next(0, 3) == 0)
				player.QuickSpawnItem(mod.ItemType("CorrodedShield"), 1);
			if (Main.rand.Next(0, 3) == 0)
			player.QuickSpawnItem(mod.ItemType("AmberGlowSkull"), 1);
		}

	}
	public class CirnoBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 32;
			item.height = 32;
			item.expert = true;
			item.rare = -12;
		}

		public override int BossBagNPC
		{
			get
			{
				return mod.NPCType("Cirno");
			}
		}


		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
		player.TryGettingDevArmor();

			string[] dropitems = { "Starburster", "Snowfall", "IceScepter", "RubiedBlade", "IcicleFall", "RodOfTheMistyLake", "Magishield"};
			player.QuickSpawnItem(mod.ItemType(dropitems[Main.rand.Next(dropitems.Length)]));
			player.QuickSpawnItem(mod.ItemType("CryostalBar"),Main.rand.Next(25, 40));
			player.QuickSpawnItem(mod.ItemType("CirnoWings"), 1);
			if (Main.rand.Next(3) == 0)
				player.QuickSpawnItem(mod.ItemType("GlacialStone"), 1);
		}

}
	public class SPinkyBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 32;
			item.height = 32;
			item.expert = true;
			item.rare = -12;
		}

		public override int BossBagNPC
		{
			get
			{
				return mod.NPCType("SPinky");
			}
		}

		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.TryGettingDevArmor();
				player.QuickSpawnItem(mod.ItemType("LunarRoyalGel"), Main.rand.Next(40, 60));
			Armors.Illuminant.IlluminantHelmet.IlluminantArmorDrop(2, player.Center);
			//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("LunarRoyalGel"));
			player.QuickSpawnItem(mod.ItemType("LunarSlimeHeart"));

		}

	}

}
