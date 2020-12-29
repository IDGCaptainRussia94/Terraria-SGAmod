using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using AAAAUThrowing;

namespace SGAmod.Items.Armors.Vanity
{

	[AutoloadEquip(EquipType.Head)]
	public class MasterfullyCraftedHatOfTheDragonGods : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masterfully Crafted Hat Of The Dragon Gods");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(gold: 10);
			item.rare = 6;
			item.vanity = true;
			item.defense = 0;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Color c = Main.hslToRgb((float)(Main.GlobalTime / 5f) % 1f, 0.45f, 0.65f);
			tooltips.Add(new TooltipLine(mod, "Dedicated", Idglib.ColorText(c, "Dedicated to a stupid Heroforge meme")));
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class AncientSpaceDiverHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Space Diver Helm");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(gold: 1);
			item.rare = 6;
			item.vanity = true;
			item.defense = 0;
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			if (GetType() != typeof(AncientSpaceDiverLeggings))
			{
				SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
				if (!Main.dedServ)
					sgaplayer.armorglowmasks[0] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
			}
		}
        public override bool DrawHead()
        {
            return GetType() != typeof(AncientSpaceDiverHelmet);
        }
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Color c = Main.hslToRgb((float)(Main.GlobalTime / 5f) % 1f, 0.45f, 0.65f);
			tooltips.Add(new TooltipLine(mod, "Dedicated", Idglib.ColorText(c, "Dedicated to PhilBill44, and preserving his work")));
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class AncientSpaceDiverChestplate : AncientSpaceDiverHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Space Diver Chestplate");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(gold: 1);
			item.rare = 6;
			item.vanity = true;
			item.defense = 0;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
				sgaplayer.armorglowmasks[1] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class AncientSpaceDiverLeggings : AncientSpaceDiverHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Space Diver Leggings");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(gold: 1);
			item.rare = 6;
			item.vanity = true;
			item.defense=0;
		}
	}


}