using AAAAUThrowing;
using Microsoft.Xna.Framework;
using SGAmod.Tiles;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armors.Illuminant
{

	[AutoloadEquip(EquipType.Head)]
	public class IlluminantHelmet : ModItem
	{
		public static void IlluminantArmorDrop(int ammount,Vector2 where)
        {
			List<int> armors = new List<int> {ModContent.ItemType<IlluminantHelmet>(), ModContent.ItemType<IlluminantChestplate>(), ModContent.ItemType<IlluminantLeggings>() };
			armors = armors.OrderBy(testby => Main.rand.Next()).ToList();
			for (int i = 0; i < ammount; i += 1)
            {
				Item.NewItem(where, Vector2.Zero, armors[0]);
            }
        }

		public override bool Autoload(ref string name)
        {
			SGAPlayer.PostUpdateEquipsEvent += SetBonus;
			return true;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Helmet");
			Tooltip.SetDefault("Gain an additional free Cooldown Stack");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0,50,0,0);
			item.rare = ItemRarityID.Cyan;
			item.defense = 20;
		}

		public static void SetBonus(SGAPlayer sgaplayer)
        {
			if (sgaplayer.illuminantSet.Item1>0)
            {
				Player player = sgaplayer.player;

				Lighting.AddLight(player.MountedCenter, Color.HotPink.ToVector3() * 0.75f * sgaplayer.illuminantSet.Item2);

				if (sgaplayer.illuminantSet.Item1 > 4)
				{
					player.BoostAllDamage(sgaplayer.activestacks * 0.04f, sgaplayer.activestacks*2);
					sgaplayer.actionCooldownRate *= 0.60f;

					for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
					{
						Item thisitem = player.armor[i];
						if (thisitem.prefix > 0)
						{
							int itemtype = thisitem.type;
							thisitem.type = ItemID.None;
							player.VanillaUpdateEquip(thisitem);
							bool blank1 = false; bool blank2 = false; bool blank3 = false;
							//PlayerHooks.UpdateEquips(player, ref blank1, ref blank2, ref blank3);
							thisitem.type = itemtype;
						}
					}
				}
			}
        }

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.illuminantSet.Item2 += 1;
			sgaplayer.armorglowmasks[0] = "SGAmod/Items/Armors/Illuminant/" + Name+ "_Head";
		}
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod,typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.illuminantSet.Item1 += 1;
			sgaplayer.MaxCooldownStacks += 1;
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class IlluminantChestplate : IlluminantHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Chestplate");
			Tooltip.SetDefault("Gain an additional free Cooldown Stack");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 60, 0, 0);
			item.rare = ItemRarityID.Purple;
			item.defense = 25;
			item.lifeRegen = 3;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.illuminantSet.Item2 += 1;
			sgaplayer.armorglowmasks[1] = "SGAmod/Items/Armors/Illuminant/" + Name + "_Body";
				sgaplayer.armorglowmasks[2] = "SGAmod/Items/Armors/Illuminant/" + Name + "_Arms";
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class IlluminantLeggings : IlluminantHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Leggings");
			Tooltip.SetDefault("Gain an additional free Cooldown Stack");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 50, 0, 0);
			item.rare = ItemRarityID.Purple;
			item.defense = 10;
			item.lifeRegen = 2;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.illuminantSet.Item2 += 1;
			sgaplayer.armorglowmasks[3] = "SGAmod/Items/Armors/Illuminant/" + Name + "_Legs";
		}
	}

}